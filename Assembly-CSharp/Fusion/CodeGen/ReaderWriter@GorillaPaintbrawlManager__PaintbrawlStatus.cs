using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D21 RID: 3361
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@GorillaPaintbrawlManager__PaintbrawlStatus : IElementReaderWriter<GorillaPaintbrawlManager.PaintbrawlStatus>
	{
		// Token: 0x06005404 RID: 21508 RVA: 0x0019C660 File Offset: 0x0019A860
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe GorillaPaintbrawlManager.PaintbrawlStatus Read(byte* data, int index)
		{
			return *(GorillaPaintbrawlManager.PaintbrawlStatus*)(data + index * 4);
		}

		// Token: 0x06005405 RID: 21509 RVA: 0x0019C1CC File Offset: 0x0019A3CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref GorillaPaintbrawlManager.PaintbrawlStatus ReadRef(byte* data, int index)
		{
			return ref *(GorillaPaintbrawlManager.PaintbrawlStatus*)(data + index * 4);
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x0019C670 File Offset: 0x0019A870
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, GorillaPaintbrawlManager.PaintbrawlStatus val)
		{
			*(GorillaPaintbrawlManager.PaintbrawlStatus*)(data + index * 4) = val;
		}

		// Token: 0x06005407 RID: 21511 RVA: 0x000444E2 File Offset: 0x000426E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x06005408 RID: 21512 RVA: 0x0019C684 File Offset: 0x0019A884
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(GorillaPaintbrawlManager.PaintbrawlStatus val)
		{
			return val.GetHashCode();
		}

		// Token: 0x06005409 RID: 21513 RVA: 0x0019C6A0 File Offset: 0x0019A8A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[WeaverGenerated]
		public static IElementReaderWriter<GorillaPaintbrawlManager.PaintbrawlStatus> GetInstance()
		{
			if (ReaderWriter@GorillaPaintbrawlManager__PaintbrawlStatus.Instance == null)
			{
				ReaderWriter@GorillaPaintbrawlManager__PaintbrawlStatus.Instance = default(ReaderWriter@GorillaPaintbrawlManager__PaintbrawlStatus);
			}
			return ReaderWriter@GorillaPaintbrawlManager__PaintbrawlStatus.Instance;
		}

		// Token: 0x04005715 RID: 22293
		[WeaverGenerated]
		public static IElementReaderWriter<GorillaPaintbrawlManager.PaintbrawlStatus> Instance;
	}
}
