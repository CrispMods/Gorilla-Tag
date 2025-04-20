using System;
using System.Collections.Generic;
using GorillaGameModes;
using GorillaLocomotion;
using GT_CustomMapSupportRuntime;
using JetBrains.Annotations;
using UnityEngine;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x02000A02 RID: 2562
	public class CMSMapBoundary : CMSTrigger
	{
		// Token: 0x06004004 RID: 16388 RVA: 0x0016B3AC File Offset: 0x001695AC
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

		// Token: 0x06004005 RID: 16389 RVA: 0x0016B430 File Offset: 0x00169630
		public override void Trigger(double triggerTime = -1.0, bool originatedLocally = false, bool ignoreTriggerCount = false)
		{
			base.Trigger(triggerTime, originatedLocally, ignoreTriggerCount);
			if (originatedLocally && GTPlayer.hasInstance)
			{
				GTPlayer instance = GTPlayer.Instance;
				Transform transform = CustomMapLoader.GetCustomMapsDefaultSpawnLocation();
				if (this.TeleportPoints.Count != 0)
				{
					transform = this.TeleportPoints[UnityEngine.Random.Range(0, this.TeleportPoints.Count)];
				}
				if (transform != null)
				{
					instance.TeleportTo(transform, true, false);
				}
				if (this.ShouldTagPlayer)
				{
					GameMode.ReportHit();
				}
			}
		}

		// Token: 0x04004100 RID: 16640
		[Tooltip("Teleport points used to return the player to the map. Chosen at random.")]
		[SerializeField]
		[NotNull]
		public List<Transform> TeleportPoints = new List<Transform>();

		// Token: 0x04004101 RID: 16641
		public bool ShouldTagPlayer = true;
	}
}
