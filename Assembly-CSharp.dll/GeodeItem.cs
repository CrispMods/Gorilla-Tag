using System;
using GorillaTag;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020003E0 RID: 992
public class GeodeItem : TransferrableObject
{
	// Token: 0x0600180E RID: 6158 RVA: 0x0003F53E File Offset: 0x0003D73E
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.hasEffectsGameObject = (this.effectsGameObject != null);
		this.effectsHaveBeenPlayed = false;
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x0003F560 File Offset: 0x0003D760
	protected override void Start()
	{
		base.Start();
		this.itemState = TransferrableObject.ItemStates.State0;
		this.prevItemState = TransferrableObject.ItemStates.State0;
		this.InitToDefault();
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x0003F57C File Offset: 0x0003D77C
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.InitToDefault();
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x0003F591 File Offset: 0x0003D791
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		return base.OnRelease(zoneReleased, releasingHand) && this.itemState != TransferrableObject.ItemStates.State0 && !base.InHand();
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x0003F5B5 File Offset: 0x0003D7B5
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		base.OnGrab(pointGrabbed, grabbingHand);
		UnityEvent<GeodeItem> onGeodeGrabbed = this.OnGeodeGrabbed;
		if (onGeodeGrabbed == null)
		{
			return;
		}
		onGeodeGrabbed.Invoke(this);
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x000C9234 File Offset: 0x000C7434
	private void InitToDefault()
	{
		this.cooldownRemaining = 0f;
		this.effectsHaveBeenPlayed = false;
		if (this.hasEffectsGameObject)
		{
			this.effectsGameObject.SetActive(false);
		}
		this.geodeFullMesh.SetActive(true);
		for (int i = 0; i < this.geodeCrackedMeshes.Length; i++)
		{
			this.geodeCrackedMeshes[i].SetActive(false);
		}
		this.hitLastFrame = false;
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x000C929C File Offset: 0x000C749C
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		if (this.itemState == TransferrableObject.ItemStates.State1)
		{
			this.cooldownRemaining -= Time.deltaTime;
			if (this.cooldownRemaining <= 0f)
			{
				this.itemState = TransferrableObject.ItemStates.State0;
				this.OnItemStateChanged();
			}
			return;
		}
		if (this.velocityEstimator.linearVelocity.magnitude < this.minHitVelocity)
		{
			return;
		}
		if (base.InHand())
		{
			int num = Physics.SphereCastNonAlloc(this.geodeFullMesh.transform.position, this.sphereRayRadius * Mathf.Abs(this.geodeFullMesh.transform.lossyScale.x), this.geodeFullMesh.transform.TransformDirection(Vector3.forward), this.collidersHit, this.rayCastMaxDistance, this.collisionLayerMask, QueryTriggerInteraction.Collide);
			this.hitLastFrame = (num > 0);
		}
		if (!this.hitLastFrame)
		{
			return;
		}
		if (!GorillaParent.hasInstance)
		{
			return;
		}
		UnityEvent<GeodeItem> onGeodeCracked = this.OnGeodeCracked;
		if (onGeodeCracked != null)
		{
			onGeodeCracked.Invoke(this);
		}
		this.itemState = TransferrableObject.ItemStates.State1;
		this.cooldownRemaining = this.cooldown;
		this.index = (this.randomizeGeode ? this.RandomPickCrackedGeode() : 0);
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x0003F5D0 File Offset: 0x0003D7D0
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		this.currentItemState = this.itemState;
		if (this.currentItemState != this.prevItemState)
		{
			this.OnItemStateChanged();
		}
		this.prevItemState = this.currentItemState;
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x000C93C4 File Offset: 0x000C75C4
	private void OnItemStateChanged()
	{
		if (this.itemState == TransferrableObject.ItemStates.State0)
		{
			this.InitToDefault();
			return;
		}
		this.geodeFullMesh.SetActive(false);
		for (int i = 0; i < this.geodeCrackedMeshes.Length; i++)
		{
			this.geodeCrackedMeshes[i].SetActive(i == this.index);
		}
		RigContainer rigContainer;
		if (NetworkSystem.Instance.InRoom && GorillaGameManager.instance != null && !this.effectsHaveBeenPlayed && VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.LocalPlayer, out rigContainer))
		{
			rigContainer.Rig.netView.SendRPC("RPC_PlayGeodeEffect", RpcTarget.All, new object[]
			{
				this.geodeFullMesh.transform.position
			});
			this.effectsHaveBeenPlayed = true;
		}
		if (!NetworkSystem.Instance.InRoom && !this.effectsHaveBeenPlayed)
		{
			if (this.audioSource)
			{
				this.audioSource.GTPlay();
			}
			this.effectsHaveBeenPlayed = true;
		}
	}

	// Token: 0x06001817 RID: 6167 RVA: 0x0003F604 File Offset: 0x0003D804
	private int RandomPickCrackedGeode()
	{
		return UnityEngine.Random.Range(0, this.geodeCrackedMeshes.Length);
	}

	// Token: 0x04001AB7 RID: 6839
	[Tooltip("This GameObject will activate when the geode hits the ground with enough force.")]
	public GameObject effectsGameObject;

	// Token: 0x04001AB8 RID: 6840
	public LayerMask collisionLayerMask;

	// Token: 0x04001AB9 RID: 6841
	[Tooltip("Used to calculate velocity of the geode.")]
	public GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04001ABA RID: 6842
	public float cooldown = 5f;

	// Token: 0x04001ABB RID: 6843
	[Tooltip("The velocity of the geode must be greater than this value to activate the effect.")]
	public float minHitVelocity = 0.2f;

	// Token: 0x04001ABC RID: 6844
	[Tooltip("Geode's full mesh before cracking")]
	public GameObject geodeFullMesh;

	// Token: 0x04001ABD RID: 6845
	[Tooltip("Geode's cracked open half different meshes, picked randomly")]
	public GameObject[] geodeCrackedMeshes;

	// Token: 0x04001ABE RID: 6846
	[Tooltip("The distance between te geode and the layer mask to detect whether it hits it")]
	public float rayCastMaxDistance = 0.2f;

	// Token: 0x04001ABF RID: 6847
	[FormerlySerializedAs("collisionRadius")]
	public float sphereRayRadius = 0.05f;

	// Token: 0x04001AC0 RID: 6848
	[DebugReadout]
	private float cooldownRemaining;

	// Token: 0x04001AC1 RID: 6849
	[DebugReadout]
	private bool hitLastFrame;

	// Token: 0x04001AC2 RID: 6850
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04001AC3 RID: 6851
	public bool randomizeGeode = true;

	// Token: 0x04001AC4 RID: 6852
	public UnityEvent<GeodeItem> OnGeodeCracked;

	// Token: 0x04001AC5 RID: 6853
	public UnityEvent<GeodeItem> OnGeodeGrabbed;

	// Token: 0x04001AC6 RID: 6854
	private bool hasEffectsGameObject;

	// Token: 0x04001AC7 RID: 6855
	private bool effectsHaveBeenPlayed;

	// Token: 0x04001AC8 RID: 6856
	private RaycastHit hit;

	// Token: 0x04001AC9 RID: 6857
	private RaycastHit[] collidersHit = new RaycastHit[20];

	// Token: 0x04001ACA RID: 6858
	private TransferrableObject.ItemStates currentItemState;

	// Token: 0x04001ACB RID: 6859
	private TransferrableObject.ItemStates prevItemState;

	// Token: 0x04001ACC RID: 6860
	private int index;
}
