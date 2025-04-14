using System;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public static class GTAudioClipExtensions
{
	// Token: 0x060009FB RID: 2555 RVA: 0x000376F4 File Offset: 0x000358F4
	public static float GetPeakMagnitude(this AudioClip audioClip)
	{
		if (audioClip == null)
		{
			return 0f;
		}
		float num = float.NegativeInfinity;
		float[] array = new float[audioClip.samples];
		audioClip.GetData(array, 0);
		foreach (float f in array)
		{
			num = Mathf.Max(num, Mathf.Abs(f));
		}
		return num;
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x00037750 File Offset: 0x00035950
	public static float GetRMSMagnitude(this AudioClip audioClip)
	{
		if (audioClip == null)
		{
			return 0f;
		}
		float num = 0f;
		float[] array = new float[audioClip.samples];
		audioClip.GetData(array, 0);
		foreach (float num2 in array)
		{
			num += num2 * num2;
		}
		return Mathf.Sqrt(num / (float)array.Length);
	}
}
