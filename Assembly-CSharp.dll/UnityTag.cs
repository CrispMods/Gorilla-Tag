using System;

// Token: 0x02000208 RID: 520
public enum UnityTag
{
	// Token: 0x04000EC7 RID: 3783
	Invalid = -1,
	// Token: 0x04000EC8 RID: 3784
	Untagged,
	// Token: 0x04000EC9 RID: 3785
	Respawn,
	// Token: 0x04000ECA RID: 3786
	Finish,
	// Token: 0x04000ECB RID: 3787
	EditorOnly,
	// Token: 0x04000ECC RID: 3788
	MainCamera,
	// Token: 0x04000ECD RID: 3789
	Player,
	// Token: 0x04000ECE RID: 3790
	GameController,
	// Token: 0x04000ECF RID: 3791
	SceneChanger,
	// Token: 0x04000ED0 RID: 3792
	PlayerOffset,
	// Token: 0x04000ED1 RID: 3793
	GorillaTagManager,
	// Token: 0x04000ED2 RID: 3794
	GorillaTagCollider,
	// Token: 0x04000ED3 RID: 3795
	GorillaPlayer,
	// Token: 0x04000ED4 RID: 3796
	GorillaObject,
	// Token: 0x04000ED5 RID: 3797
	GorillaGameManager,
	// Token: 0x04000ED6 RID: 3798
	GorillaCosmetic,
	// Token: 0x04000ED7 RID: 3799
	projectile,
	// Token: 0x04000ED8 RID: 3800
	FxTemporaire,
	// Token: 0x04000ED9 RID: 3801
	SlingshotProjectile,
	// Token: 0x04000EDA RID: 3802
	SlingshotProjectileTrail,
	// Token: 0x04000EDB RID: 3803
	SlingshotProjectilePlayerImpactFX,
	// Token: 0x04000EDC RID: 3804
	SlingshotProjectileSurfaceImpactFX,
	// Token: 0x04000EDD RID: 3805
	BalloonPopFX,
	// Token: 0x04000EDE RID: 3806
	WorldShareableItem,
	// Token: 0x04000EDF RID: 3807
	HornsSlingshotProjectile,
	// Token: 0x04000EE0 RID: 3808
	HornsSlingshotProjectileTrail,
	// Token: 0x04000EE1 RID: 3809
	HornsSlingshotProjectilePlayerImpactFX,
	// Token: 0x04000EE2 RID: 3810
	HornsSlingshotProjectileSurfaceImpactFX,
	// Token: 0x04000EE3 RID: 3811
	FryingPan,
	// Token: 0x04000EE4 RID: 3812
	LeafPileImpactFX,
	// Token: 0x04000EE5 RID: 3813
	BalloonPopFx,
	// Token: 0x04000EE6 RID: 3814
	CloudSlingshotProjectile,
	// Token: 0x04000EE7 RID: 3815
	CloudSlingshotProjectileTrail,
	// Token: 0x04000EE8 RID: 3816
	CloudSlingshotProjectilePlayerImpactFX,
	// Token: 0x04000EE9 RID: 3817
	CloudSlingshotProjectileSurfaceImpactFX,
	// Token: 0x04000EEA RID: 3818
	SnowballProjectile,
	// Token: 0x04000EEB RID: 3819
	SnowballProjectileImpactFX,
	// Token: 0x04000EEC RID: 3820
	CupidBowProjectile,
	// Token: 0x04000EED RID: 3821
	CupidBowProjectileTrail,
	// Token: 0x04000EEE RID: 3822
	CupidBowProjectileSurfaceImpactFX,
	// Token: 0x04000EEF RID: 3823
	NoCrazyCheck,
	// Token: 0x04000EF0 RID: 3824
	IceSlingshotProjectile,
	// Token: 0x04000EF1 RID: 3825
	IceSlingshotProjectileSurfaceImpactFX,
	// Token: 0x04000EF2 RID: 3826
	IceSlingshotProjectileTrail,
	// Token: 0x04000EF3 RID: 3827
	ElfBowProjectile,
	// Token: 0x04000EF4 RID: 3828
	ElfBowProjectileSurfaceImpactFX,
	// Token: 0x04000EF5 RID: 3829
	ElfBowProjectileTrail,
	// Token: 0x04000EF6 RID: 3830
	RenderIfSmall,
	// Token: 0x04000EF7 RID: 3831
	DeleteOnNonBetaBuild,
	// Token: 0x04000EF8 RID: 3832
	DeleteOnNonDebugBuild,
	// Token: 0x04000EF9 RID: 3833
	FlagColoringCauldon,
	// Token: 0x04000EFA RID: 3834
	WaterRippleEffect,
	// Token: 0x04000EFB RID: 3835
	WaterSplashEffect,
	// Token: 0x04000EFC RID: 3836
	FireworkMortarProjectile,
	// Token: 0x04000EFD RID: 3837
	FireworkMortarProjectileImpactFX,
	// Token: 0x04000EFE RID: 3838
	WaterBalloonProjectile,
	// Token: 0x04000EFF RID: 3839
	WaterBalloonProjectileImpactFX,
	// Token: 0x04000F00 RID: 3840
	PlayerHeadTrigger,
	// Token: 0x04000F01 RID: 3841
	WizardStaff,
	// Token: 0x04000F02 RID: 3842
	LurkerGhost,
	// Token: 0x04000F03 RID: 3843
	HauntedObject,
	// Token: 0x04000F04 RID: 3844
	WanderingGhost,
	// Token: 0x04000F05 RID: 3845
	LavaSurfaceRock,
	// Token: 0x04000F06 RID: 3846
	LavaRockProjectile,
	// Token: 0x04000F07 RID: 3847
	LavaRockProjectileImpactFX,
	// Token: 0x04000F08 RID: 3848
	MoltenSlingshotProjectile,
	// Token: 0x04000F09 RID: 3849
	MoltenSlingshotProjectileTrail,
	// Token: 0x04000F0A RID: 3850
	MoltenSlingshotProjectileSurfaceImpactFX,
	// Token: 0x04000F0B RID: 3851
	MoltenSlingshotProjectilePlayerImpactFX,
	// Token: 0x04000F0C RID: 3852
	SpiderBowProjectile,
	// Token: 0x04000F0D RID: 3853
	SpiderBowProjectileTrail,
	// Token: 0x04000F0E RID: 3854
	SpiderBowProjectileSurfaceImpactFX,
	// Token: 0x04000F0F RID: 3855
	SpiderBowProjectilePlayerImpactFX,
	// Token: 0x04000F10 RID: 3856
	ZoneRoot,
	// Token: 0x04000F11 RID: 3857
	DontProcessMaterials,
	// Token: 0x04000F12 RID: 3858
	OrnamentProjectileSurfaceImpactFX,
	// Token: 0x04000F13 RID: 3859
	BucketGiftCane,
	// Token: 0x04000F14 RID: 3860
	BucketGiftCoal,
	// Token: 0x04000F15 RID: 3861
	BucketGiftRoll,
	// Token: 0x04000F16 RID: 3862
	BucketGiftRound,
	// Token: 0x04000F17 RID: 3863
	BucketGiftSquare,
	// Token: 0x04000F18 RID: 3864
	OrnamentProjectile,
	// Token: 0x04000F19 RID: 3865
	OrnamentShatterFX,
	// Token: 0x04000F1A RID: 3866
	ScienceCandyProjectile,
	// Token: 0x04000F1B RID: 3867
	ScienceCandyImpactFX,
	// Token: 0x04000F1C RID: 3868
	PaperAirplaneProjectile,
	// Token: 0x04000F1D RID: 3869
	DevilBowProjectile,
	// Token: 0x04000F1E RID: 3870
	DevilBowProjectileTrail,
	// Token: 0x04000F1F RID: 3871
	DevilBowProjectileSurfaceImpactFX,
	// Token: 0x04000F20 RID: 3872
	DevilBowProjectilePlayerImpactFX,
	// Token: 0x04000F21 RID: 3873
	FireFX,
	// Token: 0x04000F22 RID: 3874
	FishFood,
	// Token: 0x04000F23 RID: 3875
	FishFoodImpactFX,
	// Token: 0x04000F24 RID: 3876
	LeafNinjaStarProjectile,
	// Token: 0x04000F25 RID: 3877
	LeafNinjaStarProjectileC1,
	// Token: 0x04000F26 RID: 3878
	LeafNinjaStarProjectileC2,
	// Token: 0x04000F27 RID: 3879
	SamuraiBowProjectile,
	// Token: 0x04000F28 RID: 3880
	SamuraiBowProjectileTrail,
	// Token: 0x04000F29 RID: 3881
	SamuraiBowProjectileSurfaceImpactFX,
	// Token: 0x04000F2A RID: 3882
	SamuraiBowProjectilePlayerImpactFX,
	// Token: 0x04000F2B RID: 3883
	DragonSlingProjectile,
	// Token: 0x04000F2C RID: 3884
	DragonSlingProjectileTrail,
	// Token: 0x04000F2D RID: 3885
	DragonSlingProjectileSurfaceImpactFX,
	// Token: 0x04000F2E RID: 3886
	DragonSlingProjectilePlayerImpactFX,
	// Token: 0x04000F2F RID: 3887
	FireballProjectile,
	// Token: 0x04000F30 RID: 3888
	StealthHandTapFX,
	// Token: 0x04000F31 RID: 3889
	EnvPieceTree01,
	// Token: 0x04000F32 RID: 3890
	FxSnapPiecePlaced,
	// Token: 0x04000F33 RID: 3891
	FxSnapPieceDisconnected,
	// Token: 0x04000F34 RID: 3892
	FxSnapPieceGrabbed,
	// Token: 0x04000F35 RID: 3893
	FxSnapPieceLocationLock,
	// Token: 0x04000F36 RID: 3894
	CyberNinjaStarProjectile,
	// Token: 0x04000F37 RID: 3895
	RoomLight,
	// Token: 0x04000F38 RID: 3896
	SamplesInfoPanel,
	// Token: 0x04000F39 RID: 3897
	GorillaHandLeft,
	// Token: 0x04000F3A RID: 3898
	GorillaHandRight,
	// Token: 0x04000F3B RID: 3899
	GorillaHandSocket,
	// Token: 0x04000F3C RID: 3900
	PlayingCardProjectile,
	// Token: 0x04000F3D RID: 3901
	RottenPumpkinProjectile,
	// Token: 0x04000F3E RID: 3902
	FxSnapPieceRecycle,
	// Token: 0x04000F3F RID: 3903
	FxSnapPieceDispenser,
	// Token: 0x04000F40 RID: 3904
	AppleProjectile,
	// Token: 0x04000F41 RID: 3905
	AppleProjectileSurfaceImpactFX,
	// Token: 0x04000F42 RID: 3906
	RecyclerForceVolumeFX,
	// Token: 0x04000F43 RID: 3907
	FxSnapPieceTooHeavy,
	// Token: 0x04000F44 RID: 3908
	FxBuilderPrivatePlotClaimed,
	// Token: 0x04000F45 RID: 3909
	TrickTreatCandy,
	// Token: 0x04000F46 RID: 3910
	TrickTreatEyeball,
	// Token: 0x04000F47 RID: 3911
	TrickTreatBat,
	// Token: 0x04000F48 RID: 3912
	TrickTreatBomb,
	// Token: 0x04000F49 RID: 3913
	TrickTreatSurfaceImpact,
	// Token: 0x04000F4A RID: 3914
	TrickTreatBatImpact,
	// Token: 0x04000F4B RID: 3915
	TrickTreatBombImpact,
	// Token: 0x04000F4C RID: 3916
	GuardianSlapFX,
	// Token: 0x04000F4D RID: 3917
	GuardianSlamFX,
	// Token: 0x04000F4E RID: 3918
	GuardianIdolLandedFX,
	// Token: 0x04000F4F RID: 3919
	GuardianIdolFallFX,
	// Token: 0x04000F50 RID: 3920
	GuardianIdolTappedFX,
	// Token: 0x04000F51 RID: 3921
	VotingRockProjectile,
	// Token: 0x04000F52 RID: 3922
	LeafPileImpactFXMedium,
	// Token: 0x04000F53 RID: 3923
	LeafPileImpactFXSmall,
	// Token: 0x04000F54 RID: 3924
	WoodenSword,
	// Token: 0x04000F55 RID: 3925
	WoodenShield,
	// Token: 0x04000F56 RID: 3926
	FxBuilderShrink,
	// Token: 0x04000F57 RID: 3927
	FxBuilderGrow,
	// Token: 0x04000F58 RID: 3928
	FxSnapPieceWreathJump,
	// Token: 0x04000F59 RID: 3929
	ElfLauncherElf,
	// Token: 0x04000F5A RID: 3930
	RubberBandCar,
	// Token: 0x04000F5B RID: 3931
	SnowPileImpactFX,
	// Token: 0x04000F5C RID: 3932
	FirecrackersProjectile,
	// Token: 0x04000F5D RID: 3933
	PaperAirplaneSquareProjectile,
	// Token: 0x04000F5E RID: 3934
	SmokeBombProjectile,
	// Token: 0x04000F5F RID: 3935
	ThrowableHeartProjectile,
	// Token: 0x04000F60 RID: 3936
	SunFlowers,
	// Token: 0x04000F61 RID: 3937
	RobotCannonProjectile,
	// Token: 0x04000F62 RID: 3938
	RobotCannonProjectileImpact,
	// Token: 0x04000F63 RID: 3939
	SmokeBombExplosionEffect,
	// Token: 0x04000F64 RID: 3940
	FireCrackerExplosionEffect,
	// Token: 0x04000F65 RID: 3941
	GorillaMouth
}
