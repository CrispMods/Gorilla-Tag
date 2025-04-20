using System;
using System.Runtime.CompilerServices;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using GorillaTag.Cosmetics;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020000F7 RID: 247
public class RCHoverboard : RCVehicle
{
	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06000678 RID: 1656 RVA: 0x00034C14 File Offset: 0x00032E14
	// (set) Token: 0x06000679 RID: 1657 RVA: 0x00034C1C File Offset: 0x00032E1C
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

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x0600067A RID: 1658 RVA: 0x00034C3D File Offset: 0x00032E3D
	// (set) Token: 0x0600067B RID: 1659 RVA: 0x00034C45 File Offset: 0x00032E45
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

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x0600067C RID: 1660 RVA: 0x00034C66 File Offset: 0x00032E66
	// (set) Token: 0x0600067D RID: 1661 RVA: 0x00034C6E File Offset: 0x00032E6E
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

	// Token: 0x0600067E RID: 1662 RVA: 0x0008631C File Offset: 0x0008451C
	protected override void Awake()
	{
		base.Awake();
		this._hasAudioSource = (this.m_audioSource != null);
		this._hasHoverSound = (this.m_hoverSound != null);
		this._MaxForwardSpeed = this.m_maxForwardSpeed;
		this._MaxTurnRate = this.m_maxTurnRate;
		this._MaxTiltAngle = this.m_maxTiltAngle;
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00086378 File Offset: 0x00084578
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

	// Token: 0x06000680 RID: 1664 RVA: 0x00086420 File Offset: 0x00084620
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

	// Token: 0x06000681 RID: 1665 RVA: 0x00086494 File Offset: 0x00084694
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

	// Token: 0x06000682 RID: 1666 RVA: 0x000864E4 File Offset: 0x000846E4
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

	// Token: 0x06000683 RID: 1667 RVA: 0x000865C8 File Offset: 0x000847C8
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

	// Token: 0x06000684 RID: 1668 RVA: 0x00086874 File Offset: 0x00084A74
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

	// Token: 0x06000685 RID: 1669 RVA: 0x00034C8F File Offset: 0x00032E8F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private float _MoveTowards(float current, float target, float maxDelta)
	{
		if (math.abs(target - current) > maxDelta)
		{
			return current + math.sign(target - current) * maxDelta;
		}
		return target;
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00086978 File Offset: 0x00084B78
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private float _SignedAngle(float3 from, float3 to, float3 axis)
	{
		float3 x = math.normalize(from);
		float3 y = math.normalize(to);
		float x2 = math.acos(math.dot(x, y));
		float num = math.sign(math.dot(math.cross(x, y), axis));
		return math.degrees(x2) * num;
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x00034CAA File Offset: 0x00032EAA
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private float3 _ProjectOnPlane(float3 vector, float3 planeNormal)
	{
		return vector - math.dot(vector, planeNormal) * planeNormal;
	}

	// Token: 0x04000793 RID: 1939
	[SerializeField]
	private RCHoverboard._SingleInputOption m_inputTurn = new RCHoverboard._SingleInputOption(RCHoverboard._EInputSource.StickX, new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 0f, 0f, 0f),
		new Keyframe(0.1f, 0f, 0f, 1.25f, 0f, 0f),
		new Keyframe(0.9f, 1f, 1.25f, 0f, 0f, 0f),
		new Keyframe(1f, 1f, 0f, 0f, 0f, 0f)
	}));

	// Token: 0x04000794 RID: 1940
	[SerializeField]
	private RCHoverboard._SingleInputOption m_inputThrustForward = new RCHoverboard._SingleInputOption(RCHoverboard._EInputSource.Trigger, AnimationCurves.EaseInCirc);

	// Token: 0x04000795 RID: 1941
	[SerializeField]
	private RCHoverboard._SingleInputOption m_inputThrustBack = new RCHoverboard._SingleInputOption(RCHoverboard._EInputSource.StickBack, new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 0f, 0f, 0f),
		new Keyframe(0.9f, 0f, 0f, 9.9999f, 0.5825f, 0.3767f),
		new Keyframe(1f, 1f, 9.9999f, 1f, 0f, 0f)
	}));

	// Token: 0x04000796 RID: 1942
	[SerializeField]
	private RCHoverboard._SingleInputOption m_inputJump = new RCHoverboard._SingleInputOption(RCHoverboard._EInputSource.PrimaryFaceButton, AnimationCurves.Linear);

	// Token: 0x04000797 RID: 1943
	[Tooltip("Desired hover height above ground from this transform's position.")]
	[SerializeField]
	private float m_hoverHeight = 0.2f;

	// Token: 0x04000798 RID: 1944
	[Tooltip("Upward force to maintain hover when below hoverHeight.")]
	[SerializeField]
	private float m_hoverForce = 200f;

	// Token: 0x04000799 RID: 1945
	[Tooltip("Damping factor to smooth out vertical movement.")]
	[SerializeField]
	private float m_hoverDamp = 5f;

	// Token: 0x0400079A RID: 1946
	[Tooltip("Upward impulse force for jump.")]
	[SerializeField]
	private float m_jumpForce = 3.5f;

	// Token: 0x0400079B RID: 1947
	private bool _hasJumped;

	// Token: 0x0400079C RID: 1948
	[SerializeField]
	[HideInInspector]
	private float m_maxForwardSpeed = 6f;

	// Token: 0x0400079D RID: 1949
	[SerializeField]
	[Tooltip("Time (seconds) to reach max forward speed from zero.")]
	private float m_forwardAccelTime = 2f;

	// Token: 0x0400079E RID: 1950
	[SerializeField]
	[HideInInspector]
	private float m_maxTurnRate = 720f;

	// Token: 0x0400079F RID: 1951
	[Tooltip("Time (seconds) to reach max turning rate.")]
	[SerializeField]
	private float m_turnAccelTime = 0.75f;

	// Token: 0x040007A0 RID: 1952
	[SerializeField]
	[HideInInspector]
	private float m_maxTiltAngle = 30f;

	// Token: 0x040007A1 RID: 1953
	[Tooltip("Time (seconds) to reach max tilt angle.")]
	[SerializeField]
	private float m_tiltTime = 0.1f;

	// Token: 0x040007A2 RID: 1954
	[Tooltip("Audio source for any motor or hover sound.")]
	[SerializeField]
	private AudioSource m_audioSource;

	// Token: 0x040007A3 RID: 1955
	[Tooltip("Looping motor/hover sound clip.")]
	[SerializeField]
	private AudioClip m_hoverSound;

	// Token: 0x040007A4 RID: 1956
	[Tooltip("Volume range for the hover sound (x = min, y = max).")]
	[SerializeField]
	private float2 m_hoverSoundVolumeMinMax = new float2(0.1f, 0.5f);

	// Token: 0x040007A5 RID: 1957
	[Tooltip("Time it takes for the volume to reach max value.")]
	[SerializeField]
	private float m_hoverSoundVolumeRampTime = 1f;

	// Token: 0x040007A6 RID: 1958
	private bool _hasAudioSource;

	// Token: 0x040007A7 RID: 1959
	private bool _hasHoverSound;

	// Token: 0x040007A8 RID: 1960
	private float _forwardAccel;

	// Token: 0x040007A9 RID: 1961
	private float _turnAccel;

	// Token: 0x040007AA RID: 1962
	private float _tiltAccel;

	// Token: 0x040007AB RID: 1963
	private float _currentTurnRate;

	// Token: 0x040007AC RID: 1964
	private float _currentTurnAngle;

	// Token: 0x040007AD RID: 1965
	private float _currentTiltAngle;

	// Token: 0x040007AE RID: 1966
	private float _motorLevel;

	// Token: 0x020000F8 RID: 248
	private enum _EInputSource
	{
		// Token: 0x040007B0 RID: 1968
		None,
		// Token: 0x040007B1 RID: 1969
		StickX,
		// Token: 0x040007B2 RID: 1970
		StickForward,
		// Token: 0x040007B3 RID: 1971
		StickBack,
		// Token: 0x040007B4 RID: 1972
		Trigger,
		// Token: 0x040007B5 RID: 1973
		PrimaryFaceButton
	}

	// Token: 0x020000F9 RID: 249
	[Serializable]
	private struct _SingleInputOption
	{
		// Token: 0x06000689 RID: 1673 RVA: 0x00034CBF File Offset: 0x00032EBF
		public _SingleInputOption(RCHoverboard._EInputSource source, AnimationCurve remapCurve)
		{
			this.source = new GTOption<StringEnum<RCHoverboard._EInputSource>>(source);
			this.remapCurve = new GTOption<AnimationCurve>(remapCurve);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00086BD4 File Offset: 0x00084DD4
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

		// Token: 0x040007B6 RID: 1974
		public GTOption<StringEnum<RCHoverboard._EInputSource>> source;

		// Token: 0x040007B7 RID: 1975
		public GTOption<AnimationCurve> remapCurve;
	}
}
