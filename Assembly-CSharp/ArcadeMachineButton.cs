using System;
using UnityEngine;

// Token: 0x0200024C RID: 588
public class ArcadeMachineButton : GorillaPressableButton
{
	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06000DA9 RID: 3497 RVA: 0x00045E88 File Offset: 0x00044088
	// (remove) Token: 0x06000DAA RID: 3498 RVA: 0x00045EC0 File Offset: 0x000440C0
	public event ArcadeMachineButton.ArcadeMachineButtonEvent OnStateChange;

	// Token: 0x06000DAB RID: 3499 RVA: 0x00045EF5 File Offset: 0x000440F5
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		if (!this.state)
		{
			this.state = true;
			if (this.OnStateChange != null)
			{
				this.OnStateChange(this.ButtonID, this.state);
			}
		}
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x00045F2C File Offset: 0x0004412C
	private void OnTriggerExit(Collider collider)
	{
		if (!base.enabled || !this.state)
		{
			return;
		}
		if (collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() == null)
		{
			return;
		}
		this.state = false;
		if (this.OnStateChange != null)
		{
			this.OnStateChange(this.ButtonID, this.state);
		}
	}

	// Token: 0x040010C0 RID: 4288
	private bool state;

	// Token: 0x040010C1 RID: 4289
	[SerializeField]
	private int ButtonID;

	// Token: 0x0200024D RID: 589
	// (Invoke) Token: 0x06000DAF RID: 3503
	public delegate void ArcadeMachineButtonEvent(int id, bool state);
}
