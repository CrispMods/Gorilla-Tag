using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag.MonkeFX
{
	// Token: 0x02000BCE RID: 3022
	public class MonkeFX : ITickSystemPost
	{
		// Token: 0x06004C39 RID: 19513 RVA: 0x0017374C File Offset: 0x0017194C
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
			MonkeFX._errorLog_nullVRRigFromVRRigCache.LogOccurrences(VRRigCache.Instance, null, "InitBonesArray", "C:\\Users\\root\\GT\\Assets\\GorillaTag\\Shared\\Scripts\\MonkeFX\\MonkeFX-Bones.cs", 106);
			MonkeFX._errorLog_nullMainSkin.LogOccurrences(null, null, "InitBonesArray", "C:\\Users\\root\\GT\\Assets\\GorillaTag\\Shared\\Scripts\\MonkeFX\\MonkeFX-Bones.cs", 107);
			MonkeFX._errorLog_nullBone.LogOccurrences(null, null, "InitBonesArray", "C:\\Users\\root\\GT\\Assets\\GorillaTag\\Shared\\Scripts\\MonkeFX\\MonkeFX-Bones.cs", 108);
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x000023F4 File Offset: 0x000005F4
		private static void UpdateBones()
		{
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x000023F4 File Offset: 0x000005F4
		private static void UpdateBone()
		{
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x001738F4 File Offset: 0x00171AF4
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

		// Token: 0x06004C3D RID: 19517 RVA: 0x0017398C File Offset: 0x00171B8C
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

		// Token: 0x06004C3E RID: 19518 RVA: 0x001739E4 File Offset: 0x00171BE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Pack0To1Floats(float x, float y)
		{
			return Mathf.Clamp01(x) * 65536f + Mathf.Clamp01(y);
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06004C3F RID: 19519 RVA: 0x001739F9 File Offset: 0x00171BF9
		// (set) Token: 0x06004C40 RID: 19520 RVA: 0x00173A00 File Offset: 0x00171C00
		public static MonkeFX instance { get; private set; }

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06004C41 RID: 19521 RVA: 0x00173A08 File Offset: 0x00171C08
		// (set) Token: 0x06004C42 RID: 19522 RVA: 0x00173A0F File Offset: 0x00171C0F
		public static bool hasInstance { get; private set; }

		// Token: 0x06004C43 RID: 19523 RVA: 0x00173A17 File Offset: 0x00171C17
		private static void EnsureInstance()
		{
			if (MonkeFX.hasInstance)
			{
				return;
			}
			MonkeFX.instance = new MonkeFX();
			MonkeFX.hasInstance = true;
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x00173A31 File Offset: 0x00171C31
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void OnAfterFirstSceneLoaded()
		{
			MonkeFX.EnsureInstance();
			TickSystem<object>.AddPostTickCallback(MonkeFX.instance);
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x00173A42 File Offset: 0x00171C42
		void ITickSystemPost.PostTick()
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			MonkeFX.UpdateBones();
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06004C46 RID: 19526 RVA: 0x00173A51 File Offset: 0x00171C51
		// (set) Token: 0x06004C47 RID: 19527 RVA: 0x00173A59 File Offset: 0x00171C59
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004C48 RID: 19528 RVA: 0x00173A62 File Offset: 0x00171C62
		private static void PauseTick()
		{
			if (!MonkeFX.hasInstance)
			{
				MonkeFX.instance = new MonkeFX();
			}
			TickSystem<object>.RemovePostTickCallback(MonkeFX.instance);
		}

		// Token: 0x06004C49 RID: 19529 RVA: 0x00173A7F File Offset: 0x00171C7F
		private static void ResumeTick()
		{
			if (!MonkeFX.hasInstance)
			{
				MonkeFX.instance = new MonkeFX();
			}
			TickSystem<object>.AddPostTickCallback(MonkeFX.instance);
		}

		// Token: 0x04004E2C RID: 20012
		private static readonly string[] _boneNames = new string[]
		{
			"body",
			"hand.L",
			"hand.R"
		};

		// Token: 0x04004E2D RID: 20013
		private static VRRig[] _rigs;

		// Token: 0x04004E2E RID: 20014
		private static Transform[] _bones;

		// Token: 0x04004E2F RID: 20015
		private static int _rigsHash;

		// Token: 0x04004E30 RID: 20016
		private static readonly GTLogErrorLimiter _errorLog_nullVRRigFromVRRigCache = new GTLogErrorLimiter("(This should never happen) Skipping null `VRRig` obtained from `VRRigCache`!", 10, "\n- ");

		// Token: 0x04004E31 RID: 20017
		private static GTLogErrorLimiter _errorLog_nullMainSkin = new GTLogErrorLimiter("(This should never happen) Skipping null `mainSkin` on `VRRig`! Scene paths: \n", 10, "\n- ");

		// Token: 0x04004E32 RID: 20018
		private static readonly GTLogErrorLimiter _errorLog_nullBone = new GTLogErrorLimiter("(This should never happen) Skipping null bone obtained from `VRRig.mainSkin.bones`! Index(es): ", 10, "\n- ");

		// Token: 0x04004E33 RID: 20019
		private readonly HashSet<MonkeFXSettingsSO> _settingsSOs = new HashSet<MonkeFXSettingsSO>(8);

		// Token: 0x04004E34 RID: 20020
		private readonly Dictionary<int, int> _srcMeshInst_to_meshId = new Dictionary<int, int>(8);

		// Token: 0x04004E35 RID: 20021
		private readonly List<Mesh> _srcMeshId_to_sourceMesh = new List<Mesh>(8);

		// Token: 0x04004E36 RID: 20022
		private readonly List<MonkeFX.ElementsRange> _srcMeshId_to_elemRange = new List<MonkeFX.ElementsRange>(8);

		// Token: 0x04004E37 RID: 20023
		private readonly Dictionary<int, List<MonkeFXSettingsSO>> _meshId_to_settingsUsers = new Dictionary<int, List<MonkeFXSettingsSO>>();

		// Token: 0x04004E38 RID: 20024
		private const float _k16BitFactor = 65536f;

		// Token: 0x02000BCF RID: 3023
		private struct ElementsRange
		{
			// Token: 0x04004E3C RID: 20028
			public int min;

			// Token: 0x04004E3D RID: 20029
			public int max;
		}
	}
}
