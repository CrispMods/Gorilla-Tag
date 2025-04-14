using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class MazePlayerCollection : MonoBehaviour
{
	// Token: 0x060003D8 RID: 984 RVA: 0x00016F99 File Offset: 0x00015199
	private void Start()
	{
		NetworkSystem.Instance.OnPlayerLeft += this.OnPlayerLeftRoom;
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x00016FB1 File Offset: 0x000151B1
	private void OnDestroy()
	{
		NetworkSystem.Instance.OnPlayerLeft -= this.OnPlayerLeftRoom;
	}

	// Token: 0x060003DA RID: 986 RVA: 0x00016FCC File Offset: 0x000151CC
	public void OnTriggerEnter(Collider other)
	{
		if (!other.GetComponent<SphereCollider>())
		{
			return;
		}
		VRRig component = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		if (component == null)
		{
			return;
		}
		if (!this.containedRigs.Contains(component))
		{
			this.containedRigs.Add(component);
		}
	}

	// Token: 0x060003DB RID: 987 RVA: 0x0001701C File Offset: 0x0001521C
	public void OnTriggerExit(Collider other)
	{
		if (!other.GetComponent<SphereCollider>())
		{
			return;
		}
		VRRig component = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		if (component == null)
		{
			return;
		}
		if (this.containedRigs.Contains(component))
		{
			this.containedRigs.Remove(component);
		}
	}

	// Token: 0x060003DC RID: 988 RVA: 0x00017070 File Offset: 0x00015270
	public void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		this.containedRigs.RemoveAll((VRRig r) => ((r != null) ? r.creator : null) == null || r.creator == otherPlayer);
	}

	// Token: 0x0400045B RID: 1115
	public List<VRRig> containedRigs = new List<VRRig>();

	// Token: 0x0400045C RID: 1116
	public List<MonkeyeAI> monkeyeAis = new List<MonkeyeAI>();
}
