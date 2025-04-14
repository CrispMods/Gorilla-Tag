using System;
using UnityEngine;

// Token: 0x02000477 RID: 1143
public class GorillaSceneCamera : MonoBehaviour
{
	// Token: 0x06001BD8 RID: 7128 RVA: 0x00088018 File Offset: 0x00086218
	public void SetSceneCamera(int sceneIndex)
	{
		base.transform.position = this.sceneTransforms[sceneIndex].scenePosition;
		base.transform.eulerAngles = this.sceneTransforms[sceneIndex].sceneRotation;
	}

	// Token: 0x04001EE3 RID: 7907
	public GorillaSceneTransform[] sceneTransforms;
}
