﻿using System;
using GorillaExtensions;
using GorillaLocomotion.Swimming;
using GorillaTag;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000370 RID: 880
public class BalloonHoldable : TransferrableObject, IFXContext
{
	// Token: 0x06001477 RID: 5239 RVA: 0x0006470C File Offset: 0x0006290C
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

	// Token: 0x06001478 RID: 5240 RVA: 0x00064765 File Offset: 0x00062965
	protected override void Start()
	{
		base.Start();
		this.EnableDynamics(false, false, false);
	}

	// Token: 0x06001479 RID: 5241 RVA: 0x00064778 File Offset: 0x00062978
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

	// Token: 0x0600147A RID: 5242 RVA: 0x000647EA File Offset: 0x000629EA
	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		if (this.worldShareableInstance != null)
		{
			return;
		}
		base.SpawnTransferableObjectViews();
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x00064807 File Offset: 0x00062A07
	private bool ShouldSimulate()
	{
		return !base.Attached() && this.balloonState == BalloonHoldable.BalloonStates.Normal;
	}

	// Token: 0x0600147C RID: 5244 RVA: 0x0006481C File Offset: 0x00062A1C
	internal override void OnDisable()
	{
		base.OnDisable();
		this.lineRenderer.enabled = false;
		this.EnableDynamics(false, false, false);
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x00064839 File Offset: 0x00062A39
	public override void PreDisable()
	{
		this.originalOwner = null;
		base.PreDisable();
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x00064848 File Offset: 0x00062A48
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.balloonState = BalloonHoldable.BalloonStates.Normal;
		base.transform.localScale = Vector3.one;
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x00064868 File Offset: 0x00062A68
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

	// Token: 0x06001480 RID: 5248 RVA: 0x000648C8 File Offset: 0x00062AC8
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

	// Token: 0x06001481 RID: 5249 RVA: 0x00064946 File Offset: 0x00062B46
	protected override void PlayDestroyedOrDisabledEffect()
	{
		base.PlayDestroyedOrDisabledEffect();
		this.PlayPopBalloonFX();
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x00064954 File Offset: 0x00062B54
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

	// Token: 0x06001483 RID: 5251 RVA: 0x00064988 File Offset: 0x00062B88
	private void PlayPopBalloonFX()
	{
		FXSystem.PlayFXForRig(FXType.BalloonPop, this, default(PhotonMessageInfoWrapped));
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x000649A8 File Offset: 0x00062BA8
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

	// Token: 0x06001485 RID: 5253 RVA: 0x00064A14 File Offset: 0x00062C14
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

	// Token: 0x06001486 RID: 5254 RVA: 0x00064B0D File Offset: 0x00062D0D
	public void PopBalloonRemote()
	{
		if (this.ShouldSimulate())
		{
			this.balloonState = BalloonHoldable.BalloonStates.Pop;
		}
	}

	// Token: 0x06001487 RID: 5255 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnOwnerChangeCb(NetPlayer newOwner, NetPlayer prevOwner)
	{
	}

	// Token: 0x06001488 RID: 5256 RVA: 0x00064B20 File Offset: 0x00062D20
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

	// Token: 0x06001489 RID: 5257 RVA: 0x00064BD0 File Offset: 0x00062DD0
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

	// Token: 0x0600148A RID: 5258 RVA: 0x00064C18 File Offset: 0x00062E18
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

	// Token: 0x0600148B RID: 5259 RVA: 0x00064E30 File Offset: 0x00063030
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

	// Token: 0x0600148C RID: 5260 RVA: 0x00064E90 File Offset: 0x00063090
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

	// Token: 0x0600148D RID: 5261 RVA: 0x00064FB9 File Offset: 0x000631B9
	protected override void LateUpdateReplicated()
	{
		base.LateUpdateReplicated();
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x00064FC4 File Offset: 0x000631C4
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

	// Token: 0x0600148F RID: 5263 RVA: 0x00065058 File Offset: 0x00063258
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

	// Token: 0x06001490 RID: 5264 RVA: 0x000650E4 File Offset: 0x000632E4
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

	// Token: 0x06001491 RID: 5265 RVA: 0x000651A8 File Offset: 0x000633A8
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

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06001492 RID: 5266 RVA: 0x00065220 File Offset: 0x00063420
	FXSystemSettings IFXContext.settings
	{
		get
		{
			return this.ownerRig.fxSettings;
		}
	}

	// Token: 0x06001493 RID: 5267 RVA: 0x00065230 File Offset: 0x00063430
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

	// Token: 0x040016AF RID: 5807
	private ITetheredObjectBehavior balloonDynamics;

	// Token: 0x040016B0 RID: 5808
	[SerializeField]
	private Renderer mesh;

	// Token: 0x040016B1 RID: 5809
	private LineRenderer lineRenderer;

	// Token: 0x040016B2 RID: 5810
	private Rigidbody rb;

	// Token: 0x040016B3 RID: 5811
	private NetPlayer originalOwner;

	// Token: 0x040016B4 RID: 5812
	public GameObject balloonPopFXPrefab;

	// Token: 0x040016B5 RID: 5813
	public Color balloonPopFXColor;

	// Token: 0x040016B6 RID: 5814
	private float timer;

	// Token: 0x040016B7 RID: 5815
	public float scaleTimerLength = 2f;

	// Token: 0x040016B8 RID: 5816
	public float poppedTimerLength = 2.5f;

	// Token: 0x040016B9 RID: 5817
	public float beginScale = 0.1f;

	// Token: 0x040016BA RID: 5818
	public float bopSpeed = 1f;

	// Token: 0x040016BB RID: 5819
	private Vector3 fullyInflatedScale;

	// Token: 0x040016BC RID: 5820
	public AudioSource balloonBopSource;

	// Token: 0x040016BD RID: 5821
	public AudioSource balloonInflatSource;

	// Token: 0x040016BE RID: 5822
	private Vector3 forceAppliedAsRemote;

	// Token: 0x040016BF RID: 5823
	private Vector3 collisionPtAsRemote;

	// Token: 0x040016C0 RID: 5824
	private WaterVolume waterVolume;

	// Token: 0x040016C1 RID: 5825
	[DebugReadout]
	private BalloonHoldable.BalloonStates balloonState;

	// Token: 0x040016C2 RID: 5826
	private float returnTimer;

	// Token: 0x040016C3 RID: 5827
	[SerializeField]
	private float maxDistanceFromOwner;

	// Token: 0x040016C4 RID: 5828
	public float lastOwnershipRequest;

	// Token: 0x040016C5 RID: 5829
	[SerializeField]
	private bool disableCollisionHandling;

	// Token: 0x040016C6 RID: 5830
	[SerializeField]
	private bool disableRelease;

	// Token: 0x02000371 RID: 881
	private enum BalloonStates
	{
		// Token: 0x040016C8 RID: 5832
		Normal,
		// Token: 0x040016C9 RID: 5833
		Pop,
		// Token: 0x040016CA RID: 5834
		Waiting,
		// Token: 0x040016CB RID: 5835
		WaitForOwnershipTransfer,
		// Token: 0x040016CC RID: 5836
		WaitForReDock,
		// Token: 0x040016CD RID: 5837
		Refilling,
		// Token: 0x040016CE RID: 5838
		Returning
	}
}
