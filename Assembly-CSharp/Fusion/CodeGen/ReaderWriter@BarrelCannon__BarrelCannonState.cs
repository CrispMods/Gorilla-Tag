using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D04 RID: 3332
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@BarrelCannon__BarrelCannonState : IElementReaderWriter<BarrelCannon.BarrelCannonState>
	{
		// Token: 0x060053B3 RID: 21427 RVA: 0x0019C1BC File Offset: 0x0019A3BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe BarrelCannon.BarrelCannonState Read(byte* data, int index)
		{
			return *(BarrelCannon.BarrelCannonState*)(data + index * 4);
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x0019C1CC File Offset: 0x0019A3CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref BarrelCannon.BarrelCannonState ReadRef(byte* data, int index)
		{
			return ref *(BarrelCannon.BarrelCannonState*)(data + index * 4);
		}

		// Token: 0x060053B5 RID: 21429 RVA: 0x0019C1D7 File Offset: 0x0019A3D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, BarrelCannon.BarrelCannonState val)
		{
			*(BarrelCannon.BarrelCannonState*)(data + index * 4) = val;
		}

		// Token: 0x060053B6 RID: 21430 RVA: 0x000444E2 File Offset: 0x000426E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x0019C1E8 File Offset: 0x0019A3E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(BarrelCannon.BarrelCannonState val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x0019C204 File Offset: 0x0019A404
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[WeaverGenerated]
		public static IElementReaderWriter<BarrelCannon.BarrelCannonState> GetInstance()
		{
			if (ReaderWriter@BarrelCannon__BarrelCannonState.Instance == null)
			{
				ReaderWriter@BarrelCannon__BarrelCannonState.Instance = default(ReaderWriter@BarrelCannon__BarrelCannonState);
			}
			return ReaderWriter@BarrelCannon__BarrelCannonState.Instance;
		}

		// Token: 0x040055FC RID: 22012
		[WeaverGenerated]
		public static IElementReaderWriter<BarrelCannon.BarrelCannonState> Instance;
	}
}
