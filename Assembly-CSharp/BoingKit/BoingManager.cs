using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE9 RID: 3305
	public static class BoingManager
	{
		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06005386 RID: 21382 RVA: 0x000662C9 File Offset: 0x000644C9
		public static IEnumerable<BoingBehavior> Behaviors
		{
			get
			{
				return BoingManager.s_behaviorMap.Values;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06005387 RID: 21383 RVA: 0x000662D5 File Offset: 0x000644D5
		public static IEnumerable<BoingReactor> Reactors
		{
			get
			{
				return BoingManager.s_reactorMap.Values;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06005388 RID: 21384 RVA: 0x000662E1 File Offset: 0x000644E1
		public static IEnumerable<BoingEffector> Effectors
		{
			get
			{
				return BoingManager.s_effectorMap.Values;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06005389 RID: 21385 RVA: 0x000662ED File Offset: 0x000644ED
		public static IEnumerable<BoingReactorField> ReactorFields
		{
			get
			{
				return BoingManager.s_fieldMap.Values;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x0600538A RID: 21386 RVA: 0x000662F9 File Offset: 0x000644F9
		public static IEnumerable<BoingReactorFieldCPUSampler> ReactorFieldCPUSamlers
		{
			get
			{
				return BoingManager.s_cpuSamplerMap.Values;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x0600538B RID: 21387 RVA: 0x00066305 File Offset: 0x00064505
		public static IEnumerable<BoingReactorFieldGPUSampler> ReactorFieldGPUSampler
		{
			get
			{
				return BoingManager.s_gpuSamplerMap.Values;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x0600538C RID: 21388 RVA: 0x00066311 File Offset: 0x00064511
		public static float DeltaTime
		{
			get
			{
				return BoingManager.s_deltaTime;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x0600538D RID: 21389 RVA: 0x00066318 File Offset: 0x00064518
		public static float FixedDeltaTime
		{
			get
			{
				return Time.fixedDeltaTime;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x0600538E RID: 21390 RVA: 0x0006631F File Offset: 0x0006451F
		internal static int NumBehaviors
		{
			get
			{
				return BoingManager.s_behaviorMap.Count;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x0600538F RID: 21391 RVA: 0x0006632B File Offset: 0x0006452B
		internal static int NumEffectors
		{
			get
			{
				return BoingManager.s_effectorMap.Count;
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06005390 RID: 21392 RVA: 0x00066337 File Offset: 0x00064537
		internal static int NumReactors
		{
			get
			{
				return BoingManager.s_reactorMap.Count;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06005391 RID: 21393 RVA: 0x00066343 File Offset: 0x00064543
		internal static int NumFields
		{
			get
			{
				return BoingManager.s_fieldMap.Count;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06005392 RID: 21394 RVA: 0x0006634F File Offset: 0x0006454F
		internal static int NumCPUFieldSamplers
		{
			get
			{
				return BoingManager.s_cpuSamplerMap.Count;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06005393 RID: 21395 RVA: 0x0006635B File Offset: 0x0006455B
		internal static int NumGPUFieldSamplers
		{
			get
			{
				return BoingManager.s_gpuSamplerMap.Count;
			}
		}

		// Token: 0x06005394 RID: 21396 RVA: 0x001C9AD0 File Offset: 0x001C7CD0
		private static void ValidateManager()
		{
			if (BoingManager.s_managerGo != null)
			{
				return;
			}
			BoingManager.s_managerGo = new GameObject("Boing Kit manager (don't delete)");
			BoingManager.s_managerGo.AddComponent<BoingManagerPreUpdatePump>();
			BoingManager.s_managerGo.AddComponent<BoingManagerPostUpdatePump>();
			UnityEngine.Object.DontDestroyOnLoad(BoingManager.s_managerGo);
			BoingManager.s_managerGo.AddComponent<SphereCollider>().enabled = false;
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06005395 RID: 21397 RVA: 0x00066367 File Offset: 0x00064567
		internal static SphereCollider SharedSphereCollider
		{
			get
			{
				if (BoingManager.s_managerGo == null)
				{
					return null;
				}
				return BoingManager.s_managerGo.GetComponent<SphereCollider>();
			}
		}

		// Token: 0x06005396 RID: 21398 RVA: 0x00066382 File Offset: 0x00064582
		internal static void Register(BoingBehavior behavior)
		{
			BoingManager.PreRegisterBehavior();
			BoingManager.s_behaviorMap.Add(behavior.GetInstanceID(), behavior);
			if (BoingManager.OnBehaviorRegister != null)
			{
				BoingManager.OnBehaviorRegister(behavior);
			}
		}

		// Token: 0x06005397 RID: 21399 RVA: 0x000663AC File Offset: 0x000645AC
		internal static void Unregister(BoingBehavior behavior)
		{
			if (BoingManager.OnBehaviorUnregister != null)
			{
				BoingManager.OnBehaviorUnregister(behavior);
			}
			BoingManager.s_behaviorMap.Remove(behavior.GetInstanceID());
			BoingManager.PostUnregisterBehavior();
		}

		// Token: 0x06005398 RID: 21400 RVA: 0x000663D6 File Offset: 0x000645D6
		internal static void Register(BoingEffector effector)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_effectorMap.Add(effector.GetInstanceID(), effector);
			if (BoingManager.OnEffectorRegister != null)
			{
				BoingManager.OnEffectorRegister(effector);
			}
		}

		// Token: 0x06005399 RID: 21401 RVA: 0x00066400 File Offset: 0x00064600
		internal static void Unregister(BoingEffector effector)
		{
			if (BoingManager.OnEffectorUnregister != null)
			{
				BoingManager.OnEffectorUnregister(effector);
			}
			BoingManager.s_effectorMap.Remove(effector.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x0600539A RID: 21402 RVA: 0x0006642A File Offset: 0x0006462A
		internal static void Register(BoingReactor reactor)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_reactorMap.Add(reactor.GetInstanceID(), reactor);
			if (BoingManager.OnReactorRegister != null)
			{
				BoingManager.OnReactorRegister(reactor);
			}
		}

		// Token: 0x0600539B RID: 21403 RVA: 0x00066454 File Offset: 0x00064654
		internal static void Unregister(BoingReactor reactor)
		{
			if (BoingManager.OnReactorUnregister != null)
			{
				BoingManager.OnReactorUnregister(reactor);
			}
			BoingManager.s_reactorMap.Remove(reactor.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x0600539C RID: 21404 RVA: 0x0006647E File Offset: 0x0006467E
		internal static void Register(BoingReactorField field)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_fieldMap.Add(field.GetInstanceID(), field);
			if (BoingManager.OnReactorFieldRegister != null)
			{
				BoingManager.OnReactorFieldRegister(field);
			}
		}

		// Token: 0x0600539D RID: 21405 RVA: 0x000664A8 File Offset: 0x000646A8
		internal static void Unregister(BoingReactorField field)
		{
			if (BoingManager.OnReactorFieldUnregister != null)
			{
				BoingManager.OnReactorFieldUnregister(field);
			}
			BoingManager.s_fieldMap.Remove(field.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x0600539E RID: 21406 RVA: 0x000664D2 File Offset: 0x000646D2
		internal static void Register(BoingReactorFieldCPUSampler sampler)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_cpuSamplerMap.Add(sampler.GetInstanceID(), sampler);
			if (BoingManager.OnReactorFieldCPUSamplerRegister != null)
			{
				BoingManager.OnReactorFieldCPUSamplerUnregister(sampler);
			}
		}

		// Token: 0x0600539F RID: 21407 RVA: 0x000664FC File Offset: 0x000646FC
		internal static void Unregister(BoingReactorFieldCPUSampler sampler)
		{
			if (BoingManager.OnReactorFieldCPUSamplerUnregister != null)
			{
				BoingManager.OnReactorFieldCPUSamplerUnregister(sampler);
			}
			BoingManager.s_cpuSamplerMap.Remove(sampler.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x060053A0 RID: 21408 RVA: 0x00066526 File Offset: 0x00064726
		internal static void Register(BoingReactorFieldGPUSampler sampler)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_gpuSamplerMap.Add(sampler.GetInstanceID(), sampler);
			if (BoingManager.OnReactorFieldGPUSamplerRegister != null)
			{
				BoingManager.OnReactorFieldGPUSamplerRegister(sampler);
			}
		}

		// Token: 0x060053A1 RID: 21409 RVA: 0x00066550 File Offset: 0x00064750
		internal static void Unregister(BoingReactorFieldGPUSampler sampler)
		{
			if (BoingManager.OnFieldGPUSamplerUnregister != null)
			{
				BoingManager.OnFieldGPUSamplerUnregister(sampler);
			}
			BoingManager.s_gpuSamplerMap.Remove(sampler.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x060053A2 RID: 21410 RVA: 0x0006657A File Offset: 0x0006477A
		internal static void Register(BoingBones bones)
		{
			BoingManager.PreRegisterBones();
			BoingManager.s_bonesMap.Add(bones.GetInstanceID(), bones);
			if (BoingManager.OnBonesRegister != null)
			{
				BoingManager.OnBonesRegister(bones);
			}
		}

		// Token: 0x060053A3 RID: 21411 RVA: 0x000665A4 File Offset: 0x000647A4
		internal static void Unregister(BoingBones bones)
		{
			if (BoingManager.OnBonesUnregister != null)
			{
				BoingManager.OnBonesUnregister(bones);
			}
			BoingManager.s_bonesMap.Remove(bones.GetInstanceID());
			BoingManager.PostUnregisterBones();
		}

		// Token: 0x060053A4 RID: 21412 RVA: 0x000665CE File Offset: 0x000647CE
		private static void PreRegisterBehavior()
		{
			BoingManager.ValidateManager();
		}

		// Token: 0x060053A5 RID: 21413 RVA: 0x000665D5 File Offset: 0x000647D5
		private static void PostUnregisterBehavior()
		{
			if (BoingManager.s_behaviorMap.Count > 0)
			{
				return;
			}
			BoingWorkAsynchronous.PostUnregisterBehaviorCleanUp();
		}

		// Token: 0x060053A6 RID: 21414 RVA: 0x001C9B2C File Offset: 0x001C7D2C
		private static void PreRegisterEffectorReactor()
		{
			BoingManager.ValidateManager();
			if (BoingManager.s_effectorParamsBuffer == null)
			{
				BoingManager.s_effectorParamsList = new List<BoingEffector.Params>(BoingManager.kEffectorParamsIncrement);
				BoingManager.s_effectorParamsBuffer = new ComputeBuffer(BoingManager.s_effectorParamsList.Capacity, BoingEffector.Params.Stride);
			}
			if (BoingManager.s_effectorMap.Count >= BoingManager.s_effectorParamsList.Capacity)
			{
				BoingManager.s_effectorParamsList.Capacity += BoingManager.kEffectorParamsIncrement;
				BoingManager.s_effectorParamsBuffer.Dispose();
				BoingManager.s_effectorParamsBuffer = new ComputeBuffer(BoingManager.s_effectorParamsList.Capacity, BoingEffector.Params.Stride);
			}
		}

		// Token: 0x060053A7 RID: 21415 RVA: 0x001C9BBC File Offset: 0x001C7DBC
		private static void PostUnregisterEffectorReactor()
		{
			if (BoingManager.s_effectorMap.Count > 0 || BoingManager.s_reactorMap.Count > 0 || BoingManager.s_fieldMap.Count > 0 || BoingManager.s_cpuSamplerMap.Count > 0 || BoingManager.s_gpuSamplerMap.Count > 0)
			{
				return;
			}
			BoingManager.s_effectorParamsList = null;
			BoingManager.s_effectorParamsBuffer.Dispose();
			BoingManager.s_effectorParamsBuffer = null;
			BoingWorkAsynchronous.PostUnregisterEffectorReactorCleanUp();
		}

		// Token: 0x060053A8 RID: 21416 RVA: 0x000665CE File Offset: 0x000647CE
		private static void PreRegisterBones()
		{
			BoingManager.ValidateManager();
		}

		// Token: 0x060053A9 RID: 21417 RVA: 0x00030607 File Offset: 0x0002E807
		private static void PostUnregisterBones()
		{
		}

		// Token: 0x060053AA RID: 21418 RVA: 0x000665EA File Offset: 0x000647EA
		internal static void Execute(BoingManager.UpdateMode updateMode)
		{
			if (updateMode == BoingManager.UpdateMode.EarlyUpdate)
			{
				BoingManager.s_deltaTime = Time.deltaTime;
			}
			BoingManager.RefreshEffectorParams();
			BoingManager.ExecuteBones(updateMode);
			BoingManager.ExecuteBehaviors(updateMode);
			BoingManager.ExecuteReactors(updateMode);
		}

		// Token: 0x060053AB RID: 21419 RVA: 0x001C9C28 File Offset: 0x001C7E28
		internal static void ExecuteBehaviors(BoingManager.UpdateMode updateMode)
		{
			if (BoingManager.s_behaviorMap.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<int, BoingBehavior> keyValuePair in BoingManager.s_behaviorMap)
			{
				BoingBehavior value = keyValuePair.Value;
				if (!value.InitRebooted)
				{
					value.Reboot();
					value.InitRebooted = true;
				}
			}
			if (BoingManager.UseAsynchronousJobs)
			{
				BoingWorkAsynchronous.ExecuteBehaviors(BoingManager.s_behaviorMap, updateMode);
				return;
			}
			BoingWorkSynchronous.ExecuteBehaviors(BoingManager.s_behaviorMap, updateMode);
		}

		// Token: 0x060053AC RID: 21420 RVA: 0x001C9CBC File Offset: 0x001C7EBC
		internal static void PullBehaviorResults(BoingManager.UpdateMode updateMode)
		{
			foreach (KeyValuePair<int, BoingBehavior> keyValuePair in BoingManager.s_behaviorMap)
			{
				if (keyValuePair.Value.UpdateMode == updateMode)
				{
					keyValuePair.Value.PullResults();
				}
			}
		}

		// Token: 0x060053AD RID: 21421 RVA: 0x001C9D24 File Offset: 0x001C7F24
		internal static void RestoreBehaviors()
		{
			foreach (KeyValuePair<int, BoingBehavior> keyValuePair in BoingManager.s_behaviorMap)
			{
				keyValuePair.Value.Restore();
			}
		}

		// Token: 0x060053AE RID: 21422 RVA: 0x001C9D7C File Offset: 0x001C7F7C
		internal static void RefreshEffectorParams()
		{
			if (BoingManager.s_effectorParamsList == null)
			{
				return;
			}
			BoingManager.s_effectorParamsIndexMap.Clear();
			BoingManager.s_effectorParamsList.Clear();
			foreach (KeyValuePair<int, BoingEffector> keyValuePair in BoingManager.s_effectorMap)
			{
				BoingEffector value = keyValuePair.Value;
				BoingManager.s_effectorParamsIndexMap.Add(value.GetInstanceID(), BoingManager.s_effectorParamsList.Count);
				BoingManager.s_effectorParamsList.Add(new BoingEffector.Params(value));
			}
			if (BoingManager.s_aEffectorParams == null || BoingManager.s_aEffectorParams.Length != BoingManager.s_effectorParamsList.Count)
			{
				BoingManager.s_aEffectorParams = BoingManager.s_effectorParamsList.ToArray();
				return;
			}
			BoingManager.s_effectorParamsList.CopyTo(BoingManager.s_aEffectorParams);
		}

		// Token: 0x060053AF RID: 21423 RVA: 0x001C9E50 File Offset: 0x001C8050
		internal static void ExecuteReactors(BoingManager.UpdateMode updateMode)
		{
			if (BoingManager.s_effectorMap.Count == 0 && BoingManager.s_reactorMap.Count == 0 && BoingManager.s_fieldMap.Count == 0 && BoingManager.s_cpuSamplerMap.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<int, BoingReactor> keyValuePair in BoingManager.s_reactorMap)
			{
				BoingReactor value = keyValuePair.Value;
				if (!value.InitRebooted)
				{
					value.Reboot();
					value.InitRebooted = true;
				}
			}
			if (BoingManager.UseAsynchronousJobs)
			{
				BoingWorkAsynchronous.ExecuteReactors(BoingManager.s_effectorMap, BoingManager.s_reactorMap, BoingManager.s_fieldMap, BoingManager.s_cpuSamplerMap, updateMode);
				return;
			}
			BoingWorkSynchronous.ExecuteReactors(BoingManager.s_aEffectorParams, BoingManager.s_reactorMap, BoingManager.s_fieldMap, BoingManager.s_cpuSamplerMap, updateMode);
		}

		// Token: 0x060053B0 RID: 21424 RVA: 0x001C9F28 File Offset: 0x001C8128
		internal static void PullReactorResults(BoingManager.UpdateMode updateMode)
		{
			foreach (KeyValuePair<int, BoingReactor> keyValuePair in BoingManager.s_reactorMap)
			{
				if (keyValuePair.Value.UpdateMode == updateMode)
				{
					keyValuePair.Value.PullResults();
				}
			}
			foreach (KeyValuePair<int, BoingReactorFieldCPUSampler> keyValuePair2 in BoingManager.s_cpuSamplerMap)
			{
				if (keyValuePair2.Value.UpdateMode == updateMode)
				{
					keyValuePair2.Value.SampleFromField();
				}
			}
		}

		// Token: 0x060053B1 RID: 21425 RVA: 0x001C9FE4 File Offset: 0x001C81E4
		internal static void RestoreReactors()
		{
			foreach (KeyValuePair<int, BoingReactor> keyValuePair in BoingManager.s_reactorMap)
			{
				keyValuePair.Value.Restore();
			}
			foreach (KeyValuePair<int, BoingReactorFieldCPUSampler> keyValuePair2 in BoingManager.s_cpuSamplerMap)
			{
				keyValuePair2.Value.Restore();
			}
		}

		// Token: 0x060053B2 RID: 21426 RVA: 0x001CA084 File Offset: 0x001C8284
		internal static void DispatchReactorFieldCompute()
		{
			if (BoingManager.s_effectorParamsBuffer == null)
			{
				return;
			}
			BoingManager.s_effectorParamsBuffer.SetData(BoingManager.s_aEffectorParams);
			float deltaTime = Time.deltaTime;
			foreach (KeyValuePair<int, BoingReactorField> keyValuePair in BoingManager.s_fieldMap)
			{
				BoingReactorField value = keyValuePair.Value;
				if (value.HardwareMode == BoingReactorField.HardwareModeEnum.GPU)
				{
					value.ExecuteGpu(deltaTime, BoingManager.s_effectorParamsBuffer, BoingManager.s_effectorParamsIndexMap);
				}
			}
		}

		// Token: 0x060053B3 RID: 21427 RVA: 0x001CA110 File Offset: 0x001C8310
		internal static void ExecuteBones(BoingManager.UpdateMode updateMode)
		{
			if (BoingManager.s_bonesMap.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<int, BoingBones> keyValuePair in BoingManager.s_bonesMap)
			{
				BoingBones value = keyValuePair.Value;
				if (!value.InitRebooted)
				{
					value.Reboot();
					value.InitRebooted = true;
				}
			}
			if (BoingManager.UseAsynchronousJobs)
			{
				BoingWorkAsynchronous.ExecuteBones(BoingManager.s_aEffectorParams, BoingManager.s_bonesMap, updateMode);
				return;
			}
			BoingWorkSynchronous.ExecuteBones(BoingManager.s_aEffectorParams, BoingManager.s_bonesMap, updateMode);
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x00066611 File Offset: 0x00064811
		internal static void PullBonesResults(BoingManager.UpdateMode updateMode)
		{
			if (BoingManager.s_bonesMap.Count == 0)
			{
				return;
			}
			if (BoingManager.UseAsynchronousJobs)
			{
				BoingWorkAsynchronous.PullBonesResults(BoingManager.s_aEffectorParams, BoingManager.s_bonesMap, updateMode);
				return;
			}
			BoingWorkSynchronous.PullBonesResults(BoingManager.s_aEffectorParams, BoingManager.s_bonesMap, updateMode);
		}

		// Token: 0x060053B5 RID: 21429 RVA: 0x001CA1B0 File Offset: 0x001C83B0
		internal static void RestoreBones()
		{
			foreach (KeyValuePair<int, BoingBones> keyValuePair in BoingManager.s_bonesMap)
			{
				keyValuePair.Value.Restore();
			}
		}

		// Token: 0x040055B6 RID: 21942
		public static BoingManager.BehaviorRegisterDelegate OnBehaviorRegister;

		// Token: 0x040055B7 RID: 21943
		public static BoingManager.BehaviorUnregisterDelegate OnBehaviorUnregister;

		// Token: 0x040055B8 RID: 21944
		public static BoingManager.EffectorRegisterDelegate OnEffectorRegister;

		// Token: 0x040055B9 RID: 21945
		public static BoingManager.EffectorUnregisterDelegate OnEffectorUnregister;

		// Token: 0x040055BA RID: 21946
		public static BoingManager.ReactorRegisterDelegate OnReactorRegister;

		// Token: 0x040055BB RID: 21947
		public static BoingManager.ReactorUnregisterDelegate OnReactorUnregister;

		// Token: 0x040055BC RID: 21948
		public static BoingManager.ReactorFieldRegisterDelegate OnReactorFieldRegister;

		// Token: 0x040055BD RID: 21949
		public static BoingManager.ReactorFieldUnregisterDelegate OnReactorFieldUnregister;

		// Token: 0x040055BE RID: 21950
		public static BoingManager.ReactorFieldCPUSamplerRegisterDelegate OnReactorFieldCPUSamplerRegister;

		// Token: 0x040055BF RID: 21951
		public static BoingManager.ReactorFieldCPUSamplerUnregisterDelegate OnReactorFieldCPUSamplerUnregister;

		// Token: 0x040055C0 RID: 21952
		public static BoingManager.ReactorFieldGPUSamplerRegisterDelegate OnReactorFieldGPUSamplerRegister;

		// Token: 0x040055C1 RID: 21953
		public static BoingManager.ReactorFieldGPUSamplerUnregisterDelegate OnFieldGPUSamplerUnregister;

		// Token: 0x040055C2 RID: 21954
		public static BoingManager.BonesRegisterDelegate OnBonesRegister;

		// Token: 0x040055C3 RID: 21955
		public static BoingManager.BonesUnregisterDelegate OnBonesUnregister;

		// Token: 0x040055C4 RID: 21956
		private static float s_deltaTime = 0f;

		// Token: 0x040055C5 RID: 21957
		private static Dictionary<int, BoingBehavior> s_behaviorMap = new Dictionary<int, BoingBehavior>();

		// Token: 0x040055C6 RID: 21958
		private static Dictionary<int, BoingEffector> s_effectorMap = new Dictionary<int, BoingEffector>();

		// Token: 0x040055C7 RID: 21959
		private static Dictionary<int, BoingReactor> s_reactorMap = new Dictionary<int, BoingReactor>();

		// Token: 0x040055C8 RID: 21960
		private static Dictionary<int, BoingReactorField> s_fieldMap = new Dictionary<int, BoingReactorField>();

		// Token: 0x040055C9 RID: 21961
		private static Dictionary<int, BoingReactorFieldCPUSampler> s_cpuSamplerMap = new Dictionary<int, BoingReactorFieldCPUSampler>();

		// Token: 0x040055CA RID: 21962
		private static Dictionary<int, BoingReactorFieldGPUSampler> s_gpuSamplerMap = new Dictionary<int, BoingReactorFieldGPUSampler>();

		// Token: 0x040055CB RID: 21963
		private static Dictionary<int, BoingBones> s_bonesMap = new Dictionary<int, BoingBones>();

		// Token: 0x040055CC RID: 21964
		private static readonly int kEffectorParamsIncrement = 16;

		// Token: 0x040055CD RID: 21965
		private static List<BoingEffector.Params> s_effectorParamsList = new List<BoingEffector.Params>(BoingManager.kEffectorParamsIncrement);

		// Token: 0x040055CE RID: 21966
		private static BoingEffector.Params[] s_aEffectorParams;

		// Token: 0x040055CF RID: 21967
		private static ComputeBuffer s_effectorParamsBuffer;

		// Token: 0x040055D0 RID: 21968
		private static Dictionary<int, int> s_effectorParamsIndexMap = new Dictionary<int, int>();

		// Token: 0x040055D1 RID: 21969
		internal static readonly bool UseAsynchronousJobs = true;

		// Token: 0x040055D2 RID: 21970
		internal static GameObject s_managerGo;

		// Token: 0x02000CEA RID: 3306
		public enum UpdateMode
		{
			// Token: 0x040055D4 RID: 21972
			FixedUpdate,
			// Token: 0x040055D5 RID: 21973
			EarlyUpdate,
			// Token: 0x040055D6 RID: 21974
			LateUpdate
		}

		// Token: 0x02000CEB RID: 3307
		public enum TranslationLockSpace
		{
			// Token: 0x040055D8 RID: 21976
			Global,
			// Token: 0x040055D9 RID: 21977
			Local
		}

		// Token: 0x02000CEC RID: 3308
		// (Invoke) Token: 0x060053B8 RID: 21432
		public delegate void BehaviorRegisterDelegate(BoingBehavior behavior);

		// Token: 0x02000CED RID: 3309
		// (Invoke) Token: 0x060053BC RID: 21436
		public delegate void BehaviorUnregisterDelegate(BoingBehavior behavior);

		// Token: 0x02000CEE RID: 3310
		// (Invoke) Token: 0x060053C0 RID: 21440
		public delegate void EffectorRegisterDelegate(BoingEffector effector);

		// Token: 0x02000CEF RID: 3311
		// (Invoke) Token: 0x060053C4 RID: 21444
		public delegate void EffectorUnregisterDelegate(BoingEffector effector);

		// Token: 0x02000CF0 RID: 3312
		// (Invoke) Token: 0x060053C8 RID: 21448
		public delegate void ReactorRegisterDelegate(BoingReactor reactor);

		// Token: 0x02000CF1 RID: 3313
		// (Invoke) Token: 0x060053CC RID: 21452
		public delegate void ReactorUnregisterDelegate(BoingReactor reactor);

		// Token: 0x02000CF2 RID: 3314
		// (Invoke) Token: 0x060053D0 RID: 21456
		public delegate void ReactorFieldRegisterDelegate(BoingReactorField field);

		// Token: 0x02000CF3 RID: 3315
		// (Invoke) Token: 0x060053D4 RID: 21460
		public delegate void ReactorFieldUnregisterDelegate(BoingReactorField field);

		// Token: 0x02000CF4 RID: 3316
		// (Invoke) Token: 0x060053D8 RID: 21464
		public delegate void ReactorFieldCPUSamplerRegisterDelegate(BoingReactorFieldCPUSampler sampler);

		// Token: 0x02000CF5 RID: 3317
		// (Invoke) Token: 0x060053DC RID: 21468
		public delegate void ReactorFieldCPUSamplerUnregisterDelegate(BoingReactorFieldCPUSampler sampler);

		// Token: 0x02000CF6 RID: 3318
		// (Invoke) Token: 0x060053E0 RID: 21472
		public delegate void ReactorFieldGPUSamplerRegisterDelegate(BoingReactorFieldGPUSampler sampler);

		// Token: 0x02000CF7 RID: 3319
		// (Invoke) Token: 0x060053E4 RID: 21476
		public delegate void ReactorFieldGPUSamplerUnregisterDelegate(BoingReactorFieldGPUSampler sampler);

		// Token: 0x02000CF8 RID: 3320
		// (Invoke) Token: 0x060053E8 RID: 21480
		public delegate void BonesRegisterDelegate(BoingBones bones);

		// Token: 0x02000CF9 RID: 3321
		// (Invoke) Token: 0x060053EC RID: 21484
		public delegate void BonesUnregisterDelegate(BoingBones bones);
	}
}
