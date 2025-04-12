using System;
using System.Collections;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200041B RID: 1051
public class SmoothLoop : MonoBehaviour, IGorillaSliceableSimple, IBuildValidation
{
	// Token: 0x060019F9 RID: 6649 RVA: 0x000408A0 File Offset: 0x0003EAA0
	public bool BuildValidationCheck()
	{
		if (this.source == null)
		{
			Debug.LogError("missing audio source, this will fail", base.gameObject);
			return false;
		}
		return true;
	}

	// Token: 0x060019FA RID: 6650 RVA: 0x000D2334 File Offset: 0x000D0534
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

	// Token: 0x060019FB RID: 6651 RVA: 0x000408C3 File Offset: 0x0003EAC3
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

	// Token: 0x060019FC RID: 6652 RVA: 0x000D23B4 File Offset: 0x000D05B4
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

	// Token: 0x060019FD RID: 6653 RVA: 0x00031B2F File Offset: 0x0002FD2F
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060019FE RID: 6654 RVA: 0x000D2418 File Offset: 0x000D0618
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

	// Token: 0x060019FF RID: 6655 RVA: 0x00040903 File Offset: 0x0003EB03
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

	// Token: 0x06001A01 RID: 6657 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001CCF RID: 7375
	public AudioSource source;

	// Token: 0x04001CD0 RID: 7376
	public float delay;

	// Token: 0x04001CD1 RID: 7377
	public bool randomStart;

	// Token: 0x04001CD2 RID: 7378
	[SerializeField]
	[Range(0f, 1f)]
	private float loopStart = 0.1f;

	// Token: 0x04001CD3 RID: 7379
	[SerializeField]
	[Range(0f, 1f)]
	private float loopEnd = 0.95f;
}
