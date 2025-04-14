using System;
using UnityEngine;

// Token: 0x020005AB RID: 1451
public class GTSignalEmitter : MonoBehaviour
{
	// Token: 0x06002400 RID: 9216 RVA: 0x000B3687 File Offset: 0x000B1887
	public virtual void Emit()
	{
		GTSignal.Emit(this.emitMode, this.signal, Array.Empty<object>());
	}

	// Token: 0x06002401 RID: 9217 RVA: 0x000B36A4 File Offset: 0x000B18A4
	public virtual void Emit(int targetActor)
	{
		GTSignal.Emit(targetActor, this.signal, Array.Empty<object>());
	}

	// Token: 0x06002402 RID: 9218 RVA: 0x000B36BC File Offset: 0x000B18BC
	public virtual void Emit(params object[] data)
	{
		GTSignal.Emit(this.emitMode, this.signal, data);
	}

	// Token: 0x040027F7 RID: 10231
	[Space]
	public GTSignalID signal;

	// Token: 0x040027F8 RID: 10232
	public GTSignal.EmitMode emitMode;
}
