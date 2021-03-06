using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Assertions;

namespace Unity.Jobs
{
    // Used by code gen. Do not remove.
    internal interface IJobBase
    {
        // Generated functions from code gen.
        // Called at the Schedule to set up safety handles.
        int PrepareJobAtScheduleTimeFn_Gen();
        // A wrapper around the user's Execute() method.
        void PrepareJobAtExecuteTimeFn_Gen(int jobIndex);
        // Free memory, performs any cleanup.
        unsafe void CleanupJobFn_Gen(void* ptr);
        // Retrieves the ExecuteMethod.
        JobsUtility.ManagedJobForEachDelegate GetExecuteMethod_Gen();
        // Retrieves the UnmanagedJobSize
        int GetUnmanagedJobSize_Gen();
        // Retrieves the job's Marshal method.
        JobsUtility.ManagedJobMarshalDelegate GetMarshalMethod_Gen();
    }

    internal class MonoPInvokeCallbackAttribute : Attribute
    {
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JobHandle
    {
        internal IntPtr JobGroup;
        internal uint Version;

        public static void ScheduleBatchedJobs()
        {
#if !UNITY_SINGLETHREADED_JOBS
            JobsUtility.ScheduleBatchedJobs(JobsUtility.BatchScheduler);
#endif
        }

        public static void CompleteAll(NativeArray<JobHandle> jobs)
        {
            var combinedJobsHandle = CombineDependencies(jobs);
            combinedJobsHandle.Complete();
        }

        public void Complete()
        {
#if !UNITY_SINGLETHREADED_JOBS
            if (JobsUtility.JobQueue == IntPtr.Zero || JobGroup == IntPtr.Zero)
                return;

            JobsUtility.Complete(JobsUtility.BatchScheduler, ref this);
#endif
        }

        public bool IsCompleted
        {
#if UNITY_SINGLETHREADED_JOBS
            get => true;
#else
            get => JobsUtility.IsCompleted(JobsUtility.BatchScheduler, ref this);
#endif
        }

        public static bool CheckFenceIsDependencyOrDidSyncFence(JobHandle dependency, JobHandle writer) => true;

        public static unsafe JobHandle CombineDependencies(NativeArray<JobHandle> jobHandles)
        {
#if UNITY_SINGLETHREADED_JOBS
            return default(JobHandle);
#else
            var fence = new JobHandle();
            JobsUtility.ScheduleMultiDependencyJob(ref fence, JobsUtility.BatchScheduler, new IntPtr(jobHandles.GetUnsafeReadOnlyPtr()), jobHandles.Length);
            return fence;
#endif
        }

        public static unsafe JobHandle CombineDependencies(JobHandle one, JobHandle two)
        {
#if UNITY_SINGLETHREADED_JOBS
            return default(JobHandle);
#else
            var fence = new JobHandle();
            var dependencies = stackalloc JobHandle[] { one, two };
            JobsUtility.ScheduleMultiDependencyJob(ref fence, JobsUtility.BatchScheduler, new IntPtr(UnsafeUtility.AddressOf(ref dependencies[0])), 2);
            return fence;
#endif
        }

        public static unsafe JobHandle CombineDependencies(JobHandle one, JobHandle two, JobHandle three)
        {
#if UNITY_SINGLETHREADED_JOBS
            return default(JobHandle);
#else
             var fence = new JobHandle();
             var dependencies = stackalloc JobHandle[] { one, two, three };
             JobsUtility.ScheduleMultiDependencyJob(ref fence, JobsUtility.BatchScheduler, new IntPtr(UnsafeUtility.AddressOf(ref dependencies[0])), 3);
             return fence;
#endif
        }
    }

    [JobProducerType(typeof(IJobExtensions.JobStruct<>))]
    public interface IJob
    {
        void Execute();
    }

    public static class IJobExtensions
    {
        internal struct JobStruct<T> where T : struct, IJob
        {
            static IntPtr JobReflectionData;
            internal T JobData;

            public static IntPtr Initialize()
            {
                if (JobReflectionData == IntPtr.Zero)
                {
                    JobReflectionData = JobsUtility.CreateJobReflectionData(typeof(JobStruct<T>), typeof(T),
                        JobType.Single,
                        (ExecuteJobFunction)Execute);
                }
                return JobReflectionData;
            }

            public delegate void ExecuteJobFunction(ref JobStruct<T> jobStruct, IntPtr additionalData,
                IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

            public static void Execute(ref JobStruct<T> jobStruct, IntPtr additionalData,
                IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
            {
                jobStruct.JobData.Execute();
            }
        }

        public static unsafe JobHandle Schedule<T>(this T jobData, JobHandle dependsOn = default(JobHandle))
            where T : struct, IJob
        {
            var jobStruct = new JobStruct<T>()
            {
                JobData = jobData
            };

            var scheduleParams = new JobsUtility.JobScheduleParameters(
                UnsafeUtility.AddressOf(ref jobStruct),
                JobStruct<T>.Initialize(),
                dependsOn,
                ScheduleMode.Batched);
            return JobsUtility.Schedule(ref scheduleParams);
        }

        public static void Run<T>(this T jobData) where T : struct, IJob
        {
            // can't just call: 'jobData.Execute();
            // because we need the setup/teardown.
            jobData.Schedule().Complete();
        }
    }

    [JobProducerType(typeof(IJobParallelForExtensions.ParallelForJobStruct<>))]
    public interface IJobParallelFor
    {
        void Execute(int index);
    }

    public static class IJobParallelForExtensions
    {
        internal struct ParallelForJobStruct<T> where T : struct, IJobParallelFor
        {
            static IntPtr JobReflectionData;
            public T JobData;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            public int Sentinel;
#endif

            public static unsafe IntPtr Initialize()
            {
                if (JobReflectionData == IntPtr.Zero)
                {
                    JobReflectionData = JobsUtility.CreateJobReflectionData(typeof(ParallelForJobStruct<T>), typeof(T),
                        JobType.ParallelFor,
                        (ExecuteJobFunction) Execute);
                }
                return JobReflectionData;
            }

            public delegate void ExecuteJobFunction(ref ParallelForJobStruct<T> jobData, IntPtr additionalData,
                IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

            public static void Execute(ref ParallelForJobStruct<T> jobStruct, IntPtr additionalData,
                IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                Assert.AreEqual(jobStruct.Sentinel - ranges.ArrayLength, 37);
#endif
                // TODO Tiny doesn't currently support work stealing. https://unity3d.atlassian.net/browse/DOTSR-286

                while (true)
                {
                    if (!JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out int begin, out int end))
                        break;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    // TODO https://unity3d.atlassian.net/browse/DOTSR-282
                    //JobsUtility.PatchBufferMinMaxRanges(IntPtr.Zero, UnsafeUtility.AddressOf(ref jobData), begin, end - begin);
#endif
                    for (var i = begin; i < end; ++i)
                    {
                        jobStruct.JobData.Execute(i);
                    }
                }
            }
        }

        public static unsafe JobHandle Schedule<T>(this T jobData, int arrayLength, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle))
            where T : struct, IJobParallelFor
        {
            var jobStruct = new ParallelForJobStruct<T>()
            {
                JobData = jobData,
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                Sentinel = 37 + arrayLength    // check that code is patched as expected
#endif
            };

            var scheduleParams = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref jobStruct),
                ParallelForJobStruct<T>.Initialize(),
                dependsOn,
                ScheduleMode.Batched);
            return JobsUtility.ScheduleParallelFor(ref scheduleParams, arrayLength, innerloopBatchCount);
        }
    }
}
