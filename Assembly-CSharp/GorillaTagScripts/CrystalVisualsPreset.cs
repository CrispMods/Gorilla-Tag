using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009AA RID: 2474
	[CreateAssetMenu(fileName = "CrystalVisualsPreset", menuName = "ScriptableObjects/CrystalVisualsPreset", order = 0)]
	public class CrystalVisualsPreset : ScriptableObject
	{
		// Token: 0x06003D46 RID: 15686 RVA: 0x001217B0 File Offset: 0x0011F9B0
		public override int GetHashCode()
		{
			return new ValueTuple<CrystalVisualsPreset.VisualState, CrystalVisualsPreset.VisualState>(this.stateA, this.stateB).GetHashCode();
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x000023F4 File Offset: 0x000005F4
		[Conditional("UNITY_EDITOR")]
		private void Save()
		{
		}

		// Token: 0x04003EAA RID: 16042
		public CrystalVisualsPreset.VisualState stateA;

		// Token: 0x04003EAB RID: 16043
		public CrystalVisualsPreset.VisualState stateB;

		// Token: 0x020009AB RID: 2475
		[Serializable]
		public struct VisualState
		{
			// Token: 0x06003D49 RID: 15689 RVA: 0x001217DC File Offset: 0x0011F9DC
			public override int GetHashCode()
			{
				int item = CrystalVisualsPreset.VisualState.<GetHashCode>g__GetColorHash|2_0(this.albedo);
				int item2 = CrystalVisualsPreset.VisualState.<GetHashCode>g__GetColorHash|2_0(this.emission);
				return new ValueTuple<int, int>(item, item2).GetHashCode();
			}

			// Token: 0x06003D4A RID: 15690 RVA: 0x00121814 File Offset: 0x0011FA14
			[CompilerGenerated]
			internal static int <GetHashCode>g__GetColorHash|2_0(Color c)
			{
				return new ValueTuple<float, float, float>(c.r, c.g, c.b).GetHashCode();
			}

			// Token: 0x04003EAC RID: 16044
			[ColorUsage(false, false)]
			public Color albedo;

			// Token: 0x04003EAD RID: 16045
			[ColorUsage(false, false)]
			public Color emission;
		}
	}
}
