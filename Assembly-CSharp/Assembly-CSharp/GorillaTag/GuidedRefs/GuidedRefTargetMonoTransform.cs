using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BE0 RID: 3040
	public class GuidedRefTargetMonoTransform : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06004C90 RID: 19600 RVA: 0x00174DC4 File Offset: 0x00172FC4
		// (set) Token: 0x06004C91 RID: 19601 RVA: 0x00174DCC File Offset: 0x00172FCC
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

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06004C92 RID: 19602 RVA: 0x0004316D File Offset: 0x0004136D
		public Object GuidedRefTargetObject
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x06004C93 RID: 19603 RVA: 0x000A70F0 File Offset: 0x000A52F0
		protected void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004C94 RID: 19604 RVA: 0x00174DD5 File Offset: 0x00172FD5
		protected void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<GuidedRefTargetMonoTransform>(this, true);
		}

		// Token: 0x06004C95 RID: 19605 RVA: 0x00174DDE File Offset: 0x00172FDE
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<GuidedRefTargetMonoTransform>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x0004316D File Offset: 0x0004136D
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x00015DCD File Offset: 0x00013FCD
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004E5E RID: 20062
		[SerializeField]
		private GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
