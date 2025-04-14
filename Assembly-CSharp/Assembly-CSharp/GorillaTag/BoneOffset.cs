using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B7E RID: 2942
	[Serializable]
	public struct BoneOffset
	{
		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06004A83 RID: 19075 RVA: 0x0016987C File Offset: 0x00167A7C
		public Vector3 pos
		{
			get
			{
				return this.offset.pos;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06004A84 RID: 19076 RVA: 0x00169889 File Offset: 0x00167A89
		public Quaternion rot
		{
			get
			{
				return this.offset.rot;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06004A85 RID: 19077 RVA: 0x00169896 File Offset: 0x00167A96
		public Vector3 scale
		{
			get
			{
				return this.offset.scale;
			}
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x001698A3 File Offset: 0x00167AA3
		public BoneOffset(GTHardCodedBones.EBone bone)
		{
			this.bone = bone;
			this.offset = XformOffset.Identity;
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x001698BC File Offset: 0x00167ABC
		public BoneOffset(GTHardCodedBones.EBone bone, XformOffset offset)
		{
			this.bone = bone;
			this.offset = offset;
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x001698D1 File Offset: 0x00167AD1
		public BoneOffset(GTHardCodedBones.EBone bone, Vector3 pos, Quaternion rot)
		{
			this.bone = bone;
			this.offset = new XformOffset(pos, rot);
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x001698EC File Offset: 0x00167AEC
		public BoneOffset(GTHardCodedBones.EBone bone, Vector3 pos, Vector3 rotAngles)
		{
			this.bone = bone;
			this.offset = new XformOffset(pos, rotAngles);
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x00169907 File Offset: 0x00167B07
		public BoneOffset(GTHardCodedBones.EBone bone, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			this.bone = bone;
			this.offset = new XformOffset(pos, rot, scale);
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x00169924 File Offset: 0x00167B24
		public BoneOffset(GTHardCodedBones.EBone bone, Vector3 pos, Vector3 rotAngles, Vector3 scale)
		{
			this.bone = bone;
			this.offset = new XformOffset(pos, rotAngles, scale);
		}

		// Token: 0x04004C28 RID: 19496
		public GTHardCodedBones.SturdyEBone bone;

		// Token: 0x04004C29 RID: 19497
		public XformOffset offset;

		// Token: 0x04004C2A RID: 19498
		public static readonly BoneOffset Identity = new BoneOffset
		{
			bone = GTHardCodedBones.EBone.None,
			offset = XformOffset.Identity
		};
	}
}
