using System;
using UnityEngine;

// Token: 0x02000089 RID: 137
public class GorillaPressableDelayButton : GorillaPressableButton, IGorillaSliceableSimple
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000366 RID: 870 RVA: 0x00015734 File Offset: 0x00013934
	// (remove) Token: 0x06000367 RID: 871 RVA: 0x0001576C File Offset: 0x0001396C
	public event Action onPressBegin;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000368 RID: 872 RVA: 0x000157A4 File Offset: 0x000139A4
	// (remove) Token: 0x06000369 RID: 873 RVA: 0x000157DC File Offset: 0x000139DC
	public event Action onPressAbort;

	// Token: 0x0600036A RID: 874 RVA: 0x00015814 File Offset: 0x00013A14
	private void Awake()
	{
		if (this.fillBar == null)
		{
			return;
		}
		this.fillBarScale = (this.fillbarStartingScale = this.fillBar.localScale);
		this.UpdateFillBar();
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00015850 File Offset: 0x00013A50
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

	// Token: 0x0600036C RID: 876 RVA: 0x000158C0 File Offset: 0x00013AC0
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

	// Token: 0x0600036D RID: 877 RVA: 0x000158F9 File Offset: 0x00013AF9
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00015902 File Offset: 0x00013B02
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0001590C File Offset: 0x00013B0C
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

	// Token: 0x06000370 RID: 880 RVA: 0x0001596C File Offset: 0x00013B6C
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

	// Token: 0x06000371 RID: 881 RVA: 0x000159B0 File Offset: 0x00013BB0
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

	// Token: 0x06000373 RID: 883 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040003F4 RID: 1012
	private Collider touching;

	// Token: 0x040003F5 RID: 1013
	private float timer;

	// Token: 0x040003F6 RID: 1014
	[SerializeField]
	[Range(0.01f, 5f)]
	public float delayTime = 1f;

	// Token: 0x040003F7 RID: 1015
	[SerializeField]
	private Transform fillBar;

	// Token: 0x040003F8 RID: 1016
	private Vector3 fillbarStartingScale;

	// Token: 0x040003F9 RID: 1017
	private Vector3 fillBarScale;
}
