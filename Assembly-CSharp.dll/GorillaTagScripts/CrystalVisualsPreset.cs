using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009AD RID: 2477
	[CreateAssetMenu(fileName = "CrystalVisualsPreset", menuName = "ScriptableObjects/CrystalVisualsPreset", order = 0)]
	public class CrystalVisualsPreset : ScriptableObject
	{
		// Token: 0x06003D52 RID: 15698 RVA: 0x0015D8F8 File Offset: 0x0015BAF8
		public override int GetHashCode()
		{
			return new ValueTuple<CrystalVisualsPreset.VisualState, CrystalVisualsPreset.VisualState>(this.stateA, this.stateB).GetHashCode();
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x0002F75F File Offset: 0x0002D95F
		[Conditional("UNITY_EDITOR")]
		private void Save()
		{
		}

		// Token: 0x04003EBC RID: 16060
		public CrystalVisualsPreset.VisualState stateA;

		// Token: 0x04003EBD RID: 16061
		public CrystalVisualsPreset.VisualState stateB;

		// Token: 0x020009AE RID: 2478
		[Serializable]
		public struct VisualState
		{
			// Token: 0x06003D55 RID: 15701 RVA: 0x0015D924 File Offset: 0x0015BB24
			public override int GetHashCode()
			{
				int item = CrystalVisualsPreset.VisualState.<GetHashCode>g__GetColorHash|2_0(this.albedo);
				int item2 = CrystalVisualsPreset.VisualState.<GetHashCode>g__GetColorHash|2_0(this.emission);
				return new ValueTuple<int, int>(item, item2).GetHashCode();
			}

			// Token: 0x06003D56 RID: 15702 RVA: 0x0015D95C File Offset: 0x0015BB5C
			[CompilerGenerated]
			internal static int <GetHashCode>g__GetColorHash|2_0(Color c)
			{
				return new ValueTuple<float, float, float>(c.r, c.g, c.b).GetHashCode();
			}

			// Token: 0x04003EBE RID: 16062
			[ColorUsage(false, false)]
			public Color albedo;

			// Token: 0x04003EBF RID: 16063
			[ColorUsage(false, false)]
			public Color emission;
		}
	}
}
