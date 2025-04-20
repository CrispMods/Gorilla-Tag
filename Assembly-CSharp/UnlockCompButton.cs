using System;
using GorillaNetworking;

// Token: 0x0200065B RID: 1627
public class UnlockCompButton : GorillaPressableButton
{
	// Token: 0x0600284B RID: 10315 RVA: 0x0004B66E File Offset: 0x0004986E
	public override void Start()
	{
		this.initialized = false;
	}

	// Token: 0x0600284C RID: 10316 RVA: 0x00110BB0 File Offset: 0x0010EDB0
	public void Update()
	{
		if (this.testPress)
		{
			this.testPress = false;
			this.ButtonActivation();
		}
		if (!this.initialized && GorillaComputer.instance != null)
		{
			this.isOn = GorillaComputer.instance.allowedInCompetitive;
			this.UpdateColor();
			this.initialized = true;
		}
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x0004B677 File Offset: 0x00049877
	public override void ButtonActivation()
	{
		if (!this.isOn)
		{
			base.ButtonActivation();
			GorillaComputer.instance.CompQueueUnlockButtonPress();
			this.isOn = true;
			this.UpdateColor();
		}
	}

	// Token: 0x04002DAB RID: 11691
	public string gameMode;

	// Token: 0x04002DAC RID: 11692
	private bool initialized;
}
