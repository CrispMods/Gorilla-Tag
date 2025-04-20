using System;
using UnityEngine;

// Token: 0x0200036A RID: 874
public class LocalizedHaptics : MonoBehaviour
{
	// Token: 0x0600145B RID: 5211 RVA: 0x0003DB0D File Offset: 0x0003BD0D
	private void Start()
	{
		this.m_controller = ((this.m_handedness == OVRInput.Handedness.LeftHanded) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
	}

	// Token: 0x0600145C RID: 5212 RVA: 0x000BBA94 File Offset: 0x000B9C94
	private void Update()
	{
		float amplitude = (OVRInput.Get(OVRInput.Axis1D.PrimaryThumbRestForce, this.m_controller) > 0.5f) ? 1f : 0f;
		OVRInput.SetControllerLocalizedVibration(OVRInput.HapticsLocation.Thumb, 0f, amplitude, this.m_controller);
		float amplitude2 = (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, this.m_controller) > 0.5f) ? 1f : 0f;
		OVRInput.SetControllerLocalizedVibration(OVRInput.HapticsLocation.Index, 0f, amplitude2, this.m_controller);
		float amplitude3 = (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, this.m_controller) > 0.5f) ? 1f : 0f;
		OVRInput.SetControllerLocalizedVibration(OVRInput.HapticsLocation.Hand, 0f, amplitude3, this.m_controller);
	}

	// Token: 0x04001676 RID: 5750
	[Header("Settings")]
	[SerializeField]
	private OVRInput.Handedness m_handedness = OVRInput.Handedness.LeftHanded;

	// Token: 0x04001677 RID: 5751
	private OVRInput.Controller m_controller;
}
