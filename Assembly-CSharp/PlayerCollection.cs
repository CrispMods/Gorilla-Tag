using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x02000373 RID: 883
public class PlayerCollection : MonoBehaviour
{
	// Token: 0x06001485 RID: 5253 RVA: 0x0003DD81 File Offset: 0x0003BF81
	private void Start()
	{
		NetworkSystem.Instance.OnPlayerLeft += this.OnPlayerLeftRoom;
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x0003DD99 File Offset: 0x0003BF99
	private void OnDestroy()
	{
		NetworkSystem.Instance.OnPlayerLeft -= this.OnPlayerLeftRoom;
	}

	// Token: 0x06001487 RID: 5255 RVA: 0x000BC350 File Offset: 0x000BA550
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

	// Token: 0x06001488 RID: 5256 RVA: 0x000BC3A0 File Offset: 0x000BA5A0
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

	// Token: 0x06001489 RID: 5257 RVA: 0x000BC444 File Offset: 0x000BA644
	public void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		this.containedRigs.RemoveAll((VRRig r) => r.creator == null || r.creator == otherPlayer);
	}

	// Token: 0x040016AA RID: 5802
	[DebugReadout]
	[NonSerialized]
	public readonly List<VRRig> containedRigs = new List<VRRig>(10);
}
