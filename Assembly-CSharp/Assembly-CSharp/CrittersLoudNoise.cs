using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class CrittersLoudNoise : CrittersActor
{
	// Token: 0x06000169 RID: 361 RVA: 0x000096AE File Offset: 0x000078AE
	public override void OnEnable()
	{
		base.OnEnable();
		this.SetTimeEnabled();
	}

	// Token: 0x0600016A RID: 362 RVA: 0x000096BC File Offset: 0x000078BC
	public void SpawnData(float _soundVolume, float _soundDuration, float _soundMultiplier, bool _soundEnabled)
	{
		this.soundVolume = _soundVolume;
		this.volumeFearAttractionMultiplier = _soundMultiplier;
		this.soundDuration = _soundDuration;
		this.soundEnabled = _soundEnabled;
		this.Initialize();
	}

	// Token: 0x0600016B RID: 363 RVA: 0x000096E4 File Offset: 0x000078E4
	public override bool ProcessLocal()
	{
		bool flag = base.ProcessLocal();
		if (!this.isEnabled)
		{
			return flag;
		}
		this.wasEnabled = base.gameObject.activeSelf;
		this.wasSoundEnabled = this.soundEnabled;
		if (PhotonNetwork.InRoom)
		{
			if (PhotonNetwork.Time > this.timeSoundEnabled + (double)this.soundDuration || this.timeSoundEnabled > PhotonNetwork.Time)
			{
				this.soundEnabled = false;
			}
		}
		else if ((double)Time.time > this.timeSoundEnabled + (double)this.soundDuration || this.timeSoundEnabled > (double)Time.time)
		{
			this.soundEnabled = false;
		}
		if (this.disableWhenSoundDisabled && !this.soundEnabled)
		{
			this.isEnabled = false;
			if (base.gameObject.activeSelf != this.isEnabled)
			{
				base.gameObject.SetActive(this.isEnabled);
			}
		}
		this.updatedSinceLastFrame = (flag || this.wasSoundEnabled != this.soundEnabled || this.wasEnabled != this.isEnabled);
		return this.updatedSinceLastFrame;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x000097E8 File Offset: 0x000079E8
	public override void ProcessRemote()
	{
		if (!this.wasEnabled && this.isEnabled)
		{
			this.SetTimeEnabled();
		}
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00009800 File Offset: 0x00007A00
	public void SetTimeEnabled()
	{
		if (PhotonNetwork.InRoom)
		{
			this.timeSoundEnabled = PhotonNetwork.Time;
			return;
		}
		this.timeSoundEnabled = (double)Time.time;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00009824 File Offset: 0x00007A24
	public override void CalculateFear(CrittersPawn critter, float multiplier)
	{
		if (this.soundEnabled)
		{
			if (this.soundDuration == 0f)
			{
				critter.IncreaseFear(this.soundVolume * this.volumeFearAttractionMultiplier * multiplier, this);
				return;
			}
			if ((PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time)) - this.timeSoundEnabled < (double)this.soundDuration)
			{
				critter.IncreaseFear(this.soundVolume * this.volumeFearAttractionMultiplier * Time.deltaTime * multiplier, this);
			}
		}
	}

	// Token: 0x0600016F RID: 367 RVA: 0x000098A0 File Offset: 0x00007AA0
	public override void CalculateAttraction(CrittersPawn critter, float multiplier)
	{
		if (this.soundEnabled)
		{
			if (this.soundDuration == 0f)
			{
				critter.IncreaseAttraction(this.soundVolume * this.volumeFearAttractionMultiplier * multiplier, this);
				return;
			}
			if ((PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time)) - this.timeSoundEnabled < (double)this.soundDuration)
			{
				critter.IncreaseAttraction(this.soundVolume * this.volumeFearAttractionMultiplier * Time.deltaTime * multiplier, this);
			}
		}
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000991C File Offset: 0x00007B1C
	public override bool UpdateSpecificActor(PhotonStream stream)
	{
		float value;
		float value2;
		bool flag;
		float value3;
		if (!(base.UpdateSpecificActor(stream) & CrittersManager.ValidateDataType<float>(stream.ReceiveNext(), out value) & CrittersManager.ValidateDataType<float>(stream.ReceiveNext(), out value2) & CrittersManager.ValidateDataType<bool>(stream.ReceiveNext(), out flag) & CrittersManager.ValidateDataType<float>(stream.ReceiveNext(), out value3)))
		{
			return false;
		}
		this.soundVolume = value.GetFinite();
		this.soundDuration = value2.GetFinite();
		this.soundEnabled = flag;
		this.volumeFearAttractionMultiplier = value3.GetFinite();
		return true;
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00009998 File Offset: 0x00007B98
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(this.soundVolume);
		stream.SendNext(this.soundDuration);
		stream.SendNext(this.soundEnabled);
		stream.SendNext(this.volumeFearAttractionMultiplier);
	}

	// Token: 0x06000172 RID: 370 RVA: 0x000099F0 File Offset: 0x00007BF0
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(this.soundVolume);
		objList.Add(this.soundDuration);
		objList.Add(this.soundEnabled);
		objList.Add(this.volumeFearAttractionMultiplier);
		return this.TotalActorDataLength();
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00009A53 File Offset: 0x00007C53
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 4;
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00009A60 File Offset: 0x00007C60
	public override int UpdateFromRPC(object[] data, int startingIndex)
	{
		startingIndex += base.UpdateFromRPC(data, startingIndex);
		float value;
		if (!CrittersManager.ValidateDataType<float>(data[startingIndex], out value))
		{
			return this.TotalActorDataLength();
		}
		float value2;
		if (!CrittersManager.ValidateDataType<float>(data[startingIndex + 1], out value2))
		{
			return this.TotalActorDataLength();
		}
		bool flag;
		if (!CrittersManager.ValidateDataType<bool>(data[startingIndex + 2], out flag))
		{
			return this.TotalActorDataLength();
		}
		float value3;
		if (!CrittersManager.ValidateDataType<float>(data[startingIndex + 3], out value3))
		{
			return this.TotalActorDataLength();
		}
		this.soundVolume = value.GetFinite();
		this.soundDuration = value2.GetFinite();
		this.soundEnabled = flag;
		this.volumeFearAttractionMultiplier = value3.GetFinite();
		return this.TotalActorDataLength();
	}

	// Token: 0x06000175 RID: 373 RVA: 0x00009AFC File Offset: 0x00007CFC
	public void PlayHandTapLocal(bool isLeft)
	{
		this.timeSoundEnabled = (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time));
		this.soundEnabled = true;
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00009B1F File Offset: 0x00007D1F
	public void PlayHandTapRemote(double serverTime, bool isLeft)
	{
		this.timeSoundEnabled = serverTime;
		this.soundEnabled = true;
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00009B2F File Offset: 0x00007D2F
	public void PlayVoiceSpeechLocal(double serverTime, float duration, float volume)
	{
		this.soundDuration = duration;
		this.timeSoundEnabled = serverTime;
		this.soundVolume = volume;
		this.soundEnabled = true;
	}

	// Token: 0x040001BA RID: 442
	public float soundVolume;

	// Token: 0x040001BB RID: 443
	public float volumeFearAttractionMultiplier;

	// Token: 0x040001BC RID: 444
	public float soundDuration;

	// Token: 0x040001BD RID: 445
	public double timeSoundEnabled;

	// Token: 0x040001BE RID: 446
	public bool soundEnabled;

	// Token: 0x040001BF RID: 447
	private bool wasSoundEnabled;

	// Token: 0x040001C0 RID: 448
	public bool disableWhenSoundDisabled;
}
