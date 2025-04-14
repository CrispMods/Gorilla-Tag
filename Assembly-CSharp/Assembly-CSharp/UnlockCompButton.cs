using System;
using GorillaNetworking;

// Token: 0x02000692 RID: 1682
public class UnlockCompButton : GorillaPressableButton
{
	// Token: 0x060029D4 RID: 10708 RVA: 0x000CFE6D File Offset: 0x000CE06D
	public override void Start()
	{
		this.initialized = false;
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x000CFE78 File Offset: 0x000CE078
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

	// Token: 0x060029D6 RID: 10710 RVA: 0x000CFED0 File Offset: 0x000CE0D0
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

	// Token: 0x04002F41 RID: 12097
	public string gameMode;

	// Token: 0x04002F42 RID: 12098
	private bool initialized;
}
