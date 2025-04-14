using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class CrittersCage : CrittersActor
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000131 RID: 305 RVA: 0x000088FC File Offset: 0x00006AFC
	public Vector3 critterScale
	{
		get
		{
			if (this.subObjectIndex < this.critterScales.Length && this.subObjectIndex >= 0)
			{
				return this.critterScales[this.subObjectIndex];
			}
			return Vector3.one;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000132 RID: 306 RVA: 0x0000892E File Offset: 0x00006B2E
	public bool CanCatch
	{
		get
		{
			return this.heldByPlayer && !this.hasCritter && !this.inReleasingPosition && this._releaseCooldownEnd <= Time.time;
		}
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0000895A File Offset: 0x00006B5A
	public void SetHasCritter(bool value)
	{
		if (this.hasCritter != value && !value)
		{
			this._releaseCooldownEnd = Time.time + this.releaseCooldown;
		}
		this.hasCritter = value;
		this.UpdateCageVisuals();
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00008987 File Offset: 0x00006B87
	public override void Initialize()
	{
		base.Initialize();
		this.hasCritter = false;
		this.heldByPlayer = false;
		this.inReleasingPosition = false;
		this.SetLidActive(true, false);
	}

	// Token: 0x06000135 RID: 309 RVA: 0x000089AC File Offset: 0x00006BAC
	public void UpdateCageVisuals()
	{
		this.SetLidActive(!this.heldByPlayer || this.hasCritter, true);
	}

	// Token: 0x06000136 RID: 310 RVA: 0x000089C8 File Offset: 0x00006BC8
	private void SetLidActive(bool active, bool playAudio = true)
	{
		if (active != this._lidActive && playAudio)
		{
			this.sound.GTPlayOneShot(active ? this.openSound : this.closeSound, 1f);
		}
		this.lid.SetActive(active);
		this._lidActive = active;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00008A19 File Offset: 0x00006C19
	protected override void RemoteGrabbedBy(CrittersActor grabbingActor)
	{
		base.RemoteGrabbed(grabbingActor);
		this.heldByPlayer = grabbingActor.isOnPlayer;
		this.UpdateCageVisuals();
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00008A34 File Offset: 0x00006C34
	public override void GrabbedBy(CrittersActor grabbingActor, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
	{
		base.GrabbedBy(grabbingActor, positionOverride, localRotation, localOffset, disableGrabbing);
		this.heldByPlayer = grabbingActor.isOnPlayer;
		this.UpdateCageVisuals();
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00008A55 File Offset: 0x00006C55
	public override void Released(bool keepWorldPosition, Quaternion rotation = default(Quaternion), Vector3 position = default(Vector3), Vector3 impulseVelocity = default(Vector3), Vector3 impulseAngularVelocity = default(Vector3))
	{
		base.Released(keepWorldPosition, rotation, position, impulseVelocity, impulseAngularVelocity);
		this.heldByPlayer = false;
		this.UpdateCageVisuals();
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00008A71 File Offset: 0x00006C71
	protected override void HandleRemoteReleased()
	{
		base.HandleRemoteReleased();
		this.heldByPlayer = false;
		this.UpdateCageVisuals();
	}

	// Token: 0x0600013B RID: 315 RVA: 0x00008A86 File Offset: 0x00006C86
	public override bool ShouldDespawn()
	{
		return base.ShouldDespawn() && !this.hasCritter;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00008A9B File Offset: 0x00006C9B
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(this.hasCritter);
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00008AB8 File Offset: 0x00006CB8
	public override bool UpdateSpecificActor(PhotonStream stream)
	{
		if (!base.UpdateSpecificActor(stream))
		{
			return false;
		}
		bool flag;
		if (!CrittersManager.ValidateDataType<bool>(stream.ReceiveNext(), out flag))
		{
			return false;
		}
		this.SetHasCritter(flag);
		return true;
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00008AE9 File Offset: 0x00006CE9
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(this.hasCritter);
		return this.TotalActorDataLength();
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00008238 File Offset: 0x00006438
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 1;
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00008B0C File Offset: 0x00006D0C
	public override int UpdateFromRPC(object[] data, int startingIndex)
	{
		startingIndex += base.UpdateFromRPC(data, startingIndex);
		bool flag;
		if (!CrittersManager.ValidateDataType<bool>(data[startingIndex], out flag))
		{
			return this.TotalActorDataLength();
		}
		this.SetHasCritter(flag);
		return this.TotalActorDataLength();
	}

	// Token: 0x0400017F RID: 383
	public Transform grabPosition;

	// Token: 0x04000180 RID: 384
	public Transform cagePosition;

	// Token: 0x04000181 RID: 385
	public float grabDistance;

	// Token: 0x04000182 RID: 386
	[SerializeField]
	private Vector3[] critterScales = new Vector3[]
	{
		Vector3.one
	};

	// Token: 0x04000183 RID: 387
	[SerializeField]
	private float releaseCooldown = 0.25f;

	// Token: 0x04000184 RID: 388
	[SerializeField]
	private AudioSource sound;

	// Token: 0x04000185 RID: 389
	[SerializeField]
	private AudioClip openSound;

	// Token: 0x04000186 RID: 390
	[SerializeField]
	private AudioClip closeSound;

	// Token: 0x04000187 RID: 391
	public GameObject lid;

	// Token: 0x04000188 RID: 392
	[NonSerialized]
	public bool heldByPlayer;

	// Token: 0x04000189 RID: 393
	[NonSerialized]
	private bool hasCritter;

	// Token: 0x0400018A RID: 394
	[NonSerialized]
	public bool inReleasingPosition;

	// Token: 0x0400018B RID: 395
	private float _releaseCooldownEnd;

	// Token: 0x0400018C RID: 396
	private bool _lidActive;
}
