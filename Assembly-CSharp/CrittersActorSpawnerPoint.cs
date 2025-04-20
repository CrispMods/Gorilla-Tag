using System;
using System.Collections.Generic;
using Photon.Pun;

// Token: 0x0200003A RID: 58
public class CrittersActorSpawnerPoint : CrittersActor
{
	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000123 RID: 291 RVA: 0x0006D5C8 File Offset: 0x0006B7C8
	// (remove) Token: 0x06000124 RID: 292 RVA: 0x0006D600 File Offset: 0x0006B800
	public event Action<CrittersActor> OnSpawnChanged;

	// Token: 0x06000125 RID: 293 RVA: 0x000310B2 File Offset: 0x0002F2B2
	public override void Initialize()
	{
		base.Initialize();
		base.UpdateImpulses(false, false);
	}

	// Token: 0x06000126 RID: 294 RVA: 0x000310C2 File Offset: 0x0002F2C2
	public override void OnDisable()
	{
		base.OnDisable();
		this.spawnedActorID = -1;
		this.spawnedActor = null;
	}

	// Token: 0x06000127 RID: 295 RVA: 0x0006D638 File Offset: 0x0006B838
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

	// Token: 0x06000128 RID: 296 RVA: 0x0006D6A4 File Offset: 0x0006B8A4
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

	// Token: 0x06000129 RID: 297 RVA: 0x000310D8 File Offset: 0x0002F2D8
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(this.spawnedActorID);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x0006D70C File Offset: 0x0006B90C
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

	// Token: 0x0600012B RID: 299 RVA: 0x000310F2 File Offset: 0x0002F2F2
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(this.spawnedActorID);
		return this.TotalActorDataLength();
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00031114 File Offset: 0x0002F314
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 1;
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0006D754 File Offset: 0x0006B954
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

	// Token: 0x04000159 RID: 345
	private CrittersActor spawnedActor;

	// Token: 0x0400015A RID: 346
	private int spawnedActorID = -1;
}
