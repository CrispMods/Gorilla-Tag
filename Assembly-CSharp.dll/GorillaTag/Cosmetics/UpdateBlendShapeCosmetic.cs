using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C5F RID: 3167
	public class UpdateBlendShapeCosmetic : MonoBehaviour
	{
		// Token: 0x06004EE4 RID: 20196 RVA: 0x00062C31 File Offset: 0x00060E31
		private void Awake()
		{
			this.targetWeight = this.blendStartWeight;
			this.currentWeight = 0f;
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x00062C4A File Offset: 0x00060E4A
		private void Update()
		{
			this.currentWeight = Mathf.Lerp(this.currentWeight, this.targetWeight, Time.deltaTime * this.blendSpeed);
			this.skinnedMeshRenderer.SetBlendShapeWeight(this.blendShapeIndex, this.currentWeight);
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x00062C86 File Offset: 0x00060E86
		public void SetBlendValue(bool leftHand, float value)
		{
			this.targetWeight = Mathf.Clamp01(value) * this.maxBlendShapeWeight;
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x00062C9B File Offset: 0x00060E9B
		public void SetBlendValue(float value)
		{
			this.targetWeight = Mathf.Clamp01(value) * this.maxBlendShapeWeight;
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x00062CB0 File Offset: 0x00060EB0
		public void FullyBlend()
		{
			this.targetWeight = this.maxBlendShapeWeight;
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x00062CBE File Offset: 0x00060EBE
		public void ResetBlend()
		{
			this.targetWeight = 0f;
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x00062CCB File Offset: 0x00060ECB
		public float GetBlendValue()
		{
			return this.skinnedMeshRenderer.GetBlendShapeWeight(this.blendShapeIndex);
		}

		// Token: 0x0400529E RID: 21150
		[SerializeField]
		private SkinnedMeshRenderer skinnedMeshRenderer;

		// Token: 0x0400529F RID: 21151
		public float maxBlendShapeWeight = 100f;

		// Token: 0x040052A0 RID: 21152
		[SerializeField]
		private int blendShapeIndex;

		// Token: 0x040052A1 RID: 21153
		[SerializeField]
		private float blendSpeed = 10f;

		// Token: 0x040052A2 RID: 21154
		[SerializeField]
		private float blendStartWeight;

		// Token: 0x040052A3 RID: 21155
		private float targetWeight;

		// Token: 0x040052A4 RID: 21156
		private float currentWeight;
	}
}
