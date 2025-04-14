using System;
using UnityEngine;

// Token: 0x02000514 RID: 1300
public class EmitSignalToBiter : GTSignalEmitter
{
	// Token: 0x06001F86 RID: 8070 RVA: 0x0009EBF4 File Offset: 0x0009CDF4
	public override void Emit()
	{
		if (this.onEdibleState == EmitSignalToBiter.EdibleState.None)
		{
			return;
		}
		if (!this.targetEdible)
		{
			return;
		}
		if (this.targetEdible.lastBiterActorID == -1)
		{
			return;
		}
		TransferrableObject.ItemStates itemState = this.targetEdible.itemState;
		if (itemState - TransferrableObject.ItemStates.State0 <= 1 || itemState == TransferrableObject.ItemStates.State2 || itemState == TransferrableObject.ItemStates.State3)
		{
			int num = (int)itemState;
			if ((this.onEdibleState & (EmitSignalToBiter.EdibleState)num) == (EmitSignalToBiter.EdibleState)num)
			{
				GTSignal.Emit(this.targetEdible.lastBiterActorID, this.signal, Array.Empty<object>());
			}
		}
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void Emit(int targetActor)
	{
	}

	// Token: 0x06001F88 RID: 8072 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void Emit(params object[] data)
	{
	}

	// Token: 0x0400235D RID: 9053
	[Space]
	public EdibleHoldable targetEdible;

	// Token: 0x0400235E RID: 9054
	[Space]
	[SerializeField]
	private EmitSignalToBiter.EdibleState onEdibleState;

	// Token: 0x02000515 RID: 1301
	[Flags]
	private enum EdibleState
	{
		// Token: 0x04002360 RID: 9056
		None = 0,
		// Token: 0x04002361 RID: 9057
		State0 = 1,
		// Token: 0x04002362 RID: 9058
		State1 = 2,
		// Token: 0x04002363 RID: 9059
		State2 = 4,
		// Token: 0x04002364 RID: 9060
		State3 = 8
	}
}
