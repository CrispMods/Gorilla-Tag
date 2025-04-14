using System;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class WingsWearable : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06000563 RID: 1379 RVA: 0x0002001F File Offset: 0x0001E21F
	private void Awake()
	{
		this.xform = this.animator.transform;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x00020032 File Offset: 0x0001E232
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		this.oldPos = this.xform.localPosition;
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x00015902 File Offset: 0x00013B02
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x0002004C File Offset: 0x0001E24C
	public void SliceUpdate()
	{
		Vector3 position = this.xform.position;
		float f = (position - this.oldPos).magnitude / Time.deltaTime;
		float value = this.flapSpeedCurve.Evaluate(Mathf.Abs(f));
		this.animator.SetFloat(this.flapSpeedParamID, value);
		this.oldPos = position;
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000642 RID: 1602
	[Tooltip("This animator must have a parameter called 'FlapSpeed'")]
	public Animator animator;

	// Token: 0x04000643 RID: 1603
	[Tooltip("X axis is move speed, Y axis is flap speed")]
	public AnimationCurve flapSpeedCurve;

	// Token: 0x04000644 RID: 1604
	private Transform xform;

	// Token: 0x04000645 RID: 1605
	private Vector3 oldPos;

	// Token: 0x04000646 RID: 1606
	private readonly int flapSpeedParamID = Animator.StringToHash("FlapSpeed");
}
