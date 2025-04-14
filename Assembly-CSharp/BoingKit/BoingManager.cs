using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CB8 RID: 3256
	public static class BoingManager
	{
		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06005224 RID: 21028 RVA: 0x00193850 File Offset: 0x00191A50
		public static IEnumerable<BoingBehavior> Behaviors
		{
			get
			{
				return BoingManager.s_behaviorMap.Values;
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06005225 RID: 21029 RVA: 0x0019385C File Offset: 0x00191A5C
		public static IEnumerable<BoingReactor> Reactors
		{
			get
			{
				return BoingManager.s_reactorMap.Values;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06005226 RID: 21030 RVA: 0x00193868 File Offset: 0x00191A68
		public static IEnumerable<BoingEffector> Effectors
		{
			get
			{
				return BoingManager.s_effectorMap.Values;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06005227 RID: 21031 RVA: 0x00193874 File Offset: 0x00191A74
		public static IEnumerable<BoingReactorField> ReactorFields
		{
			get
			{
				return BoingManager.s_fieldMap.Values;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06005228 RID: 21032 RVA: 0x00193880 File Offset: 0x00191A80
		public static IEnumerable<BoingReactorFieldCPUSampler> ReactorFieldCPUSamlers
		{
			get
			{
				return BoingManager.s_cpuSamplerMap.Values;
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06005229 RID: 21033 RVA: 0x0019388C File Offset: 0x00191A8C
		public static IEnumerable<BoingReactorFieldGPUSampler> ReactorFieldGPUSampler
		{
			get
			{
				return BoingManager.s_gpuSamplerMap.Values;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x0600522A RID: 21034 RVA: 0x00193898 File Offset: 0x00191A98
		public static float DeltaTime
		{
			get
			{
				return BoingManager.s_deltaTime;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x0600522B RID: 21035 RVA: 0x0019389F File Offset: 0x00191A9F
		public static float FixedDeltaTime
		{
			get
			{
				return Time.fixedDeltaTime;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x0600522C RID: 21036 RVA: 0x001938A6 File Offset: 0x00191AA6
		internal static int NumBehaviors
		{
			get
			{
				return BoingManager.s_behaviorMap.Count;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x0600522D RID: 21037 RVA: 0x001938B2 File Offset: 0x00191AB2
		internal static int NumEffectors
		{
			get
			{
				return BoingManager.s_effectorMap.Count;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x0600522E RID: 21038 RVA: 0x001938BE File Offset: 0x00191ABE
		internal static int NumReactors
		{
			get
			{
				return BoingManager.s_reactorMap.Count;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x0600522F RID: 21039 RVA: 0x001938CA File Offset: 0x00191ACA
		internal static int NumFields
		{
			get
			{
				return BoingManager.s_fieldMap.Count;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06005230 RID: 21040 RVA: 0x001938D6 File Offset: 0x00191AD6
		internal static int NumCPUFieldSamplers
		{
			get
			{
				return BoingManager.s_cpuSamplerMap.Count;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06005231 RID: 21041 RVA: 0x001938E2 File Offset: 0x00191AE2
		internal static int NumGPUFieldSamplers
		{
			get
			{
				return BoingManager.s_gpuSamplerMap.Count;
			}
		}

		// Token: 0x06005232 RID: 21042 RVA: 0x001938F0 File Offset: 0x00191AF0
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

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06005233 RID: 21043 RVA: 0x0019394A File Offset: 0x00191B4A
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

		// Token: 0x06005234 RID: 21044 RVA: 0x00193965 File Offset: 0x00191B65
		internal static void Register(BoingBehavior behavior)
		{
			BoingManager.PreRegisterBehavior();
			BoingManager.s_behaviorMap.Add(behavior.GetInstanceID(), behavior);
			if (BoingManager.OnBehaviorRegister != null)
			{
				BoingManager.OnBehaviorRegister(behavior);
			}
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x0019398F File Offset: 0x00191B8F
		internal static void Unregister(BoingBehavior behavior)
		{
			if (BoingManager.OnBehaviorUnregister != null)
			{
				BoingManager.OnBehaviorUnregister(behavior);
			}
			BoingManager.s_behaviorMap.Remove(behavior.GetInstanceID());
			BoingManager.PostUnregisterBehavior();
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x001939B9 File Offset: 0x00191BB9
		internal static void Register(BoingEffector effector)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_effectorMap.Add(effector.GetInstanceID(), effector);
			if (BoingManager.OnEffectorRegister != null)
			{
				BoingManager.OnEffectorRegister(effector);
			}
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x001939E3 File Offset: 0x00191BE3
		internal static void Unregister(BoingEffector effector)
		{
			if (BoingManager.OnEffectorUnregister != null)
			{
				BoingManager.OnEffectorUnregister(effector);
			}
			BoingManager.s_effectorMap.Remove(effector.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x06005238 RID: 21048 RVA: 0x00193A0D File Offset: 0x00191C0D
		internal static void Register(BoingReactor reactor)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_reactorMap.Add(reactor.GetInstanceID(), reactor);
			if (BoingManager.OnReactorRegister != null)
			{
				BoingManager.OnReactorRegister(reactor);
			}
		}

		// Token: 0x06005239 RID: 21049 RVA: 0x00193A37 File Offset: 0x00191C37
		internal static void Unregister(BoingReactor reactor)
		{
			if (BoingManager.OnReactorUnregister != null)
			{
				BoingManager.OnReactorUnregister(reactor);
			}
			BoingManager.s_reactorMap.Remove(reactor.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x0600523A RID: 21050 RVA: 0x00193A61 File Offset: 0x00191C61
		internal static void Register(BoingReactorField field)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_fieldMap.Add(field.GetInstanceID(), field);
			if (BoingManager.OnReactorFieldRegister != null)
			{
				BoingManager.OnReactorFieldRegister(field);
			}
		}

		// Token: 0x0600523B RID: 21051 RVA: 0x00193A8B File Offset: 0x00191C8B
		internal static void Unregister(BoingReactorField field)
		{
			if (BoingManager.OnReactorFieldUnregister != null)
			{
				BoingManager.OnReactorFieldUnregister(field);
			}
			BoingManager.s_fieldMap.Remove(field.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x0600523C RID: 21052 RVA: 0x00193AB5 File Offset: 0x00191CB5
		internal static void Register(BoingReactorFieldCPUSampler sampler)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_cpuSamplerMap.Add(sampler.GetInstanceID(), sampler);
			if (BoingManager.OnReactorFieldCPUSamplerRegister != null)
			{
				BoingManager.OnReactorFieldCPUSamplerUnregister(sampler);
			}
		}

		// Token: 0x0600523D RID: 21053 RVA: 0x00193ADF File Offset: 0x00191CDF
		internal static void Unregister(BoingReactorFieldCPUSampler sampler)
		{
			if (BoingManager.OnReactorFieldCPUSamplerUnregister != null)
			{
				BoingManager.OnReactorFieldCPUSamplerUnregister(sampler);
			}
			BoingManager.s_cpuSamplerMap.Remove(sampler.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x0600523E RID: 21054 RVA: 0x00193B09 File Offset: 0x00191D09
		internal static void Register(BoingReactorFieldGPUSampler sampler)
		{
			BoingManager.PreRegisterEffectorReactor();
			BoingManager.s_gpuSamplerMap.Add(sampler.GetInstanceID(), sampler);
			if (BoingManager.OnReactorFieldGPUSamplerRegister != null)
			{
				BoingManager.OnReactorFieldGPUSamplerRegister(sampler);
			}
		}

		// Token: 0x0600523F RID: 21055 RVA: 0x00193B33 File Offset: 0x00191D33
		internal static void Unregister(BoingReactorFieldGPUSampler sampler)
		{
			if (BoingManager.OnFieldGPUSamplerUnregister != null)
			{
				BoingManager.OnFieldGPUSamplerUnregister(sampler);
			}
			BoingManager.s_gpuSamplerMap.Remove(sampler.GetInstanceID());
			BoingManager.PostUnregisterEffectorReactor();
		}

		// Token: 0x06005240 RID: 21056 RVA: 0x00193B5D File Offset: 0x00191D5D
		internal static void Register(BoingBones bones)
		{
			BoingManager.PreRegisterBones();
			BoingManager.s_bonesMap.Add(bones.GetInstanceID(), bones);
			if (BoingManager.OnBonesRegister != null)
			{
				BoingManager.OnBonesRegister(bones);
			}
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x00193B87 File Offset: 0x00191D87
		internal static void Unregister(BoingBones bones)
		{
			if (BoingManager.OnBonesUnregister != null)
			{
				BoingManager.OnBonesUnregister(bones);
			}
			BoingManager.s_bonesMap.Remove(bones.GetInstanceID());
			BoingManager.PostUnregisterBones();
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x00193BB1 File Offset: 0x00191DB1
		private static void PreRegisterBehavior()
		{
			BoingManager.ValidateManager();
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x00193BB8 File Offset: 0x00191DB8
		private static void PostUnregisterBehavior()
		{
			if (BoingManager.s_behaviorMap.Count > 0)
			{
				return;
			}
			BoingWorkAsynchronous.PostUnregisterBehaviorCleanUp();
		}

		// Token: 0x06005244 RID: 21060 RVA: 0x00193BD0 File Offset: 0x00191DD0
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

		// Token: 0x06005245 RID: 21061 RVA: 0x00193C60 File Offset: 0x00191E60
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

		// Token: 0x06005246 RID: 21062 RVA: 0x00193BB1 File Offset: 0x00191DB1
		private static void PreRegisterBones()
		{
			BoingManager.ValidateManager();
		}

		// Token: 0x06005247 RID: 21063 RVA: 0x000023F4 File Offset: 0x000005F4
		private static void PostUnregisterBones()
		{
		}

		// Token: 0x06005248 RID: 21064 RVA: 0x00193CCA File Offset: 0x00191ECA
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

		// Token: 0x06005249 RID: 21065 RVA: 0x00193CF4 File Offset: 0x00191EF4
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

		// Token: 0x0600524A RID: 21066 RVA: 0x00193D88 File Offset: 0x00191F88
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

		// Token: 0x0600524B RID: 21067 RVA: 0x00193DF0 File Offset: 0x00191FF0
		internal static void RestoreBehaviors()
		{
			foreach (KeyValuePair<int, BoingBehavior> keyValuePair in BoingManager.s_behaviorMap)
			{
				keyValuePair.Value.Restore();
			}
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x00193E48 File Offset: 0x00192048
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

		// Token: 0x0600524D RID: 21069 RVA: 0x00193F1C File Offset: 0x0019211C
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

		// Token: 0x0600524E RID: 21070 RVA: 0x00193FF4 File Offset: 0x001921F4
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

		// Token: 0x0600524F RID: 21071 RVA: 0x001940B0 File Offset: 0x001922B0
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

		// Token: 0x06005250 RID: 21072 RVA: 0x00194150 File Offset: 0x00192350
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

		// Token: 0x06005251 RID: 21073 RVA: 0x001941DC File Offset: 0x001923DC
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

		// Token: 0x06005252 RID: 21074 RVA: 0x0019427C File Offset: 0x0019247C
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

		// Token: 0x06005253 RID: 21075 RVA: 0x001942B4 File Offset: 0x001924B4
		internal static void RestoreBones()
		{
			foreach (KeyValuePair<int, BoingBones> keyValuePair in BoingManager.s_bonesMap)
			{
				keyValuePair.Value.Restore();
			}
		}

		// Token: 0x040054AA RID: 21674
		public static BoingManager.BehaviorRegisterDelegate OnBehaviorRegister;

		// Token: 0x040054AB RID: 21675
		public static BoingManager.BehaviorUnregisterDelegate OnBehaviorUnregister;

		// Token: 0x040054AC RID: 21676
		public static BoingManager.EffectorRegisterDelegate OnEffectorRegister;

		// Token: 0x040054AD RID: 21677
		public static BoingManager.EffectorUnregisterDelegate OnEffectorUnregister;

		// Token: 0x040054AE RID: 21678
		public static BoingManager.ReactorRegisterDelegate OnReactorRegister;

		// Token: 0x040054AF RID: 21679
		public static BoingManager.ReactorUnregisterDelegate OnReactorUnregister;

		// Token: 0x040054B0 RID: 21680
		public static BoingManager.ReactorFieldRegisterDelegate OnReactorFieldRegister;

		// Token: 0x040054B1 RID: 21681
		public static BoingManager.ReactorFieldUnregisterDelegate OnReactorFieldUnregister;

		// Token: 0x040054B2 RID: 21682
		public static BoingManager.ReactorFieldCPUSamplerRegisterDelegate OnReactorFieldCPUSamplerRegister;

		// Token: 0x040054B3 RID: 21683
		public static BoingManager.ReactorFieldCPUSamplerUnregisterDelegate OnReactorFieldCPUSamplerUnregister;

		// Token: 0x040054B4 RID: 21684
		public static BoingManager.ReactorFieldGPUSamplerRegisterDelegate OnReactorFieldGPUSamplerRegister;

		// Token: 0x040054B5 RID: 21685
		public static BoingManager.ReactorFieldGPUSamplerUnregisterDelegate OnFieldGPUSamplerUnregister;

		// Token: 0x040054B6 RID: 21686
		public static BoingManager.BonesRegisterDelegate OnBonesRegister;

		// Token: 0x040054B7 RID: 21687
		public static BoingManager.BonesUnregisterDelegate OnBonesUnregister;

		// Token: 0x040054B8 RID: 21688
		private static float s_deltaTime = 0f;

		// Token: 0x040054B9 RID: 21689
		private static Dictionary<int, BoingBehavior> s_behaviorMap = new Dictionary<int, BoingBehavior>();

		// Token: 0x040054BA RID: 21690
		private static Dictionary<int, BoingEffector> s_effectorMap = new Dictionary<int, BoingEffector>();

		// Token: 0x040054BB RID: 21691
		private static Dictionary<int, BoingReactor> s_reactorMap = new Dictionary<int, BoingReactor>();

		// Token: 0x040054BC RID: 21692
		private static Dictionary<int, BoingReactorField> s_fieldMap = new Dictionary<int, BoingReactorField>();

		// Token: 0x040054BD RID: 21693
		private static Dictionary<int, BoingReactorFieldCPUSampler> s_cpuSamplerMap = new Dictionary<int, BoingReactorFieldCPUSampler>();

		// Token: 0x040054BE RID: 21694
		private static Dictionary<int, BoingReactorFieldGPUSampler> s_gpuSamplerMap = new Dictionary<int, BoingReactorFieldGPUSampler>();

		// Token: 0x040054BF RID: 21695
		private static Dictionary<int, BoingBones> s_bonesMap = new Dictionary<int, BoingBones>();

		// Token: 0x040054C0 RID: 21696
		private static readonly int kEffectorParamsIncrement = 16;

		// Token: 0x040054C1 RID: 21697
		private static List<BoingEffector.Params> s_effectorParamsList = new List<BoingEffector.Params>(BoingManager.kEffectorParamsIncrement);

		// Token: 0x040054C2 RID: 21698
		private static BoingEffector.Params[] s_aEffectorParams;

		// Token: 0x040054C3 RID: 21699
		private static ComputeBuffer s_effectorParamsBuffer;

		// Token: 0x040054C4 RID: 21700
		private static Dictionary<int, int> s_effectorParamsIndexMap = new Dictionary<int, int>();

		// Token: 0x040054C5 RID: 21701
		internal static readonly bool UseAsynchronousJobs = true;

		// Token: 0x040054C6 RID: 21702
		internal static GameObject s_managerGo;

		// Token: 0x02000CB9 RID: 3257
		public enum UpdateMode
		{
			// Token: 0x040054C8 RID: 21704
			FixedUpdate,
			// Token: 0x040054C9 RID: 21705
			EarlyUpdate,
			// Token: 0x040054CA RID: 21706
			LateUpdate
		}

		// Token: 0x02000CBA RID: 3258
		public enum TranslationLockSpace
		{
			// Token: 0x040054CC RID: 21708
			Global,
			// Token: 0x040054CD RID: 21709
			Local
		}

		// Token: 0x02000CBB RID: 3259
		// (Invoke) Token: 0x06005256 RID: 21078
		public delegate void BehaviorRegisterDelegate(BoingBehavior behavior);

		// Token: 0x02000CBC RID: 3260
		// (Invoke) Token: 0x0600525A RID: 21082
		public delegate void BehaviorUnregisterDelegate(BoingBehavior behavior);

		// Token: 0x02000CBD RID: 3261
		// (Invoke) Token: 0x0600525E RID: 21086
		public delegate void EffectorRegisterDelegate(BoingEffector effector);

		// Token: 0x02000CBE RID: 3262
		// (Invoke) Token: 0x06005262 RID: 21090
		public delegate void EffectorUnregisterDelegate(BoingEffector effector);

		// Token: 0x02000CBF RID: 3263
		// (Invoke) Token: 0x06005266 RID: 21094
		public delegate void ReactorRegisterDelegate(BoingReactor reactor);

		// Token: 0x02000CC0 RID: 3264
		// (Invoke) Token: 0x0600526A RID: 21098
		public delegate void ReactorUnregisterDelegate(BoingReactor reactor);

		// Token: 0x02000CC1 RID: 3265
		// (Invoke) Token: 0x0600526E RID: 21102
		public delegate void ReactorFieldRegisterDelegate(BoingReactorField field);

		// Token: 0x02000CC2 RID: 3266
		// (Invoke) Token: 0x06005272 RID: 21106
		public delegate void ReactorFieldUnregisterDelegate(BoingReactorField field);

		// Token: 0x02000CC3 RID: 3267
		// (Invoke) Token: 0x06005276 RID: 21110
		public delegate void ReactorFieldCPUSamplerRegisterDelegate(BoingReactorFieldCPUSampler sampler);

		// Token: 0x02000CC4 RID: 3268
		// (Invoke) Token: 0x0600527A RID: 21114
		public delegate void ReactorFieldCPUSamplerUnregisterDelegate(BoingReactorFieldCPUSampler sampler);

		// Token: 0x02000CC5 RID: 3269
		// (Invoke) Token: 0x0600527E RID: 21118
		public delegate void ReactorFieldGPUSamplerRegisterDelegate(BoingReactorFieldGPUSampler sampler);

		// Token: 0x02000CC6 RID: 3270
		// (Invoke) Token: 0x06005282 RID: 21122
		public delegate void ReactorFieldGPUSamplerUnregisterDelegate(BoingReactorFieldGPUSampler sampler);

		// Token: 0x02000CC7 RID: 3271
		// (Invoke) Token: 0x06005286 RID: 21126
		public delegate void BonesRegisterDelegate(BoingBones bones);

		// Token: 0x02000CC8 RID: 3272
		// (Invoke) Token: 0x0600528A RID: 21130
		public delegate void BonesUnregisterDelegate(BoingBones bones);
	}
}
