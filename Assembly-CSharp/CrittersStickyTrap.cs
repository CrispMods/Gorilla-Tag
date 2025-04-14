using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class CrittersStickyTrap : CrittersToolThrowable
{
	// Token: 0x06000258 RID: 600 RVA: 0x0000F353 File Offset: 0x0000D553
	public override void Initialize()
	{
		base.Initialize();
		base.TogglePhysics(!this.isStuck);
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000F36A File Offset: 0x0000D56A
	public override void OnDisable()
	{
		base.OnDisable();
		this.isStuck = false;
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000F37C File Offset: 0x0000D57C
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

	// Token: 0x0600025B RID: 603 RVA: 0x0000F418 File Offset: 0x0000D618
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

	// Token: 0x0600025C RID: 604 RVA: 0x0000C486 File Offset: 0x0000A686
	protected override void OnImpactCritter(CrittersPawn impactedCritter)
	{
		this.OnImpact(impactedCritter.transform.position, impactedCritter.transform.up);
	}

	// Token: 0x0600025D RID: 605 RVA: 0x0000F4E9 File Offset: 0x0000D6E9
	protected override void OnPickedUp()
	{
		if (this.isStuck)
		{
			this.isStuck = false;
			this.updatedSinceLastFrame = true;
		}
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0000F501 File Offset: 0x0000D701
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(this.isStuck);
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000F51C File Offset: 0x0000D71C
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

	// Token: 0x06000260 RID: 608 RVA: 0x0000F559 File Offset: 0x0000D759
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(this.isStuck);
		return this.TotalActorDataLength();
	}

	// Token: 0x06000261 RID: 609 RVA: 0x00008238 File Offset: 0x00006438
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 1;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0000F57C File Offset: 0x0000D77C
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

	// Token: 0x040002DB RID: 731
	[Header("Sticky Trap")]
	public bool stickOnImpact = true;

	// Token: 0x040002DC RID: 732
	public int subStickyGooIndex = -1;

	// Token: 0x040002DD RID: 733
	private bool isStuck;
}
