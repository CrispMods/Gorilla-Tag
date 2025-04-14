using System;

namespace BoingKit
{
	// Token: 0x02000CB7 RID: 3255
	public struct Version : IEquatable<Version>
	{
		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06005218 RID: 21016 RVA: 0x0019367C File Offset: 0x0019187C
		public readonly int MajorVersion { get; }

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06005219 RID: 21017 RVA: 0x00193684 File Offset: 0x00191884
		public readonly int MinorVersion { get; }

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x0600521A RID: 21018 RVA: 0x0019368C File Offset: 0x0019188C
		public readonly int Revision { get; }

		// Token: 0x0600521B RID: 21019 RVA: 0x00193694 File Offset: 0x00191894
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

		// Token: 0x0600521C RID: 21020 RVA: 0x001936EF File Offset: 0x001918EF
		public bool IsValid()
		{
			return this.MajorVersion >= 0 && this.MinorVersion >= 0 && this.Revision >= 0;
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x00193711 File Offset: 0x00191911
		public Version(int majorVersion = -1, int minorVersion = -1, int revision = -1)
		{
			this.MajorVersion = majorVersion;
			this.MinorVersion = minorVersion;
			this.Revision = revision;
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x00193728 File Offset: 0x00191928
		public static bool operator ==(Version lhs, Version rhs)
		{
			return lhs.IsValid() && rhs.IsValid() && (lhs.MajorVersion == rhs.MajorVersion && lhs.MinorVersion == rhs.MinorVersion) && lhs.Revision == rhs.Revision;
		}

		// Token: 0x0600521F RID: 21023 RVA: 0x0019377D File Offset: 0x0019197D
		public static bool operator !=(Version lhs, Version rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06005220 RID: 21024 RVA: 0x00193789 File Offset: 0x00191989
		public override bool Equals(object obj)
		{
			return obj is Version && this.Equals((Version)obj);
		}

		// Token: 0x06005221 RID: 21025 RVA: 0x001937A1 File Offset: 0x001919A1
		public bool Equals(Version other)
		{
			return this.MajorVersion == other.MajorVersion && this.MinorVersion == other.MinorVersion && this.Revision == other.Revision;
		}

		// Token: 0x06005222 RID: 21026 RVA: 0x001937D4 File Offset: 0x001919D4
		public override int GetHashCode()
		{
			return ((366299368 * -1521134295 + this.MajorVersion.GetHashCode()) * -1521134295 + this.MinorVersion.GetHashCode()) * -1521134295 + this.Revision.GetHashCode();
		}

		// Token: 0x040054A4 RID: 21668
		public static readonly Version Invalid = new Version(-1, -1, -1);

		// Token: 0x040054A5 RID: 21669
		public static readonly Version FirstTracked = new Version(1, 2, 33);

		// Token: 0x040054A6 RID: 21670
		public static readonly Version LastUntracked = new Version(1, 2, 32);
	}
}
