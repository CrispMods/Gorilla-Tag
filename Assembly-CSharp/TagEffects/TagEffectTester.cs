using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B64 RID: 2916
	public class TagEffectTester : MonoBehaviour, IHandEffectsTrigger
	{
		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x060048C2 RID: 18626 RVA: 0x0005F568 File Offset: 0x0005D768
		public bool Static
		{
			get
			{
				return this.isStatic;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x060048C3 RID: 18627 RVA: 0x0005F570 File Offset: 0x0005D770
		public IHandEffectsTrigger.Mode EffectMode { get; }

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x060048C4 RID: 18628 RVA: 0x0005F578 File Offset: 0x0005D778
		public Transform Transform { get; }

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x060048C5 RID: 18629 RVA: 0x0003924B File Offset: 0x0003744B
		public VRRig Rig
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x060048C6 RID: 18630 RVA: 0x0005F580 File Offset: 0x0005D780
		public bool FingersDown { get; }

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x060048C7 RID: 18631 RVA: 0x0005F588 File Offset: 0x0005D788
		public bool FingersUp { get; }

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x060048C8 RID: 18632 RVA: 0x0005F590 File Offset: 0x0005D790
		public Vector3 Velocity { get; }

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x060048C9 RID: 18633 RVA: 0x0005F598 File Offset: 0x0005D798
		public bool RightHand { get; }

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x060048CA RID: 18634 RVA: 0x0005F5A0 File Offset: 0x0005D7A0
		public float Magnitude { get; }

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x060048CB RID: 18635 RVA: 0x0005F5A8 File Offset: 0x0005D7A8
		public TagEffectPack CosmeticEffectPack { get; }

		// Token: 0x060048CC RID: 18636 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnTriggerEntered(IHandEffectsTrigger other)
		{
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x00030498 File Offset: 0x0002E698
		public bool InTriggerZone(IHandEffectsTrigger t)
		{
			return false;
		}

		// Token: 0x04004A09 RID: 18953
		[SerializeField]
		private bool isStatic = true;
	}
}
