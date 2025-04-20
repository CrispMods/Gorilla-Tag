using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class SnowballMaker : MonoBehaviour
{
	// Token: 0x17000069 RID: 105
	// (get) Token: 0x0600056F RID: 1391 RVA: 0x00033FEC File Offset: 0x000321EC
	// (set) Token: 0x06000570 RID: 1392 RVA: 0x00033FF3 File Offset: 0x000321F3
	public static SnowballMaker leftHandInstance { get; private set; }

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000571 RID: 1393 RVA: 0x00033FFB File Offset: 0x000321FB
	// (set) Token: 0x06000572 RID: 1394 RVA: 0x00034002 File Offset: 0x00032202
	public static SnowballMaker rightHandInstance { get; private set; }

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000573 RID: 1395 RVA: 0x0003400A File Offset: 0x0003220A
	// (set) Token: 0x06000574 RID: 1396 RVA: 0x00034012 File Offset: 0x00032212
	public SnowballThrowable[] snowballs { get; private set; }

	// Token: 0x06000575 RID: 1397 RVA: 0x00081F60 File Offset: 0x00080160
	private void Awake()
	{
		if (this.isLeftHand)
		{
			if (SnowballMaker.leftHandInstance == null)
			{
				SnowballMaker.leftHandInstance = this;
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else if (SnowballMaker.rightHandInstance == null)
		{
			SnowballMaker.rightHandInstance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (this.handTransform == null)
		{
			this.handTransform = base.transform;
		}
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0003401B File Offset: 0x0003221B
	internal void SetupThrowables(SnowballThrowable[] newThrowables)
	{
		this.snowballs = newThrowables;
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x00081FD4 File Offset: 0x000801D4
	protected void LateUpdate()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		if (!CosmeticsV2Spawner_Dirty.allPartsInstantiated)
		{
			return;
		}
		if (this.snowballs == null)
		{
			return;
		}
		if (BuilderPieceInteractor.instance != null && BuilderPieceInteractor.instance.BlockSnowballCreation())
		{
			return;
		}
		if (!GTPlayer.hasInstance || !EquipmentInteractor.hasInstance || !GorillaTagger.hasInstance || !GorillaTagger.Instance.offlineVRRig || this.snowballs.Length == 0)
		{
			return;
		}
		GTPlayer instance = GTPlayer.Instance;
		int num = this.isLeftHand ? instance.leftHandMaterialTouchIndex : instance.rightHandMaterialTouchIndex;
		if (num == 0)
		{
			if (Time.time > this.lastGroundContactTime + this.snowballCreationCooldownTime)
			{
				this.requiresFreshMaterialContact = false;
			}
			return;
		}
		this.lastGroundContactTime = Time.time;
		EquipmentInteractor instance2 = EquipmentInteractor.instance;
		bool flag = (this.isLeftHand ? instance2.leftHandHeldEquipment : instance2.rightHandHeldEquipment) != null;
		bool flag2 = this.isLeftHand ? instance2.isLeftGrabbing : instance2.isRightGrabbing;
		bool flag3 = false;
		if (flag2 && !this.requiresFreshMaterialContact)
		{
			int num2 = -1;
			for (int i = 0; i < this.snowballs.Length; i++)
			{
				if (this.snowballs[i].gameObject.activeSelf)
				{
					num2 = i;
					break;
				}
			}
			SnowballThrowable snowballThrowable = (num2 > -1) ? this.snowballs[num2] : null;
			GrowingSnowballThrowable growingSnowballThrowable = snowballThrowable as GrowingSnowballThrowable;
			bool flag4 = this.isLeftHand ? (!ConnectedControllerHandler.Instance.RightValid) : (!ConnectedControllerHandler.Instance.LeftValid);
			if (growingSnowballThrowable != null && (!GrowingSnowballThrowable.twoHandedSnowballGrowing || flag4 || flag3))
			{
				if (snowballThrowable.matDataIndexes.Contains(num))
				{
					growingSnowballThrowable.IncreaseSize(1);
					GorillaTagger.Instance.StartVibration(this.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
					this.requiresFreshMaterialContact = true;
					return;
				}
			}
			else if (!flag)
			{
				foreach (SnowballThrowable snowballThrowable2 in this.snowballs)
				{
					Transform transform = snowballThrowable2.transform;
					if (snowballThrowable2.matDataIndexes.Contains(num))
					{
						Transform transform2 = this.handTransform;
						snowballThrowable2.SetSnowballActiveLocal(true);
						snowballThrowable2.velocityEstimator = this.velocityEstimator;
						transform.position = transform2.position;
						transform.rotation = transform2.rotation;
						GorillaTagger.Instance.StartVibration(this.isLeftHand, GorillaTagger.Instance.tapHapticStrength * 0.5f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
						this.requiresFreshMaterialContact = true;
						return;
					}
				}
			}
		}
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00082274 File Offset: 0x00080474
	public bool TryCreateSnowball(int materialIndex, out SnowballThrowable result)
	{
		foreach (SnowballThrowable snowballThrowable in this.snowballs)
		{
			if (snowballThrowable.matDataIndexes.Contains(materialIndex))
			{
				Transform transform = snowballThrowable.transform;
				Transform transform2 = this.handTransform;
				snowballThrowable.SetSnowballActiveLocal(true);
				snowballThrowable.velocityEstimator = this.velocityEstimator;
				transform.position = transform2.position;
				transform.rotation = transform2.rotation;
				GorillaTagger.Instance.StartVibration(this.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
				result = snowballThrowable;
				return true;
			}
		}
		result = null;
		return false;
	}

	// Token: 0x04000646 RID: 1606
	public bool isLeftHand;

	// Token: 0x04000648 RID: 1608
	public GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04000649 RID: 1609
	private float snowballCreationCooldownTime = 0.1f;

	// Token: 0x0400064A RID: 1610
	private float lastGroundContactTime;

	// Token: 0x0400064B RID: 1611
	private bool requiresFreshMaterialContact;

	// Token: 0x0400064C RID: 1612
	[SerializeField]
	private Transform handTransform;
}
