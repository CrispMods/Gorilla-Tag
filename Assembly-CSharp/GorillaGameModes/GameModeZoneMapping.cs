using System;
using System.Collections.Generic;
using GameObjectScheduling;
using GorillaNetworking;
using UnityEngine;

namespace GorillaGameModes
{
	// Token: 0x02000969 RID: 2409
	[CreateAssetMenu(fileName = "New Game Mode Zone Map", menuName = "Game Settings/Game Mode Zone Map", order = 2)]
	public class GameModeZoneMapping : ScriptableObject
	{
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06003AD0 RID: 15056 RVA: 0x0010E587 File Offset: 0x0010C787
		public HashSet<GameModeType> AllModes
		{
			get
			{
				this.init();
				return this.allModes;
			}
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x0010E598 File Offset: 0x0010C798
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

		// Token: 0x06003AD2 RID: 15058 RVA: 0x0010E870 File Offset: 0x0010CA70
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

		// Token: 0x06003AD3 RID: 15059 RVA: 0x0010E8C7 File Offset: 0x0010CAC7
		internal string GetModeName(GameModeType mode)
		{
			this.init();
			if (this.modeNameLookup.ContainsKey(mode))
			{
				return this.modeNameLookup[mode];
			}
			return mode.ToString().ToUpper();
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x0010E8FC File Offset: 0x0010CAFC
		internal bool IsNew(GameModeType mode)
		{
			this.init();
			return this.isNewLookup.Contains(mode);
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x0010E910 File Offset: 0x0010CB10
		internal CountdownTextDate GetCountdown(GameModeType mode)
		{
			this.init();
			if (this.gameModeTypeCountdownsLookup.ContainsKey(mode))
			{
				return this.gameModeTypeCountdownsLookup[mode];
			}
			return null;
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x0010E934 File Offset: 0x0010CB34
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

		// Token: 0x04003BCD RID: 15309
		[SerializeField]
		private GameModeNameOverrides[] gameModeNameOverrides;

		// Token: 0x04003BCE RID: 15310
		[SerializeField]
		private GameModeType[] defaultGameModes;

		// Token: 0x04003BCF RID: 15311
		[SerializeField]
		private ZoneGameModes[] zoneGameModes;

		// Token: 0x04003BD0 RID: 15312
		[SerializeField]
		private GameModeTypeCountdown[] gameModeTypeCountdowns;

		// Token: 0x04003BD1 RID: 15313
		[SerializeField]
		private GameModeType[] newThisUpdate;

		// Token: 0x04003BD2 RID: 15314
		private Dictionary<GTZone, HashSet<GameModeType>> publicZoneGameModesLookup;

		// Token: 0x04003BD3 RID: 15315
		private Dictionary<GTZone, HashSet<GameModeType>> privateZoneGameModesLookup;

		// Token: 0x04003BD4 RID: 15316
		private Dictionary<GameModeType, string> modeNameLookup;

		// Token: 0x04003BD5 RID: 15317
		private HashSet<GameModeType> isNewLookup;

		// Token: 0x04003BD6 RID: 15318
		private Dictionary<GameModeType, CountdownTextDate> gameModeTypeCountdownsLookup;

		// Token: 0x04003BD7 RID: 15319
		private HashSet<GameModeType> allModes;
	}
}
