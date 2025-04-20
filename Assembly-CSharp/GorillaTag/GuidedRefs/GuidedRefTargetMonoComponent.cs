using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000C09 RID: 3081
	public class GuidedRefTargetMonoComponent : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06004DBE RID: 19902 RVA: 0x00062EAB File Offset: 0x000610AB
		// (set) Token: 0x06004DBF RID: 19903 RVA: 0x00062EB3 File Offset: 0x000610B3
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

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06004DC0 RID: 19904 RVA: 0x00062EBC File Offset: 0x000610BC
		public UnityEngine.Object GuidedRefTargetObject
		{
			get
			{
				return this.targetComponent;
			}
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x0004722A File Offset: 0x0004542A
		protected void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x00062EC4 File Offset: 0x000610C4
		protected void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<GuidedRefTargetMonoComponent>(this, true);
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x00062ECD File Offset: 0x000610CD
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<GuidedRefTargetMonoComponent>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x00039243 File Offset: 0x00037443
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x00032CAE File Offset: 0x00030EAE
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004F3F RID: 20287
		[SerializeField]
		private Component targetComponent;

		// Token: 0x04004F40 RID: 20288
		[SerializeField]
		private GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
