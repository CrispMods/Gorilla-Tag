using System;
using UnityEngine;

// Token: 0x020005B9 RID: 1465
public class GTSignalEmitter : MonoBehaviour
{
	// Token: 0x06002460 RID: 9312 RVA: 0x00048A88 File Offset: 0x00046C88
	public virtual void Emit()
	{
		GTSignal.Emit(this.emitMode, this.signal, Array.Empty<object>());
	}

	// Token: 0x06002461 RID: 9313 RVA: 0x00048AA5 File Offset: 0x00046CA5
	public virtual void Emit(int targetActor)
	{
		GTSignal.Emit(targetActor, this.signal, Array.Empty<object>());
	}

	// Token: 0x06002462 RID: 9314 RVA: 0x00048ABD File Offset: 0x00046CBD
	public virtual void Emit(params object[] data)
	{
		GTSignal.Emit(this.emitMode, this.signal, data);
	}

	// Token: 0x04002853 RID: 10323
	[Space]
	public GTSignalID signal;

	// Token: 0x04002854 RID: 10324
	public GTSignal.EmitMode emitMode;
}
