using System;
using UnityEngine;

// Token: 0x02000559 RID: 1369
public class GorillaEyeExpressions : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06002171 RID: 8561 RVA: 0x00045C32 File Offset: 0x00043E32
	private void Awake()
	{
		this.loudness = base.GetComponent<GorillaSpeakerLoudness>();
	}

	// Token: 0x06002172 RID: 8562 RVA: 0x00045C40 File Offset: 0x00043E40
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.timeLastUpdated = Time.time;
		this.deltaTime = Time.deltaTime;
	}

	// Token: 0x06002173 RID: 8563 RVA: 0x00030F5E File Offset: 0x0002F15E
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x00045C5F File Offset: 0x00043E5F
	public void SliceUpdate()
	{
		this.deltaTime = Time.time - this.timeLastUpdated;
		this.timeLastUpdated = Time.time;
		this.CheckEyeEffects();
		this.UpdateEyeExpression();
	}

	// Token: 0x06002175 RID: 8565 RVA: 0x000F3E0C File Offset: 0x000F200C
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

	// Token: 0x06002176 RID: 8566 RVA: 0x000F3EA4 File Offset: 0x000F20A4
	private void UpdateEyeExpression()
	{
		Material material = this.targetFace.GetComponent<Renderer>().material;
		material.SetFloat(this._EyeOverrideUV, this.IsEyeExpressionOverriden ? 1f : 0f);
		material.SetVector(this._EyeOverrideUVTransform, new Vector4(1f, 1f, this.overrideUV.x, this.overrideUV.y));
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002529 RID: 9513
	public GameObject targetFace;

	// Token: 0x0400252A RID: 9514
	[Space]
	[SerializeField]
	private float screamVolume = 0.2f;

	// Token: 0x0400252B RID: 9515
	[SerializeField]
	private float screamDuration = 0.5f;

	// Token: 0x0400252C RID: 9516
	[SerializeField]
	private Vector2 ScreamUV = new Vector2(0.8f, 0f);

	// Token: 0x0400252D RID: 9517
	private GorillaSpeakerLoudness loudness;

	// Token: 0x0400252E RID: 9518
	private bool IsEyeExpressionOverriden;

	// Token: 0x0400252F RID: 9519
	private float overrideDuration;

	// Token: 0x04002530 RID: 9520
	private Vector2 overrideUV;

	// Token: 0x04002531 RID: 9521
	private float timeLastUpdated;

	// Token: 0x04002532 RID: 9522
	private float deltaTime;

	// Token: 0x04002533 RID: 9523
	private ShaderHashId _EyeOverrideUV = "_EyeOverrideUV";

	// Token: 0x04002534 RID: 9524
	private ShaderHashId _EyeOverrideUVTransform = "_EyeOverrideUVTransform";
}
