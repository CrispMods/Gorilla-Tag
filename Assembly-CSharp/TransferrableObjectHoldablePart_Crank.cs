using System;
using GorillaExtensions;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200039A RID: 922
public class TransferrableObjectHoldablePart_Crank : TransferrableObjectHoldablePart
{
	// Token: 0x06001580 RID: 5504 RVA: 0x00068B03 File Offset: 0x00066D03
	public void SetOnCrankedCallback(Action<float> onCrankedCallback)
	{
		this.onCrankedCallback = onCrankedCallback;
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x00068B0C File Offset: 0x00066D0C
	private void Awake()
	{
		if (this.rotatingPart == null)
		{
			this.rotatingPart = base.transform;
		}
		Vector3 vector = this.rotatingPart.parent.InverseTransformPoint(this.rotatingPart.TransformPoint(Vector3.right));
		this.lastAngle = Mathf.Atan2(vector.y, vector.x);
		this.baseLocalAngle = this.rotatingPart.localRotation;
		this.baseLocalAngleInverse = Quaternion.Inverse(this.baseLocalAngle);
		this.crankRadius = new Vector2(this.crankHandleX, this.crankHandleY).magnitude;
		this.crankAngleOffset = Mathf.Atan2(this.crankHandleY, this.crankHandleX) * 57.29578f;
		if (this.crankHandleMaxZ < this.crankHandleMinZ)
		{
			float num = this.crankHandleMaxZ;
			float num2 = this.crankHandleMinZ;
			this.crankHandleMinZ = num;
			this.crankHandleMaxZ = num2;
		}
	}

	// Token: 0x06001582 RID: 5506 RVA: 0x00068BF4 File Offset: 0x00066DF4
	protected override void UpdateHeld(VRRig rig, bool isHeldLeftHand)
	{
		Vector3 a;
		if (rig.isOfflineVRRig)
		{
			Transform transform = isHeldLeftHand ? GTPlayer.Instance.leftControllerTransform : GTPlayer.Instance.rightControllerTransform;
			Vector3 vector = this.rotatingPart.InverseTransformPoint(transform.position);
			Vector3 position = (vector.xy().normalized * this.crankRadius).WithZ(Mathf.Clamp(vector.z, this.crankHandleMinZ, this.crankHandleMaxZ));
			Vector3 vector2 = this.rotatingPart.TransformPoint(position);
			if (this.maxHandSnapDistance > 0f && (transform.position - vector2).IsLongerThan(this.maxHandSnapDistance))
			{
				this.OnRelease(null, isHeldLeftHand ? EquipmentInteractor.instance.leftHand : EquipmentInteractor.instance.rightHand);
				return;
			}
			transform.position = vector2;
			a = transform.position;
		}
		else
		{
			VRMap vrmap = isHeldLeftHand ? rig.leftHand : rig.rightHand;
			a = vrmap.GetExtrapolatedControllerPosition();
			a -= vrmap.rigTarget.rotation * (isHeldLeftHand ? GTPlayer.Instance.leftHandOffset : GTPlayer.Instance.rightHandOffset) * rig.scaleFactor;
		}
		Vector3 vector3 = this.baseLocalAngleInverse * Quaternion.Inverse(this.rotatingPart.parent.rotation) * (a - this.rotatingPart.position);
		float num = Mathf.Atan2(vector3.y, vector3.x) * 57.29578f;
		float num2 = Mathf.DeltaAngle(this.lastAngle, num);
		this.lastAngle = num;
		if (num2 != 0f)
		{
			if (this.onCrankedCallback != null)
			{
				this.onCrankedCallback(num2);
			}
			for (int i = 0; i < this.thresholds.Length; i++)
			{
				this.thresholds[i].OnCranked(num2);
			}
		}
		this.rotatingPart.localRotation = this.baseLocalAngle * Quaternion.AngleAxis(num - this.crankAngleOffset, Vector3.forward);
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x00068E10 File Offset: 0x00067010
	private void OnDrawGizmosSelected()
	{
		Transform transform = (this.rotatingPart != null) ? this.rotatingPart : base.transform;
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.TransformPoint(new Vector3(this.crankHandleX, this.crankHandleY, this.crankHandleMinZ)), transform.TransformPoint(new Vector3(this.crankHandleX, this.crankHandleY, this.crankHandleMaxZ)));
	}

	// Token: 0x040017CB RID: 6091
	[SerializeField]
	private float crankHandleX;

	// Token: 0x040017CC RID: 6092
	[SerializeField]
	private float crankHandleY;

	// Token: 0x040017CD RID: 6093
	[SerializeField]
	private float crankHandleMinZ;

	// Token: 0x040017CE RID: 6094
	[SerializeField]
	private float crankHandleMaxZ;

	// Token: 0x040017CF RID: 6095
	[SerializeField]
	private float maxHandSnapDistance;

	// Token: 0x040017D0 RID: 6096
	private float crankAngleOffset;

	// Token: 0x040017D1 RID: 6097
	private float crankRadius;

	// Token: 0x040017D2 RID: 6098
	[SerializeField]
	private Transform rotatingPart;

	// Token: 0x040017D3 RID: 6099
	private float lastAngle;

	// Token: 0x040017D4 RID: 6100
	private Quaternion baseLocalAngle;

	// Token: 0x040017D5 RID: 6101
	private Quaternion baseLocalAngleInverse;

	// Token: 0x040017D6 RID: 6102
	private Action<float> onCrankedCallback;

	// Token: 0x040017D7 RID: 6103
	[SerializeField]
	private TransferrableObjectHoldablePart_Crank.CrankThreshold[] thresholds;

	// Token: 0x0200039B RID: 923
	[Serializable]
	private struct CrankThreshold
	{
		// Token: 0x06001585 RID: 5509 RVA: 0x00068E8B File Offset: 0x0006708B
		public void OnCranked(float deltaAngle)
		{
			this.currentAngle += deltaAngle;
			if (Mathf.Abs(this.currentAngle) > this.angleThreshold)
			{
				this.currentAngle = 0f;
				this.onReached.Invoke();
			}
		}

		// Token: 0x040017D8 RID: 6104
		public float angleThreshold;

		// Token: 0x040017D9 RID: 6105
		public UnityEvent onReached;

		// Token: 0x040017DA RID: 6106
		[HideInInspector]
		public float currentAngle;
	}
}
