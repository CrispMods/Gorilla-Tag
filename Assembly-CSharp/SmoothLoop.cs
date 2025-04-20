using System;
using System.Collections;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000426 RID: 1062
public class SmoothLoop : MonoBehaviour, IGorillaSliceableSimple, IBuildValidation
{
	// Token: 0x06001A43 RID: 6723 RVA: 0x00041B8A File Offset: 0x0003FD8A
	public bool BuildValidationCheck()
	{
		if (this.source == null)
		{
			Debug.LogError("missing audio source, this will fail", base.gameObject);
			return false;
		}
		return true;
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x000D4B5C File Offset: 0x000D2D5C
	private void Start()
	{
		if (this.delay != 0f && !this.randomStart)
		{
			this.source.GTStop();
			base.StartCoroutine(this.DelayedStart());
			return;
		}
		if (this.randomStart)
		{
			if (this.source.isActiveAndEnabled)
			{
				this.source.GTPlay();
			}
			this.source.time = UnityEngine.Random.Range(0f, this.source.clip.length);
		}
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x00041BAD File Offset: 0x0003FDAD
	public void SliceUpdate()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.source.time > this.source.clip.length * this.loopEnd)
		{
			this.source.time = this.loopStart;
		}
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x000D4BDC File Offset: 0x000D2DDC
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		if (!this.sourceCheck())
		{
			return;
		}
		if (this.randomStart)
		{
			if (this.source.isActiveAndEnabled)
			{
				this.source.GTPlay();
			}
			this.source.time = UnityEngine.Random.Range(0f, this.source.clip.length);
		}
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x00032C92 File Offset: 0x00030E92
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x000D4C40 File Offset: 0x000D2E40
	private bool sourceCheck()
	{
		if (!this.source || !this.source.clip)
		{
			Debug.LogError("SmoothLoop: Disabling because AudioSource is null or has no clip assigned. Path: " + base.transform.GetPathQ(), this);
			base.enabled = false;
			base.StopAllCoroutines();
			return false;
		}
		return true;
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x00041BED File Offset: 0x0003FDED
	public IEnumerator DelayedStart()
	{
		if (!this.sourceCheck())
		{
			yield break;
		}
		yield return new WaitForSeconds(this.delay);
		this.source.GTPlay();
		yield break;
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001D17 RID: 7447
	public AudioSource source;

	// Token: 0x04001D18 RID: 7448
	public float delay;

	// Token: 0x04001D19 RID: 7449
	public bool randomStart;

	// Token: 0x04001D1A RID: 7450
	[SerializeField]
	[Range(0f, 1f)]
	private float loopStart = 0.1f;

	// Token: 0x04001D1B RID: 7451
	[SerializeField]
	[Range(0f, 1f)]
	private float loopEnd = 0.95f;
}
