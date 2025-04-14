using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BD9 RID: 3033
	public abstract class GuidedRefIdBaseSO : ScriptableObject, IGuidedRefObject
	{
		// Token: 0x06004C76 RID: 19574 RVA: 0x000023F4 File Offset: 0x000005F4
		public virtual void GuidedRefInitialize()
		{
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x00015DCD File Offset: 0x00013FCD
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}
	}
}
