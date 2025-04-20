using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class CrittersStickyTrap : CrittersToolThrowable
{
	// Token: 0x06000285 RID: 645 RVA: 0x00031F6C File Offset: 0x0003016C
	public override void Initialize()
	{
		base.Initialize();
		this.TogglePhysics(!this.isStuck);
	}

	// Token: 0x06000286 RID: 646 RVA: 0x00031F83 File Offset: 0x00030183
	public override void OnDisable()
	{
		base.OnDisable();
		this.isStuck = false;
	}

	// Token: 0x06000287 RID: 647 RVA: 0x000741B0 File Offset: 0x000723B0
	public override void SetImpulse()
	{
		if (this.isOnPlayer || this.isSceneActor)
		{
			return;
		}
		this.localLastImpulse = this.lastImpulseTime;
		base.MoveActor(this.lastImpulsePosition, this.lastImpulseQuaternion, this.parentActorId >= 0, false, true);
		this.TogglePhysics(this.usesRB && this.parentActorId == -1 && !this.isStuck);
		if (!this.rb.isKinematic)
		{
			this.rb.velocity = this.lastImpulseVelocity;
			this.rb.angularVelocity = this.lastImpulseAngularVelocity;
		}
	}

	// Token: 0x06000288 RID: 648 RVA: 0x0007424C File Offset: 0x0007244C
	protected override void OnImpact(Vector3 hitPosition, Vector3 hitNormal)
	{
		if (CrittersManager.instance.LocalAuthority())
		{
			if (this.stickOnImpact)
			{
				this.rb.isKinematic = true;
				this.isStuck = true;
				this.updatedSinceLastFrame = true;
				base.UpdateImpulses(false, true);
			}
			CrittersStickyGoo crittersStickyGoo = (CrittersStickyGoo)CrittersManager.instance.SpawnActor(CrittersActor.CrittersActorType.StickyGoo, this.subStickyGooIndex);
			if (crittersStickyGoo == null)
			{
				return;
			}
			CrittersManager.instance.TriggerEvent(CrittersManager.CritterEvent.StickyDeployed, this.actorId, base.transform.position, Quaternion.LookRotation(hitNormal));
			Vector3 vector = base.transform.forward;
			vector -= hitNormal * Vector3.Dot(hitNormal, vector);
			crittersStickyGoo.MoveActor(hitPosition, Quaternion.LookRotation(vector, hitNormal), false, true, true);
			crittersStickyGoo.SetImpulseVelocity(Vector3.zero, Vector3.zero);
			base.UpdateImpulses(true, false);
		}
	}

	// Token: 0x06000289 RID: 649 RVA: 0x000319AE File Offset: 0x0002FBAE
	protected override void OnImpactCritter(CrittersPawn impactedCritter)
	{
		this.OnImpact(impactedCritter.transform.position, impactedCritter.transform.up);
	}

	// Token: 0x0600028A RID: 650 RVA: 0x00031F92 File Offset: 0x00030192
	protected override void OnPickedUp()
	{
		if (this.isStuck)
		{
			this.isStuck = false;
			this.updatedSinceLastFrame = true;
		}
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00031FAA File Offset: 0x000301AA
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(this.isStuck);
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00074328 File Offset: 0x00072528
	public override bool UpdateSpecificActor(PhotonStream stream)
	{
		bool flag;
		if (!(base.UpdateSpecificActor(stream) & CrittersManager.ValidateDataType<bool>(stream.ReceiveNext(), out flag)))
		{
			return false;
		}
		this.isStuck = flag;
		this.TogglePhysics(!this.isStuck);
		return true;
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00031FC4 File Offset: 0x000301C4
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(this.isStuck);
		return this.TotalActorDataLength();
	}

	// Token: 0x0600028E RID: 654 RVA: 0x00031114 File Offset: 0x0002F314
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 1;
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00074368 File Offset: 0x00072568
	public override int UpdateFromRPC(object[] data, int startingIndex)
	{
		startingIndex += base.UpdateFromRPC(data, startingIndex);
		bool flag;
		if (!CrittersManager.ValidateDataType<bool>(data[startingIndex], out flag))
		{
			return this.TotalActorDataLength();
		}
		this.isStuck = flag;
		this.TogglePhysics(!this.isStuck);
		return this.TotalActorDataLength();
	}

	// Token: 0x0400030A RID: 778
	[Header("Sticky Trap")]
	public bool stickOnImpact = true;

	// Token: 0x0400030B RID: 779
	public int subStickyGooIndex = -1;

	// Token: 0x0400030C RID: 780
	private bool isStuck;
}
