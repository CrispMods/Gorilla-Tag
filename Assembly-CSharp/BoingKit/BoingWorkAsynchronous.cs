using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000D0D RID: 3341
	public static class BoingWorkAsynchronous
	{
		// Token: 0x0600544D RID: 21581 RVA: 0x00066A61 File Offset: 0x00064C61
		internal static void PostUnregisterBehaviorCleanUp()
		{
			if (BoingWorkAsynchronous.s_behaviorJobNeedsGather)
			{
				BoingWorkAsynchronous.s_hBehaviorJob.Complete();
				BoingWorkAsynchronous.s_aBehaviorParams.Dispose();
				BoingWorkAsynchronous.s_aBehaviorOutput.Dispose();
				BoingWorkAsynchronous.s_behaviorJobNeedsGather = false;
			}
		}

		// Token: 0x0600544E RID: 21582 RVA: 0x00066A8E File Offset: 0x00064C8E
		internal static void PostUnregisterEffectorReactorCleanUp()
		{
			if (BoingWorkAsynchronous.s_reactorJobNeedsGather)
			{
				BoingWorkAsynchronous.s_hReactorJob.Complete();
				BoingWorkAsynchronous.s_aEffectors.Dispose();
				BoingWorkAsynchronous.s_aReactorExecParams.Dispose();
				BoingWorkAsynchronous.s_aReactorExecOutput.Dispose();
				BoingWorkAsynchronous.s_reactorJobNeedsGather = false;
			}
		}

		// Token: 0x0600544F RID: 21583 RVA: 0x001CE900 File Offset: 0x001CCB00
		internal static void ExecuteBehaviors(Dictionary<int, BoingBehavior> behaviorMap, BoingManager.UpdateMode updateMode)
		{
			int num = 0;
			BoingWorkAsynchronous.s_aBehaviorParams = new NativeArray<BoingWork.Params>(behaviorMap.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
			BoingWorkAsynchronous.s_aBehaviorOutput = new NativeArray<BoingWork.Output>(behaviorMap.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
			foreach (KeyValuePair<int, BoingBehavior> keyValuePair in behaviorMap)
			{
				BoingBehavior value = keyValuePair.Value;
				if (value.UpdateMode == updateMode)
				{
					value.PrepareExecute();
					BoingWorkAsynchronous.s_aBehaviorParams[num++] = value.Params;
				}
			}
			if (num > 0)
			{
				BoingWorkAsynchronous.BehaviorJob jobData = new BoingWorkAsynchronous.BehaviorJob
				{
					Params = BoingWorkAsynchronous.s_aBehaviorParams,
					Output = BoingWorkAsynchronous.s_aBehaviorOutput,
					DeltaTime = BoingManager.DeltaTime,
					FixedDeltaTime = BoingManager.FixedDeltaTime
				};
				int innerloopBatchCount = (int)Mathf.Ceil((float)num / (float)Environment.ProcessorCount);
				BoingWorkAsynchronous.s_hBehaviorJob = jobData.Schedule(num, innerloopBatchCount, default(JobHandle));
				JobHandle.ScheduleBatchedJobs();
			}
			BoingWorkAsynchronous.s_behaviorJobNeedsGather = true;
			if (BoingWorkAsynchronous.s_behaviorJobNeedsGather)
			{
				if (num > 0)
				{
					BoingWorkAsynchronous.s_hBehaviorJob.Complete();
					for (int i = 0; i < num; i++)
					{
						BoingWorkAsynchronous.s_aBehaviorOutput[i].GatherOutput(behaviorMap, updateMode);
					}
				}
				BoingWorkAsynchronous.s_aBehaviorParams.Dispose();
				BoingWorkAsynchronous.s_aBehaviorOutput.Dispose();
				BoingWorkAsynchronous.s_behaviorJobNeedsGather = false;
			}
		}

		// Token: 0x06005450 RID: 21584 RVA: 0x001CEA60 File Offset: 0x001CCC60
		internal static void ExecuteReactors(Dictionary<int, BoingEffector> effectorMap, Dictionary<int, BoingReactor> reactorMap, Dictionary<int, BoingReactorField> fieldMap, Dictionary<int, BoingReactorFieldCPUSampler> cpuSamplerMap, BoingManager.UpdateMode updateMode)
		{
			int num = 0;
			BoingWorkAsynchronous.s_aEffectors = new NativeArray<BoingEffector.Params>(effectorMap.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
			BoingWorkAsynchronous.s_aReactorExecParams = new NativeArray<BoingWork.Params>(reactorMap.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
			BoingWorkAsynchronous.s_aReactorExecOutput = new NativeArray<BoingWork.Output>(reactorMap.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
			foreach (KeyValuePair<int, BoingReactor> keyValuePair in reactorMap)
			{
				BoingReactor value = keyValuePair.Value;
				if (value.UpdateMode == updateMode)
				{
					value.PrepareExecute();
					BoingWorkAsynchronous.s_aReactorExecParams[num++] = value.Params;
				}
			}
			if (num > 0)
			{
				int num2 = 0;
				BoingEffector.Params value2 = default(BoingEffector.Params);
				foreach (KeyValuePair<int, BoingEffector> keyValuePair2 in effectorMap)
				{
					BoingEffector value3 = keyValuePair2.Value;
					value2.Fill(keyValuePair2.Value);
					BoingWorkAsynchronous.s_aEffectors[num2++] = value2;
				}
			}
			if (num > 0)
			{
				BoingWorkAsynchronous.s_hReactorJob = new BoingWorkAsynchronous.ReactorJob
				{
					Effectors = BoingWorkAsynchronous.s_aEffectors,
					Params = BoingWorkAsynchronous.s_aReactorExecParams,
					Output = BoingWorkAsynchronous.s_aReactorExecOutput,
					DeltaTime = BoingManager.DeltaTime,
					FixedDeltaTime = BoingManager.FixedDeltaTime
				}.Schedule(num, 32, default(JobHandle));
				JobHandle.ScheduleBatchedJobs();
			}
			foreach (KeyValuePair<int, BoingReactorField> keyValuePair3 in fieldMap)
			{
				BoingReactorField value4 = keyValuePair3.Value;
				if (value4.HardwareMode == BoingReactorField.HardwareModeEnum.CPU)
				{
					value4.ExecuteCpu(BoingManager.DeltaTime);
				}
			}
			foreach (KeyValuePair<int, BoingReactorFieldCPUSampler> keyValuePair4 in cpuSamplerMap)
			{
				BoingReactorFieldCPUSampler value5 = keyValuePair4.Value;
			}
			BoingWorkAsynchronous.s_reactorJobNeedsGather = true;
			if (BoingWorkAsynchronous.s_reactorJobNeedsGather)
			{
				if (num > 0)
				{
					BoingWorkAsynchronous.s_hReactorJob.Complete();
					for (int i = 0; i < num; i++)
					{
						BoingWorkAsynchronous.s_aReactorExecOutput[i].GatherOutput(reactorMap, updateMode);
					}
				}
				BoingWorkAsynchronous.s_aEffectors.Dispose();
				BoingWorkAsynchronous.s_aReactorExecParams.Dispose();
				BoingWorkAsynchronous.s_aReactorExecOutput.Dispose();
				BoingWorkAsynchronous.s_reactorJobNeedsGather = false;
			}
		}

		// Token: 0x06005451 RID: 21585 RVA: 0x001CECE0 File Offset: 0x001CCEE0
		internal static void ExecuteBones(BoingEffector.Params[] aEffectorParams, Dictionary<int, BoingBones> bonesMap, BoingManager.UpdateMode updateMode)
		{
			float deltaTime = BoingManager.DeltaTime;
			foreach (KeyValuePair<int, BoingBones> keyValuePair in bonesMap)
			{
				BoingBones value = keyValuePair.Value;
				if (value.UpdateMode == updateMode)
				{
					value.PrepareExecute();
					if (aEffectorParams != null)
					{
						for (int i = 0; i < aEffectorParams.Length; i++)
						{
							value.AccumulateTarget(ref aEffectorParams[i], deltaTime);
						}
					}
					value.EndAccumulateTargets();
					BoingManager.UpdateMode updateMode2 = value.UpdateMode;
					if (updateMode2 != BoingManager.UpdateMode.FixedUpdate)
					{
						if (updateMode2 - BoingManager.UpdateMode.EarlyUpdate <= 1)
						{
							value.Params.Execute(value, BoingManager.DeltaTime);
						}
					}
					else
					{
						value.Params.Execute(value, BoingManager.FixedDeltaTime);
					}
				}
			}
		}

		// Token: 0x06005452 RID: 21586 RVA: 0x001CEDAC File Offset: 0x001CCFAC
		internal static void PullBonesResults(BoingEffector.Params[] aEffectorParams, Dictionary<int, BoingBones> bonesMap, BoingManager.UpdateMode updateMode)
		{
			foreach (KeyValuePair<int, BoingBones> keyValuePair in bonesMap)
			{
				BoingBones value = keyValuePair.Value;
				if (value.UpdateMode == updateMode)
				{
					value.Params.PullResults(value);
				}
			}
		}

		// Token: 0x04005699 RID: 22169
		private static bool s_behaviorJobNeedsGather;

		// Token: 0x0400569A RID: 22170
		private static JobHandle s_hBehaviorJob;

		// Token: 0x0400569B RID: 22171
		private static NativeArray<BoingWork.Params> s_aBehaviorParams;

		// Token: 0x0400569C RID: 22172
		private static NativeArray<BoingWork.Output> s_aBehaviorOutput;

		// Token: 0x0400569D RID: 22173
		private static bool s_reactorJobNeedsGather;

		// Token: 0x0400569E RID: 22174
		private static JobHandle s_hReactorJob;

		// Token: 0x0400569F RID: 22175
		private static NativeArray<BoingEffector.Params> s_aEffectors;

		// Token: 0x040056A0 RID: 22176
		private static NativeArray<BoingWork.Params> s_aReactorExecParams;

		// Token: 0x040056A1 RID: 22177
		private static NativeArray<BoingWork.Output> s_aReactorExecOutput;

		// Token: 0x02000D0E RID: 3342
		private struct BehaviorJob : IJobParallelFor
		{
			// Token: 0x06005453 RID: 21587 RVA: 0x001CEE10 File Offset: 0x001CD010
			public void Execute(int index)
			{
				BoingWork.Params @params = this.Params[index];
				if (@params.Bits.IsBitSet(9))
				{
					@params.Execute(this.FixedDeltaTime);
				}
				else
				{
					@params.Execute(this.DeltaTime);
				}
				this.Output[index] = new BoingWork.Output(@params.InstanceID, ref @params.Instance.PositionSpring, ref @params.Instance.RotationSpring, ref @params.Instance.ScaleSpring);
			}

			// Token: 0x040056A2 RID: 22178
			public NativeArray<BoingWork.Params> Params;

			// Token: 0x040056A3 RID: 22179
			public NativeArray<BoingWork.Output> Output;

			// Token: 0x040056A4 RID: 22180
			public float DeltaTime;

			// Token: 0x040056A5 RID: 22181
			public float FixedDeltaTime;
		}

		// Token: 0x02000D0F RID: 3343
		private struct ReactorJob : IJobParallelFor
		{
			// Token: 0x06005454 RID: 21588 RVA: 0x001CEE94 File Offset: 0x001CD094
			public void Execute(int index)
			{
				BoingWork.Params @params = this.Params[index];
				int i = 0;
				int length = this.Effectors.Length;
				while (i < length)
				{
					BoingEffector.Params params2 = this.Effectors[i];
					@params.AccumulateTarget(ref params2, this.DeltaTime);
					i++;
				}
				@params.EndAccumulateTargets();
				if (@params.Bits.IsBitSet(9))
				{
					@params.Execute(this.FixedDeltaTime);
				}
				else
				{
					@params.Execute(BoingManager.DeltaTime);
				}
				this.Output[index] = new BoingWork.Output(@params.InstanceID, ref @params.Instance.PositionSpring, ref @params.Instance.RotationSpring, ref @params.Instance.ScaleSpring);
			}

			// Token: 0x040056A6 RID: 22182
			[ReadOnly]
			public NativeArray<BoingEffector.Params> Effectors;

			// Token: 0x040056A7 RID: 22183
			public NativeArray<BoingWork.Params> Params;

			// Token: 0x040056A8 RID: 22184
			public NativeArray<BoingWork.Output> Output;

			// Token: 0x040056A9 RID: 22185
			public float DeltaTime;

			// Token: 0x040056AA RID: 22186
			public float FixedDeltaTime;
		}
	}
}
