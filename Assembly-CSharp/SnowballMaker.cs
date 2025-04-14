using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020000C9 RID: 201
public class SnowballMaker : MonoBehaviour
{
	// Token: 0x17000064 RID: 100
	// (get) Token: 0x0600052F RID: 1327 RVA: 0x0001EDB7 File Offset: 0x0001CFB7
	// (set) Token: 0x06000530 RID: 1328 RVA: 0x0001EDBE File Offset: 0x0001CFBE
	public static SnowballMaker leftHandInstance { get; private set; }

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000531 RID: 1329 RVA: 0x0001EDC6 File Offset: 0x0001CFC6
	// (set) Token: 0x06000532 RID: 1330 RVA: 0x0001EDCD File Offset: 0x0001CFCD
	public static SnowballMaker rightHandInstance { get; private set; }

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06000533 RID: 1331 RVA: 0x0001EDD5 File Offset: 0x0001CFD5
	// (set) Token: 0x06000534 RID: 1332 RVA: 0x0001EDDD File Offset: 0x0001CFDD
	public SnowballThrowable[] snowballs { get; private set; }

	// Token: 0x06000535 RID: 1333 RVA: 0x0001EDE8 File Offset: 0x0001CFE8
	private void Awake()
	{
		if (this.isLeftHand)
		{
			if (SnowballMaker.leftHandInstance == null)
			{
				SnowballMaker.leftHandInstance = this;
				return;
			}
			Object.Destroy(base.gameObject);
			return;
		}
		else
		{
			if (SnowballMaker.rightHandInstance == null)
			{
				SnowballMaker.rightHandInstance = this;
				return;
			}
			Object.Destroy(base.gameObject);
			return;
		}
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0001EE3C File Offset: 0x0001D03C
	internal void SetupThrowables(SnowballThrowable[] newThrowables)
	{
		this.snowballs = newThrowables;
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0001EE48 File Offset: 0x0001D048
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
						Transform transform2 = base.transform;
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

	// Token: 0x06000538 RID: 1336 RVA: 0x0001F0E8 File Offset: 0x0001D2E8
	public bool TryCreateSnowball(int materialIndex, out SnowballThrowable result)
	{
		foreach (SnowballThrowable snowballThrowable in this.snowballs)
		{
			if (snowballThrowable.matDataIndexes.Contains(materialIndex))
			{
				Transform transform = snowballThrowable.transform;
				Transform transform2 = base.transform;
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

	// Token: 0x04000606 RID: 1542
	public bool isLeftHand;

	// Token: 0x04000608 RID: 1544
	public GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04000609 RID: 1545
	private float snowballCreationCooldownTime = 0.1f;

	// Token: 0x0400060A RID: 1546
	private float lastGroundContactTime;

	// Token: 0x0400060B RID: 1547
	private bool requiresFreshMaterialContact;
}
