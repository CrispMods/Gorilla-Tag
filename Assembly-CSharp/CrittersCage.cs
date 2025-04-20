using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class CrittersCage : CrittersActor
{
	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000147 RID: 327 RVA: 0x00031222 File Offset: 0x0002F422
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

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000148 RID: 328 RVA: 0x00031254 File Offset: 0x0002F454
	public bool CanCatch
	{
		get
		{
			return this.heldByPlayer && !this.hasCritter && !this.inReleasingPosition && this._releaseCooldownEnd <= Time.time;
		}
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00031280 File Offset: 0x0002F480
	public void SetHasCritter(bool value)
	{
		if (this.hasCritter != value && !value)
		{
			this._releaseCooldownEnd = Time.time + this.releaseCooldown;
		}
		this.hasCritter = value;
		this.UpdateCageVisuals();
	}

	// Token: 0x0600014A RID: 330 RVA: 0x000312AD File Offset: 0x0002F4AD
	public override void Initialize()
	{
		base.Initialize();
		this.hasCritter = false;
		this.heldByPlayer = false;
		this.inReleasingPosition = false;
		this.SetLidActive(true, false);
	}

	// Token: 0x0600014B RID: 331 RVA: 0x000312D2 File Offset: 0x0002F4D2
	private void UpdateCageVisuals()
	{
		this.SetLidActive(!this.heldByPlayer || this.hasCritter, true);
	}

	// Token: 0x0600014C RID: 332 RVA: 0x0006DE54 File Offset: 0x0006C054
	private void SetLidActive(bool active, bool playAudio = true)
	{
		if (active != this._lidActive && playAudio)
		{
			this.sound.GTPlayOneShot(active ? this.openSound : this.closeSound, 1f);
		}
		this.lid.SetActive(active);
		this._lidActive = active;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x000312EC File Offset: 0x0002F4EC
	protected override void RemoteGrabbedBy(CrittersActor grabbingActor)
	{
		base.RemoteGrabbedBy(grabbingActor);
		this.heldByPlayer = grabbingActor.isOnPlayer;
		this.UpdateCageVisuals();
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00031307 File Offset: 0x0002F507
	public override void GrabbedBy(CrittersActor grabbingActor, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
	{
		base.GrabbedBy(grabbingActor, positionOverride, localRotation, localOffset, disableGrabbing);
		this.heldByPlayer = grabbingActor.isOnPlayer;
		this.UpdateCageVisuals();
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00031328 File Offset: 0x0002F528
	public override void Released(bool keepWorldPosition, Quaternion rotation = default(Quaternion), Vector3 position = default(Vector3), Vector3 impulseVelocity = default(Vector3), Vector3 impulseAngularVelocity = default(Vector3))
	{
		base.Released(keepWorldPosition, rotation, position, impulseVelocity, impulseAngularVelocity);
		this.heldByPlayer = false;
		this.UpdateCageVisuals();
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00031344 File Offset: 0x0002F544
	protected override void HandleRemoteReleased()
	{
		base.HandleRemoteReleased();
		this.heldByPlayer = false;
		this.UpdateCageVisuals();
	}

	// Token: 0x06000151 RID: 337 RVA: 0x00031359 File Offset: 0x0002F559
	public override bool ShouldDespawn()
	{
		return base.ShouldDespawn() && !this.hasCritter;
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0003136E File Offset: 0x0002F56E
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(this.hasCritter);
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0006DEA8 File Offset: 0x0006C0A8
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

	// Token: 0x06000154 RID: 340 RVA: 0x00031388 File Offset: 0x0002F588
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(this.hasCritter);
		return this.TotalActorDataLength();
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00031114 File Offset: 0x0002F314
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 1;
	}

	// Token: 0x06000156 RID: 342 RVA: 0x0006DEDC File Offset: 0x0006C0DC
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

	// Token: 0x04000192 RID: 402
	public Transform grabPosition;

	// Token: 0x04000193 RID: 403
	public Transform cagePosition;

	// Token: 0x04000194 RID: 404
	public float grabDistance;

	// Token: 0x04000195 RID: 405
	[SerializeField]
	private Vector3[] critterScales = new Vector3[]
	{
		Vector3.one
	};

	// Token: 0x04000196 RID: 406
	[SerializeField]
	private float releaseCooldown = 0.25f;

	// Token: 0x04000197 RID: 407
	[SerializeField]
	private AudioSource sound;

	// Token: 0x04000198 RID: 408
	[SerializeField]
	private AudioClip openSound;

	// Token: 0x04000199 RID: 409
	[SerializeField]
	private AudioClip closeSound;

	// Token: 0x0400019A RID: 410
	public GameObject lid;

	// Token: 0x0400019B RID: 411
	[NonSerialized]
	public bool heldByPlayer;

	// Token: 0x0400019C RID: 412
	[NonSerialized]
	private bool hasCritter;

	// Token: 0x0400019D RID: 413
	[NonSerialized]
	public bool inReleasingPosition;

	// Token: 0x0400019E RID: 414
	private float _releaseCooldownEnd;

	// Token: 0x0400019F RID: 415
	private bool _lidActive;
}
