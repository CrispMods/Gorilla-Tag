using System;
using UnityEngine;

// Token: 0x02000558 RID: 1368
public class GorillaEyeExpressions : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06002169 RID: 8553 RVA: 0x000A64E5 File Offset: 0x000A46E5
	private void Awake()
	{
		this.loudness = base.GetComponent<GorillaSpeakerLoudness>();
	}

	// Token: 0x0600216A RID: 8554 RVA: 0x000A64F3 File Offset: 0x000A46F3
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.timeLastUpdated = Time.time;
		this.deltaTime = Time.deltaTime;
	}

	// Token: 0x0600216B RID: 8555 RVA: 0x0000F86B File Offset: 0x0000DA6B
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x0600216C RID: 8556 RVA: 0x000A6512 File Offset: 0x000A4712
	public void SliceUpdate()
	{
		this.deltaTime = Time.time - this.timeLastUpdated;
		this.timeLastUpdated = Time.time;
		this.CheckEyeEffects();
		this.UpdateEyeExpression();
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x000A6540 File Offset: 0x000A4740
	private void CheckEyeEffects()
	{
		if (this.loudness == null)
		{
			this.loudness = base.GetComponent<GorillaSpeakerLoudness>();
		}
		if (this.loudness.IsSpeaking && this.loudness.Loudness > this.screamVolume)
		{
			this.IsEyeExpressionOverriden = true;
			this.overrideDuration = this.screamDuration;
			this.overrideUV = this.ScreamUV;
			return;
		}
		if (this.IsEyeExpressionOverriden)
		{
			this.overrideDuration -= this.deltaTime;
			if (this.overrideDuration < 0f)
			{
				this.IsEyeExpressionOverriden = false;
			}
		}
	}

	// Token: 0x0600216E RID: 8558 RVA: 0x000A65D8 File Offset: 0x000A47D8
	private void UpdateEyeExpression()
	{
		Material material = this.targetFace.GetComponent<Renderer>().material;
		material.SetFloat(this._EyeOverrideUV, this.IsEyeExpressionOverriden ? 1f : 0f);
		material.SetVector(this._EyeOverrideUVTransform, new Vector4(1f, 1f, this.overrideUV.x, this.overrideUV.y));
	}

	// Token: 0x06002170 RID: 8560 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002523 RID: 9507
	public GameObject targetFace;

	// Token: 0x04002524 RID: 9508
	[Space]
	[SerializeField]
	private float screamVolume = 0.2f;

	// Token: 0x04002525 RID: 9509
	[SerializeField]
	private float screamDuration = 0.5f;

	// Token: 0x04002526 RID: 9510
	[SerializeField]
	private Vector2 ScreamUV = new Vector2(0.8f, 0f);

	// Token: 0x04002527 RID: 9511
	private GorillaSpeakerLoudness loudness;

	// Token: 0x04002528 RID: 9512
	private bool IsEyeExpressionOverriden;

	// Token: 0x04002529 RID: 9513
	private float overrideDuration;

	// Token: 0x0400252A RID: 9514
	private Vector2 overrideUV;

	// Token: 0x0400252B RID: 9515
	private float timeLastUpdated;

	// Token: 0x0400252C RID: 9516
	private float deltaTime;

	// Token: 0x0400252D RID: 9517
	private ShaderHashId _EyeOverrideUV = "_EyeOverrideUV";

	// Token: 0x0400252E RID: 9518
	private ShaderHashId _EyeOverrideUVTransform = "_EyeOverrideUVTransform";
}
