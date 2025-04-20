using System;

namespace BoingKit
{
	// Token: 0x02000CE8 RID: 3304
	public struct Version : IEquatable<Version>
	{
		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x0600537A RID: 21370 RVA: 0x000661F8 File Offset: 0x000643F8
		public readonly int MajorVersion { get; }

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x0600537B RID: 21371 RVA: 0x00066200 File Offset: 0x00064400
		public readonly int MinorVersion { get; }

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x0600537C RID: 21372 RVA: 0x00066208 File Offset: 0x00064408
		public readonly int Revision { get; }

		// Token: 0x0600537D RID: 21373 RVA: 0x001C99C8 File Offset: 0x001C7BC8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.MajorVersion.ToString(),
				".",
				this.MinorVersion.ToString(),
				".",
				this.Revision.ToString()
			});
		}

		// Token: 0x0600537E RID: 21374 RVA: 0x00066210 File Offset: 0x00064410
		public bool IsValid()
		{
			return this.MajorVersion >= 0 && this.MinorVersion >= 0 && this.Revision >= 0;
		}

		// Token: 0x0600537F RID: 21375 RVA: 0x00066232 File Offset: 0x00064432
		public Version(int majorVersion = -1, int minorVersion = -1, int revision = -1)
		{
			this.MajorVersion = majorVersion;
			this.MinorVersion = minorVersion;
			this.Revision = revision;
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x001C9A24 File Offset: 0x001C7C24
		public static bool operator ==(Version lhs, Version rhs)
		{
			return lhs.IsValid() && rhs.IsValid() && (lhs.MajorVersion == rhs.MajorVersion && lhs.MinorVersion == rhs.MinorVersion) && lhs.Revision == rhs.Revision;
		}

		// Token: 0x06005381 RID: 21377 RVA: 0x00066249 File Offset: 0x00064449
		public static bool operator !=(Version lhs, Version rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06005382 RID: 21378 RVA: 0x00066255 File Offset: 0x00064455
		public override bool Equals(object obj)
		{
			return obj is Version && this.Equals((Version)obj);
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x0006626D File Offset: 0x0006446D
		public bool Equals(Version other)
		{
			return this.MajorVersion == other.MajorVersion && this.MinorVersion == other.MinorVersion && this.Revision == other.Revision;
		}

		// Token: 0x06005384 RID: 21380 RVA: 0x001C9A7C File Offset: 0x001C7C7C
		public override int GetHashCode()
		{
			return ((366299368 * -1521134295 + this.MajorVersion.GetHashCode()) * -1521134295 + this.MinorVersion.GetHashCode()) * -1521134295 + this.Revision.GetHashCode();
		}

		// Token: 0x040055B0 RID: 21936
		public static readonly Version Invalid = new Version(-1, -1, -1);

		// Token: 0x040055B1 RID: 21937
		public static readonly Version FirstTracked = new Version(1, 2, 33);

		// Token: 0x040055B2 RID: 21938
		public static readonly Version LastUntracked = new Version(1, 2, 32);
	}
}
