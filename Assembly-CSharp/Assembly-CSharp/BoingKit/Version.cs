using System;

namespace BoingKit
{
	// Token: 0x02000CBA RID: 3258
	public struct Version : IEquatable<Version>
	{
		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06005224 RID: 21028 RVA: 0x00193C44 File Offset: 0x00191E44
		public readonly int MajorVersion { get; }

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06005225 RID: 21029 RVA: 0x00193C4C File Offset: 0x00191E4C
		public readonly int MinorVersion { get; }

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06005226 RID: 21030 RVA: 0x00193C54 File Offset: 0x00191E54
		public readonly int Revision { get; }

		// Token: 0x06005227 RID: 21031 RVA: 0x00193C5C File Offset: 0x00191E5C
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

		// Token: 0x06005228 RID: 21032 RVA: 0x00193CB7 File Offset: 0x00191EB7
		public bool IsValid()
		{
			return this.MajorVersion >= 0 && this.MinorVersion >= 0 && this.Revision >= 0;
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x00193CD9 File Offset: 0x00191ED9
		public Version(int majorVersion = -1, int minorVersion = -1, int revision = -1)
		{
			this.MajorVersion = majorVersion;
			this.MinorVersion = minorVersion;
			this.Revision = revision;
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x00193CF0 File Offset: 0x00191EF0
		public static bool operator ==(Version lhs, Version rhs)
		{
			return lhs.IsValid() && rhs.IsValid() && (lhs.MajorVersion == rhs.MajorVersion && lhs.MinorVersion == rhs.MinorVersion) && lhs.Revision == rhs.Revision;
		}

		// Token: 0x0600522B RID: 21035 RVA: 0x00193D45 File Offset: 0x00191F45
		public static bool operator !=(Version lhs, Version rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600522C RID: 21036 RVA: 0x00193D51 File Offset: 0x00191F51
		public override bool Equals(object obj)
		{
			return obj is Version && this.Equals((Version)obj);
		}

		// Token: 0x0600522D RID: 21037 RVA: 0x00193D69 File Offset: 0x00191F69
		public bool Equals(Version other)
		{
			return this.MajorVersion == other.MajorVersion && this.MinorVersion == other.MinorVersion && this.Revision == other.Revision;
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x00193D9C File Offset: 0x00191F9C
		public override int GetHashCode()
		{
			return ((366299368 * -1521134295 + this.MajorVersion.GetHashCode()) * -1521134295 + this.MinorVersion.GetHashCode()) * -1521134295 + this.Revision.GetHashCode();
		}

		// Token: 0x040054B6 RID: 21686
		public static readonly Version Invalid = new Version(-1, -1, -1);

		// Token: 0x040054B7 RID: 21687
		public static readonly Version FirstTracked = new Version(1, 2, 33);

		// Token: 0x040054B8 RID: 21688
		public static readonly Version LastUntracked = new Version(1, 2, 32);
	}
}
