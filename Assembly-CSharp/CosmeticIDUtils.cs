using System;
using Unity.Mathematics;

// Token: 0x0200018B RID: 395
public static class CosmeticIDUtils
{
	// Token: 0x060009E9 RID: 2537 RVA: 0x00093258 File Offset: 0x00091458
	public static int PlayFabIdToIndexInCategory(string playFabIdString)
	{
		if (playFabIdString == null || playFabIdString.Length < 6)
		{
			throw new ArgumentException("PlayFabIdToIndexInCategory: playFabId cannot be null or less than 6 characters.");
		}
		if (playFabIdString[0] != 'L' || playFabIdString[playFabIdString.Length - 1] != '.')
		{
			throw new ArgumentException("PlayFabIdToIndexInCategory: playFabId must start with 'L' and end with '.', instead got " + playFabIdString + ".");
		}
		int num = playFabIdString.Length - 2;
		int num2 = 0;
		for (int i = 2; i <= num; i++)
		{
			char c = playFabIdString[i];
			if (c < 'A' || c > 'Z')
			{
				throw new ArgumentException("String must contain only uppercase letters A-Z.");
			}
			int num3 = (int)(playFabIdString[i] - 'A');
			num2 += num3 * (int)math.pow(26f, (float)(num - i));
		}
		return num2;
	}
}
