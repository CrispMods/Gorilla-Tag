﻿using System;
using System.Collections.Generic;
using GorillaGameModes;
using GorillaLocomotion;
using GT_CustomMapSupportRuntime;
using JetBrains.Annotations;
using UnityEngine;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x020009F1 RID: 2545
	public class MapBoundary : CustomMapTrigger
	{
		// Token: 0x06003F96 RID: 16278 RVA: 0x0016783C File Offset: 0x00165A3C
		public override void CopyTriggerSettings(TriggerSettings settings)
		{
			if (settings.GetType() == typeof(MapBoundarySettings))
			{
				MapBoundarySettings mapBoundarySettings = (MapBoundarySettings)settings;
				this.TeleportPoints = mapBoundarySettings.TeleportPoints;
				this.ShouldTagPlayer = mapBoundarySettings.ShouldTagPlayer;
			}
			for (int i = this.TeleportPoints.Count - 1; i >= 0; i--)
			{
				if (this.TeleportPoints[i] == null)
				{
					this.TeleportPoints.RemoveAt(i);
				}
			}
			base.CopyTriggerSettings(settings);
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x001678C0 File Offset: 0x00165AC0
		public override void Trigger(double triggerTime = -1.0, bool originatedLocally = false, bool ignoreTriggerCount = false)
		{
			base.Trigger(triggerTime, originatedLocally, ignoreTriggerCount);
			if (originatedLocally && GTPlayer.hasInstance)
			{
				GTPlayer instance = GTPlayer.Instance;
				Transform transform = ModIOMapLoader.GetCustomMapsDefaultSpawnLocation();
				if (this.TeleportPoints.Count != 0)
				{
					transform = this.TeleportPoints[UnityEngine.Random.Range(0, this.TeleportPoints.Count)];
				}
				if (transform != null)
				{
					instance.TeleportTo(transform, false);
				}
				if (this.ShouldTagPlayer)
				{
					GameMode.ReportHit();
				}
			}
		}

		// Token: 0x040040A7 RID: 16551
		[Tooltip("Teleport points used to return the player to the map. Chosen at random.")]
		[SerializeField]
		[NotNull]
		public List<Transform> TeleportPoints = new List<Transform>();

		// Token: 0x040040A8 RID: 16552
		public bool ShouldTagPlayer = true;
	}
}
