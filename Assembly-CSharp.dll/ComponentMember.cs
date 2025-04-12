using System;

// Token: 0x0200019D RID: 413
public class ComponentMember
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x060009EB RID: 2539 RVA: 0x00035FBC File Offset: 0x000341BC
	public string Name { get; }

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x060009EC RID: 2540 RVA: 0x00035FC4 File Offset: 0x000341C4
	public string Value
	{
		get
		{
			return this.getValue();
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x060009ED RID: 2541 RVA: 0x00035FD1 File Offset: 0x000341D1
	public bool IsStarred { get; }

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x060009EE RID: 2542 RVA: 0x00035FD9 File Offset: 0x000341D9
	public string Color { get; }

	// Token: 0x060009EF RID: 2543 RVA: 0x00035FE1 File Offset: 0x000341E1
	public ComponentMember(string name, Func<string> getValue, bool isStarred, string color)
	{
		this.Name = name;
		this.getValue = getValue;
		this.IsStarred = isStarred;
		this.Color = color;
	}

	// Token: 0x04000C43 RID: 3139
	private Func<string> getValue;

	// Token: 0x04000C44 RID: 3140
	public string computedPrefix;

	// Token: 0x04000C45 RID: 3141
	public string computedSuffix;
}
