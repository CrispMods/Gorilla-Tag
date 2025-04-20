using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000C0A RID: 3082
	public class GuidedRefTargetMonoGameObject : MonoBehaviour, IGuidedRefTargetMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06004DC7 RID: 19911 RVA: 0x00062EE1 File Offset: 0x000610E1
		// (set) Token: 0x06004DC8 RID: 19912 RVA: 0x00062EE9 File Offset: 0x000610E9
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

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06004DC9 RID: 19913 RVA: 0x00032616 File Offset: 0x00030816
		public UnityEngine.Object GuidedRefTargetObject
		{
			get
			{
				return base.gameObject;
			}
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x0004722A File Offset: 0x0004542A
		protected void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x00062EF2 File Offset: 0x000610F2
		protected void OnDestroy()
		{
			GuidedRefHub.UnregisterTarget<GuidedRefTargetMonoGameObject>(this, true);
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x00062EFB File Offset: 0x000610FB
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterTarget<GuidedRefTargetMonoGameObject>(this, this.guidedRefTargetInfo.hubIds, this);
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x00039243 File Offset: 0x00037443
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x00032CAE File Offset: 0x00030EAE
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004F41 RID: 20289
		[SerializeField]
		private GuidedRefBasicTargetInfo guidedRefTargetInfo;
	}
}
