using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class CrittersNoiseMaker : CrittersToolThrowable
{
	// Token: 0x060001DD RID: 477 RVA: 0x0000C459 File Offset: 0x0000A659
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

	// Token: 0x060001DE RID: 478 RVA: 0x0000C486 File Offset: 0x0000A686
	protected override void OnImpactCritter(CrittersPawn impactedCritter)
	{
		this.OnImpact(impactedCritter.transform.position, impactedCritter.transform.up);
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000C4A4 File Offset: 0x0000A6A4
	protected override void OnPickedUp()
	{
		this.StopPlayRepeatNoise();
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x0000C4AC File Offset: 0x0000A6AC
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

	// Token: 0x060001E1 RID: 481 RVA: 0x0000C529 File Offset: 0x0000A729
	private void StartPlayingRepeatNoise()
	{
		this.StopPlayRepeatNoise();
		this.repeatPlayNoise = base.StartCoroutine(this.PlayRepeatNoise());
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x0000C543 File Offset: 0x0000A743
	private void StopPlayRepeatNoise()
	{
		if (this.repeatPlayNoise != null)
		{
			base.StopCoroutine(this.repeatPlayNoise);
			this.repeatPlayNoise = null;
		}
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0000C560 File Offset: 0x0000A760
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

	// Token: 0x04000241 RID: 577
	[Header("Noise Maker")]
	public int soundSubIndex = 3;

	// Token: 0x04000242 RID: 578
	public bool playOnce = true;

	// Token: 0x04000243 RID: 579
	public float repeatNoiseDuration;

	// Token: 0x04000244 RID: 580
	public float repeatNoiseRate;

	// Token: 0x04000245 RID: 581
	public bool destroyAfterPlayingRepeatNoise = true;

	// Token: 0x04000246 RID: 582
	private Coroutine repeatPlayNoise;
}
