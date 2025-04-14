using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Cysharp.Text;
using UnityEngine;

// Token: 0x02000884 RID: 2180
public static class StringUtils
{
	// Token: 0x060034D3 RID: 13523 RVA: 0x000FC691 File Offset: 0x000FA891
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrEmpty(this string s)
	{
		return string.IsNullOrEmpty(s);
	}

	// Token: 0x060034D4 RID: 13524 RVA: 0x000FC699 File Offset: 0x000FA899
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrWhiteSpace(this string s)
	{
		return string.IsNullOrWhiteSpace(s);
	}

	// Token: 0x060034D5 RID: 13525 RVA: 0x000FC6A4 File Offset: 0x000FA8A4
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

	// Token: 0x060034D6 RID: 13526 RVA: 0x000FC720 File Offset: 0x000FA920
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

	// Token: 0x060034D7 RID: 13527 RVA: 0x000FC74F File Offset: 0x000FA94F
	public static string Concat(this IEnumerable<string> source)
	{
		return string.Concat(source);
	}

	// Token: 0x060034D8 RID: 13528 RVA: 0x000FC757 File Offset: 0x000FA957
	public static string Join(this IEnumerable<string> source, string separator)
	{
		return string.Join(separator, source);
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x000FC760 File Offset: 0x000FA960
	public static string Join(this IEnumerable<string> source, char separator)
	{
		return string.Join<string>(separator, source);
	}

	// Token: 0x060034DA RID: 13530 RVA: 0x000FC769 File Offset: 0x000FA969
	public static string RemoveAll(this string s, string value, StringComparison mode = StringComparison.OrdinalIgnoreCase)
	{
		if (string.IsNullOrEmpty(s))
		{
			return s;
		}
		return s.Replace(value, string.Empty, mode);
	}

	// Token: 0x060034DB RID: 13531 RVA: 0x000FC782 File Offset: 0x000FA982
	public static string RemoveAll(this string s, char value, StringComparison mode = StringComparison.OrdinalIgnoreCase)
	{
		return s.RemoveAll(value.ToString(), mode);
	}

	// Token: 0x060034DC RID: 13532 RVA: 0x000FC792 File Offset: 0x000FA992
	public static byte[] ToBytesASCII(this string s)
	{
		return Encoding.ASCII.GetBytes(s);
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x000FC79F File Offset: 0x000FA99F
	public static byte[] ToBytesUTF8(this string s)
	{
		return Encoding.UTF8.GetBytes(s);
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x000FC7AC File Offset: 0x000FA9AC
	public static byte[] ToBytesUnicode(this string s)
	{
		return Encoding.Unicode.GetBytes(s);
	}

	// Token: 0x060034DF RID: 13535 RVA: 0x000FC7BC File Offset: 0x000FA9BC
	public static string ComputeSHV2(this string s)
	{
		return Hash128.Compute(s).ToString();
	}

	// Token: 0x060034E0 RID: 13536 RVA: 0x000FC7DD File Offset: 0x000FA9DD
	public static string ToQueryString(this Dictionary<string, string> d)
	{
		if (d == null)
		{
			return null;
		}
		return "?" + string.Join("&", from x in d
		select x.Key + "=" + x.Value);
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x000FC820 File Offset: 0x000FAA20
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

	// Token: 0x060034E2 RID: 13538 RVA: 0x000FC870 File Offset: 0x000FAA70
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

	// Token: 0x060034E3 RID: 13539 RVA: 0x000FC904 File Offset: 0x000FAB04
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

	// Token: 0x060034E4 RID: 13540 RVA: 0x000FC9A4 File Offset: 0x000FABA4
	public static string RemoveStart(this string s, string value, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
	{
		if (string.IsNullOrEmpty(s) || !s.StartsWith(value, comparison))
		{
			return s;
		}
		return s.Substring(value.Length);
	}

	// Token: 0x060034E5 RID: 13541 RVA: 0x000FC9C6 File Offset: 0x000FABC6
	public static string RemoveEnd(this string s, string value, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
	{
		if (string.IsNullOrEmpty(s) || !s.EndsWith(value, comparison))
		{
			return s;
		}
		return s.Substring(0, s.Length - value.Length);
	}

	// Token: 0x060034E6 RID: 13542 RVA: 0x000FC9F0 File Offset: 0x000FABF0
	public static string RemoveBothEnds(this string s, string value, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
	{
		return s.RemoveEnd(value, comparison).RemoveStart(value, comparison);
	}

	// Token: 0x04003778 RID: 14200
	public const string kForwardSlash = "/";

	// Token: 0x04003779 RID: 14201
	public const string kBackSlash = "/";

	// Token: 0x0400377A RID: 14202
	public const string kBackTick = "`";

	// Token: 0x0400377B RID: 14203
	public const string kMinusDash = "-";

	// Token: 0x0400377C RID: 14204
	public const string kPeriod = ".";

	// Token: 0x0400377D RID: 14205
	public const string kUnderScore = "_";

	// Token: 0x0400377E RID: 14206
	public const string kColon = ":";
}
