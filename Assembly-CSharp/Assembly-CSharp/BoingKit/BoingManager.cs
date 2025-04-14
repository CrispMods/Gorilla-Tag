using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CBB RID: 3259
	public static class BoingManager
	{
		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06005230 RID: 21040 RVA: 0x00193E18 File Offset: 0x00192018
		public static IEnumerable<BoingBehavior> Behaviors
		{
			get
			{
				return BoingManager.s_behaviorMap.Values;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06005231 RID: 21041 RVA: 0x00193E24 File Offset: 0x00192024
		public static IEnumerable<BoingReactor> Reactors
		{
			get
			{
				return BoingManager.s_reactorMap.Values;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06005232 RID: 21042 RVA: 0x00193E30 File Offset: 0x00192030
		public static IEnumerable<BoingEffector> Effectors
		{
			get
			{
				return BoingManager.s_effectorMap.Values;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06005233 RID: 21043 RVA: 0x00193E3C File Offset: 0x0019203C
		public static IEnumerable<BoingReactorField> ReactorFields
		{
			get
			{
				return BoingManager.s_fieldMap.Values;
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06005234 RID: 21044 RVA: 0x00193E48 File Offset: 0x00192048
		public static IEnumerable<BoingReactorFieldCPUSampler> ReactorFieldCPUSamlers
		{
			get
			{
				return BoingManager.s_cpuSamplerMap.Values;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06005235 RID: 21045 RVA: 0x00193E54 File Offset: 0x00192054
		public static IEnumerable<BoingReactorFieldGPUSampler> ReactorFieldGPUSampler
		{
			get
			{
				return BoingManager.s_gpuSamplerMap.Values;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06005236 RID: 21046 RVA: 0x00193E60 File Offset: 0x00192060
		public static float DeltaTime
		{
			get
			{
				return BoingManager.s_deltaTime;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06005237 RID: 21047 RVA: 0x00193E67 File Offset: 0x00192067
		public static float FixedDeltaTime
		{
			get
			{
				return Time.fixedDeltaTime;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06005238 RID: 21048 RVA: 0x00193E6E File Offset: 0x0019206E
		internal static int NumBehaviors
		{
			get
			{
				return BoingManager.s_behaviorMap.Count;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06005239 RID: 21049 RVA: 0x00193E7A File Offset: 0x0019207A
		internal static int NumEffectors
		{
			get
			{
				return BoingManager.s_effectorMap.Count;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x0600523A RID: 21050 RVA: 0x00193E86 File Offset: 0x00192086
		internal static int NumReactors
		{
			get
			{
				return BoingManager.s_reactorMap.Count;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x0600523B RID: 21051 RVA: 0x00193E92 File Offset: 0x00192092
		internal static int NumFields
		{
			get
			{
				return BoingManager.s_fieldMap.Count;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x0600523C RID: 21052 RVA: 0x00193E9E File Offset: 0x0019209E
		internal static int NumCPUFieldSamplers
		{
			get
			{
				return BoingManager.s_cpuSamplerMap.Count;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x0600523D RID: 21053 RVA: 0x00193EAA File Offset: 0x001920AA
		internal static int NumGPUFieldSamplers
		{
			get
			{
				return BoingManager.s_gpuSamplerMap.Count;
			}
		}

		// Token: 0x0600523E RID: 21054 RVA: 0x00193EB8 File Offset: 0x001920B8
		private static void ValidateManager()
		{
			if (BoingManager.s_managerGo != null)
			{
				return;
			}
			BoingManager.s_managerGo = new GameObject("Boing Kit manager (don't delete)");
			BoingManager.s_managerGo.AddComponent<BoingManagerPreUpdatePump>();
			BoingManager.s_managerGo.AddComponent<BoingManagerPostUpdatePump>();
			Object.DontDestroyOnLoad(BoingManager.s_managerGo);
			BoingManager.s_managerGo.AddComponent<SphereCollider>().enabled = false;
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x0600523F RID: 21055 RVA: 0x00193F12 File Offset: 0x00192112
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

		// Token: 0x06005240 RID: 21056 RVA: 0x00193F2D File Offset: 0x0019212D
		internal static void Register(BoingBehavior behavior)
		{
			BoingManager.PreRegisterBehavior();
			BoingManager.s_behaviorMap.Add(behavior.GetInstanceID(), behavior);
			if (BoingManager.OnBehaviorRegister != null)
			{
				BoingManager.OnBehaviorRegister(behavior);
			}
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x00193F57 File Offset: 0x00192157
		internal static void Unregister(BoingBehavior behavior)
		{
			if (BoingManager.OnBehaviorUnregister != null)
			{
				BoingManager.OnBehaviorUnregister(behavior);
			}
			BoingManager.s_behaviorMap.Remove(behavior.GetInstanceID());
			BoingManager.PostUnregisterBehavior();
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x00193F81 File Offset: 0x00192181
		internal static void Register(BoingEffector effector)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_effectorMap.Add(effector.GetInstanceID(), effector);
			if (BoingManager.OnEffectorRegister != null)
			{
				BoingManager.OnEffectorRegister(effector);
			}
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x00193FAB File Offset: 0x001921AB
		internal static void Unregister(BoingEffector effector)
		{
			if (BoingManager.OnEffectorUnregister != null)
			{
				BoingManager.OnEffectorUnregister(effector);
			}
			BoingManager.s_effectorMap.Remove(effector.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x06005244 RID: 21060 RVA: 0x00193FD5 File Offset: 0x001921D5
		internal static void Register(BoingReactor reactor)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_reactorMap.Add(reactor.GetInstanceID(), reactor);
			if (BoingManager.OnReactorRegister != null)
			{
				BoingManager.OnReactorRegister(reactor);
			}
		}

		// Token: 0x06005245 RID: 21061 RVA: 0x00193FFF File Offset: 0x001921FF
		internal static void Unregister(BoingReactor reactor)
		{
			if (BoingManager.OnReactorUnregister != null)
			{
				BoingManager.OnReactorUnregister(reactor);
			}
			BoingManager.s_reactorMap.Remove(reactor.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x06005246 RID: 21062 RVA: 0x00194029 File Offset: 0x00192229
		internal static void Register(BoingReactorField field)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_fieldMap.Add(field.GetInstanceID(), field);
			if (BoingManager.OnReactorFieldRegister != null)
			{
				BoingManager.OnReactorFieldRegister(field);
			}
		}

		// Token: 0x06005247 RID: 21063 RVA: 0x00194053 File Offset: 0x00192253
		internal static void Unregister(BoingReactorField field)
		{
			if (BoingManager.OnReactorFieldUnregister != null)
			{
				BoingManager.OnReactorFieldUnregister(field);
			}
			BoingManager.s_fieldMap.Remove(field.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x06005248 RID: 21064 RVA: 0x0019407D File Offset: 0x0019227D
		internal static void Register(BoingReactorFieldCPUSampler sampler)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_cpuSamplerMap.Add(sampler.GetInstanceID(), sampler);
			if (BoingManager.OnReactorFieldCPUSamplerRegister != null)
			{
				BoingManager.OnReactorFieldCPUSamplerUnregister(sampler);
			}
		}

		// Token: 0x06005249 RID: 21065 RVA: 0x001940A7 File Offset: 0x001922A7
		internal static void Unregister(BoingReactorFieldCPUSampler sampler)
		{
			if (BoingManager.OnReactorFieldCPUSamplerUnregister != null)
			{
				BoingManager.OnReactorFieldCPUSamplerUnregister(sampler);
			}
			BoingManager.s_cpuSamplerMap.Remove(sampler.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x0600524A RID: 21066 RVA: 0x001940D1 File Offset: 0x001922D1
		internal static void Register(BoingReactorFieldGPUSampler sampler)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_gpuSamplerMap.Add(sampler.GetInstanceID(), sampler);
			if (BoingManager.OnReactorFieldGPUSamplerRegister != null)
			{
				BoingManager.OnReactorFieldGPUSamplerRegister(sampler);
			}
		}

		// Token: 0x0600524B RID: 21067 RVA: 0x001940FB File Offset: 0x001922FB
		internal static void Unregister(BoingReactorFieldGPUSampler sampler)
		{
			if (BoingManager.OnFieldGPUSamplerUnregister != null)
			{
				BoingManager.OnFieldGPUSamplerUnregister(sampler);
			}
			BoingManager.s_gpuSamplerMap.Remove(sampler.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x00194125 File Offset: 0x00192325
		internal static void Register(BoingBones bones)
		{
			BoingManager.PreRegisterBones();
			BoingManager.s_bonesMap.Add(bones.GetInstanceID(), bones);
			if (BoingManager.OnBonesRegister != null)
			{
				BoingManager.OnBonesRegister(bones);
			}
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x0019414F File Offset: 0x0019234F
		internal static void Unregister(BoingBones bones)
		{
			if (BoingManager.OnBonesUnregister != null)
			{
				BoingManager.OnBonesUnregister(bones);
			}
			BoingManager.s_bonesMap.Remove(bones.GetInstanceID());
			BoingManager.PostUnregisterBones();
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x00194179 File Offset: 0x00192379
		private static void PreRegisterBehavior()
		{
			BoingManager.ValidateManager();
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x00194180 File Offset: 0x00192380
		private static void PostUnregisterBehavior()
		{
			if (BoingManager.s_behaviorMap.Count > 0)
			{
				return;
			}
			BoingWorkAsynchronous.PostUnregisterBehaviorCleanUp();
		}

		// Token: 0x06005250 RID: 21072 RVA: 0x00194198 File Offset: 0x00192398
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

		// Token: 0x06005251 RID: 21073 RVA: 0x00194228 File Offset: 0x00192428
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

		// Token: 0x06005252 RID: 21074 RVA: 0x00194179 File Offset: 0x00192379
		private static void PreRegisterBones()
		{
			BoingManager.ValidateManager();
		}

		// Token: 0x06005253 RID: 21075 RVA: 0x000023F4 File Offset: 0x000005F4
		private static void PostUnregisterBones()
		{
		}

		// Token: 0x06005254 RID: 21076 RVA: 0x00194292 File Offset: 0x00192492
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

		// Token: 0x06005255 RID: 21077 RVA: 0x001942BC File Offset: 0x001924BC
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

		// Token: 0x06005256 RID: 21078 RVA: 0x00194350 File Offset: 0x00192550
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

		// Token: 0x06005257 RID: 21079 RVA: 0x001943B8 File Offset: 0x001925B8
		internal static void RestoreBehaviors()
		{
			foreach (KeyValuePair<int, BoingBehavior> keyValuePair in BoingManager.s_behaviorMap)
			{
				keyValuePair.Value.Restore();
			}
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x00194410 File Offset: 0x00192610
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

		// Token: 0x06005259 RID: 21081 RVA: 0x001944E4 File Offset: 0x001926E4
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

		// Token: 0x0600525A RID: 21082 RVA: 0x001945BC File Offset: 0x001927BC
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

		// Token: 0x0600525B RID: 21083 RVA: 0x00194678 File Offset: 0x00192878
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

		// Token: 0x0600525C RID: 21084 RVA: 0x00194718 File Offset: 0x00192918
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

		// Token: 0x0600525D RID: 21085 RVA: 0x001947A4 File Offset: 0x001929A4
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

		// Token: 0x0600525E RID: 21086 RVA: 0x00194844 File Offset: 0x00192A44
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

		// Token: 0x0600525F RID: 21087 RVA: 0x0019487C File Offset: 0x00192A7C
		internal static void RestoreBones()
		{
			foreach (KeyValuePair<int, BoingBones> keyValuePair in BoingManager.s_bonesMap)
			{
				keyValuePair.Value.Restore();
			}
		}

		// Token: 0x040054BC RID: 21692
		public static BoingManager.BehaviorRegisterDelegate OnBehaviorRegister;

		// Token: 0x040054BD RID: 21693
		public static BoingManager.BehaviorUnregisterDelegate OnBehaviorUnregister;

		// Token: 0x040054BE RID: 21694
		public static BoingManager.EffectorRegisterDelegate OnEffectorRegister;

		// Token: 0x040054BF RID: 21695
		public static BoingManager.EffectorUnregisterDelegate OnEffectorUnregister;

		// Token: 0x040054C0 RID: 21696
		public static BoingManager.ReactorRegisterDelegate OnReactorRegister;

		// Token: 0x040054C1 RID: 21697
		public static BoingManager.ReactorUnregisterDelegate OnReactorUnregister;

		// Token: 0x040054C2 RID: 21698
		public static BoingManager.ReactorFieldRegisterDelegate OnReactorFieldRegister;

		// Token: 0x040054C3 RID: 21699
		public static BoingManager.ReactorFieldUnregisterDelegate OnReactorFieldUnregister;

		// Token: 0x040054C4 RID: 21700
		public static BoingManager.ReactorFieldCPUSamplerRegisterDelegate OnReactorFieldCPUSamplerRegister;

		// Token: 0x040054C5 RID: 21701
		public static BoingManager.ReactorFieldCPUSamplerUnregisterDelegate OnReactorFieldCPUSamplerUnregister;

		// Token: 0x040054C6 RID: 21702
		public static BoingManager.ReactorFieldGPUSamplerRegisterDelegate OnReactorFieldGPUSamplerRegister;

		// Token: 0x040054C7 RID: 21703
		public static BoingManager.ReactorFieldGPUSamplerUnregisterDelegate OnFieldGPUSamplerUnregister;

		// Token: 0x040054C8 RID: 21704
		public static BoingManager.BonesRegisterDelegate OnBonesRegister;

		// Token: 0x040054C9 RID: 21705
		public static BoingManager.BonesUnregisterDelegate OnBonesUnregister;

		// Token: 0x040054CA RID: 21706
		private static float s_deltaTime = 0f;

		// Token: 0x040054CB RID: 21707
		private static Dictionary<int, BoingBehavior> s_behaviorMap = new Dictionary<int, BoingBehavior>();

		// Token: 0x040054CC RID: 21708
		private static Dictionary<int, BoingEffector> s_effectorMap = new Dictionary<int, BoingEffector>();

		// Token: 0x040054CD RID: 21709
		private static Dictionary<int, BoingReactor> s_reactorMap = new Dictionary<int, BoingReactor>();

		// Token: 0x040054CE RID: 21710
		private static Dictionary<int, BoingReactorField> s_fieldMap = new Dictionary<int, BoingReactorField>();

		// Token: 0x040054CF RID: 21711
		private static Dictionary<int, BoingReactorFieldCPUSampler> s_cpuSamplerMap = new Dictionary<int, BoingReactorFieldCPUSampler>();

		// Token: 0x040054D0 RID: 21712
		private static Dictionary<int, BoingReactorFieldGPUSampler> s_gpuSamplerMap = new Dictionary<int, BoingReactorFieldGPUSampler>();

		// Token: 0x040054D1 RID: 21713
		private static Dictionary<int, BoingBones> s_bonesMap = new Dictionary<int, BoingBones>();

		// Token: 0x040054D2 RID: 21714
		private static readonly int kEffectorParamsIncrement = 16;

		// Token: 0x040054D3 RID: 21715
		private static List<BoingEffector.Params> s_effectorParamsList = new List<BoingEffector.Params>(BoingManager.kEffectorParamsIncrement);

		// Token: 0x040054D4 RID: 21716
		private static BoingEffector.Params[] s_aEffectorParams;

		// Token: 0x040054D5 RID: 21717
		private static ComputeBuffer s_effectorParamsBuffer;

		// Token: 0x040054D6 RID: 21718
		private static Dictionary<int, int> s_effectorParamsIndexMap = new Dictionary<int, int>();

		// Token: 0x040054D7 RID: 21719
		internal static readonly bool UseAsynchronousJobs = true;

		// Token: 0x040054D8 RID: 21720
		internal static GameObject s_managerGo;

		// Token: 0x02000CBC RID: 3260
		public enum UpdateMode
		{
			// Token: 0x040054DA RID: 21722
			FixedUpdate,
			// Token: 0x040054DB RID: 21723
			EarlyUpdate,
			// Token: 0x040054DC RID: 21724
			LateUpdate
		}

		// Token: 0x02000CBD RID: 3261
		public enum TranslationLockSpace
		{
			// Token: 0x040054DE RID: 21726
			Global,
			// Token: 0x040054DF RID: 21727
			Local
		}

		// Token: 0x02000CBE RID: 3262
		// (Invoke) Token: 0x06005262 RID: 21090
		public delegate void BehaviorRegisterDelegate(BoingBehavior behavior);

		// Token: 0x02000CBF RID: 3263
		// (Invoke) Token: 0x06005266 RID: 21094
		public delegate void BehaviorUnregisterDelegate(BoingBehavior behavior);

		// Token: 0x02000CC0 RID: 3264
		// (Invoke) Token: 0x0600526A RID: 21098
		public delegate void EffectorRegisterDelegate(BoingEffector effector);

		// Token: 0x02000CC1 RID: 3265
		// (Invoke) Token: 0x0600526E RID: 21102
		public delegate void EffectorUnregisterDelegate(BoingEffector effector);

		// Token: 0x02000CC2 RID: 3266
		// (Invoke) Token: 0x06005272 RID: 21106
		public delegate void ReactorRegisterDelegate(BoingReactor reactor);

		// Token: 0x02000CC3 RID: 3267
		// (Invoke) Token: 0x06005276 RID: 21110
		public delegate void ReactorUnregisterDelegate(BoingReactor reactor);

		// Token: 0x02000CC4 RID: 3268
		// (Invoke) Token: 0x0600527A RID: 21114
		public delegate void ReactorFieldRegisterDelegate(BoingReactorField field);

		// Token: 0x02000CC5 RID: 3269
		// (Invoke) Token: 0x0600527E RID: 21118
		public delegate void ReactorFieldUnregisterDelegate(BoingReactorField field);

		// Token: 0x02000CC6 RID: 3270
		// (Invoke) Token: 0x06005282 RID: 21122
		public delegate void ReactorFieldCPUSamplerRegisterDelegate(BoingReactorFieldCPUSampler sampler);

		// Token: 0x02000CC7 RID: 3271
		// (Invoke) Token: 0x06005286 RID: 21126
		public delegate void ReactorFieldCPUSamplerUnregisterDelegate(BoingReactorFieldCPUSampler sampler);

		// Token: 0x02000CC8 RID: 3272
		// (Invoke) Token: 0x0600528A RID: 21130
		public delegate void ReactorFieldGPUSamplerRegisterDelegate(BoingReactorFieldGPUSampler sampler);

		// Token: 0x02000CC9 RID: 3273
		// (Invoke) Token: 0x0600528E RID: 21134
		public delegate void ReactorFieldGPUSamplerUnregisterDelegate(BoingReactorFieldGPUSampler sampler);

		// Token: 0x02000CCA RID: 3274
		// (Invoke) Token: 0x06005292 RID: 21138
		public delegate void BonesRegisterDelegate(BoingBones bones);

		// Token: 0x02000CCB RID: 3275
		// (Invoke) Token: 0x06005296 RID: 21142
		public delegate void BonesUnregisterDelegate(BoingBones bones);
	}
}
