using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BDF RID: 3039
	public class GuidedRefTargetMonoGameObject : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06004C87 RID: 19591 RVA: 0x00174D96 File Offset: 0x00172F96
		// (set) Token: 0x06004C88 RID: 19592 RVA: 0x00174D9E File Offset: 0x00172F9E
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

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06004C89 RID: 19593 RVA: 0x00012597 File Offset: 0x00010797
		public Object GuidedRefTargetObject
		{
			get
			{
				return base.gameObject;
			}
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x000A70F0 File Offset: 0x000A52F0
		protected void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x00174DA7 File Offset: 0x00172FA7
		protected void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<GuidedRefTargetMonoGameObject>(this, true);
		}

		// Token: 0x06004C8C RID: 19596 RVA: 0x00174DB0 File Offset: 0x00172FB0
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<GuidedRefTargetMonoGameObject>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004C8E RID: 19598 RVA: 0x0004316D File Offset: 0x0004136D
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004C8F RID: 19599 RVA: 0x00015DCD File Offset: 0x00013FCD
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004E5D RID: 20061
		[SerializeField]
		private GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
