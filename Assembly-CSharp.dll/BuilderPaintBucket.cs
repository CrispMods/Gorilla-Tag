using System;
using UnityEngine;

// Token: 0x020004AC RID: 1196
public class BuilderPaintBucket : MonoBehaviour
{
	// Token: 0x06001D15 RID: 7445 RVA: 0x000DE2D0 File Offset: 0x000DC4D0
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

	// Token: 0x06001D16 RID: 7446 RVA: 0x000DE344 File Offset: 0x000DC544
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

	// Token: 0x04002013 RID: 8211
	[SerializeField]
	private BuilderMaterialOptions bucketMaterialOptions;

	// Token: 0x04002014 RID: 8212
	[SerializeField]
	private MeshRenderer paintBucketRenderer;

	// Token: 0x04002015 RID: 8213
	[SerializeField]
	private string materialId;

	// Token: 0x04002016 RID: 8214
	private int materialType = -1;
}
