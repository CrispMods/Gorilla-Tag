using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class ElfLauncherElf : MonoBehaviour
{
	// Token: 0x060008A4 RID: 2212 RVA: 0x0002F61B File Offset: 0x0002D81B
	private void OnEnable()
	{
		base.StartCoroutine(this.ReturnToPoolAfterDelayCo());
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0002F62A File Offset: 0x0002D82A
	private IEnumerator ReturnToPoolAfterDelayCo()
	{
		yield return new WaitForSeconds(this.destroyAfterDuration);
		ObjectPools.instance.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0002F639 File Offset: 0x0002D839
	private void OnCollisionEnter(Collision collision)
	{
		if (this.bounceAudioCoolingDownUntilTimestamp > Time.time)
		{
			return;
		}
		this.bounceAudio.Play();
		this.bounceAudioCoolingDownUntilTimestamp = Time.time + this.bounceAudioCooldownDuration;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0002F666 File Offset: 0x0002D866
	private void FixedUpdate()
	{
		this.rb.AddForce(base.transform.lossyScale.x * Physics.gravity, ForceMode.Acceleration);
	}

	// Token: 0x04000A5C RID: 2652
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x04000A5D RID: 2653
	[SerializeField]
	private SoundBankPlayer bounceAudio;

	// Token: 0x04000A5E RID: 2654
	[SerializeField]
	private float bounceAudioCooldownDuration;

	// Token: 0x04000A5F RID: 2655
	[SerializeField]
	private float destroyAfterDuration;

	// Token: 0x04000A60 RID: 2656
	private float bounceAudioCoolingDownUntilTimestamp;
}
