using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C5C RID: 3164
	public class UpdateBlendShapeCosmetic : MonoBehaviour
	{
		// Token: 0x06004ED8 RID: 20184 RVA: 0x00183327 File Offset: 0x00181527
		private void Awake()
		{
			this.targetWeight = this.blendStartWeight;
			this.currentWeight = 0f;
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x00183340 File Offset: 0x00181540
		private void Update()
		{
			this.currentWeight = Mathf.Lerp(this.currentWeight, this.targetWeight, Time.deltaTime * this.blendSpeed);
			this.skinnedMeshRenderer.SetBlendShapeWeight(this.blendShapeIndex, this.currentWeight);
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x0018337C File Offset: 0x0018157C
		public void SetBlendValue(bool leftHand, float value)
		{
			this.targetWeight = Mathf.Clamp01(value) * this.maxBlendShapeWeight;
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x00183391 File Offset: 0x00181591
		public void SetBlendValue(float value)
		{
			this.targetWeight = Mathf.Clamp01(value) * this.maxBlendShapeWeight;
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x001833A6 File Offset: 0x001815A6
		public void FullyBlend()
		{
			this.targetWeight = this.maxBlendShapeWeight;
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x001833B4 File Offset: 0x001815B4
		public void ResetBlend()
		{
			this.targetWeight = 0f;
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x001833C1 File Offset: 0x001815C1
		public float GetBlendValue()
		{
			return this.skinnedMeshRenderer.GetBlendShapeWeight(this.blendShapeIndex);
		}

		// Token: 0x0400528C RID: 21132
		[SerializeField]
		private SkinnedMeshRenderer skinnedMeshRenderer;

		// Token: 0x0400528D RID: 21133
		public float maxBlendShapeWeight = 100f;

		// Token: 0x0400528E RID: 21134
		[SerializeField]
		private int blendShapeIndex;

		// Token: 0x0400528F RID: 21135
		[SerializeField]
		private float blendSpeed = 10f;

		// Token: 0x04005290 RID: 21136
		[SerializeField]
		private float blendStartWeight;

		// Token: 0x04005291 RID: 21137
		private float targetWeight;

		// Token: 0x04005292 RID: 21138
		private float currentWeight;
	}
}
