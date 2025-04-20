using System;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public class WingsWearable : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x060005A4 RID: 1444 RVA: 0x00034200 File Offset: 0x00032400
	private void Awake()
	{
		this.xform = this.animator.transform;
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x00034213 File Offset: 0x00032413
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		this.oldPos = this.xform.localPosition;
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x00032C92 File Offset: 0x00030E92
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00082FBC File Offset: 0x000811BC
	public void SliceUpdate()
	{
		Vector3 position = this.xform.position;
		float f = (position - this.oldPos).magnitude / Time.deltaTime;
		float value = this.flapSpeedCurve.Evaluate(Mathf.Abs(f));
		this.animator.SetFloat(this.flapSpeedParamID, value);
		this.oldPos = position;
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000683 RID: 1667
	[Tooltip("This animator must have a parameter called 'FlapSpeed'")]
	public Animator animator;

	// Token: 0x04000684 RID: 1668
	[Tooltip("X axis is move speed, Y axis is flap speed")]
	public AnimationCurve flapSpeedCurve;

	// Token: 0x04000685 RID: 1669
	private Transform xform;

	// Token: 0x04000686 RID: 1670
	private Vector3 oldPos;

	// Token: 0x04000687 RID: 1671
	private readonly int flapSpeedParamID = Animator.StringToHash("FlapSpeed");
}
