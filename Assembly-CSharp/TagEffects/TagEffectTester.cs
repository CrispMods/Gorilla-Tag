using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B37 RID: 2871
	public class TagEffectTester : MonoBehaviour, IHandEffectsTrigger
	{
		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06004779 RID: 18297 RVA: 0x001540DF File Offset: 0x001522DF
		public bool Static
		{
			get
			{
				return this.isStatic;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x0600477A RID: 18298 RVA: 0x001540E7 File Offset: 0x001522E7
		public IHandEffectsTrigger.Mode EffectMode { get; }

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x0600477B RID: 18299 RVA: 0x001540EF File Offset: 0x001522EF
		public Transform Transform { get; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x0600477C RID: 18300 RVA: 0x00042E31 File Offset: 0x00041031
		public VRRig Rig
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x0600477D RID: 18301 RVA: 0x001540F7 File Offset: 0x001522F7
		public bool FingersDown { get; }

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600477E RID: 18302 RVA: 0x001540FF File Offset: 0x001522FF
		public bool FingersUp { get; }

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x0600477F RID: 18303 RVA: 0x00154107 File Offset: 0x00152307
		public Vector3 Velocity { get; }

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06004780 RID: 18304 RVA: 0x0015410F File Offset: 0x0015230F
		public bool RightHand { get; }

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06004781 RID: 18305 RVA: 0x00154117 File Offset: 0x00152317
		public float Magnitude { get; }

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06004782 RID: 18306 RVA: 0x0015411F File Offset: 0x0015231F
		public TagEffectPack CosmeticEffectPack { get; }

		// Token: 0x06004783 RID: 18307 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnTriggerEntered(IHandEffectsTrigger other)
		{
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x00002076 File Offset: 0x00000276
		public bool InTriggerZone(IHandEffectsTrigger t)
		{
			return false;
		}

		// Token: 0x04004914 RID: 18708
		[SerializeField]
		private bool isStatic = true;
	}
}
