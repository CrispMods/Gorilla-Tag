using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class MazePlayerCollection : MonoBehaviour
{
	// Token: 0x060003DA RID: 986 RVA: 0x000172BD File Offset: 0x000154BD
	private void Start()
	{
		NetworkSystem.Instance.OnPlayerLeft += this.OnPlayerLeftRoom;
	}

	// Token: 0x060003DB RID: 987 RVA: 0x000172D5 File Offset: 0x000154D5
	private void OnDestroy()
	{
		NetworkSystem.Instance.OnPlayerLeft -= this.OnPlayerLeftRoom;
	}

	// Token: 0x060003DC RID: 988 RVA: 0x000172F0 File Offset: 0x000154F0
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

	// Token: 0x060003DD RID: 989 RVA: 0x00017340 File Offset: 0x00015540
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

	// Token: 0x060003DE RID: 990 RVA: 0x00017394 File Offset: 0x00015594
	public void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		this.containedRigs.RemoveAll((VRRig r) => ((r != null) ? r.creator : null) == null || r.creator == otherPlayer);
	}

	// Token: 0x0400045C RID: 1116
	public List<VRRig> containedRigs = new List<VRRig>();

	// Token: 0x0400045D RID: 1117
	public List<MonkeyeAI> monkeyeAis = new List<MonkeyeAI>();
}
