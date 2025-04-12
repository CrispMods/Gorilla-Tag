using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class CrittersCage : CrittersActor
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000132 RID: 306 RVA: 0x0003022D File Offset: 0x0002E42D
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
	// (get) Token: 0x06000133 RID: 307 RVA: 0x0003025F File Offset: 0x0002E45F
	public bool CanCatch
	{
		get
		{
			return this.heldByPlayer && !this.hasCritter && !this.inReleasingPosition && this._releaseCooldownEnd <= Time.time;
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x0003028B File Offset: 0x0002E48B
	public void SetHasCritter(bool value)
	{
		if (this.hasCritter != value && !value)
		{
			this._releaseCooldownEnd = Time.time + this.releaseCooldown;
		}
		this.hasCritter = value;
		this.UpdateCageVisuals();
	}

	// Token: 0x06000135 RID: 309 RVA: 0x000302B8 File Offset: 0x0002E4B8
	public override void Initialize()
	{
		base.Initialize();
		this.hasCritter = false;
		this.heldByPlayer = false;
		this.inReleasingPosition = false;
		this.SetLidActive(true, false);
	}

	// Token: 0x06000136 RID: 310 RVA: 0x000302DD File Offset: 0x0002E4DD
	private void UpdateCageVisuals()
	{
		this.SetLidActive(!this.heldByPlayer || this.hasCritter, true);
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0006BC10 File Offset: 0x00069E10
	private void SetLidActive(bool active, bool playAudio = true)
	{
		if (active != this._lidActive && playAudio)
		{
			this.sound.GTPlayOneShot(active ? this.openSound : this.closeSound, 1f);
		}
		this.lid.SetActive(active);
		this._lidActive = active;
	}

	// Token: 0x06000138 RID: 312 RVA: 0x000302F7 File Offset: 0x0002E4F7
	protected override void RemoteGrabbedBy(CrittersActor grabbingActor)
	{
		base.RemoteGrabbed(grabbingActor);
		this.heldByPlayer = grabbingActor.isOnPlayer;
		this.UpdateCageVisuals();
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00030312 File Offset: 0x0002E512
	public override void GrabbedBy(CrittersActor grabbingActor, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
	{
		base.GrabbedBy(grabbingActor, positionOverride, localRotation, localOffset, disableGrabbing);
		this.heldByPlayer = grabbingActor.isOnPlayer;
		this.UpdateCageVisuals();
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00030333 File Offset: 0x0002E533
	public override void Released(bool keepWorldPosition, Quaternion rotation = default(Quaternion), Vector3 position = default(Vector3), Vector3 impulseVelocity = default(Vector3), Vector3 impulseAngularVelocity = default(Vector3))
	{
		base.Released(keepWorldPosition, rotation, position, impulseVelocity, impulseAngularVelocity);
		this.heldByPlayer = false;
		this.UpdateCageVisuals();
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0003034F File Offset: 0x0002E54F
	protected override void HandleRemoteReleased()
	{
		base.HandleRemoteReleased();
		this.heldByPlayer = false;
		this.UpdateCageVisuals();
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00030364 File Offset: 0x0002E564
	public override bool ShouldDespawn()
	{
		return base.ShouldDespawn() && !this.hasCritter;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00030379 File Offset: 0x0002E579
	public override void SendDataByCrittersActorType(PhotonStream stream)
	{
		base.SendDataByCrittersActorType(stream);
		stream.SendNext(this.hasCritter);
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0006BC64 File Offset: 0x00069E64
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

	// Token: 0x0600013F RID: 319 RVA: 0x00030393 File Offset: 0x0002E593
	public override int AddActorDataToList(ref List<object> objList)
	{
		base.AddActorDataToList(ref objList);
		objList.Add(this.hasCritter);
		return this.TotalActorDataLength();
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0003013F File Offset: 0x0002E33F
	public override int TotalActorDataLength()
	{
		return base.BaseActorDataLength() + 1;
	}

	// Token: 0x06000141 RID: 321 RVA: 0x0006BC98 File Offset: 0x00069E98
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
