using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BDE RID: 3038
	public class GuidedRefTargetMonoComponent : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06004C7E RID: 19582 RVA: 0x000614EA File Offset: 0x0005F6EA
		// (set) Token: 0x06004C7F RID: 19583 RVA: 0x000614F2 File Offset: 0x0005F6F2
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
		// (get) Token: 0x06004C80 RID: 19584 RVA: 0x000614FB File Offset: 0x0005F6FB
		public UnityEngine.Object GuidedRefTargetObject
		{
			get
			{
				return this.targetComponent;
			}
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x00045E85 File Offset: 0x00044085
		protected void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x00061503 File Offset: 0x0005F703
		protected void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<GuidedRefTargetMonoComponent>(this, true);
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x0006150C File Offset: 0x0005F70C
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<GuidedRefTargetMonoComponent>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x00037F83 File Offset: 0x00036183
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x00031B4B File Offset: 0x0002FD4B
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
