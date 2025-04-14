using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag.MonkeFX
{
	// Token: 0x02000BCB RID: 3019
	public class MonkeFX : ITickSystemPost
	{
		// Token: 0x06004C2D RID: 19501 RVA: 0x00173184 File Offset: 0x00171384
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

		// Token: 0x06004C2E RID: 19502 RVA: 0x000023F4 File Offset: 0x000005F4
		private static void UpdateBones()
		{
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x000023F4 File Offset: 0x000005F4
		private static void UpdateBone()
		{
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x0017332C File Offset: 0x0017152C
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

		// Token: 0x06004C31 RID: 19505 RVA: 0x001733C4 File Offset: 0x001715C4
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

		// Token: 0x06004C32 RID: 19506 RVA: 0x0017341C File Offset: 0x0017161C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Pack0To1Floats(float x, float y)
		{
			return Mathf.Clamp01(x) * 65536f + Mathf.Clamp01(y);
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06004C33 RID: 19507 RVA: 0x00173431 File Offset: 0x00171631
		// (set) Token: 0x06004C34 RID: 19508 RVA: 0x00173438 File Offset: 0x00171638
		public static MonkeFX instance { get; private set; }

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06004C35 RID: 19509 RVA: 0x00173440 File Offset: 0x00171640
		// (set) Token: 0x06004C36 RID: 19510 RVA: 0x00173447 File Offset: 0x00171647
		public static bool hasInstance { get; private set; }

		// Token: 0x06004C37 RID: 19511 RVA: 0x0017344F File Offset: 0x0017164F
		private static void EnsureInstance()
		{
			if (MonkeFX.hasInstance)
			{
				return;
			}
			MonkeFX.instance = new MonkeFX();
			MonkeFX.hasInstance = true;
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x00173469 File Offset: 0x00171669
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void OnAfterFirstSceneLoaded()
		{
			MonkeFX.EnsureInstance();
			TickSystem<object>.AddPostTickCallback(MonkeFX.instance);
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x0017347A File Offset: 0x0017167A
		void ITickSystemPost.PostTick()
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			MonkeFX.UpdateBones();
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06004C3A RID: 19514 RVA: 0x00173489 File Offset: 0x00171689
		// (set) Token: 0x06004C3B RID: 19515 RVA: 0x00173491 File Offset: 0x00171691
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004C3C RID: 19516 RVA: 0x0017349A File Offset: 0x0017169A
		private static void PauseTick()
		{
			if (!MonkeFX.hasInstance)
			{
				MonkeFX.instance = new MonkeFX();
			}
			TickSystem<object>.RemovePostTickCallback(MonkeFX.instance);
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x001734B7 File Offset: 0x001716B7
		private static void ResumeTick()
		{
			if (!MonkeFX.hasInstance)
			{
				MonkeFX.instance = new MonkeFX();
			}
			TickSystem<object>.AddPostTickCallback(MonkeFX.instance);
		}

		// Token: 0x04004E1A RID: 19994
		private static readonly string[] _boneNames = new string[]
		{
			"body",
			"hand.L",
			"hand.R"
		};

		// Token: 0x04004E1B RID: 19995
		private static VRRig[] _rigs;

		// Token: 0x04004E1C RID: 19996
		private static Transform[] _bones;

		// Token: 0x04004E1D RID: 19997
		private static int _rigsHash;

		// Token: 0x04004E1E RID: 19998
		private static readonly GTLogErrorLimiter _errorLog_nullVRRigFromVRRigCache = new GTLogErrorLimiter("(This should never happen) Skipping null `VRRig` obtained from `VRRigCache`!", 10, "\n- ");

		// Token: 0x04004E1F RID: 19999
		private static GTLogErrorLimiter _errorLog_nullMainSkin = new GTLogErrorLimiter("(This should never happen) Skipping null `mainSkin` on `VRRig`! Scene paths: \n", 10, "\n- ");

		// Token: 0x04004E20 RID: 20000
		private static readonly GTLogErrorLimiter _errorLog_nullBone = new GTLogErrorLimiter("(This should never happen) Skipping null bone obtained from `VRRig.mainSkin.bones`! Index(es): ", 10, "\n- ");

		// Token: 0x04004E21 RID: 20001
		private readonly HashSet<MonkeFXSettingsSO> _settingsSOs = new HashSet<MonkeFXSettingsSO>(8);

		// Token: 0x04004E22 RID: 20002
		private readonly Dictionary<int, int> _srcMeshInst_to_meshId = new Dictionary<int, int>(8);

		// Token: 0x04004E23 RID: 20003
		private readonly List<Mesh> _srcMeshId_to_sourceMesh = new List<Mesh>(8);

		// Token: 0x04004E24 RID: 20004
		private readonly List<MonkeFX.ElementsRange> _srcMeshId_to_elemRange = new List<MonkeFX.ElementsRange>(8);

		// Token: 0x04004E25 RID: 20005
		private readonly Dictionary<int, List<MonkeFXSettingsSO>> _meshId_to_settingsUsers = new Dictionary<int, List<MonkeFXSettingsSO>>();

		// Token: 0x04004E26 RID: 20006
		private const float _k16BitFactor = 65536f;

		// Token: 0x02000BCC RID: 3020
		private struct ElementsRange
		{
			// Token: 0x04004E2A RID: 20010
			public int min;

			// Token: 0x04004E2B RID: 20011
			public int max;
		}
	}
}
