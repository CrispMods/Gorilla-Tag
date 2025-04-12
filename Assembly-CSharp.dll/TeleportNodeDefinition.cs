using System;
using UnityEngine;

// Token: 0x020008B5 RID: 2229
[CreateAssetMenu(fileName = "New TeleportNode Definition", menuName = "Teleportation/TeleportNode Definition", order = 1)]
public class TeleportNodeDefinition : ScriptableObject
{
	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x060035EA RID: 13802 RVA: 0x00052BA1 File Offset: 0x00050DA1
	public TeleportNode Forward
	{
		get
		{
			return this.forward;
		}
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x060035EB RID: 13803 RVA: 0x00052BA9 File Offset: 0x00050DA9
	public TeleportNode Backward
	{
		get
		{
			return this.backward;
		}
	}

	// Token: 0x060035EC RID: 13804 RVA: 0x00052BB1 File Offset: 0x00050DB1
	public void SetForward(TeleportNode node)
	{
		Debug.Log("registered fwd node " + node.name);
		this.forward = node;
	}

	// Token: 0x060035ED RID: 13805 RVA: 0x00052BCF File Offset: 0x00050DCF
	public void SetBackward(TeleportNode node)
	{
		Debug.Log("registered bkwd node " + node.name);
		this.backward = node;
	}

	// Token: 0x04003822 RID: 14370
	[SerializeField]
	private TeleportNode forward;

	// Token: 0x04003823 RID: 14371
	[SerializeField]
	private TeleportNode backward;
}
