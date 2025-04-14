using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009B5 RID: 2485
	[NetworkStructWeaved(13)]
	[StructLayout(LayoutKind.Explicit, Size = 52)]
	public struct FlowersDataStruct : INetworkStruct
	{
		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06003D9B RID: 15771 RVA: 0x001230CB File Offset: 0x001212CB
		// (set) Token: 0x06003D9C RID: 15772 RVA: 0x001230D3 File Offset: 0x001212D3
		public int FlowerCount { readonly get; set; }

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06003D9D RID: 15773 RVA: 0x001230DC File Offset: 0x001212DC
		[Networked]
		public NetworkLinkedList<byte> FlowerWateredData
		{
			get
			{
				return new NetworkLinkedList<byte>(Native.ReferenceToPointer<FixedStorage@6>(ref this._FlowerWateredData), 1, ReaderWriter@System_Byte.GetInstance());
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06003D9E RID: 15774 RVA: 0x00123100 File Offset: 0x00121300
		[Networked]
		public NetworkLinkedList<int> FlowerStateData
		{
			get
			{
				return new NetworkLinkedList<int>(Native.ReferenceToPointer<FixedStorage@6>(ref this._FlowerStateData), 1, ReaderWriter@System_Int32.GetInstance());
			}
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x00123124 File Offset: 0x00121324
		public FlowersDataStruct(List<Flower> allFlowers)
		{
			this.FlowerCount = allFlowers.Count;
			foreach (Flower flower in allFlowers)
			{
				this.FlowerWateredData.Add(flower.IsWatered ? 1 : 0);
				this.FlowerStateData.Add((int)flower.GetCurrentState());
			}
		}

		// Token: 0x04003EF6 RID: 16118
		[FixedBufferProperty(typeof(NetworkLinkedList<byte>), typeof(UnityLinkedListSurrogate@ReaderWriter@System_Byte), 1, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(4)]
		private FixedStorage@6 _FlowerWateredData;

		// Token: 0x04003EF7 RID: 16119
		[FixedBufferProperty(typeof(NetworkLinkedList<int>), typeof(UnityLinkedListSurrogate@ReaderWriter@System_Int32), 1, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(28)]
		private FixedStorage@6 _FlowerStateData;
	}
}
