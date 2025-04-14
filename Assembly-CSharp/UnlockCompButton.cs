using System;
using GorillaNetworking;

// Token: 0x02000691 RID: 1681
public class UnlockCompButton : GorillaPressableButton
{
	// Token: 0x060029CC RID: 10700 RVA: 0x000CF9ED File Offset: 0x000CDBED
	public override void Start()
	{
		this.initialized = false;
	}

	// Token: 0x060029CD RID: 10701 RVA: 0x000CF9F8 File Offset: 0x000CDBF8
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

	// Token: 0x060029CE RID: 10702 RVA: 0x000CFA50 File Offset: 0x000CDC50
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

	// Token: 0x04002F3B RID: 12091
	public string gameMode;

	// Token: 0x04002F3C RID: 12092
	private bool initialized;
}
