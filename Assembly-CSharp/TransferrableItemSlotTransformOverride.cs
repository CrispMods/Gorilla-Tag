using System;
using System.Collections.Generic;
using System.Linq;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000402 RID: 1026
public class TransferrableItemSlotTransformOverride : MonoBehaviour, IGorillaSliceableSimple, ISpawnable
{
	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06001916 RID: 6422 RVA: 0x0007AD7F File Offset: 0x00078F7F
	// (set) Token: 0x06001917 RID: 6423 RVA: 0x0007AD87 File Offset: 0x00078F87
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06001918 RID: 6424 RVA: 0x0007AD90 File Offset: 0x00078F90
	// (set) Token: 0x06001919 RID: 6425 RVA: 0x0007AD98 File Offset: 0x00078F98
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x0600191A RID: 6426 RVA: 0x0007ADA4 File Offset: 0x00078FA4
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.defaultPosition = new SlotTransformOverride
		{
			positionState = TransferrableObject.PositionState.None
		};
		this.lastPosition = TransferrableObject.PositionState.None;
		foreach (SlotTransformOverride slotTransformOverride in this.transformOverridesDeprecated)
		{
			slotTransformOverride.Initialize(this, this.anchor);
		}
	}

	// Token: 0x0600191B RID: 6427 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x0007AE14 File Offset: 0x00079014
	public void AddGripPosition(TransferrableObject.PositionState state, TransferrableObjectGripPosition togp)
	{
		foreach (SlotTransformOverride slotTransformOverride in this.transformOverridesDeprecated)
		{
			if (slotTransformOverride.positionState == state)
			{
				slotTransformOverride.AddSubGrabPoint(togp);
				return;
			}
		}
		SlotTransformOverride slotTransformOverride2 = new SlotTransformOverride
		{
			positionState = state
		};
		this.transformOverridesDeprecated.Add(slotTransformOverride2);
		slotTransformOverride2.AddSubGrabPoint(togp);
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x000158F9 File Offset: 0x00013AF9
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x00015902 File Offset: 0x00013B02
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x0007AE94 File Offset: 0x00079094
	public void SliceUpdate()
	{
		if (this.followingTransferrableObject == null)
		{
			return;
		}
		if (this.followingTransferrableObject.currentState != this.lastPosition)
		{
			SlotTransformOverride slotTransformOverride = this.transformOverridesDeprecated.Find((SlotTransformOverride x) => (x.positionState & this.followingTransferrableObject.currentState) > TransferrableObject.PositionState.None);
			if (slotTransformOverride != null && slotTransformOverride.positionState == TransferrableObject.PositionState.None)
			{
				slotTransformOverride = this.defaultPosition;
			}
		}
		this.lastPosition = this.followingTransferrableObject.currentState;
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x0007AEFE File Offset: 0x000790FE
	private void Awake()
	{
		this.GenerateTransformFromPositionState();
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x0007AF08 File Offset: 0x00079108
	public void GenerateTransformFromPositionState()
	{
		this.transformFromPosition = new Dictionary<TransferrableObject.PositionState, Transform>();
		if (this.transformOverridesDeprecated.Count > 0)
		{
			this.transformFromPosition[TransferrableObject.PositionState.None] = this.transformOverridesDeprecated[0].overrideTransform;
		}
		foreach (TransferrableObject.PositionState positionState in Enum.GetValues(typeof(TransferrableObject.PositionState)).Cast<TransferrableObject.PositionState>())
		{
			if (positionState == TransferrableObject.PositionState.None)
			{
				this.transformFromPosition[positionState] = null;
			}
			else
			{
				Transform value = null;
				foreach (SlotTransformOverride slotTransformOverride in this.transformOverridesDeprecated)
				{
					if ((slotTransformOverride.positionState & positionState) != TransferrableObject.PositionState.None)
					{
						value = slotTransformOverride.overrideTransform;
						break;
					}
				}
				this.transformFromPosition[positionState] = value;
			}
		}
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x0007B004 File Offset: 0x00079204
	[CanBeNull]
	public Transform GetTransformFromPositionState(TransferrableObject.PositionState currentState)
	{
		if (this.transformFromPosition == null)
		{
			this.GenerateTransformFromPositionState();
		}
		return this.transformFromPosition[currentState];
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x0007B020 File Offset: 0x00079220
	public bool GetTransformFromPositionState(TransferrableObject.PositionState currentState, AdvancedItemState advancedItemState, Transform targetDockXf, out Matrix4x4 matrix4X4)
	{
		if (currentState != TransferrableObject.PositionState.None)
		{
			foreach (SlotTransformOverride slotTransformOverride in this.transformOverridesDeprecated)
			{
				if ((slotTransformOverride.positionState & currentState) != TransferrableObject.PositionState.None)
				{
					if (!slotTransformOverride.useAdvancedGrab)
					{
						matrix4X4 = slotTransformOverride.overrideTransformMatrix;
						return true;
					}
					if (advancedItemState.index >= slotTransformOverride.multiPoints.Count)
					{
						matrix4X4 = slotTransformOverride.overrideTransformMatrix;
						return true;
					}
					SubGrabPoint subGrabPoint = slotTransformOverride.multiPoints[advancedItemState.index];
					matrix4X4 = subGrabPoint.GetTransformFromPositionState(advancedItemState, slotTransformOverride, targetDockXf);
					return true;
				}
			}
			matrix4X4 = Matrix4x4.identity;
			return false;
		}
		if (this.transformOverridesDeprecated.Count > 0)
		{
			matrix4X4 = this.transformOverridesDeprecated[0].overrideTransformMatrix;
			return true;
		}
		matrix4X4 = Matrix4x4.identity;
		return false;
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x0007B124 File Offset: 0x00079324
	public AdvancedItemState GetAdvancedItemStateFromHand(TransferrableObject.PositionState currentState, Transform handTransform, Transform targetDock)
	{
		foreach (SlotTransformOverride slotTransformOverride in this.transformOverridesDeprecated)
		{
			if ((slotTransformOverride.positionState & currentState) != TransferrableObject.PositionState.None && slotTransformOverride.multiPoints.Count != 0)
			{
				SubGrabPoint subGrabPoint = slotTransformOverride.multiPoints[0];
				float num = float.PositiveInfinity;
				int index = -1;
				for (int i = 0; i < slotTransformOverride.multiPoints.Count; i++)
				{
					SubGrabPoint subGrabPoint2 = slotTransformOverride.multiPoints[i];
					if (!(subGrabPoint2.gripPoint == null))
					{
						float num2 = subGrabPoint2.EvaluateScore(base.transform, handTransform, targetDock);
						if (num2 < num)
						{
							subGrabPoint = subGrabPoint2;
							num = num2;
							index = i;
						}
					}
				}
				AdvancedItemState advancedItemStateFromHand = subGrabPoint.GetAdvancedItemStateFromHand(base.transform, handTransform, targetDock, slotTransformOverride);
				advancedItemStateFromHand.index = index;
				return advancedItemStateFromHand;
			}
		}
		return new AdvancedItemState();
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x0007B224 File Offset: 0x00079424
	public void Edit()
	{
		if (TransferrableItemSlotTransformOverride.OnBringUpWindow != null)
		{
			TransferrableItemSlotTransformOverride.OnBringUpWindow(base.GetComponent<TransferrableObject>());
		}
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001BFB RID: 7163
	[FormerlySerializedAs("transformOverridesList")]
	public List<SlotTransformOverride> transformOverridesDeprecated;

	// Token: 0x04001BFC RID: 7164
	[SerializeReference]
	public List<SlotTransformOverride> transformOverrides;

	// Token: 0x04001BFD RID: 7165
	private TransferrableObject.PositionState lastPosition;

	// Token: 0x04001BFE RID: 7166
	[Tooltip("(2024-08-20 MattO) For cosmetics this is almost always assigned to the TransferrableObject component in the same prefab and almost always belonging to the same gameobject as this Component.")]
	public TransferrableObject followingTransferrableObject;

	// Token: 0x04001BFF RID: 7167
	[Tooltip("(2024-08-20 MattO) This is filled in automatically by the cosmetic spawner.")]
	public SlotTransformOverride defaultPosition;

	// Token: 0x04001C00 RID: 7168
	[Obsolete("(2024-08-2024) This used to be assigned to `defaultPosition.overrideTransform` before, but was there ever an instance where it wasn't null? Keeping it serialized just in case there is a reason for it.")]
	public Transform defaultTransform;

	// Token: 0x04001C01 RID: 7169
	public Transform anchor;

	// Token: 0x04001C04 RID: 7172
	public Dictionary<TransferrableObject.PositionState, Transform> transformFromPosition;

	// Token: 0x04001C05 RID: 7173
	public static Action<TransferrableObject> OnBringUpWindow;
}
