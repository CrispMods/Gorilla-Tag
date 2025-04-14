using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200051A RID: 1306
public class FishingRod : TransferrableObject
{
	// Token: 0x06001F95 RID: 8085 RVA: 0x0009EF28 File Offset: 0x0009D128
	public override void OnActivate()
	{
		base.OnActivate();
		Transform transform = base.transform;
		Vector3 force = transform.up + transform.forward * 640f;
		this.bobRigidbody.AddForce(force, ForceMode.Impulse);
		this.line.tensionScale = 0.86f;
		this.ReelOut();
	}

	// Token: 0x06001F96 RID: 8086 RVA: 0x0009EF81 File Offset: 0x0009D181
	public override void OnDeactivate()
	{
		base.OnDeactivate();
		this.line.tensionScale = 1f;
		this.ReelStop();
	}

	// Token: 0x06001F97 RID: 8087 RVA: 0x0009EF9F File Offset: 0x0009D19F
	protected override void Start()
	{
		base.Start();
		this.rig = base.GetComponentInParent<VRRig>();
	}

	// Token: 0x06001F98 RID: 8088 RVA: 0x0009EFB3 File Offset: 0x0009D1B3
	public void SetBobFloat(bool enable)
	{
		if (!this.bobRigidbody)
		{
			return;
		}
		this._bobFloatPlaneY = this.bobRigidbody.position.y;
		this._bobFloating = enable;
	}

	// Token: 0x06001F99 RID: 8089 RVA: 0x0009EFE0 File Offset: 0x0009D1E0
	private void QuickReel()
	{
		if (this._lineResizing)
		{
			return;
		}
		this.bobCollider.enabled = false;
		this.ReelIn();
	}

	// Token: 0x06001F9A RID: 8090 RVA: 0x0009F000 File Offset: 0x0009D200
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

	// Token: 0x06001F9B RID: 8091 RVA: 0x0009F119 File Offset: 0x0009D319
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

	// Token: 0x06001F9C RID: 8092 RVA: 0x0009F150 File Offset: 0x0009D350
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

	// Token: 0x06001F9D RID: 8093 RVA: 0x0009F1D0 File Offset: 0x0009D3D0
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

	// Token: 0x06001F9E RID: 8094 RVA: 0x0009F250 File Offset: 0x0009D450
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

	// Token: 0x06001F9F RID: 8095 RVA: 0x0009F2BC File Offset: 0x0009D4BC
	private static void SetHandleMotorUse(bool useMotor, float spinRate, HingeJoint handleJoint, bool reverse)
	{
		JointMotor motor = handleJoint.motor;
		motor.force = (useMotor ? 1f : 0f) * spinRate;
		motor.targetVelocity = 16384f * (reverse ? -1f : 1f);
		handleJoint.motor = motor;
	}

	// Token: 0x06001FA0 RID: 8096 RVA: 0x0009F30C File Offset: 0x0009D50C
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

	// Token: 0x06001FA1 RID: 8097 RVA: 0x0009F39F File Offset: 0x0009D59F
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

	// Token: 0x06001FA2 RID: 8098 RVA: 0x0009F3DC File Offset: 0x0009D5DC
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

	// Token: 0x06001FA3 RID: 8099 RVA: 0x0009F6D4 File Offset: 0x0009D8D4
	private static float GetSignedDeltaYZ(ref Quaternion a, ref Quaternion b)
	{
		Vector3 forward = Vector3.forward;
		Vector3 vector = a * forward;
		Vector3 vector2 = b * forward;
		float current = Mathf.Atan2(vector.y, vector.z) * 57.29578f;
		float target = Mathf.Atan2(vector2.y, vector2.z) * 57.29578f;
		return Mathf.DeltaAngle(current, target);
	}

	// Token: 0x0400237A RID: 9082
	public Transform handleTransform;

	// Token: 0x0400237B RID: 9083
	public HingeJoint handleJoint;

	// Token: 0x0400237C RID: 9084
	public Rigidbody handleRigidbody;

	// Token: 0x0400237D RID: 9085
	public BoxCollider handleCollider;

	// Token: 0x0400237E RID: 9086
	public Rigidbody bobRigidbody;

	// Token: 0x0400237F RID: 9087
	public Collider bobCollider;

	// Token: 0x04002380 RID: 9088
	public VerletLine line;

	// Token: 0x04002381 RID: 9089
	public GorillaVelocityEstimator tipTracker;

	// Token: 0x04002382 RID: 9090
	public Rigidbody tipBody;

	// Token: 0x04002383 RID: 9091
	[NonSerialized]
	public VRRig rig;

	// Token: 0x04002384 RID: 9092
	[Space]
	public Vector3 reelFreezeLocalPosition;

	// Token: 0x04002385 RID: 9093
	public Transform reelFrom;

	// Token: 0x04002386 RID: 9094
	public Transform reelTo;

	// Token: 0x04002387 RID: 9095
	public Transform reelToSync;

	// Token: 0x04002388 RID: 9096
	[Space]
	public float reelSpinRate = 1f;

	// Token: 0x04002389 RID: 9097
	public float lineResizeRate = 1f;

	// Token: 0x0400238A RID: 9098
	public float lineCastFactor = 3f;

	// Token: 0x0400238B RID: 9099
	public float lineLengthMin = 0.1f;

	// Token: 0x0400238C RID: 9100
	public float lineLengthMax = 8f;

	// Token: 0x0400238D RID: 9101
	[Space]
	[NonSerialized]
	private bool _bobFloating;

	// Token: 0x0400238E RID: 9102
	public float bobFloatForce = 8f;

	// Token: 0x0400238F RID: 9103
	public float bobStaticDrag = 3.2f;

	// Token: 0x04002390 RID: 9104
	public float bobDynamicDrag = 1.1f;

	// Token: 0x04002391 RID: 9105
	[NonSerialized]
	private float _bobFloatPlaneY;

	// Token: 0x04002392 RID: 9106
	[Space]
	[NonSerialized]
	private float _targetSegmentMin;

	// Token: 0x04002393 RID: 9107
	[NonSerialized]
	private float _targetSegmentMax;

	// Token: 0x04002394 RID: 9108
	[Space]
	[NonSerialized]
	private bool _manualReeling;

	// Token: 0x04002395 RID: 9109
	[NonSerialized]
	private bool _lineResizing;

	// Token: 0x04002396 RID: 9110
	[NonSerialized]
	private bool _lineExpanding;

	// Token: 0x04002397 RID: 9111
	[NonSerialized]
	private bool _lineResetting;

	// Token: 0x04002398 RID: 9112
	[NonSerialized]
	private TimeSince _sinceReset;

	// Token: 0x04002399 RID: 9113
	[Space]
	[NonSerialized]
	private Quaternion _lastLocalRot = Quaternion.identity;

	// Token: 0x0400239A RID: 9114
	[NonSerialized]
	private float _localRotDelta;

	// Token: 0x0400239B RID: 9115
	[NonSerialized]
	private bool _isGrippingHandle;

	// Token: 0x0400239C RID: 9116
	[NonSerialized]
	private Transform _grippingHand;

	// Token: 0x0400239D RID: 9117
	private TimeSince _sinceGripLoss;
}
