using System;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000206 RID: 518
public static class GTPosRotScaleToString
{
	// Token: 0x06000C2C RID: 3116 RVA: 0x0009D99C File Offset: 0x0009BB9C
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

	// Token: 0x06000C2D RID: 3117 RVA: 0x000387FB File Offset: 0x000369FB
	private static string ValToStr(Vector3 v)
	{
		return string.Format("({0:R}, {1:R}, {2:R})", v.x, v.y, v.z);
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x00038828 File Offset: 0x00036A28
	public static bool ParseIsWorldSpace(string input)
	{
		return input.Contains("WorldPRS");
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x0009DA28 File Offset: 0x0009BC28
	public static string ParseParentPath(string input)
	{
		MatchCollection matchCollection = Regex.Matches(input, "parent\\s*=\\s*\"(?<parent>.*?)\"");
		if (matchCollection.Count <= 0)
		{
			return null;
		}
		return matchCollection[0].Groups["parent"].Value;
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x00038835 File Offset: 0x00036A35
	public static bool TryParsePos(string input, out Vector3 v)
	{
		return GTPosRotScaleToString.TryParseVec3_internal(GTRegex.k_Pos, input, out v);
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x00038843 File Offset: 0x00036A43
	public static bool TryParseRot(string input, out Vector3 v)
	{
		return GTPosRotScaleToString.TryParseVec3_internal(GTRegex.k_Rot, input, out v);
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x00038851 File Offset: 0x00036A51
	public static bool TryParseScale(string input, out Vector3 v)
	{
		return GTPosRotScaleToString.TryParseVec3_internal(GTRegex.k_Scale, input, out v) || GTPosRotScaleToString.TryParseVec3_internal(GTRegex.k_Vec3, input, out v);
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x0003886F File Offset: 0x00036A6F
	public static bool TryParseVec3(string input, out Vector3 v)
	{
		return GTPosRotScaleToString.TryParseVec3_internal(GTRegex.k_Vec3, input, out v);
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x0009DA68 File Offset: 0x0009BC68
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

	// Token: 0x06000C35 RID: 3125 RVA: 0x0009DAA8 File Offset: 0x0009BCA8
	private static Vector3 StringToVector3(Match match)
	{
		float x = float.Parse(match.Groups["x"].Value);
		float y = float.Parse(match.Groups["y"].Value);
		float z = float.Parse(match.Groups["z"].Value);
		return new Vector3(x, y, z);
	}

	// Token: 0x04000E83 RID: 3715
	public const string k_LocalPRSLabel = "LocalPRS";

	// Token: 0x04000E84 RID: 3716
	public const string k_WorldPRSLabel = "WorldPRS";
}
