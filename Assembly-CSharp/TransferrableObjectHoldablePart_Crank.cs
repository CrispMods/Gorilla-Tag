using System;
using GorillaExtensions;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020003A5 RID: 933
public class TransferrableObjectHoldablePart_Crank : TransferrableObjectHoldablePart
{
	// Token: 0x060015CC RID: 5580 RVA: 0x0003EB10 File Offset: 0x0003CD10
	public void SetOnCrankedCallback(Action<float> onCrankedCallback)
	{
		this.onCrankedCallback = onCrankedCallback;
	}

	// Token: 0x060015CD RID: 5581 RVA: 0x000C109C File Offset: 0x000BF29C
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

	// Token: 0x060015CE RID: 5582 RVA: 0x000C1184 File Offset: 0x000BF384
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

	// Token: 0x060015CF RID: 5583 RVA: 0x000C13A0 File Offset: 0x000BF5A0
	private void OnDrawGizmosSelected()
	{
		Transform transform = (this.rotatingPart != null) ? this.rotatingPart : base.transform;
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.TransformPoint(new Vector3(this.crankHandleX, this.crankHandleY, this.crankHandleMinZ)), transform.TransformPoint(new Vector3(this.crankHandleX, this.crankHandleY, this.crankHandleMaxZ)));
	}

	// Token: 0x04001812 RID: 6162
	[SerializeField]
	private float crankHandleX;

	// Token: 0x04001813 RID: 6163
	[SerializeField]
	private float crankHandleY;

	// Token: 0x04001814 RID: 6164
	[SerializeField]
	private float crankHandleMinZ;

	// Token: 0x04001815 RID: 6165
	[SerializeField]
	private float crankHandleMaxZ;

	// Token: 0x04001816 RID: 6166
	[SerializeField]
	private float maxHandSnapDistance;

	// Token: 0x04001817 RID: 6167
	private float crankAngleOffset;

	// Token: 0x04001818 RID: 6168
	private float crankRadius;

	// Token: 0x04001819 RID: 6169
	[SerializeField]
	private Transform rotatingPart;

	// Token: 0x0400181A RID: 6170
	private float lastAngle;

	// Token: 0x0400181B RID: 6171
	private Quaternion baseLocalAngle;

	// Token: 0x0400181C RID: 6172
	private Quaternion baseLocalAngleInverse;

	// Token: 0x0400181D RID: 6173
	private Action<float> onCrankedCallback;

	// Token: 0x0400181E RID: 6174
	[SerializeField]
	private TransferrableObjectHoldablePart_Crank.CrankThreshold[] thresholds;

	// Token: 0x020003A6 RID: 934
	[Serializable]
	private struct CrankThreshold
	{
		// Token: 0x060015D1 RID: 5585 RVA: 0x0003EB21 File Offset: 0x0003CD21
		public void OnCranked(float deltaAngle)
		{
			this.currentAngle += deltaAngle;
			if (Mathf.Abs(this.currentAngle) > this.angleThreshold)
			{
				this.currentAngle = 0f;
				this.onReached.Invoke();
			}
		}

		// Token: 0x0400181F RID: 6175
		public float angleThreshold;

		// Token: 0x04001820 RID: 6176
		public UnityEvent onReached;

		// Token: 0x04001821 RID: 6177
		[HideInInspector]
		public float currentAngle;
	}
}
