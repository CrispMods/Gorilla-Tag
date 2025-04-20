using System;

// Token: 0x02000211 RID: 529
[Flags]
public enum UnityLayerMask
{
	// Token: 0x04000EE9 RID: 3817
	Everything = -1,
	// Token: 0x04000EEA RID: 3818
	Nothing = 0,
	// Token: 0x04000EEB RID: 3819
	Default = 1,
	// Token: 0x04000EEC RID: 3820
	TransparentFX = 2,
	// Token: 0x04000EED RID: 3821
	IgnoreRaycast = 4,
	// Token: 0x04000EEE RID: 3822
	Zone = 8,
	// Token: 0x04000EEF RID: 3823
	Water = 16,
	// Token: 0x04000EF0 RID: 3824
	UI = 32,
	// Token: 0x04000EF1 RID: 3825
	MeshBakerAtlas = 64,
	// Token: 0x04000EF2 RID: 3826
	GorillaEquipment = 128,
	// Token: 0x04000EF3 RID: 3827
	GorillaBodyCollider = 256,
	// Token: 0x04000EF4 RID: 3828
	GorillaObject = 512,
	// Token: 0x04000EF5 RID: 3829
	GorillaHand = 1024,
	// Token: 0x04000EF6 RID: 3830
	GorillaTrigger = 2048,
	// Token: 0x04000EF7 RID: 3831
	MetaReportScreen = 4096,
	// Token: 0x04000EF8 RID: 3832
	GorillaHead = 8192,
	// Token: 0x04000EF9 RID: 3833
	GorillaTagCollider = 16384,
	// Token: 0x04000EFA RID: 3834
	GorillaBoundary = 32768,
	// Token: 0x04000EFB RID: 3835
	GorillaEquipmentContainer = 65536,
	// Token: 0x04000EFC RID: 3836
	LCKHide = 131072,
	// Token: 0x04000EFD RID: 3837
	GorillaInteractable = 262144,
	// Token: 0x04000EFE RID: 3838
	FirstPersonOnly = 524288,
	// Token: 0x04000EFF RID: 3839
	GorillaParticle = 1048576,
	// Token: 0x04000F00 RID: 3840
	GorillaCosmetics = 2097152,
	// Token: 0x04000F01 RID: 3841
	MirrorOnly = 4194304,
	// Token: 0x04000F02 RID: 3842
	GorillaThrowable = 8388608,
	// Token: 0x04000F03 RID: 3843
	GorillaHandSocket = 16777216,
	// Token: 0x04000F04 RID: 3844
	GorillaCosmeticParticle = 33554432,
	// Token: 0x04000F05 RID: 3845
	BuilderProp = 67108864,
	// Token: 0x04000F06 RID: 3846
	NoMirror = 134217728,
	// Token: 0x04000F07 RID: 3847
	GorillaSlingshotCollider = 268435456,
	// Token: 0x04000F08 RID: 3848
	RopeSwing = 536870912,
	// Token: 0x04000F09 RID: 3849
	Prop = 1073741824,
	// Token: 0x04000F0A RID: 3850
	Bake = -2147483648
}
