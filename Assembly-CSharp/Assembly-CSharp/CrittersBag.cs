﻿using System;
using System.Collections.Generic;
using System.Linq;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class CrittersBag : CrittersActor
{
	// Token: 0x06000125 RID: 293 RVA: 0x00008583 File Offset: 0x00006783
	protected override void Awake()
	{
		base.Awake();
		this.overlapColliders = new Collider[20];
		this.attachedColliders = new Dictionary<int, GameObject>();
		this.isAttachedToPlayer = false;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x000085AA File Offset: 0x000067AA
	public override void OnHover(bool isLeft)
	{
		if (this.isAttachedToPlayer)
		{
			GorillaTagger.Instance.StartVibration(isLeft, GorillaTagger.Instance.tapHapticStrength / 4f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
			return;
		}
		base.OnHover(isLeft);
	}

	// Token: 0x06000127 RID: 295 RVA: 0x000085E8 File Offset: 0x000067E8
	protected override void CleanupActor()
	{
		base.CleanupActor();
		for (int i = this.attachedColliders.Count - 1; i >= 0; i--)
		{
			this.attachedColliders[this.attachedColliders.ElementAt(i).Key].gameObject.Destroy();
		}
		this.attachedColliders.Clear();
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00008648 File Offset: 0x00006848
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
			this.audioSrc.GTPlayOneShot(this.attachedToLocalPlayer ? this.equipSound : this.unequipSound, flag2 ? 1f : 0.5f);
		}
	}

	// Token: 0x06000129 RID: 297 RVA: 0x000086E6 File Offset: 0x000068E6
	public override void GrabbedBy(CrittersActor grabbedBy, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
	{
		base.GrabbedBy(grabbedBy, positionOverride, localRotation, localOffset, disableGrabbing);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x000086F8 File Offset: 0x000068F8
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

	// Token: 0x0600012B RID: 299 RVA: 0x00008868 File Offset: 0x00006A68
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
		this.audioSrc.GTPlayOneShot(this.attachSound, 1f);
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00008908 File Offset: 0x00006B08
	public void RemoveStoredObjectCollider(CrittersActor actor, bool playSound = true)
	{
		GameObject obj;
		if (this.attachedColliders.TryGetValue(actor.actorId, out obj))
		{
			Object.Destroy(obj);
			this.attachedColliders.Remove(actor.actorId);
		}
		if (playSound)
		{
			this.audioSrc.GTPlayOneShot(this.detachSound, 1f);
		}
	}

	// Token: 0x04000161 RID: 353
	public AudioSource audioSrc;

	// Token: 0x04000162 RID: 354
	public CrittersAttachPoint.AnchoredLocationTypes anchorLocation;

	// Token: 0x04000163 RID: 355
	public Collider attachableCollider;

	// Token: 0x04000164 RID: 356
	public BoxCollider dropCube;

	// Token: 0x04000165 RID: 357
	private Collider[] overlapColliders;

	// Token: 0x04000166 RID: 358
	public List<Collider> attachDisableColliders;

	// Token: 0x04000167 RID: 359
	public Dictionary<int, GameObject> attachedColliders;

	// Token: 0x04000168 RID: 360
	[Header("Child object attachment sounds")]
	public AudioClip attachSound;

	// Token: 0x04000169 RID: 361
	public AudioClip detachSound;

	// Token: 0x0400016A RID: 362
	[Header("Monke equip sounds")]
	public AudioClip equipSound;

	// Token: 0x0400016B RID: 363
	public AudioClip unequipSound;

	// Token: 0x0400016C RID: 364
	private bool isAttachedToPlayer;

	// Token: 0x0400016D RID: 365
	private bool attachedToLocalPlayer;
}
