using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C8D RID: 3213
	public class UpdateBlendShapeCosmetic : MonoBehaviour
	{
		// Token: 0x06005038 RID: 20536 RVA: 0x00064656 File Offset: 0x00062856
		private void Awake()
		{
			this.targetWeight = this.blendStartWeight;
			this.currentWeight = 0f;
		}

		// Token: 0x06005039 RID: 20537 RVA: 0x0006466F File Offset: 0x0006286F
		private void Update()
		{
			this.currentWeight = Mathf.Lerp(this.currentWeight, this.targetWeight, Time.deltaTime * this.blendSpeed);
			this.skinnedMeshRenderer.SetBlendShapeWeight(this.blendShapeIndex, this.currentWeight);
		}

		// Token: 0x0600503A RID: 20538 RVA: 0x000646AB File Offset: 0x000628AB
		public void SetBlendValue(bool leftHand, float value)
		{
			this.targetWeight = Mathf.Clamp01(value) * this.maxBlendShapeWeight;
		}

		// Token: 0x0600503B RID: 20539 RVA: 0x000646C0 File Offset: 0x000628C0
		public void SetBlendValue(float value)
		{
			this.targetWeight = Mathf.Clamp01(value) * this.maxBlendShapeWeight;
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x000646D5 File Offset: 0x000628D5
		public void FullyBlend()
		{
			this.targetWeight = this.maxBlendShapeWeight;
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x000646E3 File Offset: 0x000628E3
		public void ResetBlend()
		{
			this.targetWeight = 0f;
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x000646F0 File Offset: 0x000628F0
		public float GetBlendValue()
		{
			return this.skinnedMeshRenderer.GetBlendShapeWeight(this.blendShapeIndex);
		}

		// Token: 0x04005398 RID: 21400
		[SerializeField]
		private SkinnedMeshRenderer skinnedMeshRenderer;

		// Token: 0x04005399 RID: 21401
		public float maxBlendShapeWeight = 100f;

		// Token: 0x0400539A RID: 21402
		[SerializeField]
		private int blendShapeIndex;

		// Token: 0x0400539B RID: 21403
		[SerializeField]
		private float blendSpeed = 10f;

		// Token: 0x0400539C RID: 21404
		[SerializeField]
		private float blendStartWeight;

		// Token: 0x0400539D RID: 21405
		private float targetWeight;

		// Token: 0x0400539E RID: 21406
		private float currentWeight;
	}
}
