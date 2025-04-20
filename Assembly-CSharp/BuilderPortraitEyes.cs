using System;
using UnityEngine;

// Token: 0x020004BC RID: 1212
public class BuilderPortraitEyes : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06001D70 RID: 7536 RVA: 0x00044219 File Offset: 0x00042419
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.scale = base.transform.lossyScale.x;
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x00044238 File Offset: 0x00042438
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.eyes.transform.position = this.eyeCenter.transform.position;
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x000E1040 File Offset: 0x000DF240
	public void SliceUpdate()
	{
		if (GorillaTagger.Instance == null)
		{
			return;
		}
		Vector3 b = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(GorillaTagger.Instance.headCollider.transform.position - this.eyeCenter.position, this.eyeCenter.forward), this.moveRadius * this.scale);
		this.eyes.transform.position = this.eyeCenter.position + b;
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x0400206D RID: 8301
	[SerializeField]
	private Transform eyeCenter;

	// Token: 0x0400206E RID: 8302
	[SerializeField]
	private GameObject eyes;

	// Token: 0x0400206F RID: 8303
	[SerializeField]
	private float moveRadius = 0.5f;

	// Token: 0x04002070 RID: 8304
	private float scale = 1f;
}
