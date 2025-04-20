using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D52 RID: 3410
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@GorillaPaintbrawlManager__PaintbrawlStatus : IElementReaderWriter<GorillaPaintbrawlManager.PaintbrawlStatus>
	{
		// Token: 0x06005566 RID: 21862 RVA: 0x00067605 File Offset: 0x00065805
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe GorillaPaintbrawlManager.PaintbrawlStatus Read(byte* data, int index)
		{
			return *(GorillaPaintbrawlManager.PaintbrawlStatus*)(data + index * 4);
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x000673A8 File Offset: 0x000655A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref GorillaPaintbrawlManager.PaintbrawlStatus ReadRef(byte* data, int index)
		{
			return ref *(GorillaPaintbrawlManager.PaintbrawlStatus*)(data + index * 4);
		}

		// Token: 0x06005568 RID: 21864 RVA: 0x00067615 File Offset: 0x00065815
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, GorillaPaintbrawlManager.PaintbrawlStatus val)
		{
			*(GorillaPaintbrawlManager.PaintbrawlStatus*)(data + index * 4) = val;
		}

		// Token: 0x06005569 RID: 21865 RVA: 0x00039846 File Offset: 0x00037A46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x0600556A RID: 21866 RVA: 0x001D142C File Offset: 0x001CF62C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(GorillaPaintbrawlManager.PaintbrawlStatus val)
		{
			return val.GetHashCode();
		}

		// Token: 0x0600556B RID: 21867 RVA: 0x001D1448 File Offset: 0x001CF648
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

		// Token: 0x04005821 RID: 22561
		[WeaverGenerated]
		public static IElementReaderWriter<GorillaPaintbrawlManager.PaintbrawlStatus> Instance;
	}
}
