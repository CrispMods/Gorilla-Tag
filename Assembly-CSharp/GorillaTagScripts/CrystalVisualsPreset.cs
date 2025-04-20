using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009D0 RID: 2512
	[CreateAssetMenu(fileName = "CrystalVisualsPreset", menuName = "ScriptableObjects/CrystalVisualsPreset", order = 0)]
	public class CrystalVisualsPreset : ScriptableObject
	{
		// Token: 0x06003E5E RID: 15966 RVA: 0x001638E0 File Offset: 0x00161AE0
		public override int GetHashCode()
		{
			return new ValueTuple<CrystalVisualsPreset.VisualState, CrystalVisualsPreset.VisualState>(this.stateA, this.stateB).GetHashCode();
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x00030607 File Offset: 0x0002E807
		[Conditional("UNITY_EDITOR")]
		private void Save()
		{
		}

		// Token: 0x04003F84 RID: 16260
		public CrystalVisualsPreset.VisualState stateA;

		// Token: 0x04003F85 RID: 16261
		public CrystalVisualsPreset.VisualState stateB;

		// Token: 0x020009D1 RID: 2513
		[Serializable]
		public struct VisualState
		{
			// Token: 0x06003E61 RID: 15969 RVA: 0x0016390C File Offset: 0x00161B0C
			public override int GetHashCode()
			{
				int item = CrystalVisualsPreset.VisualState.<GetHashCode>g__GetColorHash|2_0(this.albedo);
				int item2 = CrystalVisualsPreset.VisualState.<GetHashCode>g__GetColorHash|2_0(this.emission);
				return new ValueTuple<int, int>(item, item2).GetHashCode();
			}

			// Token: 0x06003E62 RID: 15970 RVA: 0x00163944 File Offset: 0x00161B44
			[CompilerGenerated]
			internal static int <GetHashCode>g__GetColorHash|2_0(Color c)
			{
				return new ValueTuple<float, float, float>(c.r, c.g, c.b).GetHashCode();
			}

			// Token: 0x04003F86 RID: 16262
			[ColorUsage(false, false)]
			public Color albedo;

			// Token: 0x04003F87 RID: 16263
			[ColorUsage(false, false)]
			public Color emission;
		}
	}
}
