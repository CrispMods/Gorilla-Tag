using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x020001C7 RID: 455
[Serializable]
public struct GorillaPosRotConstraint
{
	// Token: 0x04000D09 RID: 3337
	[Tooltip("Transform that should be moved, rotated, and scaled to match the `source` Transform in world space.")]
	public Transform follower;

	// Token: 0x04000D0A RID: 3338
	[Tooltip("Bone that `follower` should match. Set to `None` to assign a specific Transform within the same prefab.")]
	public GTHardCodedBones.SturdyEBone sourceGorillaBone;

	// Token: 0x04000D0B RID: 3339
	[Tooltip("Transform that `follower` should match. This is overridden at runtime if `sourceGorillaBone` is not `None`. If set in inspector, then it should be only set to a child of the the prefab this component belongs to.")]
	public Transform source;

	// Token: 0x04000D0C RID: 3340
	public string sourceRelativePath;

	// Token: 0x04000D0D RID: 3341
	[Tooltip("Offset to be applied to the follower's position.")]
	public Vector3 positionOffset;

	// Token: 0x04000D0E RID: 3342
	[Tooltip("Offset to be applied to the follower's rotation.")]
	public Quaternion rotationOffset;
}
