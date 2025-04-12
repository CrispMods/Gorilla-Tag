using System;
using System.Collections.Generic;
using Photon.Pun;

// Token: 0x02000037 RID: 55
public class CrittersActorSpawnerPoint : CrittersActor
{
	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000112 RID: 274 RVA: 0x0006B4B0 File Offset: 0x000696B0
	// (remove) Token: 0x06000113 RID: 275 RVA: 0x0006B4E8 File Offset: 0x000696E8
	public event Action<CrittersActor> OnSpawnChanged;

	// Token: 0x06000114 RID: 276 RVA: 0x000300DD File Offset: 0x0002E2DD
	public override void Initialize()
	{
		base.Initialize();
		base.UpdateImpulses(false, false);
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000300ED File Offset: 0x0002E2ED
	public override void OnDisable()
	{
		base.OnDisable();
		this.spawnedActorID = -1;
		this.spawnedActor = null;
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0006B520 File Offset: 0x00069720
	public void SetSpawnedActor(CrittersActor actor)
	{
		if (this.spawnedActor == actor)
		{
			return;
		}
		this.spawnedActor = actor;
		if (this.spawnedActor != null)
		{
			this.spawnedActorID = this.spawnedActor.actorId;
		}
		else
		{
			this.spawnedActorID = -1;
		}
		Action<CrittersActor> onSpawnChanged = this.OnSpawnChanged;
		if (onSpawnChanged != null)
		{
			onSpawnChanged(this.spawnedActor);
		}
		this.updatedSinceLastFrame = true;
	}

	// Token: 0x06000117 RID: 279 RVA: 0x0006B58C File Offset: 0x0006978C
	private void UpdateSpawnedActor(int newSpawnedActorID)
	{
		if (this.spawnedActorID == newSpawnedActorID)
		{
			return;
		}
		if (newSpawnedActorID == -1)
		{
			this.spawnedActorID = newSpawnedActorID;
			this.spawnedActor = null;
		}
		else
		{
			CrittersActor crittersActor;
			if (!CrittersManager.instance.actorById.TryGetValue(newSpawnedActorID, out crittersActor))
			{
				return;
			}
			this.spawnedActorID = newSpawnedActorID;
			this.spawnedActor = crittersActor;
		}
		Action<CrittersActor> onSpawnChanged = this.OnSpawnChanged;
		if (onSpawnChanged == null)
		{
			return;
		}
		onSpawnChanged(this.spawnedActor);
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00030103 File Offset: 0x0002E303
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(this.spawnedActorID);
	}

	// Token: 0x06000119 RID: 281 RVA: 0x0006B5F4 File Offset: 0x000697F4
	public override bool UpdateSpecificActor(PhotonStream stream)
	{
		if (!base.UpdateSpecificActor(stream))
		{
			return false;
		}
		int num;
		if (!CrittersManager.ValidateDataType<int>(stream.ReceiveNext(), out num))
		{
			return false;
		}
		if (num < -1 || num >= CrittersManager.instance.universalActorId)
		{
			return false;
		}
		this.UpdateSpawnedActor(num);
		return true;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x0003011D File Offset: 0x0002E31D
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(this.spawnedActorID);
		return this.TotalActorDataLength();
	}

	// Token: 0x0600011B RID: 283 RVA: 0x0003013F File Offset: 0x0002E33F
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 1;
	}

	// Token: 0x0600011C RID: 284 RVA: 0x0006B63C File Offset: 0x0006983C
	public override int UpdateFromRPC(object[] data, int startingIndex)
	{
		startingIndex += base.UpdateFromRPC(data, startingIndex);
		int num;
		if (!CrittersManager.ValidateDataType<int>(data[startingIndex], out num))
		{
			return this.TotalActorDataLength();
		}
		if (num >= -1 && num < CrittersManager.instance.universalActorId)
		{
			return this.TotalActorDataLength();
		}
		this.UpdateSpawnedActor(num);
		return this.TotalActorDataLength();
	}

	// Token: 0x04000150 RID: 336
	private CrittersActor spawnedActor;

	// Token: 0x04000151 RID: 337
	private int spawnedActorID = -1;
}
