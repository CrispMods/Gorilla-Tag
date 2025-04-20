using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class CrittersFood : CrittersActor
{
	// Token: 0x0600016F RID: 367 RVA: 0x00031488 File Offset: 0x0002F688
	public override void Initialize()
	{
		base.Initialize();
		this.currentFood = this.maxFood;
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0006E4CC File Offset: 0x0006C6CC
	public void SpawnData(float _maxFood, float _currentFood, float _startingSize)
	{
		this.maxFood = _maxFood;
		this.currentFood = _currentFood;
		this.startingSize = _startingSize;
		this.currentSize = this.currentFood / this.maxFood * this.startingSize;
		this.food.localScale = new Vector3(this.currentSize, this.currentSize, this.currentSize);
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0006E52C File Offset: 0x0006C72C
	public override bool ProcessLocal()
	{
		bool flag = base.ProcessLocal();
		if (!this.isEnabled)
		{
			return flag;
		}
		this.wasEnabled = base.gameObject.activeSelf;
		this.ProcessFood();
		bool flag2 = Mathf.FloorToInt(this.currentFood) != this.lastFood;
		this.lastFood = Mathf.FloorToInt(this.currentFood);
		if (this.currentFood == 0f && this.disableWhenEmpty)
		{
			this.isEnabled = false;
		}
		if (base.gameObject.activeSelf != this.isEnabled)
		{
			base.gameObject.SetActive(this.isEnabled);
		}
		this.updatedSinceLastFrame = (flag || flag2 || this.wasEnabled != this.isEnabled);
		return this.updatedSinceLastFrame;
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0003149C File Offset: 0x0002F69C
	public override void ProcessRemote()
	{
		base.ProcessRemote();
		if (!this.isEnabled)
		{
			return;
		}
		this.ProcessFood();
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0006E5F0 File Offset: 0x0006C7F0
	public void ProcessFood()
	{
		if (this.currentSize != this.currentFood / this.maxFood * this.startingSize)
		{
			this.currentSize = this.currentFood / this.maxFood * this.startingSize;
			this.food.localScale = new Vector3(this.currentSize, this.currentSize, this.currentSize);
			if (this.storeCollider != null)
			{
				this.storeCollider.radius = this.currentSize / 2f;
			}
		}
	}

	// Token: 0x06000174 RID: 372 RVA: 0x000314B3 File Offset: 0x0002F6B3
	public void Feed(float amountEaten)
	{
		this.currentFood = Mathf.Max(0f, this.currentFood - amountEaten);
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0006E67C File Offset: 0x0006C87C
	public override bool UpdateSpecificActor(PhotonStream stream)
	{
		int num;
		float value;
		float value2;
		if (!(base.UpdateSpecificActor(stream) & CrittersManager.ValidateDataType<int>(stream.ReceiveNext(), out num) & CrittersManager.ValidateDataType<float>(stream.ReceiveNext(), out value) & CrittersManager.ValidateDataType<float>(stream.ReceiveNext(), out value2)))
		{
			return false;
		}
		this.currentFood = (float)num;
		this.maxFood = value.GetFinite();
		this.startingSize = value2.GetFinite();
		return true;
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0006E6E0 File Offset: 0x0006C8E0
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(Mathf.FloorToInt(this.currentFood));
		stream.SendNext(this.maxFood);
		stream.SendNext(this.startingSize);
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0006E72C File Offset: 0x0006C92C
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(Mathf.FloorToInt(this.currentFood));
		objList.Add(this.maxFood);
		objList.Add(this.startingSize);
		return this.TotalActorDataLength();
	}

	// Token: 0x06000178 RID: 376 RVA: 0x000314CD File Offset: 0x0002F6CD
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 3;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0006E784 File Offset: 0x0006C984
	public override int UpdateFromRPC(object[] data, int startingIndex)
	{
		startingIndex += base.UpdateFromRPC(data, startingIndex);
		int num;
		if (!CrittersManager.ValidateDataType<int>(data[startingIndex], out num))
		{
			return this.TotalActorDataLength();
		}
		float value;
		if (!CrittersManager.ValidateDataType<float>(data[startingIndex + 1], out value))
		{
			return this.TotalActorDataLength();
		}
		float value2;
		if (!CrittersManager.ValidateDataType<float>(data[startingIndex + 2], out value2))
		{
			return this.TotalActorDataLength();
		}
		this.currentFood = (float)num;
		this.maxFood = value.GetFinite();
		this.startingSize = value2.GetFinite();
		return this.TotalActorDataLength();
	}

	// Token: 0x040001C9 RID: 457
	public float maxFood;

	// Token: 0x040001CA RID: 458
	public float currentFood;

	// Token: 0x040001CB RID: 459
	private int lastFood;

	// Token: 0x040001CC RID: 460
	public float startingSize;

	// Token: 0x040001CD RID: 461
	public float currentSize;

	// Token: 0x040001CE RID: 462
	public Transform food;

	// Token: 0x040001CF RID: 463
	public bool disableWhenEmpty = true;
}
