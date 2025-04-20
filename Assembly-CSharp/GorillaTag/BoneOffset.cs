using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BA8 RID: 2984
	[Serializable]
	public struct BoneOffset
	{
		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06004BC2 RID: 19394 RVA: 0x00061CD7 File Offset: 0x0005FED7
		public Vector3 pos
		{
			get
			{
				return this.offset.pos;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06004BC3 RID: 19395 RVA: 0x00061CE4 File Offset: 0x0005FEE4
		public Quaternion rot
		{
			get
			{
				return this.offset.rot;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06004BC4 RID: 19396 RVA: 0x00061CF1 File Offset: 0x0005FEF1
		public Vector3 scale
		{
			get
			{
				return this.offset.scale;
			}
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x00061CFE File Offset: 0x0005FEFE
		public BoneOffset(GTHardCodedBones.EBone bone)
		{
			this.bone = bone;
			this.offset = XformOffset.Identity;
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x00061D17 File Offset: 0x0005FF17
		public BoneOffset(GTHardCodedBones.EBone bone, XformOffset offset)
		{
			this.bone = bone;
			this.offset = offset;
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x00061D2C File Offset: 0x0005FF2C
		public BoneOffset(GTHardCodedBones.EBone bone, Vector3 pos, Quaternion rot)
		{
			this.bone = bone;
			this.offset = new XformOffset(pos, rot);
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x00061D47 File Offset: 0x0005FF47
		public BoneOffset(GTHardCodedBones.EBone bone, Vector3 pos, Vector3 rotAngles)
		{
			this.bone = bone;
			this.offset = new XformOffset(pos, rotAngles);
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x00061D62 File Offset: 0x0005FF62
		public BoneOffset(GTHardCodedBones.EBone bone, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			this.bone = bone;
			this.offset = new XformOffset(pos, rot, scale);
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x00061D7F File Offset: 0x0005FF7F
		public BoneOffset(GTHardCodedBones.EBone bone, Vector3 pos, Vector3 rotAngles, Vector3 scale)
		{
			this.bone = bone;
			this.offset = new XformOffset(pos, rotAngles, scale);
		}

		// Token: 0x04004D0C RID: 19724
		public GTHardCodedBones.SturdyEBone bone;

		// Token: 0x04004D0D RID: 19725
		public XformOffset offset;

		// Token: 0x04004D0E RID: 19726
		public static readonly BoneOffset Identity = new BoneOffset
		{
			bone = GTHardCodedBones.EBone.None,
			offset = XformOffset.Identity
		};
	}
}
