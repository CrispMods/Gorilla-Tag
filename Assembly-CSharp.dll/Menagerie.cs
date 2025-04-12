using System;
using System.Collections.Generic;
using System.Linq;
using GorillaExtensions;
using Newtonsoft.Json;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200006A RID: 106
public class Menagerie : MonoBehaviour
{
	// Token: 0x0600029E RID: 670 RVA: 0x000735A8 File Offset: 0x000717A8
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

	// Token: 0x0600029F RID: 671 RVA: 0x000736A0 File Offset: 0x000718A0
	private void CritterDepositedInDonationBox(MenagerieCritter critter)
	{
		if (this.newCritterPen.Contains(critter.Slot))
		{
			critter.currentState = MenagerieCritter.MenagerieCritterState.Donating;
			this.DonateCritter(critter.CritterData);
			this._savedCritters.newCritters.Remove(critter.CritterData);
			this.DespawnCritterFromSlot(critter.Slot);
			this.Save();
		}
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0003114A File Offset: 0x0002F34A
	private void CritterDepositedInFavoriteBox(MenagerieCritter critter)
	{
		if (this.collection.Contains(critter.Slot))
		{
			this._savedCritters.favoriteCritter = critter.CritterData.critterType;
			this.Save();
			this.UpdateFavoriteCritter();
		}
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x000736FC File Offset: 0x000718FC
	private void CritterDepositedInCollectionBox(MenagerieCritter critter)
	{
		if (this.newCritterPen.Contains(critter.Slot))
		{
			this.AddCritterToCollection(critter.CritterData);
			this._savedCritters.newCritters.Remove(critter.CritterData);
			this.DespawnCritterFromSlot(critter.Slot);
			this.Save();
			this.UpdateFavoriteCritter();
		}
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x00073758 File Offset: 0x00071958
	private void OnDepositCritter(CrittersPawn depositedCritter, int playerID)
	{
		try
		{
			if (playerID == PhotonNetwork.LocalPlayer.ActorNumber)
			{
				Menagerie.CritterData critterData = new Menagerie.CritterData(depositedCritter.visuals);
				this.AddCritterToNewCritterPen(critterData);
				this.Save();
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x000737A4 File Offset: 0x000719A4
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

	// Token: 0x060002A4 RID: 676 RVA: 0x00073818 File Offset: 0x00071A18
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

	// Token: 0x060002A5 RID: 677 RVA: 0x00031181 File Offset: 0x0002F381
	private void DonateCritter(Menagerie.CritterData critterData)
	{
		this._savedCritters.donatedCritterCount++;
		this.donationCounter.SetText(string.Format(this.DonationText, this._savedCritters.donatedCritterCount), true);
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00073864 File Offset: 0x00071A64
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

	// Token: 0x060002A7 RID: 679 RVA: 0x000738E0 File Offset: 0x00071AE0
	private void SpawnCollectionCritterIfShowing(Menagerie.CritterData critter)
	{
		int num = critter.critterType - this._collectionPageIndex * this.collection.Length;
		if (num < 0 || num >= this.collection.Length)
		{
			return;
		}
		this.SpawnCritterInSlot(this.collection[num], critter);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x000311BD File Offset: 0x0002F3BD
	private void UpdateMenagerie()
	{
		this.UpdateNewCritterPen();
		this.UpdateCollection();
		this.UpdateFavoriteCritter();
		this.donationCounter.SetText(string.Format(this.DonationText, this._savedCritters.donatedCritterCount), true);
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x00073924 File Offset: 0x00071B24
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

	// Token: 0x060002AA RID: 682 RVA: 0x00073988 File Offset: 0x00071B88
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

	// Token: 0x060002AB RID: 683 RVA: 0x00073A18 File Offset: 0x00071C18
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

	// Token: 0x060002AC RID: 684 RVA: 0x000311F8 File Offset: 0x0002F3F8
	public void NextGroupCollectedCritters()
	{
		this._collectionPageIndex++;
		if (this._collectionPageIndex >= this._totalPages)
		{
			this._collectionPageIndex = 0;
		}
		this.UpdateCollection();
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00031223 File Offset: 0x0002F423
	public void PrevGroupCollectedCritters()
	{
		this._collectionPageIndex--;
		if (this._collectionPageIndex < 0)
		{
			this._collectionPageIndex = this._totalPages - 1;
		}
		this.UpdateCollection();
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00031250 File Offset: 0x0002F450
	private void GenerateNewCritters()
	{
		this.GenerateNewCritterCount(UnityEngine.Random.Range(Mathf.Min(1, this.newCritterPen.Length), this.newCritterPen.Length + 1));
	}

	// Token: 0x060002AF RID: 687 RVA: 0x00073A60 File Offset: 0x00071C60
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

	// Token: 0x060002B0 RID: 688 RVA: 0x00073AC4 File Offset: 0x00071CC4
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

	// Token: 0x060002B1 RID: 689 RVA: 0x00073B1C File Offset: 0x00071D1C
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

	// Token: 0x060002B2 RID: 690 RVA: 0x00073B80 File Offset: 0x00071D80
	private void MoveNewCrittersToCollection()
	{
		foreach (MenagerieSlot menagerieSlot in this.newCritterPen)
		{
			if (menagerieSlot.critter)
			{
				this.AddCritterToCollection(menagerieSlot.critter.CritterData);
				this.DespawnCritterFromSlot(menagerieSlot);
			}
		}
		this._savedCritters.newCritters.Clear();
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00031275 File Offset: 0x0002F475
	private void ClearSlot(MenagerieSlot slot)
	{
		this.DespawnCritterFromSlot(slot);
		if (slot.label)
		{
			slot.label.text = "";
		}
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00073BDC File Offset: 0x00071DDC
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

	// Token: 0x060002B5 RID: 693 RVA: 0x0003129B File Offset: 0x0002F49B
	private void ClearNewCritterPen()
	{
		this._savedCritters.newCritters.Clear();
		this.UpdateNewCritterPen();
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x000312B3 File Offset: 0x0002F4B3
	private void ClearCollection()
	{
		this._savedCritters.collectedCritters.Clear();
		this.UpdateCollection();
		this.UpdateFavoriteCritter();
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x000312D1 File Offset: 0x0002F4D1
	private void ClearAll()
	{
		this._savedCritters.Clear();
		this.UpdateMenagerie();
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x000312E4 File Offset: 0x0002F4E4
	private void ResetSavedCreatures()
	{
		this.ClearAll();
		this.Save();
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00073C48 File Offset: 0x00071E48
	private void Load()
	{
		this.ClearAll();
		string @string = PlayerPrefs.GetString("_SavedCritters", string.Empty);
		this.LoadCrittersFromJson(@string);
		this.UpdateMenagerie();
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00073C78 File Offset: 0x00071E78
	private void Save()
	{
		Debug.Log(string.Format("Saving {0} critters", this._critters.Count));
		string value = this.SaveCrittersToJson();
		PlayerPrefs.SetString("_SavedCritters", value);
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00073CB8 File Offset: 0x00071EB8
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

	// Token: 0x060002BC RID: 700 RVA: 0x00073D14 File Offset: 0x00071F14
	private string SaveCrittersToJson()
	{
		this.ValidateSaveData();
		string text = JsonConvert.SerializeObject(this._savedCritters, Formatting.Indented);
		Debug.Log("Critters save to JSON: " + text);
		return text;
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00073D48 File Offset: 0x00071F48
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

	// Token: 0x060002BE RID: 702 RVA: 0x00073DDC File Offset: 0x00071FDC
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

	// Token: 0x04000350 RID: 848
	[FormerlySerializedAs("creatureIndex")]
	public CritterIndex critterIndex;

	// Token: 0x04000351 RID: 849
	public MenagerieCritter prefab;

	// Token: 0x04000352 RID: 850
	private List<MenagerieCritter> _critters = new List<MenagerieCritter>();

	// Token: 0x04000353 RID: 851
	private Menagerie.CritterSaveData _savedCritters = new Menagerie.CritterSaveData();

	// Token: 0x04000354 RID: 852
	public MenagerieSlot[] collection;

	// Token: 0x04000355 RID: 853
	public MenagerieSlot[] newCritterPen;

	// Token: 0x04000356 RID: 854
	public MenagerieSlot favoriteCritterSlot;

	// Token: 0x04000357 RID: 855
	private int _collectionPageIndex;

	// Token: 0x04000358 RID: 856
	private int _totalPages;

	// Token: 0x04000359 RID: 857
	public MenagerieDepositBox DonationBox;

	// Token: 0x0400035A RID: 858
	public MenagerieDepositBox FavoriteBox;

	// Token: 0x0400035B RID: 859
	public MenagerieDepositBox CollectionBox;

	// Token: 0x0400035C RID: 860
	public TextMeshPro donationCounter;

	// Token: 0x0400035D RID: 861
	public string DonationText = "DONATED:{0}";

	// Token: 0x0400035E RID: 862
	private const string CrittersSavePrefsKey = "_SavedCritters";

	// Token: 0x0200006B RID: 107
	public class CritterData
	{
		// Token: 0x060002C0 RID: 704 RVA: 0x0003131B File Offset: 0x0002F51B
		public CritterConfiguration GetConfiguration()
		{
			return CrittersManager.instance.creatureIndex[this.critterType];
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0002F5E8 File Offset: 0x0002D7E8
		public CritterData()
		{
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00031334 File Offset: 0x0002F534
		public CritterData(CritterConfiguration config, CritterAppearance appearance)
		{
			this.critterType = CrittersManager.instance.creatureIndex.critterTypes.IndexOf(config);
			this.appearance = appearance;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00031360 File Offset: 0x0002F560
		public CritterData(int critterType, CritterAppearance appearance)
		{
			this.critterType = critterType;
			this.appearance = appearance;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00031376 File Offset: 0x0002F576
		public CritterData(CritterVisuals visuals)
		{
			this.critterType = visuals.critterType;
			this.appearance = visuals.Appearance;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00031396 File Offset: 0x0002F596
		public CritterData(Menagerie.CritterData source)
		{
			this.critterType = source.critterType;
			this.appearance = source.appearance;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000313B6 File Offset: 0x0002F5B6
		public override string ToString()
		{
			return string.Format("{0} {1} [instance]", this.critterType, this.appearance);
		}

		// Token: 0x0400035F RID: 863
		public int critterType;

		// Token: 0x04000360 RID: 864
		public CritterAppearance appearance;

		// Token: 0x04000361 RID: 865
		[NonSerialized]
		public MenagerieCritter instance;
	}

	// Token: 0x0200006C RID: 108
	[Serializable]
	public class CritterSaveData
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x000313D8 File Offset: 0x0002F5D8
		public void Clear()
		{
			this.newCritters.Clear();
			this.collectedCritters.Clear();
			this.donatedCritterCount = 0;
			this.favoriteCritter = -1;
		}

		// Token: 0x04000362 RID: 866
		public List<Menagerie.CritterData> newCritters = new List<Menagerie.CritterData>();

		// Token: 0x04000363 RID: 867
		public Dictionary<int, Menagerie.CritterData> collectedCritters = new Dictionary<int, Menagerie.CritterData>();

		// Token: 0x04000364 RID: 868
		public int donatedCritterCount;

		// Token: 0x04000365 RID: 869
		public int favoriteCritter = -1;
	}
}
