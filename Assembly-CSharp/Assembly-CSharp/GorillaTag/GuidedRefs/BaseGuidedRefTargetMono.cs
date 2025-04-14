using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BD2 RID: 3026
	public abstract class BaseGuidedRefTargetMono : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x06004C4F RID: 19535 RVA: 0x000A70F0 File Offset: 0x000A52F0
		protected virtual void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x00173B66 File Offset: 0x00171D66
		protected virtual void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<BaseGuidedRefTargetMono>(this, true);
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06004C51 RID: 19537 RVA: 0x00173B6F File Offset: 0x00171D6F
		// (set) Token: 0x06004C52 RID: 19538 RVA: 0x00173B77 File Offset: 0x00171D77
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
		// (get) Token: 0x06004C53 RID: 19539 RVA: 0x000402B6 File Offset: 0x0003E4B6
		Object IGuidedRefTargetMono.GuidedRefTargetObject
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06004C54 RID: 19540 RVA: 0x00173B80 File Offset: 0x00171D80
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<BaseGuidedRefTargetMono>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x0004316D File Offset: 0x0004136D
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x00015DCD File Offset: 0x00013FCD
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004E41 RID: 20033
		public GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
