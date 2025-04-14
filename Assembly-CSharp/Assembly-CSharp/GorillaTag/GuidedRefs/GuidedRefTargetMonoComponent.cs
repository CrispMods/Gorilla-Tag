using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BDE RID: 3038
	public class GuidedRefTargetMonoComponent : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06004C7E RID: 19582 RVA: 0x00174D60 File Offset: 0x00172F60
		// (set) Token: 0x06004C7F RID: 19583 RVA: 0x00174D68 File Offset: 0x00172F68
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

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06004C80 RID: 19584 RVA: 0x00174D71 File Offset: 0x00172F71
		public Object GuidedRefTargetObject
		{
			get
			{
				return this.targetComponent;
			}
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x000A70F0 File Offset: 0x000A52F0
		protected void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x00174D79 File Offset: 0x00172F79
		protected void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<GuidedRefTargetMonoComponent>(this, true);
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x00174D82 File Offset: 0x00172F82
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<GuidedRefTargetMonoComponent>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x0004316D File Offset: 0x0004136D
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x00015DCD File Offset: 0x00013FCD
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004E5B RID: 20059
		[SerializeField]
		private Component targetComponent;

		// Token: 0x04004E5C RID: 20060
		[SerializeField]
		private GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
