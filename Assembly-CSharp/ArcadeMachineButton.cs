using System;
using UnityEngine;

// Token: 0x02000257 RID: 599
public class ArcadeMachineButton : GorillaPressableButton
{
	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06000DF4 RID: 3572 RVA: 0x000A3054 File Offset: 0x000A1254
	// (remove) Token: 0x06000DF5 RID: 3573 RVA: 0x000A308C File Offset: 0x000A128C
	public event ArcadeMachineButton.ArcadeMachineButtonEvent OnStateChange;

	// Token: 0x06000DF6 RID: 3574 RVA: 0x00039FB2 File Offset: 0x000381B2
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

	// Token: 0x06000DF7 RID: 3575 RVA: 0x000A30C4 File Offset: 0x000A12C4
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

	// Token: 0x04001106 RID: 4358
	private bool state;

	// Token: 0x04001107 RID: 4359
	[SerializeField]
	private int ButtonID;

	// Token: 0x02000258 RID: 600
	// (Invoke) Token: 0x06000DFA RID: 3578
	public delegate void ArcadeMachineButtonEvent(int id, bool state);
}
