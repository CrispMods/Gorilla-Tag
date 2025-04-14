using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B7B RID: 2939
	[Serializable]
	public struct BoneOffset
	{
		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06004A77 RID: 19063 RVA: 0x001692B4 File Offset: 0x001674B4
		public Vector3 pos
		{
			get
			{
				return this.offset.pos;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06004A78 RID: 19064 RVA: 0x001692C1 File Offset: 0x001674C1
		public Quaternion rot
		{
			get
			{
				return this.offset.rot;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06004A79 RID: 19065 RVA: 0x001692CE File Offset: 0x001674CE
		public Vector3 scale
		{
			get
			{
				return this.offset.scale;
			}
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x001692DB File Offset: 0x001674DB
		public BoneOffset(GTHardCodedBones.EBone bone)
		{
			this.bone = bone;
			this.offset = XformOffset.Identity;
		}

		// Token: 0x06004A7B RID: 19067 RVA: 0x001692F4 File Offset: 0x001674F4
		public BoneOffset(GTHardCodedBones.EBone bone, XformOffset offset)
		{
			this.bone = bone;
			this.offset = offset;
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x00169309 File Offset: 0x00167509
		public BoneOffset(GTHardCodedBones.EBone bone, Vector3 pos, Quaternion rot)
		{
			this.bone = bone;
			this.offset = new XformOffset(pos, rot);
		}

		// Token: 0x06004A7D RID: 19069 RVA: 0x00169324 File Offset: 0x00167524
		public BoneOffset(GTHardCodedBones.EBone bone, Vector3 pos, Vector3 rotAngles)
		{
			this.bone = bone;
			this.offset = new XformOffset(pos, rotAngles);
		}

		// Token: 0x06004A7E RID: 19070 RVA: 0x0016933F File Offset: 0x0016753F
		public BoneOffset(GTHardCodedBones.EBone bone, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			this.bone = bone;
			this.offset = new XformOffset(pos, rot, scale);
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x0016935C File Offset: 0x0016755C
		public BoneOffset(GTHardCodedBones.EBone bone, Vector3 pos, Vector3 rotAngles, Vector3 scale)
		{
			this.bone = bone;
			this.offset = new XformOffset(pos, rotAngles, scale);
		}

		// Token: 0x04004C16 RID: 19478
		public GTHardCodedBones.SturdyEBone bone;

		// Token: 0x04004C17 RID: 19479
		public XformOffset offset;

		// Token: 0x04004C18 RID: 19480
		public static readonly BoneOffset Identity = new BoneOffset
		{
			bone = GTHardCodedBones.EBone.None,
			offset = XformOffset.Identity
		};
	}
}
