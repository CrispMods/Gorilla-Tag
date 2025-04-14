using System;
using UnityEngine;

// Token: 0x0200046E RID: 1134
public class GorillaCameraSceneTrigger : MonoBehaviour
{
	// Token: 0x06001BBB RID: 7099 RVA: 0x000879D8 File Offset: 0x00085BD8
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

	// Token: 0x04001E9F RID: 7839
	public GorillaSceneCamera sceneCamera;

	// Token: 0x04001EA0 RID: 7840
	public GorillaCameraTriggerIndex currentSceneTrigger;

	// Token: 0x04001EA1 RID: 7841
	public GorillaCameraTriggerIndex mostRecentSceneTrigger;
}
