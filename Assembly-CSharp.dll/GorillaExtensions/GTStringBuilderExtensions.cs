using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Text;
using UnityEngine;

namespace GorillaExtensions
{
	// Token: 0x02000B6D RID: 2925
	public static class GTStringBuilderExtensions
	{
		// Token: 0x0600491F RID: 18719 RVA: 0x0019763C File Offset: 0x0019583C
		public unsafe static IEnumerable<ReadOnlyMemory<char>> GetSegmentsOfMem(this Utf16ValueStringBuilder sb, int maxCharsPerSegment = 16300)
		{
			int i = 0;
			List<ReadOnlyMemory<char>> list = new List<ReadOnlyMemory<char>>(64);
			ReadOnlyMemory<char> readOnlyMemory = sb.AsMemory();
			while (i < readOnlyMemory.Length)
			{
				int num = Mathf.Min(i + maxCharsPerSegment, readOnlyMemory.Length);
				if (num < readOnlyMemory.Length)
				{
					int num2 = -1;
					for (int j = num - 1; j >= i; j--)
					{
						if (*readOnlyMemory.Span[j] == 10)
						{
							num2 = j;
							break;
						}
					}
					if (num2 != -1)
					{
						num = num2;
					}
				}
				list.Add(readOnlyMemory.Slice(i, num - i));
				i = num + 1;
			}
			return list;
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x0005EBD3 File Offset: 0x0005CDD3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GTAddPath(this Utf16ValueStringBuilder stringBuilderToAddTo, GameObject gameObject)
		{
			gameObject.transform.GetPathQ(ref stringBuilderToAddTo);
		}

		// Token: 0x06004921 RID: 18721 RVA: 0x0005EBE2 File Offset: 0x0005CDE2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GTAddPath(this Utf16ValueStringBuilder stringBuilderToAddTo, Transform transform)
		{
			transform.GetPathQ(ref stringBuilderToAddTo);
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x0005EBEC File Offset: 0x0005CDEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Q(this Utf16ValueStringBuilder sb, string value)
		{
			sb.Append('"');
			sb.Append(value);
			sb.Append('"');
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x0005EC08 File Offset: 0x0005CE08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GTMany(this Utf16ValueStringBuilder sb, string a, string b)
		{
			sb.Append(a);
			sb.Append(b);
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x0005EC1A File Offset: 0x0005CE1A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GTMany(this Utf16ValueStringBuilder sb, string a, string b, string c)
		{
			sb.Append(a);
			sb.Append(b);
			sb.Append(c);
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x0005EC34 File Offset: 0x0005CE34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GTMany(this Utf16ValueStringBuilder sb, string a, string b, string c, string d)
		{
			sb.Append(a);
			sb.Append(b);
			sb.Append(c);
			sb.Append(d);
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x0005EC57 File Offset: 0x0005CE57
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GTMany(this Utf16ValueStringBuilder sb, string a, string b, string c, string d, string e)
		{
			sb.Append(a);
			sb.Append(b);
			sb.Append(c);
			sb.Append(d);
			sb.Append(e);
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x0005EC83 File Offset: 0x0005CE83
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GTMany(this Utf16ValueStringBuilder sb, string a, string b, string c, string d, string e, string f)
		{
			sb.Append(a);
			sb.Append(b);
			sb.Append(c);
			sb.Append(d);
			sb.Append(e);
			sb.Append(f);
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x0005ECB8 File Offset: 0x0005CEB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GTMany(this Utf16ValueStringBuilder sb, string a, string b, string c, string d, string e, string f, string g)
		{
			sb.Append(a);
			sb.Append(b);
			sb.Append(c);
			sb.Append(d);
			sb.Append(e);
			sb.Append(f);
			sb.Append(g);
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x001976D4 File Offset: 0x001958D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GTMany(this Utf16ValueStringBuilder sb, string a, string b, string c, string d, string e, string f, string g, string h)
		{
			sb.Append(a);
			sb.Append(b);
			sb.Append(c);
			sb.Append(d);
			sb.Append(e);
			sb.Append(f);
			sb.Append(g);
			sb.Append(h);
		}

		// Token: 0x0600492A RID: 18730 RVA: 0x00197728 File Offset: 0x00195928
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GTMany(this Utf16ValueStringBuilder sb, string a, string b, string c, string d, string e, string f, string g, string h, string i)
		{
			sb.Append(a);
			sb.Append(b);
			sb.Append(c);
			sb.Append(d);
			sb.Append(e);
			sb.Append(f);
			sb.Append(g);
			sb.Append(h);
			sb.Append(i);
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x00197784 File Offset: 0x00195984
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GTMany(this Utf16ValueStringBuilder sb, string a, string b, string c, string d, string e, string f, string g, string h, string i, string j)
		{
			sb.Append(a);
			sb.Append(b);
			sb.Append(c);
			sb.Append(d);
			sb.Append(e);
			sb.Append(f);
			sb.Append(g);
			sb.Append(h);
			sb.Append(i);
			sb.Append(j);
		}
	}
}
