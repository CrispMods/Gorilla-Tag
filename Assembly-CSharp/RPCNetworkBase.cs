using System;
using UnityEngine;

// Token: 0x020007F1 RID: 2033
internal abstract class RPCNetworkBase : MonoBehaviour
{
	// Token: 0x06003223 RID: 12835
	public abstract void SetClassTarget(IWrappedSerializable target, GorillaWrappedSerializer netHandler);
}
