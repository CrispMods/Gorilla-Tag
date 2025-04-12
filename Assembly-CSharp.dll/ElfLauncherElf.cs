using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class ElfLauncherElf : MonoBehaviour
{
	// Token: 0x060008A6 RID: 2214 RVA: 0x000352DF File Offset: 0x000334DF
	private void OnEnable()
	{
		base.StartCoroutine(this.ReturnToPoolAfterDelayCo());
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x000352EE File Offset: 0x000334EE
	private IEnumerator ReturnToPoolAfterDelayCo()
	{
		yield return new WaitForSeconds(this.destroyAfterDuration);
		ObjectPools.instance.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x000352FD File Offset: 0x000334FD
	private void OnCollisionEnter(Collision collision)
	{
		if (this.bounceAudioCoolingDownUntilTimestamp > Time.time)
		{
			return;
		}
		this.bounceAudio.Play();
		this.bounceAudioCoolingDownUntilTimestamp = Time.time + this.bounceAudioCooldownDuration;
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x0003532A File Offset: 0x0003352A
	private void FixedUpdate()
	{
		this.rb.AddForce(base.transform.lossyScale.x * Physics.gravity, ForceMode.Acceleration);
	}

	// Token: 0x04000A5D RID: 2653
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x04000A5E RID: 2654
	[SerializeField]
	private SoundBankPlayer bounceAudio;

	// Token: 0x04000A5F RID: 2655
	[SerializeField]
	private float bounceAudioCooldownDuration;

	// Token: 0x04000A60 RID: 2656
	[SerializeField]
	private float destroyAfterDuration;

	// Token: 0x04000A61 RID: 2657
	private float bounceAudioCoolingDownUntilTimestamp;
}
