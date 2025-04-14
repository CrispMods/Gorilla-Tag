using System;
using TMPro;
using UnityEngine;

// Token: 0x02000365 RID: 869
public class UiDeviceInspector : MonoBehaviour
{
	// Token: 0x06001425 RID: 5157 RVA: 0x00062B7F File Offset: 0x00060D7F
	private void Start()
	{
		this.m_controller = ((this.m_handedness == OVRInput.Handedness.LeftHanded) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x00062B94 File Offset: 0x00060D94
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

	// Token: 0x06001427 RID: 5159 RVA: 0x00062D93 File Offset: 0x00060F93
	private static string ToDeviceModel()
	{
		return "Touch";
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x00062D9A File Offset: 0x00060F9A
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

	// Token: 0x0400164A RID: 5706
	[Header("Settings")]
	[SerializeField]
	private OVRInput.Handedness m_handedness = OVRInput.Handedness.LeftHanded;

	// Token: 0x0400164B RID: 5707
	[Header("Left Column Components")]
	[SerializeField]
	private TextMeshProUGUI m_title;

	// Token: 0x0400164C RID: 5708
	[SerializeField]
	private TextMeshProUGUI m_status;

	// Token: 0x0400164D RID: 5709
	[SerializeField]
	private UiBoolInspector m_thumbRestTouch;

	// Token: 0x0400164E RID: 5710
	[SerializeField]
	private UiAxis1dInspector m_thumbRestForce;

	// Token: 0x0400164F RID: 5711
	[SerializeField]
	private UiAxis1dInspector m_indexTrigger;

	// Token: 0x04001650 RID: 5712
	[SerializeField]
	private UiAxis1dInspector m_gripTrigger;

	// Token: 0x04001651 RID: 5713
	[SerializeField]
	private UiAxis1dInspector m_stylusTipForce;

	// Token: 0x04001652 RID: 5714
	[SerializeField]
	private UiAxis1dInspector m_indexCurl1d;

	// Token: 0x04001653 RID: 5715
	[SerializeField]
	private UiAxis1dInspector m_indexSlider1d;

	// Token: 0x04001654 RID: 5716
	[Header("Right Column Components")]
	[SerializeField]
	private UiBoolInspector m_ax;

	// Token: 0x04001655 RID: 5717
	[SerializeField]
	private UiBoolInspector m_axTouch;

	// Token: 0x04001656 RID: 5718
	[SerializeField]
	private UiBoolInspector m_by;

	// Token: 0x04001657 RID: 5719
	[SerializeField]
	private UiBoolInspector m_byTouch;

	// Token: 0x04001658 RID: 5720
	[SerializeField]
	private UiBoolInspector m_indexTouch;

	// Token: 0x04001659 RID: 5721
	[SerializeField]
	private UiAxis2dInspector m_thumbstick;

	// Token: 0x0400165A RID: 5722
	private OVRInput.Controller m_controller;
}
