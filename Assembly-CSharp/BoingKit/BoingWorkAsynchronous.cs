using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CDC RID: 3292
	public static class BoingWorkAsynchronous
	{
		// Token: 0x060052EB RID: 21227 RVA: 0x00198E18 File Offset: 0x00197018
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

		// Token: 0x060052EC RID: 21228 RVA: 0x00198E45 File Offset: 0x00197045
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

		// Token: 0x060052ED RID: 21229 RVA: 0x00198E7C File Offset: 0x0019707C
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

		// Token: 0x060052EE RID: 21230 RVA: 0x00198FDC File Offset: 0x001971DC
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

		// Token: 0x060052EF RID: 21231 RVA: 0x0019925C File Offset: 0x0019745C
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

		// Token: 0x060052F0 RID: 21232 RVA: 0x00199328 File Offset: 0x00197528
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

		// Token: 0x0400558D RID: 21901
		private static bool s_behaviorJobNeedsGather;

		// Token: 0x0400558E RID: 21902
		private static JobHandle s_hBehaviorJob;

		// Token: 0x0400558F RID: 21903
		private static NativeArray<BoingWork.Params> s_aBehaviorParams;

		// Token: 0x04005590 RID: 21904
		private static NativeArray<BoingWork.Output> s_aBehaviorOutput;

		// Token: 0x04005591 RID: 21905
		private static bool s_reactorJobNeedsGather;

		// Token: 0x04005592 RID: 21906
		private static JobHandle s_hReactorJob;

		// Token: 0x04005593 RID: 21907
		private static NativeArray<BoingEffector.Params> s_aEffectors;

		// Token: 0x04005594 RID: 21908
		private static NativeArray<BoingWork.Params> s_aReactorExecParams;

		// Token: 0x04005595 RID: 21909
		private static NativeArray<BoingWork.Output> s_aReactorExecOutput;

		// Token: 0x02000CDD RID: 3293
		private struct BehaviorJob : IJobParallelFor
		{
			// Token: 0x060052F1 RID: 21233 RVA: 0x0019938C File Offset: 0x0019758C
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

			// Token: 0x04005596 RID: 21910
			public NativeArray<BoingWork.Params> Params;

			// Token: 0x04005597 RID: 21911
			public NativeArray<BoingWork.Output> Output;

			// Token: 0x04005598 RID: 21912
			public float DeltaTime;

			// Token: 0x04005599 RID: 21913
			public float FixedDeltaTime;
		}

		// Token: 0x02000CDE RID: 3294
		private struct ReactorJob : IJobParallelFor
		{
			// Token: 0x060052F2 RID: 21234 RVA: 0x00199410 File Offset: 0x00197610
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

			// Token: 0x0400559A RID: 21914
			[ReadOnly]
			public NativeArray<BoingEffector.Params> Effectors;

			// Token: 0x0400559B RID: 21915
			public NativeArray<BoingWork.Params> Params;

			// Token: 0x0400559C RID: 21916
			public NativeArray<BoingWork.Output> Output;

			// Token: 0x0400559D RID: 21917
			public float DeltaTime;

			// Token: 0x0400559E RID: 21918
			public float FixedDeltaTime;
		}
	}
}
