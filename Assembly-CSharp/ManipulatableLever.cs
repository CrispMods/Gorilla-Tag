using System;
using UnityEngine;

// Token: 0x020003F2 RID: 1010
public class ManipulatableLever : ManipulatableObject
{
	// Token: 0x060018A8 RID: 6312 RVA: 0x00040B58 File Offset: 0x0003ED58
	private void Awake()
	{
		this.localSpace = base.transform.worldToLocalMatrix;
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x000CCC80 File Offset: 0x000CAE80
	protected override bool ShouldHandDetach(GameObject hand)
	{
		Vector3 position = this.leverGrip.position;
		Vector3 position2 = hand.transform.position;
		return Vector3.SqrMagnitude(position - position2) > this.breakDistance * this.breakDistance;
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x000CCCC0 File Offset: 0x000CAEC0
	protected override void OnHeldUpdate(GameObject hand)
	{
		Vector3 position = hand.transform.position;
		Vector3 upwards = Vector3.Normalize(this.localSpace.MultiplyPoint3x4(position) - base.transform.localPosition);
		Vector3 eulerAngles = Quaternion.LookRotation(Vector3.forward, upwards).eulerAngles;
		if (eulerAngles.z > 180f)
		{
			eulerAngles.z -= 360f;
		}
		else if (eulerAngles.z < -180f)
		{
			eulerAngles.z += 360f;
		}
		eulerAngles.z = Mathf.Clamp(eulerAngles.z, this.minAngle, this.maxAngle);
		base.transform.localEulerAngles = eulerAngles;
	}

	// Token: 0x060018AB RID: 6315 RVA: 0x000CCD78 File Offset: 0x000CAF78
	public void SetValue(float value)
	{
		float z = Mathf.Lerp(this.minAngle, this.maxAngle, value);
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles.z = z;
		base.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x060018AC RID: 6316 RVA: 0x000CCDB8 File Offset: 0x000CAFB8
	public void SetNotch(int notchValue)
	{
		if (this.notches == null)
		{
			return;
		}
		foreach (ManipulatableLever.LeverNotch leverNotch in this.notches)
		{
			if (leverNotch.value == notchValue)
			{
				this.SetValue(Mathf.Lerp(leverNotch.minAngleValue, leverNotch.maxAngleValue, 0.5f));
				return;
			}
		}
	}

	// Token: 0x060018AD RID: 6317 RVA: 0x000CCE10 File Offset: 0x000CB010
	public float GetValue()
	{
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		if (localEulerAngles.z > 180f)
		{
			localEulerAngles.z -= 360f;
		}
		else if (localEulerAngles.z < -180f)
		{
			localEulerAngles.z += 360f;
		}
		return Mathf.InverseLerp(this.minAngle, this.maxAngle, localEulerAngles.z);
	}

	// Token: 0x060018AE RID: 6318 RVA: 0x000CCE7C File Offset: 0x000CB07C
	public int GetNotch()
	{
		if (this.notches == null)
		{
			return 0;
		}
		float value = this.GetValue();
		foreach (ManipulatableLever.LeverNotch leverNotch in this.notches)
		{
			if (value >= leverNotch.minAngleValue && value <= leverNotch.maxAngleValue)
			{
				return leverNotch.value;
			}
		}
		return 0;
	}

	// Token: 0x04001B48 RID: 6984
	[SerializeField]
	private float breakDistance = 0.2f;

	// Token: 0x04001B49 RID: 6985
	[SerializeField]
	private Transform leverGrip;

	// Token: 0x04001B4A RID: 6986
	[SerializeField]
	private float maxAngle = 22.5f;

	// Token: 0x04001B4B RID: 6987
	[SerializeField]
	private float minAngle = -22.5f;

	// Token: 0x04001B4C RID: 6988
	[SerializeField]
	private ManipulatableLever.LeverNotch[] notches;

	// Token: 0x04001B4D RID: 6989
	private Matrix4x4 localSpace;

	// Token: 0x020003F3 RID: 1011
	[Serializable]
	public class LeverNotch
	{
		// Token: 0x04001B4E RID: 6990
		public float minAngleValue;

		// Token: 0x04001B4F RID: 6991
		public float maxAngleValue;

		// Token: 0x04001B50 RID: 6992
		public int value;
	}
}
