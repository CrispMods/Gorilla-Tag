﻿using System;
using System.Collections;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class CrittersCageDeposit : CrittersActorDeposit
{
	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000158 RID: 344 RVA: 0x0006DF18 File Offset: 0x0006C118
	// (remove) Token: 0x06000159 RID: 345 RVA: 0x0006DF50 File Offset: 0x0006C150
	public event Action<Menagerie.CritterData, int> OnDepositCritter;

	// Token: 0x0600015A RID: 346 RVA: 0x000313D5 File Offset: 0x0002F5D5
	private void Awake()
	{
		this.attachPoint.OnGrabbedChild += this.StartProcessCage;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x000313EE File Offset: 0x0002F5EE
	protected override bool CanDeposit(CrittersActor depositActor)
	{
		return base.CanDeposit(depositActor) && !this.isHandlingDeposit;
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00031404 File Offset: 0x0002F604
	private void StartProcessCage(CrittersActor depositedActor)
	{
		this.currentCage = depositedActor;
		base.StartCoroutine(this.ProcessCage());
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0003141A File Offset: 0x0002F61A
	private IEnumerator ProcessCage()
	{
		this.isHandlingDeposit = true;
		bool isLocalDeposit = this.currentCage.lastGrabbedPlayer == PhotonNetwork.LocalPlayer.ActorNumber;
		this.depositAudio.GTPlayOneShot(this.depositStartSound, isLocalDeposit ? 1f : 0.5f);
		float transition = 0f;
		CrittersPawn crittersPawn = this.currentCage.GetComponentInChildren<CrittersPawn>();
		int lastGrabbedPlayer = this.currentCage.lastGrabbedPlayer;
		Menagerie.CritterData critterData;
		if (crittersPawn.IsNotNull())
		{
			critterData = new Menagerie.CritterData(crittersPawn.visuals);
		}
		else
		{
			critterData = new Menagerie.CritterData();
		}
		while (transition < this.submitDuration)
		{
			transition += Time.deltaTime;
			this.attachPoint.transform.localPosition = Vector3.Lerp(this.depositStartLocation, this.depositEndLocation, Mathf.Min(transition / this.submitDuration, 1f));
			yield return null;
		}
		if (crittersPawn.IsNotNull())
		{
			Action<Menagerie.CritterData, int> onDepositCritter = this.OnDepositCritter;
			if (onDepositCritter != null)
			{
				onDepositCritter(critterData, lastGrabbedPlayer);
			}
			CrittersActor crittersActor = crittersPawn;
			bool keepWorldPosition = false;
			Vector3 zero = Vector3.zero;
			crittersActor.Released(keepWorldPosition, default(Quaternion), zero, default(Vector3), default(Vector3));
			crittersPawn.gameObject.SetActive(false);
			this.depositAudio.GTPlayOneShot(this.depositCritterSound, isLocalDeposit ? 1f : 0.5f);
		}
		else
		{
			this.depositAudio.GTPlayOneShot(this.depositEmptySound, isLocalDeposit ? 1f : 0.5f);
		}
		this.currentCage.transform.position = Vector3.zero;
		this.currentCage.gameObject.SetActive(false);
		this.currentCage = null;
		transition = 0f;
		while (transition < this.returnDuration)
		{
			transition += Time.deltaTime;
			this.attachPoint.transform.localPosition = Vector3.Lerp(this.depositEndLocation, this.depositStartLocation, Mathf.Min(transition / this.returnDuration, 1f));
			yield return null;
		}
		this.isHandlingDeposit = false;
		yield break;
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0006DF88 File Offset: 0x0006C188
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.TransformPoint(this.depositStartLocation), 0.1f);
		Gizmos.DrawLine(base.transform.TransformPoint(this.depositStartLocation), base.transform.TransformPoint(this.depositEndLocation));
		Gizmos.DrawWireSphere(base.transform.TransformPoint(this.depositEndLocation), 0.1f);
	}

	// Token: 0x040001A0 RID: 416
	private bool isHandlingDeposit;

	// Token: 0x040001A1 RID: 417
	public Vector3 depositStartLocation;

	// Token: 0x040001A2 RID: 418
	public Vector3 depositEndLocation;

	// Token: 0x040001A3 RID: 419
	public float submitDuration = 0.5f;

	// Token: 0x040001A4 RID: 420
	public float returnDuration = 1f;

	// Token: 0x040001A5 RID: 421
	public AudioSource depositAudio;

	// Token: 0x040001A6 RID: 422
	public AudioClip depositStartSound;

	// Token: 0x040001A7 RID: 423
	public AudioClip depositEmptySound;

	// Token: 0x040001A8 RID: 424
	public AudioClip depositCritterSound;

	// Token: 0x040001A9 RID: 425
	private CrittersActor currentCage;
}
