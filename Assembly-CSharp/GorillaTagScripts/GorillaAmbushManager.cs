using System;
using GorillaGameModes;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009BA RID: 2490
	public class GorillaAmbushManager : GorillaTagManager
	{
		// Token: 0x06003DEA RID: 15850 RVA: 0x00125B07 File Offset: 0x00123D07
		public override GameModeType GameType()
		{
			if (!this.isGhostTag)
			{
				return GameModeType.Ambush;
			}
			return GameModeType.Ghost;
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06003DEB RID: 15851 RVA: 0x00125B14 File Offset: 0x00123D14
		public static int HandEffectHash
		{
			get
			{
				return GorillaAmbushManager.handTapHash;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06003DEC RID: 15852 RVA: 0x00125B1B File Offset: 0x00123D1B
		// (set) Token: 0x06003DED RID: 15853 RVA: 0x00125B22 File Offset: 0x00123D22
		public static float HandFXScaleModifier { get; private set; }

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06003DEE RID: 15854 RVA: 0x00125B2A File Offset: 0x00123D2A
		// (set) Token: 0x06003DEF RID: 15855 RVA: 0x00125B32 File Offset: 0x00123D32
		public bool isGhostTag { get; private set; }

		// Token: 0x06003DF0 RID: 15856 RVA: 0x00125B3B File Offset: 0x00123D3B
		public override void Awake()
		{
			base.Awake();
			if (this.handTapFX != null)
			{
				GorillaAmbushManager.handTapHash = PoolUtils.GameObjHashCode(this.handTapFX);
			}
			GorillaAmbushManager.HandFXScaleModifier = this.handTapScaleFactor;
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x00125B6C File Offset: 0x00123D6C
		private void Start()
		{
			this.hasScryingPlane = this.scryingPlaneRef.TryResolve<MeshRenderer>(out this.scryingPlane);
			this.hasScryingPlane3p = this.scryingPlane3pRef.TryResolve<MeshRenderer>(out this.scryingPlane3p);
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x00125B9C File Offset: 0x00123D9C
		public override string GameModeName()
		{
			if (!this.isGhostTag)
			{
				return "AMBUSH";
			}
			return "GHOST";
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x00125BB4 File Offset: 0x00123DB4
		public override void UpdatePlayerAppearance(VRRig rig)
		{
			int materialIndex = this.MyMatIndex(rig.creator);
			rig.ChangeMaterialLocal(materialIndex);
			bool flag = base.IsInfected(rig.Creator);
			bool flag2 = base.IsInfected(NetworkSystem.Instance.LocalPlayer);
			rig.bodyRenderer.SetGameModeBodyType(flag ? GorillaBodyType.Skeleton : GorillaBodyType.Default);
			rig.SetInvisibleToLocalPlayer(flag && !flag2);
			if (this.isGhostTag && rig.isOfflineVRRig)
			{
				CosmeticsController.instance.SetHideCosmeticsFromRemotePlayers(flag);
				if (this.hasScryingPlane)
				{
					this.scryingPlane.enabled = flag2;
				}
				if (this.hasScryingPlane3p)
				{
					this.scryingPlane3p.enabled = flag2;
				}
			}
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x00125C5A File Offset: 0x00123E5A
		public override int MyMatIndex(NetPlayer forPlayer)
		{
			if (!base.IsInfected(forPlayer))
			{
				return 0;
			}
			return 13;
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x00125C6C File Offset: 0x00123E6C
		public override void StopPlaying()
		{
			base.StopPlaying();
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				GorillaSkin.ApplyToRig(vrrig, null, GorillaSkin.SkinType.gameMode);
				vrrig.bodyRenderer.SetGameModeBodyType(GorillaBodyType.Default);
				vrrig.SetInvisibleToLocalPlayer(false);
			}
			CosmeticsController.instance.SetHideCosmeticsFromRemotePlayers(false);
			if (this.hasScryingPlane)
			{
				this.scryingPlane.enabled = false;
			}
			if (this.hasScryingPlane3p)
			{
				this.scryingPlane3p.enabled = false;
			}
		}

		// Token: 0x04003F28 RID: 16168
		public GameObject handTapFX;

		// Token: 0x04003F29 RID: 16169
		public GorillaSkin ambushSkin;

		// Token: 0x04003F2A RID: 16170
		[SerializeField]
		private AudioClip[] firstPersonTaggedSounds;

		// Token: 0x04003F2B RID: 16171
		[SerializeField]
		private float firstPersonTaggedSoundVolume;

		// Token: 0x04003F2C RID: 16172
		private static int handTapHash = -1;

		// Token: 0x04003F2D RID: 16173
		public float handTapScaleFactor = 0.5f;

		// Token: 0x04003F2F RID: 16175
		public float crawlingSpeedForMaxVolume;

		// Token: 0x04003F31 RID: 16177
		[SerializeField]
		private XSceneRef scryingPlaneRef;

		// Token: 0x04003F32 RID: 16178
		[SerializeField]
		private XSceneRef scryingPlane3pRef;

		// Token: 0x04003F33 RID: 16179
		private const int STEALTH_MATERIAL_INDEX = 13;

		// Token: 0x04003F34 RID: 16180
		private MeshRenderer scryingPlane;

		// Token: 0x04003F35 RID: 16181
		private bool hasScryingPlane;

		// Token: 0x04003F36 RID: 16182
		private MeshRenderer scryingPlane3p;

		// Token: 0x04003F37 RID: 16183
		private bool hasScryingPlane3p;
	}
}
