using System;
using UnityEngine;

// Token: 0x0200043B RID: 1083
public class HotPepperFace : MonoBehaviour
{
	// Token: 0x06001AA8 RID: 6824 RVA: 0x00083748 File Offset: 0x00081948
	public void PlayFX(float delay)
	{
		if (delay < 0f)
		{
			this.PlayFX();
			return;
		}
		base.Invoke("PlayFX", delay);
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x00083768 File Offset: 0x00081968
	public void PlayFX()
	{
		this._faceMesh.SetActive(true);
		this._thermalSourceVolume.SetActive(true);
		this._fireFX.Play();
		this._flameSpeaker.GTPlay();
		this._breathSpeaker.GTPlay();
		base.Invoke("StopFX", this._effectLength);
	}

	// Token: 0x06001AAA RID: 6826 RVA: 0x000837BF File Offset: 0x000819BF
	public void StopFX()
	{
		this._faceMesh.SetActive(false);
		this._thermalSourceVolume.SetActive(false);
		this._fireFX.Stop();
		this._flameSpeaker.GTStop();
		this._breathSpeaker.GTStop();
	}

	// Token: 0x04001D70 RID: 7536
	[SerializeField]
	private GameObject _faceMesh;

	// Token: 0x04001D71 RID: 7537
	[SerializeField]
	private ParticleSystem _fireFX;

	// Token: 0x04001D72 RID: 7538
	[SerializeField]
	private AudioSource _flameSpeaker;

	// Token: 0x04001D73 RID: 7539
	[SerializeField]
	private AudioSource _breathSpeaker;

	// Token: 0x04001D74 RID: 7540
	[SerializeField]
	private float _effectLength = 1.5f;

	// Token: 0x04001D75 RID: 7541
	[SerializeField]
	private GameObject _thermalSourceVolume;
}
