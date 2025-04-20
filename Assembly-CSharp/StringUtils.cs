using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Cysharp.Text;
using UnityEngine;

// Token: 0x020008A0 RID: 2208
public static class StringUtils
{
	// Token: 0x0600359F RID: 13727 RVA: 0x00053355 File Offset: 0x00051555
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrEmpty(this string s)
	{
		return string.IsNullOrEmpty(s);
	}

	// Token: 0x060035A0 RID: 13728 RVA: 0x0005335D File Offset: 0x0005155D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrWhiteSpace(this string s)
	{
		return string.IsNullOrWhiteSpace(s);
	}

	// Token: 0x060035A1 RID: 13729 RVA: 0x0014318C File Offset: 0x0014138C
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

	// Token: 0x060035A2 RID: 13730 RVA: 0x00143208 File Offset: 0x00141408
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

	// Token: 0x060035A3 RID: 13731 RVA: 0x00053365 File Offset: 0x00051565
	public static string Concat(this IEnumerable<string> source)
	{
		return string.Concat(source);
	}

	// Token: 0x060035A4 RID: 13732 RVA: 0x0005336D File Offset: 0x0005156D
	public static string Join(this IEnumerable<string> source, string separator)
	{
		return string.Join(separator, source);
	}

	// Token: 0x060035A5 RID: 13733 RVA: 0x00053376 File Offset: 0x00051576
	public static string Join(this IEnumerable<string> source, char separator)
	{
		return string.Join<string>(separator, source);
	}

	// Token: 0x060035A6 RID: 13734 RVA: 0x0005337F File Offset: 0x0005157F
	public static string RemoveAll(this string s, string value, StringComparison mode = StringComparison.OrdinalIgnoreCase)
	{
		if (string.IsNullOrEmpty(s))
		{
			return s;
		}
		return s.Replace(value, string.Empty, mode);
	}

	// Token: 0x060035A7 RID: 13735 RVA: 0x00053398 File Offset: 0x00051598
	public static string RemoveAll(this string s, char value, StringComparison mode = StringComparison.OrdinalIgnoreCase)
	{
		return s.RemoveAll(value.ToString(), mode);
	}

	// Token: 0x060035A8 RID: 13736 RVA: 0x000533A8 File Offset: 0x000515A8
	public static byte[] ToBytesASCII(this string s)
	{
		return Encoding.ASCII.GetBytes(s);
	}

	// Token: 0x060035A9 RID: 13737 RVA: 0x000533B5 File Offset: 0x000515B5
	public static byte[] ToBytesUTF8(this string s)
	{
		return Encoding.UTF8.GetBytes(s);
	}

	// Token: 0x060035AA RID: 13738 RVA: 0x000533C2 File Offset: 0x000515C2
	public static byte[] ToBytesUnicode(this string s)
	{
		return Encoding.Unicode.GetBytes(s);
	}

	// Token: 0x060035AB RID: 13739 RVA: 0x00143238 File Offset: 0x00141438
	public static string ComputeSHV2(this string s)
	{
		return Hash128.Compute(s).ToString();
	}

	// Token: 0x060035AC RID: 13740 RVA: 0x000533CF File Offset: 0x000515CF
	public static string ToQueryString(this Dictionary<string, string> d)
	{
		if (d == null)
		{
			return null;
		}
		return "?" + string.Join("&", from x in d
		select x.Key + "=" + x.Value);
	}

	// Token: 0x060035AD RID: 13741 RVA: 0x0014325C File Offset: 0x0014145C
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

	// Token: 0x060035AE RID: 13742 RVA: 0x001432AC File Offset: 0x001414AC
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

	// Token: 0x060035AF RID: 13743 RVA: 0x00143340 File Offset: 0x00141540
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

	// Token: 0x060035B0 RID: 13744 RVA: 0x0005340F File Offset: 0x0005160F
	public static string RemoveStart(this string s, string value, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
	{
		if (string.IsNullOrEmpty(s) || !s.StartsWith(value, comparison))
		{
			return s;
		}
		return s.Substring(value.Length);
	}

	// Token: 0x060035B1 RID: 13745 RVA: 0x00053431 File Offset: 0x00051631
	public static string RemoveEnd(this string s, string value, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
	{
		if (string.IsNullOrEmpty(s) || !s.EndsWith(value, comparison))
		{
			return s;
		}
		return s.Substring(0, s.Length - value.Length);
	}

	// Token: 0x060035B2 RID: 13746 RVA: 0x0005345B File Offset: 0x0005165B
	public static string RemoveBothEnds(this string s, string value, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
	{
		return s.RemoveEnd(value, comparison).RemoveStart(value, comparison);
	}

	// Token: 0x04003838 RID: 14392
	public const string kForwardSlash = "/";

	// Token: 0x04003839 RID: 14393
	public const string kBackSlash = "/";

	// Token: 0x0400383A RID: 14394
	public const string kBackTick = "`";

	// Token: 0x0400383B RID: 14395
	public const string kMinusDash = "-";

	// Token: 0x0400383C RID: 14396
	public const string kPeriod = ".";

	// Token: 0x0400383D RID: 14397
	public const string kUnderScore = "_";

	// Token: 0x0400383E RID: 14398
	public const string kColon = ":";
}
