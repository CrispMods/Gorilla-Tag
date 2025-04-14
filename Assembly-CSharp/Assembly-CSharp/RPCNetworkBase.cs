using System;
using UnityEngine;

// Token: 0x020007F4 RID: 2036
internal abstract class RPCNetworkBase : MonoBehaviour
{
	// Token: 0x0600322F RID: 12847
	public abstract void SetClassTarget(IWrappedSerializable target, GorillaWrappedSerializer netHandler);
}
