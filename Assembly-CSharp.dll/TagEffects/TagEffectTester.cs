using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B3A RID: 2874
	public class TagEffectTester : MonoBehaviour, IHandEffectsTrigger
	{
		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06004785 RID: 18309 RVA: 0x0005DB51 File Offset: 0x0005BD51
		public bool Static
		{
			get
			{
				return this.isStatic;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06004786 RID: 18310 RVA: 0x0005DB59 File Offset: 0x0005BD59
		public IHandEffectsTrigger.Mode EffectMode { get; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06004787 RID: 18311 RVA: 0x0005DB61 File Offset: 0x0005BD61
		public Transform Transform { get; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06004788 RID: 18312 RVA: 0x00037F8B File Offset: 0x0003618B
		public VRRig Rig
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06004789 RID: 18313 RVA: 0x0005DB69 File Offset: 0x0005BD69
		public bool FingersDown { get; }

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x0600478A RID: 18314 RVA: 0x0005DB71 File Offset: 0x0005BD71
		public bool FingersUp { get; }

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x0600478B RID: 18315 RVA: 0x0005DB79 File Offset: 0x0005BD79
		public Vector3 Velocity { get; }

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x0600478C RID: 18316 RVA: 0x0005DB81 File Offset: 0x0005BD81
		public bool RightHand { get; }

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600478D RID: 18317 RVA: 0x0005DB89 File Offset: 0x0005BD89
		public float Magnitude { get; }

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600478E RID: 18318 RVA: 0x0005DB91 File Offset: 0x0005BD91
		public TagEffectPack CosmeticEffectPack { get; }

		// Token: 0x0600478F RID: 18319 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnTriggerEntered(IHandEffectsTrigger other)
		{
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
		public bool InTriggerZone(IHandEffectsTrigger t)
		{
			return false;
		}

		// Token: 0x04004926 RID: 18726
		[SerializeField]
		private bool isStatic = true;
	}
}
