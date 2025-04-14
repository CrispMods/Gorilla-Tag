using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x0200063A RID: 1594
public class SpoonClacker : MonoBehaviour
{
	// Token: 0x0600279E RID: 10142 RVA: 0x000C1EFE File Offset: 0x000C00FE
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x0600279F RID: 10143 RVA: 0x000C1F08 File Offset: 0x000C0108
	private void Setup()
	{
		JointLimits limits = this.hingeJoint.limits;
		this.hingeMin = limits.min;
		this.hingeMax = limits.max;
	}

	// Token: 0x060027A0 RID: 10144 RVA: 0x000C1F3C File Offset: 0x000C013C
	private void Update()
	{
		if (!this.transferObject)
		{
			return;
		}
		TransferrableObject.PositionState currentState = this.transferObject.currentState;
		if (currentState != TransferrableObject.PositionState.InLeftHand && currentState != TransferrableObject.PositionState.InRightHand)
		{
			return;
		}
		float num = MathUtils.Linear(this.hingeJoint.angle, this.hingeMin, this.hingeMax, 0f, 1f);
		float value = (this.invertOut ? (1f - num) : num) * 100f;
		this.skinnedMesh.SetBlendShapeWeight(this.targetBlendShape, value);
		if (!this._lockMin && num <= this.minThreshold)
		{
			this.OnHitMin.Invoke();
			this._lockMin = true;
		}
		else if (!this._lockMax && num >= 1f - this.maxThreshold)
		{
			this.OnHitMax.Invoke();
			this._lockMax = true;
			if (this._sincelastHit.HasElapsed(this.multiHitCutoff, true))
			{
				this.soundsSingle.Play();
			}
			else
			{
				this.soundsMulti.Play();
			}
		}
		if (this._lockMin && num > this.minThreshold * this.hysterisisFactor)
		{
			this._lockMin = false;
		}
		if (this._lockMax && num < 1f - this.maxThreshold * this.hysterisisFactor)
		{
			this._lockMax = false;
		}
	}

	// Token: 0x04002B5A RID: 11098
	public TransferrableObject transferObject;

	// Token: 0x04002B5B RID: 11099
	public SkinnedMeshRenderer skinnedMesh;

	// Token: 0x04002B5C RID: 11100
	public HingeJoint hingeJoint;

	// Token: 0x04002B5D RID: 11101
	public int targetBlendShape;

	// Token: 0x04002B5E RID: 11102
	public float hingeMin;

	// Token: 0x04002B5F RID: 11103
	public float hingeMax;

	// Token: 0x04002B60 RID: 11104
	public bool invertOut;

	// Token: 0x04002B61 RID: 11105
	public float minThreshold = 0.01f;

	// Token: 0x04002B62 RID: 11106
	public float maxThreshold = 0.01f;

	// Token: 0x04002B63 RID: 11107
	public float hysterisisFactor = 4f;

	// Token: 0x04002B64 RID: 11108
	public UnityEvent OnHitMin;

	// Token: 0x04002B65 RID: 11109
	public UnityEvent OnHitMax;

	// Token: 0x04002B66 RID: 11110
	private bool _lockMin;

	// Token: 0x04002B67 RID: 11111
	private bool _lockMax;

	// Token: 0x04002B68 RID: 11112
	public SoundBankPlayer soundsSingle;

	// Token: 0x04002B69 RID: 11113
	public SoundBankPlayer soundsMulti;

	// Token: 0x04002B6A RID: 11114
	private TimeSince _sincelastHit;

	// Token: 0x04002B6B RID: 11115
	[FormerlySerializedAs("multiHitInterval")]
	public float multiHitCutoff = 0.1f;
}
