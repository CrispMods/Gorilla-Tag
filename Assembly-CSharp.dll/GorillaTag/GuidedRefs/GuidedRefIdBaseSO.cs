using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BD9 RID: 3033
	public abstract class GuidedRefIdBaseSO : ScriptableObject, IGuidedRefObject
	{
		// Token: 0x06004C76 RID: 19574 RVA: 0x0002F75F File Offset: 0x0002D95F
		public virtual void GuidedRefInitialize()
		{
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x00031B4B File Offset: 0x0002FD4B
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}
	}
}
