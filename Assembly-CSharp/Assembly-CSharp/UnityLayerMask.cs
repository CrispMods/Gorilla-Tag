using System;

// Token: 0x02000206 RID: 518
[Flags]
public enum UnityLayerMask
{
	// Token: 0x04000EA4 RID: 3748
	Everything = -1,
	// Token: 0x04000EA5 RID: 3749
	Nothing = 0,
	// Token: 0x04000EA6 RID: 3750
	Default = 1,
	// Token: 0x04000EA7 RID: 3751
	TransparentFX = 2,
	// Token: 0x04000EA8 RID: 3752
	IgnoreRaycast = 4,
	// Token: 0x04000EA9 RID: 3753
	Zone = 8,
	// Token: 0x04000EAA RID: 3754
	Water = 16,
	// Token: 0x04000EAB RID: 3755
	UI = 32,
	// Token: 0x04000EAC RID: 3756
	MeshBakerAtlas = 64,
	// Token: 0x04000EAD RID: 3757
	GorillaEquipment = 128,
	// Token: 0x04000EAE RID: 3758
	GorillaBodyCollider = 256,
	// Token: 0x04000EAF RID: 3759
	GorillaObject = 512,
	// Token: 0x04000EB0 RID: 3760
	GorillaHand = 1024,
	// Token: 0x04000EB1 RID: 3761
	GorillaTrigger = 2048,
	// Token: 0x04000EB2 RID: 3762
	MetaReportScreen = 4096,
	// Token: 0x04000EB3 RID: 3763
	GorillaHead = 8192,
	// Token: 0x04000EB4 RID: 3764
	GorillaTagCollider = 16384,
	// Token: 0x04000EB5 RID: 3765
	GorillaBoundary = 32768,
	// Token: 0x04000EB6 RID: 3766
	GorillaEquipmentContainer = 65536,
	// Token: 0x04000EB7 RID: 3767
	LCKHide = 131072,
	// Token: 0x04000EB8 RID: 3768
	GorillaInteractable = 262144,
	// Token: 0x04000EB9 RID: 3769
	FirstPersonOnly = 524288,
	// Token: 0x04000EBA RID: 3770
	GorillaParticle = 1048576,
	// Token: 0x04000EBB RID: 3771
	GorillaCosmetics = 2097152,
	// Token: 0x04000EBC RID: 3772
	MirrorOnly = 4194304,
	// Token: 0x04000EBD RID: 3773
	GorillaThrowable = 8388608,
	// Token: 0x04000EBE RID: 3774
	GorillaHandSocket = 16777216,
	// Token: 0x04000EBF RID: 3775
	GorillaCosmeticParticle = 33554432,
	// Token: 0x04000EC0 RID: 3776
	BuilderProp = 67108864,
	// Token: 0x04000EC1 RID: 3777
	NoMirror = 134217728,
	// Token: 0x04000EC2 RID: 3778
	GorillaSlingshotCollider = 268435456,
	// Token: 0x04000EC3 RID: 3779
	RopeSwing = 536870912,
	// Token: 0x04000EC4 RID: 3780
	Prop = 1073741824,
	// Token: 0x04000EC5 RID: 3781
	Bake = -2147483648
}
