using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x020001BC RID: 444
[Serializable]
public struct GorillaPosRotConstraint
{
	// Token: 0x04000CC4 RID: 3268
	[Tooltip("Transform that should be moved, rotated, and scaled to match the `source` Transform in world space.")]
	public Transform follower;

	// Token: 0x04000CC5 RID: 3269
	[Tooltip("Bone that `follower` should match. Set to `None` to assign a specific Transform within the same prefab.")]
	public GTHardCodedBones.SturdyEBone sourceGorillaBone;

	// Token: 0x04000CC6 RID: 3270
	[Tooltip("Transform that `follower` should match. This is overridden at runtime if `sourceGorillaBone` is not `None`. If set in inspector, then it should be only set to a child of the the prefab this component belongs to.")]
	public Transform source;

	// Token: 0x04000CC7 RID: 3271
	public string sourceRelativePath;

	// Token: 0x04000CC8 RID: 3272
	[Tooltip("Offset to be applied to the follower's position.")]
	public Vector3 positionOffset;

	// Token: 0x04000CC9 RID: 3273
	[Tooltip("Offset to be applied to the follower's rotation.")]
	public Quaternion rotationOffset;
}
