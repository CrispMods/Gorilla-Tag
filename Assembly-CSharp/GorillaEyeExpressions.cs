using System;
using UnityEngine;

// Token: 0x02000566 RID: 1382
public class GorillaEyeExpressions : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x060021C7 RID: 8647 RVA: 0x00046FD7 File Offset: 0x000451D7
	private void Awake()
	{
		this.loudness = base.GetComponent<GorillaSpeakerLoudness>();
	}

	// Token: 0x060021C8 RID: 8648 RVA: 0x00046FE5 File Offset: 0x000451E5
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.timeLastUpdated = Time.time;
		this.deltaTime = Time.deltaTime;
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x000320C8 File Offset: 0x000302C8
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060021CA RID: 8650 RVA: 0x00047004 File Offset: 0x00045204
	public void SliceUpdate()
	{
		this.deltaTime = Time.time - this.timeLastUpdated;
		this.timeLastUpdated = Time.time;
		this.CheckEyeEffects();
		this.UpdateEyeExpression();
	}

	// Token: 0x060021CB RID: 8651 RVA: 0x000F6B88 File Offset: 0x000F4D88
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

	// Token: 0x060021CC RID: 8652 RVA: 0x000F6C20 File Offset: 0x000F4E20
	private void UpdateEyeExpression()
	{
		Material material = this.targetFace.GetComponent<Renderer>().material;
		material.SetFloat(this._EyeOverrideUV, this.IsEyeExpressionOverriden ? 1f : 0f);
		material.SetVector(this._EyeOverrideUVTransform, new Vector4(1f, 1f, this.overrideUV.x, this.overrideUV.y));
	}

	// Token: 0x060021CE RID: 8654 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x0400257B RID: 9595
	public GameObject targetFace;

	// Token: 0x0400257C RID: 9596
	[Space]
	[SerializeField]
	private float screamVolume = 0.2f;

	// Token: 0x0400257D RID: 9597
	[SerializeField]
	private float screamDuration = 0.5f;

	// Token: 0x0400257E RID: 9598
	[SerializeField]
	private Vector2 ScreamUV = new Vector2(0.8f, 0f);

	// Token: 0x0400257F RID: 9599
	private GorillaSpeakerLoudness loudness;

	// Token: 0x04002580 RID: 9600
	private bool IsEyeExpressionOverriden;

	// Token: 0x04002581 RID: 9601
	private float overrideDuration;

	// Token: 0x04002582 RID: 9602
	private Vector2 overrideUV;

	// Token: 0x04002583 RID: 9603
	private float timeLastUpdated;

	// Token: 0x04002584 RID: 9604
	private float deltaTime;

	// Token: 0x04002585 RID: 9605
	private ShaderHashId _EyeOverrideUV = "_EyeOverrideUV";

	// Token: 0x04002586 RID: 9606
	private ShaderHashId _EyeOverrideUVTransform = "_EyeOverrideUVTransform";
}
