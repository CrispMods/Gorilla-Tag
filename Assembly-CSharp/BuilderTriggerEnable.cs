using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000505 RID: 1285
public class BuilderTriggerEnable : MonoBehaviour
{
	// Token: 0x06001F34 RID: 7988 RVA: 0x000EE83C File Offset: 0x000ECA3C
	private void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody == null)
		{
			return;
		}
		VRRig component = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		if (component == null || component.OwningNetPlayer == null)
		{
			return;
		}
		if (!component.OwningNetPlayer.IsLocal)
		{
			return;
		}
		if (this.activateOnEnter != null)
		{
			for (int i = 0; i < this.activateOnEnter.Count; i++)
			{
				if (this.activateOnEnter[i] != null)
				{
					this.activateOnEnter[i].SetActive(true);
				}
			}
		}
		if (this.deactivateOnEnter != null)
		{
			for (int j = 0; j < this.deactivateOnEnter.Count; j++)
			{
				if (this.deactivateOnEnter[j] != null)
				{
					this.deactivateOnEnter[j].SetActive(false);
				}
			}
		}
	}

	// Token: 0x06001F35 RID: 7989 RVA: 0x000EE914 File Offset: 0x000ECB14
	private void OnTriggerExit(Collider other)
	{
		if (other.attachedRigidbody == null)
		{
			return;
		}
		VRRig component = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		if (component == null || component.OwningNetPlayer == null)
		{
			return;
		}
		if (!component.OwningNetPlayer.IsLocal)
		{
			return;
		}
		if (this.activateOnExit != null)
		{
			for (int i = 0; i < this.activateOnExit.Count; i++)
			{
				if (this.activateOnExit[i] != null)
				{
					this.activateOnExit[i].SetActive(true);
				}
			}
		}
		if (this.deactivateOnExit != null)
		{
			for (int j = 0; j < this.deactivateOnExit.Count; j++)
			{
				if (this.deactivateOnExit[j] != null)
				{
					this.deactivateOnExit[j].SetActive(false);
				}
			}
		}
	}

	// Token: 0x040022E0 RID: 8928
	public List<GameObject> activateOnEnter;

	// Token: 0x040022E1 RID: 8929
	public List<GameObject> deactivateOnEnter;

	// Token: 0x040022E2 RID: 8930
	public List<GameObject> activateOnExit;

	// Token: 0x040022E3 RID: 8931
	public List<GameObject> deactivateOnExit;
}
