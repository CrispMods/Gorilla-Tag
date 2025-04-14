using System;
using UnityEngine;

// Token: 0x020004AC RID: 1196
public class BuilderPaintBucket : MonoBehaviour
{
	// Token: 0x06001D12 RID: 7442 RVA: 0x0008DCE0 File Offset: 0x0008BEE0
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

	// Token: 0x06001D13 RID: 7443 RVA: 0x0008DD54 File Offset: 0x0008BF54
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

	// Token: 0x04002012 RID: 8210
	[SerializeField]
	private BuilderMaterialOptions bucketMaterialOptions;

	// Token: 0x04002013 RID: 8211
	[SerializeField]
	private MeshRenderer paintBucketRenderer;

	// Token: 0x04002014 RID: 8212
	[SerializeField]
	private string materialId;

	// Token: 0x04002015 RID: 8213
	private int materialType = -1;
}
