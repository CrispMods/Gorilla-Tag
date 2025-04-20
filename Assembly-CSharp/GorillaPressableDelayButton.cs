using System;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class GorillaPressableDelayButton : GorillaPressableButton, IGorillaSliceableSimple
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000398 RID: 920 RVA: 0x0007999C File Offset: 0x00077B9C
	// (remove) Token: 0x06000399 RID: 921 RVA: 0x000799D4 File Offset: 0x00077BD4
	public event Action onPressBegin;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x0600039A RID: 922 RVA: 0x00079A0C File Offset: 0x00077C0C
	// (remove) Token: 0x0600039B RID: 923 RVA: 0x00079A44 File Offset: 0x00077C44
	public event Action onPressAbort;

	// Token: 0x0600039C RID: 924 RVA: 0x00079A7C File Offset: 0x00077C7C
	private void Awake()
	{
		if (this.fillBar == null)
		{
			return;
		}
		this.fillBarScale = (this.fillbarStartingScale = this.fillBar.localScale);
		this.UpdateFillBar();
	}

	// Token: 0x0600039D RID: 925 RVA: 0x00079AB8 File Offset: 0x00077CB8
	private new void OnTriggerEnter(Collider collider)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.touchTime + this.debounceTime >= Time.time)
		{
			return;
		}
		if (this.touching)
		{
			return;
		}
		if (collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() == null)
		{
			return;
		}
		this.touching = collider;
		this.timer = 0f;
		this.UpdateFillBar();
		Action action = this.onPressBegin;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x0600039E RID: 926 RVA: 0x00032C50 File Offset: 0x00030E50
	private void OnTriggerExit(Collider other)
	{
		if (other != this.touching)
		{
			return;
		}
		this.touching = null;
		this.timer = 0f;
		this.UpdateFillBar();
		Action action = this.onPressAbort;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x0600039F RID: 927 RVA: 0x00032C89 File Offset: 0x00030E89
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x00032C92 File Offset: 0x00030E92
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x00079B28 File Offset: 0x00077D28
	public void SliceUpdate()
	{
		if (this.touching == null)
		{
			return;
		}
		this.timer += Time.deltaTime;
		if (this.timer > this.delayTime)
		{
			base.OnTriggerEnter(this.touching);
			this.touching = null;
			this.timer = 0f;
		}
		this.UpdateFillBar();
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x00079B88 File Offset: 0x00077D88
	public void SetFillBar(Transform newFillBar)
	{
		this.fillBar = newFillBar;
		if (this.fillBar == null)
		{
			return;
		}
		this.fillBarScale = (this.fillbarStartingScale = this.fillBar.localScale);
		this.UpdateFillBar();
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00079BCC File Offset: 0x00077DCC
	private void UpdateFillBar()
	{
		if (this.fillBar == null)
		{
			return;
		}
		float num = (this.delayTime > 0f) ? Mathf.Clamp01(this.timer / this.delayTime) : ((float)((this.timer > 0f) ? 1 : 0));
		this.fillBarScale.x = this.fillbarStartingScale.x * num;
		this.fillBar.localScale = this.fillBarScale;
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000428 RID: 1064
	private Collider touching;

	// Token: 0x04000429 RID: 1065
	private float timer;

	// Token: 0x0400042A RID: 1066
	[SerializeField]
	[Range(0.01f, 5f)]
	public float delayTime = 1f;

	// Token: 0x0400042B RID: 1067
	[SerializeField]
	private Transform fillBar;

	// Token: 0x0400042C RID: 1068
	private Vector3 fillbarStartingScale;

	// Token: 0x0400042D RID: 1069
	private Vector3 fillBarScale;
}
