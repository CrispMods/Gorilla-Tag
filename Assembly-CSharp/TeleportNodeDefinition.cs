using System;
using UnityEngine;

// Token: 0x020008CE RID: 2254
[CreateAssetMenu(fileName = "New TeleportNode Definition", menuName = "Teleportation/TeleportNode Definition", order = 1)]
public class TeleportNodeDefinition : ScriptableObject
{
	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x060036A6 RID: 13990 RVA: 0x000540BE File Offset: 0x000522BE
	public TeleportNode Forward
	{
		get
		{
			return this.forward;
		}
	}

	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x060036A7 RID: 13991 RVA: 0x000540C6 File Offset: 0x000522C6
	public TeleportNode Backward
	{
		get
		{
			return this.backward;
		}
	}

	// Token: 0x060036A8 RID: 13992 RVA: 0x000540CE File Offset: 0x000522CE
	public void SetForward(TeleportNode node)
	{
		Debug.Log("registered fwd node " + node.name);
		this.forward = node;
	}

	// Token: 0x060036A9 RID: 13993 RVA: 0x000540EC File Offset: 0x000522EC
	public void SetBackward(TeleportNode node)
	{
		Debug.Log("registered bkwd node " + node.name);
		this.backward = node;
	}

	// Token: 0x040038D1 RID: 14545
	[SerializeField]
	private TeleportNode forward;

	// Token: 0x040038D2 RID: 14546
	[SerializeField]
	private TeleportNode backward;
}
