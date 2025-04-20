using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020003C0 RID: 960
[Serializable]
public class VRMap
{
	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06001736 RID: 5942 RVA: 0x0003FC2C File Offset: 0x0003DE2C
	// (set) Token: 0x06001737 RID: 5943 RVA: 0x0003FC39 File Offset: 0x0003DE39
	public Vector3 syncPos
	{
		get
		{
			return this.netSyncPos.CurrentSyncTarget;
		}
		set
		{
			this.netSyncPos.SetNewSyncTarget(value);
		}
	}

	// Token: 0x06001738 RID: 5944 RVA: 0x000C7558 File Offset: 0x000C5758
	public void MapOther(float lerpValue)
	{
		this.rigTarget.localPosition = Vector3.Lerp(this.rigTarget.localPosition, this.syncPos, lerpValue);
		this.rigTarget.localRotation = Quaternion.Lerp(this.rigTarget.localRotation, this.syncRotation, lerpValue);
	}

	// Token: 0x06001739 RID: 5945 RVA: 0x000C75AC File Offset: 0x000C57AC
	public void MapMine(float ratio, Transform playerOffsetTransform)
	{
		Vector3 position = this.rigTarget.position;
		if (this.overrideTarget != null)
		{
			this.rigTarget.rotation = this.overrideTarget.rotation * Quaternion.Euler(this.trackingRotationOffset);
			this.rigTarget.position = this.overrideTarget.position + this.rigTarget.rotation * this.trackingPositionOffset * ratio;
		}
		else
		{
			if (!this.hasInputDevice && ConnectedControllerHandler.Instance.GetValidForXRNode(this.vrTargetNode))
			{
				this.myInputDevice = InputDevices.GetDeviceAtXRNode(this.vrTargetNode);
				this.hasInputDevice = true;
			}
			if (this.hasInputDevice && this.myInputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out this.tempRotation) && this.myInputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out this.tempPosition))
			{
				this.rigTarget.rotation = this.tempRotation * Quaternion.Euler(this.trackingRotationOffset);
				this.rigTarget.position = this.tempPosition + this.rigTarget.rotation * this.trackingPositionOffset * ratio + playerOffsetTransform.position;
				this.rigTarget.RotateAround(playerOffsetTransform.position, Vector3.up, playerOffsetTransform.eulerAngles.y);
			}
		}
		if (this.handholdOverrideTarget != null)
		{
			this.rigTarget.position = Vector3.MoveTowards(position, this.handholdOverrideTarget.position - this.handholdOverrideTargetOffset + this.rigTarget.rotation * this.trackingPositionOffset * ratio, Time.deltaTime * 2f);
		}
	}

	// Token: 0x0600173A RID: 5946 RVA: 0x0003FC47 File Offset: 0x0003DE47
	public Vector3 GetExtrapolatedControllerPosition()
	{
		return this.rigTarget.position - this.rigTarget.rotation * this.trackingPositionOffset * this.rigTarget.lossyScale.x;
	}

	// Token: 0x0600173B RID: 5947 RVA: 0x0003FC84 File Offset: 0x0003DE84
	public virtual void MapOtherFinger(float handSync, float lerpValue)
	{
		this.calcT = handSync;
		this.LerpFinger(lerpValue, true);
	}

	// Token: 0x0600173C RID: 5948 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void MapMyFinger(float lerpValue)
	{
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void LerpFinger(float lerpValue, bool isOther)
	{
	}

	// Token: 0x0400199F RID: 6559
	public XRNode vrTargetNode;

	// Token: 0x040019A0 RID: 6560
	public Transform overrideTarget;

	// Token: 0x040019A1 RID: 6561
	public Transform rigTarget;

	// Token: 0x040019A2 RID: 6562
	public Vector3 trackingPositionOffset;

	// Token: 0x040019A3 RID: 6563
	public Vector3 trackingRotationOffset;

	// Token: 0x040019A4 RID: 6564
	public Transform headTransform;

	// Token: 0x040019A5 RID: 6565
	internal NetworkVector3 netSyncPos = new NetworkVector3();

	// Token: 0x040019A6 RID: 6566
	public Quaternion syncRotation;

	// Token: 0x040019A7 RID: 6567
	public float calcT;

	// Token: 0x040019A8 RID: 6568
	private InputDevice myInputDevice;

	// Token: 0x040019A9 RID: 6569
	private bool hasInputDevice;

	// Token: 0x040019AA RID: 6570
	private Vector3 tempPosition;

	// Token: 0x040019AB RID: 6571
	private Quaternion tempRotation;

	// Token: 0x040019AC RID: 6572
	public int tempInt;

	// Token: 0x040019AD RID: 6573
	public Transform handholdOverrideTarget;

	// Token: 0x040019AE RID: 6574
	public Vector3 handholdOverrideTargetOffset;
}
