using System;
using UnityEngine;

// Token: 0x02000477 RID: 1143
public class GorillaSceneCamera : MonoBehaviour
{
	// Token: 0x06001BD5 RID: 7125 RVA: 0x00087C94 File Offset: 0x00085E94
	public void SetSceneCamera(int sceneIndex)
	{
		base.transform.position = this.sceneTransforms[sceneIndex].scenePosition;
		base.transform.eulerAngles = this.sceneTransforms[sceneIndex].sceneRotation;
	}

	// Token: 0x04001EE2 RID: 7906
	public GorillaSceneTransform[] sceneTransforms;
}
