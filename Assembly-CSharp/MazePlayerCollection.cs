using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class MazePlayerCollection : MonoBehaviour
{
	// Token: 0x06000414 RID: 1044 RVA: 0x000331FB File Offset: 0x000313FB
	private void Start()
	{
		NetworkSystem.Instance.OnPlayerLeft += this.OnPlayerLeftRoom;
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00033213 File Offset: 0x00031413
	private void OnDestroy()
	{
		NetworkSystem.Instance.OnPlayerLeft -= this.OnPlayerLeftRoom;
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0007AE34 File Offset: 0x00079034
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

	// Token: 0x06000417 RID: 1047 RVA: 0x0007AE84 File Offset: 0x00079084
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

	// Token: 0x06000418 RID: 1048 RVA: 0x0007AED8 File Offset: 0x000790D8
	public void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		this.containedRigs.RemoveAll((VRRig r) => ((r != null) ? r.creator : null) == null || r.creator == otherPlayer);
	}

	// Token: 0x0400049B RID: 1179
	public List<VRRig> containedRigs = new List<VRRig>();

	// Token: 0x0400049C RID: 1180
	public List<MonkeyeAI> monkeyeAis = new List<MonkeyeAI>();
}
