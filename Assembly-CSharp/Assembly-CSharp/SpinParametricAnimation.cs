using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020001F9 RID: 505
public class SpinParametricAnimation : MonoBehaviour
{
	// Token: 0x06000BD4 RID: 3028 RVA: 0x0003ED15 File Offset: 0x0003CF15
	protected void OnEnable()
	{
		this.axis = this.axis.normalized;
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x0003ED28 File Offset: 0x0003CF28
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

	// Token: 0x04000E35 RID: 3637
	[Tooltip("Axis to rotate around.")]
	public Vector3 axis = Vector3.up;

	// Token: 0x04000E36 RID: 3638
	[Tooltip("Whether rotation is in World Space or Local Space")]
	public bool WorldSpaceRotation = true;

	// Token: 0x04000E37 RID: 3639
	[FormerlySerializedAs("speed")]
	[Tooltip("Speed of rotation.")]
	public float revolutionsPerSecond = 0.25f;

	// Token: 0x04000E38 RID: 3640
	[Tooltip("Affects the progress of the animation over time.")]
	public AnimationCurve timeCurve;

	// Token: 0x04000E39 RID: 3641
	private float _animationProgress;

	// Token: 0x04000E3A RID: 3642
	private float _oldAngle;
}
