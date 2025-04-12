using System;
using GorillaGameModes;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009BD RID: 2493
	public class GorillaAmbushManager : GorillaTagManager
	{
		// Token: 0x06003DF6 RID: 15862 RVA: 0x00057819 File Offset: 0x00055A19
		public override GameModeType GameType()
		{
			if (!this.isGhostTag)
			{
				return GameModeType.Ambush;
			}
			return GameModeType.Ghost;
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x00057826 File Offset: 0x00055A26
		public static int HandEffectHash
		{
			get
			{
				return GorillaAmbushManager.handTapHash;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06003DF8 RID: 15864 RVA: 0x0005782D File Offset: 0x00055A2D
		// (set) Token: 0x06003DF9 RID: 15865 RVA: 0x00057834 File Offset: 0x00055A34
		public static float HandFXScaleModifier { get; private set; }

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06003DFA RID: 15866 RVA: 0x0005783C File Offset: 0x00055A3C
		// (set) Token: 0x06003DFB RID: 15867 RVA: 0x00057844 File Offset: 0x00055A44
		public bool isGhostTag { get; private set; }

		// Token: 0x06003DFC RID: 15868 RVA: 0x0005784D File Offset: 0x00055A4D
		public override void Awake()
		{
			base.Awake();
			if (this.handTapFX != null)
			{
				GorillaAmbushManager.handTapHash = PoolUtils.GameObjHashCode(this.handTapFX);
			}
			GorillaAmbushManager.HandFXScaleModifier = this.handTapScaleFactor;
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x0005787E File Offset: 0x00055A7E
		private void Start()
		{
			this.hasScryingPlane = this.scryingPlaneRef.TryResolve<MeshRenderer>(out this.scryingPlane);
			this.hasScryingPlane3p = this.scryingPlane3pRef.TryResolve<MeshRenderer>(out this.scryingPlane3p);
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x000578AE File Offset: 0x00055AAE
		public override string GameModeName()
		{
			if (!this.isGhostTag)
			{
				return "AMBUSH";
			}
			return "GHOST";
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x001614D4 File Offset: 0x0015F6D4
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

		// Token: 0x06003E00 RID: 15872 RVA: 0x000578C3 File Offset: 0x00055AC3
		public override int MyMatIndex(NetPlayer forPlayer)
		{
			if (!base.IsInfected(forPlayer))
			{
				return 0;
			}
			return 13;
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x0016157C File Offset: 0x0015F77C
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

		// Token: 0x04003F3A RID: 16186
		public GameObject handTapFX;

		// Token: 0x04003F3B RID: 16187
		public GorillaSkin ambushSkin;

		// Token: 0x04003F3C RID: 16188
		[SerializeField]
		private AudioClip[] firstPersonTaggedSounds;

		// Token: 0x04003F3D RID: 16189
		[SerializeField]
		private float firstPersonTaggedSoundVolume;

		// Token: 0x04003F3E RID: 16190
		private static int handTapHash = -1;

		// Token: 0x04003F3F RID: 16191
		public float handTapScaleFactor = 0.5f;

		// Token: 0x04003F41 RID: 16193
		public float crawlingSpeedForMaxVolume;

		// Token: 0x04003F43 RID: 16195
		[SerializeField]
		private XSceneRef scryingPlaneRef;

		// Token: 0x04003F44 RID: 16196
		[SerializeField]
		private XSceneRef scryingPlane3pRef;

		// Token: 0x04003F45 RID: 16197
		private const int STEALTH_MATERIAL_INDEX = 13;

		// Token: 0x04003F46 RID: 16198
		private MeshRenderer scryingPlane;

		// Token: 0x04003F47 RID: 16199
		private bool hasScryingPlane;

		// Token: 0x04003F48 RID: 16200
		private MeshRenderer scryingPlane3p;

		// Token: 0x04003F49 RID: 16201
		private bool hasScryingPlane3p;
	}
}
