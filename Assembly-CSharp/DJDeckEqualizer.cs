using System;
using UnityEngine;

// Token: 0x0200014E RID: 334
public class DJDeckEqualizer : MonoBehaviour
{
	// Token: 0x06000886 RID: 2182 RVA: 0x0002EB05 File Offset: 0x0002CD05
	private void Start()
	{
		this.inputColorHash = this.inputColorProperty;
		this.material = this.display.material;
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x0002EB2C File Offset: 0x0002CD2C
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

	// Token: 0x04000A24 RID: 2596
	[SerializeField]
	private MeshRenderer display;

	// Token: 0x04000A25 RID: 2597
	[SerializeField]
	private AnimationCurve[] redTrackCurves;

	// Token: 0x04000A26 RID: 2598
	[SerializeField]
	private AnimationCurve[] greenTrackCurves;

	// Token: 0x04000A27 RID: 2599
	[SerializeField]
	private AudioSource[] redTracks;

	// Token: 0x04000A28 RID: 2600
	[SerializeField]
	private AudioSource[] greenTracks;

	// Token: 0x04000A29 RID: 2601
	private Material material;

	// Token: 0x04000A2A RID: 2602
	[SerializeField]
	private string inputColorProperty;

	// Token: 0x04000A2B RID: 2603
	private ShaderHashId inputColorHash;
}
