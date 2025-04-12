using System;
using UnityEngine;

// Token: 0x02000514 RID: 1300
public class EmitSignalToBiter : GTSignalEmitter
{
	// Token: 0x06001F89 RID: 8073 RVA: 0x000ED97C File Offset: 0x000EBB7C
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

	// Token: 0x06001F8A RID: 8074 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void Emit(int targetActor)
	{
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void Emit(params object[] data)
	{
	}

	// Token: 0x0400235E RID: 9054
	[Space]
	public EdibleHoldable targetEdible;

	// Token: 0x0400235F RID: 9055
	[Space]
	[SerializeField]
	private EmitSignalToBiter.EdibleState onEdibleState;

	// Token: 0x02000515 RID: 1301
	[Flags]
	private enum EdibleState
	{
		// Token: 0x04002361 RID: 9057
		None = 0,
		// Token: 0x04002362 RID: 9058
		State0 = 1,
		// Token: 0x04002363 RID: 9059
		State1 = 2,
		// Token: 0x04002364 RID: 9060
		State2 = 4,
		// Token: 0x04002365 RID: 9061
		State3 = 8
	}
}
