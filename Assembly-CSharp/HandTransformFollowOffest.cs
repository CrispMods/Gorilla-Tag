using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200050E RID: 1294
[Serializable]
internal class HandTransformFollowOffest
{
	// Token: 0x06001F67 RID: 8039 RVA: 0x000EF1A4 File Offset: 0x000ED3A4
	internal void UpdatePositionRotation()
	{
		if (this.followTransform == null || this.targetTransforms == null)
		{
			return;
		}
		this.position = this.followTransform.position + this.followTransform.rotation * this.positionOffset * GTPlayer.Instance.scale;
		this.rotation = this.followTransform.rotation * this.rotationOffset;
		foreach (Transform transform in this.targetTransforms)
		{
			transform.position = this.position;
			transform.rotation = this.rotation;
		}
	}

	// Token: 0x04002322 RID: 8994
	internal Transform followTransform;

	// Token: 0x04002323 RID: 8995
	[SerializeField]
	private Transform[] targetTransforms;

	// Token: 0x04002324 RID: 8996
	[SerializeField]
	internal Vector3 positionOffset;

	// Token: 0x04002325 RID: 8997
	[SerializeField]
	internal Quaternion rotationOffset;

	// Token: 0x04002326 RID: 8998
	private Vector3 position;

	// Token: 0x04002327 RID: 8999
	private Quaternion rotation;
}
