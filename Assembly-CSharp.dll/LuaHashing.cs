using System;
using Unity.Burst;

// Token: 0x02000769 RID: 1897
public static class LuaHashing
{
	// Token: 0x06002E81 RID: 11905 RVA: 0x00126A88 File Offset: 0x00124C88
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

	// Token: 0x06002E82 RID: 11906 RVA: 0x00126AD0 File Offset: 0x00124CD0
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

	// Token: 0x06002E83 RID: 11907 RVA: 0x00126B1C File Offset: 0x00124D1C
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

	// Token: 0x06002E84 RID: 11908 RVA: 0x00126B74 File Offset: 0x00124D74
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

	// Token: 0x040032E2 RID: 13026
	private const int k_enhancer = 1648465312;

	// Token: 0x040032E3 RID: 13027
	private const int k_Seed = 352654597;
}
