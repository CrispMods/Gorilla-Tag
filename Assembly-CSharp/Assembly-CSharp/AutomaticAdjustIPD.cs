using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200048C RID: 1164
public class AutomaticAdjustIPD : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06001C1C RID: 7196 RVA: 0x00015C1D File Offset: 0x00013E1D
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x00015C26 File Offset: 0x00013E26
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x00088AD4 File Offset: 0x00086CD4
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

	// Token: 0x06001C20 RID: 7200 RVA: 0x0000FD18 File Offset: 0x0000DF18
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001F1E RID: 7966
	public InputDevice headset;

	// Token: 0x04001F1F RID: 7967
	public float currentIPD;

	// Token: 0x04001F20 RID: 7968
	public Vector3 leftEyePosition;

	// Token: 0x04001F21 RID: 7969
	public Vector3 rightEyePosition;

	// Token: 0x04001F22 RID: 7970
	public bool testOverride;

	// Token: 0x04001F23 RID: 7971
	public Transform[] adjustXScaleObjects;

	// Token: 0x04001F24 RID: 7972
	public float sizeAt58mm = 1f;

	// Token: 0x04001F25 RID: 7973
	public float sizeAt63mm = 1.12f;

	// Token: 0x04001F26 RID: 7974
	public float lastIPD;
}
