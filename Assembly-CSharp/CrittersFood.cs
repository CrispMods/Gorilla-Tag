﻿using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class CrittersFood : CrittersActor
{
	// Token: 0x06000156 RID: 342 RVA: 0x00009049 File Offset: 0x00007249
	public override void Initialize()
	{
		base.Initialize();
		this.currentFood = this.maxFood;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00009060 File Offset: 0x00007260
	public void SpawnData(float _maxFood, float _currentFood, float _startingSize)
	{
		this.maxFood = _maxFood;
		this.currentFood = _currentFood;
		this.startingSize = _startingSize;
		this.currentSize = this.currentFood / this.maxFood * this.startingSize;
		this.food.localScale = new Vector3(this.currentSize, this.currentSize, this.currentSize);
	}

	// Token: 0x06000158 RID: 344 RVA: 0x000090C0 File Offset: 0x000072C0
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

	// Token: 0x06000159 RID: 345 RVA: 0x00009182 File Offset: 0x00007382
	public override void ProcessRemote()
	{
		base.ProcessRemote();
		if (!this.isEnabled)
		{
			return;
		}
		this.ProcessFood();
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000919C File Offset: 0x0000739C
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

	// Token: 0x0600015B RID: 347 RVA: 0x00009226 File Offset: 0x00007426
	public void Feed(float amountEaten)
	{
		this.currentFood = Mathf.Max(0f, this.currentFood - amountEaten);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00009240 File Offset: 0x00007440
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

	// Token: 0x0600015D RID: 349 RVA: 0x000092A4 File Offset: 0x000074A4
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(Mathf.FloorToInt(this.currentFood));
		stream.SendNext(this.maxFood);
		stream.SendNext(this.startingSize);
	}

	// Token: 0x0600015E RID: 350 RVA: 0x000092F0 File Offset: 0x000074F0
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(Mathf.FloorToInt(this.currentFood));
		objList.Add(this.maxFood);
		objList.Add(this.startingSize);
		return this.TotalActorDataLength();
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00009346 File Offset: 0x00007546
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 3;
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00009350 File Offset: 0x00007550
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

	// Token: 0x040001A4 RID: 420
	public float maxFood;

	// Token: 0x040001A5 RID: 421
	public float currentFood;

	// Token: 0x040001A6 RID: 422
	private int lastFood;

	// Token: 0x040001A7 RID: 423
	public float startingSize;

	// Token: 0x040001A8 RID: 424
	public float currentSize;

	// Token: 0x040001A9 RID: 425
	public Transform food;

	// Token: 0x040001AA RID: 426
	public bool disableWhenEmpty = true;
}
