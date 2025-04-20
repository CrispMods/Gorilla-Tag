using System;
using UnityEngine;

// Token: 0x02000447 RID: 1095
public class HotPepperFace : MonoBehaviour
{
	// Token: 0x06001AF9 RID: 6905 RVA: 0x00042503 File Offset: 0x00040703
	public void PlayFX(float delay)
	{
		if (delay < 0f)
		{
			this.PlayFX();
			return;
		}
		base.Invoke("PlayFX", delay);
	}

	// Token: 0x06001AFA RID: 6906 RVA: 0x000D8398 File Offset: 0x000D6598
	public void PlayFX()
	{
		this._faceMesh.SetActive(true);
		this._thermalSourceVolume.SetActive(true);
		this._fireFX.Play();
		this._flameSpeaker.GTPlay();
		this._breathSpeaker.GTPlay();
		base.Invoke("StopFX", this._effectLength);
	}

	// Token: 0x06001AFB RID: 6907 RVA: 0x00042520 File Offset: 0x00040720
	public void StopFX()
	{
		this._faceMesh.SetActive(false);
		this._thermalSourceVolume.SetActive(false);
		this._fireFX.Stop();
		this._flameSpeaker.GTStop();
		this._breathSpeaker.GTStop();
	}

	// Token: 0x04001DBE RID: 7614
	[SerializeField]
	private GameObject _faceMesh;

	// Token: 0x04001DBF RID: 7615
	[SerializeField]
	private ParticleSystem _fireFX;

	// Token: 0x04001DC0 RID: 7616
	[SerializeField]
	private AudioSource _flameSpeaker;

	// Token: 0x04001DC1 RID: 7617
	[SerializeField]
	private AudioSource _breathSpeaker;

	// Token: 0x04001DC2 RID: 7618
	[SerializeField]
	private float _effectLength = 1.5f;

	// Token: 0x04001DC3 RID: 7619
	[SerializeField]
	private GameObject _thermalSourceVolume;
}
