using System;

// Token: 0x020001A8 RID: 424
public class ComponentMember
{
	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0003727C File Offset: 0x0003547C
	public string Name { get; }

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06000A36 RID: 2614 RVA: 0x00037284 File Offset: 0x00035484
	public string Value
	{
		get
		{
			return this.getValue();
		}
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00037291 File Offset: 0x00035491
	public bool IsStarred { get; }

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00037299 File Offset: 0x00035499
	public string Color { get; }

	// Token: 0x06000A39 RID: 2617 RVA: 0x000372A1 File Offset: 0x000354A1
	public ComponentMember(string name, Func<string> getValue, bool isStarred, string color)
	{
		this.Name = name;
		this.getValue = getValue;
		this.IsStarred = isStarred;
		this.Color = color;
	}

	// Token: 0x04000C88 RID: 3208
	private Func<string> getValue;

	// Token: 0x04000C89 RID: 3209
	public string computedPrefix;

	// Token: 0x04000C8A RID: 3210
	public string computedSuffix;
}
