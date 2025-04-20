using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000527 RID: 1319
public class FishingRod : TransferrableObject
{
	// Token: 0x06001FEE RID: 8174 RVA: 0x000F093C File Offset: 0x000EEB3C
	public override void OnActivate()
	{
		base.OnActivate();
		Transform transform = base.transform;
		Vector3 force = transform.up + transform.forward * 640f;
		this.bobRigidbody.AddForce(force, ForceMode.Impulse);
		this.line.tensionScale = 0.86f;
		this.ReelOut();
	}

	// Token: 0x06001FEF RID: 8175 RVA: 0x00045B8B File Offset: 0x00043D8B
	public override void OnDeactivate()
	{
		base.OnDeactivate();
		this.line.tensionScale = 1f;
		this.ReelStop();
	}

	// Token: 0x06001FF0 RID: 8176 RVA: 0x00045BA9 File Offset: 0x00043DA9
	protected override void Start()
	{
		base.Start();
		this.rig = base.GetComponentInParent<VRRig>();
	}

	// Token: 0x06001FF1 RID: 8177 RVA: 0x00045BBD File Offset: 0x00043DBD
	public void SetBobFloat(bool enable)
	{
		if (!this.bobRigidbody)
		{
			return;
		}
		this._bobFloatPlaneY = this.bobRigidbody.position.y;
		this._bobFloating = enable;
	}

	// Token: 0x06001FF2 RID: 8178 RVA: 0x00045BEA File Offset: 0x00043DEA
	private void QuickReel()
	{
		if (this._lineResizing)
		{
			return;
		}
		this.bobCollider.enabled = false;
		this.ReelIn();
	}

	// Token: 0x06001FF3 RID: 8179 RVA: 0x000F0998 File Offset: 0x000EEB98
	public bool IsFreeHandGripping()
	{
		bool flag = base.InLeftHand();
		Transform transform = flag ? this.rig.rightHandTransform : this.rig.leftHandTransform;
		float magnitude = (this.reelToSync.position - transform.position).magnitude;
		bool flag2 = this._grippingHand || magnitude <= 0.16f;
		this.disableStealing = flag2;
		if (!flag2)
		{
			return false;
		}
		VRMapThumb vrmapThumb = flag ? this.rig.rightThumb : this.rig.leftThumb;
		VRMapIndex vrmapIndex = flag ? this.rig.rightIndex : this.rig.leftIndex;
		VRMap vrmap = flag ? this.rig.rightMiddle : this.rig.leftMiddle;
		float calcT = vrmapThumb.calcT;
		float calcT2 = vrmapIndex.calcT;
		float calcT3 = vrmap.calcT;
		bool flag3 = calcT >= 0.1f && calcT2 >= 0.2f && calcT3 >= 0.2f;
		this._grippingHand = (flag3 ? transform : null);
		return flag3;
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x00045C07 File Offset: 0x00043E07
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		if (this._grippingHand)
		{
			this._grippingHand = null;
		}
		this.ResetLineLength(this.lineLengthMin * 1.32f);
		return true;
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x000F0AB4 File Offset: 0x000EECB4
	public void ReelIn()
	{
		this._manualReeling = false;
		FishingRod.SetHandleMotorUse(true, this.reelSpinRate, this.handleJoint, true);
		this._lineResizing = true;
		this._lineExpanding = false;
		float num = (float)this.line.segmentNumber + 0.0001f;
		this.line.segmentMinLength = (this._targetSegmentMin = this.lineLengthMin / num);
		this.line.segmentMaxLength = (this._targetSegmentMax = this.lineLengthMax / num);
	}

	// Token: 0x06001FF6 RID: 8182 RVA: 0x000F0B34 File Offset: 0x000EED34
	public void ReelOut()
	{
		this._manualReeling = false;
		FishingRod.SetHandleMotorUse(true, this.reelSpinRate, this.handleJoint, false);
		this._lineResizing = true;
		this._lineExpanding = true;
		float num = (float)this.line.segmentNumber + 0.0001f;
		this.line.segmentMinLength = (this._targetSegmentMin = this.lineLengthMin / num);
		this.line.segmentMaxLength = (this._targetSegmentMax = this.lineLengthMax / num);
	}

	// Token: 0x06001FF7 RID: 8183 RVA: 0x000F0BB4 File Offset: 0x000EEDB4
	public void ReelStop()
	{
		if (this._manualReeling)
		{
			this._localRotDelta = 0f;
		}
		else
		{
			FishingRod.SetHandleMotorUse(false, 0f, this.handleJoint, false);
		}
		this.bobCollider.enabled = true;
		if (this.line)
		{
			this.line.resizeScale = 1f;
		}
		this._lineResizing = false;
		this._lineExpanding = false;
	}

	// Token: 0x06001FF8 RID: 8184 RVA: 0x000F0C20 File Offset: 0x000EEE20
	private static void SetHandleMotorUse(bool useMotor, float spinRate, HingeJoint handleJoint, bool reverse)
	{
		JointMotor motor = handleJoint.motor;
		motor.force = (useMotor ? 1f : 0f) * spinRate;
		motor.targetVelocity = 16384f * (reverse ? -1f : 1f);
		handleJoint.motor = motor;
	}

	// Token: 0x06001FF9 RID: 8185 RVA: 0x000F0C70 File Offset: 0x000EEE70
	public override void TriggeredLateUpdate()
	{
		base.TriggeredLateUpdate();
		this._manualReeling = (this._isGrippingHandle = this.IsFreeHandGripping());
		if (ControllerInputPoller.instance && ControllerInputPoller.PrimaryButtonPress(base.InLeftHand() ? XRNode.LeftHand : XRNode.RightHand))
		{
			this.QuickReel();
		}
		if (this._lineResetting && this._sinceReset.HasElapsed(this.line.resizeSpeed))
		{
			this.bobCollider.enabled = true;
			this._lineResetting = false;
		}
		this.handleTransform.localPosition = this.reelFreezeLocalPosition;
	}

	// Token: 0x06001FFA RID: 8186 RVA: 0x00045C3C File Offset: 0x00043E3C
	private void ResetLineLength(float length)
	{
		if (!this.line)
		{
			return;
		}
		this._lineResetting = true;
		this.bobCollider.enabled = false;
		this.line.ForceTotalLength(length);
		this._sinceReset = TimeSince.Now();
	}

	// Token: 0x06001FFB RID: 8187 RVA: 0x000F0D04 File Offset: 0x000EEF04
	private void FixedUpdate()
	{
		Transform transform = base.transform;
		this.handleRigidbody.useGravity = !this._manualReeling;
		if (this._bobFloating && this.bobRigidbody)
		{
			float y = this.bobRigidbody.position.y;
			float num = this.bobFloatForce * this.bobRigidbody.mass;
			float num2 = num * Mathf.Clamp01(this._bobFloatPlaneY - y);
			num += num2;
			if (y <= this._bobFloatPlaneY)
			{
				this.bobRigidbody.AddForce(0f, num, 0f);
			}
		}
		if (this._manualReeling)
		{
			if (this._isGrippingHandle && this._grippingHand)
			{
				this.reelTo.position = this._grippingHand.position;
			}
			Vector3 vector = this.reelFrom.InverseTransformPoint(this.reelTo.position);
			vector.x = 0f;
			vector.Normalize();
			vector *= 2f;
			Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, vector);
			quaternion = (base.InRightHand() ? quaternion : Quaternion.Inverse(quaternion));
			this._localRotDelta = FishingRod.GetSignedDeltaYZ(ref this._lastLocalRot, ref quaternion);
			this._lastLocalRot = quaternion;
			Quaternion rot = transform.rotation * quaternion;
			this.handleRigidbody.MoveRotation(rot);
		}
		else
		{
			this.reelTo.localPosition = transform.InverseTransformPoint(this.reelToSync.position);
		}
		if (!this.line)
		{
			return;
		}
		if (this._manualReeling)
		{
			this._lineResizing = (Mathf.Abs(this._localRotDelta) >= 0.001f);
			this._lineExpanding = (Mathf.Sign(this._localRotDelta) >= 0f);
		}
		if (!this._lineResizing)
		{
			return;
		}
		float num3 = this._manualReeling ? (Mathf.Abs(this._localRotDelta) * 0.66f * Time.fixedDeltaTime) : (this.lineResizeRate * this.lineCastFactor);
		this.line.resizeScale = this.lineCastFactor;
		float num4 = num3 * Time.fixedDeltaTime;
		float num5 = this.line.segmentTargetLength;
		if (this._manualReeling)
		{
			float num6 = 1f / ((float)this.line.segmentNumber + 0.0001f);
			float num7 = this.lineLengthMin * num6;
			float num8 = this.lineLengthMax * num6;
			num4 *= (this._lineExpanding ? 1f : -1f);
			num4 *= (base.InRightHand() ? -1f : 1f);
			float num9 = num5 + num4;
			if (num9 > num7 && num9 < num8)
			{
				num5 += num4;
			}
		}
		else if (this._lineExpanding)
		{
			if (num5 < this._targetSegmentMax)
			{
				num5 += num4;
			}
			else
			{
				this._lineResizing = false;
			}
		}
		else if (num5 > this._targetSegmentMin)
		{
			num5 -= num4;
		}
		else
		{
			this._lineResizing = false;
		}
		if (this._lineResizing)
		{
			this.line.segmentTargetLength = num5;
			return;
		}
		this.ReelStop();
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x000F0FFC File Offset: 0x000EF1FC
	private static float GetSignedDeltaYZ(ref Quaternion a, ref Quaternion b)
	{
		Vector3 forward = Vector3.forward;
		Vector3 vector = a * forward;
		Vector3 vector2 = b * forward;
		float current = Mathf.Atan2(vector.y, vector.z) * 57.29578f;
		float target = Mathf.Atan2(vector2.y, vector2.z) * 57.29578f;
		return Mathf.DeltaAngle(current, target);
	}

	// Token: 0x040023CD RID: 9165
	public Transform handleTransform;

	// Token: 0x040023CE RID: 9166
	public HingeJoint handleJoint;

	// Token: 0x040023CF RID: 9167
	public Rigidbody handleRigidbody;

	// Token: 0x040023D0 RID: 9168
	public BoxCollider handleCollider;

	// Token: 0x040023D1 RID: 9169
	public Rigidbody bobRigidbody;

	// Token: 0x040023D2 RID: 9170
	public Collider bobCollider;

	// Token: 0x040023D3 RID: 9171
	public VerletLine line;

	// Token: 0x040023D4 RID: 9172
	public GorillaVelocityEstimator tipTracker;

	// Token: 0x040023D5 RID: 9173
	public Rigidbody tipBody;

	// Token: 0x040023D6 RID: 9174
	[NonSerialized]
	public VRRig rig;

	// Token: 0x040023D7 RID: 9175
	[Space]
	public Vector3 reelFreezeLocalPosition;

	// Token: 0x040023D8 RID: 9176
	public Transform reelFrom;

	// Token: 0x040023D9 RID: 9177
	public Transform reelTo;

	// Token: 0x040023DA RID: 9178
	public Transform reelToSync;

	// Token: 0x040023DB RID: 9179
	[Space]
	public float reelSpinRate = 1f;

	// Token: 0x040023DC RID: 9180
	public float lineResizeRate = 1f;

	// Token: 0x040023DD RID: 9181
	public float lineCastFactor = 3f;

	// Token: 0x040023DE RID: 9182
	public float lineLengthMin = 0.1f;

	// Token: 0x040023DF RID: 9183
	public float lineLengthMax = 8f;

	// Token: 0x040023E0 RID: 9184
	[Space]
	[NonSerialized]
	private bool _bobFloating;

	// Token: 0x040023E1 RID: 9185
	public float bobFloatForce = 8f;

	// Token: 0x040023E2 RID: 9186
	public float bobStaticDrag = 3.2f;

	// Token: 0x040023E3 RID: 9187
	public float bobDynamicDrag = 1.1f;

	// Token: 0x040023E4 RID: 9188
	[NonSerialized]
	private float _bobFloatPlaneY;

	// Token: 0x040023E5 RID: 9189
	[Space]
	[NonSerialized]
	private float _targetSegmentMin;

	// Token: 0x040023E6 RID: 9190
	[NonSerialized]
	private float _targetSegmentMax;

	// Token: 0x040023E7 RID: 9191
	[Space]
	[NonSerialized]
	private bool _manualReeling;

	// Token: 0x040023E8 RID: 9192
	[NonSerialized]
	private bool _lineResizing;

	// Token: 0x040023E9 RID: 9193
	[NonSerialized]
	private bool _lineExpanding;

	// Token: 0x040023EA RID: 9194
	[NonSerialized]
	private bool _lineResetting;

	// Token: 0x040023EB RID: 9195
	[NonSerialized]
	private TimeSince _sinceReset;

	// Token: 0x040023EC RID: 9196
	[Space]
	[NonSerialized]
	private Quaternion _lastLocalRot = Quaternion.identity;

	// Token: 0x040023ED RID: 9197
	[NonSerialized]
	private float _localRotDelta;

	// Token: 0x040023EE RID: 9198
	[NonSerialized]
	private bool _isGrippingHandle;

	// Token: 0x040023EF RID: 9199
	[NonSerialized]
	private Transform _grippingHand;

	// Token: 0x040023F0 RID: 9200
	private TimeSince _sinceGripLoss;
}
