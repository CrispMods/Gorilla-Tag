using System;
using UnityEngine;

// Token: 0x020001AE RID: 430
public static class GTAudioClipExtensions
{
	// Token: 0x06000A45 RID: 2629 RVA: 0x0009724C File Offset: 0x0009544C
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

	// Token: 0x06000A46 RID: 2630 RVA: 0x000972A8 File Offset: 0x000954A8
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
