using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class CrittersCageDepositShim : MonoBehaviour
{
	// Token: 0x06000166 RID: 358 RVA: 0x0006E2B4 File Offset: 0x0006C4B4
	[ContextMenu("Copy Deposit Data To Shim")]
	private CrittersCageDeposit CopySpawnerDataInPrefab()
	{
		CrittersCageDeposit component = base.gameObject.GetComponent<CrittersCageDeposit>();
		this.cageBoxCollider = (BoxCollider)component.gameObject.GetComponent<Collider>();
		this.type = component.actorType;
		this.disableGrabOnAttach = component.disableGrabOnAttach;
		this.allowMultiAttach = component.allowMultiAttach;
		this.snapOnAttach = component.snapOnAttach;
		this.startLocation = component.depositStartLocation;
		this.endLocation = component.depositEndLocation;
		this.submitDuration = component.submitDuration;
		this.returnDuration = component.returnDuration;
		this.depositAudio = component.depositAudio;
		this.depositStartSound = component.depositStartSound;
		this.depositEmptySound = component.depositEmptySound;
		this.depositCritterSound = component.depositCritterSound;
		this.attachPointTransform = component.GetComponentInChildren<CrittersActor>().transform;
		this.visiblePlatformTransform = this.attachPointTransform.transform.GetChild(0).transform;
		return component;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0006E3A4 File Offset: 0x0006C5A4
	[ContextMenu("Replace Deposit With Shim")]
	private void ReplaceSpawnerWithShim()
	{
		CrittersCageDeposit crittersCageDeposit = this.CopySpawnerDataInPrefab();
		if (crittersCageDeposit.attachPoint.GetComponent<Rigidbody>() != null)
		{
			UnityEngine.Object.DestroyImmediate(crittersCageDeposit.attachPoint.GetComponent<Rigidbody>());
		}
		UnityEngine.Object.DestroyImmediate(crittersCageDeposit.attachPoint);
		UnityEngine.Object.DestroyImmediate(crittersCageDeposit);
	}

	// Token: 0x040001B3 RID: 435
	public BoxCollider cageBoxCollider;

	// Token: 0x040001B4 RID: 436
	public CrittersActor.CrittersActorType type;

	// Token: 0x040001B5 RID: 437
	public bool disableGrabOnAttach;

	// Token: 0x040001B6 RID: 438
	public bool allowMultiAttach;

	// Token: 0x040001B7 RID: 439
	public bool snapOnAttach;

	// Token: 0x040001B8 RID: 440
	public Vector3 startLocation;

	// Token: 0x040001B9 RID: 441
	public Vector3 endLocation;

	// Token: 0x040001BA RID: 442
	public float submitDuration;

	// Token: 0x040001BB RID: 443
	public float returnDuration;

	// Token: 0x040001BC RID: 444
	public AudioSource depositAudio;

	// Token: 0x040001BD RID: 445
	public AudioClip depositStartSound;

	// Token: 0x040001BE RID: 446
	public AudioClip depositEmptySound;

	// Token: 0x040001BF RID: 447
	public AudioClip depositCritterSound;

	// Token: 0x040001C0 RID: 448
	public Transform attachPointTransform;

	// Token: 0x040001C1 RID: 449
	public Transform visiblePlatformTransform;
}
