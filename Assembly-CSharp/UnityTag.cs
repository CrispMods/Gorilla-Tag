using System;

// Token: 0x02000213 RID: 531
public enum UnityTag
{
	// Token: 0x04000F0C RID: 3852
	Invalid = -1,
	// Token: 0x04000F0D RID: 3853
	Untagged,
	// Token: 0x04000F0E RID: 3854
	Respawn,
	// Token: 0x04000F0F RID: 3855
	Finish,
	// Token: 0x04000F10 RID: 3856
	EditorOnly,
	// Token: 0x04000F11 RID: 3857
	MainCamera,
	// Token: 0x04000F12 RID: 3858
	Player,
	// Token: 0x04000F13 RID: 3859
	GameController,
	// Token: 0x04000F14 RID: 3860
	SceneChanger,
	// Token: 0x04000F15 RID: 3861
	PlayerOffset,
	// Token: 0x04000F16 RID: 3862
	GorillaTagManager,
	// Token: 0x04000F17 RID: 3863
	GorillaTagCollider,
	// Token: 0x04000F18 RID: 3864
	GorillaPlayer,
	// Token: 0x04000F19 RID: 3865
	GorillaObject,
	// Token: 0x04000F1A RID: 3866
	GorillaGameManager,
	// Token: 0x04000F1B RID: 3867
	GorillaCosmetic,
	// Token: 0x04000F1C RID: 3868
	projectile,
	// Token: 0x04000F1D RID: 3869
	FxTemporaire,
	// Token: 0x04000F1E RID: 3870
	SlingshotProjectile,
	// Token: 0x04000F1F RID: 3871
	SlingshotProjectileTrail,
	// Token: 0x04000F20 RID: 3872
	SlingshotProjectilePlayerImpactFX,
	// Token: 0x04000F21 RID: 3873
	SlingshotProjectileSurfaceImpactFX,
	// Token: 0x04000F22 RID: 3874
	BalloonPopFX,
	// Token: 0x04000F23 RID: 3875
	WorldShareableItem,
	// Token: 0x04000F24 RID: 3876
	HornsSlingshotProjectile,
	// Token: 0x04000F25 RID: 3877
	HornsSlingshotProjectileTrail,
	// Token: 0x04000F26 RID: 3878
	HornsSlingshotProjectilePlayerImpactFX,
	// Token: 0x04000F27 RID: 3879
	HornsSlingshotProjectileSurfaceImpactFX,
	// Token: 0x04000F28 RID: 3880
	FryingPan,
	// Token: 0x04000F29 RID: 3881
	LeafPileImpactFX,
	// Token: 0x04000F2A RID: 3882
	BalloonPopFx,
	// Token: 0x04000F2B RID: 3883
	CloudSlingshotProjectile,
	// Token: 0x04000F2C RID: 3884
	CloudSlingshotProjectileTrail,
	// Token: 0x04000F2D RID: 3885
	CloudSlingshotProjectilePlayerImpactFX,
	// Token: 0x04000F2E RID: 3886
	CloudSlingshotProjectileSurfaceImpactFX,
	// Token: 0x04000F2F RID: 3887
	SnowballProjectile,
	// Token: 0x04000F30 RID: 3888
	SnowballProjectileImpactFX,
	// Token: 0x04000F31 RID: 3889
	CupidBowProjectile,
	// Token: 0x04000F32 RID: 3890
	CupidBowProjectileTrail,
	// Token: 0x04000F33 RID: 3891
	CupidBowProjectileSurfaceImpactFX,
	// Token: 0x04000F34 RID: 3892
	NoCrazyCheck,
	// Token: 0x04000F35 RID: 3893
	IceSlingshotProjectile,
	// Token: 0x04000F36 RID: 3894
	IceSlingshotProjectileSurfaceImpactFX,
	// Token: 0x04000F37 RID: 3895
	IceSlingshotProjectileTrail,
	// Token: 0x04000F38 RID: 3896
	ElfBowProjectile,
	// Token: 0x04000F39 RID: 3897
	ElfBowProjectileSurfaceImpactFX,
	// Token: 0x04000F3A RID: 3898
	ElfBowProjectileTrail,
	// Token: 0x04000F3B RID: 3899
	RenderIfSmall,
	// Token: 0x04000F3C RID: 3900
	DeleteOnNonBetaBuild,
	// Token: 0x04000F3D RID: 3901
	DeleteOnNonDebugBuild,
	// Token: 0x04000F3E RID: 3902
	FlagColoringCauldon,
	// Token: 0x04000F3F RID: 3903
	WaterRippleEffect,
	// Token: 0x04000F40 RID: 3904
	WaterSplashEffect,
	// Token: 0x04000F41 RID: 3905
	FireworkMortarProjectile,
	// Token: 0x04000F42 RID: 3906
	FireworkMortarProjectileImpactFX,
	// Token: 0x04000F43 RID: 3907
	WaterBalloonProjectile,
	// Token: 0x04000F44 RID: 3908
	WaterBalloonProjectileImpactFX,
	// Token: 0x04000F45 RID: 3909
	PlayerHeadTrigger,
	// Token: 0x04000F46 RID: 3910
	WizardStaff,
	// Token: 0x04000F47 RID: 3911
	LurkerGhost,
	// Token: 0x04000F48 RID: 3912
	HauntedObject,
	// Token: 0x04000F49 RID: 3913
	WanderingGhost,
	// Token: 0x04000F4A RID: 3914
	LavaSurfaceRock,
	// Token: 0x04000F4B RID: 3915
	LavaRockProjectile,
	// Token: 0x04000F4C RID: 3916
	LavaRockProjectileImpactFX,
	// Token: 0x04000F4D RID: 3917
	MoltenSlingshotProjectile,
	// Token: 0x04000F4E RID: 3918
	MoltenSlingshotProjectileTrail,
	// Token: 0x04000F4F RID: 3919
	MoltenSlingshotProjectileSurfaceImpactFX,
	// Token: 0x04000F50 RID: 3920
	MoltenSlingshotProjectilePlayerImpactFX,
	// Token: 0x04000F51 RID: 3921
	SpiderBowProjectile,
	// Token: 0x04000F52 RID: 3922
	SpiderBowProjectileTrail,
	// Token: 0x04000F53 RID: 3923
	SpiderBowProjectileSurfaceImpactFX,
	// Token: 0x04000F54 RID: 3924
	SpiderBowProjectilePlayerImpactFX,
	// Token: 0x04000F55 RID: 3925
	ZoneRoot,
	// Token: 0x04000F56 RID: 3926
	DontProcessMaterials,
	// Token: 0x04000F57 RID: 3927
	OrnamentProjectileSurfaceImpactFX,
	// Token: 0x04000F58 RID: 3928
	BucketGiftCane,
	// Token: 0x04000F59 RID: 3929
	BucketGiftCoal,
	// Token: 0x04000F5A RID: 3930
	BucketGiftRoll,
	// Token: 0x04000F5B RID: 3931
	BucketGiftRound,
	// Token: 0x04000F5C RID: 3932
	BucketGiftSquare,
	// Token: 0x04000F5D RID: 3933
	OrnamentProjectile,
	// Token: 0x04000F5E RID: 3934
	OrnamentShatterFX,
	// Token: 0x04000F5F RID: 3935
	ScienceCandyProjectile,
	// Token: 0x04000F60 RID: 3936
	ScienceCandyImpactFX,
	// Token: 0x04000F61 RID: 3937
	PaperAirplaneProjectile,
	// Token: 0x04000F62 RID: 3938
	DevilBowProjectile,
	// Token: 0x04000F63 RID: 3939
	DevilBowProjectileTrail,
	// Token: 0x04000F64 RID: 3940
	DevilBowProjectileSurfaceImpactFX,
	// Token: 0x04000F65 RID: 3941
	DevilBowProjectilePlayerImpactFX,
	// Token: 0x04000F66 RID: 3942
	FireFX,
	// Token: 0x04000F67 RID: 3943
	FishFood,
	// Token: 0x04000F68 RID: 3944
	FishFoodImpactFX,
	// Token: 0x04000F69 RID: 3945
	LeafNinjaStarProjectile,
	// Token: 0x04000F6A RID: 3946
	LeafNinjaStarProjectileC1,
	// Token: 0x04000F6B RID: 3947
	LeafNinjaStarProjectileC2,
	// Token: 0x04000F6C RID: 3948
	SamuraiBowProjectile,
	// Token: 0x04000F6D RID: 3949
	SamuraiBowProjectileTrail,
	// Token: 0x04000F6E RID: 3950
	SamuraiBowProjectileSurfaceImpactFX,
	// Token: 0x04000F6F RID: 3951
	SamuraiBowProjectilePlayerImpactFX,
	// Token: 0x04000F70 RID: 3952
	DragonSlingProjectile,
	// Token: 0x04000F71 RID: 3953
	DragonSlingProjectileTrail,
	// Token: 0x04000F72 RID: 3954
	DragonSlingProjectileSurfaceImpactFX,
	// Token: 0x04000F73 RID: 3955
	DragonSlingProjectilePlayerImpactFX,
	// Token: 0x04000F74 RID: 3956
	FireballProjectile,
	// Token: 0x04000F75 RID: 3957
	StealthHandTapFX,
	// Token: 0x04000F76 RID: 3958
	EnvPieceTree01,
	// Token: 0x04000F77 RID: 3959
	FxSnapPiecePlaced,
	// Token: 0x04000F78 RID: 3960
	FxSnapPieceDisconnected,
	// Token: 0x04000F79 RID: 3961
	FxSnapPieceGrabbed,
	// Token: 0x04000F7A RID: 3962
	FxSnapPieceLocationLock,
	// Token: 0x04000F7B RID: 3963
	CyberNinjaStarProjectile,
	// Token: 0x04000F7C RID: 3964
	RoomLight,
	// Token: 0x04000F7D RID: 3965
	SamplesInfoPanel,
	// Token: 0x04000F7E RID: 3966
	GorillaHandLeft,
	// Token: 0x04000F7F RID: 3967
	GorillaHandRight,
	// Token: 0x04000F80 RID: 3968
	GorillaHandSocket,
	// Token: 0x04000F81 RID: 3969
	PlayingCardProjectile,
	// Token: 0x04000F82 RID: 3970
	RottenPumpkinProjectile,
	// Token: 0x04000F83 RID: 3971
	FxSnapPieceRecycle,
	// Token: 0x04000F84 RID: 3972
	FxSnapPieceDispenser,
	// Token: 0x04000F85 RID: 3973
	AppleProjectile,
	// Token: 0x04000F86 RID: 3974
	AppleProjectileSurfaceImpactFX,
	// Token: 0x04000F87 RID: 3975
	RecyclerForceVolumeFX,
	// Token: 0x04000F88 RID: 3976
	FxSnapPieceTooHeavy,
	// Token: 0x04000F89 RID: 3977
	FxBuilderPrivatePlotClaimed,
	// Token: 0x04000F8A RID: 3978
	TrickTreatCandy,
	// Token: 0x04000F8B RID: 3979
	TrickTreatEyeball,
	// Token: 0x04000F8C RID: 3980
	TrickTreatBat,
	// Token: 0x04000F8D RID: 3981
	TrickTreatBomb,
	// Token: 0x04000F8E RID: 3982
	TrickTreatSurfaceImpact,
	// Token: 0x04000F8F RID: 3983
	TrickTreatBatImpact,
	// Token: 0x04000F90 RID: 3984
	TrickTreatBombImpact,
	// Token: 0x04000F91 RID: 3985
	GuardianSlapFX,
	// Token: 0x04000F92 RID: 3986
	GuardianSlamFX,
	// Token: 0x04000F93 RID: 3987
	GuardianIdolLandedFX,
	// Token: 0x04000F94 RID: 3988
	GuardianIdolFallFX,
	// Token: 0x04000F95 RID: 3989
	GuardianIdolTappedFX,
	// Token: 0x04000F96 RID: 3990
	VotingRockProjectile,
	// Token: 0x04000F97 RID: 3991
	LeafPileImpactFXMedium,
	// Token: 0x04000F98 RID: 3992
	LeafPileImpactFXSmall,
	// Token: 0x04000F99 RID: 3993
	WoodenSword,
	// Token: 0x04000F9A RID: 3994
	WoodenShield,
	// Token: 0x04000F9B RID: 3995
	FxBuilderShrink,
	// Token: 0x04000F9C RID: 3996
	FxBuilderGrow,
	// Token: 0x04000F9D RID: 3997
	FxSnapPieceWreathJump,
	// Token: 0x04000F9E RID: 3998
	ElfLauncherElf,
	// Token: 0x04000F9F RID: 3999
	RubberBandCar,
	// Token: 0x04000FA0 RID: 4000
	SnowPileImpactFX,
	// Token: 0x04000FA1 RID: 4001
	FirecrackersProjectile,
	// Token: 0x04000FA2 RID: 4002
	PaperAirplaneSquareProjectile,
	// Token: 0x04000FA3 RID: 4003
	SmokeBombProjectile,
	// Token: 0x04000FA4 RID: 4004
	ThrowableHeartProjectile,
	// Token: 0x04000FA5 RID: 4005
	SunFlowers,
	// Token: 0x04000FA6 RID: 4006
	RobotCannonProjectile,
	// Token: 0x04000FA7 RID: 4007
	RobotCannonProjectileImpact,
	// Token: 0x04000FA8 RID: 4008
	SmokeBombExplosionEffect,
	// Token: 0x04000FA9 RID: 4009
	FireCrackerExplosionEffect,
	// Token: 0x04000FAA RID: 4010
	GorillaMouth
}
