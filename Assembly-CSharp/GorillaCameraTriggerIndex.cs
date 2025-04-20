using System;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public class GorillaCameraTriggerIndex : MonoBehaviour
{
	// Token: 0x06001C11 RID: 7185 RVA: 0x00043457 File Offset: 0x00041657
	private void Start()
	{
		this.parentTrigger = base.GetComponentInParent<GorillaCameraSceneTrigger>();
	}

	// Token: 0x06001C12 RID: 7186 RVA: 0x00043465 File Offset: 0x00041665
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("SceneChanger"))
		{
			this.parentTrigger.mostRecentSceneTrigger = this;
			this.parentTrigger.ChangeScene(this);
		}
	}

	// Token: 0x06001C13 RID: 7187 RVA: 0x00043491 File Offset: 0x00041691
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("SceneChanger"))
		{
			this.parentTrigger.ChangeScene(this);
		}
	}

	// Token: 0x04001EF1 RID: 7921
	public int sceneTriggerIndex;

	// Token: 0x04001EF2 RID: 7922
	public GorillaCameraSceneTrigger parentTrigger;
}
