using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BDD RID: 3037
	public class GuidedRefTargetMonoTransform : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06004C84 RID: 19588 RVA: 0x001747FC File Offset: 0x001729FC
		// (set) Token: 0x06004C85 RID: 19589 RVA: 0x00174804 File Offset: 0x00172A04
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

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06004C86 RID: 19590 RVA: 0x00042E29 File Offset: 0x00041029
		public Object GuidedRefTargetObject
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x06004C87 RID: 19591 RVA: 0x000A6C70 File Offset: 0x000A4E70
		protected void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004C88 RID: 19592 RVA: 0x0017480D File Offset: 0x00172A0D
		protected void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<GuidedRefTargetMonoTransform>(this, true);
		}

		// Token: 0x06004C89 RID: 19593 RVA: 0x00174816 File Offset: 0x00172A16
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<GuidedRefTargetMonoTransform>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x00042E29 File Offset: 0x00041029
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004C8C RID: 19596 RVA: 0x00015AA9 File Offset: 0x00013CA9
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004E4C RID: 20044
		[SerializeField]
		private GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
