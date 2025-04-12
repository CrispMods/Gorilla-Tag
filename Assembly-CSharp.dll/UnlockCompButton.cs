using System;
using GorillaNetworking;

// Token: 0x02000692 RID: 1682
public class UnlockCompButton : GorillaPressableButton
{
	// Token: 0x060029D4 RID: 10708 RVA: 0x0004B769 File Offset: 0x00049969
	public override void Start()
	{
		this.initialized = false;
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x001177A8 File Offset: 0x001159A8
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

	// Token: 0x060029D6 RID: 10710 RVA: 0x0004B772 File Offset: 0x00049972
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
