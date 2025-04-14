using System;
using System.Collections.Generic;
using GorillaGameModes;
using GorillaLocomotion;
using GT_CustomMapSupportRuntime;
using JetBrains.Annotations;
using UnityEngine;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x020009EE RID: 2542
	public class MapBoundary : CustomMapTrigger
	{
		// Token: 0x06003F8A RID: 16266 RVA: 0x0012D254 File Offset: 0x0012B454
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

		// Token: 0x06003F8B RID: 16267 RVA: 0x0012D2D8 File Offset: 0x0012B4D8
		public override void Trigger(double triggerTime = -1.0, bool originatedLocally = false, bool ignoreTriggerCount = false)
		{
			base.Trigger(triggerTime, originatedLocally, ignoreTriggerCount);
			if (originatedLocally && GTPlayer.hasInstance)
			{
				GTPlayer instance = GTPlayer.Instance;
				Transform transform = ModIOMapLoader.GetCustomMapsDefaultSpawnLocation();
				if (this.TeleportPoints.Count != 0)
				{
					transform = this.TeleportPoints[Random.Range(0, this.TeleportPoints.Count)];
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

		// Token: 0x04004095 RID: 16533
		[Tooltip("Teleport points used to return the player to the map. Chosen at random.")]
		[SerializeField]
		[NotNull]
		public List<Transform> TeleportPoints = new List<Transform>();

		// Token: 0x04004096 RID: 16534
		public bool ShouldTagPlayer = true;
	}
}
