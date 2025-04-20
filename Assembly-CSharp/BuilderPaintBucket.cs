using System;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
public class BuilderPaintBucket : MonoBehaviour
{
	// Token: 0x06001D66 RID: 7526 RVA: 0x000E0F88 File Offset: 0x000DF188
	private void Awake()
	{
		if (string.IsNullOrEmpty(this.materialId))
		{
			return;
		}
		this.materialType = this.materialId.GetHashCode();
		if (this.bucketMaterialOptions != null && this.paintBucketRenderer != null)
		{
			Material material;
			int num;
			this.bucketMaterialOptions.GetMaterialFromType(this.materialType, out material, out num);
			if (material != null)
			{
				this.paintBucketRenderer.material = material;
			}
		}
	}

	// Token: 0x06001D67 RID: 7527 RVA: 0x000E0FFC File Offset: 0x000DF1FC
	private void OnTriggerEnter(Collider other)
	{
		if (this.materialType == -1)
		{
			return;
		}
		Rigidbody attachedRigidbody = other.attachedRigidbody;
		if (attachedRigidbody != null)
		{
			BuilderPaintBrush component = attachedRigidbody.GetComponent<BuilderPaintBrush>();
			if (component != null)
			{
				component.SetBrushMaterial(this.materialType);
			}
		}
	}

	// Token: 0x04002061 RID: 8289
	[SerializeField]
	private BuilderMaterialOptions bucketMaterialOptions;

	// Token: 0x04002062 RID: 8290
	[SerializeField]
	private MeshRenderer paintBucketRenderer;

	// Token: 0x04002063 RID: 8291
	[SerializeField]
	private string materialId;

	// Token: 0x04002064 RID: 8292
	private int materialType = -1;
}
