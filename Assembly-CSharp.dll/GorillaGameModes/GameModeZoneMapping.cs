using System;
using System.Collections.Generic;
using GameObjectScheduling;
using GorillaNetworking;
using UnityEngine;

namespace GorillaGameModes
{
	// Token: 0x0200096C RID: 2412
	[CreateAssetMenu(fileName = "New Game Mode Zone Map", menuName = "Game Settings/Game Mode Zone Map", order = 2)]
	public class GameModeZoneMapping : ScriptableObject
	{
		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x0005592B File Offset: 0x00053B2B
		public HashSet<GameModeType> AllModes
		{
			get
			{
				this.init();
				return this.allModes;
			}
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x0014BE8C File Offset: 0x0014A08C
		private void init()
		{
			if (this.allModes != null)
			{
				return;
			}
			this.allModes = new HashSet<GameModeType>();
			for (int i = 0; i < this.defaultGameModes.Length; i++)
			{
				if (!this.allModes.Contains(this.defaultGameModes[i]))
				{
					this.allModes.Add(this.defaultGameModes[i]);
				}
			}
			this.publicZoneGameModesLookup = new Dictionary<GTZone, HashSet<GameModeType>>();
			this.privateZoneGameModesLookup = new Dictionary<GTZone, HashSet<GameModeType>>();
			for (int j = 0; j < this.zoneGameModes.Length; j++)
			{
				for (int k = 0; k < this.zoneGameModes[j].zone.Length; k++)
				{
					this.publicZoneGameModesLookup.Add(this.zoneGameModes[j].zone[k], new HashSet<GameModeType>(this.zoneGameModes[j].modes));
					for (int l = 0; l < this.zoneGameModes[j].modes.Length; l++)
					{
						if (!this.allModes.Contains(this.zoneGameModes[j].modes[l]))
						{
							this.allModes.Add(this.zoneGameModes[j].modes[l]);
						}
					}
					if (this.zoneGameModes[j].privateModes.Length != 0)
					{
						this.privateZoneGameModesLookup.Add(this.zoneGameModes[j].zone[k], new HashSet<GameModeType>(this.zoneGameModes[j].privateModes));
						for (int m = 0; m < this.zoneGameModes[j].privateModes.Length; m++)
						{
							if (!this.allModes.Contains(this.zoneGameModes[j].privateModes[m]))
							{
								this.allModes.Add(this.zoneGameModes[j].privateModes[m]);
							}
						}
					}
					else
					{
						this.privateZoneGameModesLookup.Add(this.zoneGameModes[j].zone[k], new HashSet<GameModeType>(this.zoneGameModes[j].modes));
					}
				}
			}
			this.modeNameLookup = new Dictionary<GameModeType, string>();
			for (int n = 0; n < this.gameModeNameOverrides.Length; n++)
			{
				this.modeNameLookup.Add(this.gameModeNameOverrides[n].mode, this.gameModeNameOverrides[n].displayName);
			}
			this.isNewLookup = new HashSet<GameModeType>(this.newThisUpdate);
			this.gameModeTypeCountdownsLookup = new Dictionary<GameModeType, CountdownTextDate>();
			for (int num = 0; num < this.gameModeTypeCountdowns.Length; num++)
			{
				this.gameModeTypeCountdownsLookup.Add(this.gameModeTypeCountdowns[num].mode, this.gameModeTypeCountdowns[num].countdownTextDate);
			}
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x0014C164 File Offset: 0x0014A364
		public HashSet<GameModeType> GetModesForZone(GTZone zone, bool isPrivate)
		{
			this.init();
			if (isPrivate && this.privateZoneGameModesLookup.ContainsKey(zone))
			{
				return this.privateZoneGameModesLookup[zone];
			}
			if (this.publicZoneGameModesLookup.ContainsKey(zone))
			{
				return this.publicZoneGameModesLookup[zone];
			}
			return new HashSet<GameModeType>(this.defaultGameModes);
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x00055939 File Offset: 0x00053B39
		internal string GetModeName(GameModeType mode)
		{
			this.init();
			if (this.modeNameLookup.ContainsKey(mode))
			{
				return this.modeNameLookup[mode];
			}
			return mode.ToString().ToUpper();
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x0005596E File Offset: 0x00053B6E
		internal bool IsNew(GameModeType mode)
		{
			this.init();
			return this.isNewLookup.Contains(mode);
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x00055982 File Offset: 0x00053B82
		internal CountdownTextDate GetCountdown(GameModeType mode)
		{
			this.init();
			if (this.gameModeTypeCountdownsLookup.ContainsKey(mode))
			{
				return this.gameModeTypeCountdownsLookup[mode];
			}
			return null;
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x0014C1BC File Offset: 0x0014A3BC
		internal GameModeType VerifyModeForZone(GTZone zone, GameModeType mode, bool isPrivate)
		{
			if (GorillaComputer.instance.IsPlayerInVirtualStump())
			{
				zone = GTZone.customMaps;
			}
			if (zone == GTZone.none)
			{
				return mode;
			}
			HashSet<GameModeType> hashSet;
			if (isPrivate && this.privateZoneGameModesLookup.ContainsKey(zone))
			{
				hashSet = this.privateZoneGameModesLookup[zone];
			}
			else if (this.publicZoneGameModesLookup.ContainsKey(zone))
			{
				hashSet = this.publicZoneGameModesLookup[zone];
			}
			else
			{
				hashSet = new HashSet<GameModeType>(this.defaultGameModes);
			}
			if (hashSet.Contains(mode))
			{
				return mode;
			}
			using (HashSet<GameModeType>.Enumerator enumerator = hashSet.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return GameModeType.Casual;
		}

		// Token: 0x04003BDF RID: 15327
		[SerializeField]
		private GameModeNameOverrides[] gameModeNameOverrides;

		// Token: 0x04003BE0 RID: 15328
		[SerializeField]
		private GameModeType[] defaultGameModes;

		// Token: 0x04003BE1 RID: 15329
		[SerializeField]
		private ZoneGameModes[] zoneGameModes;

		// Token: 0x04003BE2 RID: 15330
		[SerializeField]
		private GameModeTypeCountdown[] gameModeTypeCountdowns;

		// Token: 0x04003BE3 RID: 15331
		[SerializeField]
		private GameModeType[] newThisUpdate;

		// Token: 0x04003BE4 RID: 15332
		private Dictionary<GTZone, HashSet<GameModeType>> publicZoneGameModesLookup;

		// Token: 0x04003BE5 RID: 15333
		private Dictionary<GTZone, HashSet<GameModeType>> privateZoneGameModesLookup;

		// Token: 0x04003BE6 RID: 15334
		private Dictionary<GameModeType, string> modeNameLookup;

		// Token: 0x04003BE7 RID: 15335
		private HashSet<GameModeType> isNewLookup;

		// Token: 0x04003BE8 RID: 15336
		private Dictionary<GameModeType, CountdownTextDate> gameModeTypeCountdownsLookup;

		// Token: 0x04003BE9 RID: 15337
		private HashSet<GameModeType> allModes;
	}
}
