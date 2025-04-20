using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class CrittersLoudNoise : CrittersActor
{
	// Token: 0x06000182 RID: 386 RVA: 0x0003155D File Offset: 0x0002F75D
	public override void OnEnable()
	{
		base.OnEnable();
		this.SetTimeEnabled();
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0003156B File Offset: 0x0002F76B
	public void SpawnData(float _soundVolume, float _soundDuration, float _soundMultiplier, bool _soundEnabled)
	{
		this.soundVolume = _soundVolume;
		this.volumeFearAttractionMultiplier = _soundMultiplier;
		this.soundDuration = _soundDuration;
		this.soundEnabled = _soundEnabled;
		this.Initialize();
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0006E880 File Offset: 0x0006CA80
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

	// Token: 0x06000185 RID: 389 RVA: 0x00031590 File Offset: 0x0002F790
	public override void ProcessRemote()
	{
		if (!this.wasEnabled && this.isEnabled)
		{
			this.SetTimeEnabled();
		}
	}

	// Token: 0x06000186 RID: 390 RVA: 0x000315A8 File Offset: 0x0002F7A8
	public void SetTimeEnabled()
	{
		if (PhotonNetwork.InRoom)
		{
			this.timeSoundEnabled = PhotonNetwork.Time;
			return;
		}
		this.timeSoundEnabled = (double)Time.time;
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0006E984 File Offset: 0x0006CB84
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

	// Token: 0x06000188 RID: 392 RVA: 0x0006EA00 File Offset: 0x0006CC00
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

	// Token: 0x06000189 RID: 393 RVA: 0x0006EA7C File Offset: 0x0006CC7C
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

	// Token: 0x0600018A RID: 394 RVA: 0x0006EAF8 File Offset: 0x0006CCF8
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(this.soundVolume);
		stream.SendNext(this.soundDuration);
		stream.SendNext(this.soundEnabled);
		stream.SendNext(this.volumeFearAttractionMultiplier);
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0006EB50 File Offset: 0x0006CD50
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(this.soundVolume);
		objList.Add(this.soundDuration);
		objList.Add(this.soundEnabled);
		objList.Add(this.volumeFearAttractionMultiplier);
		return this.TotalActorDataLength();
	}

	// Token: 0x0600018C RID: 396 RVA: 0x000315C9 File Offset: 0x0002F7C9
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 4;
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0006EBB4 File Offset: 0x0006CDB4
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

	// Token: 0x0600018E RID: 398 RVA: 0x000315D3 File Offset: 0x0002F7D3
	public void PlayHandTapLocal(bool isLeft)
	{
		this.timeSoundEnabled = (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time));
		this.soundEnabled = true;
	}

	// Token: 0x0600018F RID: 399 RVA: 0x000315F6 File Offset: 0x0002F7F6
	public void PlayHandTapRemote(double serverTime, bool isLeft)
	{
		this.timeSoundEnabled = serverTime;
		this.soundEnabled = true;
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00031606 File Offset: 0x0002F806
	public void PlayVoiceSpeechLocal(double serverTime, float duration, float volume)
	{
		this.soundDuration = duration;
		this.timeSoundEnabled = serverTime;
		this.soundVolume = volume;
		this.soundEnabled = true;
	}

	// Token: 0x040001DD RID: 477
	public float soundVolume;

	// Token: 0x040001DE RID: 478
	public float volumeFearAttractionMultiplier;

	// Token: 0x040001DF RID: 479
	public float soundDuration;

	// Token: 0x040001E0 RID: 480
	public double timeSoundEnabled;

	// Token: 0x040001E1 RID: 481
	public bool soundEnabled;

	// Token: 0x040001E2 RID: 482
	private bool wasSoundEnabled;

	// Token: 0x040001E3 RID: 483
	public bool disableWhenSoundDisabled;
}
