using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x020001FA RID: 506
public class StringFormatter
{
	// Token: 0x06000BD7 RID: 3031 RVA: 0x0003EDEB File Offset: 0x0003CFEB
	public StringFormatter(string[] spans, int[] indices)
	{
		this.spans = spans;
		this.indices = indices;
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x0003EE04 File Offset: 0x0003D004
	public string Format(string term1)
	{
		StringFormatter.builder.Clear();
		StringFormatter.builder.Append(this.spans[0]);
		for (int i = 1; i < this.spans.Length; i++)
		{
			StringFormatter.builder.Append(term1);
			StringFormatter.builder.Append(this.spans[i]);
		}
		return StringFormatter.builder.ToString();
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x0003EE6C File Offset: 0x0003D06C
	public string Format(Func<string> term1)
	{
		StringFormatter.builder.Clear();
		StringFormatter.builder.Append(this.spans[0]);
		for (int i = 1; i < this.spans.Length; i++)
		{
			StringFormatter.builder.Append(term1());
			StringFormatter.builder.Append(this.spans[i]);
		}
		return StringFormatter.builder.ToString();
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x0003EED8 File Offset: 0x0003D0D8
	public string Format(string term1, string term2)
	{
		StringFormatter.builder.Clear();
		StringFormatter.builder.Append(this.spans[0]);
		for (int i = 1; i < this.spans.Length; i++)
		{
			StringFormatter.builder.Append((this.indices[i - 1] == 0) ? term1 : term2);
			StringFormatter.builder.Append(this.spans[i]);
		}
		return StringFormatter.builder.ToString();
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x0003EF50 File Offset: 0x0003D150
	public string Format(string term1, string term2, string term3)
	{
		StringFormatter.builder.Clear();
		StringFormatter.builder.Append(this.spans[0]);
		for (int i = 1; i < this.spans.Length; i++)
		{
			int num = this.indices[i - 1];
			if (num != 0)
			{
				if (num != 1)
				{
					StringFormatter.builder.Append(term3);
				}
				else
				{
					StringFormatter.builder.Append(term2);
				}
			}
			else
			{
				StringFormatter.builder.Append(term1);
			}
			StringFormatter.builder.Append(this.spans[i]);
		}
		return StringFormatter.builder.ToString();
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x0003EFE8 File Offset: 0x0003D1E8
	public string Format(Func<string> term1, Func<string> term2)
	{
		StringFormatter.builder.Clear();
		StringFormatter.builder.Append(this.spans[0]);
		for (int i = 1; i < this.spans.Length; i++)
		{
			if (this.indices[i - 1] == 0)
			{
				StringFormatter.builder.Append(term1());
			}
			else
			{
				StringFormatter.builder.Append(term2());
			}
			StringFormatter.builder.Append(this.spans[i]);
		}
		return StringFormatter.builder.ToString();
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x0003F074 File Offset: 0x0003D274
	public string Format(Func<string> term1, Func<string> term2, Func<string> term3)
	{
		StringFormatter.builder.Clear();
		StringFormatter.builder.Append(this.spans[0]);
		for (int i = 1; i < this.spans.Length; i++)
		{
			int num = this.indices[i - 1];
			if (num != 0)
			{
				if (num != 1)
				{
					StringFormatter.builder.Append(term3());
				}
				else
				{
					StringFormatter.builder.Append(term2());
				}
			}
			else
			{
				StringFormatter.builder.Append(term1());
			}
			StringFormatter.builder.Append(this.spans[i]);
		}
		return StringFormatter.builder.ToString();
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x0003F11C File Offset: 0x0003D31C
	public string Format(Func<string> term1, Func<string> term2, Func<string> term3, Func<string> term4)
	{
		StringFormatter.builder.Clear();
		StringFormatter.builder.Append(this.spans[0]);
		for (int i = 1; i < this.spans.Length; i++)
		{
			switch (this.indices[i - 1])
			{
			case 0:
				StringFormatter.builder.Append(term1());
				break;
			case 1:
				StringFormatter.builder.Append(term2());
				break;
			case 2:
				StringFormatter.builder.Append(term3());
				break;
			default:
				StringFormatter.builder.Append(term4());
				break;
			}
			StringFormatter.builder.Append(this.spans[i]);
		}
		return StringFormatter.builder.ToString();
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x0003F1E8 File Offset: 0x0003D3E8
	public string Format(params string[] terms)
	{
		StringFormatter.builder.Clear();
		StringFormatter.builder.Append(this.spans[0]);
		for (int i = 1; i < this.spans.Length; i++)
		{
			StringFormatter.builder.Append(terms[this.indices[i - 1]]);
			StringFormatter.builder.Append(this.spans[i]);
		}
		return StringFormatter.builder.ToString();
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x0003F25C File Offset: 0x0003D45C
	public string Format(params Func<string>[] terms)
	{
		StringFormatter.builder.Clear();
		StringFormatter.builder.Append(this.spans[0]);
		for (int i = 1; i < this.spans.Length; i++)
		{
			StringFormatter.builder.Append(terms[this.indices[i - 1]]());
			StringFormatter.builder.Append(this.spans[i]);
		}
		return StringFormatter.builder.ToString();
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x0003F2D4 File Offset: 0x0003D4D4
	public static StringFormatter Parse(string input)
	{
		int num = 0;
		List<string> list = new List<string>();
		List<int> list2 = new List<int>();
		for (;;)
		{
			int num2 = input.IndexOf('%', num);
			if (num2 == -1)
			{
				break;
			}
			list.Add(input.Substring(num, num2 - num));
			list2.Add((int)(input[num2 + 1] - '0'));
			num = num2 + 2;
		}
		list.Add(input.Substring(num));
		return new StringFormatter(list.ToArray(), list2.ToArray());
	}

	// Token: 0x04000E3B RID: 3643
	private static StringBuilder builder = new StringBuilder();

	// Token: 0x04000E3C RID: 3644
	private string[] spans;

	// Token: 0x04000E3D RID: 3645
	private int[] indices;
}
