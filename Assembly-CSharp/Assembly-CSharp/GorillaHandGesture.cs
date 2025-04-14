using System;
using UnityEngine;

// Token: 0x02000160 RID: 352
[CreateAssetMenu(fileName = "New Hand Gesture", menuName = "Gorilla/Hand Gesture")]
public class GorillaHandGesture : ScriptableObject
{
	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060008CE RID: 2254 RVA: 0x000306FC File Offset: 0x0002E8FC
	// (set) Token: 0x060008CF RID: 2255 RVA: 0x0003070B File Offset: 0x0002E90B
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
	// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00030716 File Offset: 0x0002E916
	// (set) Token: 0x060008D1 RID: 2257 RVA: 0x00030720 File Offset: 0x0002E920
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
	// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0003072B File Offset: 0x0002E92B
	// (set) Token: 0x060008D3 RID: 2259 RVA: 0x00030735 File Offset: 0x0002E935
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
	// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00030740 File Offset: 0x0002E940
	// (set) Token: 0x060008D5 RID: 2261 RVA: 0x0003074A File Offset: 0x0002E94A
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
	// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00030755 File Offset: 0x0002E955
	// (set) Token: 0x060008D7 RID: 2263 RVA: 0x00030764 File Offset: 0x0002E964
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
	// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0003076F File Offset: 0x0002E96F
	// (set) Token: 0x060008D9 RID: 2265 RVA: 0x0003077E File Offset: 0x0002E97E
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
	// (get) Token: 0x060008DA RID: 2266 RVA: 0x00030789 File Offset: 0x0002E989
	// (set) Token: 0x060008DB RID: 2267 RVA: 0x00030798 File Offset: 0x0002E998
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

	// Token: 0x060008DC RID: 2268 RVA: 0x000307A3 File Offset: 0x0002E9A3
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
