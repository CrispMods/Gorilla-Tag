using System;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class WingsWearable : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06000565 RID: 1381 RVA: 0x00032F9C File Offset: 0x0003119C
	private void Awake()
	{
		this.xform = this.animator.transform;
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x00032FAF File Offset: 0x000311AF
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		this.oldPos = this.xform.localPosition;
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00031B2F File Offset: 0x0002FD2F
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x000806B4 File Offset: 0x0007E8B4
	public void SliceUpdate()
	{
		Vector3 position = this.xform.position;
		float f = (position - this.oldPos).magnitude / Time.deltaTime;
		float value = this.flapSpeedCurve.Evaluate(Mathf.Abs(f));
		this.animator.SetFloat(this.flapSpeedParamID, value);
		this.oldPos = position;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000643 RID: 1603
	[Tooltip("This animator must have a parameter called 'FlapSpeed'")]
	public Animator animator;

	// Token: 0x04000644 RID: 1604
	[Tooltip("X axis is move speed, Y axis is flap speed")]
	public AnimationCurve flapSpeedCurve;

	// Token: 0x04000645 RID: 1605
	private Transform xform;

	// Token: 0x04000646 RID: 1606
	private Vector3 oldPos;

	// Token: 0x04000647 RID: 1607
	private readonly int flapSpeedParamID = Animator.StringToHash("FlapSpeed");
}
