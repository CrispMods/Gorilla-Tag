using System;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x020001FB RID: 507
public static class GTPosRotScaleToString
{
	// Token: 0x06000BE3 RID: 3043 RVA: 0x0003F350 File Offset: 0x0003D550
	public static string ToString(Vector3 pos, Vector3 rot, Vector3 scale, bool isWorldSpace, string parentPath = null)
	{
		string text = isWorldSpace ? "WorldPRS" : "LocalPRS";
		string str = string.Concat(new string[]
		{
			text,
			" { p=",
			GTPosRotScaleToString.ValToStr(pos),
			", r=",
			GTPosRotScaleToString.ValToStr(rot),
			", s=",
			GTPosRotScaleToString.ValToStr(scale)
		});
		if (!string.IsNullOrEmpty(parentPath))
		{
			str = str + " parent=\"" + parentPath + "\"";
		}
		return str + " }";
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x0003F3D9 File Offset: 0x0003D5D9
	private static string ValToStr(Vector3 v)
	{
		return string.Format("({0:R}, {1:R}, {2:R})", v.x, v.y, v.z);
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x0003F406 File Offset: 0x0003D606
	public static bool ParseIsWorldSpace(string input)
	{
		return input.Contains("WorldPRS");
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x0003F414 File Offset: 0x0003D614
	public static string ParseParentPath(string input)
	{
		MatchCollection matchCollection = Regex.Matches(input, "parent\\s*=\\s*\"(?<parent>.*?)\"");
		if (matchCollection.Count <= 0)
		{
			return null;
		}
		return matchCollection[0].Groups["parent"].Value;
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x0003F453 File Offset: 0x0003D653
	public static bool TryParsePos(string input, out Vector3 v)
	{
		return GTPosRotScaleToString.TryParseVec3_internal(GTRegex.k_Pos, input, out v);
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x0003F461 File Offset: 0x0003D661
	public static bool TryParseRot(string input, out Vector3 v)
	{
		return GTPosRotScaleToString.TryParseVec3_internal(GTRegex.k_Rot, input, out v);
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x0003F46F File Offset: 0x0003D66F
	public static bool TryParseScale(string input, out Vector3 v)
	{
		return GTPosRotScaleToString.TryParseVec3_internal(GTRegex.k_Scale, input, out v) || GTPosRotScaleToString.TryParseVec3_internal(GTRegex.k_Vec3, input, out v);
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x0003F48D File Offset: 0x0003D68D
	public static bool TryParseVec3(string input, out Vector3 v)
	{
		return GTPosRotScaleToString.TryParseVec3_internal(GTRegex.k_Vec3, input, out v);
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x0003F49C File Offset: 0x0003D69C
	private static bool TryParseVec3_internal(Regex regex, string input, out Vector3 v)
	{
		v = Vector3.zero;
		MatchCollection matchCollection = regex.Matches(input);
		if (matchCollection.Count <= 0)
		{
			return false;
		}
		v = GTPosRotScaleToString.StringToVector3(matchCollection[0]);
		return true;
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x0003F4DC File Offset: 0x0003D6DC
	private static Vector3 StringToVector3(Match match)
	{
		float x = float.Parse(match.Groups["x"].Value);
		float y = float.Parse(match.Groups["y"].Value);
		float z = float.Parse(match.Groups["z"].Value);
		return new Vector3(x, y, z);
	}

	// Token: 0x04000E3E RID: 3646
	public const string k_LocalPRSLabel = "LocalPRS";

	// Token: 0x04000E3F RID: 3647
	public const string k_WorldPRSLabel = "WorldPRS";
}
