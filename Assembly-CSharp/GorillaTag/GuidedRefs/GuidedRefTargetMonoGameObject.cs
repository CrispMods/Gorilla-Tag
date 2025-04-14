using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BDC RID: 3036
	public class GuidedRefTargetMonoGameObject : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06004C7B RID: 19579 RVA: 0x001747CE File Offset: 0x001729CE
		// (set) Token: 0x06004C7C RID: 19580 RVA: 0x001747D6 File Offset: 0x001729D6
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

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06004C7D RID: 19581 RVA: 0x00012273 File Offset: 0x00010473
		public Object GuidedRefTargetObject
		{
			get
			{
				return base.gameObject;
			}
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x000A6C70 File Offset: 0x000A4E70
		protected void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x001747DF File Offset: 0x001729DF
		protected void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<GuidedRefTargetMonoGameObject>(this, true);
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x001747E8 File Offset: 0x001729E8
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<GuidedRefTargetMonoGameObject>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x00042E29 File Offset: 0x00041029
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x00015AA9 File Offset: 0x00013CA9
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004E4B RID: 20043
		[SerializeField]
		private GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
