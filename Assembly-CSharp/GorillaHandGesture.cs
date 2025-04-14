using System;
using UnityEngine;

// Token: 0x02000160 RID: 352
[CreateAssetMenu(fileName = "New Hand Gesture", menuName = "Gorilla/Hand Gesture")]
public class GorillaHandGesture : ScriptableObject
{
	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060008CC RID: 2252 RVA: 0x000303D8 File Offset: 0x0002E5D8
	// (set) Token: 0x060008CD RID: 2253 RVA: 0x000303E7 File Offset: 0x0002E5E7
	public GestureHandNode hand
	{
		get
		{
			return (GestureHandNode)this.nodes[0];
		}
		set
		{
			this.nodes[0] = value;
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060008CE RID: 2254 RVA: 0x000303F2 File Offset: 0x0002E5F2
	// (set) Token: 0x060008CF RID: 2255 RVA: 0x000303FC File Offset: 0x0002E5FC
	public GestureNode palm
	{
		get
		{
			return this.nodes[1];
		}
		set
		{
			this.nodes[1] = value;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00030407 File Offset: 0x0002E607
	// (set) Token: 0x060008D1 RID: 2257 RVA: 0x00030411 File Offset: 0x0002E611
	public GestureNode wrist
	{
		get
		{
			return this.nodes[2];
		}
		set
		{
			this.nodes[2] = value;
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0003041C File Offset: 0x0002E61C
	// (set) Token: 0x060008D3 RID: 2259 RVA: 0x00030426 File Offset: 0x0002E626
	public GestureNode digits
	{
		get
		{
			return this.nodes[3];
		}
		set
		{
			this.nodes[3] = value;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00030431 File Offset: 0x0002E631
	// (set) Token: 0x060008D5 RID: 2261 RVA: 0x00030440 File Offset: 0x0002E640
	public GestureDigitNode thumb
	{
		get
		{
			return (GestureDigitNode)this.nodes[4];
		}
		set
		{
			this.nodes[4] = value;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060008D6 RID: 2262 RVA: 0x0003044B File Offset: 0x0002E64B
	// (set) Token: 0x060008D7 RID: 2263 RVA: 0x0003045A File Offset: 0x0002E65A
	public GestureDigitNode index
	{
		get
		{
			return (GestureDigitNode)this.nodes[5];
		}
		set
		{
			this.nodes[5] = value;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060008D8 RID: 2264 RVA: 0x00030465 File Offset: 0x0002E665
	// (set) Token: 0x060008D9 RID: 2265 RVA: 0x00030474 File Offset: 0x0002E674
	public GestureDigitNode middle
	{
		get
		{
			return (GestureDigitNode)this.nodes[6];
		}
		set
		{
			this.nodes[6] = value;
		}
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0003047F File Offset: 0x0002E67F
	private static GestureNode[] InitNodes()
	{
		return new GestureNode[]
		{
			new GestureHandNode(),
			new GestureNode(),
			new GestureNode(),
			new GestureNode(),
			new GestureDigitNode(),
			new GestureDigitNode(),
			new GestureDigitNode()
		};
	}

	// Token: 0x04000AC3 RID: 2755
	public bool track = true;

	// Token: 0x04000AC4 RID: 2756
	public GestureNode[] nodes = GorillaHandGesture.InitNodes();
}
