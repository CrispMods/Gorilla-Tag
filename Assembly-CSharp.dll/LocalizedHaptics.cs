using System;
using UnityEngine;

// Token: 0x0200035F RID: 863
public class LocalizedHaptics : MonoBehaviour
{
	// Token: 0x06001412 RID: 5138 RVA: 0x0003C84D File Offset: 0x0003AA4D
	private void Start()
	{
		this.m_controller = ((this.m_handedness == OVRInput.Handedness.LeftHanded) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x000B91FC File Offset: 0x000B73FC
	private void Update()
	{
		float amplitude = (OVRInput.Get(OVRInput.Axis1D.PrimaryThumbRestForce, this.m_controller) > 0.5f) ? 1f : 0f;
		OVRInput.SetControllerLocalizedVibration(OVRInput.HapticsLocation.Thumb, 0f, amplitude, this.m_controller);
		float amplitude2 = (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, this.m_controller) > 0.5f) ? 1f : 0f;
		OVRInput.SetControllerLocalizedVibration(OVRInput.HapticsLocation.Index, 0f, amplitude2, this.m_controller);
		float amplitude3 = (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, this.m_controller) > 0.5f) ? 1f : 0f;
		OVRInput.SetControllerLocalizedVibration(OVRInput.HapticsLocation.Hand, 0f, amplitude3, this.m_controller);
	}

	// Token: 0x0400162F RID: 5679
	[Header("Settings")]
	[SerializeField]
	private OVRInput.Handedness m_handedness = OVRInput.Handedness.LeftHanded;

	// Token: 0x04001630 RID: 5680
	private OVRInput.Controller m_controller;
}
