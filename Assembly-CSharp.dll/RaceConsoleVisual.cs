using System;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class RaceConsoleVisual : MonoBehaviour
{
	// Token: 0x06000AFD RID: 2813 RVA: 0x00097D20 File Offset: 0x00095F20
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

	// Token: 0x06000AFE RID: 2814 RVA: 0x00097E34 File Offset: 0x00096034
	public void ShowCanStartRace()
	{
		this.button1.transform.localPosition = Vector3.zero;
		this.button3.transform.localPosition = Vector3.zero;
		this.button5.transform.localPosition = Vector3.zero;
		this.button1.sharedMaterial = this.pressableButton;
		this.button3.sharedMaterial = this.pressableButton;
		this.button5.sharedMaterial = this.pressableButton;
	}

	// Token: 0x04000D61 RID: 3425
	[SerializeField]
	private MeshRenderer button1;

	// Token: 0x04000D62 RID: 3426
	[SerializeField]
	private MeshRenderer button3;

	// Token: 0x04000D63 RID: 3427
	[SerializeField]
	private MeshRenderer button5;

	// Token: 0x04000D64 RID: 3428
	[SerializeField]
	private Vector3 buttonPressedOffset;

	// Token: 0x04000D65 RID: 3429
	[SerializeField]
	private Material pressableButton;

	// Token: 0x04000D66 RID: 3430
	[SerializeField]
	private Material selectedButton;

	// Token: 0x04000D67 RID: 3431
	[SerializeField]
	private Material inactiveButton;
}
