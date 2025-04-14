using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BCF RID: 3023
	public abstract class BaseGuidedRefTargetMono : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x06004C43 RID: 19523 RVA: 0x000A6C70 File Offset: 0x000A4E70
		protected virtual void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x0017359E File Offset: 0x0017179E
		protected virtual void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<BaseGuidedRefTargetMono>(this, true);
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06004C45 RID: 19525 RVA: 0x001735A7 File Offset: 0x001717A7
		// (set) Token: 0x06004C46 RID: 19526 RVA: 0x001735AF File Offset: 0x001717AF
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

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06004C47 RID: 19527 RVA: 0x0003FF72 File Offset: 0x0003E172
		Object IGuidedRefTargetMono.GuidedRefTargetObject
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06004C48 RID: 19528 RVA: 0x001735B8 File Offset: 0x001717B8
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<BaseGuidedRefTargetMono>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004C4A RID: 19530 RVA: 0x00042E29 File Offset: 0x00041029
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x00015AA9 File Offset: 0x00013CA9
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004E2F RID: 20015
		public GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
