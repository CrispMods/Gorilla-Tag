using System;
using UnityEngine;

// Token: 0x0200046F RID: 1135
public class GorillaCameraTriggerIndex : MonoBehaviour
{
	// Token: 0x06001BBD RID: 7101 RVA: 0x00087A3E File Offset: 0x00085C3E
	private void Start()
	{
		this.parentTrigger = base.GetComponentInParent<GorillaCameraSceneTrigger>();
	}

	// Token: 0x06001BBE RID: 7102 RVA: 0x00087A4C File Offset: 0x00085C4C
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("SceneChanger"))
		{
			this.parentTrigger.mostRecentSceneTrigger = this;
			this.parentTrigger.ChangeScene(this);
		}
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x00087A78 File Offset: 0x00085C78
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("SceneChanger"))
		{
			this.parentTrigger.ChangeScene(this);
		}
	}

	// Token: 0x04001EA2 RID: 7842
	public int sceneTriggerIndex;

	// Token: 0x04001EA3 RID: 7843
	public GorillaCameraSceneTrigger parentTrigger;
}
