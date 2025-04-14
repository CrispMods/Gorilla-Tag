using System;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class RaceConsoleVisual : MonoBehaviour
{
	// Token: 0x06000AFB RID: 2811 RVA: 0x0003B2D4 File Offset: 0x000394D4
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

	// Token: 0x06000AFC RID: 2812 RVA: 0x0003B3E8 File Offset: 0x000395E8
	public void ShowCanStartRace()
	{
		this.button1.transform.localPosition = Vector3.zero;
		this.button3.transform.localPosition = Vector3.zero;
		this.button5.transform.localPosition = Vector3.zero;
		this.button1.sharedMaterial = this.pressableButton;
		this.button3.sharedMaterial = this.pressableButton;
		this.button5.sharedMaterial = this.pressableButton;
	}

	// Token: 0x04000D60 RID: 3424
	[SerializeField]
	private MeshRenderer button1;

	// Token: 0x04000D61 RID: 3425
	[SerializeField]
	private MeshRenderer button3;

	// Token: 0x04000D62 RID: 3426
	[SerializeField]
	private MeshRenderer button5;

	// Token: 0x04000D63 RID: 3427
	[SerializeField]
	private Vector3 buttonPressedOffset;

	// Token: 0x04000D64 RID: 3428
	[SerializeField]
	private Material pressableButton;

	// Token: 0x04000D65 RID: 3429
	[SerializeField]
	private Material selectedButton;

	// Token: 0x04000D66 RID: 3430
	[SerializeField]
	private Material inactiveButton;
}
