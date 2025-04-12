using System;
using GorillaExtensions;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200017E RID: 382
public class TriggerOnJump : MonoBehaviour, ITickSystemTick
{
	// Token: 0x06000992 RID: 2450 RVA: 0x00090664 File Offset: 0x0008E864
	private void OnEnable()
	{
		if (this.myRig.IsNull())
		{
			this.myRig = base.GetComponentInParent<VRRig>();
		}
		if (this._events == null && this.myRig != null && this.myRig.Creator != null)
		{
			this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
			this._events.Init(this.myRig.creator);
		}
		if (this._events != null)
		{
			this._events.Activate += this.OnActivate;
		}
		bool flag = !PhotonNetwork.InRoom && this.myRig != null && this.myRig.isOfflineVRRig;
		RigContainer rigContainer;
		bool flag2 = PhotonNetwork.InRoom && this.myRig != null && VRRigCache.Instance.TryGetVrrig(PhotonNetwork.LocalPlayer, out rigContainer) && rigContainer != null && rigContainer.Rig != null && rigContainer.Rig == this.myRig;
		if (flag || flag2)
		{
			TickSystem<object>.AddCallbackTarget(this);
		}
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0009078C File Offset: 0x0008E98C
	private void OnDisable()
	{
		TickSystem<object>.RemoveCallbackTarget(this);
		this.playerOnGround = false;
		this.jumpStartTime = 0f;
		this.lastActivationTime = 0f;
		this.waitingForGrounding = false;
		if (this._events != null)
		{
			this._events.Activate -= this.OnActivate;
			UnityEngine.Object.Destroy(this._events);
			this._events = null;
		}
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x00035C9B File Offset: 0x00033E9B
	private void OnActivate(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "OnJumpActivate");
		if (info.senderID != this.myRig.creator.ActorNumber)
		{
			return;
		}
		if (sender != target)
		{
			return;
		}
		this.onJumping.Invoke();
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x00090808 File Offset: 0x0008EA08
	public void Tick()
	{
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null)
		{
			bool flag = this.playerOnGround;
			this.playerOnGround = (instance.BodyOnGround || instance.IsHandTouching(true) || instance.IsHandTouching(false));
			float time = Time.time;
			if (this.playerOnGround)
			{
				this.waitingForGrounding = false;
			}
			if (!this.playerOnGround && flag)
			{
				this.jumpStartTime = time;
			}
			if (!this.playerOnGround && !this.waitingForGrounding && instance.RigidbodyVelocity.sqrMagnitude > this.minJumpStrength * this.minJumpStrength && instance.RigidbodyVelocity.y > this.minJumpVertical && time > this.jumpStartTime + this.minJumpTime)
			{
				this.waitingForGrounding = true;
				if (time > this.lastActivationTime + this.cooldownTime)
				{
					this.lastActivationTime = time;
					if (PhotonNetwork.InRoom)
					{
						this._events.Activate.RaiseAll(Array.Empty<object>());
						return;
					}
					this.onJumping.Invoke();
				}
			}
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000996 RID: 2454 RVA: 0x00035CD3 File Offset: 0x00033ED3
	// (set) Token: 0x06000997 RID: 2455 RVA: 0x00035CDB File Offset: 0x00033EDB
	public bool TickRunning { get; set; }

	// Token: 0x04000B8F RID: 2959
	[SerializeField]
	private float minJumpStrength = 1f;

	// Token: 0x04000B90 RID: 2960
	[SerializeField]
	private float minJumpVertical = 1f;

	// Token: 0x04000B91 RID: 2961
	[SerializeField]
	private float cooldownTime = 1f;

	// Token: 0x04000B92 RID: 2962
	[SerializeField]
	private UnityEvent onJumping;

	// Token: 0x04000B93 RID: 2963
	private RubberDuckEvents _events;

	// Token: 0x04000B94 RID: 2964
	private bool playerOnGround;

	// Token: 0x04000B95 RID: 2965
	private float minJumpTime = 0.05f;

	// Token: 0x04000B96 RID: 2966
	private bool waitingForGrounding;

	// Token: 0x04000B97 RID: 2967
	private float jumpStartTime;

	// Token: 0x04000B98 RID: 2968
	private float lastActivationTime;

	// Token: 0x04000B99 RID: 2969
	private VRRig myRig;
}
