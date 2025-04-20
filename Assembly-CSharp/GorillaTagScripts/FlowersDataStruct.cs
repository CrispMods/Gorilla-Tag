using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009DB RID: 2523
	[NetworkStructWeaved(13)]
	[StructLayout(LayoutKind.Explicit, Size = 52)]
	public struct FlowersDataStruct : INetworkStruct
	{
		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06003EB3 RID: 16051 RVA: 0x00058DCB File Offset: 0x00056FCB
		// (set) Token: 0x06003EB4 RID: 16052 RVA: 0x00058DD3 File Offset: 0x00056FD3
		public int FlowerCount { readonly get; set; }

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06003EB5 RID: 16053 RVA: 0x00058DDC File Offset: 0x00056FDC
		[Networked]
		public NetworkLinkedList<byte> FlowerWateredData
		{
			get
			{
				return new NetworkLinkedList<byte>(Native.ReferenceToPointer<FixedStorage@6>(ref this._FlowerWateredData), 1, ReaderWriter@System_Byte.GetInstance());
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06003EB6 RID: 16054 RVA: 0x00058DF4 File Offset: 0x00056FF4
		[Networked]
		public NetworkLinkedList<int> FlowerStateData
		{
			get
			{
				return new NetworkLinkedList<int>(Native.ReferenceToPointer<FixedStorage@6>(ref this._FlowerStateData), 1, ReaderWriter@System_Int32.GetInstance());
			}
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x00164D84 File Offset: 0x00162F84
		public FlowersDataStruct(List<Flower> allFlowers)
		{
			this.FlowerCount = allFlowers.Count;
			foreach (Flower flower in allFlowers)
			{
				this.FlowerWateredData.Add(flower.IsWatered ? 1 : 0);
				this.FlowerStateData.Add((int)flower.GetCurrentState());
			}
		}

		// Token: 0x04003FD0 RID: 16336
		[FixedBufferProperty(typeof(NetworkLinkedList<byte>), typeof(UnityLinkedListSurrogate@ReaderWriter@System_Byte), 1, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(4)]
		private FixedStorage@6 _FlowerWateredData;

		// Token: 0x04003FD1 RID: 16337
		[FixedBufferProperty(typeof(NetworkLinkedList<int>), typeof(UnityLinkedListSurrogate@ReaderWriter@System_Int32), 1, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(28)]
		private FixedStorage@6 _FlowerStateData;
	}
}
