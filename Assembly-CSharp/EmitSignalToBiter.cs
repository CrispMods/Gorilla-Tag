using System;
using UnityEngine;

// Token: 0x02000521 RID: 1313
public class EmitSignalToBiter : GTSignalEmitter
{
	// Token: 0x06001FDF RID: 8159 RVA: 0x000F0700 File Offset: 0x000EE900
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

	// Token: 0x06001FE0 RID: 8160 RVA: 0x00030607 File Offset: 0x0002E807
	public override void Emit(int targetActor)
	{
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x00030607 File Offset: 0x0002E807
	public override void Emit(params object[] data)
	{
	}

	// Token: 0x040023B0 RID: 9136
	[Space]
	public EdibleHoldable targetEdible;

	// Token: 0x040023B1 RID: 9137
	[Space]
	[SerializeField]
	private EmitSignalToBiter.EdibleState onEdibleState;

	// Token: 0x02000522 RID: 1314
	[Flags]
	private enum EdibleState
	{
		// Token: 0x040023B3 RID: 9139
		None = 0,
		// Token: 0x040023B4 RID: 9140
		State0 = 1,
		// Token: 0x040023B5 RID: 9141
		State1 = 2,
		// Token: 0x040023B6 RID: 9142
		State2 = 4,
		// Token: 0x040023B7 RID: 9143
		State3 = 8
	}
}
