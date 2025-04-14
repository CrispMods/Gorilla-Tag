using System;
using GorillaExtensions;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200039C RID: 924
public class TransferrableObjectHoldablePart_Slide : TransferrableObjectHoldablePart
{
	// Token: 0x06001589 RID: 5513 RVA: 0x00069248 File Offset: 0x00067448
	protected override void UpdateHeld(VRRig rig, bool isHeldLeftHand)
	{
		int num = isHeldLeftHand ? 0 : 1;
		GTPlayer instance = GTPlayer.Instance;
		if (!rig.isOfflineVRRig)
		{
			Vector3 b = (isHeldLeftHand ? instance.leftHandOffset : instance.rightHandOffset) * rig.scaleFactor;
			VRMap vrmap = isHeldLeftHand ? rig.leftHand : rig.rightHand;
			this._snapToLine.target.position = vrmap.GetExtrapolatedControllerPosition() - b;
			return;
		}
		Transform transform = (num == 0) ? instance.leftControllerTransform : instance.rightControllerTransform;
		Vector3 position = transform.position;
		Vector3 vector = this._snapToLine.GetSnappedPoint(position);
		vector = Vector3.Lerp(position, vector, 0.25f);
		if (this._maxHandSnapDistance > 0f && (transform.position - vector).IsLongerThan(this._maxHandSnapDistance))
		{
			this.OnRelease(null, isHeldLeftHand ? EquipmentInteractor.instance.leftHand : EquipmentInteractor.instance.rightHand);
			return;
		}
		transform.position = vector;
		this._snapToLine.target.position = vector;
	}

	// Token: 0x040017DC RID: 6108
	[SerializeField]
	private float _maxHandSnapDistance;

	// Token: 0x040017DD RID: 6109
	[SerializeField]
	private SnapXformToLine _snapToLine;

	// Token: 0x040017DE RID: 6110
	private const int LEFT = 0;

	// Token: 0x040017DF RID: 6111
	private const int RIGHT = 1;
}
