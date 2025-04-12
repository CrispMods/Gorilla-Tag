using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Cysharp.Text;
using UnityEngine;

// Token: 0x02000887 RID: 2183
public static class StringUtils
{
	// Token: 0x060034DF RID: 13535 RVA: 0x00051E48 File Offset: 0x00050048
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrEmpty(this string s)
	{
		return string.IsNullOrEmpty(s);
	}

	// Token: 0x060034E0 RID: 13536 RVA: 0x00051E50 File Offset: 0x00050050
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrWhiteSpace(this string s)
	{
		return string.IsNullOrWhiteSpace(s);
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x0013DBA4 File Offset: 0x0013BDA4
	public static string ToAlphaNumeric(this string s)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			return string.Empty;
		}
		string result;
		using (Utf16ValueStringBuilder utf16ValueStringBuilder = ZString.CreateStringBuilder())
		{
			foreach (char c in s)
			{
				if (char.IsLetterOrDigit(c))
				{
					utf16ValueStringBuilder.Append(c);
				}
			}
			result = utf16ValueStringBuilder.ToString();
		}
		return result;
	}

	// Token: 0x060034E2 RID: 13538 RVA: 0x0013DC20 File Offset: 0x0013BE20
	public static string Capitalize(this string s)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			return s;
		}
		char[] array = s.ToCharArray();
		array[0] = char.ToUpperInvariant(array[0]);
		return new string(array);
	}

	// Token: 0x060034E3 RID: 13539 RVA: 0x00051E58 File Offset: 0x00050058
	public static string Concat(this IEnumerable<string> source)
	{
		return string.Concat(source);
	}

	// Token: 0x060034E4 RID: 13540 RVA: 0x00051E60 File Offset: 0x00050060
	public static string Join(this IEnumerable<string> source, string separator)
	{
		return string.Join(separator, source);
	}

	// Token: 0x060034E5 RID: 13541 RVA: 0x00051E69 File Offset: 0x00050069
	public static string Join(this IEnumerable<string> source, char separator)
	{
		return string.Join<string>(separator, source);
	}

	// Token: 0x060034E6 RID: 13542 RVA: 0x00051E72 File Offset: 0x00050072
	public static string RemoveAll(this string s, string value, StringComparison mode = StringComparison.OrdinalIgnoreCase)
	{
		if (string.IsNullOrEmpty(s))
		{
			return s;
		}
		return s.Replace(value, string.Empty, mode);
	}

	// Token: 0x060034E7 RID: 13543 RVA: 0x00051E8B File Offset: 0x0005008B
	public static string RemoveAll(this string s, char value, StringComparison mode = StringComparison.OrdinalIgnoreCase)
	{
		return s.RemoveAll(value.ToString(), mode);
	}

	// Token: 0x060034E8 RID: 13544 RVA: 0x00051E9B File Offset: 0x0005009B
	public static byte[] ToBytesASCII(this string s)
	{
		return Encoding.ASCII.GetBytes(s);
	}

	// Token: 0x060034E9 RID: 13545 RVA: 0x00051EA8 File Offset: 0x000500A8
	public static byte[] ToBytesUTF8(this string s)
	{
		return Encoding.UTF8.GetBytes(s);
	}

	// Token: 0x060034EA RID: 13546 RVA: 0x00051EB5 File Offset: 0x000500B5
	public static byte[] ToBytesUnicode(this string s)
	{
		return Encoding.Unicode.GetBytes(s);
	}

	// Token: 0x060034EB RID: 13547 RVA: 0x0013DC50 File Offset: 0x0013BE50
	public static string ComputeSHV2(this string s)
	{
		return Hash128.Compute(s).ToString();
	}

	// Token: 0x060034EC RID: 13548 RVA: 0x00051EC2 File Offset: 0x000500C2
	public static string ToQueryString(this Dictionary<string, string> d)
	{
		if (d == null)
		{
			return null;
		}
		return "?" + string.Join("&", from x in d
		select x.Key + "=" + x.Value);
	}

	// Token: 0x060034ED RID: 13549 RVA: 0x0013DC74 File Offset: 0x0013BE74
	public static string Combine(string separator, params string[] values)
	{
		if (values == null || values.Length == 0)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = !string.IsNullOrEmpty(separator);
		for (int i = 0; i < values.Length; i++)
		{
			if (flag)
			{
				stringBuilder.Append(separator);
			}
			stringBuilder.Append(values);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060034EE RID: 13550 RVA: 0x0013DCC4 File Offset: 0x0013BEC4
	public static string ToUpperCamelCase(this string input)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			return string.Empty;
		}
		string[] array = Regex.Split(input, "[^A-Za-z0-9]+");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Length > 0)
			{
				string[] array2 = array;
				int num = i;
				string str = char.ToUpper(array[i][0]).ToString();
				string str2;
				if (array[i].Length <= 1)
				{
					str2 = "";
				}
				else
				{
					string text = array[i];
					str2 = text.Substring(1, text.Length - 1).ToLower();
				}
				array2[num] = str + str2;
			}
		}
		return string.Join("", array);
	}

	// Token: 0x060034EF RID: 13551 RVA: 0x0013DD58 File Offset: 0x0013BF58
	public static string ToUpperCaseFromCamelCase(this string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		input = input.Trim();
		string result;
		using (Utf16ValueStringBuilder utf16ValueStringBuilder = ZString.CreateStringBuilder())
		{
			bool flag = true;
			foreach (char c in input)
			{
				if (char.IsUpper(c) && !flag)
				{
					utf16ValueStringBuilder.Append(' ');
				}
				utf16ValueStringBuilder.Append(char.ToUpper(c));
				flag = char.IsUpper(c);
			}
			result = utf16ValueStringBuilder.ToString().Trim();
		}
		return result;
	}

	// Token: 0x060034F0 RID: 13552 RVA: 0x00051F02 File Offset: 0x00050102
	public static string RemoveStart(this string s, string value, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
	{
		if (string.IsNullOrEmpty(s) || !s.StartsWith(value, comparison))
		{
			return s;
		}
		return s.Substring(value.Length);
	}

	// Token: 0x060034F1 RID: 13553 RVA: 0x00051F24 File Offset: 0x00050124
	public static string RemoveEnd(this string s, string value, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
	{
		if (string.IsNullOrEmpty(s) || !s.EndsWith(value, comparison))
		{
			return s;
		}
		return s.Substring(0, s.Length - value.Length);
	}

	// Token: 0x060034F2 RID: 13554 RVA: 0x00051F4E File Offset: 0x0005014E
	public static string RemoveBothEnds(this string s, string value, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
	{
		return s.RemoveEnd(value, comparison).RemoveStart(value, comparison);
	}

	// Token: 0x0400378A RID: 14218
	public const string kForwardSlash = "/";

	// Token: 0x0400378B RID: 14219
	public const string kBackSlash = "/";

	// Token: 0x0400378C RID: 14220
	public const string kBackTick = "`";

	// Token: 0x0400378D RID: 14221
	public const string kMinusDash = "-";

	// Token: 0x0400378E RID: 14222
	public const string kPeriod = ".";

	// Token: 0x0400378F RID: 14223
	public const string kUnderScore = "_";

	// Token: 0x04003790 RID: 14224
	public const string kColon = ":";
}
