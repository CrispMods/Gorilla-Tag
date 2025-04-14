using System;
using UnityEngine;

// Token: 0x0200043B RID: 1083
public class HotPepperFace : MonoBehaviour
{
	// Token: 0x06001AA5 RID: 6821 RVA: 0x000833C4 File Offset: 0x000815C4
	public void PlayFX(float delay)
	{
		if (delay < 0f)
		{
			this.PlayFX();
			return;
		}
		base.Invoke("PlayFX", delay);
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x000833E4 File Offset: 0x000815E4
	public void PlayFX()
	{
		this._faceMesh.SetActive(true);
		this._thermalSourceVolume.SetActive(true);
		this._fireFX.Play();
		this._flameSpeaker.GTPlay();
		this._breathSpeaker.GTPlay();
		base.Invoke("StopFX", this._effectLength);
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x0008343B File Offset: 0x0008163B
	public void StopFX()
	{
		this._faceMesh.SetActive(false);
		this._thermalSourceVolume.SetActive(false);
		this._fireFX.Stop();
		this._flameSpeaker.GTStop();
		this._breathSpeaker.GTStop();
	}

	// Token: 0x04001D6F RID: 7535
	[SerializeField]
	private GameObject _faceMesh;

	// Token: 0x04001D70 RID: 7536
	[SerializeField]
	private ParticleSystem _fireFX;

	// Token: 0x04001D71 RID: 7537
	[SerializeField]
	private AudioSource _flameSpeaker;

	// Token: 0x04001D72 RID: 7538
	[SerializeField]
	private AudioSource _breathSpeaker;

	// Token: 0x04001D73 RID: 7539
	[SerializeField]
	private float _effectLength = 1.5f;

	// Token: 0x04001D74 RID: 7540
	[SerializeField]
	private GameObject _thermalSourceVolume;
}
