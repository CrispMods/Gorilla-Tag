using System;
using UnityEngine;

// Token: 0x0200002B RID: 43
[Serializable]
internal class SoundIdRemapping
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x060000A0 RID: 160 RVA: 0x0002FC3D File Offset: 0x0002DE3D
	public int SoundIn
	{
		get
		{
			return this.soundIn;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x060000A1 RID: 161 RVA: 0x0002FC45 File Offset: 0x0002DE45
	public int SoundOut
	{
		get
		{
			return this.soundOut;
		}
	}

	// Token: 0x040000C3 RID: 195
	[GorillaSoundLookup]
	[SerializeField]
	private int soundIn = 1;

	// Token: 0x040000C4 RID: 196
	[GorillaSoundLookup]
	[SerializeField]
	private int soundOut = 2;
}
