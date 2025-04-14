using System;
using UnityEngine;

// Token: 0x020003E7 RID: 999
public class ManipulatableLever : ManipulatableObject
{
	// Token: 0x0600185B RID: 6235 RVA: 0x00076743 File Offset: 0x00074943
	private void Awake()
	{
		this.localSpace = base.transform.worldToLocalMatrix;
	}

	// Token: 0x0600185C RID: 6236 RVA: 0x00076758 File Offset: 0x00074958
	protected override bool ShouldHandDetach(GameObject hand)
	{
		Vector3 position = this.leverGrip.position;
		Vector3 position2 = hand.transform.position;
		return Vector3.SqrMagnitude(position - position2) > this.breakDistance * this.breakDistance;
	}

	// Token: 0x0600185D RID: 6237 RVA: 0x00076798 File Offset: 0x00074998
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

	// Token: 0x0600185E RID: 6238 RVA: 0x00076850 File Offset: 0x00074A50
	public void SetValue(float value)
	{
		float z = Mathf.Lerp(this.minAngle, this.maxAngle, value);
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles.z = z;
		base.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x0600185F RID: 6239 RVA: 0x00076890 File Offset: 0x00074A90
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

	// Token: 0x06001860 RID: 6240 RVA: 0x000768E8 File Offset: 0x00074AE8
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

	// Token: 0x06001861 RID: 6241 RVA: 0x00076954 File Offset: 0x00074B54
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

	// Token: 0x04001AFF RID: 6911
	[SerializeField]
	private float breakDistance = 0.2f;

	// Token: 0x04001B00 RID: 6912
	[SerializeField]
	private Transform leverGrip;

	// Token: 0x04001B01 RID: 6913
	[SerializeField]
	private float maxAngle = 22.5f;

	// Token: 0x04001B02 RID: 6914
	[SerializeField]
	private float minAngle = -22.5f;

	// Token: 0x04001B03 RID: 6915
	[SerializeField]
	private ManipulatableLever.LeverNotch[] notches;

	// Token: 0x04001B04 RID: 6916
	private Matrix4x4 localSpace;

	// Token: 0x020003E8 RID: 1000
	[Serializable]
	public class LeverNotch
	{
		// Token: 0x04001B05 RID: 6917
		public float minAngleValue;

		// Token: 0x04001B06 RID: 6918
		public float maxAngleValue;

		// Token: 0x04001B07 RID: 6919
		public int value;
	}
}
