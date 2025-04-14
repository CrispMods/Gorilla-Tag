using System;
using System.Runtime.CompilerServices;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using GorillaTag.Cosmetics;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020000ED RID: 237
public class RCHoverboard : RCVehicle
{
	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000637 RID: 1591 RVA: 0x00023D9B File Offset: 0x00021F9B
	// (set) Token: 0x06000638 RID: 1592 RVA: 0x00023DA3 File Offset: 0x00021FA3
	private float _MaxForwardSpeed
	{
		get
		{
			return this.m_maxForwardSpeed;
		}
		set
		{
			this.m_maxForwardSpeed = value;
			this._forwardAccel = value / math.max(0.01f, this.m_forwardAccelTime);
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x06000639 RID: 1593 RVA: 0x00023DC4 File Offset: 0x00021FC4
	// (set) Token: 0x0600063A RID: 1594 RVA: 0x00023DCC File Offset: 0x00021FCC
	private float _MaxTurnRate
	{
		get
		{
			return this.m_maxTurnRate;
		}
		set
		{
			this.m_maxTurnRate = value;
			this._turnAccel = value / math.max(1E-06f, this.m_turnAccelTime);
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600063B RID: 1595 RVA: 0x00023DED File Offset: 0x00021FED
	// (set) Token: 0x0600063C RID: 1596 RVA: 0x00023DF5 File Offset: 0x00021FF5
	private float _MaxTiltAngle
	{
		get
		{
			return this.m_maxTiltAngle;
		}
		set
		{
			this.m_maxTiltAngle = value;
			this._tiltAccel = value / math.max(1E-06f, this.m_tiltTime);
		}
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x00023E18 File Offset: 0x00022018
	protected override void Awake()
	{
		base.Awake();
		this._hasAudioSource = (this.m_audioSource != null);
		this._hasHoverSound = (this.m_hoverSound != null);
		this._MaxForwardSpeed = this.m_maxForwardSpeed;
		this._MaxTurnRate = this.m_maxTurnRate;
		this._MaxTiltAngle = this.m_maxTiltAngle;
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x00023E74 File Offset: 0x00022074
	protected override void AuthorityBeginDocked()
	{
		base.AuthorityBeginDocked();
		this._currentTurnRate = 0f;
		this._currentTiltAngle = 0f;
		float3 to = this._ProjectOnPlane(base.transform.forward, math.up());
		this._currentTurnAngle = this._SignedAngle(new float3(0f, 0f, 1f), to, new float3(0f, 1f, 0f));
		this._motorLevel = 0f;
		if (this._hasAudioSource)
		{
			this.m_audioSource.Stop();
			this.m_audioSource.volume = 0f;
		}
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x00023F1C File Offset: 0x0002211C
	protected override void AuthorityUpdate(float dt)
	{
		base.AuthorityUpdate(dt);
		if (this.localState == RCVehicle.State.Mobilized)
		{
			float x = math.length(this.activeInput.joystick);
			this._motorLevel = math.saturate(x);
			if (this.hasNetworkSync)
			{
				this.networkSync.syncedState.dataA = (byte)((uint)(this._motorLevel * 255f));
				return;
			}
		}
		else
		{
			this._motorLevel = 0f;
		}
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x00023F90 File Offset: 0x00022190
	protected override void RemoteUpdate(float dt)
	{
		base.RemoteUpdate(dt);
		if (this.localState == RCVehicle.State.Mobilized && this.hasNetworkSync)
		{
			this._motorLevel = (float)this.networkSync.syncedState.dataA / 255f;
			return;
		}
		this._motorLevel = 0f;
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x00023FE0 File Offset: 0x000221E0
	protected override void SharedUpdate(float dt)
	{
		base.SharedUpdate(dt);
		switch (this.localState)
		{
		case RCVehicle.State.Disabled:
		case RCVehicle.State.DockedLeft:
		case RCVehicle.State.DockedRight:
		case RCVehicle.State.Crashed:
			break;
		case RCVehicle.State.Mobilized:
			if (this._hasAudioSource && this._hasHoverSound)
			{
				if (this.localStatePrev != RCVehicle.State.Mobilized)
				{
					this.m_audioSource.volume = 0f;
					this.m_audioSource.clip = this.m_hoverSound;
					this.m_audioSource.loop = true;
					this.m_audioSource.Play();
					return;
				}
				float target = math.lerp(this.m_hoverSoundVolumeMinMax.x, this.m_hoverSoundVolumeMinMax.y, this._motorLevel);
				float maxDelta = this.m_hoverSoundVolumeMinMax.y / this.m_hoverSoundVolumeRampTime * dt;
				this.m_audioSource.volume = this._MoveTowards(this.m_audioSource.volume, target, maxDelta);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x000240C4 File Offset: 0x000222C4
	protected void FixedUpdate()
	{
		if (!base.HasLocalAuthority || this.localState != RCVehicle.State.Mobilized)
		{
			return;
		}
		float fixedDeltaTime = Time.fixedDeltaTime;
		float num = this.m_inputThrustForward.Get(this.activeInput) - this.m_inputThrustBack.Get(this.activeInput);
		float num2 = this.m_inputTurn.Get(this.activeInput);
		float num3 = this.m_inputJump.Get(this.activeInput);
		RaycastHit raycastHit;
		bool flag = Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, 10f, 134218241, QueryTriggerInteraction.Ignore);
		bool flag2 = flag && raycastHit.distance <= this.m_hoverHeight + 0.1f;
		if (num3 > 0.001f && flag2 && !this._hasJumped)
		{
			this.rb.AddForce(Vector3.up * this.m_jumpForce, ForceMode.Impulse);
			this._hasJumped = true;
		}
		else if (num3 <= 0.001f)
		{
			this._hasJumped = false;
		}
		float target = num2 * this._MaxTurnRate;
		this._currentTurnRate = this._MoveTowards(this._currentTurnRate, target, this._turnAccel * fixedDeltaTime);
		this._currentTurnAngle += this._currentTurnRate * fixedDeltaTime;
		float target2 = math.lerp(-this.m_maxTiltAngle, this.m_maxTiltAngle, math.unlerp(-1f, 1f, num));
		this._currentTiltAngle = this._MoveTowards(this._currentTiltAngle, target2, this._tiltAccel * fixedDeltaTime);
		base.transform.rotation = quaternion.EulerXYZ(math.radians(new float3(this._currentTiltAngle, this._currentTurnAngle, 0f)));
		float3 @float = base.transform.forward;
		float num4 = math.dot(@float, this.rb.velocity);
		float num5 = num * this.m_maxForwardSpeed;
		float rhs = (math.abs(num5) > 0.001f && ((num5 > 0f && num4 < num5) || (num5 < 0f && num4 > num5))) ? math.sign(num5) : 0f;
		this.rb.AddForce(@float * this._forwardAccel * rhs, ForceMode.Acceleration);
		if (flag)
		{
			float num6 = math.saturate(this.m_hoverHeight - raycastHit.distance);
			float num7 = math.dot(this.rb.velocity, Vector3.up);
			float rhs2 = num6 * this.m_hoverForce - num7 * this.m_hoverDamp;
			this.rb.AddForce(math.up() * rhs2, ForceMode.Force);
		}
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x00024370 File Offset: 0x00022570
	protected void OnCollisionEnter(Collision collision)
	{
		GameObject gameObject = collision.collider.gameObject;
		bool flag = gameObject.IsOnLayer(UnityLayer.GorillaThrowable);
		bool flag2 = gameObject.IsOnLayer(UnityLayer.GorillaHand);
		if ((flag || flag2) && this.localState == RCVehicle.State.Mobilized)
		{
			Vector3 vector = Vector3.zero;
			if (flag2)
			{
				GorillaHandClimber component = gameObject.GetComponent<GorillaHandClimber>();
				if (component != null)
				{
					vector = ((component.xrNode == XRNode.LeftHand) ? GTPlayer.Instance.leftHandCenterVelocityTracker : GTPlayer.Instance.rightHandCenterVelocityTracker).GetAverageVelocity(true, 0.15f, false);
				}
			}
			else if (collision.rigidbody != null)
			{
				vector = collision.rigidbody.velocity;
			}
			if (flag || vector.sqrMagnitude > 0.01f)
			{
				if (base.HasLocalAuthority)
				{
					this.AuthorityApplyImpact(vector, flag);
					return;
				}
				if (this.networkSync != null)
				{
					this.networkSync.photonView.RPC("HitRCVehicleRPC", RpcTarget.Others, new object[]
					{
						vector,
						flag
					});
				}
			}
		}
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x00024471 File Offset: 0x00022671
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private float _MoveTowards(float current, float target, float maxDelta)
	{
		if (math.abs(target - current) > maxDelta)
		{
			return current + math.sign(target - current) * maxDelta;
		}
		return target;
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x0002448C File Offset: 0x0002268C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private float _SignedAngle(float3 from, float3 to, float3 axis)
	{
		float3 x = math.normalize(from);
		float3 y = math.normalize(to);
		float x2 = math.acos(math.dot(x, y));
		float num = math.sign(math.dot(math.cross(x, y), axis));
		return math.degrees(x2) * num;
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x000244CD File Offset: 0x000226CD
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private float3 _ProjectOnPlane(float3 vector, float3 planeNormal)
	{
		return vector - math.dot(vector, planeNormal) * planeNormal;
	}

	// Token: 0x04000752 RID: 1874
	[SerializeField]
	private RCHoverboard._SingleInputOption m_inputTurn = new RCHoverboard._SingleInputOption(RCHoverboard._EInputSource.StickX, new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 0f, 0f, 0f),
		new Keyframe(0.1f, 0f, 0f, 1.25f, 0f, 0f),
		new Keyframe(0.9f, 1f, 1.25f, 0f, 0f, 0f),
		new Keyframe(1f, 1f, 0f, 0f, 0f, 0f)
	}));

	// Token: 0x04000753 RID: 1875
	[SerializeField]
	private RCHoverboard._SingleInputOption m_inputThrustForward = new RCHoverboard._SingleInputOption(RCHoverboard._EInputSource.Trigger, AnimationCurves.EaseInCirc);

	// Token: 0x04000754 RID: 1876
	[SerializeField]
	private RCHoverboard._SingleInputOption m_inputThrustBack = new RCHoverboard._SingleInputOption(RCHoverboard._EInputSource.StickBack, new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 0f, 0f, 0f),
		new Keyframe(0.9f, 0f, 0f, 9.9999f, 0.5825f, 0.3767f),
		new Keyframe(1f, 1f, 9.9999f, 1f, 0f, 0f)
	}));

	// Token: 0x04000755 RID: 1877
	[SerializeField]
	private RCHoverboard._SingleInputOption m_inputJump = new RCHoverboard._SingleInputOption(RCHoverboard._EInputSource.PrimaryFaceButton, AnimationCurves.Linear);

	// Token: 0x04000756 RID: 1878
	[Tooltip("Desired hover height above ground from this transform's position.")]
	[SerializeField]
	private float m_hoverHeight = 0.2f;

	// Token: 0x04000757 RID: 1879
	[Tooltip("Upward force to maintain hover when below hoverHeight.")]
	[SerializeField]
	private float m_hoverForce = 200f;

	// Token: 0x04000758 RID: 1880
	[Tooltip("Damping factor to smooth out vertical movement.")]
	[SerializeField]
	private float m_hoverDamp = 5f;

	// Token: 0x04000759 RID: 1881
	[Tooltip("Upward impulse force for jump.")]
	[SerializeField]
	private float m_jumpForce = 3.5f;

	// Token: 0x0400075A RID: 1882
	private bool _hasJumped;

	// Token: 0x0400075B RID: 1883
	[SerializeField]
	[HideInInspector]
	private float m_maxForwardSpeed = 6f;

	// Token: 0x0400075C RID: 1884
	[SerializeField]
	[Tooltip("Time (seconds) to reach max forward speed from zero.")]
	private float m_forwardAccelTime = 2f;

	// Token: 0x0400075D RID: 1885
	[SerializeField]
	[HideInInspector]
	private float m_maxTurnRate = 720f;

	// Token: 0x0400075E RID: 1886
	[Tooltip("Time (seconds) to reach max turning rate.")]
	[SerializeField]
	private float m_turnAccelTime = 0.75f;

	// Token: 0x0400075F RID: 1887
	[SerializeField]
	[HideInInspector]
	private float m_maxTiltAngle = 30f;

	// Token: 0x04000760 RID: 1888
	[Tooltip("Time (seconds) to reach max tilt angle.")]
	[SerializeField]
	private float m_tiltTime = 0.1f;

	// Token: 0x04000761 RID: 1889
	[Tooltip("Audio source for any motor or hover sound.")]
	[SerializeField]
	private AudioSource m_audioSource;

	// Token: 0x04000762 RID: 1890
	[Tooltip("Looping motor/hover sound clip.")]
	[SerializeField]
	private AudioClip m_hoverSound;

	// Token: 0x04000763 RID: 1891
	[Tooltip("Volume range for the hover sound (x = min, y = max).")]
	[SerializeField]
	private float2 m_hoverSoundVolumeMinMax = new float2(0.1f, 0.5f);

	// Token: 0x04000764 RID: 1892
	[Tooltip("Time it takes for the volume to reach max value.")]
	[SerializeField]
	private float m_hoverSoundVolumeRampTime = 1f;

	// Token: 0x04000765 RID: 1893
	private bool _hasAudioSource;

	// Token: 0x04000766 RID: 1894
	private bool _hasHoverSound;

	// Token: 0x04000767 RID: 1895
	private float _forwardAccel;

	// Token: 0x04000768 RID: 1896
	private float _turnAccel;

	// Token: 0x04000769 RID: 1897
	private float _tiltAccel;

	// Token: 0x0400076A RID: 1898
	private float _currentTurnRate;

	// Token: 0x0400076B RID: 1899
	private float _currentTurnAngle;

	// Token: 0x0400076C RID: 1900
	private float _currentTiltAngle;

	// Token: 0x0400076D RID: 1901
	private float _motorLevel;

	// Token: 0x020000EE RID: 238
	private enum _EInputSource
	{
		// Token: 0x0400076F RID: 1903
		None,
		// Token: 0x04000770 RID: 1904
		StickX,
		// Token: 0x04000771 RID: 1905
		StickForward,
		// Token: 0x04000772 RID: 1906
		StickBack,
		// Token: 0x04000773 RID: 1907
		Trigger,
		// Token: 0x04000774 RID: 1908
		PrimaryFaceButton
	}

	// Token: 0x020000EF RID: 239
	[Serializable]
	private struct _SingleInputOption
	{
		// Token: 0x06000648 RID: 1608 RVA: 0x000246FB File Offset: 0x000228FB
		public _SingleInputOption(RCHoverboard._EInputSource source, AnimationCurve remapCurve)
		{
			this.source = new GTOption<StringEnum<RCHoverboard._EInputSource>>(source);
			this.remapCurve = new GTOption<AnimationCurve>(remapCurve);
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0002471C File Offset: 0x0002291C
		public float Get(RCRemoteHoldable.RCInput input)
		{
			float num;
			switch (this.source.ResolvedValue.Value)
			{
			case RCHoverboard._EInputSource.None:
				num = 0f;
				break;
			case RCHoverboard._EInputSource.StickX:
				num = input.joystick.x;
				break;
			case RCHoverboard._EInputSource.StickForward:
				num = math.saturate(input.joystick.y);
				break;
			case RCHoverboard._EInputSource.StickBack:
				num = math.saturate(-input.joystick.y);
				break;
			case RCHoverboard._EInputSource.Trigger:
				num = input.trigger;
				break;
			case RCHoverboard._EInputSource.PrimaryFaceButton:
				num = (float)input.buttons;
				break;
			default:
				num = 0f;
				break;
			}
			float x = num;
			return this.remapCurve.ResolvedValue.Evaluate(math.abs(x)) * math.sign(x);
		}

		// Token: 0x04000775 RID: 1909
		public GTOption<StringEnum<RCHoverboard._EInputSource>> source;

		// Token: 0x04000776 RID: 1910
		public GTOption<AnimationCurve> remapCurve;
	}
}
