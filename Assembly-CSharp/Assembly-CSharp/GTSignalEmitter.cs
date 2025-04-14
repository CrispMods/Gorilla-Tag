using System;
using UnityEngine;

// Token: 0x020005AC RID: 1452
public class GTSignalEmitter : MonoBehaviour
{
	// Token: 0x06002408 RID: 9224 RVA: 0x000B3B07 File Offset: 0x000B1D07
	public virtual void Emit()
	{
		GTSignal.Emit(this.emitMode, this.signal, Array.Empty<object>());
	}

	// Token: 0x06002409 RID: 9225 RVA: 0x000B3B24 File Offset: 0x000B1D24
	public virtual void Emit(int targetActor)
	{
		GTSignal.Emit(targetActor, this.signal, Array.Empty<object>());
	}

	// Token: 0x0600240A RID: 9226 RVA: 0x000B3B3C File Offset: 0x000B1D3C
	public virtual void Emit(params object[] data)
	{
		GTSignal.Emit(this.emitMode, this.signal, data);
	}

	// Token: 0x040027FD RID: 10237
	[Space]
	public GTSignalID signal;

	// Token: 0x040027FE RID: 10238
	public GTSignal.EmitMode emitMode;
}
