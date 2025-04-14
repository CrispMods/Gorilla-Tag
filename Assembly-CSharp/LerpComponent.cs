using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006BE RID: 1726
public abstract class LerpComponent : MonoBehaviour
{
	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06002ABF RID: 10943 RVA: 0x000D451C File Offset: 0x000D271C
	// (set) Token: 0x06002AC0 RID: 10944 RVA: 0x000D4524 File Offset: 0x000D2724
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

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x000D455F File Offset: 0x000D275F
	// (set) Token: 0x06002AC2 RID: 10946 RVA: 0x000D4567 File Offset: 0x000D2767
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

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06002AC3 RID: 10947 RVA: 0x000444E2 File Offset: 0x000426E2
	protected virtual bool CanRender
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002AC4 RID: 10948
	protected abstract void OnLerp(float t);

	// Token: 0x06002AC5 RID: 10949 RVA: 0x000D457F File Offset: 0x000D277F
	protected void RenderLerp()
	{
		this.OnLerp(this._lerp);
	}

	// Token: 0x06002AC6 RID: 10950 RVA: 0x000D4590 File Offset: 0x000D2790
	protected virtual int GetState()
	{
		return new ValueTuple<float, int>(this._lerp, 779562875).GetHashCode();
	}

	// Token: 0x06002AC7 RID: 10951 RVA: 0x000D45BB File Offset: 0x000D27BB
	protected virtual void Validate()
	{
		if (this._lerpLength < 0f)
		{
			this._lerpLength = 0f;
		}
	}

	// Token: 0x06002AC8 RID: 10952 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	private void OnDrawGizmosSelected()
	{
	}

	// Token: 0x06002AC9 RID: 10953 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	private void TryEditorRender(bool playModeCheck = true)
	{
	}

	// Token: 0x06002ACA RID: 10954 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	private void LerpToOne()
	{
	}

	// Token: 0x06002ACB RID: 10955 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	private void LerpToZero()
	{
	}

	// Token: 0x06002ACC RID: 10956 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	private void StartPreview(float lerpFrom, float lerpTo)
	{
	}

	// Token: 0x04003022 RID: 12322
	[SerializeField]
	[Range(0f, 1f)]
	protected float _lerp;

	// Token: 0x04003023 RID: 12323
	[SerializeField]
	protected float _lerpLength = 1f;

	// Token: 0x04003024 RID: 12324
	[Space]
	[SerializeField]
	protected LerpChangedEvent _onLerpChanged;

	// Token: 0x04003025 RID: 12325
	[SerializeField]
	protected bool _previewInEditor = true;

	// Token: 0x04003026 RID: 12326
	[NonSerialized]
	private bool _previewing;

	// Token: 0x04003027 RID: 12327
	[NonSerialized]
	private bool _cancelPreview;

	// Token: 0x04003028 RID: 12328
	[NonSerialized]
	private bool _rendering;

	// Token: 0x04003029 RID: 12329
	[NonSerialized]
	private int _lastState;

	// Token: 0x0400302A RID: 12330
	[NonSerialized]
	private float _prevLerpFrom;

	// Token: 0x0400302B RID: 12331
	[NonSerialized]
	private float _prevLerpTo;
}
