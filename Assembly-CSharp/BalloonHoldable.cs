using System;
using GorillaExtensions;
using GorillaLocomotion.Swimming;
using GorillaTag;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200037B RID: 891
public class BalloonHoldable : TransferrableObject, IFXContext
{
	// Token: 0x060014C3 RID: 5315 RVA: 0x000BD7FC File Offset: 0x000BB9FC
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.balloonDynamics = base.GetComponent<ITetheredObjectBehavior>();
		if (this.mesh == null)
		{
			this.mesh = base.GetComponent<Renderer>();
		}
		this.lineRenderer = base.GetComponent<LineRenderer>();
		this.itemState = (TransferrableObject.ItemStates)0;
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060014C4 RID: 5316 RVA: 0x0003DFAE File Offset: 0x0003C1AE
	protected override void Start()
	{
		base.Start();
		this.EnableDynamics(false, false, false);
	}

	// Token: 0x060014C5 RID: 5317 RVA: 0x000BD858 File Offset: 0x000BBA58
	internal override void OnEnable()
	{
		base.OnEnable();
		this.EnableDynamics(false, false, false);
		this.mesh.enabled = true;
		this.lineRenderer.enabled = false;
		if (NetworkSystem.Instance.InRoom)
		{
			if (this.worldShareableInstance != null)
			{
				return;
			}
			base.SpawnTransferableObjectViews();
		}
		if (base.InHand())
		{
			this.Grab();
			return;
		}
		if (base.Dropped())
		{
			this.Release();
		}
	}

	// Token: 0x060014C6 RID: 5318 RVA: 0x0003DFBF File Offset: 0x0003C1BF
	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		if (this.worldShareableInstance != null)
		{
			return;
		}
		base.SpawnTransferableObjectViews();
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x0003DFDC File Offset: 0x0003C1DC
	private bool ShouldSimulate()
	{
		return !base.Attached() && this.balloonState == BalloonHoldable.BalloonStates.Normal;
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x0003DFF1 File Offset: 0x0003C1F1
	internal override void OnDisable()
	{
		base.OnDisable();
		this.lineRenderer.enabled = false;
		this.EnableDynamics(false, false, false);
	}

	// Token: 0x060014C9 RID: 5321 RVA: 0x0003E00E File Offset: 0x0003C20E
	public override void PreDisable()
	{
		this.originalOwner = null;
		base.PreDisable();
	}

	// Token: 0x060014CA RID: 5322 RVA: 0x0003E01D File Offset: 0x0003C21D
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.balloonState = BalloonHoldable.BalloonStates.Normal;
		base.transform.localScale = Vector3.one;
	}

	// Token: 0x060014CB RID: 5323 RVA: 0x000BD8CC File Offset: 0x000BBACC
	protected override void OnWorldShareableItemSpawn()
	{
		WorldShareableItem worldShareableInstance = this.worldShareableInstance;
		if (worldShareableInstance != null)
		{
			worldShareableInstance.rpcCallBack = new Action(this.PopBalloonRemote);
			worldShareableInstance.onOwnerChangeCb = new WorldShareableItem.OnOwnerChangeDelegate(this.OnOwnerChangeCb);
			worldShareableInstance.EnableRemoteSync = this.ShouldSimulate();
		}
		this.originalOwner = worldShareableInstance.target.owner;
	}

	// Token: 0x060014CC RID: 5324 RVA: 0x000BD92C File Offset: 0x000BBB2C
	public override void ResetToHome()
	{
		if (base.IsLocalObject() && this.worldShareableInstance != null && !this.worldShareableInstance.guard.isTrulyMine)
		{
			PhotonView photonView = PhotonView.Get(this.worldShareableInstance);
			if (photonView != null)
			{
				photonView.RPC("RPCWorldShareable", RpcTarget.Others, Array.Empty<object>());
			}
			this.worldShareableInstance.guard.RequestOwnershipImmediatelyWithGuaranteedAuthority();
		}
		this.PopBalloon();
		this.balloonState = BalloonHoldable.BalloonStates.WaitForReDock;
		base.ResetToHome();
	}

	// Token: 0x060014CD RID: 5325 RVA: 0x0003E03C File Offset: 0x0003C23C
	protected override void PlayDestroyedOrDisabledEffect()
	{
		base.PlayDestroyedOrDisabledEffect();
		this.PlayPopBalloonFX();
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x0003E04A File Offset: 0x0003C24A
	protected override void OnItemDestroyedOrDisabled()
	{
		this.PlayPopBalloonFX();
		if (this.balloonDynamics != null)
		{
			this.balloonDynamics.ReParent();
		}
		base.transform.parent = base.DefaultAnchor();
		base.OnItemDestroyedOrDisabled();
	}

	// Token: 0x060014CF RID: 5327 RVA: 0x000BD9AC File Offset: 0x000BBBAC
	private void PlayPopBalloonFX()
	{
		FXSystem.PlayFXForRig(FXType.BalloonPop, this, default(PhotonMessageInfoWrapped));
	}

	// Token: 0x060014D0 RID: 5328 RVA: 0x000BD9CC File Offset: 0x000BBBCC
	private void EnableDynamics(bool enable, bool collider, bool forceKinematicOn = false)
	{
		bool kinematic = false;
		if (forceKinematicOn)
		{
			kinematic = true;
		}
		else if (NetworkSystem.Instance.InRoom && this.worldShareableInstance != null)
		{
			PhotonView photonView = PhotonView.Get(this.worldShareableInstance.gameObject);
			if (photonView != null && !photonView.IsMine)
			{
				kinematic = true;
			}
		}
		if (this.balloonDynamics != null)
		{
			this.balloonDynamics.EnableDynamics(enable, collider, kinematic);
		}
	}

	// Token: 0x060014D1 RID: 5329 RVA: 0x000BDA38 File Offset: 0x000BBC38
	private void PopBalloon()
	{
		this.PlayPopBalloonFX();
		this.EnableDynamics(false, false, false);
		this.mesh.enabled = false;
		this.lineRenderer.enabled = false;
		if (this.gripInteractor != null)
		{
			this.gripInteractor.gameObject.SetActive(false);
		}
		if ((object.Equals(this.originalOwner, PhotonNetwork.LocalPlayer) || !NetworkSystem.Instance.InRoom) && NetworkSystem.Instance.InRoom && this.worldShareableInstance != null && !this.worldShareableInstance.guard.isTrulyMine)
		{
			this.worldShareableInstance.guard.RequestOwnershipImmediatelyWithGuaranteedAuthority();
		}
		if (this.balloonDynamics != null)
		{
			this.balloonDynamics.ReParent();
			this.EnableDynamics(false, false, false);
		}
		if (this.IsMyItem())
		{
			if (base.InLeftHand())
			{
				EquipmentInteractor.instance.ReleaseLeftHand();
			}
			if (base.InRightHand())
			{
				EquipmentInteractor.instance.ReleaseRightHand();
			}
		}
	}

	// Token: 0x060014D2 RID: 5330 RVA: 0x0003E07C File Offset: 0x0003C27C
	public void PopBalloonRemote()
	{
		if (this.ShouldSimulate())
		{
			this.balloonState = BalloonHoldable.BalloonStates.Pop;
		}
	}

	// Token: 0x060014D3 RID: 5331 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnOwnerChangeCb(NetPlayer newOwner, NetPlayer prevOwner)
	{
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x000BDB34 File Offset: 0x000BBD34
	public override void OnOwnershipTransferred(NetPlayer newOwner, NetPlayer prevOwner)
	{
		base.OnOwnershipTransferred(newOwner, prevOwner);
		if (object.Equals(prevOwner, NetworkSystem.Instance.LocalPlayer) && newOwner == null)
		{
			return;
		}
		if (!object.Equals(newOwner, NetworkSystem.Instance.LocalPlayer))
		{
			this.EnableDynamics(false, true, true);
			return;
		}
		if (this.ShouldSimulate() && this.balloonDynamics != null)
		{
			this.balloonDynamics.EnableDynamics(true, true, false);
		}
		if (!this.rb)
		{
			return;
		}
		if (!this.rb.isKinematic)
		{
			this.rb.AddForceAtPosition(this.forceAppliedAsRemote, this.collisionPtAsRemote, ForceMode.VelocityChange);
		}
		this.forceAppliedAsRemote = Vector3.zero;
		this.collisionPtAsRemote = Vector3.zero;
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x000BDBE4 File Offset: 0x000BBDE4
	private void OwnerPopBalloon()
	{
		if (this.worldShareableInstance != null)
		{
			PhotonView photonView = PhotonView.Get(this.worldShareableInstance);
			if (photonView != null)
			{
				photonView.RPC("RPCWorldShareable", RpcTarget.Others, Array.Empty<object>());
			}
		}
		this.balloonState = BalloonHoldable.BalloonStates.Pop;
	}

	// Token: 0x060014D6 RID: 5334 RVA: 0x000BDC2C File Offset: 0x000BBE2C
	private void RunLocalPopSM()
	{
		switch (this.balloonState)
		{
		case BalloonHoldable.BalloonStates.Normal:
			break;
		case BalloonHoldable.BalloonStates.Pop:
			this.timer = Time.time;
			this.PopBalloon();
			this.balloonState = BalloonHoldable.BalloonStates.WaitForOwnershipTransfer;
			this.lastOwnershipRequest = Time.time;
			return;
		case BalloonHoldable.BalloonStates.Waiting:
			if (Time.time - this.timer >= this.poppedTimerLength)
			{
				this.timer = Time.time;
				this.mesh.enabled = true;
				this.balloonInflatSource.GTPlay();
				this.balloonState = BalloonHoldable.BalloonStates.Refilling;
				return;
			}
			base.transform.localScale = new Vector3(this.beginScale, this.beginScale, this.beginScale);
			return;
		case BalloonHoldable.BalloonStates.WaitForOwnershipTransfer:
			if (!NetworkSystem.Instance.InRoom)
			{
				this.balloonState = BalloonHoldable.BalloonStates.WaitForReDock;
				base.ReDock();
				return;
			}
			if (this.worldShareableInstance != null)
			{
				WorldShareableItem worldShareableInstance = this.worldShareableInstance;
				NetPlayer owner = worldShareableInstance.Owner;
				if (worldShareableInstance != null && owner == this.originalOwner)
				{
					this.balloonState = BalloonHoldable.BalloonStates.WaitForReDock;
					base.ReDock();
				}
				if (base.IsLocalObject() && this.lastOwnershipRequest + 5f < Time.time)
				{
					this.worldShareableInstance.guard.RequestOwnershipImmediatelyWithGuaranteedAuthority();
					this.lastOwnershipRequest = Time.time;
					return;
				}
			}
			break;
		case BalloonHoldable.BalloonStates.WaitForReDock:
			if (base.Attached())
			{
				this.fullyInflatedScale = base.transform.localScale;
				base.ReDock();
				this.balloonState = BalloonHoldable.BalloonStates.Waiting;
				return;
			}
			break;
		case BalloonHoldable.BalloonStates.Refilling:
		{
			float num = Time.time - this.timer;
			if (num >= this.scaleTimerLength)
			{
				base.transform.localScale = this.fullyInflatedScale;
				this.balloonState = BalloonHoldable.BalloonStates.Normal;
				if (this.gripInteractor != null)
				{
					this.gripInteractor.gameObject.SetActive(true);
				}
			}
			num = Mathf.Clamp01(num / this.scaleTimerLength);
			float d = Mathf.Lerp(this.beginScale, 1f, num);
			base.transform.localScale = this.fullyInflatedScale * d;
			return;
		}
		case BalloonHoldable.BalloonStates.Returning:
			if (this.balloonDynamics.ReturnStep())
			{
				this.balloonState = BalloonHoldable.BalloonStates.Normal;
				base.ReDock();
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060014D7 RID: 5335 RVA: 0x000BDE44 File Offset: 0x000BC044
	protected override void OnStateChanged()
	{
		if (base.InHand())
		{
			this.Grab();
			return;
		}
		if (base.Dropped())
		{
			this.Release();
			return;
		}
		if (base.OnShoulder())
		{
			if (this.balloonDynamics != null && this.balloonDynamics.IsEnabled())
			{
				this.EnableDynamics(false, false, false);
			}
			this.lineRenderer.enabled = false;
		}
	}

	// Token: 0x060014D8 RID: 5336 RVA: 0x000BDEA4 File Offset: 0x000BC0A4
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		if (Time.frameCount == this.enabledOnFrame)
		{
			this.OnStateChanged();
		}
		if (base.InHand() && this.detatchOnGrab)
		{
			float d = (this.targetRig != null) ? this.targetRig.transform.localScale.x : 1f;
			base.transform.localScale = Vector3.one * d;
		}
		if (base.Dropped() && this.balloonState == BalloonHoldable.BalloonStates.Normal && this.maxDistanceFromOwner > 0f && (!NetworkSystem.Instance.InRoom || this.originalOwner.IsLocal) && (VRRig.LocalRig.transform.position - base.transform.position).IsLongerThan(this.maxDistanceFromOwner * base.transform.localScale.x))
		{
			this.OwnerPopBalloon();
		}
		if (this.worldShareableInstance != null && !this.worldShareableInstance.guard.isMine)
		{
			this.worldShareableInstance.EnableRemoteSync = this.ShouldSimulate();
		}
		if (this.balloonState != BalloonHoldable.BalloonStates.Normal)
		{
			this.RunLocalPopSM();
		}
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x0003E08D File Offset: 0x0003C28D
	protected override void LateUpdateReplicated()
	{
		base.LateUpdateReplicated();
	}

	// Token: 0x060014DA RID: 5338 RVA: 0x000BDFD0 File Offset: 0x000BC1D0
	private void Grab()
	{
		if (this.balloonDynamics == null)
		{
			return;
		}
		if (this.detatchOnGrab)
		{
			float num = (this.targetRig != null) ? this.targetRig.transform.localScale.x : 1f;
			base.transform.localScale = Vector3.one * num;
			this.EnableDynamics(true, true, false);
			this.balloonDynamics.EnableDistanceConstraints(true, num);
			this.lineRenderer.enabled = true;
			return;
		}
		base.transform.localScale = Vector3.one;
	}

	// Token: 0x060014DB RID: 5339 RVA: 0x000BE064 File Offset: 0x000BC264
	private void Release()
	{
		if (this.disableRelease)
		{
			this.balloonState = BalloonHoldable.BalloonStates.Returning;
			return;
		}
		if (this.balloonDynamics == null)
		{
			return;
		}
		float num = (this.targetRig != null) ? this.targetRig.transform.localScale.x : 1f;
		base.transform.localScale = Vector3.one * num;
		this.EnableDynamics(true, true, false);
		this.balloonDynamics.EnableDistanceConstraints(false, num);
		this.lineRenderer.enabled = false;
	}

	// Token: 0x060014DC RID: 5340 RVA: 0x000BE0F0 File Offset: 0x000BC2F0
	public void OnTriggerEnter(Collider other)
	{
		if (!this.ShouldSimulate())
		{
			return;
		}
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		bool flag = false;
		if (this.balloonDynamics != null)
		{
			this.balloonDynamics.TriggerEnter(other, ref zero, ref zero2, ref flag);
		}
		if (!NetworkSystem.Instance.InRoom)
		{
			return;
		}
		if (this.worldShareableInstance == null)
		{
			return;
		}
		if (flag)
		{
			RequestableOwnershipGuard component = PhotonView.Get(this.worldShareableInstance.gameObject).GetComponent<RequestableOwnershipGuard>();
			if (!component.isTrulyMine)
			{
				if (zero.magnitude > this.forceAppliedAsRemote.magnitude)
				{
					this.forceAppliedAsRemote = zero;
					this.collisionPtAsRemote = zero2;
				}
				component.RequestOwnershipImmediately(delegate
				{
				});
			}
		}
	}

	// Token: 0x060014DD RID: 5341 RVA: 0x000BE1B4 File Offset: 0x000BC3B4
	public void OnCollisionEnter(Collision collision)
	{
		if (!this.ShouldSimulate() || this.disableCollisionHandling)
		{
			return;
		}
		this.balloonBopSource.GTPlay();
		if (!collision.gameObject.IsOnLayer(UnityLayer.GorillaThrowable))
		{
			return;
		}
		if (!NetworkSystem.Instance.InRoom)
		{
			this.OwnerPopBalloon();
			return;
		}
		if (this.worldShareableInstance == null)
		{
			return;
		}
		if (PhotonView.Get(this.worldShareableInstance.gameObject).IsMine)
		{
			this.OwnerPopBalloon();
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x060014DE RID: 5342 RVA: 0x0003E095 File Offset: 0x0003C295
	FXSystemSettings IFXContext.settings
	{
		get
		{
			return this.ownerRig.fxSettings;
		}
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x000BE22C File Offset: 0x000BC42C
	void IFXContext.OnPlayFX()
	{
		GameObject gameObject = ObjectPools.instance.Instantiate(this.balloonPopFXPrefab);
		gameObject.transform.SetPositionAndRotation(base.transform.position, base.transform.rotation);
		GorillaColorizableBase componentInChildren = gameObject.GetComponentInChildren<GorillaColorizableBase>();
		if (componentInChildren != null)
		{
			componentInChildren.SetColor(this.balloonPopFXColor);
		}
	}

	// Token: 0x040016F7 RID: 5879
	private ITetheredObjectBehavior balloonDynamics;

	// Token: 0x040016F8 RID: 5880
	[SerializeField]
	private Renderer mesh;

	// Token: 0x040016F9 RID: 5881
	private LineRenderer lineRenderer;

	// Token: 0x040016FA RID: 5882
	private Rigidbody rb;

	// Token: 0x040016FB RID: 5883
	private NetPlayer originalOwner;

	// Token: 0x040016FC RID: 5884
	public GameObject balloonPopFXPrefab;

	// Token: 0x040016FD RID: 5885
	public Color balloonPopFXColor;

	// Token: 0x040016FE RID: 5886
	private float timer;

	// Token: 0x040016FF RID: 5887
	public float scaleTimerLength = 2f;

	// Token: 0x04001700 RID: 5888
	public float poppedTimerLength = 2.5f;

	// Token: 0x04001701 RID: 5889
	public float beginScale = 0.1f;

	// Token: 0x04001702 RID: 5890
	public float bopSpeed = 1f;

	// Token: 0x04001703 RID: 5891
	private Vector3 fullyInflatedScale;

	// Token: 0x04001704 RID: 5892
	public AudioSource balloonBopSource;

	// Token: 0x04001705 RID: 5893
	public AudioSource balloonInflatSource;

	// Token: 0x04001706 RID: 5894
	private Vector3 forceAppliedAsRemote;

	// Token: 0x04001707 RID: 5895
	private Vector3 collisionPtAsRemote;

	// Token: 0x04001708 RID: 5896
	private WaterVolume waterVolume;

	// Token: 0x04001709 RID: 5897
	[DebugReadout]
	private BalloonHoldable.BalloonStates balloonState;

	// Token: 0x0400170A RID: 5898
	private float returnTimer;

	// Token: 0x0400170B RID: 5899
	[SerializeField]
	private float maxDistanceFromOwner;

	// Token: 0x0400170C RID: 5900
	public float lastOwnershipRequest;

	// Token: 0x0400170D RID: 5901
	[SerializeField]
	private bool disableCollisionHandling;

	// Token: 0x0400170E RID: 5902
	[SerializeField]
	private bool disableRelease;

	// Token: 0x0200037C RID: 892
	private enum BalloonStates
	{
		// Token: 0x04001710 RID: 5904
		Normal,
		// Token: 0x04001711 RID: 5905
		Pop,
		// Token: 0x04001712 RID: 5906
		Waiting,
		// Token: 0x04001713 RID: 5907
		WaitForOwnershipTransfer,
		// Token: 0x04001714 RID: 5908
		WaitForReDock,
		// Token: 0x04001715 RID: 5909
		Refilling,
		// Token: 0x04001716 RID: 5910
		Returning
	}
}
