using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F8 RID: 1272
public class BuilderTriggerEnable : MonoBehaviour
{
	// Token: 0x06001EDE RID: 7902 RVA: 0x000EBB00 File Offset: 0x000E9D00
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

	// Token: 0x06001EDF RID: 7903 RVA: 0x000EBBD8 File Offset: 0x000E9DD8
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

	// Token: 0x0400228E RID: 8846
	public List<GameObject> activateOnEnter;

	// Token: 0x0400228F RID: 8847
	public List<GameObject> deactivateOnEnter;

	// Token: 0x04002290 RID: 8848
	public List<GameObject> activateOnExit;

	// Token: 0x04002291 RID: 8849
	public List<GameObject> deactivateOnExit;
}
