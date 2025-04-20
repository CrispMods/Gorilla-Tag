using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000498 RID: 1176
public class AutomaticAdjustIPD : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06001C6D RID: 7277 RVA: 0x00032C89 File Offset: 0x00030E89
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06001C6E RID: 7278 RVA: 0x00032C92 File Offset: 0x00030E92
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x000DC248 File Offset: 0x000DA448
	public void SliceUpdate()
	{
		if (!this.headset.isValid)
		{
			this.headset = InputDevices.GetDeviceAtXRNode(XRNode.Head);
		}
		if (this.headset.isValid && this.headset.TryGetFeatureValue(CommonUsages.leftEyePosition, out this.leftEyePosition) && this.headset.TryGetFeatureValue(CommonUsages.rightEyePosition, out this.rightEyePosition))
		{
			this.currentIPD = (this.rightEyePosition - this.leftEyePosition).magnitude;
			if (Mathf.Abs(this.lastIPD - this.currentIPD) < 0.01f)
			{
				return;
			}
			this.lastIPD = this.currentIPD;
			for (int i = 0; i < this.adjustXScaleObjects.Length; i++)
			{
				Transform transform = this.adjustXScaleObjects[i];
				if (!transform)
				{
					return;
				}
				transform.localScale = new Vector3(Mathf.LerpUnclamped(1f, 1.12f, (this.currentIPD - 0.058f) / 0.0050000027f), 1f, 1f);
			}
		}
	}

	// Token: 0x06001C71 RID: 7281 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001F6C RID: 8044
	public InputDevice headset;

	// Token: 0x04001F6D RID: 8045
	public float currentIPD;

	// Token: 0x04001F6E RID: 8046
	public Vector3 leftEyePosition;

	// Token: 0x04001F6F RID: 8047
	public Vector3 rightEyePosition;

	// Token: 0x04001F70 RID: 8048
	public bool testOverride;

	// Token: 0x04001F71 RID: 8049
	public Transform[] adjustXScaleObjects;

	// Token: 0x04001F72 RID: 8050
	public float sizeAt58mm = 1f;

	// Token: 0x04001F73 RID: 8051
	public float sizeAt63mm = 1.12f;

	// Token: 0x04001F74 RID: 8052
	public float lastIPD;
}
