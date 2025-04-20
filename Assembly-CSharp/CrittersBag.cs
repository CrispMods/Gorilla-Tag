using System;
using System.Collections.Generic;
using System.Linq;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class CrittersBag : CrittersActor
{
	// Token: 0x06000139 RID: 313 RVA: 0x00031187 File Offset: 0x0002F387
	protected override void Awake()
	{
		base.Awake();
		this.overlapColliders = new Collider[20];
		this.attachedColliders = new Dictionary<int, GameObject>();
		this.isAttachedToPlayer = false;
	}

	// Token: 0x0600013A RID: 314 RVA: 0x000311AE File Offset: 0x0002F3AE
	public override void OnHover(bool isLeft)
	{
		if (this.isAttachedToPlayer)
		{
			GorillaTagger.Instance.StartVibration(isLeft, GorillaTagger.Instance.tapHapticStrength / 4f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
			return;
		}
		base.OnHover(isLeft);
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0006D8E8 File Offset: 0x0006BAE8
	protected override void CleanupActor()
	{
		base.CleanupActor();
		for (int i = this.attachedColliders.Count - 1; i >= 0; i--)
		{
			this.attachedColliders[this.attachedColliders.ElementAt(i).Key].gameObject.Destroy();
		}
		this.attachedColliders.Clear();
	}

	// Token: 0x0600013C RID: 316 RVA: 0x0006D948 File Offset: 0x0006BB48
	protected override void GlobalGrabbedBy(CrittersActor grabbedBy)
	{
		base.GlobalGrabbedBy(grabbedBy);
		bool flag = this.attachedToLocalPlayer;
		if (grabbedBy.IsNotNull())
		{
			CrittersAttachPoint crittersAttachPoint = grabbedBy as CrittersAttachPoint;
			if (crittersAttachPoint != null)
			{
				this.isAttachedToPlayer = true;
				this.attachedToLocalPlayer = (crittersAttachPoint.rigPlayerId == PhotonNetwork.LocalPlayer.ActorNumber);
				goto IL_4F;
			}
		}
		this.isAttachedToPlayer = false;
		this.attachedToLocalPlayer = false;
		IL_4F:
		if (this.attachedToLocalPlayer != flag)
		{
			bool flag2 = this.attachedToLocalPlayer || flag;
			this.audioSrc.transform.localPosition = Vector3.zero;
			this.audioSrc.GTPlayOneShot(this.attachedToLocalPlayer ? this.equipSound : this.unequipSound, flag2 ? 1f : 0.5f);
		}
	}

	// Token: 0x0600013D RID: 317 RVA: 0x000311EB File Offset: 0x0002F3EB
	public override void GrabbedBy(CrittersActor grabbedBy, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
	{
		base.GrabbedBy(grabbedBy, positionOverride, localRotation, localOffset, disableGrabbing);
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0006D9FC File Offset: 0x0006BBFC
	public override void Released(bool keepWorldPosition, Quaternion rotation = default(Quaternion), Vector3 position = default(Vector3), Vector3 impulse = default(Vector3), Vector3 impulseRotation = default(Vector3))
	{
		if (this.parentActorId >= 0)
		{
			base.AttemptRemoveStoredObjectCollider(this.parentActorId, true);
		}
		int num = Physics.OverlapBoxNonAlloc(this.dropCube.transform.position, this.dropCube.size / 2f, this.overlapColliders, this.dropCube.transform.rotation, CrittersManager.instance.objectLayers, QueryTriggerInteraction.Collide);
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				Rigidbody attachedRigidbody = this.overlapColliders[i].attachedRigidbody;
				if (!(attachedRigidbody == null))
				{
					CrittersAttachPoint component = attachedRigidbody.GetComponent<CrittersAttachPoint>();
					if (!(component == null) && component.anchorLocation == this.anchorLocation && !(component.GetComponentInChildren<CrittersBag>() != null))
					{
						CrittersActor crittersActor;
						if (this.lastGrabbedPlayer == PhotonNetwork.LocalPlayer.ActorNumber && CrittersManager.instance.actorById.TryGetValue(this.parentActorId, out crittersActor))
						{
							CrittersGrabber crittersGrabber = crittersActor as CrittersGrabber;
							if (crittersGrabber != null)
							{
								GorillaTagger.Instance.StartVibration(crittersGrabber.isLeft, GorillaTagger.Instance.tapHapticStrength, GorillaTagger.Instance.tapHapticDuration);
							}
						}
						this.GrabbedBy(component, true, default(Quaternion), default(Vector3), false);
						return;
					}
				}
			}
		}
		base.Released(keepWorldPosition, rotation, position, impulse, impulseRotation);
	}

	// Token: 0x0600013F RID: 319 RVA: 0x0006DB6C File Offset: 0x0006BD6C
	public void AddStoredObjectCollider(CrittersActor actor)
	{
		if (this.attachedColliders.ContainsKey(actor.actorId))
		{
			if (this.attachedColliders[actor.actorId].IsNull())
			{
				this.attachedColliders[actor.actorId] = CrittersManager.DuplicateCapsuleCollider(base.transform, actor.storeCollider).gameObject;
			}
		}
		else
		{
			this.attachedColliders.Add(actor.actorId, CrittersManager.DuplicateCapsuleCollider(base.transform, actor.storeCollider).gameObject);
		}
		this.audioSrc.transform.position = actor.transform.position;
		this.audioSrc.GTPlayOneShot(this.attachSound, 1f);
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0006DC28 File Offset: 0x0006BE28
	public void RemoveStoredObjectCollider(CrittersActor actor, bool playSound = true)
	{
		GameObject obj;
		if (this.attachedColliders.TryGetValue(actor.actorId, out obj))
		{
			UnityEngine.Object.Destroy(obj);
			this.attachedColliders.Remove(actor.actorId);
		}
		if (playSound)
		{
			this.audioSrc.transform.position = actor.transform.position;
			this.audioSrc.GTPlayOneShot(this.detachSound, 1f);
		}
	}

	// Token: 0x06000141 RID: 321 RVA: 0x000311FA File Offset: 0x0002F3FA
	public bool IsActorValidStore(CrittersActor actor)
	{
		return this.blockAttachTypes == null || !this.blockAttachTypes.Contains(actor.crittersActorType);
	}

	// Token: 0x04000172 RID: 370
	public AudioSource audioSrc;

	// Token: 0x04000173 RID: 371
	public CrittersAttachPoint.AnchoredLocationTypes anchorLocation;

	// Token: 0x04000174 RID: 372
	public Collider attachableCollider;

	// Token: 0x04000175 RID: 373
	public BoxCollider dropCube;

	// Token: 0x04000176 RID: 374
	private Collider[] overlapColliders;

	// Token: 0x04000177 RID: 375
	public List<Collider> attachDisableColliders;

	// Token: 0x04000178 RID: 376
	public Dictionary<int, GameObject> attachedColliders;

	// Token: 0x04000179 RID: 377
	[Header("Child object attachment sounds")]
	public AudioClip attachSound;

	// Token: 0x0400017A RID: 378
	public AudioClip detachSound;

	// Token: 0x0400017B RID: 379
	[Header("Monke equip sounds")]
	public AudioClip equipSound;

	// Token: 0x0400017C RID: 380
	public AudioClip unequipSound;

	// Token: 0x0400017D RID: 381
	[Header("Attachment Blocking")]
	public List<CrittersActor.CrittersActorType> blockAttachTypes;

	// Token: 0x0400017E RID: 382
	private bool isAttachedToPlayer;

	// Token: 0x0400017F RID: 383
	private bool attachedToLocalPlayer;
}
