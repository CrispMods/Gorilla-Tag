using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x02000368 RID: 872
public class PlayerCollection : MonoBehaviour
{
	// Token: 0x06001439 RID: 5177 RVA: 0x00062FD4 File Offset: 0x000611D4
	private void Start()
	{
		NetworkSystem.Instance.OnPlayerLeft += this.OnPlayerLeftRoom;
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x00062FEC File Offset: 0x000611EC
	private void OnDestroy()
	{
		NetworkSystem.Instance.OnPlayerLeft -= this.OnPlayerLeftRoom;
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x00063004 File Offset: 0x00061204
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

	// Token: 0x0600143C RID: 5180 RVA: 0x00063054 File Offset: 0x00061254
	public void OnTriggerExit(Collider other)
	{
		SphereCollider component = other.GetComponent<SphereCollider>();
		if (!component)
		{
			return;
		}
		VRRig component2 = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		if (component2 == null)
		{
			return;
		}
		if (this.containedRigs.Contains(component2))
		{
			Collider[] components = base.GetComponents<Collider>();
			for (int i = 0; i < components.Length; i++)
			{
				Vector3 vector;
				float num;
				if (Physics.ComputePenetration(components[i], base.transform.position, base.transform.rotation, component, component.transform.position, component.transform.rotation, out vector, out num))
				{
					return;
				}
			}
			this.containedRigs.Remove(component2);
		}
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x000630F8 File Offset: 0x000612F8
	public void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		this.containedRigs.RemoveAll((VRRig r) => r.creator == null || r.creator == otherPlayer);
	}

	// Token: 0x04001662 RID: 5730
	[DebugReadout]
	[NonSerialized]
	public readonly List<VRRig> containedRigs = new List<VRRig>(10);
}
