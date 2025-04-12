using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BD2 RID: 3026
	public abstract class BaseGuidedRefTargetMono : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x06004C4F RID: 19535 RVA: 0x00045E85 File Offset: 0x00044085
		protected virtual void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x000613D6 File Offset: 0x0005F5D6
		protected virtual void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<BaseGuidedRefTargetMono>(this, true);
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06004C51 RID: 19537 RVA: 0x000613DF File Offset: 0x0005F5DF
		// (set) Token: 0x06004C52 RID: 19538 RVA: 0x000613E7 File Offset: 0x0005F5E7
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

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06004C53 RID: 19539 RVA: 0x00037764 File Offset: 0x00035964
		UnityEngine.Object IGuidedRefTargetMono.GuidedRefTargetObject
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06004C54 RID: 19540 RVA: 0x000613F0 File Offset: 0x0005F5F0
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<BaseGuidedRefTargetMono>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x00037F83 File Offset: 0x00036183
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x00031B4B File Offset: 0x0002FD4B
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004E41 RID: 20033
		public GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
