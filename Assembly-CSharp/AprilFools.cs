using System;
using UnityEngine;

// Token: 0x020000D4 RID: 212
public static class AprilFools
{
	// Token: 0x06000578 RID: 1400 RVA: 0x000204AD File Offset: 0x0001E6AD
	public static int mod(int x, int m)
	{
		return (x % m + m) % m;
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x000204B8 File Offset: 0x0001E6B8
	public static float GenerateTarget(string username, string roomName, string areaName, int startTime)
	{
		float num = (float)AprilFools.mod(string.Format("{0}-{1}-{2}-{3}", new object[]
		{
			username,
			roomName,
			areaName,
			startTime
		}).GetHashCode(), 10000) / 10000f;
		float num2 = 1.5f;
		float num3 = 0.5f / 2f;
		num = num * num2 + 0.5f;
		if (num > 0.75f && num < 1.25f)
		{
			if (num < 1f)
			{
				num = 0.75f - num3;
			}
			else
			{
				num = 1.25f + num3;
			}
		}
		return num;
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x00020548 File Offset: 0x0001E748
	public static float Slerp(float a, float b, float t)
	{
		float num = Mathf.Acos(Mathf.Clamp(Vector2.Dot(new Vector2(a, b).normalized, Vector2.right), -1f, 1f)) * t;
		float num2 = Mathf.Sin(num);
		float num3 = Mathf.Sin((1f - t) * num) / num2;
		float num4 = Mathf.Sin(t * num) / num2;
		return a * num3 + b * num4;
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x000205B0 File Offset: 0x0001E7B0
	public static float SmoothSlerp(float a, float b, float t)
	{
		t = Mathf.Clamp01(t);
		float num = 0.00013888889f;
		float num2 = Mathf.Clamp01(t - num);
		float num3 = Mathf.Clamp01(t + num);
		float a2 = 3f * num2 * num2 - 2f * num2 * num2 * num2;
		float b2 = 3f * num3 * num3 - 2f * num3 * num3 * num3;
		return AprilFools.Slerp(a, b, Mathf.Lerp(a2, b2, (t - num2) / (num3 - num2)));
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x00020624 File Offset: 0x0001E824
	public static float GenerateSmoothTarget(string username, string roomName, string areaName)
	{
		float num = (float)DateTime.UtcNow.Subtract(new DateTime(2023, 3, 30)).TotalSeconds;
		int num2 = (int)num / 300;
		int num3 = num2 * 300;
		int startTime = (num2 + 1) * 300;
		float a = AprilFools.GenerateTarget(username, roomName, areaName, num3);
		float b = AprilFools.GenerateTarget(username, roomName, areaName, startTime);
		float num4 = (num - (float)num3) / 120f;
		if (num4 > 1f)
		{
			num4 = 1f;
		}
		float num5 = AprilFools.SmoothSlerp(a, b, num4);
		if (float.IsInfinity(num5))
		{
			return 0.5f;
		}
		if (float.IsNaN(num5))
		{
			return 0.5f;
		}
		if (num5 < 0.5f)
		{
			return 0.5f;
		}
		if (num5 > 2f)
		{
			return 2f;
		}
		return num5;
	}

	// Token: 0x04000666 RID: 1638
	private const int changeIntervalSeconds = 300;

	// Token: 0x04000667 RID: 1639
	private const int lerpIntervalSeconds = 120;

	// Token: 0x04000668 RID: 1640
	private const float minRange = 0.5f;

	// Token: 0x04000669 RID: 1641
	private const float maxRange = 2f;

	// Token: 0x0400066A RID: 1642
	private const float excludeRangeStart = 0.75f;

	// Token: 0x0400066B RID: 1643
	private const float excludeRangeEnd = 1.25f;
}
