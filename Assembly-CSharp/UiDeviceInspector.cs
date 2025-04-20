using System;
using TMPro;
using UnityEngine;

// Token: 0x02000370 RID: 880
public class UiDeviceInspector : MonoBehaviour
{
	// Token: 0x06001471 RID: 5233 RVA: 0x0003DC51 File Offset: 0x0003BE51
	private void Start()
	{
		this.m_controller = ((this.m_handedness == OVRInput.Handedness.LeftHanded) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
	}

	// Token: 0x06001472 RID: 5234 RVA: 0x000BC028 File Offset: 0x000BA228
	private void Update()
	{
		string sourceText = UiDeviceInspector.ToDeviceModel() + " [" + UiDeviceInspector.ToHandednessString(this.m_handedness) + "]";
		this.m_title.SetText(sourceText, true);
		string text = OVRInput.IsControllerConnected(this.m_controller) ? "<color=#66ff87>o</color>" : "<color=#ff8991>x</color>";
		string text2 = (OVRInput.GetControllerOrientationTracked(this.m_controller) && OVRInput.GetControllerPositionTracked(this.m_controller)) ? "<color=#66ff87>o</color>" : "<color=#ff8991>x</color>";
		this.m_status.SetText(string.Concat(new string[]
		{
			"Connected [",
			text,
			"] Tracked [",
			text2,
			"]"
		}), true);
		this.m_thumbRestTouch.SetValue(OVRInput.Get(OVRInput.Touch.PrimaryThumbRest, this.m_controller));
		this.m_indexTrigger.SetValue(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, this.m_controller));
		this.m_gripTrigger.SetValue(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, this.m_controller));
		this.m_thumbRestForce.SetValue(OVRInput.Get(OVRInput.Axis1D.PrimaryThumbRestForce, this.m_controller));
		this.m_stylusTipForce.SetValue(OVRInput.Get(OVRInput.Axis1D.PrimaryStylusForce, this.m_controller));
		this.m_indexCurl1d.SetValue(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTriggerCurl, this.m_controller));
		this.m_indexSlider1d.SetValue(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTriggerSlide, this.m_controller));
		this.m_ax.SetValue(OVRInput.Get(OVRInput.Button.One, this.m_controller));
		this.m_axTouch.SetValue(OVRInput.Get(OVRInput.Touch.One, this.m_controller));
		this.m_by.SetValue(OVRInput.Get(OVRInput.Button.Two, this.m_controller));
		this.m_byTouch.SetValue(OVRInput.Get(OVRInput.Touch.Two, this.m_controller));
		this.m_indexTouch.SetValue(OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger, this.m_controller));
		this.m_thumbstick.SetValue(OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, this.m_controller), OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, this.m_controller));
	}

	// Token: 0x06001473 RID: 5235 RVA: 0x0003DC66 File Offset: 0x0003BE66
	private static string ToDeviceModel()
	{
		return "Touch";
	}

	// Token: 0x06001474 RID: 5236 RVA: 0x0003DC6D File Offset: 0x0003BE6D
	private static string ToHandednessString(OVRInput.Handedness handedness)
	{
		if (handedness == OVRInput.Handedness.LeftHanded)
		{
			return "L";
		}
		if (handedness != OVRInput.Handedness.RightHanded)
		{
			return "-";
		}
		return "R";
	}

	// Token: 0x04001692 RID: 5778
	[Header("Settings")]
	[SerializeField]
	private OVRInput.Handedness m_handedness = OVRInput.Handedness.LeftHanded;

	// Token: 0x04001693 RID: 5779
	[Header("Left Column Components")]
	[SerializeField]
	private TextMeshProUGUI m_title;

	// Token: 0x04001694 RID: 5780
	[SerializeField]
	private TextMeshProUGUI m_status;

	// Token: 0x04001695 RID: 5781
	[SerializeField]
	private UiBoolInspector m_thumbRestTouch;

	// Token: 0x04001696 RID: 5782
	[SerializeField]
	private UiAxis1dInspector m_thumbRestForce;

	// Token: 0x04001697 RID: 5783
	[SerializeField]
	private UiAxis1dInspector m_indexTrigger;

	// Token: 0x04001698 RID: 5784
	[SerializeField]
	private UiAxis1dInspector m_gripTrigger;

	// Token: 0x04001699 RID: 5785
	[SerializeField]
	private UiAxis1dInspector m_stylusTipForce;

	// Token: 0x0400169A RID: 5786
	[SerializeField]
	private UiAxis1dInspector m_indexCurl1d;

	// Token: 0x0400169B RID: 5787
	[SerializeField]
	private UiAxis1dInspector m_indexSlider1d;

	// Token: 0x0400169C RID: 5788
	[Header("Right Column Components")]
	[SerializeField]
	private UiBoolInspector m_ax;

	// Token: 0x0400169D RID: 5789
	[SerializeField]
	private UiBoolInspector m_axTouch;

	// Token: 0x0400169E RID: 5790
	[SerializeField]
	private UiBoolInspector m_by;

	// Token: 0x0400169F RID: 5791
	[SerializeField]
	private UiBoolInspector m_byTouch;

	// Token: 0x040016A0 RID: 5792
	[SerializeField]
	private UiBoolInspector m_indexTouch;

	// Token: 0x040016A1 RID: 5793
	[SerializeField]
	private UiAxis2dInspector m_thumbstick;

	// Token: 0x040016A2 RID: 5794
	private OVRInput.Controller m_controller;
}
