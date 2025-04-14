using System;
using UnityEngine;

// Token: 0x0200046F RID: 1135
public class GorillaCameraTriggerIndex : MonoBehaviour
{
	// Token: 0x06001BC0 RID: 7104 RVA: 0x00087DC2 File Offset: 0x00085FC2
	private void Start()
	{
		this.parentTrigger = base.GetComponentInParent<GorillaCameraSceneTrigger>();
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x00087DD0 File Offset: 0x00085FD0
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("SceneChanger"))
		{
			this.parentTrigger.mostRecentSceneTrigger = this;
			this.parentTrigger.ChangeScene(this);
		}
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x00087DFC File Offset: 0x00085FFC
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("SceneChanger"))
		{
			this.parentTrigger.ChangeScene(this);
		}
	}

	// Token: 0x04001EA3 RID: 7843
	public int sceneTriggerIndex;

	// Token: 0x04001EA4 RID: 7844
	public GorillaCameraSceneTrigger parentTrigger;
}
