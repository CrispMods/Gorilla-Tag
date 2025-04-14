using System;
using UnityEngine;

// Token: 0x020008B2 RID: 2226
[CreateAssetMenu(fileName = "New TeleportNode Definition", menuName = "Teleportation/TeleportNode Definition", order = 1)]
public class TeleportNodeDefinition : ScriptableObject
{
	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x060035DE RID: 13790 RVA: 0x000FF3A8 File Offset: 0x000FD5A8
	public TeleportNode Forward
	{
		get
		{
			return this.forward;
		}
	}

	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x060035DF RID: 13791 RVA: 0x000FF3B0 File Offset: 0x000FD5B0
	public TeleportNode Backward
	{
		get
		{
			return this.backward;
		}
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x000FF3B8 File Offset: 0x000FD5B8
	public void SetForward(TeleportNode node)
	{
		Debug.Log("registered fwd node " + node.name);
		this.forward = node;
	}

	// Token: 0x060035E1 RID: 13793 RVA: 0x000FF3D6 File Offset: 0x000FD5D6
	public void SetBackward(TeleportNode node)
	{
		Debug.Log("registered bkwd node " + node.name);
		this.backward = node;
	}

	// Token: 0x04003810 RID: 14352
	[SerializeField]
	private TeleportNode forward;

	// Token: 0x04003811 RID: 14353
	[SerializeField]
	private TeleportNode backward;
}
