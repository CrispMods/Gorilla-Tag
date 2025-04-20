using System;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class RaceConsoleVisual : MonoBehaviour
{
	// Token: 0x06000B47 RID: 2887 RVA: 0x0009A614 File Offset: 0x00098814
	public void ShowRaceInProgress(int laps)
	{
		this.button1.sharedMaterial = this.inactiveButton;
		this.button3.sharedMaterial = this.inactiveButton;
		this.button5.sharedMaterial = this.inactiveButton;
		this.button1.transform.localPosition = Vector3.zero;
		this.button3.transform.localPosition = Vector3.zero;
		this.button5.transform.localPosition = Vector3.zero;
		switch (laps)
		{
		default:
			this.button1.sharedMaterial = this.selectedButton;
			this.button1.transform.localPosition = this.buttonPressedOffset;
			return;
		case 3:
			this.button3.sharedMaterial = this.selectedButton;
			this.button3.transform.localPosition = this.buttonPressedOffset;
			return;
		case 5:
			this.button5.sharedMaterial = this.selectedButton;
			this.button5.transform.localPosition = this.buttonPressedOffset;
			return;
		}
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x0009A728 File Offset: 0x00098928
	public void ShowCanStartRace()
	{
		this.button1.transform.localPosition = Vector3.zero;
		this.button3.transform.localPosition = Vector3.zero;
		this.button5.transform.localPosition = Vector3.zero;
		this.button1.sharedMaterial = this.pressableButton;
		this.button3.sharedMaterial = this.pressableButton;
		this.button5.sharedMaterial = this.pressableButton;
	}

	// Token: 0x04000DA6 RID: 3494
	[SerializeField]
	private MeshRenderer button1;

	// Token: 0x04000DA7 RID: 3495
	[SerializeField]
	private MeshRenderer button3;

	// Token: 0x04000DA8 RID: 3496
	[SerializeField]
	private MeshRenderer button5;

	// Token: 0x04000DA9 RID: 3497
	[SerializeField]
	private Vector3 buttonPressedOffset;

	// Token: 0x04000DAA RID: 3498
	[SerializeField]
	private Material pressableButton;

	// Token: 0x04000DAB RID: 3499
	[SerializeField]
	private Material selectedButton;

	// Token: 0x04000DAC RID: 3500
	[SerializeField]
	private Material inactiveButton;
}
