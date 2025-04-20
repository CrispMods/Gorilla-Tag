using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000C0B RID: 3083
	public class GuidedRefTargetMonoTransform : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06004DD0 RID: 19920 RVA: 0x00062F0F File Offset: 0x0006110F
		// (set) Token: 0x06004DD1 RID: 19921 RVA: 0x00062F17 File Offset: 0x00061117
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

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06004DD2 RID: 19922 RVA: 0x00039243 File Offset: 0x00037443
		public UnityEngine.Object GuidedRefTargetObject
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x0004722A File Offset: 0x0004542A
		protected void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004DD4 RID: 19924 RVA: 0x00062F20 File Offset: 0x00061120
		protected void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<GuidedRefTargetMonoTransform>(this, true);
		}

		// Token: 0x06004DD5 RID: 19925 RVA: 0x00062F29 File Offset: 0x00061129
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<GuidedRefTargetMonoTransform>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004DD7 RID: 19927 RVA: 0x00039243 File Offset: 0x00037443
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004DD8 RID: 19928 RVA: 0x00032CAE File Offset: 0x00030EAE
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004F42 RID: 20290
		[SerializeField]
		private GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
