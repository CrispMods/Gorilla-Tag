using System;
using Unity.Burst;

// Token: 0x02000768 RID: 1896
public static class LuaHashing
{
	// Token: 0x06002E79 RID: 11897 RVA: 0x000E1B74 File Offset: 0x000DFD74
	[BurstCompile]
	public unsafe static int ByteHash(byte* bytes, int len)
	{
		int num = 352654597;
		int num2 = num;
		for (int i = 0; i < len; i += 2)
		{
			num = ((num << 5) + num ^ (int)bytes[i]);
			if (i == len - 1)
			{
				break;
			}
			num2 = ((num2 << 5) + num2 ^ (int)bytes[i + 1]);
		}
		return num + num2 * 1648465312;
	}

	// Token: 0x06002E7A RID: 11898 RVA: 0x000E1BBC File Offset: 0x000DFDBC
	[BurstCompile]
	public unsafe static int ByteHash(byte* bytes)
	{
		int num = 352654597;
		int num2 = num;
		int num3 = 0;
		while (bytes[num3] != 0)
		{
			num = ((num << 5) + num ^ (int)bytes[num3]);
			num3++;
			if (bytes[num3] == 0)
			{
				break;
			}
			num2 = ((num2 << 5) + num2 ^ (int)bytes[num3]);
			num3++;
		}
		return num + num2 * 1648465312;
	}

	// Token: 0x06002E7B RID: 11899 RVA: 0x000E1C08 File Offset: 0x000DFE08
	public static int ByteHash(string bytes)
	{
		int length = bytes.Length;
		int num = 352654597;
		int num2 = num;
		for (int i = 0; i < length; i += 2)
		{
			num = ((num << 5) + num ^ (int)bytes[i]);
			if (i == length - 1)
			{
				break;
			}
			num2 = ((num2 << 5) + num2 ^ (int)bytes[i + 1]);
		}
		return num + num2 * 1648465312;
	}

	// Token: 0x06002E7C RID: 11900 RVA: 0x000E1C60 File Offset: 0x000DFE60
	[BurstCompile]
	public static int ByteHash(byte[] bytes)
	{
		int num = bytes.Length;
		int num2 = 352654597;
		int num3 = num2;
		for (int i = 0; i < num; i += 2)
		{
			num2 = ((num2 << 5) + num2 ^ (int)bytes[i]);
			if (i == num - 1)
			{
				break;
			}
			num3 = ((num3 << 5) + num3 ^ (int)bytes[i + 1]);
		}
		return num2 + num3 * 1648465312;
	}

	// Token: 0x040032DC RID: 13020
	private const int k_enhancer = 1648465312;

	// Token: 0x040032DD RID: 13021
	private const int k_Seed = 352654597;
}
