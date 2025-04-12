using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000501 RID: 1281
[Serializable]
internal class HandTransformFollowOffest
{
	// Token: 0x06001F11 RID: 7953 RVA: 0x000EC468 File Offset: 0x000EA668
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

	// Token: 0x040022D0 RID: 8912
	internal Transform followTransform;

	// Token: 0x040022D1 RID: 8913
	[SerializeField]
	private Transform[] targetTransforms;

	// Token: 0x040022D2 RID: 8914
	[SerializeField]
	internal Vector3 positionOffset;

	// Token: 0x040022D3 RID: 8915
	[SerializeField]
	internal Quaternion rotationOffset;

	// Token: 0x040022D4 RID: 8916
	private Vector3 position;

	// Token: 0x040022D5 RID: 8917
	private Quaternion rotation;
}
