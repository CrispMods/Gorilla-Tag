using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009DE RID: 2526
	[NetworkStructWeaved(9)]
	[StructLayout(LayoutKind.Explicit, Size = 36)]
	public struct ObstacleCourseData : INetworkStruct
	{
		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06003EFF RID: 16127 RVA: 0x000585A1 File Offset: 0x000567A1
		// (set) Token: 0x06003F00 RID: 16128 RVA: 0x000585A9 File Offset: 0x000567A9
		public int ObstacleCourseCount { readonly get; set; }

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06003F01 RID: 16129 RVA: 0x000585B2 File Offset: 0x000567B2
		[Networked]
		[Capacity(4)]
		public NetworkArray<int> WinnerActorNumber
		{
			get
			{
				return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@4>(ref this._WinnerActorNumber), 4, ReaderWriter@System_Int32.GetInstance());
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06003F02 RID: 16130 RVA: 0x000585CA File Offset: 0x000567CA
		[Networked]
		[Capacity(4)]
		public NetworkArray<int> CurrentRaceState
		{
			get
			{
				return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@4>(ref this._CurrentRaceState), 4, ReaderWriter@System_Int32.GetInstance());
			}
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x00165324 File Offset: 0x00163524
		public ObstacleCourseData(List<ObstacleCourse> courses)
		{
			this.ObstacleCourseCount = courses.Count;
			int[] array = new int[this.ObstacleCourseCount];
			int[] array2 = new int[this.ObstacleCourseCount];
			for (int i = 0; i < courses.Count; i++)
			{
				array[i] = courses[i].winnerActorNumber;
				array2[i] = (int)courses[i].currentState;
			}
			this.WinnerActorNumber.CopyFrom(array, 0, this.ObstacleCourseCount);
			this.CurrentRaceState.CopyFrom(array2, 0, this.ObstacleCourseCount);
		}

		// Token: 0x04004043 RID: 16451
		[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 4, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(4)]
		private FixedStorage@4 _WinnerActorNumber;

		// Token: 0x04004044 RID: 16452
		[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 4, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(20)]
		private FixedStorage@4 _CurrentRaceState;
	}
}
