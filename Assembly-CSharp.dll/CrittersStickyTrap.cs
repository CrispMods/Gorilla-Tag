using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class CrittersStickyTrap : CrittersToolThrowable
{
	// Token: 0x0600025A RID: 602 RVA: 0x00030E2A File Offset: 0x0002F02A
	public override void Initialize()
	{
		base.Initialize();
		base.TogglePhysics(!this.isStuck);
	}

	// Token: 0x0600025B RID: 603 RVA: 0x00030E41 File Offset: 0x0002F041
	public override void OnDisable()
	{
		base.OnDisable();
		this.isStuck = false;
	}

	// Token: 0x0600025C RID: 604 RVA: 0x00071BDC File Offset: 0x0006FDDC
	public override void SetImpulse()
	{
		if (this.isOnPlayer || this.isSceneActor)
		{
			return;
		}
		this.localLastImpulse = this.lastImpulseTime;
		base.MoveActor(this.lastImpulsePosition, this.lastImpulseQuaternion, this.parentActorId >= 0, false, true);
		base.TogglePhysics(this.usesRB && this.parentActorId == -1 && !this.isStuck);
		if (!this.rb.isKinematic)
		{
			this.rb.velocity = this.lastImpulseVelocity;
			this.rb.angularVelocity = this.lastImpulseAngularVelocity;
		}
	}

	// Token: 0x0600025D RID: 605 RVA: 0x00071C78 File Offset: 0x0006FE78
	protected override void OnImpact(Vector3 hitPosition, Vector3 hitNormal)
	{
		if (CrittersManager.instance.LocalAuthority())
		{
			if (this.stickOnImpact)
			{
				this.rb.isKinematic = true;
				this.isStuck = true;
				this.updatedSinceLastFrame = true;
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

	// Token: 0x0600025E RID: 606 RVA: 0x00030944 File Offset: 0x0002EB44
	protected override void OnImpactCritter(CrittersPawn impactedCritter)
	{
		this.OnImpact(impactedCritter.transform.position, impactedCritter.transform.up);
	}

	// Token: 0x0600025F RID: 607 RVA: 0x00030E50 File Offset: 0x0002F050
	protected override void OnPickedUp()
	{
		if (this.isStuck)
		{
			this.isStuck = false;
			this.updatedSinceLastFrame = true;
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x00030E68 File Offset: 0x0002F068
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(this.isStuck);
	}

	// Token: 0x06000261 RID: 609 RVA: 0x00071D4C File Offset: 0x0006FF4C
	public override bool UpdateSpecificActor(PhotonStream stream)
	{
		bool flag;
		if (!(base.UpdateSpecificActor(stream) & CrittersManager.ValidateDataType<bool>(stream.ReceiveNext(), out flag)))
		{
			return false;
		}
		this.isStuck = flag;
		base.TogglePhysics(!this.isStuck);
		return true;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x00030E82 File Offset: 0x0002F082
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(this.isStuck);
		return this.TotalActorDataLength();
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0003013F File Offset: 0x0002E33F
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 1;
	}

	// Token: 0x06000264 RID: 612 RVA: 0x00071D8C File Offset: 0x0006FF8C
	public override int UpdateFromRPC(object[] data, int startingIndex)
	{
		startingIndex += base.UpdateFromRPC(data, startingIndex);
		bool flag;
		if (!CrittersManager.ValidateDataType<bool>(data[startingIndex], out flag))
		{
			return this.TotalActorDataLength();
		}
		this.isStuck = flag;
		base.TogglePhysics(!this.isStuck);
		return this.TotalActorDataLength();
	}

	// Token: 0x040002DC RID: 732
	[Header("Sticky Trap")]
	public bool stickOnImpact = true;

	// Token: 0x040002DD RID: 733
	public int subStickyGooIndex = -1;

	// Token: 0x040002DE RID: 734
	private bool isStuck;
}
