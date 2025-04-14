using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020003B5 RID: 949
[Serializable]
public class VRMap
{
	// Token: 0x1700028A RID: 650
	// (get) Token: 0x060016E9 RID: 5865 RVA: 0x000700D4 File Offset: 0x0006E2D4
	// (set) Token: 0x060016EA RID: 5866 RVA: 0x000700E1 File Offset: 0x0006E2E1
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

	// Token: 0x060016EB RID: 5867 RVA: 0x000700F0 File Offset: 0x0006E2F0
	public void MapOther(float lerpValue)
	{
		this.rigTarget.localPosition = Vector3.Lerp(this.rigTarget.localPosition, this.syncPos, lerpValue);
		this.rigTarget.localRotation = Quaternion.Lerp(this.rigTarget.localRotation, this.syncRotation, lerpValue);
	}

	// Token: 0x060016EC RID: 5868 RVA: 0x00070144 File Offset: 0x0006E344
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

	// Token: 0x060016ED RID: 5869 RVA: 0x0007031D File Offset: 0x0006E51D
	public Vector3 GetExtrapolatedControllerPosition()
	{
		return this.rigTarget.position - this.rigTarget.rotation * this.trackingPositionOffset * this.rigTarget.lossyScale.x;
	}

	// Token: 0x060016EE RID: 5870 RVA: 0x0007035A File Offset: 0x0006E55A
	public virtual void MapOtherFinger(float handSync, float lerpValue)
	{
		this.calcT = handSync;
		this.LerpFinger(lerpValue, true);
	}

	// Token: 0x060016EF RID: 5871 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void MapMyFinger(float lerpValue)
	{
	}

	// Token: 0x060016F0 RID: 5872 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void LerpFinger(float lerpValue, bool isOther)
	{
	}

	// Token: 0x04001956 RID: 6486
	public XRNode vrTargetNode;

	// Token: 0x04001957 RID: 6487
	public Transform overrideTarget;

	// Token: 0x04001958 RID: 6488
	public Transform rigTarget;

	// Token: 0x04001959 RID: 6489
	public Vector3 trackingPositionOffset;

	// Token: 0x0400195A RID: 6490
	public Vector3 trackingRotationOffset;

	// Token: 0x0400195B RID: 6491
	public Transform headTransform;

	// Token: 0x0400195C RID: 6492
	internal NetworkVector3 netSyncPos = new NetworkVector3();

	// Token: 0x0400195D RID: 6493
	public Quaternion syncRotation;

	// Token: 0x0400195E RID: 6494
	public float calcT;

	// Token: 0x0400195F RID: 6495
	private InputDevice myInputDevice;

	// Token: 0x04001960 RID: 6496
	private bool hasInputDevice;

	// Token: 0x04001961 RID: 6497
	private Vector3 tempPosition;

	// Token: 0x04001962 RID: 6498
	private Quaternion tempRotation;

	// Token: 0x04001963 RID: 6499
	public int tempInt;

	// Token: 0x04001964 RID: 6500
	public Transform handholdOverrideTarget;

	// Token: 0x04001965 RID: 6501
	public Vector3 handholdOverrideTargetOffset;
}
