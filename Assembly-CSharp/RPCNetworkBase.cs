using System;
using UnityEngine;

// Token: 0x0200080B RID: 2059
internal abstract class RPCNetworkBase : MonoBehaviour
{
	// Token: 0x060032D9 RID: 13017
	public abstract void SetClassTarget(IWrappedSerializable target, GorillaWrappedSerializer netHandler);
}
