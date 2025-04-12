using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BDF RID: 3039
	public class GuidedRefTargetMonoGameObject : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06004C87 RID: 19591 RVA: 0x00061520 File Offset: 0x0005F720
		// (set) Token: 0x06004C88 RID: 19592 RVA: 0x00061528 File Offset: 0x0005F728
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
		// (get) Token: 0x06004C89 RID: 19593 RVA: 0x000314E3 File Offset: 0x0002F6E3
		public UnityEngine.Object GuidedRefTargetObject
		{
			get
			{
				return base.gameObject;
			}
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x00045E85 File Offset: 0x00044085
		protected void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x00061531 File Offset: 0x0005F731
		protected void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<GuidedRefTargetMonoGameObject>(this, true);
		}

		// Token: 0x06004C8C RID: 19596 RVA: 0x0006153A File Offset: 0x0005F73A
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<GuidedRefTargetMonoGameObject>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004C8E RID: 19598 RVA: 0x00037F83 File Offset: 0x00036183
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004C8F RID: 19599 RVA: 0x00031B4B File Offset: 0x0002FD4B
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004E5D RID: 20061
		[SerializeField]
		private GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
