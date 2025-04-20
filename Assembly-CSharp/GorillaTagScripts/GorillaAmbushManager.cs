using System;
using GorillaGameModes;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009E0 RID: 2528
	public class GorillaAmbushManager : GorillaTagManager
	{
		// Token: 0x06003F02 RID: 16130 RVA: 0x000590B0 File Offset: 0x000572B0
		public override GameModeType GameType()
		{
			if (!this.isGhostTag)
			{
				return GameModeType.Ambush;
			}
			return GameModeType.Ghost;
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06003F03 RID: 16131 RVA: 0x000590BD File Offset: 0x000572BD
		public static int HandEffectHash
		{
			get
			{
				return GorillaAmbushManager.handTapHash;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06003F04 RID: 16132 RVA: 0x000590C4 File Offset: 0x000572C4
		// (set) Token: 0x06003F05 RID: 16133 RVA: 0x000590CB File Offset: 0x000572CB
		public static float HandFXScaleModifier { get; private set; }

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06003F06 RID: 16134 RVA: 0x000590D3 File Offset: 0x000572D3
		// (set) Token: 0x06003F07 RID: 16135 RVA: 0x000590DB File Offset: 0x000572DB
		public bool isGhostTag { get; private set; }

		// Token: 0x06003F08 RID: 16136 RVA: 0x000590E4 File Offset: 0x000572E4
		public override void Awake()
		{
			base.Awake();
			if (this.handTapFX != null)
			{
				GorillaAmbushManager.handTapHash = PoolUtils.GameObjHashCode(this.handTapFX);
			}
			GorillaAmbushManager.HandFXScaleModifier = this.handTapScaleFactor;
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x00059115 File Offset: 0x00057315
		private void Start()
		{
			this.hasScryingPlane = this.scryingPlaneRef.TryResolve<MeshRenderer>(out this.scryingPlane);
			this.hasScryingPlane3p = this.scryingPlane3pRef.TryResolve<MeshRenderer>(out this.scryingPlane3p);
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x00059145 File Offset: 0x00057345
		public override string GameModeName()
		{
			if (!this.isGhostTag)
			{
				return "AMBUSH";
			}
			return "GHOST";
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x001674F8 File Offset: 0x001656F8
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

		// Token: 0x06003F0C RID: 16140 RVA: 0x0005915A File Offset: 0x0005735A
		public override int MyMatIndex(NetPlayer forPlayer)
		{
			if (!base.IsInfected(forPlayer))
			{
				return 0;
			}
			return 13;
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x001675A0 File Offset: 0x001657A0
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

		// Token: 0x04004002 RID: 16386
		public GameObject handTapFX;

		// Token: 0x04004003 RID: 16387
		public GorillaSkin ambushSkin;

		// Token: 0x04004004 RID: 16388
		[SerializeField]
		private AudioClip[] firstPersonTaggedSounds;

		// Token: 0x04004005 RID: 16389
		[SerializeField]
		private float firstPersonTaggedSoundVolume;

		// Token: 0x04004006 RID: 16390
		private static int handTapHash = -1;

		// Token: 0x04004007 RID: 16391
		public float handTapScaleFactor = 0.5f;

		// Token: 0x04004009 RID: 16393
		public float crawlingSpeedForMaxVolume;

		// Token: 0x0400400B RID: 16395
		[SerializeField]
		private XSceneRef scryingPlaneRef;

		// Token: 0x0400400C RID: 16396
		[SerializeField]
		private XSceneRef scryingPlane3pRef;

		// Token: 0x0400400D RID: 16397
		private const int STEALTH_MATERIAL_INDEX = 13;

		// Token: 0x0400400E RID: 16398
		private MeshRenderer scryingPlane;

		// Token: 0x0400400F RID: 16399
		private bool hasScryingPlane;

		// Token: 0x04004010 RID: 16400
		private MeshRenderer scryingPlane3p;

		// Token: 0x04004011 RID: 16401
		private bool hasScryingPlane3p;
	}
}
