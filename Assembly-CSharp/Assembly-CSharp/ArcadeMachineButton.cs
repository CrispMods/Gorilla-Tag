using System;
using UnityEngine;

// Token: 0x0200024C RID: 588
public class ArcadeMachineButton : GorillaPressableButton
{
	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06000DAB RID: 3499 RVA: 0x000461CC File Offset: 0x000443CC
	// (remove) Token: 0x06000DAC RID: 3500 RVA: 0x00046204 File Offset: 0x00044404
	public event ArcadeMachineButton.ArcadeMachineButtonEvent OnStateChange;

	// Token: 0x06000DAD RID: 3501 RVA: 0x00046239 File Offset: 0x00044439
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

	// Token: 0x06000DAE RID: 3502 RVA: 0x00046270 File Offset: 0x00044470
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

	// Token: 0x040010C1 RID: 4289
	private bool state;

	// Token: 0x040010C2 RID: 4290
	[SerializeField]
	private int ButtonID;

	// Token: 0x0200024D RID: 589
	// (Invoke) Token: 0x06000DB1 RID: 3505
	public delegate void ArcadeMachineButtonEvent(int id, bool state);
}
