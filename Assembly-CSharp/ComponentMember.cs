using System;

// Token: 0x0200019D RID: 413
public class ComponentMember
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x060009E9 RID: 2537 RVA: 0x00037229 File Offset: 0x00035429
	public string Name { get; }

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x060009EA RID: 2538 RVA: 0x00037231 File Offset: 0x00035431
	public string Value
	{
		get
		{
			return this.getValue();
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x060009EB RID: 2539 RVA: 0x0003723E File Offset: 0x0003543E
	public bool IsStarred { get; }

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x060009EC RID: 2540 RVA: 0x00037246 File Offset: 0x00035446
	public string Color { get; }

	// Token: 0x060009ED RID: 2541 RVA: 0x0003724E File Offset: 0x0003544E
	public ComponentMember(string name, Func<string> getValue, bool isStarred, string color)
	{
		this.Name = name;
		this.getValue = getValue;
		this.IsStarred = isStarred;
		this.Color = color;
	}

	// Token: 0x04000C42 RID: 3138
	private Func<string> getValue;

	// Token: 0x04000C43 RID: 3139
	public string computedPrefix;

	// Token: 0x04000C44 RID: 3140
	public string computedSuffix;
}
