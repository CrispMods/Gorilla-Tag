using System;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class DJDeckEqualizer : MonoBehaviour
{
	// Token: 0x060008CA RID: 2250 RVA: 0x00036420 File Offset: 0x00034620
	private void Start()
	{
		this.inputColorHash = this.inputColorProperty;
		this.material = this.display.material;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0008F8FC File Offset: 0x0008DAFC
	private void Update()
	{
		Color value = default(Color);
		value.r = 0.25f;
		value.g = 0.25f;
		value.b = 0.5f;
		for (int i = 0; i < this.redTracks.Length; i++)
		{
			AudioSource audioSource = this.redTracks[i];
			if (audioSource.isPlaying)
			{
				value.r = Mathf.Lerp(0.25f, 1f, this.redTrackCurves[i].Evaluate(audioSource.time));
				break;
			}
		}
		for (int j = 0; j < this.greenTracks.Length; j++)
		{
			AudioSource audioSource2 = this.greenTracks[j];
			if (audioSource2.isPlaying)
			{
				value.g = Mathf.Lerp(0.25f, 1f, this.greenTrackCurves[j].Evaluate(audioSource2.time));
				break;
			}
		}
		this.material.SetColor(this.inputColorHash, value);
	}

	// Token: 0x04000A67 RID: 2663
	[SerializeField]
	private MeshRenderer display;

	// Token: 0x04000A68 RID: 2664
	[SerializeField]
	private AnimationCurve[] redTrackCurves;

	// Token: 0x04000A69 RID: 2665
	[SerializeField]
	private AnimationCurve[] greenTrackCurves;

	// Token: 0x04000A6A RID: 2666
	[SerializeField]
	private AudioSource[] redTracks;

	// Token: 0x04000A6B RID: 2667
	[SerializeField]
	private AudioSource[] greenTracks;

	// Token: 0x04000A6C RID: 2668
	private Material material;

	// Token: 0x04000A6D RID: 2669
	[SerializeField]
	private string inputColorProperty;

	// Token: 0x04000A6E RID: 2670
	private ShaderHashId inputColorHash;
}
