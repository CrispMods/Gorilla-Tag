using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class CrittersNoiseMaker : CrittersToolThrowable
{
	// Token: 0x060001FB RID: 507 RVA: 0x00031981 File Offset: 0x0002FB81
	protected override void OnImpact(Vector3 hitPosition, Vector3 hitNormal)
	{
		if (CrittersManager.instance.LocalAuthority())
		{
			if (this.destroyOnImpact || this.playOnce)
			{
				this.PlaySingleNoise();
				return;
			}
			this.StartPlayingRepeatNoise();
		}
	}

	// Token: 0x060001FC RID: 508 RVA: 0x000319AE File Offset: 0x0002FBAE
	protected override void OnImpactCritter(CrittersPawn impactedCritter)
	{
		this.OnImpact(impactedCritter.transform.position, impactedCritter.transform.up);
	}

	// Token: 0x060001FD RID: 509 RVA: 0x000319CC File Offset: 0x0002FBCC
	protected override void OnPickedUp()
	{
		this.StopPlayRepeatNoise();
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0007165C File Offset: 0x0006F85C
	private void PlaySingleNoise()
	{
		CrittersLoudNoise crittersLoudNoise = (CrittersLoudNoise)CrittersManager.instance.SpawnActor(CrittersActor.CrittersActorType.LoudNoise, this.soundSubIndex);
		if (crittersLoudNoise == null)
		{
			return;
		}
		crittersLoudNoise.MoveActor(base.transform.position, base.transform.rotation, false, true, true);
		crittersLoudNoise.SetImpulseVelocity(Vector3.zero, Vector3.zero);
		CrittersManager.instance.TriggerEvent(CrittersManager.CritterEvent.NoiseMakerTriggered, this.actorId, base.transform.position);
	}

	// Token: 0x060001FF RID: 511 RVA: 0x000319D4 File Offset: 0x0002FBD4
	private void StartPlayingRepeatNoise()
	{
		this.StopPlayRepeatNoise();
		this.repeatPlayNoise = base.StartCoroutine(this.PlayRepeatNoise());
	}

	// Token: 0x06000200 RID: 512 RVA: 0x000319EE File Offset: 0x0002FBEE
	private void StopPlayRepeatNoise()
	{
		if (this.repeatPlayNoise != null)
		{
			base.StopCoroutine(this.repeatPlayNoise);
			this.repeatPlayNoise = null;
		}
	}

	// Token: 0x06000201 RID: 513 RVA: 0x00031A0B File Offset: 0x0002FC0B
	private IEnumerator PlayRepeatNoise()
	{
		int num = Mathf.FloorToInt(this.repeatNoiseDuration / this.repeatNoiseRate);
		int num2;
		for (int i = num; i > 0; i = num2 - 1)
		{
			this.PlaySingleNoise();
			yield return new WaitForSeconds(this.repeatNoiseRate);
			num2 = i;
		}
		if (this.destroyAfterPlayingRepeatNoise)
		{
			this.shouldDisable = true;
		}
		yield break;
	}

	// Token: 0x04000266 RID: 614
	[Header("Noise Maker")]
	public int soundSubIndex = 3;

	// Token: 0x04000267 RID: 615
	public bool playOnce = true;

	// Token: 0x04000268 RID: 616
	public float repeatNoiseDuration;

	// Token: 0x04000269 RID: 617
	public float repeatNoiseRate;

	// Token: 0x0400026A RID: 618
	public bool destroyAfterPlayingRepeatNoise = true;

	// Token: 0x0400026B RID: 619
	private Coroutine repeatPlayNoise;
}
