using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006D3 RID: 1747
public abstract class LerpComponent : MonoBehaviour
{
	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x06002B55 RID: 11093 RVA: 0x0004D3DB File Offset: 0x0004B5DB
	// (set) Token: 0x06002B56 RID: 11094 RVA: 0x00120560 File Offset: 0x0011E760
	public float Lerp
	{
		get
		{
			return this._lerp;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (!Mathf.Approximately(this._lerp, num))
			{
				LerpChangedEvent onLerpChanged = this._onLerpChanged;
				if (onLerpChanged != null)
				{
					onLerpChanged.Invoke(num);
				}
			}
			this._lerp = num;
		}
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x06002B57 RID: 11095 RVA: 0x0004D3E3 File Offset: 0x0004B5E3
	// (set) Token: 0x06002B58 RID: 11096 RVA: 0x0004D3EB File Offset: 0x0004B5EB
	public float LerpTime
	{
		get
		{
			return this._lerpLength;
		}
		set
		{
			this._lerpLength = ((value < 0f) ? 0f : value);
		}
	}

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x06002B59 RID: 11097 RVA: 0x00039846 File Offset: 0x00037A46
	protected virtual bool CanRender
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002B5A RID: 11098
	protected abstract void OnLerp(float t);

	// Token: 0x06002B5B RID: 11099 RVA: 0x0004D403 File Offset: 0x0004B603
	protected void RenderLerp()
	{
		this.OnLerp(this._lerp);
	}

	// Token: 0x06002B5C RID: 11100 RVA: 0x0012059C File Offset: 0x0011E79C
	protected virtual int GetState()
	{
		return new ValueTuple<float, int>(this._lerp, 779562875).GetHashCode();
	}

	// Token: 0x06002B5D RID: 11101 RVA: 0x0004D411 File Offset: 0x0004B611
	protected virtual void Validate()
	{
		if (this._lerpLength < 0f)
		{
			this._lerpLength = 0f;
		}
	}

	// Token: 0x06002B5E RID: 11102 RVA: 0x00030607 File Offset: 0x0002E807
	[Conditional("UNITY_EDITOR")]
	private void OnDrawGizmosSelected()
	{
	}

	// Token: 0x06002B5F RID: 11103 RVA: 0x00030607 File Offset: 0x0002E807
	[Conditional("UNITY_EDITOR")]
	private void TryEditorRender(bool playModeCheck = true)
	{
	}

	// Token: 0x06002B60 RID: 11104 RVA: 0x00030607 File Offset: 0x0002E807
	[Conditional("UNITY_EDITOR")]
	private void LerpToOne()
	{
	}

	// Token: 0x06002B61 RID: 11105 RVA: 0x00030607 File Offset: 0x0002E807
	[Conditional("UNITY_EDITOR")]
	private void LerpToZero()
	{
	}

	// Token: 0x06002B62 RID: 11106 RVA: 0x00030607 File Offset: 0x0002E807
	[Conditional("UNITY_EDITOR")]
	private void StartPreview(float lerpFrom, float lerpTo)
	{
	}

	// Token: 0x040030BF RID: 12479
	[SerializeField]
	[Range(0f, 1f)]
	protected float _lerp;

	// Token: 0x040030C0 RID: 12480
	[SerializeField]
	protected float _lerpLength = 1f;

	// Token: 0x040030C1 RID: 12481
	[Space]
	[SerializeField]
	protected LerpChangedEvent _onLerpChanged;

	// Token: 0x040030C2 RID: 12482
	[SerializeField]
	protected bool _previewInEditor = true;

	// Token: 0x040030C3 RID: 12483
	[NonSerialized]
	private bool _previewing;

	// Token: 0x040030C4 RID: 12484
	[NonSerialized]
	private bool _cancelPreview;

	// Token: 0x040030C5 RID: 12485
	[NonSerialized]
	private bool _rendering;

	// Token: 0x040030C6 RID: 12486
	[NonSerialized]
	private int _lastState;

	// Token: 0x040030C7 RID: 12487
	[NonSerialized]
	private float _prevLerpFrom;

	// Token: 0x040030C8 RID: 12488
	[NonSerialized]
	private float _prevLerpTo;
}
