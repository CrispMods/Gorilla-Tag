﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BFA RID: 3066
	public static class GTHardCodedBones
	{
		// Token: 0x06004CCF RID: 19663 RVA: 0x00061779 File Offset: 0x0005F979
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void HandleRuntimeInitialize_OnBeforeSceneLoad()
		{
			VRRigCache.OnPostInitialize += GTHardCodedBones.HandleVRRigCache_OnPostInitialize;
		}

		// Token: 0x06004CD0 RID: 19664 RVA: 0x0006178C File Offset: 0x0005F98C
		private static void HandleVRRigCache_OnPostInitialize()
		{
			VRRigCache.OnPostInitialize -= GTHardCodedBones.HandleVRRigCache_OnPostInitialize;
			GTHardCodedBones.HandleVRRigCache_OnPostSpawnRig();
			VRRigCache.OnPostSpawnRig += GTHardCodedBones.HandleVRRigCache_OnPostSpawnRig;
		}

		// Token: 0x06004CD1 RID: 19665 RVA: 0x001A6A8C File Offset: 0x001A4C8C
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

		// Token: 0x06004CD2 RID: 19666 RVA: 0x00037764 File Offset: 0x00035964
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetBoneIndex(GTHardCodedBones.EBone bone)
		{
			return (int)bone;
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x001A6B04 File Offset: 0x001A4D04
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

		// Token: 0x06004CD4 RID: 19668 RVA: 0x001A6B38 File Offset: 0x001A4D38
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

		// Token: 0x06004CD5 RID: 19669 RVA: 0x000617B5 File Offset: 0x0005F9B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GTHardCodedBones.EBone GetBone(string name)
		{
			return (GTHardCodedBones.EBone)GTHardCodedBones.GetBoneIndex(name);
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x001A6B70 File Offset: 0x001A4D70
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

		// Token: 0x06004CD7 RID: 19671 RVA: 0x000617BD File Offset: 0x0005F9BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetBoneName(int boneIndex)
		{
			return GTHardCodedBones.kBoneNames[boneIndex];
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x000617C6 File Offset: 0x0005F9C6
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

		// Token: 0x06004CD9 RID: 19673 RVA: 0x000617E9 File Offset: 0x0005F9E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetBoneName(GTHardCodedBones.EBone bone)
		{
			return GTHardCodedBones.GetBoneName((int)bone);
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x000617F1 File Offset: 0x0005F9F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetBoneName(GTHardCodedBones.EBone bone, out string out_name)
		{
			return GTHardCodedBones.TryGetBoneName((int)bone, out out_name);
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x001A6B90 File Offset: 0x001A4D90
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

		// Token: 0x06004CDC RID: 19676 RVA: 0x000617FA File Offset: 0x0005F9FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long GetBoneBitFlag(GTHardCodedBones.EBone bone)
		{
			if (bone == GTHardCodedBones.EBone.None)
			{
				return 0L;
			}
			return 1L << bone - GTHardCodedBones.EBone.rig;
		}

		// Token: 0x06004CDD RID: 19677 RVA: 0x0006180B File Offset: 0x0005FA0B
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

		// Token: 0x06004CDE RID: 19678 RVA: 0x001A6BDC File Offset: 0x001A4DDC
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

		// Token: 0x06004CDF RID: 19679 RVA: 0x001A6CA8 File Offset: 0x001A4EA8
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

		// Token: 0x06004CE0 RID: 19680 RVA: 0x001A6D60 File Offset: 0x001A4F60
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

		// Token: 0x06004CE1 RID: 19681 RVA: 0x00061836 File Offset: 0x0005FA36
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetBoneXform(Transform[] boneXforms, string boneName, out Transform boneXform)
		{
			boneXform = boneXforms[GTHardCodedBones.GetBoneIndex(boneName)];
			return boneXform != null;
		}

		// Token: 0x06004CE2 RID: 19682 RVA: 0x0006184A File Offset: 0x0005FA4A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetBoneXform(Transform[] boneXforms, GTHardCodedBones.EBone eBone, out Transform boneXform)
		{
			boneXform = boneXforms[GTHardCodedBones.GetBoneIndex(eBone)];
			return boneXform != null;
		}

		// Token: 0x06004CE3 RID: 19683 RVA: 0x001A7100 File Offset: 0x001A5300
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

		// Token: 0x06004CE4 RID: 19684 RVA: 0x001A71F0 File Offset: 0x001A53F0
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

		// Token: 0x06004CE5 RID: 19685 RVA: 0x001A7270 File Offset: 0x001A5470
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

		// Token: 0x06004CE6 RID: 19686 RVA: 0x001A72C8 File Offset: 0x001A54C8
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

		// Token: 0x06004CE7 RID: 19687 RVA: 0x0006185E File Offset: 0x0005FA5E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetTransferrablePosStateFromBoneEnum(GTHardCodedBones.EBone eBone, out TransferrableObject.PositionState outPosState)
		{
			return GTHardCodedBones._k_eBone_to_transferrablePosState.TryGetValue(eBone, out outPosState);
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x001A731C File Offset: 0x001A551C
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

		// Token: 0x04004ECD RID: 20173
		public const int kBoneCount = 53;

		// Token: 0x04004ECE RID: 20174
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

		// Token: 0x04004ECF RID: 20175
		private const long kLeftSideMask = 1728432283058160L;

		// Token: 0x04004ED0 RID: 20176
		private const long kRightSideMask = 1769114204897280L;

		// Token: 0x04004ED1 RID: 20177
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

		// Token: 0x04004ED2 RID: 20178
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

		// Token: 0x04004ED3 RID: 20179
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

		// Token: 0x04004ED4 RID: 20180
		[OnEnterPlay_Clear]
		[OnExitPlay_Clear]
		private static readonly List<int> _gMissingBonesReport = new List<int>(53);

		// Token: 0x04004ED5 RID: 20181
		[OnEnterPlay_Clear]
		[OnExitPlay_Clear]
		private static readonly Dictionary<int, Transform[]> _gInstIds_To_boneXforms = new Dictionary<int, Transform[]>(20);

		// Token: 0x04004ED6 RID: 20182
		[OnEnterPlay_Clear]
		[OnExitPlay_Clear]
		private static readonly Dictionary<int, Transform[]> _gInstIds_To_slotXforms = new Dictionary<int, Transform[]>(20);

		// Token: 0x02000BFB RID: 3067
		public enum EBone
		{
			// Token: 0x04004ED8 RID: 20184
			None,
			// Token: 0x04004ED9 RID: 20185
			rig,
			// Token: 0x04004EDA RID: 20186
			body,
			// Token: 0x04004EDB RID: 20187
			head,
			// Token: 0x04004EDC RID: 20188
			head_end,
			// Token: 0x04004EDD RID: 20189
			shoulder_L,
			// Token: 0x04004EDE RID: 20190
			upper_arm_L,
			// Token: 0x04004EDF RID: 20191
			forearm_L,
			// Token: 0x04004EE0 RID: 20192
			hand_L,
			// Token: 0x04004EE1 RID: 20193
			palm_01_L,
			// Token: 0x04004EE2 RID: 20194
			palm_02_L,
			// Token: 0x04004EE3 RID: 20195
			thumb_01_L,
			// Token: 0x04004EE4 RID: 20196
			thumb_02_L,
			// Token: 0x04004EE5 RID: 20197
			thumb_03_L,
			// Token: 0x04004EE6 RID: 20198
			thumb_03_L_end,
			// Token: 0x04004EE7 RID: 20199
			f_index_01_L,
			// Token: 0x04004EE8 RID: 20200
			f_index_02_L,
			// Token: 0x04004EE9 RID: 20201
			f_index_03_L,
			// Token: 0x04004EEA RID: 20202
			f_index_03_L_end,
			// Token: 0x04004EEB RID: 20203
			f_middle_01_L,
			// Token: 0x04004EEC RID: 20204
			f_middle_02_L,
			// Token: 0x04004EED RID: 20205
			f_middle_03_L,
			// Token: 0x04004EEE RID: 20206
			f_middle_03_L_end,
			// Token: 0x04004EEF RID: 20207
			shoulder_R,
			// Token: 0x04004EF0 RID: 20208
			upper_arm_R,
			// Token: 0x04004EF1 RID: 20209
			forearm_R,
			// Token: 0x04004EF2 RID: 20210
			hand_R,
			// Token: 0x04004EF3 RID: 20211
			palm_01_R,
			// Token: 0x04004EF4 RID: 20212
			palm_02_R,
			// Token: 0x04004EF5 RID: 20213
			thumb_01_R,
			// Token: 0x04004EF6 RID: 20214
			thumb_02_R,
			// Token: 0x04004EF7 RID: 20215
			thumb_03_R,
			// Token: 0x04004EF8 RID: 20216
			thumb_03_R_end,
			// Token: 0x04004EF9 RID: 20217
			f_index_01_R,
			// Token: 0x04004EFA RID: 20218
			f_index_02_R,
			// Token: 0x04004EFB RID: 20219
			f_index_03_R,
			// Token: 0x04004EFC RID: 20220
			f_index_03_R_end,
			// Token: 0x04004EFD RID: 20221
			f_middle_01_R,
			// Token: 0x04004EFE RID: 20222
			f_middle_02_R,
			// Token: 0x04004EFF RID: 20223
			f_middle_03_R,
			// Token: 0x04004F00 RID: 20224
			f_middle_03_R_end,
			// Token: 0x04004F01 RID: 20225
			body_AnchorTop_Neck,
			// Token: 0x04004F02 RID: 20226
			body_AnchorFront_StowSlot,
			// Token: 0x04004F03 RID: 20227
			body_AnchorFrontLeft_Badge,
			// Token: 0x04004F04 RID: 20228
			body_AnchorFrontRight_NameTag,
			// Token: 0x04004F05 RID: 20229
			body_AnchorBack,
			// Token: 0x04004F06 RID: 20230
			body_AnchorBackLeft_StowSlot,
			// Token: 0x04004F07 RID: 20231
			body_AnchorBackRight_StowSlot,
			// Token: 0x04004F08 RID: 20232
			body_AnchorBottom,
			// Token: 0x04004F09 RID: 20233
			body_AnchorBackBottom_Tail,
			// Token: 0x04004F0A RID: 20234
			hand_L_AnchorBack,
			// Token: 0x04004F0B RID: 20235
			hand_R_AnchorBack,
			// Token: 0x04004F0C RID: 20236
			hand_L_AnchorFront_GameModeItemSlot
		}

		// Token: 0x02000BFC RID: 3068
		public enum EStowSlots
		{
			// Token: 0x04004F0E RID: 20238
			None,
			// Token: 0x04004F0F RID: 20239
			forearm_L = 7,
			// Token: 0x04004F10 RID: 20240
			forearm_R = 25,
			// Token: 0x04004F11 RID: 20241
			body_AnchorFront_Chest = 42,
			// Token: 0x04004F12 RID: 20242
			body_AnchorBackLeft = 46,
			// Token: 0x04004F13 RID: 20243
			body_AnchorBackRight
		}

		// Token: 0x02000BFD RID: 3069
		public enum EHandAndStowSlots
		{
			// Token: 0x04004F15 RID: 20245
			None,
			// Token: 0x04004F16 RID: 20246
			forearm_L = 7,
			// Token: 0x04004F17 RID: 20247
			hand_L,
			// Token: 0x04004F18 RID: 20248
			forearm_R = 25,
			// Token: 0x04004F19 RID: 20249
			hand_R,
			// Token: 0x04004F1A RID: 20250
			body_AnchorFront_Chest = 42,
			// Token: 0x04004F1B RID: 20251
			body_AnchorBackLeft = 46,
			// Token: 0x04004F1C RID: 20252
			body_AnchorBackRight
		}

		// Token: 0x02000BFE RID: 3070
		public enum ECosmeticSlots
		{
			// Token: 0x04004F1E RID: 20254
			Hat = 4,
			// Token: 0x04004F1F RID: 20255
			Badge = 43,
			// Token: 0x04004F20 RID: 20256
			Face = 3,
			// Token: 0x04004F21 RID: 20257
			ArmLeft = 6,
			// Token: 0x04004F22 RID: 20258
			ArmRight = 24,
			// Token: 0x04004F23 RID: 20259
			BackLeft = 46,
			// Token: 0x04004F24 RID: 20260
			BackRight,
			// Token: 0x04004F25 RID: 20261
			HandLeft = 8,
			// Token: 0x04004F26 RID: 20262
			HandRight = 26,
			// Token: 0x04004F27 RID: 20263
			Chest = 42,
			// Token: 0x04004F28 RID: 20264
			Fur = 1,
			// Token: 0x04004F29 RID: 20265
			Shirt,
			// Token: 0x04004F2A RID: 20266
			Pants = 48,
			// Token: 0x04004F2B RID: 20267
			Back = 45,
			// Token: 0x04004F2C RID: 20268
			Arms = 2,
			// Token: 0x04004F2D RID: 20269
			TagEffect = 0
		}

		// Token: 0x02000BFF RID: 3071
		[Serializable]
		public struct SturdyEBone : ISerializationCallbackReceiver
		{
			// Token: 0x170007FC RID: 2044
			// (get) Token: 0x06004CEA RID: 19690 RVA: 0x0006186C File Offset: 0x0005FA6C
			// (set) Token: 0x06004CEB RID: 19691 RVA: 0x00061874 File Offset: 0x0005FA74
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

			// Token: 0x06004CEC RID: 19692 RVA: 0x0006188E File Offset: 0x0005FA8E
			public SturdyEBone(GTHardCodedBones.EBone bone)
			{
				this._bone = bone;
				this._boneName = null;
			}

			// Token: 0x06004CED RID: 19693 RVA: 0x0006189E File Offset: 0x0005FA9E
			public SturdyEBone(string boneName)
			{
				this._bone = GTHardCodedBones.GetBone(boneName);
				this._boneName = null;
			}

			// Token: 0x06004CEE RID: 19694 RVA: 0x000618B3 File Offset: 0x0005FAB3
			public static implicit operator GTHardCodedBones.EBone(GTHardCodedBones.SturdyEBone sturdyBone)
			{
				return sturdyBone.Bone;
			}

			// Token: 0x06004CEF RID: 19695 RVA: 0x000618BC File Offset: 0x0005FABC
			public static implicit operator GTHardCodedBones.SturdyEBone(GTHardCodedBones.EBone bone)
			{
				return new GTHardCodedBones.SturdyEBone(bone);
			}

			// Token: 0x06004CF0 RID: 19696 RVA: 0x000618B3 File Offset: 0x0005FAB3
			public static explicit operator int(GTHardCodedBones.SturdyEBone sturdyBone)
			{
				return (int)sturdyBone.Bone;
			}

			// Token: 0x06004CF1 RID: 19697 RVA: 0x000618C4 File Offset: 0x0005FAC4
			public override string ToString()
			{
				return this._boneName;
			}

			// Token: 0x06004CF2 RID: 19698 RVA: 0x0002F75F File Offset: 0x0002D95F
			void ISerializationCallbackReceiver.OnBeforeSerialize()
			{
			}

			// Token: 0x06004CF3 RID: 19699 RVA: 0x001A7678 File Offset: 0x001A5878
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

			// Token: 0x04004F2E RID: 20270
			[SerializeField]
			private GTHardCodedBones.EBone _bone;

			// Token: 0x04004F2F RID: 20271
			[SerializeField]
			private string _boneName;
		}
	}
}
