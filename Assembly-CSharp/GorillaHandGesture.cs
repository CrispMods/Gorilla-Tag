using System;
using UnityEngine;

// Token: 0x0200016B RID: 363
[CreateAssetMenu(fileName = "New Hand Gesture", menuName = "Gorilla/Hand Gesture")]
public class GorillaHandGesture : ScriptableObject
{
	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06000919 RID: 2329 RVA: 0x00036694 File Offset: 0x00034894
	// (set) Token: 0x0600091A RID: 2330 RVA: 0x000366A3 File Offset: 0x000348A3
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

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x0600091B RID: 2331 RVA: 0x000366AE File Offset: 0x000348AE
	// (set) Token: 0x0600091C RID: 2332 RVA: 0x000366B8 File Offset: 0x000348B8
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

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x0600091D RID: 2333 RVA: 0x000366C3 File Offset: 0x000348C3
	// (set) Token: 0x0600091E RID: 2334 RVA: 0x000366CD File Offset: 0x000348CD
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

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x0600091F RID: 2335 RVA: 0x000366D8 File Offset: 0x000348D8
	// (set) Token: 0x06000920 RID: 2336 RVA: 0x000366E2 File Offset: 0x000348E2
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

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x06000921 RID: 2337 RVA: 0x000366ED File Offset: 0x000348ED
	// (set) Token: 0x06000922 RID: 2338 RVA: 0x000366FC File Offset: 0x000348FC
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

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06000923 RID: 2339 RVA: 0x00036707 File Offset: 0x00034907
	// (set) Token: 0x06000924 RID: 2340 RVA: 0x00036716 File Offset: 0x00034916
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

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000925 RID: 2341 RVA: 0x00036721 File Offset: 0x00034921
	// (set) Token: 0x06000926 RID: 2342 RVA: 0x00036730 File Offset: 0x00034930
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

	// Token: 0x06000927 RID: 2343 RVA: 0x0003673B File Offset: 0x0003493B
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

	// Token: 0x04000B0A RID: 2826
	public bool track = true;

	// Token: 0x04000B0B RID: 2827
	public GestureNode[] nodes = GorillaHandGesture.InitNodes();
}
