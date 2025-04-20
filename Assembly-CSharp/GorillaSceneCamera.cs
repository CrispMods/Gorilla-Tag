using System;
using UnityEngine;

// Token: 0x02000483 RID: 1155
public class GorillaSceneCamera : MonoBehaviour
{
	// Token: 0x06001C29 RID: 7209 RVA: 0x000435C1 File Offset: 0x000417C1
	public void SetSceneCamera(int sceneIndex)
	{
		base.transform.position = this.sceneTransforms[sceneIndex].scenePosition;
		base.transform.eulerAngles = this.sceneTransforms[sceneIndex].sceneRotation;
	}

	// Token: 0x04001F31 RID: 7985
	public GorillaSceneTransform[] sceneTransforms;
}
