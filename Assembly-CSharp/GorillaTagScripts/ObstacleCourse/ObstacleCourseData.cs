using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x02000A17 RID: 2583
	[NetworkStructWeaved(9)]
	[StructLayout(LayoutKind.Explicit, Size = 36)]
	public struct ObstacleCourseData : INetworkStruct
	{
		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060040B8 RID: 16568 RVA: 0x0005A57B File Offset: 0x0005877B
		// (set) Token: 0x060040B9 RID: 16569 RVA: 0x0005A583 File Offset: 0x00058783
		public int ObstacleCourseCount { readonly get; set; }

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060040BA RID: 16570 RVA: 0x0005A58C File Offset: 0x0005878C
		[Networked]
		[Capacity(4)]
		public NetworkArray<int> WinnerActorNumber
		{
			get
			{
				return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@4>(ref this._WinnerActorNumber), 4, ReaderWriter@System_Int32.GetInstance());
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x060040BB RID: 16571 RVA: 0x0005A5A4 File Offset: 0x000587A4
		[Networked]
		[Capacity(4)]
		public NetworkArray<int> CurrentRaceState
		{
			get
			{
				return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@4>(ref this._CurrentRaceState), 4, ReaderWriter@System_Int32.GetInstance());
			}
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x0016E6A4 File Offset: 0x0016C8A4
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

		// Token: 0x04004189 RID: 16777
		[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 4, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(4)]
		private FixedStorage@4 _WinnerActorNumber;

		// Token: 0x0400418A RID: 16778
		[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 4, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(20)]
		private FixedStorage@4 _CurrentRaceState;
	}
}
