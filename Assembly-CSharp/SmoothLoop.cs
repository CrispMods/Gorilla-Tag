using System;
using System.Collections;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200041B RID: 1051
public class SmoothLoop : MonoBehaviour, IGorillaSliceableSimple, IBuildValidation
{
	// Token: 0x060019F6 RID: 6646 RVA: 0x0007F6E1 File Offset: 0x0007D8E1
	public bool BuildValidationCheck()
	{
		if (this.source == null)
		{
			Debug.LogError("missing audio source, this will fail", base.gameObject);
			return false;
		}
		return true;
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x0007F704 File Offset: 0x0007D904
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
			this.source.time = Random.Range(0f, this.source.clip.length);
		}
	}

	// Token: 0x060019F8 RID: 6648 RVA: 0x0007F784 File Offset: 0x0007D984
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

	// Token: 0x060019F9 RID: 6649 RVA: 0x0007F7C4 File Offset: 0x0007D9C4
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
			this.source.time = Random.Range(0f, this.source.clip.length);
		}
	}

	// Token: 0x060019FA RID: 6650 RVA: 0x00015902 File Offset: 0x00013B02
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060019FB RID: 6651 RVA: 0x0007F828 File Offset: 0x0007DA28
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

	// Token: 0x060019FC RID: 6652 RVA: 0x0007F87F File Offset: 0x0007DA7F
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

	// Token: 0x060019FE RID: 6654 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001CCE RID: 7374
	public AudioSource source;

	// Token: 0x04001CCF RID: 7375
	public float delay;

	// Token: 0x04001CD0 RID: 7376
	public bool randomStart;

	// Token: 0x04001CD1 RID: 7377
	[SerializeField]
	[Range(0f, 1f)]
	private float loopStart = 0.1f;

	// Token: 0x04001CD2 RID: 7378
	[SerializeField]
	[Range(0f, 1f)]
	private float loopEnd = 0.95f;
}
