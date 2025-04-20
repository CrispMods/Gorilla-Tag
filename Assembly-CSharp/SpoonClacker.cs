using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x02000618 RID: 1560
public class SpoonClacker : MonoBehaviour
{
	// Token: 0x060026C1 RID: 9921 RVA: 0x0004A72F File Offset: 0x0004892F
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x060026C2 RID: 9922 RVA: 0x00109234 File Offset: 0x00107434
	private void Setup()
	{
		JointLimits limits = this.hingeJoint.limits;
		this.hingeMin = limits.min;
		this.hingeMax = limits.max;
	}

	// Token: 0x060026C3 RID: 9923 RVA: 0x00109268 File Offset: 0x00107468
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

	// Token: 0x04002ABA RID: 10938
	public TransferrableObject transferObject;

	// Token: 0x04002ABB RID: 10939
	public SkinnedMeshRenderer skinnedMesh;

	// Token: 0x04002ABC RID: 10940
	public HingeJoint hingeJoint;

	// Token: 0x04002ABD RID: 10941
	public int targetBlendShape;

	// Token: 0x04002ABE RID: 10942
	public float hingeMin;

	// Token: 0x04002ABF RID: 10943
	public float hingeMax;

	// Token: 0x04002AC0 RID: 10944
	public bool invertOut;

	// Token: 0x04002AC1 RID: 10945
	public float minThreshold = 0.01f;

	// Token: 0x04002AC2 RID: 10946
	public float maxThreshold = 0.01f;

	// Token: 0x04002AC3 RID: 10947
	public float hysterisisFactor = 4f;

	// Token: 0x04002AC4 RID: 10948
	public UnityEvent OnHitMin;

	// Token: 0x04002AC5 RID: 10949
	public UnityEvent OnHitMax;

	// Token: 0x04002AC6 RID: 10950
	private bool _lockMin;

	// Token: 0x04002AC7 RID: 10951
	private bool _lockMax;

	// Token: 0x04002AC8 RID: 10952
	public SoundBankPlayer soundsSingle;

	// Token: 0x04002AC9 RID: 10953
	public SoundBankPlayer soundsMulti;

	// Token: 0x04002ACA RID: 10954
	private TimeSince _sincelastHit;

	// Token: 0x04002ACB RID: 10955
	[FormerlySerializedAs("multiHitInterval")]
	public float multiHitCutoff = 0.1f;
}
