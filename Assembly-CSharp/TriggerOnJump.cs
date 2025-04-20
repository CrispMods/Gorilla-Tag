using System;
using GorillaExtensions;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000189 RID: 393
public class TriggerOnJump : MonoBehaviour, ITickSystemTick
{
	// Token: 0x060009DC RID: 2524 RVA: 0x00092F58 File Offset: 0x00091158
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

	// Token: 0x060009DD RID: 2525 RVA: 0x00093080 File Offset: 0x00091280
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

	// Token: 0x060009DE RID: 2526 RVA: 0x00036F5B File Offset: 0x0003515B
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

	// Token: 0x060009DF RID: 2527 RVA: 0x000930FC File Offset: 0x000912FC
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

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x060009E0 RID: 2528 RVA: 0x00036F93 File Offset: 0x00035193
	// (set) Token: 0x060009E1 RID: 2529 RVA: 0x00036F9B File Offset: 0x0003519B
	public bool TickRunning { get; set; }

	// Token: 0x04000BD4 RID: 3028
	[SerializeField]
	private float minJumpStrength = 1f;

	// Token: 0x04000BD5 RID: 3029
	[SerializeField]
	private float minJumpVertical = 1f;

	// Token: 0x04000BD6 RID: 3030
	[SerializeField]
	private float cooldownTime = 1f;

	// Token: 0x04000BD7 RID: 3031
	[SerializeField]
	private UnityEvent onJumping;

	// Token: 0x04000BD8 RID: 3032
	private RubberDuckEvents _events;

	// Token: 0x04000BD9 RID: 3033
	private bool playerOnGround;

	// Token: 0x04000BDA RID: 3034
	private float minJumpTime = 0.05f;

	// Token: 0x04000BDB RID: 3035
	private bool waitingForGrounding;

	// Token: 0x04000BDC RID: 3036
	private float jumpStartTime;

	// Token: 0x04000BDD RID: 3037
	private float lastActivationTime;

	// Token: 0x04000BDE RID: 3038
	private VRRig myRig;
}
