using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BFD RID: 3069
	public abstract class BaseGuidedRefTargetMono : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x06004D8F RID: 19855 RVA: 0x0004722A File Offset: 0x0004542A
		protected virtual void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x00062D97 File Offset: 0x00060F97
		protected virtual void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<BaseGuidedRefTargetMono>(this, true);
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06004D91 RID: 19857 RVA: 0x00062DA0 File Offset: 0x00060FA0
		// (set) Token: 0x06004D92 RID: 19858 RVA: 0x00062DA8 File Offset: 0x00060FA8
		GuidedRefBasicTargetInfo IGuidedRefTargetMono.GRefTargetInfo
		{
			get
			{
				return this.guidedRefTargetInfo;
			}
			set
			{
				this.guidedRefTargetInfo = value;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06004D93 RID: 19859 RVA: 0x00038A24 File Offset: 0x00036C24
		UnityEngine.Object IGuidedRefTargetMono.GuidedRefTargetObject
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x00062DB1 File Offset: 0x00060FB1
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<BaseGuidedRefTargetMono>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x00039243 File Offset: 0x00037443
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x00032CAE File Offset: 0x00030EAE
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004F25 RID: 20261
		public GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
