using System;
using GorillaTag;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020003EB RID: 1003
public class GeodeItem : TransferrableObject
{
	// Token: 0x06001858 RID: 6232 RVA: 0x00040828 File Offset: 0x0003EA28
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.hasEffectsGameObject = (this.effectsGameObject != null);
		this.effectsHaveBeenPlayed = false;
	}

	// Token: 0x06001859 RID: 6233 RVA: 0x0004084A File Offset: 0x0003EA4A
	protected override void Start()
	{
		base.Start();
		this.itemState = TransferrableObject.ItemStates.State0;
		this.prevItemState = TransferrableObject.ItemStates.State0;
		this.InitToDefault();
	}

	// Token: 0x0600185A RID: 6234 RVA: 0x00040866 File Offset: 0x0003EA66
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.InitToDefault();
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x0600185B RID: 6235 RVA: 0x0004087B File Offset: 0x0003EA7B
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		return base.OnRelease(zoneReleased, releasingHand) && this.itemState != TransferrableObject.ItemStates.State0 && !base.InHand();
	}

	// Token: 0x0600185C RID: 6236 RVA: 0x0004089F File Offset: 0x0003EA9F
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

	// Token: 0x0600185D RID: 6237 RVA: 0x000CBA5C File Offset: 0x000C9C5C
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

	// Token: 0x0600185E RID: 6238 RVA: 0x000CBAC4 File Offset: 0x000C9CC4
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

	// Token: 0x0600185F RID: 6239 RVA: 0x000408BA File Offset: 0x0003EABA
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

	// Token: 0x06001860 RID: 6240 RVA: 0x000CBBEC File Offset: 0x000C9DEC
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

	// Token: 0x06001861 RID: 6241 RVA: 0x000408EE File Offset: 0x0003EAEE
	private int RandomPickCrackedGeode()
	{
		return UnityEngine.Random.Range(0, this.geodeCrackedMeshes.Length);
	}

	// Token: 0x04001AFF RID: 6911
	[Tooltip("This GameObject will activate when the geode hits the ground with enough force.")]
	public GameObject effectsGameObject;

	// Token: 0x04001B00 RID: 6912
	public LayerMask collisionLayerMask;

	// Token: 0x04001B01 RID: 6913
	[Tooltip("Used to calculate velocity of the geode.")]
	public GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04001B02 RID: 6914
	public float cooldown = 5f;

	// Token: 0x04001B03 RID: 6915
	[Tooltip("The velocity of the geode must be greater than this value to activate the effect.")]
	public float minHitVelocity = 0.2f;

	// Token: 0x04001B04 RID: 6916
	[Tooltip("Geode's full mesh before cracking")]
	public GameObject geodeFullMesh;

	// Token: 0x04001B05 RID: 6917
	[Tooltip("Geode's cracked open half different meshes, picked randomly")]
	public GameObject[] geodeCrackedMeshes;

	// Token: 0x04001B06 RID: 6918
	[Tooltip("The distance between te geode and the layer mask to detect whether it hits it")]
	public float rayCastMaxDistance = 0.2f;

	// Token: 0x04001B07 RID: 6919
	[FormerlySerializedAs("collisionRadius")]
	public float sphereRayRadius = 0.05f;

	// Token: 0x04001B08 RID: 6920
	[DebugReadout]
	private float cooldownRemaining;

	// Token: 0x04001B09 RID: 6921
	[DebugReadout]
	private bool hitLastFrame;

	// Token: 0x04001B0A RID: 6922
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04001B0B RID: 6923
	public bool randomizeGeode = true;

	// Token: 0x04001B0C RID: 6924
	public UnityEvent<GeodeItem> OnGeodeCracked;

	// Token: 0x04001B0D RID: 6925
	public UnityEvent<GeodeItem> OnGeodeGrabbed;

	// Token: 0x04001B0E RID: 6926
	private bool hasEffectsGameObject;

	// Token: 0x04001B0F RID: 6927
	private bool effectsHaveBeenPlayed;

	// Token: 0x04001B10 RID: 6928
	private RaycastHit hit;

	// Token: 0x04001B11 RID: 6929
	private RaycastHit[] collidersHit = new RaycastHit[20];

	// Token: 0x04001B12 RID: 6930
	private TransferrableObject.ItemStates currentItemState;

	// Token: 0x04001B13 RID: 6931
	private TransferrableObject.ItemStates prevItemState;

	// Token: 0x04001B14 RID: 6932
	private int index;
}
