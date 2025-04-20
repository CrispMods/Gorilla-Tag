using System;
using UnityEngine;

// Token: 0x0200047A RID: 1146
public class GorillaCameraSceneTrigger : MonoBehaviour
{
	// Token: 0x06001C0F RID: 7183 RVA: 0x000DBA18 File Offset: 0x000D9C18
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

	// Token: 0x04001EEE RID: 7918
	public GorillaSceneCamera sceneCamera;

	// Token: 0x04001EEF RID: 7919
	public GorillaCameraTriggerIndex currentSceneTrigger;

	// Token: 0x04001EF0 RID: 7920
	public GorillaCameraTriggerIndex mostRecentSceneTrigger;
}
