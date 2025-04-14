using System;
using UnityEngine;

// Token: 0x0200046E RID: 1134
public class GorillaCameraSceneTrigger : MonoBehaviour
{
	// Token: 0x06001BBE RID: 7102 RVA: 0x00087D5C File Offset: 0x00085F5C
	public void ChangeScene(GorillaCameraTriggerIndex triggerLeft)
	{
		if (triggerLeft == this.currentSceneTrigger || this.currentSceneTrigger == null)
		{
			if (this.mostRecentSceneTrigger != this.currentSceneTrigger)
			{
				this.sceneCamera.SetSceneCamera(this.mostRecentSceneTrigger.sceneTriggerIndex);
				this.currentSceneTrigger = this.mostRecentSceneTrigger;
				return;
			}
			this.currentSceneTrigger = null;
		}
	}

	// Token: 0x04001EA0 RID: 7840
	public GorillaSceneCamera sceneCamera;

	// Token: 0x04001EA1 RID: 7841
	public GorillaCameraTriggerIndex currentSceneTrigger;

	// Token: 0x04001EA2 RID: 7842
	public GorillaCameraTriggerIndex mostRecentSceneTrigger;
}
