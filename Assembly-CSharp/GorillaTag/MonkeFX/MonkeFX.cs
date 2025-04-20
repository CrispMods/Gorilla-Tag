using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag.MonkeFX
{
	// Token: 0x02000BF9 RID: 3065
	public class MonkeFX : ITickSystemPost
	{
		// Token: 0x06004D79 RID: 19833 RVA: 0x001ABB98 File Offset: 0x001A9D98
		private static void InitBonesArray()
		{
			MonkeFX._rigs = VRRigCache.Instance.GetAllRigs();
			MonkeFX._bones = new Transform[MonkeFX._rigs.Length * MonkeFX._boneNames.Length];
			for (int i = 0; i < MonkeFX._rigs.Length; i++)
			{
				if (MonkeFX._rigs[i] == null)
				{
					MonkeFX._errorLog_nullVRRigFromVRRigCache.AddOccurrence(i.ToString());
				}
				else
				{
					int num = i * MonkeFX._boneNames.Length;
					if (MonkeFX._rigs[i].mainSkin == null)
					{
						MonkeFX._errorLog_nullMainSkin.AddOccurrence(MonkeFX._rigs[i].transform.GetPath());
						Debug.LogError("(This should never happen) Skipping null `mainSkin` on `VRRig`! Scene path: \n- \"" + MonkeFX._rigs[i].transform.GetPath() + "\"");
					}
					else
					{
						for (int j = 0; j < MonkeFX._rigs[i].mainSkin.bones.Length; j++)
						{
							Transform transform = MonkeFX._rigs[i].mainSkin.bones[j];
							if (transform == null)
							{
								MonkeFX._errorLog_nullBone.AddOccurrence(j.ToString());
							}
							else
							{
								for (int k = 0; k < MonkeFX._boneNames.Length; k++)
								{
									if (MonkeFX._boneNames[k] == transform.name)
									{
										MonkeFX._bones[num + k] = transform;
									}
								}
							}
						}
					}
				}
			}
			MonkeFX._errorLog_nullVRRigFromVRRigCache.LogOccurrences(VRRigCache.Instance, null, "InitBonesArray", "E:\\Dev\\GT\\Assets\\GorillaTag\\Shared\\Scripts\\MonkeFX\\MonkeFX-Bones.cs", 106);
			MonkeFX._errorLog_nullMainSkin.LogOccurrences(null, null, "InitBonesArray", "E:\\Dev\\GT\\Assets\\GorillaTag\\Shared\\Scripts\\MonkeFX\\MonkeFX-Bones.cs", 107);
			MonkeFX._errorLog_nullBone.LogOccurrences(null, null, "InitBonesArray", "E:\\Dev\\GT\\Assets\\GorillaTag\\Shared\\Scripts\\MonkeFX\\MonkeFX-Bones.cs", 108);
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x00030607 File Offset: 0x0002E807
		private static void UpdateBones()
		{
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x00030607 File Offset: 0x0002E807
		private static void UpdateBone()
		{
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x001ABD40 File Offset: 0x001A9F40
		public static void Register(MonkeFXSettingsSO settingsSO)
		{
			MonkeFX.EnsureInstance();
			if (settingsSO == null || !MonkeFX.instance._settingsSOs.Add(settingsSO))
			{
				return;
			}
			int num = MonkeFX.instance._srcMeshId_to_sourceMesh.Count;
			for (int i = 0; i < settingsSO.sourceMeshes.Length; i++)
			{
				Mesh obj = settingsSO.sourceMeshes[i].obj;
				if (!(obj == null) && MonkeFX.instance._srcMeshInst_to_meshId.TryAdd(obj.GetInstanceID(), num))
				{
					MonkeFX.instance._srcMeshId_to_sourceMesh.Add(obj);
					num++;
				}
			}
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x001ABDD8 File Offset: 0x001A9FD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetScaleToFitInBounds(Mesh mesh)
		{
			Bounds bounds = mesh.bounds;
			float num = Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z));
			if (num <= 0f)
			{
				return 0f;
			}
			return 1f / num;
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x00062CD7 File Offset: 0x00060ED7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Pack0To1Floats(float x, float y)
		{
			return Mathf.Clamp01(x) * 65536f + Mathf.Clamp01(y);
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06004D7F RID: 19839 RVA: 0x00062CEC File Offset: 0x00060EEC
		// (set) Token: 0x06004D80 RID: 19840 RVA: 0x00062CF3 File Offset: 0x00060EF3
		public static MonkeFX instance { get; private set; }

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06004D81 RID: 19841 RVA: 0x00062CFB File Offset: 0x00060EFB
		// (set) Token: 0x06004D82 RID: 19842 RVA: 0x00062D02 File Offset: 0x00060F02
		public static bool hasInstance { get; private set; }

		// Token: 0x06004D83 RID: 19843 RVA: 0x00062D0A File Offset: 0x00060F0A
		private static void EnsureInstance()
		{
			if (MonkeFX.hasInstance)
			{
				return;
			}
			MonkeFX.instance = new MonkeFX();
			MonkeFX.hasInstance = true;
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x00062D24 File Offset: 0x00060F24
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void OnAfterFirstSceneLoaded()
		{
			MonkeFX.EnsureInstance();
			TickSystem<object>.AddPostTickCallback(MonkeFX.instance);
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x00062D35 File Offset: 0x00060F35
		void ITickSystemPost.PostTick()
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			MonkeFX.UpdateBones();
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06004D86 RID: 19846 RVA: 0x00062D44 File Offset: 0x00060F44
		// (set) Token: 0x06004D87 RID: 19847 RVA: 0x00062D4C File Offset: 0x00060F4C
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004D88 RID: 19848 RVA: 0x00062D55 File Offset: 0x00060F55
		private static void PauseTick()
		{
			if (!MonkeFX.hasInstance)
			{
				MonkeFX.instance = new MonkeFX();
			}
			TickSystem<object>.RemovePostTickCallback(MonkeFX.instance);
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x00062D72 File Offset: 0x00060F72
		private static void ResumeTick()
		{
			if (!MonkeFX.hasInstance)
			{
				MonkeFX.instance = new MonkeFX();
			}
			TickSystem<object>.AddPostTickCallback(MonkeFX.instance);
		}

		// Token: 0x04004F10 RID: 20240
		private static readonly string[] _boneNames = new string[]
		{
			"body",
			"hand.L",
			"hand.R"
		};

		// Token: 0x04004F11 RID: 20241
		private static VRRig[] _rigs;

		// Token: 0x04004F12 RID: 20242
		private static Transform[] _bones;

		// Token: 0x04004F13 RID: 20243
		private static int _rigsHash;

		// Token: 0x04004F14 RID: 20244
		private static readonly GTLogErrorLimiter _errorLog_nullVRRigFromVRRigCache = new GTLogErrorLimiter("(This should never happen) Skipping null `VRRig` obtained from `VRRigCache`!", 10, "\n- ");

		// Token: 0x04004F15 RID: 20245
		private static GTLogErrorLimiter _errorLog_nullMainSkin = new GTLogErrorLimiter("(This should never happen) Skipping null `mainSkin` on `VRRig`! Scene paths: \n", 10, "\n- ");

		// Token: 0x04004F16 RID: 20246
		private static readonly GTLogErrorLimiter _errorLog_nullBone = new GTLogErrorLimiter("(This should never happen) Skipping null bone obtained from `VRRig.mainSkin.bones`! Index(es): ", 10, "\n- ");

		// Token: 0x04004F17 RID: 20247
		private readonly HashSet<MonkeFXSettingsSO> _settingsSOs = new HashSet<MonkeFXSettingsSO>(8);

		// Token: 0x04004F18 RID: 20248
		private readonly Dictionary<int, int> _srcMeshInst_to_meshId = new Dictionary<int, int>(8);

		// Token: 0x04004F19 RID: 20249
		private readonly List<Mesh> _srcMeshId_to_sourceMesh = new List<Mesh>(8);

		// Token: 0x04004F1A RID: 20250
		private readonly List<MonkeFX.ElementsRange> _srcMeshId_to_elemRange = new List<MonkeFX.ElementsRange>(8);

		// Token: 0x04004F1B RID: 20251
		private readonly Dictionary<int, List<MonkeFXSettingsSO>> _meshId_to_settingsUsers = new Dictionary<int, List<MonkeFXSettingsSO>>();

		// Token: 0x04004F1C RID: 20252
		private const float _k16BitFactor = 65536f;

		// Token: 0x02000BFA RID: 3066
		private struct ElementsRange
		{
			// Token: 0x04004F20 RID: 20256
			public int min;

			// Token: 0x04004F21 RID: 20257
			public int max;
		}
	}
}
