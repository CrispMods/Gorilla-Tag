using System;
using System.Collections.Generic;
using System.Linq;
using GorillaExtensions;
using Newtonsoft.Json;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000070 RID: 112
public class Menagerie : MonoBehaviour
{
	// Token: 0x060002CA RID: 714 RVA: 0x00075BF4 File Offset: 0x00073DF4
	private void Start()
	{
		CrittersCageDeposit[] array = UnityEngine.Object.FindObjectsByType<CrittersCageDeposit>(FindObjectsInactive.Include, FindObjectsSortMode.None);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnDepositCritter += this.OnDepositCritter;
		}
		CrittersManager.CheckInitialize();
		this._totalPages = this.critterIndex.critterTypes.Count / this.collection.Length + ((this.critterIndex.critterTypes.Count % this.collection.Length == 0) ? 0 : 1);
		this.Load();
		MenagerieDepositBox donationBox = this.DonationBox;
		donationBox.OnCritterInserted = (Action<MenagerieCritter>)Delegate.Combine(donationBox.OnCritterInserted, new Action<MenagerieCritter>(this.CritterDepositedInDonationBox));
		MenagerieDepositBox favoriteBox = this.FavoriteBox;
		favoriteBox.OnCritterInserted = (Action<MenagerieCritter>)Delegate.Combine(favoriteBox.OnCritterInserted, new Action<MenagerieCritter>(this.CritterDepositedInFavoriteBox));
		MenagerieDepositBox collectionBox = this.CollectionBox;
		collectionBox.OnCritterInserted = (Action<MenagerieCritter>)Delegate.Combine(collectionBox.OnCritterInserted, new Action<MenagerieCritter>(this.CritterDepositedInCollectionBox));
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00075CEC File Offset: 0x00073EEC
	private void CritterDepositedInDonationBox(MenagerieCritter critter)
	{
		if (this.newCritterPen.Contains(critter.Slot))
		{
			critter.currentState = MenagerieCritter.MenagerieCritterState.Donating;
			this.DonateCritter(critter.CritterData);
			this._savedCritters.newCritters.Remove(critter.CritterData);
			this.DespawnCritterFromSlot(critter.Slot);
			this.Save();
			PlayerGameEvents.CritterEvent("Donate" + this.critterIndex[critter.CritterData.critterType].critterName);
		}
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00075D74 File Offset: 0x00073F74
	private void CritterDepositedInFavoriteBox(MenagerieCritter critter)
	{
		if (this.collection.Contains(critter.Slot))
		{
			this._savedCritters.favoriteCritter = critter.CritterData.critterType;
			this.Save();
			this.UpdateFavoriteCritter();
			PlayerGameEvents.CritterEvent("Favorite" + this.critterIndex[critter.CritterData.critterType].critterName);
		}
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00075DE0 File Offset: 0x00073FE0
	private void CritterDepositedInCollectionBox(MenagerieCritter critter)
	{
		if (this.newCritterPen.Contains(critter.Slot))
		{
			this.AddCritterToCollection(critter.CritterData);
			this._savedCritters.newCritters.Remove(critter.CritterData);
			this.DespawnCritterFromSlot(critter.Slot);
			this.Save();
			this.UpdateFavoriteCritter();
			PlayerGameEvents.CritterEvent("Collect" + this.critterIndex[critter.CritterData.critterType].critterName);
		}
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00075E68 File Offset: 0x00074068
	private void OnDepositCritter(Menagerie.CritterData depositedCritter, int playerID)
	{
		try
		{
			if (playerID == PhotonNetwork.LocalPlayer.ActorNumber)
			{
				this.AddCritterToNewCritterPen(depositedCritter);
				this.Save();
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	// Token: 0x060002CF RID: 719 RVA: 0x00075EA8 File Offset: 0x000740A8
	private void AddCritterToNewCritterPen(Menagerie.CritterData critterData)
	{
		if (this._savedCritters.newCritters.Count < this.newCritterPen.Length)
		{
			foreach (MenagerieSlot menagerieSlot in this.newCritterPen)
			{
				if (!menagerieSlot.critter)
				{
					this.SpawnCritterInSlot(menagerieSlot, critterData);
					this._savedCritters.newCritters.Add(critterData);
					return;
				}
			}
		}
		this.DonateCritter(critterData);
		this.Save();
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00075F1C File Offset: 0x0007411C
	private void AddCritterToCollection(Menagerie.CritterData critterData)
	{
		Menagerie.CritterData critterData2;
		if (this._savedCritters.collectedCritters.TryGetValue(critterData.critterType, out critterData2))
		{
			this.DonateCritter(critterData2);
		}
		this._savedCritters.collectedCritters[critterData.critterType] = critterData;
		this.SpawnCollectionCritterIfShowing(critterData);
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x000322B4 File Offset: 0x000304B4
	private void DonateCritter(Menagerie.CritterData critterData)
	{
		this._savedCritters.donatedCritterCount++;
		this.donationCounter.SetText(string.Format(this.DonationText, this._savedCritters.donatedCritterCount), true);
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00075F68 File Offset: 0x00074168
	private void SpawnCritterInSlot(MenagerieSlot slot, Menagerie.CritterData critterData)
	{
		if (slot.IsNull() || critterData == null)
		{
			return;
		}
		this.DespawnCritterFromSlot(slot);
		MenagerieCritter menagerieCritter = UnityEngine.Object.Instantiate<MenagerieCritter>(this.prefab, slot.critterMountPoint);
		menagerieCritter.Slot = slot;
		menagerieCritter.ApplyCritterData(critterData);
		this._critters.Add(menagerieCritter);
		if (slot.label)
		{
			slot.label.text = this.critterIndex[critterData.critterType].critterName;
		}
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00075FE4 File Offset: 0x000741E4
	private void SpawnCollectionCritterIfShowing(Menagerie.CritterData critter)
	{
		int num = critter.critterType - this._collectionPageIndex * this.collection.Length;
		if (num < 0 || num >= this.collection.Length)
		{
			return;
		}
		this.SpawnCritterInSlot(this.collection[num], critter);
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x000322F0 File Offset: 0x000304F0
	private void UpdateMenagerie()
	{
		this.UpdateNewCritterPen();
		this.UpdateCollection();
		this.UpdateFavoriteCritter();
		this.donationCounter.SetText(string.Format(this.DonationText, this._savedCritters.donatedCritterCount), true);
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00076028 File Offset: 0x00074228
	private void UpdateNewCritterPen()
	{
		for (int i = 0; i < this.newCritterPen.Length; i++)
		{
			if (i < this._savedCritters.newCritters.Count)
			{
				this.SpawnCritterInSlot(this.newCritterPen[i], this._savedCritters.newCritters[i]);
			}
			else
			{
				this.DespawnCritterFromSlot(this.newCritterPen[i]);
			}
		}
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0007608C File Offset: 0x0007428C
	private void UpdateCollection()
	{
		int num = this._collectionPageIndex * this.collection.Length;
		for (int i = 0; i < this.collection.Length; i++)
		{
			int num2 = num + i;
			MenagerieSlot menagerieSlot = this.collection[i];
			Menagerie.CritterData critterData;
			if (this._savedCritters.collectedCritters.TryGetValue(num2, out critterData))
			{
				this.SpawnCritterInSlot(menagerieSlot, critterData);
			}
			else
			{
				this.DespawnCritterFromSlot(menagerieSlot);
				CritterConfiguration critterConfiguration = this.critterIndex[num2];
				menagerieSlot.label.text = ((critterConfiguration == null) ? "" : "??????");
			}
		}
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0007611C File Offset: 0x0007431C
	private void UpdateFavoriteCritter()
	{
		Menagerie.CritterData critterData;
		if (this._savedCritters.collectedCritters.TryGetValue(this._savedCritters.favoriteCritter, out critterData))
		{
			this.SpawnCritterInSlot(this.favoriteCritterSlot, critterData);
			return;
		}
		this.ClearSlot(this.favoriteCritterSlot);
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0003232B File Offset: 0x0003052B
	public void NextGroupCollectedCritters()
	{
		this._collectionPageIndex++;
		if (this._collectionPageIndex >= this._totalPages)
		{
			this._collectionPageIndex = 0;
		}
		this.UpdateCollection();
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00032356 File Offset: 0x00030556
	public void PrevGroupCollectedCritters()
	{
		this._collectionPageIndex--;
		if (this._collectionPageIndex < 0)
		{
			this._collectionPageIndex = this._totalPages - 1;
		}
		this.UpdateCollection();
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00032383 File Offset: 0x00030583
	private void GenerateNewCritters()
	{
		this.GenerateNewCritterCount(UnityEngine.Random.Range(Mathf.Min(1, this.newCritterPen.Length), this.newCritterPen.Length + 1));
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00076164 File Offset: 0x00074364
	private void GenerateLegalNewCritters()
	{
		this.ClearNewCritterPen();
		for (int i = 0; i < this.newCritterPen.Length; i++)
		{
			int randomCritterType = this.critterIndex.GetRandomCritterType(null);
			if (randomCritterType < 0)
			{
				Debug.LogError("Failed to spawn valid critter. No critter configuration found.");
				return;
			}
			Menagerie.CritterData critterData = new Menagerie.CritterData(randomCritterType, this.critterIndex[randomCritterType].GenerateAppearance());
			this.AddCritterToNewCritterPen(critterData);
		}
	}

	// Token: 0x060002DC RID: 732 RVA: 0x000761C8 File Offset: 0x000743C8
	private void GenerateNewCritterCount(int critterCount)
	{
		this.ClearNewCritterPen();
		for (int i = 0; i < critterCount; i++)
		{
			int num = UnityEngine.Random.Range(0, this.critterIndex.critterTypes.Count);
			CritterConfiguration critterConfiguration = this.critterIndex[num];
			Menagerie.CritterData critterData = new Menagerie.CritterData(num, critterConfiguration.GenerateAppearance());
			this.AddCritterToNewCritterPen(critterData);
		}
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00076220 File Offset: 0x00074420
	private void GenerateCollectedCritters(float spawnChance)
	{
		this.ClearCollection();
		for (int i = 0; i < this.critterIndex.critterTypes.Count; i++)
		{
			if (UnityEngine.Random.value <= spawnChance)
			{
				CritterConfiguration critterConfiguration = this.critterIndex[i];
				Menagerie.CritterData critterData = new Menagerie.CritterData(i, critterConfiguration.GenerateAppearance());
				this.AddCritterToCollection(critterData);
				critterData.instance;
			}
		}
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00076284 File Offset: 0x00074484
	private void MoveNewCrittersToCollection()
	{
		foreach (MenagerieSlot menagerieSlot in this.newCritterPen)
		{
			if (menagerieSlot.critter)
			{
				this.CritterDepositedInCollectionBox(menagerieSlot.critter);
			}
		}
	}

	// Token: 0x060002DF RID: 735 RVA: 0x000762C4 File Offset: 0x000744C4
	private void DonateNewCritters()
	{
		foreach (MenagerieSlot menagerieSlot in this.newCritterPen)
		{
			if (menagerieSlot.critter)
			{
				this.CritterDepositedInDonationBox(menagerieSlot.critter);
			}
		}
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x000323A8 File Offset: 0x000305A8
	private void ClearSlot(MenagerieSlot slot)
	{
		this.DespawnCritterFromSlot(slot);
		if (slot.label)
		{
			slot.label.text = "";
		}
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x00076304 File Offset: 0x00074504
	private void DespawnCritterFromSlot(MenagerieSlot slot)
	{
		if (slot.IsNull())
		{
			return;
		}
		if (!slot.critter)
		{
			return;
		}
		this._critters.Remove(slot.critter);
		UnityEngine.Object.Destroy(slot.critter.gameObject);
		slot.critter = null;
		if (slot.label)
		{
			slot.label.text = "";
		}
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x000323CE File Offset: 0x000305CE
	private void ClearNewCritterPen()
	{
		this._savedCritters.newCritters.Clear();
		this.UpdateNewCritterPen();
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000323E6 File Offset: 0x000305E6
	private void ClearCollection()
	{
		this._savedCritters.collectedCritters.Clear();
		this.UpdateCollection();
		this.UpdateFavoriteCritter();
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x00032404 File Offset: 0x00030604
	private void ClearAll()
	{
		this._savedCritters.Clear();
		this.UpdateMenagerie();
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00032417 File Offset: 0x00030617
	private void ResetSavedCreatures()
	{
		this.ClearAll();
		this.Save();
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x00076370 File Offset: 0x00074570
	private void Load()
	{
		this.ClearAll();
		string @string = PlayerPrefs.GetString("_SavedCritters", string.Empty);
		this.LoadCrittersFromJson(@string);
		this.UpdateMenagerie();
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x000763A0 File Offset: 0x000745A0
	private void Save()
	{
		Debug.Log(string.Format("Saving {0} critters", this._critters.Count));
		string value = this.SaveCrittersToJson();
		PlayerPrefs.SetString("_SavedCritters", value);
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x000763E0 File Offset: 0x000745E0
	private void LoadCrittersFromJson(string jsonString)
	{
		this._savedCritters.Clear();
		if (!string.IsNullOrEmpty(jsonString))
		{
			try
			{
				this._savedCritters = JsonConvert.DeserializeObject<Menagerie.CritterSaveData>(jsonString);
			}
			catch (Exception exception)
			{
				Debug.LogError("Unable to deserialize critters from json: " + jsonString);
				Debug.LogException(exception);
			}
		}
		this.ValidateSaveData();
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x0007643C File Offset: 0x0007463C
	private string SaveCrittersToJson()
	{
		this.ValidateSaveData();
		string text = JsonConvert.SerializeObject(this._savedCritters, Formatting.Indented);
		Debug.Log("Critters save to JSON: " + text);
		return text;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00076470 File Offset: 0x00074670
	private void ValidateSaveData()
	{
		if (this._savedCritters.newCritters.Count > this.newCritterPen.Length)
		{
			Debug.LogError(string.Format("Too many new critters in CrittersSaveData ({0} vs {1}) - correcting.", this._savedCritters.newCritters.Count, this.newCritterPen.Length));
			while (this._savedCritters.newCritters.Count > this.newCritterPen.Length)
			{
				this._savedCritters.newCritters.RemoveAt(this.newCritterPen.Length);
			}
			this.Save();
		}
	}

	// Token: 0x060002EB RID: 747 RVA: 0x00076504 File Offset: 0x00074704
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		MenagerieSlot[] array = this.newCritterPen;
		for (int i = 0; i < array.Length; i++)
		{
			Gizmos.DrawWireSphere(array[i].critterMountPoint.position, 0.1f);
		}
		array = this.collection;
		for (int i = 0; i < array.Length; i++)
		{
			Gizmos.DrawWireSphere(array[i].critterMountPoint.position, 0.1f);
		}
		Gizmos.DrawWireSphere(this.favoriteCritterSlot.critterMountPoint.position, 0.1f);
	}

	// Token: 0x04000381 RID: 897
	[FormerlySerializedAs("creatureIndex")]
	public CritterIndex critterIndex;

	// Token: 0x04000382 RID: 898
	public MenagerieCritter prefab;

	// Token: 0x04000383 RID: 899
	private List<MenagerieCritter> _critters = new List<MenagerieCritter>();

	// Token: 0x04000384 RID: 900
	private Menagerie.CritterSaveData _savedCritters = new Menagerie.CritterSaveData();

	// Token: 0x04000385 RID: 901
	public MenagerieSlot[] collection;

	// Token: 0x04000386 RID: 902
	public MenagerieSlot[] newCritterPen;

	// Token: 0x04000387 RID: 903
	public MenagerieSlot favoriteCritterSlot;

	// Token: 0x04000388 RID: 904
	private int _collectionPageIndex;

	// Token: 0x04000389 RID: 905
	private int _totalPages;

	// Token: 0x0400038A RID: 906
	public MenagerieDepositBox DonationBox;

	// Token: 0x0400038B RID: 907
	public MenagerieDepositBox FavoriteBox;

	// Token: 0x0400038C RID: 908
	public MenagerieDepositBox CollectionBox;

	// Token: 0x0400038D RID: 909
	public TextMeshPro donationCounter;

	// Token: 0x0400038E RID: 910
	public string DonationText = "DONATED:{0}";

	// Token: 0x0400038F RID: 911
	private const string CrittersSavePrefsKey = "_SavedCritters";

	// Token: 0x02000071 RID: 113
	public class CritterData
	{
		// Token: 0x060002ED RID: 749 RVA: 0x0003244E File Offset: 0x0003064E
		public CritterConfiguration GetConfiguration()
		{
			return CrittersManager.instance.creatureIndex[this.critterType];
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00030490 File Offset: 0x0002E690
		public CritterData()
		{
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00032467 File Offset: 0x00030667
		public CritterData(CritterConfiguration config, CritterAppearance appearance)
		{
			this.critterType = CrittersManager.instance.creatureIndex.critterTypes.IndexOf(config);
			this.appearance = appearance;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00032493 File Offset: 0x00030693
		public CritterData(int critterType, CritterAppearance appearance)
		{
			this.critterType = critterType;
			this.appearance = appearance;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000324A9 File Offset: 0x000306A9
		public CritterData(CritterVisuals visuals)
		{
			this.critterType = visuals.critterType;
			this.appearance = visuals.Appearance;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x000324C9 File Offset: 0x000306C9
		public CritterData(Menagerie.CritterData source)
		{
			this.critterType = source.critterType;
			this.appearance = source.appearance;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x000324E9 File Offset: 0x000306E9
		public override string ToString()
		{
			return string.Format("{0} {1} [instance]", this.critterType, this.appearance);
		}

		// Token: 0x04000390 RID: 912
		public int critterType;

		// Token: 0x04000391 RID: 913
		public CritterAppearance appearance;

		// Token: 0x04000392 RID: 914
		[NonSerialized]
		public MenagerieCritter instance;
	}

	// Token: 0x02000072 RID: 114
	[Serializable]
	public class CritterSaveData
	{
		// Token: 0x060002F4 RID: 756 RVA: 0x0003250B File Offset: 0x0003070B
		public void Clear()
		{
			this.newCritters.Clear();
			this.collectedCritters.Clear();
			this.donatedCritterCount = 0;
			this.favoriteCritter = -1;
		}

		// Token: 0x04000393 RID: 915
		public List<Menagerie.CritterData> newCritters = new List<Menagerie.CritterData>();

		// Token: 0x04000394 RID: 916
		public Dictionary<int, Menagerie.CritterData> collectedCritters = new Dictionary<int, Menagerie.CritterData>();

		// Token: 0x04000395 RID: 917
		public int donatedCritterCount;

		// Token: 0x04000396 RID: 918
		public int favoriteCritter = -1;
	}
}
