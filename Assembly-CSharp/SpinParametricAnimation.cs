using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000204 RID: 516
public class SpinParametricAnimation : MonoBehaviour
{
	// Token: 0x06000C1D RID: 3101 RVA: 0x000387A1 File Offset: 0x000369A1
	protected void OnEnable()
	{
		this.axis = this.axis.normalized;
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x0009D3BC File Offset: 0x0009B5BC
	protected void LateUpdate()
	{
		Transform transform = base.transform;
		this._animationProgress = (this._animationProgress + Time.deltaTime * this.revolutionsPerSecond) % 1f;
		float num = this.timeCurve.Evaluate(this._animationProgress) * 360f;
		float angle = num - this._oldAngle;
		this._oldAngle = num;
		if (this.WorldSpaceRotation)
		{
			transform.rotation = Quaternion.AngleAxis(angle, this.axis) * transform.rotation;
			return;
		}
		transform.localRotation = Quaternion.AngleAxis(angle, this.axis) * transform.localRotation;
	}

	// Token: 0x04000E7A RID: 3706
	[Tooltip("Axis to rotate around.")]
	public Vector3 axis = Vector3.up;

	// Token: 0x04000E7B RID: 3707
	[Tooltip("Whether rotation is in World Space or Local Space")]
	public bool WorldSpaceRotation = true;

	// Token: 0x04000E7C RID: 3708
	[FormerlySerializedAs("speed")]
	[Tooltip("Speed of rotation.")]
	public float revolutionsPerSecond = 0.25f;

	// Token: 0x04000E7D RID: 3709
	[Tooltip("Affects the progress of the animation over time.")]
	public AnimationCurve timeCurve;

	// Token: 0x04000E7E RID: 3710
	private float _animationProgress;

	// Token: 0x04000E7F RID: 3711
	private float _oldAngle;
}
