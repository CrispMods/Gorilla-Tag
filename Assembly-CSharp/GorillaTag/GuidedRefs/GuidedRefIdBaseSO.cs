using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000C04 RID: 3076
	public abstract class GuidedRefIdBaseSO : ScriptableObject, IGuidedRefObject
	{
		// Token: 0x06004DB6 RID: 19894 RVA: 0x00030607 File Offset: 0x0002E807
		public virtual void GuidedRefInitialize()
		{
		}

		// Token: 0x06004DB8 RID: 19896 RVA: 0x00032CAE File Offset: 0x00030EAE
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}
	}
}
