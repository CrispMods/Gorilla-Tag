using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class ElfLauncherElf : MonoBehaviour
{
	// Token: 0x060008E8 RID: 2280 RVA: 0x00036555 File Offset: 0x00034755
	private void OnEnable()
	{
		base.StartCoroutine(this.ReturnToPoolAfterDelayCo());
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x00036564 File Offset: 0x00034764
	private IEnumerator ReturnToPoolAfterDelayCo()
	{
		yield return new WaitForSeconds(this.destroyAfterDuration);
		ObjectPools.instance.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x00036573 File Offset: 0x00034773
	private void OnCollisionEnter(Collision collision)
	{
		if (this.bounceAudioCoolingDownUntilTimestamp > Time.time)
		{
			return;
		}
		this.bounceAudio.Play();
		this.bounceAudioCoolingDownUntilTimestamp = Time.time + this.bounceAudioCooldownDuration;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x000365A0 File Offset: 0x000347A0
	private void FixedUpdate()
	{
		this.rb.AddForce(base.transform.lossyScale.x * Physics.gravity, ForceMode.Acceleration);
	}

	// Token: 0x04000A9F RID: 2719
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x04000AA0 RID: 2720
	[SerializeField]
	private SoundBankPlayer bounceAudio;

	// Token: 0x04000AA1 RID: 2721
	[SerializeField]
	private float bounceAudioCooldownDuration;

	// Token: 0x04000AA2 RID: 2722
	[SerializeField]
	private float destroyAfterDuration;

	// Token: 0x04000AA3 RID: 2723
	private float bounceAudioCoolingDownUntilTimestamp;
}
