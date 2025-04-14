﻿using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class CrittersStickyTrap : CrittersToolThrowable
{
	// Token: 0x0600025A RID: 602 RVA: 0x0000F6F7 File Offset: 0x0000D8F7
	public override void Initialize()
	{
		base.Initialize();
		base.TogglePhysics(!this.isStuck);
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000F70E File Offset: 0x0000D90E
	public override void OnDisable()
	{
		base.OnDisable();
		this.isStuck = false;
	}

	// Token: 0x0600025C RID: 604 RVA: 0x0000F720 File Offset: 0x0000D920
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

	// Token: 0x0600025D RID: 605 RVA: 0x0000F7BC File Offset: 0x0000D9BC
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

	// Token: 0x0600025E RID: 606 RVA: 0x0000C7F3 File Offset: 0x0000A9F3
	protected override void OnImpactCritter(CrittersPawn impactedCritter)
	{
		this.OnImpact(impactedCritter.transform.position, impactedCritter.transform.up);
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000F88D File Offset: 0x0000DA8D
	protected override void OnPickedUp()
	{
		if (this.isStuck)
		{
			this.isStuck = false;
			this.updatedSinceLastFrame = true;
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0000F8A5 File Offset: 0x0000DAA5
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(this.isStuck);
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0000F8C0 File Offset: 0x0000DAC0
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

	// Token: 0x06000262 RID: 610 RVA: 0x0000F8FD File Offset: 0x0000DAFD
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(this.isStuck);
		return this.TotalActorDataLength();
	}

	// Token: 0x06000263 RID: 611 RVA: 0x00008450 File Offset: 0x00006650
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 1;
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0000F920 File Offset: 0x0000DB20
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
