using System;
using System.Collections;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class CrittersCageDeposit : CrittersActorDeposit
{
	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000143 RID: 323 RVA: 0x0006BCD4 File Offset: 0x00069ED4
	// (remove) Token: 0x06000144 RID: 324 RVA: 0x0006BD0C File Offset: 0x00069F0C
	public event Action<CrittersPawn, int> OnDepositCritter;

	// Token: 0x06000145 RID: 325 RVA: 0x000303E0 File Offset: 0x0002E5E0
	private void Awake()
	{
		this.attachPoint.OnGrabbedChild += this.StartProcessCage;
	}

	// Token: 0x06000146 RID: 326 RVA: 0x000303F9 File Offset: 0x0002E5F9
	protected override bool CanDeposit(CrittersActor depositActor)
	{
		return base.CanDeposit(depositActor) && !this.isHandlingDeposit;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0003040F File Offset: 0x0002E60F
	private void StartProcessCage(CrittersActor depositedActor)
	{
		this.currentCage = depositedActor;
		base.StartCoroutine(this.ProcessCage());
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00030425 File Offset: 0x0002E625
	private IEnumerator ProcessCage()
	{
		this.isHandlingDeposit = true;
		bool isLocalDeposit = this.currentCage.lastGrabbedPlayer == PhotonNetwork.LocalPlayer.ActorNumber;
		this.depositAudio.GTPlayOneShot(this.depositStartSound, isLocalDeposit ? 1f : 0.5f);
		float transition = 0f;
		while (transition < this.submitDuration)
		{
			transition += Time.deltaTime;
			this.attachPoint.transform.localPosition = Vector3.Lerp(this.depositStartLocation, this.depositEndLocation, Mathf.Min(transition / this.submitDuration, 1f));
			yield return null;
		}
		CrittersPawn componentInChildren = this.currentCage.GetComponentInChildren<CrittersPawn>();
		if (componentInChildren.IsNotNull())
		{
			Action<CrittersPawn, int> onDepositCritter = this.OnDepositCritter;
			if (onDepositCritter != null)
			{
				onDepositCritter(componentInChildren, this.currentCage.lastGrabbedPlayer);
			}
			CrittersActor crittersActor = componentInChildren;
			bool keepWorldPosition = false;
			Vector3 zero = Vector3.zero;
			crittersActor.Released(keepWorldPosition, default(Quaternion), zero, default(Vector3), default(Vector3));
			componentInChildren.gameObject.SetActive(false);
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

	// Token: 0x06000149 RID: 329 RVA: 0x0006BD44 File Offset: 0x00069F44
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.TransformPoint(this.depositStartLocation), 0.1f);
		Gizmos.DrawLine(base.transform.TransformPoint(this.depositStartLocation), base.transform.TransformPoint(this.depositEndLocation));
		Gizmos.DrawWireSphere(base.transform.TransformPoint(this.depositEndLocation), 0.1f);
	}

	// Token: 0x0400018D RID: 397
	private bool isHandlingDeposit;

	// Token: 0x0400018E RID: 398
	public Vector3 depositStartLocation;

	// Token: 0x0400018F RID: 399
	public Vector3 depositEndLocation;

	// Token: 0x04000190 RID: 400
	public float submitDuration = 0.5f;

	// Token: 0x04000191 RID: 401
	public float returnDuration = 1f;

	// Token: 0x04000192 RID: 402
	public AudioSource depositAudio;

	// Token: 0x04000193 RID: 403
	public AudioClip depositStartSound;

	// Token: 0x04000194 RID: 404
	public AudioClip depositEmptySound;

	// Token: 0x04000195 RID: 405
	public AudioClip depositCritterSound;

	// Token: 0x04000196 RID: 406
	private CrittersActor currentCage;
}
