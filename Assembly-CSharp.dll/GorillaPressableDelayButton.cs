using System;
using UnityEngine;

// Token: 0x02000089 RID: 137
public class GorillaPressableDelayButton : GorillaPressableButton, IGorillaSliceableSimple
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000368 RID: 872 RVA: 0x00077274 File Offset: 0x00075474
	// (remove) Token: 0x06000369 RID: 873 RVA: 0x000772AC File Offset: 0x000754AC
	public event Action onPressBegin;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x0600036A RID: 874 RVA: 0x000772E4 File Offset: 0x000754E4
	// (remove) Token: 0x0600036B RID: 875 RVA: 0x0007731C File Offset: 0x0007551C
	public event Action onPressAbort;

	// Token: 0x0600036C RID: 876 RVA: 0x00077354 File Offset: 0x00075554
	private void Awake()
	{
		if (this.fillBar == null)
		{
			return;
		}
		this.fillBarScale = (this.fillbarStartingScale = this.fillBar.localScale);
		this.UpdateFillBar();
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00077390 File Offset: 0x00075590
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

	// Token: 0x0600036E RID: 878 RVA: 0x00031AED File Offset: 0x0002FCED
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

	// Token: 0x0600036F RID: 879 RVA: 0x00031B26 File Offset: 0x0002FD26
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00031B2F File Offset: 0x0002FD2F
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00077400 File Offset: 0x00075600
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

	// Token: 0x06000372 RID: 882 RVA: 0x00077460 File Offset: 0x00075660
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

	// Token: 0x06000373 RID: 883 RVA: 0x000774A4 File Offset: 0x000756A4
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

	// Token: 0x06000375 RID: 885 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040003F5 RID: 1013
	private Collider touching;

	// Token: 0x040003F6 RID: 1014
	private float timer;

	// Token: 0x040003F7 RID: 1015
	[SerializeField]
	[Range(0.01f, 5f)]
	public float delayTime = 1f;

	// Token: 0x040003F8 RID: 1016
	[SerializeField]
	private Transform fillBar;

	// Token: 0x040003F9 RID: 1017
	private Vector3 fillbarStartingScale;

	// Token: 0x040003FA RID: 1018
	private Vector3 fillBarScale;
}
