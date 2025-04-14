using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B3A RID: 2874
	public class TagEffectTester : MonoBehaviour, IHandEffectsTrigger
	{
		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06004785 RID: 18309 RVA: 0x001546A7 File Offset: 0x001528A7
		public bool Static
		{
			get
			{
				return this.isStatic;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06004786 RID: 18310 RVA: 0x001546AF File Offset: 0x001528AF
		public IHandEffectsTrigger.Mode EffectMode { get; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06004787 RID: 18311 RVA: 0x001546B7 File Offset: 0x001528B7
		public Transform Transform { get; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06004788 RID: 18312 RVA: 0x00043175 File Offset: 0x00041375
		public VRRig Rig
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06004789 RID: 18313 RVA: 0x001546BF File Offset: 0x001528BF
		public bool FingersDown { get; }

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x0600478A RID: 18314 RVA: 0x001546C7 File Offset: 0x001528C7
		public bool FingersUp { get; }

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x0600478B RID: 18315 RVA: 0x001546CF File Offset: 0x001528CF
		public Vector3 Velocity { get; }

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x0600478C RID: 18316 RVA: 0x001546D7 File Offset: 0x001528D7
		public bool RightHand { get; }

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600478D RID: 18317 RVA: 0x001546DF File Offset: 0x001528DF
		public float Magnitude { get; }

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600478E RID: 18318 RVA: 0x001546E7 File Offset: 0x001528E7
		public TagEffectPack CosmeticEffectPack { get; }

		// Token: 0x0600478F RID: 18319 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnTriggerEntered(IHandEffectsTrigger other)
		{
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x00002076 File Offset: 0x00000276
		public bool InTriggerZone(IHandEffectsTrigger t)
		{
			return false;
		}

		// Token: 0x04004926 RID: 18726
		[SerializeField]
		private bool isStatic = true;
	}
}
