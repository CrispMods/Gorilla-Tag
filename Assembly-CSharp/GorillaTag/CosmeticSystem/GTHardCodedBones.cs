using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000C25 RID: 3109
	public static class GTHardCodedBones
	{
		// Token: 0x06004E0F RID: 19983 RVA: 0x0006313A File Offset: 0x0006133A
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void HandleRuntimeInitialize_OnBeforeSceneLoad()
		{
			VRRigCache.OnPostInitialize += GTHardCodedBones.HandleVRRigCache_OnPostInitialize;
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x0006314D File Offset: 0x0006134D
		private static void HandleVRRigCache_OnPostInitialize()
		{
			VRRigCache.OnPostInitialize -= GTHardCodedBones.HandleVRRigCache_OnPostInitialize;
			GTHardCodedBones.HandleVRRigCache_OnPostSpawnRig();
			VRRigCache.OnPostSpawnRig += GTHardCodedBones.HandleVRRigCache_OnPostSpawnRig;
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x001ADA58 File Offset: 0x001ABC58
		private static void HandleVRRigCache_OnPostSpawnRig()
		{
			if (!VRRigCache.isInitialized || ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			Transform[] array;
			string str;
			if (!GTHardCodedBones.TryGetBoneXforms(VRRig.LocalRig, out array, out str))
			{
				Debug.LogError("GTHardCodedBones.HandleVRRigCache_OnPostSpawnRig: Error getting bone Transforms: " + str);
			}
			VRRig[] allRigs = VRRigCache.Instance.GetAllRigs();
			for (int i = 0; i < allRigs.Length; i++)
			{
				Transform[] array2;
				string text;
				if (!GTHardCodedBones.TryGetBoneXforms(allRigs[i], out array2, out text))
				{
					Debug.LogError("GTHardCodedBones.HandleVRRigCache_OnPostSpawnRig: Error getting bone Transforms: " + str, allRigs[i]);
					return;
				}
			}
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x00038A24 File Offset: 0x00036C24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetBoneIndex(GTHardCodedBones.EBone bone)
		{
			return (int)bone;
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x001ADAD0 File Offset: 0x001ABCD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetBoneIndex(string name)
		{
			for (int i = 0; i < GTHardCodedBones.kBoneNames.Length; i++)
			{
				if (GTHardCodedBones.kBoneNames[i] == name)
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x001ADB04 File Offset: 0x001ABD04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetBoneIndexByName(string name, out int out_index)
		{
			for (int i = 0; i < GTHardCodedBones.kBoneNames.Length; i++)
			{
				if (GTHardCodedBones.kBoneNames[i] == name)
				{
					out_index = i;
					return true;
				}
			}
			out_index = 0;
			return false;
		}

		// Token: 0x06004E15 RID: 19989 RVA: 0x00063176 File Offset: 0x00061376
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GTHardCodedBones.EBone GetBone(string name)
		{
			return (GTHardCodedBones.EBone)GTHardCodedBones.GetBoneIndex(name);
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x001ADB3C File Offset: 0x001ABD3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetBoneByName(string name, out GTHardCodedBones.EBone out_eBone)
		{
			int num;
			if (GTHardCodedBones.TryGetBoneIndexByName(name, out num))
			{
				out_eBone = (GTHardCodedBones.EBone)num;
				return true;
			}
			out_eBone = GTHardCodedBones.EBone.None;
			return false;
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x0006317E File Offset: 0x0006137E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetBoneName(int boneIndex)
		{
			return GTHardCodedBones.kBoneNames[boneIndex];
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x00063187 File Offset: 0x00061387
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetBoneName(int boneIndex, out string out_name)
		{
			if (boneIndex >= 0 && boneIndex < GTHardCodedBones.kBoneNames.Length)
			{
				out_name = GTHardCodedBones.kBoneNames[boneIndex];
				return true;
			}
			out_name = "None";
			return false;
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x000631AA File Offset: 0x000613AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetBoneName(GTHardCodedBones.EBone bone)
		{
			return GTHardCodedBones.GetBoneName((int)bone);
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x000631B2 File Offset: 0x000613B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetBoneName(GTHardCodedBones.EBone bone, out string out_name)
		{
			return GTHardCodedBones.TryGetBoneName((int)bone, out out_name);
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x001ADB5C File Offset: 0x001ABD5C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long GetBoneBitFlag(string name)
		{
			if (name == "None")
			{
				return 0L;
			}
			for (int i = 0; i < GTHardCodedBones.kBoneNames.Length; i++)
			{
				if (GTHardCodedBones.kBoneNames[i] == name)
				{
					return 1L << i - 1;
				}
			}
			return 0L;
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x000631BB File Offset: 0x000613BB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long GetBoneBitFlag(GTHardCodedBones.EBone bone)
		{
			if (bone == GTHardCodedBones.EBone.None)
			{
				return 0L;
			}
			return 1L << bone - GTHardCodedBones.EBone.rig;
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x000631CC File Offset: 0x000613CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static EHandedness GetHandednessFromBone(GTHardCodedBones.EBone bone)
		{
			if ((GTHardCodedBones.GetBoneBitFlag(bone) & 1728432283058160L) != 0L)
			{
				return EHandedness.Left;
			}
			if ((GTHardCodedBones.GetBoneBitFlag(bone) & 1769114204897280L) == 0L)
			{
				return EHandedness.None;
			}
			return EHandedness.Right;
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x001ADBA8 File Offset: 0x001ABDA8
		public static bool TryGetBoneXforms(VRRig vrRig, out Transform[] outBoneXforms, out string outErrorMsg)
		{
			outErrorMsg = string.Empty;
			if (vrRig == null)
			{
				outErrorMsg = "The VRRig is null.";
				outBoneXforms = Array.Empty<Transform>();
				return false;
			}
			int instanceID = vrRig.GetInstanceID();
			if (GTHardCodedBones._gInstIds_To_boneXforms.TryGetValue(instanceID, out outBoneXforms))
			{
				return true;
			}
			if (!GTHardCodedBones.TryGetBoneXforms(vrRig.mainSkin, out outBoneXforms, out outErrorMsg))
			{
				return false;
			}
			VRRigAnchorOverrides componentInChildren = vrRig.GetComponentInChildren<VRRigAnchorOverrides>(true);
			BodyDockPositions componentInChildren2 = vrRig.GetComponentInChildren<BodyDockPositions>(true);
			outBoneXforms[46] = componentInChildren2.leftBackTransform;
			outBoneXforms[47] = componentInChildren2.rightBackTransform;
			outBoneXforms[42] = componentInChildren2.chestTransform;
			outBoneXforms[43] = componentInChildren.CurrentBadgeTransform;
			outBoneXforms[44] = componentInChildren.nameTransform;
			outBoneXforms[52] = componentInChildren.huntComputer;
			outBoneXforms[50] = componentInChildren.friendshipBraceletLeftAnchor;
			outBoneXforms[51] = componentInChildren.friendshipBraceletRightAnchor;
			GTHardCodedBones._gInstIds_To_boneXforms[instanceID] = outBoneXforms;
			return true;
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x001ADC74 File Offset: 0x001ABE74
		public static bool TryGetSlotAnchorXforms(VRRig vrRig, out Transform[] outSlotXforms, out string outErrorMsg)
		{
			outErrorMsg = string.Empty;
			if (vrRig == null)
			{
				outErrorMsg = "The VRRig is null.";
				outSlotXforms = Array.Empty<Transform>();
				return false;
			}
			int instanceID = vrRig.GetInstanceID();
			if (GTHardCodedBones._gInstIds_To_slotXforms.TryGetValue(instanceID, out outSlotXforms))
			{
				return true;
			}
			Transform[] array;
			if (!GTHardCodedBones.TryGetBoneXforms(vrRig.mainSkin, out array, out outErrorMsg))
			{
				return false;
			}
			outSlotXforms = new Transform[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				outSlotXforms[i] = array[i];
			}
			BodyDockPositions componentInChildren = vrRig.GetComponentInChildren<BodyDockPositions>(true);
			outSlotXforms[7] = componentInChildren.leftArmTransform;
			outSlotXforms[25] = componentInChildren.rightArmTransform;
			outSlotXforms[8] = componentInChildren.leftHandTransform;
			outSlotXforms[26] = componentInChildren.rightHandTransform;
			GTHardCodedBones._gInstIds_To_slotXforms[instanceID] = outSlotXforms;
			return true;
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x001ADD2C File Offset: 0x001ABF2C
		public static bool TryGetBoneXforms(SkinnedMeshRenderer skinnedMeshRenderer, out Transform[] outBoneXforms, out string outErrorMsg)
		{
			outErrorMsg = string.Empty;
			if (skinnedMeshRenderer == null)
			{
				outErrorMsg = "The SkinnedMeshRenderer was null.";
				outBoneXforms = Array.Empty<Transform>();
				return false;
			}
			int instanceID = skinnedMeshRenderer.GetInstanceID();
			if (GTHardCodedBones._gInstIds_To_boneXforms.TryGetValue(instanceID, out outBoneXforms))
			{
				return true;
			}
			GTHardCodedBones._gMissingBonesReport.Clear();
			Transform[] bones = skinnedMeshRenderer.bones;
			for (int i = 0; i < bones.Length; i++)
			{
				if (bones[i] == null)
				{
					Debug.LogError(string.Format("this should never happen -- skinned mesh bone index {0} is null in component: ", i) + "\"" + skinnedMeshRenderer.GetComponentPath(int.MaxValue) + "\"", skinnedMeshRenderer);
				}
				else if (bones[i].parent == null)
				{
					Debug.LogError(string.Format("unexpected and unhandled scenario -- skinned mesh bone at index {0} has no parent in ", i) + "component: \"" + skinnedMeshRenderer.GetComponentPath(int.MaxValue) + "\"", skinnedMeshRenderer);
				}
				else
				{
					bones[i] = (bones[i].name.EndsWith("_new") ? bones[i].parent : bones[i]);
				}
			}
			outBoneXforms = new Transform[GTHardCodedBones.kBoneNames.Length];
			for (int j = 1; j < GTHardCodedBones.kBoneNames.Length; j++)
			{
				string text = GTHardCodedBones.kBoneNames[j];
				if (!(text == "None") && !text.EndsWith("_end") && !text.Contains("Anchor") && j != 1)
				{
					bool flag = false;
					foreach (Transform transform in bones)
					{
						if (!(transform == null) && !(transform.name != text))
						{
							outBoneXforms[j] = transform;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						GTHardCodedBones._gMissingBonesReport.Add(j);
					}
				}
			}
			for (int l = 1; l < GTHardCodedBones.kBoneNames.Length; l++)
			{
				string text2 = GTHardCodedBones.kBoneNames[l];
				if (text2.EndsWith("_end"))
				{
					string text3 = text2;
					int boneIndex = GTHardCodedBones.GetBoneIndex(text3.Substring(0, text3.Length - 4));
					if (boneIndex < 0)
					{
						GTHardCodedBones._gMissingBonesReport.Add(l);
					}
					else
					{
						Transform transform2 = outBoneXforms[boneIndex];
						if (transform2 == null)
						{
							GTHardCodedBones._gMissingBonesReport.Add(l);
						}
						else
						{
							Transform transform3 = transform2.Find(text2);
							if (transform3 == null)
							{
								GTHardCodedBones._gMissingBonesReport.Add(l);
							}
							else
							{
								outBoneXforms[l] = transform3;
							}
						}
					}
				}
			}
			Transform transform4 = outBoneXforms[2];
			if (transform4 != null && transform4.parent != null)
			{
				outBoneXforms[1] = transform4.parent;
			}
			else
			{
				GTHardCodedBones._gMissingBonesReport.Add(1);
			}
			for (int m = 1; m < GTHardCodedBones.kBoneNames.Length; m++)
			{
				string text4 = GTHardCodedBones.kBoneNames[m];
				if (text4.Contains("Anchor"))
				{
					Transform transform5;
					if (transform4.TryFindByPath("/**/" + text4, out transform5, false))
					{
						outBoneXforms[m] = transform5;
					}
					else
					{
						GameObject gameObject = new GameObject(text4);
						gameObject.transform.SetParent(transform4, false);
						outBoneXforms[m] = gameObject.transform;
					}
				}
			}
			GTHardCodedBones._gInstIds_To_boneXforms[instanceID] = outBoneXforms;
			if (GTHardCodedBones._gMissingBonesReport.Count == 0)
			{
				return true;
			}
			string text5 = "The SkinnedMeshRenderer on \"" + skinnedMeshRenderer.name + "\" did not have these expected bones: ";
			foreach (int num in GTHardCodedBones._gMissingBonesReport)
			{
				text5 = text5 + "\n- " + GTHardCodedBones.kBoneNames[num];
			}
			outErrorMsg = text5;
			return true;
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x000631F7 File Offset: 0x000613F7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetBoneXform(Transform[] boneXforms, string boneName, out Transform boneXform)
		{
			boneXform = boneXforms[GTHardCodedBones.GetBoneIndex(boneName)];
			return boneXform != null;
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x0006320B File Offset: 0x0006140B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetBoneXform(Transform[] boneXforms, GTHardCodedBones.EBone eBone, out Transform boneXform)
		{
			boneXform = boneXforms[GTHardCodedBones.GetBoneIndex(eBone)];
			return boneXform != null;
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x001AE0CC File Offset: 0x001AC2CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetFirstBoneInParents(Transform transform, out GTHardCodedBones.EBone eBone, out Transform boneXform)
		{
			while (transform != null)
			{
				string name = transform.name;
				if (name == "DropZoneAnchor" && transform.parent != null)
				{
					string name2 = transform.parent.name;
					if (name2 == "Slingshot Chest Snap")
					{
						eBone = GTHardCodedBones.EBone.body_AnchorFront_StowSlot;
						boneXform = transform;
						return true;
					}
					if (name2 == "TransferrableItemLeftArm")
					{
						eBone = GTHardCodedBones.EBone.forearm_L;
						boneXform = transform;
						return true;
					}
					if (name2 == "TransferrableItemLeftShoulder")
					{
						eBone = GTHardCodedBones.EBone.body_AnchorBackLeft_StowSlot;
						boneXform = transform;
						return true;
					}
					if (name2 == "TransferrableItemRightShoulder")
					{
						eBone = GTHardCodedBones.EBone.body_AnchorBackRight_StowSlot;
						boneXform = transform;
						return true;
					}
				}
				else
				{
					if (name == "TransferrableItemLeftHand")
					{
						eBone = GTHardCodedBones.EBone.hand_L;
						boneXform = transform;
						return true;
					}
					if (name == "TransferrableItemRightHand")
					{
						eBone = GTHardCodedBones.EBone.hand_R;
						boneXform = transform;
						return true;
					}
				}
				GTHardCodedBones.EBone bone = GTHardCodedBones.GetBone(transform.name);
				if (bone != GTHardCodedBones.EBone.None)
				{
					eBone = bone;
					boneXform = transform;
					return true;
				}
				transform = transform.parent;
			}
			eBone = GTHardCodedBones.EBone.None;
			boneXform = null;
			return false;
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x001AE1BC File Offset: 0x001AC3BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GTHardCodedBones.EBone GetBoneEnumOfCosmeticPosStateFlag(TransferrableObject.PositionState positionState)
		{
			if (positionState <= TransferrableObject.PositionState.OnChest)
			{
				switch (positionState)
				{
				case TransferrableObject.PositionState.None:
					break;
				case TransferrableObject.PositionState.OnLeftArm:
					return GTHardCodedBones.EBone.forearm_L;
				case TransferrableObject.PositionState.OnRightArm:
					return GTHardCodedBones.EBone.forearm_R;
				case TransferrableObject.PositionState.OnLeftArm | TransferrableObject.PositionState.OnRightArm:
				case TransferrableObject.PositionState.OnLeftArm | TransferrableObject.PositionState.InLeftHand:
				case TransferrableObject.PositionState.OnRightArm | TransferrableObject.PositionState.InLeftHand:
				case TransferrableObject.PositionState.OnLeftArm | TransferrableObject.PositionState.OnRightArm | TransferrableObject.PositionState.InLeftHand:
					goto IL_5F;
				case TransferrableObject.PositionState.InLeftHand:
					return GTHardCodedBones.EBone.hand_L;
				case TransferrableObject.PositionState.InRightHand:
					return GTHardCodedBones.EBone.hand_R;
				default:
					if (positionState != TransferrableObject.PositionState.OnChest)
					{
						goto IL_5F;
					}
					return GTHardCodedBones.EBone.body_AnchorFront_StowSlot;
				}
			}
			else
			{
				if (positionState == TransferrableObject.PositionState.OnLeftShoulder)
				{
					return GTHardCodedBones.EBone.body_AnchorBackLeft_StowSlot;
				}
				if (positionState == TransferrableObject.PositionState.OnRightShoulder)
				{
					return GTHardCodedBones.EBone.body_AnchorBackRight_StowSlot;
				}
				if (positionState != TransferrableObject.PositionState.Dropped)
				{
					goto IL_5F;
				}
			}
			return GTHardCodedBones.EBone.None;
			IL_5F:
			throw new ArgumentOutOfRangeException(positionState.ToString());
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x001AE23C File Offset: 0x001AC43C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<GTHardCodedBones.EBone> GetBoneEnumsFromCosmeticBodyDockDropPosFlags(BodyDockPositions.DropPositions enumFlags)
		{
			BodyDockPositions.DropPositions[] values = EnumData<BodyDockPositions.DropPositions>.Shared.Values;
			List<GTHardCodedBones.EBone> list = new List<GTHardCodedBones.EBone>(32);
			foreach (BodyDockPositions.DropPositions dropPositions in values)
			{
				if (dropPositions != BodyDockPositions.DropPositions.All && dropPositions != BodyDockPositions.DropPositions.None && dropPositions != BodyDockPositions.DropPositions.MaxDropPostions && (enumFlags & dropPositions) != BodyDockPositions.DropPositions.None)
				{
					list.Add(GTHardCodedBones._k_bodyDockDropPosition_to_eBone[dropPositions]);
				}
			}
			return list;
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x001AE294 File Offset: 0x001AC494
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<GTHardCodedBones.EBone> GetBoneEnumsFromCosmeticTransferrablePosStateFlags(TransferrableObject.PositionState enumFlags)
		{
			TransferrableObject.PositionState[] values = EnumData<TransferrableObject.PositionState>.Shared.Values;
			List<GTHardCodedBones.EBone> list = new List<GTHardCodedBones.EBone>(32);
			foreach (TransferrableObject.PositionState positionState in values)
			{
				if (positionState != TransferrableObject.PositionState.None && positionState != TransferrableObject.PositionState.Dropped && (enumFlags & positionState) != TransferrableObject.PositionState.None)
				{
					list.Add(GTHardCodedBones._k_transferrablePosState_to_eBone[positionState]);
				}
			}
			return list;
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x0006321F File Offset: 0x0006141F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetTransferrablePosStateFromBoneEnum(GTHardCodedBones.EBone eBone, out TransferrableObject.PositionState outPosState)
		{
			return GTHardCodedBones._k_eBone_to_transferrablePosState.TryGetValue(eBone, out outPosState);
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x001AE2E8 File Offset: 0x001AC4E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Transform GetBoneXformOfCosmeticPosStateFlag(TransferrableObject.PositionState anchorPosState, Transform[] bones)
		{
			if (bones.Length != 53)
			{
				throw new Exception(string.Format("{0}: Supplied bones array length is {1} but requires ", "GTHardCodedBones", bones.Length) + string.Format("{0}.", 53));
			}
			int boneIndex = GTHardCodedBones.GetBoneIndex(GTHardCodedBones.GetBoneEnumOfCosmeticPosStateFlag(anchorPosState));
			if (boneIndex != -1)
			{
				return bones[boneIndex];
			}
			return null;
		}

		// Token: 0x04004FB1 RID: 20401
		public const int kBoneCount = 53;

		// Token: 0x04004FB2 RID: 20402
		public static readonly string[] kBoneNames = new string[]
		{
			"None",
			"rig",
			"body",
			"head",
			"head_end",
			"shoulder.L",
			"upper_arm.L",
			"forearm.L",
			"hand.L",
			"palm.01.L",
			"palm.02.L",
			"thumb.01.L",
			"thumb.02.L",
			"thumb.03.L",
			"thumb.03.L_end",
			"f_index.01.L",
			"f_index.02.L",
			"f_index.03.L",
			"f_index.03.L_end",
			"f_middle.01.L",
			"f_middle.02.L",
			"f_middle.03.L",
			"f_middle.03.L_end",
			"shoulder.R",
			"upper_arm.R",
			"forearm.R",
			"hand.R",
			"palm.01.R",
			"palm.02.R",
			"thumb.01.R",
			"thumb.02.R",
			"thumb.03.R",
			"thumb.03.R_end",
			"f_index.01.R",
			"f_index.02.R",
			"f_index.03.R",
			"f_index.03.R_end",
			"f_middle.01.R",
			"f_middle.02.R",
			"f_middle.03.R",
			"f_middle.03.R_end",
			"body_AnchorTop_Neck",
			"body_AnchorFront_StowSlot",
			"body_AnchorFrontLeft_Badge",
			"body_AnchorFrontRight_NameTag",
			"body_AnchorBack",
			"body_AnchorBackLeft_StowSlot",
			"body_AnchorBackRight_StowSlot",
			"body_AnchorBottom",
			"body_AnchorBackBottom_Tail",
			"hand_L_AnchorBack",
			"hand_R_AnchorBack",
			"hand_L_AnchorFront_GameModeItemSlot"
		};

		// Token: 0x04004FB3 RID: 20403
		private const long kLeftSideMask = 1728432283058160L;

		// Token: 0x04004FB4 RID: 20404
		private const long kRightSideMask = 1769114204897280L;

		// Token: 0x04004FB5 RID: 20405
		private static readonly Dictionary<BodyDockPositions.DropPositions, GTHardCodedBones.EBone> _k_bodyDockDropPosition_to_eBone = new Dictionary<BodyDockPositions.DropPositions, GTHardCodedBones.EBone>
		{
			{
				BodyDockPositions.DropPositions.None,
				GTHardCodedBones.EBone.None
			},
			{
				BodyDockPositions.DropPositions.LeftArm,
				GTHardCodedBones.EBone.forearm_L
			},
			{
				BodyDockPositions.DropPositions.RightArm,
				GTHardCodedBones.EBone.forearm_R
			},
			{
				BodyDockPositions.DropPositions.Chest,
				GTHardCodedBones.EBone.body_AnchorFront_StowSlot
			},
			{
				BodyDockPositions.DropPositions.LeftBack,
				GTHardCodedBones.EBone.body_AnchorBackLeft_StowSlot
			},
			{
				BodyDockPositions.DropPositions.RightBack,
				GTHardCodedBones.EBone.body_AnchorBackRight_StowSlot
			}
		};

		// Token: 0x04004FB6 RID: 20406
		private static readonly Dictionary<TransferrableObject.PositionState, GTHardCodedBones.EBone> _k_transferrablePosState_to_eBone = new Dictionary<TransferrableObject.PositionState, GTHardCodedBones.EBone>
		{
			{
				TransferrableObject.PositionState.None,
				GTHardCodedBones.EBone.None
			},
			{
				TransferrableObject.PositionState.OnLeftArm,
				GTHardCodedBones.EBone.forearm_L
			},
			{
				TransferrableObject.PositionState.OnRightArm,
				GTHardCodedBones.EBone.forearm_R
			},
			{
				TransferrableObject.PositionState.InLeftHand,
				GTHardCodedBones.EBone.hand_L
			},
			{
				TransferrableObject.PositionState.InRightHand,
				GTHardCodedBones.EBone.hand_R
			},
			{
				TransferrableObject.PositionState.OnChest,
				GTHardCodedBones.EBone.body_AnchorFront_StowSlot
			},
			{
				TransferrableObject.PositionState.OnLeftShoulder,
				GTHardCodedBones.EBone.body_AnchorBackLeft_StowSlot
			},
			{
				TransferrableObject.PositionState.OnRightShoulder,
				GTHardCodedBones.EBone.body_AnchorBackRight_StowSlot
			},
			{
				TransferrableObject.PositionState.Dropped,
				GTHardCodedBones.EBone.None
			}
		};

		// Token: 0x04004FB7 RID: 20407
		private static readonly Dictionary<GTHardCodedBones.EBone, TransferrableObject.PositionState> _k_eBone_to_transferrablePosState = new Dictionary<GTHardCodedBones.EBone, TransferrableObject.PositionState>
		{
			{
				GTHardCodedBones.EBone.None,
				TransferrableObject.PositionState.None
			},
			{
				GTHardCodedBones.EBone.forearm_L,
				TransferrableObject.PositionState.OnLeftArm
			},
			{
				GTHardCodedBones.EBone.forearm_R,
				TransferrableObject.PositionState.OnRightArm
			},
			{
				GTHardCodedBones.EBone.hand_L,
				TransferrableObject.PositionState.InLeftHand
			},
			{
				GTHardCodedBones.EBone.hand_R,
				TransferrableObject.PositionState.InRightHand
			},
			{
				GTHardCodedBones.EBone.body_AnchorFront_StowSlot,
				TransferrableObject.PositionState.OnChest
			},
			{
				GTHardCodedBones.EBone.body_AnchorBackLeft_StowSlot,
				TransferrableObject.PositionState.OnLeftShoulder
			},
			{
				GTHardCodedBones.EBone.body_AnchorBackRight_StowSlot,
				TransferrableObject.PositionState.OnRightShoulder
			}
		};

		// Token: 0x04004FB8 RID: 20408
		[OnEnterPlay_Clear]
		[OnExitPlay_Clear]
		private static readonly List<int> _gMissingBonesReport = new List<int>(53);

		// Token: 0x04004FB9 RID: 20409
		[OnEnterPlay_Clear]
		[OnExitPlay_Clear]
		private static readonly Dictionary<int, Transform[]> _gInstIds_To_boneXforms = new Dictionary<int, Transform[]>(20);

		// Token: 0x04004FBA RID: 20410
		[OnEnterPlay_Clear]
		[OnExitPlay_Clear]
		private static readonly Dictionary<int, Transform[]> _gInstIds_To_slotXforms = new Dictionary<int, Transform[]>(20);

		// Token: 0x02000C26 RID: 3110
		public enum EBone
		{
			// Token: 0x04004FBC RID: 20412
			None,
			// Token: 0x04004FBD RID: 20413
			rig,
			// Token: 0x04004FBE RID: 20414
			body,
			// Token: 0x04004FBF RID: 20415
			head,
			// Token: 0x04004FC0 RID: 20416
			head_end,
			// Token: 0x04004FC1 RID: 20417
			shoulder_L,
			// Token: 0x04004FC2 RID: 20418
			upper_arm_L,
			// Token: 0x04004FC3 RID: 20419
			forearm_L,
			// Token: 0x04004FC4 RID: 20420
			hand_L,
			// Token: 0x04004FC5 RID: 20421
			palm_01_L,
			// Token: 0x04004FC6 RID: 20422
			palm_02_L,
			// Token: 0x04004FC7 RID: 20423
			thumb_01_L,
			// Token: 0x04004FC8 RID: 20424
			thumb_02_L,
			// Token: 0x04004FC9 RID: 20425
			thumb_03_L,
			// Token: 0x04004FCA RID: 20426
			thumb_03_L_end,
			// Token: 0x04004FCB RID: 20427
			f_index_01_L,
			// Token: 0x04004FCC RID: 20428
			f_index_02_L,
			// Token: 0x04004FCD RID: 20429
			f_index_03_L,
			// Token: 0x04004FCE RID: 20430
			f_index_03_L_end,
			// Token: 0x04004FCF RID: 20431
			f_middle_01_L,
			// Token: 0x04004FD0 RID: 20432
			f_middle_02_L,
			// Token: 0x04004FD1 RID: 20433
			f_middle_03_L,
			// Token: 0x04004FD2 RID: 20434
			f_middle_03_L_end,
			// Token: 0x04004FD3 RID: 20435
			shoulder_R,
			// Token: 0x04004FD4 RID: 20436
			upper_arm_R,
			// Token: 0x04004FD5 RID: 20437
			forearm_R,
			// Token: 0x04004FD6 RID: 20438
			hand_R,
			// Token: 0x04004FD7 RID: 20439
			palm_01_R,
			// Token: 0x04004FD8 RID: 20440
			palm_02_R,
			// Token: 0x04004FD9 RID: 20441
			thumb_01_R,
			// Token: 0x04004FDA RID: 20442
			thumb_02_R,
			// Token: 0x04004FDB RID: 20443
			thumb_03_R,
			// Token: 0x04004FDC RID: 20444
			thumb_03_R_end,
			// Token: 0x04004FDD RID: 20445
			f_index_01_R,
			// Token: 0x04004FDE RID: 20446
			f_index_02_R,
			// Token: 0x04004FDF RID: 20447
			f_index_03_R,
			// Token: 0x04004FE0 RID: 20448
			f_index_03_R_end,
			// Token: 0x04004FE1 RID: 20449
			f_middle_01_R,
			// Token: 0x04004FE2 RID: 20450
			f_middle_02_R,
			// Token: 0x04004FE3 RID: 20451
			f_middle_03_R,
			// Token: 0x04004FE4 RID: 20452
			f_middle_03_R_end,
			// Token: 0x04004FE5 RID: 20453
			body_AnchorTop_Neck,
			// Token: 0x04004FE6 RID: 20454
			body_AnchorFront_StowSlot,
			// Token: 0x04004FE7 RID: 20455
			body_AnchorFrontLeft_Badge,
			// Token: 0x04004FE8 RID: 20456
			body_AnchorFrontRight_NameTag,
			// Token: 0x04004FE9 RID: 20457
			body_AnchorBack,
			// Token: 0x04004FEA RID: 20458
			body_AnchorBackLeft_StowSlot,
			// Token: 0x04004FEB RID: 20459
			body_AnchorBackRight_StowSlot,
			// Token: 0x04004FEC RID: 20460
			body_AnchorBottom,
			// Token: 0x04004FED RID: 20461
			body_AnchorBackBottom_Tail,
			// Token: 0x04004FEE RID: 20462
			hand_L_AnchorBack,
			// Token: 0x04004FEF RID: 20463
			hand_R_AnchorBack,
			// Token: 0x04004FF0 RID: 20464
			hand_L_AnchorFront_GameModeItemSlot
		}

		// Token: 0x02000C27 RID: 3111
		public enum EStowSlots
		{
			// Token: 0x04004FF2 RID: 20466
			None,
			// Token: 0x04004FF3 RID: 20467
			forearm_L = 7,
			// Token: 0x04004FF4 RID: 20468
			forearm_R = 25,
			// Token: 0x04004FF5 RID: 20469
			body_AnchorFront_Chest = 42,
			// Token: 0x04004FF6 RID: 20470
			body_AnchorBackLeft = 46,
			// Token: 0x04004FF7 RID: 20471
			body_AnchorBackRight
		}

		// Token: 0x02000C28 RID: 3112
		public enum EHandAndStowSlots
		{
			// Token: 0x04004FF9 RID: 20473
			None,
			// Token: 0x04004FFA RID: 20474
			forearm_L = 7,
			// Token: 0x04004FFB RID: 20475
			hand_L,
			// Token: 0x04004FFC RID: 20476
			forearm_R = 25,
			// Token: 0x04004FFD RID: 20477
			hand_R,
			// Token: 0x04004FFE RID: 20478
			body_AnchorFront_Chest = 42,
			// Token: 0x04004FFF RID: 20479
			body_AnchorBackLeft = 46,
			// Token: 0x04005000 RID: 20480
			body_AnchorBackRight
		}

		// Token: 0x02000C29 RID: 3113
		public enum ECosmeticSlots
		{
			// Token: 0x04005002 RID: 20482
			Hat = 4,
			// Token: 0x04005003 RID: 20483
			Badge = 43,
			// Token: 0x04005004 RID: 20484
			Face = 3,
			// Token: 0x04005005 RID: 20485
			ArmLeft = 6,
			// Token: 0x04005006 RID: 20486
			ArmRight = 24,
			// Token: 0x04005007 RID: 20487
			BackLeft = 46,
			// Token: 0x04005008 RID: 20488
			BackRight,
			// Token: 0x04005009 RID: 20489
			HandLeft = 8,
			// Token: 0x0400500A RID: 20490
			HandRight = 26,
			// Token: 0x0400500B RID: 20491
			Chest = 42,
			// Token: 0x0400500C RID: 20492
			Fur = 1,
			// Token: 0x0400500D RID: 20493
			Shirt,
			// Token: 0x0400500E RID: 20494
			Pants = 48,
			// Token: 0x0400500F RID: 20495
			Back = 45,
			// Token: 0x04005010 RID: 20496
			Arms = 2,
			// Token: 0x04005011 RID: 20497
			TagEffect = 0
		}

		// Token: 0x02000C2A RID: 3114
		[Serializable]
		public struct SturdyEBone : ISerializationCallbackReceiver
		{
			// Token: 0x17000819 RID: 2073
			// (get) Token: 0x06004E2A RID: 20010 RVA: 0x0006322D File Offset: 0x0006142D
			// (set) Token: 0x06004E2B RID: 20011 RVA: 0x00063235 File Offset: 0x00061435
			public GTHardCodedBones.EBone Bone
			{
				get
				{
					return this._bone;
				}
				set
				{
					this._bone = value;
					this._boneName = GTHardCodedBones.GetBoneName(this._bone);
				}
			}

			// Token: 0x06004E2C RID: 20012 RVA: 0x0006324F File Offset: 0x0006144F
			public SturdyEBone(GTHardCodedBones.EBone bone)
			{
				this._bone = bone;
				this._boneName = null;
			}

			// Token: 0x06004E2D RID: 20013 RVA: 0x0006325F File Offset: 0x0006145F
			public SturdyEBone(string boneName)
			{
				this._bone = GTHardCodedBones.GetBone(boneName);
				this._boneName = null;
			}

			// Token: 0x06004E2E RID: 20014 RVA: 0x00063274 File Offset: 0x00061474
			public static implicit operator GTHardCodedBones.EBone(GTHardCodedBones.SturdyEBone sturdyBone)
			{
				return sturdyBone.Bone;
			}

			// Token: 0x06004E2F RID: 20015 RVA: 0x0006327D File Offset: 0x0006147D
			public static implicit operator GTHardCodedBones.SturdyEBone(GTHardCodedBones.EBone bone)
			{
				return new GTHardCodedBones.SturdyEBone(bone);
			}

			// Token: 0x06004E30 RID: 20016 RVA: 0x00063274 File Offset: 0x00061474
			public static explicit operator int(GTHardCodedBones.SturdyEBone sturdyBone)
			{
				return (int)sturdyBone.Bone;
			}

			// Token: 0x06004E31 RID: 20017 RVA: 0x00063285 File Offset: 0x00061485
			public override string ToString()
			{
				return this._boneName;
			}

			// Token: 0x06004E32 RID: 20018 RVA: 0x00030607 File Offset: 0x0002E807
			void ISerializationCallbackReceiver.OnBeforeSerialize()
			{
			}

			// Token: 0x06004E33 RID: 20019 RVA: 0x001AE644 File Offset: 0x001AC844
			void ISerializationCallbackReceiver.OnAfterDeserialize()
			{
				if (string.IsNullOrEmpty(this._boneName))
				{
					this._bone = GTHardCodedBones.EBone.None;
					this._boneName = "None";
					return;
				}
				GTHardCodedBones.EBone bone = GTHardCodedBones.GetBone(this._boneName);
				if (bone != GTHardCodedBones.EBone.None)
				{
					this._bone = bone;
				}
			}

			// Token: 0x04005012 RID: 20498
			[SerializeField]
			private GTHardCodedBones.EBone _bone;

			// Token: 0x04005013 RID: 20499
			[SerializeField]
			private string _boneName;
		}
	}
}
