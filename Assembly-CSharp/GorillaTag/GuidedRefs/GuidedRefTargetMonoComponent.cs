using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BDB RID: 3035
	public class GuidedRefTargetMonoComponent : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06004C72 RID: 19570 RVA: 0x00174798 File Offset: 0x00172998
		// (set) Token: 0x06004C73 RID: 19571 RVA: 0x001747A0 File Offset: 0x001729A0
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

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06004C74 RID: 19572 RVA: 0x001747A9 File Offset: 0x001729A9
		public Object GuidedRefTargetObject
		{
			get
			{
				return this.targetComponent;
			}
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x000A6C70 File Offset: 0x000A4E70
		protected void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x001747B1 File Offset: 0x001729B1
		protected void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<GuidedRefTargetMonoComponent>(this, true);
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x001747BA File Offset: 0x001729BA
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<GuidedRefTargetMonoComponent>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x00042E29 File Offset: 0x00041029
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x00015AA9 File Offset: 0x00013CA9
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004E49 RID: 20041
		[SerializeField]
		private Component targetComponent;

		// Token: 0x04004E4A RID: 20042
		[SerializeField]
		private GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
