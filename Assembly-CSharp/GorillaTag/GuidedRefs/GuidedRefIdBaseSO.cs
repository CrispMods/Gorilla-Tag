using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BD6 RID: 3030
	public abstract class GuidedRefIdBaseSO : ScriptableObject, IGuidedRefObject
	{
		// Token: 0x06004C6A RID: 19562 RVA: 0x000023F4 File Offset: 0x000005F4
		public virtual void GuidedRefInitialize()
		{
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x00015AA9 File Offset: 0x00013CA9
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}
	}
}
