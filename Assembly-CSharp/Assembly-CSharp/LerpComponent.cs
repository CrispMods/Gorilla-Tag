using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006BF RID: 1727
public abstract class LerpComponent : MonoBehaviour
{
	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06002AC7 RID: 10951 RVA: 0x000D499C File Offset: 0x000D2B9C
	// (set) Token: 0x06002AC8 RID: 10952 RVA: 0x000D49A4 File Offset: 0x000D2BA4
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

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06002AC9 RID: 10953 RVA: 0x000D49DF File Offset: 0x000D2BDF
	// (set) Token: 0x06002ACA RID: 10954 RVA: 0x000D49E7 File Offset: 0x000D2BE7
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

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06002ACB RID: 10955 RVA: 0x00044826 File Offset: 0x00042A26
	protected virtual bool CanRender
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002ACC RID: 10956
	protected abstract void OnLerp(float t);

	// Token: 0x06002ACD RID: 10957 RVA: 0x000D49FF File Offset: 0x000D2BFF
	protected void RenderLerp()
	{
		this.OnLerp(this._lerp);
	}

	// Token: 0x06002ACE RID: 10958 RVA: 0x000D4A10 File Offset: 0x000D2C10
	protected virtual int GetState()
	{
		return new ValueTuple<float, int>(this._lerp, 779562875).GetHashCode();
	}

	// Token: 0x06002ACF RID: 10959 RVA: 0x000D4A3B File Offset: 0x000D2C3B
	protected virtual void Validate()
	{
		if (this._lerpLength < 0f)
		{
			this._lerpLength = 0f;
		}
	}

	// Token: 0x06002AD0 RID: 10960 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	private void OnDrawGizmosSelected()
	{
	}

	// Token: 0x06002AD1 RID: 10961 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	private void TryEditorRender(bool playModeCheck = true)
	{
	}

	// Token: 0x06002AD2 RID: 10962 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	private void LerpToOne()
	{
	}

	// Token: 0x06002AD3 RID: 10963 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	private void LerpToZero()
	{
	}

	// Token: 0x06002AD4 RID: 10964 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	private void StartPreview(float lerpFrom, float lerpTo)
	{
	}

	// Token: 0x04003028 RID: 12328
	[SerializeField]
	[Range(0f, 1f)]
	protected float _lerp;

	// Token: 0x04003029 RID: 12329
	[SerializeField]
	protected float _lerpLength = 1f;

	// Token: 0x0400302A RID: 12330
	[Space]
	[SerializeField]
	protected LerpChangedEvent _onLerpChanged;

	// Token: 0x0400302B RID: 12331
	[SerializeField]
	protected bool _previewInEditor = true;

	// Token: 0x0400302C RID: 12332
	[NonSerialized]
	private bool _previewing;

	// Token: 0x0400302D RID: 12333
	[NonSerialized]
	private bool _cancelPreview;

	// Token: 0x0400302E RID: 12334
	[NonSerialized]
	private bool _rendering;

	// Token: 0x0400302F RID: 12335
	[NonSerialized]
	private int _lastState;

	// Token: 0x04003030 RID: 12336
	[NonSerialized]
	private float _prevLerpFrom;

	// Token: 0x04003031 RID: 12337
	[NonSerialized]
	private float _prevLerpTo;
}
