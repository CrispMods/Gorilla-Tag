using System;
using UnityEngine;

// Token: 0x02000160 RID: 352
[CreateAssetMenu(fileName = "New Hand Gesture", menuName = "Gorilla/Hand Gesture")]
public class GorillaHandGesture : ScriptableObject
{
	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060008CE RID: 2254 RVA: 0x000353C9 File Offset: 0x000335C9
	// (set) Token: 0x060008CF RID: 2255 RVA: 0x000353D8 File Offset: 0x000335D8
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
	// (get) Token: 0x060008D0 RID: 2256 RVA: 0x000353E3 File Offset: 0x000335E3
	// (set) Token: 0x060008D1 RID: 2257 RVA: 0x000353ED File Offset: 0x000335ED
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
	// (get) Token: 0x060008D2 RID: 2258 RVA: 0x000353F8 File Offset: 0x000335F8
	// (set) Token: 0x060008D3 RID: 2259 RVA: 0x00035402 File Offset: 0x00033602
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
	// (get) Token: 0x060008D4 RID: 2260 RVA: 0x0003540D File Offset: 0x0003360D
	// (set) Token: 0x060008D5 RID: 2261 RVA: 0x00035417 File Offset: 0x00033617
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
	// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00035422 File Offset: 0x00033622
	// (set) Token: 0x060008D7 RID: 2263 RVA: 0x00035431 File Offset: 0x00033631
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
	// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0003543C File Offset: 0x0003363C
	// (set) Token: 0x060008D9 RID: 2265 RVA: 0x0003544B File Offset: 0x0003364B
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
	// (get) Token: 0x060008DA RID: 2266 RVA: 0x00035456 File Offset: 0x00033656
	// (set) Token: 0x060008DB RID: 2267 RVA: 0x00035465 File Offset: 0x00033665
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

	// Token: 0x060008DC RID: 2268 RVA: 0x00035470 File Offset: 0x00033670
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

	// Token: 0x04000AC4 RID: 2756
	public bool track = true;

	// Token: 0x04000AC5 RID: 2757
	public GestureNode[] nodes = GorillaHandGesture.InitNodes();
}
