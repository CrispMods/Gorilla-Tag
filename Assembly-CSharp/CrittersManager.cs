using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Critters.Scripts;
using Fusion;
using GorillaExtensions;
using GorillaNetworking;
using Photon.Pun;
using PlayFab;
using UnityEngine;

// Token: 0x02000051 RID: 81
[NetworkBehaviourWeaved(0)]
public class CrittersManager : NetworkComponent, IRequestableOwnershipGuardCallbacks, IBuildValidation
{
	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000194 RID: 404 RVA: 0x00031637 File Offset: 0x0002F837
	// (set) Token: 0x06000195 RID: 405 RVA: 0x0003163E File Offset: 0x0002F83E
	public static bool hasInstance { get; private set; }

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000196 RID: 406 RVA: 0x00031646 File Offset: 0x0002F846
	public bool allowGrabbingEntireBag
	{
		get
		{
			if (!NetworkSystem.Instance.SessionIsPrivate)
			{
				return (CrittersManager.AllowGrabbingFlags.EntireBag & this.publicRoomGrabbingFlags) > CrittersManager.AllowGrabbingFlags.None;
			}
			return (CrittersManager.AllowGrabbingFlags.EntireBag & this.privateRoomGrabbingFlags) > CrittersManager.AllowGrabbingFlags.None;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000197 RID: 407 RVA: 0x0003166B File Offset: 0x0002F86B
	public bool allowGrabbingOutOfHands
	{
		get
		{
			if (!NetworkSystem.Instance.SessionIsPrivate)
			{
				return (CrittersManager.AllowGrabbingFlags.OutOfHands & this.publicRoomGrabbingFlags) > CrittersManager.AllowGrabbingFlags.None;
			}
			return (CrittersManager.AllowGrabbingFlags.OutOfHands & this.privateRoomGrabbingFlags) > CrittersManager.AllowGrabbingFlags.None;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000198 RID: 408 RVA: 0x00031690 File Offset: 0x0002F890
	public bool allowGrabbingFromBags
	{
		get
		{
			if (!NetworkSystem.Instance.SessionIsPrivate)
			{
				return (CrittersManager.AllowGrabbingFlags.FromBags & this.publicRoomGrabbingFlags) > CrittersManager.AllowGrabbingFlags.None;
			}
			return (CrittersManager.AllowGrabbingFlags.FromBags & this.privateRoomGrabbingFlags) > CrittersManager.AllowGrabbingFlags.None;
		}
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0006ECAC File Offset: 0x0006CEAC
	public void LoadGrabSettings()
	{
		PlayFabTitleDataCache.Instance.GetTitleData("PublicCrittersGrabSettings", delegate(string data)
		{
			int num;
			if (int.TryParse(data, out num))
			{
				this.publicRoomGrabbingFlags = (CrittersManager.AllowGrabbingFlags)num;
			}
		}, delegate(PlayFabError e)
		{
		});
		PlayFabTitleDataCache.Instance.GetTitleData("PrivateCrittersGrabSettings", delegate(string data)
		{
			int num;
			if (int.TryParse(data, out num))
			{
				this.privateRoomGrabbingFlags = (CrittersManager.AllowGrabbingFlags)num;
			}
		}, delegate(PlayFabError e)
		{
		});
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x0600019A RID: 410 RVA: 0x0006ED30 File Offset: 0x0006CF30
	// (remove) Token: 0x0600019B RID: 411 RVA: 0x0006ED68 File Offset: 0x0006CF68
	public event Action<CrittersManager.CritterEvent, int, Vector3, Quaternion> OnCritterEventReceived;

	// Token: 0x0600019C RID: 412 RVA: 0x0006EDA0 File Offset: 0x0006CFA0
	public bool BuildValidationCheck()
	{
		if (this.guard == null)
		{
			Debug.LogError("requestable owner guard missing", base.gameObject);
			return false;
		}
		if (this.crittersPool == null)
		{
			Debug.LogError("critters pool missing", base.gameObject);
			return false;
		}
		return true;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x000316B5 File Offset: 0x0002F8B5
	protected override void Start()
	{
		base.Start();
		CrittersManager.instance.LoadGrabSettings();
		CrittersManager.CheckInitialize();
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0006EDF0 File Offset: 0x0006CFF0
	public static void InitializeCrittersManager()
	{
		if (CrittersManager.hasInstance)
		{
			return;
		}
		CrittersManager.hasInstance = true;
		CrittersManager.instance = UnityEngine.Object.FindAnyObjectByType<CrittersManager>();
		CrittersManager.instance.crittersActors = new List<CrittersActor>();
		CrittersManager.instance.crittersPawns = new List<CrittersPawn>();
		CrittersManager.instance.awareOfActors = new Dictionary<CrittersPawn, List<CrittersActor>>();
		CrittersManager.instance.despawnableActors = new List<CrittersActor>();
		CrittersManager.instance.newlyDisabledActors = new List<CrittersActor>();
		CrittersManager.instance.rigActorSetups = new List<CrittersRigActorSetup>();
		CrittersManager.instance.rigSetupByRig = new Dictionary<VRRig, CrittersRigActorSetup>();
		CrittersManager.instance.updatesToSend = new List<int>();
		CrittersManager.instance.objList = new List<object>();
		CrittersManager.instance.lowPriorityPawnsToProcess = new List<CrittersActor>();
		CrittersManager.instance.actorSpawners = UnityEngine.Object.FindObjectsByType<CrittersActorSpawner>(FindObjectsSortMode.None).ToList<CrittersActorSpawner>();
		CrittersManager.instance._spawnRegions = CrittersRegion.Regions;
		CrittersManager.instance.poolCounts = new Dictionary<CrittersActor.CrittersActorType, int>();
		CrittersManager.instance.despawnDecayValue = new Dictionary<CrittersActor.CrittersActorType, float>();
		CrittersManager.instance.actorTypes = (CrittersActor.CrittersActorType[])Enum.GetValues(typeof(CrittersActor.CrittersActorType));
		CrittersManager.instance.poolIndexDict = new Dictionary<CrittersActor.CrittersActorType, int>();
		for (int i = 0; i < CrittersManager.instance.actorTypes.Length; i++)
		{
			CrittersManager.instance.poolCounts[CrittersManager.instance.actorTypes[i]] = 0;
			CrittersManager.instance.despawnDecayValue[CrittersManager.instance.actorTypes[i]] = 0f;
		}
		CrittersManager.instance.PopulatePools();
		List<CrittersRigActorSetup> list = UnityEngine.Object.FindObjectsByType<CrittersRigActorSetup>(FindObjectsSortMode.None).ToList<CrittersRigActorSetup>();
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].enabled)
			{
				CrittersManager.RegisterRigActorSetup(list[j]);
			}
		}
		CrittersActorGrabber[] array = UnityEngine.Object.FindObjectsByType<CrittersActorGrabber>(FindObjectsSortMode.None);
		for (int k = 0; k < array.Length; k++)
		{
			if (array[k].isLeft)
			{
				CrittersManager._leftGrabber = array[k];
			}
			else
			{
				CrittersManager._rightGrabber = array[k];
			}
		}
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(CrittersManager.instance.JoinedRoomEvent));
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(CrittersManager.instance.LeftRoomEvent));
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0006F060 File Offset: 0x0006D260
	private void ResetRoom()
	{
		this.lastSpawnTime = 0.0;
		for (int i = 0; i < this.allActors.Count; i++)
		{
			CrittersActor crittersActor = this.allActors[i];
			if (crittersActor.gameObject.activeSelf)
			{
				if (this.persistentActors.Contains(this.allActors[i]))
				{
					this.allActors[i].Initialize();
				}
				else
				{
					crittersActor.gameObject.SetActive(false);
				}
			}
		}
		for (int j = 0; j < this.actorSpawners.Count; j++)
		{
			this.actorSpawners[j].DoReset();
		}
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x000316CE File Offset: 0x0002F8CE
	public void Update()
	{
		this.HandleZonesAndOwnership();
		if (this.localInZone)
		{
			this.ProcessSpawning();
			this.ProcessActorBinLocations();
			this.ProcessRigSetups();
			this.ProcessCritterAwareness();
			this.ProcessDespawningIdles();
			this.ProcessActors();
		}
		this.ProcessNewlyDisabledActors();
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0006F10C File Offset: 0x0006D30C
	public void ProcessRigSetups()
	{
		if (!this.LocalAuthority())
		{
			return;
		}
		this.objList.Clear();
		for (int i = 0; i < this.rigActorSetups.Count; i++)
		{
			this.rigActorSetups[i].CheckUpdate(ref this.objList, false);
		}
		if (this.objList.Count > 0 && NetworkSystem.Instance.InRoom)
		{
			CrittersManager.instance.SendRPC("RemoteUpdatePlayerCrittersActorData", RpcTarget.Others, new object[]
			{
				this.objList.ToArray()
			});
		}
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0006F19C File Offset: 0x0006D39C
	private void ProcessCritterAwareness()
	{
		if (!this.LocalAuthority())
		{
			return;
		}
		int num = 0;
		this.lowPriorityPawnsToProcess.Clear();
		int i = 0;
		while (i < this.crittersPawns.Count)
		{
			CrittersPawn key = this.crittersPawns[i];
			if (!this.awareOfActors.ContainsKey(key))
			{
				this.awareOfActors[key] = new List<CrittersActor>();
			}
			else
			{
				this.awareOfActors[key].Clear();
			}
			this.nearbyActors.Clear();
			int num2 = this.actorBinIndices[key];
			if (this.priorityBins[num2])
			{
				goto IL_D9;
			}
			if (i >= this.lowPriorityIndex && num < this.lowPriorityActorsPerFrame)
			{
				this.lowPriorityPawnsToProcess.Add(this.crittersPawns[i]);
				num++;
				this.lowPriorityIndex++;
				if (this.lowPriorityIndex >= this.crittersPawns.Count)
				{
					this.lowPriorityIndex = 0;
					goto IL_D9;
				}
				goto IL_D9;
			}
			IL_1C4:
			i++;
			continue;
			IL_D9:
			int num3 = Mathf.FloorToInt((float)(num2 / this.binXCount));
			int num4 = num2 % this.binXCount;
			for (int j = -1; j <= 1; j++)
			{
				for (int k = -1; k <= 1; k++)
				{
					if (num3 + j < this.binXCount && num3 + j >= 0 && num4 + k < this.binZCount && num4 + k >= 0)
					{
						this.nearbyActors.AddRange(this.actorBins[num4 + k + (num3 + j) * this.binXCount]);
					}
				}
			}
			for (int l = 0; l < this.nearbyActors.Count; l++)
			{
				if (this.crittersPawns[i].AwareOfActor(this.nearbyActors[l]))
				{
					this.awareOfActors[this.crittersPawns[i]].Add(this.nearbyActors[l]);
				}
			}
			goto IL_1C4;
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0006F384 File Offset: 0x0006D584
	private void ProcessSpawning()
	{
		if (!this.LocalAuthority())
		{
			return;
		}
		if (this.lastSpawnTime + this.spawnDelay <= (NetworkSystem.Instance.InRoom ? PhotonNetwork.Time : ((double)Time.time)))
		{
			int nextSpawnRegion = this.GetNextSpawnRegion();
			if (nextSpawnRegion >= 0)
			{
				this.SpawnCritter(nextSpawnRegion);
			}
			else
			{
				this.lastSpawnTime = (NetworkSystem.Instance.InRoom ? PhotonNetwork.Time : ((double)Time.time));
			}
		}
		if (this.spawnerIndex >= this.actorSpawners.Count)
		{
			this.spawnerIndex = 0;
		}
		if (this.actorSpawners.Count == 0)
		{
			return;
		}
		this.actorSpawners[this.spawnerIndex].ProcessLocal();
		this.spawnerIndex++;
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0006F444 File Offset: 0x0006D644
	private int GetNextSpawnRegion()
	{
		for (int i = 1; i <= this._spawnRegions.Count; i++)
		{
			int num = (this._currentRegionIndex + i) % this._spawnRegions.Count;
			CrittersRegion crittersRegion = this._spawnRegions[num];
			if (crittersRegion.CritterCount < crittersRegion.maxCritters)
			{
				this._currentRegionIndex = num;
				return this._currentRegionIndex;
			}
		}
		return -1;
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0006F4A8 File Offset: 0x0006D6A8
	private void ProcessActorBinLocations()
	{
		if (this.LocalAuthority())
		{
			for (int i = 0; i < this.actorBins.Length; i++)
			{
				this.actorBins[i].Clear();
				this.priorityBins[i] = false;
			}
			for (int j = this.crittersActors.Count - 1; j >= 0; j--)
			{
				CrittersActor crittersActor = this.crittersActors[j];
				if (crittersActor == null)
				{
					this.crittersActors.RemoveAt(j);
				}
				else
				{
					Transform transform = crittersActor.transform;
					int num = Mathf.Clamp(Mathf.FloorToInt((transform.position.x - this.binDimensionXMin) / this.individualBinSide), 0, this.binXCount - 1);
					int num2 = Mathf.Clamp(Mathf.FloorToInt((transform.position.z - this.binDimensionZMin) / this.individualBinSide), 0, this.binZCount - 1);
					int num3 = num + num2 * this.binXCount;
					if (this.actorBinIndices.ContainsKey(crittersActor))
					{
						this.actorBinIndices[crittersActor] = num3;
					}
					else
					{
						this.actorBinIndices.Add(crittersActor, num3);
					}
					this.actorBins[num3].Add(crittersActor);
				}
			}
			for (int k = 0; k < RoomSystem.PlayersInRoom.Count; k++)
			{
				RigContainer rigContainer;
				if (VRRigCache.Instance.TryGetVrrig(RoomSystem.PlayersInRoom[k], out rigContainer))
				{
					Transform transform2 = rigContainer.Rig.transform;
					float num4 = (transform2.position.x - this.binDimensionXMin) / this.individualBinSide;
					float num5 = (transform2.position.z - this.binDimensionZMin) / this.individualBinSide;
					int num6 = Mathf.FloorToInt(num4);
					int num7 = Mathf.FloorToInt(num5);
					int num8 = (num4 % 1f > 0.5f) ? 1 : -1;
					int num9 = (num5 % 1f > 0.5f) ? 1 : -1;
					if (num6 < 0 || num6 >= this.binXCount || num7 < 0 || num7 >= this.binZCount)
					{
						return;
					}
					int num10 = num6 + num7 * this.binXCount;
					this.priorityBins[num10] = true;
					num8 = Mathf.Clamp(num6 + num8, 0, this.binXCount - 1);
					num9 = Mathf.Clamp(num7 + num9, 0, this.binZCount - 1);
					this.priorityBins[num8 + num7 * this.binXCount] = true;
					this.priorityBins[num6 + num9 * this.binXCount] = true;
					this.priorityBins[num8 + num9 * this.binXCount] = true;
				}
			}
		}
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0006F728 File Offset: 0x0006D928
	private void ProcessDespawningIdles()
	{
		for (int i = 0; i < this.actorTypes.Length; i++)
		{
			this.despawnDecayValue[this.actorTypes[i]] = Mathf.Lerp(this.despawnDecayValue[this.actorTypes[i]], (float)this.despawnThreshold, 1f - Mathf.Exp(-this.decayRate * (Time.realtimeSinceStartup - Time.deltaTime)));
		}
		if (!this.LocalAuthority())
		{
			return;
		}
		if (this.despawnableActors.Count == 0)
		{
			return;
		}
		int j = 0;
		while (j <= this.lowPriorityActorsPerFrame)
		{
			this.despawnIndex++;
			if (this.despawnIndex >= this.despawnableActors.Count)
			{
				this.despawnIndex = 0;
			}
			j++;
			CrittersActor crittersActor = this.despawnableActors[this.despawnIndex];
			if (this.despawnDecayValue[crittersActor.crittersActorType] >= (float)this.despawnThreshold && crittersActor.ShouldDespawn())
			{
				this.DespawnActor(crittersActor);
			}
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0006F824 File Offset: 0x0006DA24
	public void DespawnActor(CrittersActor actor)
	{
		int actorId = actor.actorId;
		if (!this.updatesToSend.Contains(actorId))
		{
			this.updatesToSend.Add(actorId);
		}
		actor.gameObject.SetActive(false);
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0006F860 File Offset: 0x0006DA60
	public void IncrementPoolCount(CrittersActor.CrittersActorType type)
	{
		int num;
		if (!this.poolCounts.TryGetValue(type, out num))
		{
			this.poolCounts[type] = 1;
		}
		else
		{
			this.poolCounts[type] = this.poolCounts[type] + 1;
		}
		float num2;
		if (!this.despawnDecayValue.TryGetValue(type, out num2))
		{
			this.despawnDecayValue[type] = 1f;
			return;
		}
		this.despawnDecayValue[type] = this.despawnDecayValue[type] + 1f;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0006F8E8 File Offset: 0x0006DAE8
	public void DecrementPoolCount(CrittersActor.CrittersActorType type)
	{
		int num;
		if (this.poolCounts.TryGetValue(type, out num))
		{
			this.poolCounts[type] = Mathf.Max(0, num - 1);
			return;
		}
		this.poolCounts[type] = 0;
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0006F928 File Offset: 0x0006DB28
	private void ProcessActors()
	{
		if (this.LocalAuthority())
		{
			for (int i = this.crittersActors.Count - 1; i >= 0; i--)
			{
				if (this.crittersActors[i].crittersActorType != CrittersActor.CrittersActorType.Creature || this.priorityBins[this.actorBinIndices[this.crittersActors[i]]] || this.lowPriorityPawnsToProcess.Contains(this.crittersActors[i]))
				{
					int actorId = this.crittersActors[i].actorId;
					if (this.crittersActors[i].ProcessLocal() && !this.updatesToSend.Contains(actorId))
					{
						this.updatesToSend.Add(actorId);
					}
				}
			}
			return;
		}
		for (int j = 0; j < this.crittersActors.Count; j++)
		{
			this.crittersActors[j].ProcessRemote();
		}
	}

	// Token: 0x060001AB RID: 427 RVA: 0x0006FA14 File Offset: 0x0006DC14
	private void ProcessNewlyDisabledActors()
	{
		for (int i = 0; i < this.newlyDisabledActors.Count; i++)
		{
			CrittersActor crittersActor = this.newlyDisabledActors[i];
			if (CrittersManager.instance.crittersActors.Contains(crittersActor))
			{
				CrittersManager.instance.crittersActors.Remove(crittersActor);
			}
			if (crittersActor.despawnWhenIdle && CrittersManager.instance.despawnableActors.Contains(crittersActor))
			{
				CrittersManager.instance.despawnableActors.Remove(crittersActor);
			}
			CrittersManager.instance.DecrementPoolCount(crittersActor.crittersActorType);
			crittersActor.SetTransformToDefaultParent(true);
		}
		this.newlyDisabledActors.Clear();
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00031708 File Offset: 0x0002F908
	public static void RegisterCritter(CrittersPawn crittersPawn)
	{
		CrittersManager.CheckInitialize();
		if (!CrittersManager.instance.crittersPawns.Contains(crittersPawn))
		{
			CrittersManager.instance.crittersPawns.Add(crittersPawn);
		}
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0006FAC4 File Offset: 0x0006DCC4
	public static void RegisterRigActorSetup(CrittersRigActorSetup setup)
	{
		CrittersManager.CheckInitialize();
		if (!CrittersManager.instance.rigActorSetups.Contains(setup))
		{
			CrittersManager.instance.rigActorSetups.Add(setup);
		}
		CrittersManager.instance.rigSetupByRig.AddOrUpdate(setup.myRig, setup);
	}

	// Token: 0x060001AE RID: 430 RVA: 0x00031735 File Offset: 0x0002F935
	public static void DeregisterCritter(CrittersPawn crittersPawn)
	{
		CrittersManager.CheckInitialize();
		CrittersManager.instance.SetCritterRegion(crittersPawn, 0);
		if (CrittersManager.instance.crittersPawns.Contains(crittersPawn))
		{
			CrittersManager.instance.crittersPawns.Remove(crittersPawn);
		}
	}

	// Token: 0x060001AF RID: 431 RVA: 0x0006FB14 File Offset: 0x0006DD14
	public static void RegisterActor(CrittersActor actor)
	{
		CrittersManager.CheckInitialize();
		if (!CrittersManager.instance.crittersActors.Contains(actor))
		{
			CrittersManager.instance.crittersActors.Add(actor);
		}
		if (actor.despawnWhenIdle && !CrittersManager.instance.despawnableActors.Contains(actor))
		{
			CrittersManager.instance.despawnableActors.Add(actor);
		}
		if (CrittersManager.instance.newlyDisabledActors.Contains(actor))
		{
			CrittersManager.instance.newlyDisabledActors.Remove(actor);
		}
		CrittersManager.instance.IncrementPoolCount(actor.crittersActorType);
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00031771 File Offset: 0x0002F971
	public static void DeregisterActor(CrittersActor actor)
	{
		CrittersManager.CheckInitialize();
		if (!CrittersManager.instance.newlyDisabledActors.Contains(actor))
		{
			CrittersManager.instance.newlyDisabledActors.Add(actor);
		}
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0003179E File Offset: 0x0002F99E
	public static void CheckInitialize()
	{
		if (!CrittersManager.hasInstance)
		{
			CrittersManager.InitializeCrittersManager();
		}
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x000317AC File Offset: 0x0002F9AC
	public static bool CritterAwareOfAny(CrittersPawn creature)
	{
		return CrittersManager.instance.awareOfActors[creature].Count > 0;
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0006FBB4 File Offset: 0x0006DDB4
	public static bool AnyFoodNearby(CrittersPawn creature)
	{
		List<CrittersActor> list = CrittersManager.instance.awareOfActors[creature];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].crittersActorType == CrittersActor.CrittersActorType.Food)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x0006FBF8 File Offset: 0x0006DDF8
	public static CrittersFood ClosestFood(CrittersPawn creature)
	{
		float num = float.MaxValue;
		CrittersFood result = null;
		List<CrittersActor> list = CrittersManager.instance.awareOfActors[creature];
		for (int i = 0; i < list.Count; i++)
		{
			CrittersActor crittersActor = list[i];
			if (crittersActor.crittersActorType == CrittersActor.CrittersActorType.Food)
			{
				CrittersFood crittersFood = (CrittersFood)crittersActor;
				float sqrMagnitude = (creature.transform.position - crittersFood.food.transform.position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					result = crittersFood;
					num = sqrMagnitude;
				}
			}
		}
		return result;
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x000317C8 File Offset: 0x0002F9C8
	public static void PlayHaptics(AudioClip clip, float strength, bool isLeftHand)
	{
		(isLeftHand ? CrittersManager._leftGrabber : CrittersManager._rightGrabber).PlayHaptics(clip, strength);
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x000317E0 File Offset: 0x0002F9E0
	public static void StopHaptics(bool isLeftHand)
	{
		(isLeftHand ? CrittersManager._leftGrabber : CrittersManager._rightGrabber).StopHaptics();
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0006FC88 File Offset: 0x0006DE88
	public CrittersPawn SpawnCritter(int regionIndex = -1)
	{
		CrittersRegion crittersRegion = (regionIndex >= 0 && regionIndex < this._spawnRegions.Count) ? this._spawnRegions[regionIndex] : null;
		int randomCritterType = this.creatureIndex.GetRandomCritterType(crittersRegion);
		if (randomCritterType < 0)
		{
			return null;
		}
		Vector3 position = crittersRegion ? crittersRegion.GetSpawnPoint() : this._spawnRegions[0].transform.position;
		Quaternion rotation = Quaternion.Euler(0f, (float)UnityEngine.Random.Range(0, 360), 0f);
		CrittersPawn crittersPawn = this.SpawnCritter(randomCritterType, position, rotation);
		this.SetCritterRegion(crittersPawn, crittersRegion);
		this.lastSpawnTime = (NetworkSystem.Instance.InRoom ? PhotonNetwork.Time : ((double)Time.time));
		return crittersPawn;
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0006FD44 File Offset: 0x0006DF44
	public CrittersPawn SpawnCritter(int critterType, Vector3 position, Quaternion rotation)
	{
		CrittersPawn crittersPawn = (CrittersPawn)this.SpawnActor(CrittersActor.CrittersActorType.Creature, -1);
		crittersPawn.SetTemplate(critterType);
		crittersPawn.currentState = CrittersPawn.CreatureState.Idle;
		crittersPawn.MoveActor(position, Quaternion.Euler(0f, (float)UnityEngine.Random.Range(0, 360), 0f), false, true, true);
		crittersPawn.SetImpulseVelocity(Vector3.zero, Vector3.zero);
		crittersPawn.SetState(CrittersPawn.CreatureState.Spawning);
		if (NetworkSystem.Instance.InRoom && this.LocalAuthority())
		{
			base.SendRPC("RemoteSpawnCreature", RpcTarget.Others, new object[]
			{
				crittersPawn.actorId,
				crittersPawn.regionId,
				crittersPawn.visuals.Appearance.WriteToRPCData()
			});
		}
		return crittersPawn;
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x000317F6 File Offset: 0x0002F9F6
	public void DespawnCritter(CrittersPawn crittersPawn)
	{
		this.DeactivateActor(crittersPawn);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0006FE04 File Offset: 0x0006E004
	public void QueueDespawnAllCritters()
	{
		if (!this.LocalAuthority())
		{
			return;
		}
		foreach (CrittersPawn crittersPawn in this.crittersPawns)
		{
			crittersPawn.SetState(CrittersPawn.CreatureState.Despawning);
		}
	}

	// Token: 0x060001BB RID: 443 RVA: 0x000317FF File Offset: 0x0002F9FF
	private void SetCritterRegion(CrittersPawn critter, CrittersRegion region)
	{
		this.SetCritterRegion(critter, region ? region.ID : 0);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00031819 File Offset: 0x0002FA19
	private void SetCritterRegion(CrittersPawn critter, int regionId)
	{
		if (critter.regionId != 0)
		{
			CrittersRegion.RemoveCritterFromRegion(critter);
		}
		if (regionId != 0)
		{
			CrittersRegion.AddCritterToRegion(critter, regionId);
		}
		critter.regionId = regionId;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0003183A File Offset: 0x0002FA3A
	public void DeactivateActor(CrittersActor actor)
	{
		actor.gameObject.SetActive(false);
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0006FE60 File Offset: 0x0006E060
	private void CamCapture()
	{
		Camera component = base.GetComponent<Camera>();
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = component.targetTexture;
		component.Render();
		Texture2D texture2D = new Texture2D(component.targetTexture.width, component.targetTexture.height);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)component.targetTexture.width, (float)component.targetTexture.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		texture2D.EncodeToPNG();
		UnityEngine.Object.Destroy(texture2D);
	}

	// Token: 0x060001BF RID: 447 RVA: 0x00031848 File Offset: 0x0002FA48
	private IEnumerator RemoteDataInitialization(NetPlayer player, int actorNumber)
	{
		List<object> nonPlayerActorObjList = new List<object>();
		List<object> playerActorObjList = new List<object>();
		int worldActorDataCount = 0;
		int playerActorDataCount = 0;
		int num;
		for (int i = 0; i < this.allActors.Count; i = num + 1)
		{
			if (!NetworkSystem.Instance.InRoom || !this.LocalAuthority())
			{
				this.RemoveInitializingPlayer(actorNumber);
				yield break;
			}
			if (this.allActors[i].isOnPlayer)
			{
				num = playerActorDataCount;
				playerActorDataCount = num + 1;
				this.allActors[i].AddPlayerCrittersActorDataToList(ref playerActorObjList);
			}
			if (playerActorDataCount >= this.actorsPerInitializationCall || (i == this.allActors.Count - 1 && playerActorDataCount > 0))
			{
				if (!player.InRoom || player.ActorNumber != actorNumber)
				{
					this.RemoveInitializingPlayer(actorNumber);
					yield break;
				}
				if (NetworkSystem.Instance.InRoom && this.LocalAuthority())
				{
					base.SendRPC("RemoteUpdatePlayerCrittersActorData", player, new object[]
					{
						playerActorObjList.ToArray()
					});
				}
				playerActorObjList.Clear();
				playerActorDataCount = 0;
				yield return new WaitForSeconds(this.actorsInitializationCallCooldown);
			}
			num = i;
		}
		if (!player.InRoom || player.ActorNumber != actorNumber)
		{
			this.RemoveInitializingPlayer(actorNumber);
			yield break;
		}
		if (NetworkSystem.Instance.InRoom && this.LocalAuthority() && playerActorDataCount > 0)
		{
			base.SendRPC("RemoteUpdatePlayerCrittersActorData", player, new object[]
			{
				playerActorObjList.ToArray()
			});
		}
		for (int i = 0; i < this.allActors.Count; i = num + 1)
		{
			if (!player.InRoom || player.ActorNumber != actorNumber)
			{
				this.RemoveInitializingPlayer(actorNumber);
				yield break;
			}
			if (!NetworkSystem.Instance.InRoom || !this.LocalAuthority())
			{
				this.RemoveInitializingPlayer(actorNumber);
				yield break;
			}
			CrittersActor crittersActor = this.allActors[i];
			if (crittersActor.gameObject.activeSelf)
			{
				num = worldActorDataCount;
				worldActorDataCount = num + 1;
				if (crittersActor.parentActorId == -1)
				{
					crittersActor.UpdateImpulses(false, false);
					crittersActor.UpdateImpulseVelocity();
				}
				crittersActor.AddActorDataToList(ref nonPlayerActorObjList);
				if (worldActorDataCount >= this.actorsPerInitializationCall)
				{
					if (!player.InRoom || player.ActorNumber != actorNumber)
					{
						this.RemoveInitializingPlayer(actorNumber);
						yield break;
					}
					if (!NetworkSystem.Instance.InRoom || !this.LocalAuthority())
					{
						this.RemoveInitializingPlayer(actorNumber);
						yield break;
					}
					base.SendRPC("RemoteUpdateCritterData", player, new object[]
					{
						nonPlayerActorObjList.ToArray()
					});
					nonPlayerActorObjList.Clear();
					worldActorDataCount = 0;
					yield return new WaitForSeconds(this.actorsInitializationCallCooldown);
				}
			}
			num = i;
		}
		if (NetworkSystem.Instance.InRoom && this.LocalAuthority() && worldActorDataCount > 0)
		{
			base.SendRPC("RemoteUpdateCritterData", player, new object[]
			{
				nonPlayerActorObjList.ToArray()
			});
		}
		this.RemoveInitializingPlayer(actorNumber);
		yield break;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00031865 File Offset: 0x0002FA65
	private IEnumerator DelayedInitialization(NetPlayer player, List<object> nonPlayerActorObjList)
	{
		yield return new WaitForSeconds(30f);
		base.SendRPC("RemoteUpdateCritterData", player, new object[]
		{
			nonPlayerActorObjList.ToArray()
		});
		yield break;
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00031882 File Offset: 0x0002FA82
	public void RemoveInitializingPlayer(int actorNumber)
	{
		if (this.updatingPlayers.Contains(actorNumber))
		{
			this.updatingPlayers.Remove(actorNumber);
		}
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0003189F File Offset: 0x0002FA9F
	private void JoinedRoomEvent()
	{
		if (this.localInZone && !this.LocalAuthority())
		{
			this.ResetRoom();
		}
		this.hasNewlyInitialized = false;
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x000318BE File Offset: 0x0002FABE
	private void LeftRoomEvent()
	{
		this.guard.TransferOwnership(NetworkSystem.Instance.LocalPlayer, "");
		this.ResetRoom();
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0006FEF0 File Offset: 0x0006E0F0
	[PunRPC]
	public void RequestDataInitialization(PhotonMessageInfo info)
	{
		if (!NetworkSystem.Instance.InRoom || !this.LocalAuthority())
		{
			return;
		}
		if (this.updatingPlayers == null)
		{
			this.updatingPlayers = new List<int>();
		}
		if (this.updatingPlayers.Contains(info.Sender.ActorNumber))
		{
			return;
		}
		this.updatingPlayers.Add(info.Sender.ActorNumber);
		base.StartCoroutine(this.RemoteDataInitialization(info.Sender, info.Sender.ActorNumber));
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0006FF78 File Offset: 0x0006E178
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.SenderIsOwner(info))
		{
			this.OwnerSentError(info);
			return;
		}
		if (!this.localInZone)
		{
			return;
		}
		int num;
		if (!CrittersManager.ValidateDataType<int>(stream.ReceiveNext(), out num))
		{
			return;
		}
		if (num > this.actorsPerInitializationCall)
		{
			return;
		}
		int num2 = 0;
		while (num2 < num && this.UpdateActorByType(stream))
		{
			num2++;
		}
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0006FFD0 File Offset: 0x0006E1D0
	public bool UpdateActorByType(PhotonStream stream)
	{
		int num;
		CrittersActor crittersActor;
		return CrittersManager.ValidateDataType<int>(stream.ReceiveNext(), out num) && num >= 0 && num < this.universalActorId && this.actorById.TryGetValue(num, out crittersActor) && crittersActor.UpdateSpecificActor(stream);
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00070018 File Offset: 0x0006E218
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!ZoneManagement.IsInZone(GTZone.critters))
		{
			return;
		}
		int num = Mathf.Min(this.updatesToSend.Count, this.actorsPerInitializationCall);
		stream.SendNext(num);
		for (int i = 0; i < num; i++)
		{
			this.allActors[this.updatesToSend[i]].SendDataByCrittersActorType(stream);
		}
		this.updatesToSend.RemoveRange(0, num);
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x00070088 File Offset: 0x0006E288
	[PunRPC]
	public void RemoteCritterActorReleased(int releasedActorID, bool keepWorldPosition, Quaternion rotation, Vector3 position, Vector3 velocity, Vector3 angularVelocity, PhotonMessageInfo info)
	{
		if (!this.LocalAuthority())
		{
			return;
		}
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
		{
			return;
		}
		if (rotation.IsValid())
		{
			float num = 10000f;
			if (position.IsValid(num))
			{
				float num2 = 10000f;
				if (velocity.IsValid(num2))
				{
					float num3 = 10000f;
					if (angularVelocity.IsValid(num3))
					{
						this.CheckValidRemoteActorRelease(releasedActorID, keepWorldPosition, rotation, position, velocity, angularVelocity, info);
						return;
					}
				}
			}
		}
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x00070104 File Offset: 0x0006E304
	[PunRPC]
	public void RemoteSpawnCreature(int actorID, int regionId, object[] spawnData, PhotonMessageInfo info)
	{
		if (!this.SenderIsOwner(info))
		{
			this.OwnerSentError(info);
			return;
		}
		if (!this.localInZone)
		{
			return;
		}
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
		{
			return;
		}
		if (!CritterAppearance.ValidateData(spawnData))
		{
			return;
		}
		CrittersActor crittersActor;
		if (this.actorById.TryGetValue(actorID, out crittersActor))
		{
			CrittersPawn crittersPawn = (CrittersPawn)crittersActor;
			this.SetCritterRegion(crittersPawn, regionId);
			crittersPawn.SetSpawnData(spawnData);
		}
	}

	// Token: 0x060001CA RID: 458 RVA: 0x00070174 File Offset: 0x0006E374
	[PunRPC]
	public void RemoteCrittersActorGrabbedby(int grabbedActorID, int grabberActorID, Quaternion offsetRotation, Vector3 offsetPosition, bool isGrabDisabled, PhotonMessageInfo info)
	{
		if (!this.LocalAuthority())
		{
			return;
		}
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
		{
			return;
		}
		if (offsetRotation.IsValid())
		{
			float num = 10000f;
			if (offsetPosition.IsValid(num))
			{
				this.CheckValidRemoteActorGrab(grabbedActorID, grabberActorID, offsetRotation, offsetPosition, isGrabDisabled, info);
				return;
			}
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x000701CC File Offset: 0x0006E3CC
	[PunRPC]
	public void RemoteUpdatePlayerCrittersActorData(object[] data, PhotonMessageInfo info)
	{
		if (!this.SenderIsOwner(info))
		{
			this.OwnerSentError(info);
			return;
		}
		if (!this.localInZone)
		{
			return;
		}
		if (data == null)
		{
			return;
		}
		CrittersActor crittersActor;
		for (int i = 0; i < data.Length; i += crittersActor.UpdatePlayerCrittersActorFromRPC(data, i))
		{
			int key;
			if (!CrittersManager.ValidateDataType<int>(data[i], out key))
			{
				return;
			}
			if (!this.actorById.TryGetValue(key, out crittersActor))
			{
				return;
			}
		}
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0007022C File Offset: 0x0006E42C
	[PunRPC]
	public void RemoteUpdateCritterData(object[] data, PhotonMessageInfo info)
	{
		if (!this.SenderIsOwner(info))
		{
			this.OwnerSentError(info);
			return;
		}
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
		{
			return;
		}
		if (!this.localInZone)
		{
			return;
		}
		if (data == null)
		{
			return;
		}
		CrittersActor crittersActor;
		for (int i = 0; i < data.Length; i += crittersActor.UpdateFromRPC(data, i))
		{
			int key;
			if (!CrittersManager.ValidateDataType<int>(data[i], out key))
			{
				return;
			}
			if (!this.actorById.TryGetValue(key, out crittersActor))
			{
				return;
			}
		}
	}

	// Token: 0x060001CD RID: 461 RVA: 0x000702A0 File Offset: 0x0006E4A0
	public CrittersActor SpawnActor(CrittersActor.CrittersActorType type, int subObjectIndex = -1)
	{
		List<CrittersActor> list;
		if (!this.actorPools.TryGetValue(type, out list))
		{
			return null;
		}
		int num = this.poolIndexDict[type];
		for (int i = 0; i < list.Count; i++)
		{
			if (!list[(i + num) % list.Count].gameObject.activeSelf)
			{
				num = (i + num) % list.Count;
				this.poolIndexDict[type] = num + 1;
				list[num].subObjectIndex = subObjectIndex;
				list[num].gameObject.SetActive(true);
				return list[num];
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			CrittersActor key = list[j];
			int num2 = this.actorBinIndices[key];
			if (!this.priorityBins[num2])
			{
				list[j].gameObject.SetActive(false);
				list[j].subObjectIndex = subObjectIndex;
				list[j].gameObject.SetActive(true);
				return list[j];
			}
		}
		return null;
	}

	// Token: 0x060001CE RID: 462 RVA: 0x00030607 File Offset: 0x0002E807
	public override void WriteDataFusion()
	{
	}

	// Token: 0x060001CF RID: 463 RVA: 0x00030607 File Offset: 0x0002E807
	public override void ReadDataFusion()
	{
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x000703A8 File Offset: 0x0006E5A8
	public void PopulatePools()
	{
		this.binDimensionXMin = this.crittersRange.position.x - this.crittersRange.localScale.x / 2f;
		this.binDimensionZMin = this.crittersRange.position.z - this.crittersRange.localScale.z / 2f;
		this.xLength = this.crittersRange.localScale.x;
		this.zLength = this.crittersRange.localScale.z;
		float f = this.xLength * this.zLength / (float)this.totalBinsApproximate;
		this.individualBinSide = Mathf.Sqrt(f);
		this.binXCount = Mathf.CeilToInt(this.xLength / this.individualBinSide);
		this.binZCount = Mathf.CeilToInt(this.zLength / this.individualBinSide);
		int num = this.binXCount * this.binZCount;
		this.actorBins = new List<CrittersActor>[num];
		for (int i = 0; i < num; i++)
		{
			this.actorBins[i] = new List<CrittersActor>();
		}
		this.priorityBins = new bool[num];
		this.actorBinIndices = new Dictionary<CrittersActor, int>();
		this.nearbyActors = new List<CrittersActor>();
		this.allActors = new List<CrittersActor>();
		this.actorPools = new Dictionary<CrittersActor.CrittersActorType, List<CrittersActor>>();
		this.actorPools.Add(CrittersActor.CrittersActorType.Bag, new List<CrittersActor>());
		this.actorPools.Add(CrittersActor.CrittersActorType.Cage, new List<CrittersActor>());
		this.actorPools.Add(CrittersActor.CrittersActorType.Food, new List<CrittersActor>());
		this.actorPools.Add(CrittersActor.CrittersActorType.Creature, new List<CrittersActor>());
		this.actorPools.Add(CrittersActor.CrittersActorType.LoudNoise, new List<CrittersActor>());
		this.actorPools.Add(CrittersActor.CrittersActorType.Grabber, new List<CrittersActor>());
		this.actorPools.Add(CrittersActor.CrittersActorType.FoodSpawner, new List<CrittersActor>());
		this.actorPools.Add(CrittersActor.CrittersActorType.AttachPoint, new List<CrittersActor>());
		this.actorPools.Add(CrittersActor.CrittersActorType.StunBomb, new List<CrittersActor>());
		this.actorPools.Add(CrittersActor.CrittersActorType.BodyAttachPoint, new List<CrittersActor>());
		this.actorPools.Add(CrittersActor.CrittersActorType.NoiseMaker, new List<CrittersActor>());
		this.actorPools.Add(CrittersActor.CrittersActorType.StickyTrap, new List<CrittersActor>());
		this.actorPools.Add(CrittersActor.CrittersActorType.StickyGoo, new List<CrittersActor>());
		this.actorById = new Dictionary<int, CrittersActor>();
		this.universalActorId = 0;
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = base.transform;
		this.poolParent = gameObject.transform;
		this.poolParent.name = "Critter Actors Pool Parent";
		List<CrittersActor> list;
		this.actorPools.TryGetValue(CrittersActor.CrittersActorType.Food, out list);
		this.persistentActors = UnityEngine.Object.FindObjectsByType<CrittersActor>(FindObjectsSortMode.InstanceID).ToList<CrittersActor>();
		this.persistentActors.Sort((CrittersActor x, CrittersActor y) => x.transform.position.magnitude.CompareTo(y.transform.position.magnitude));
		this.persistentActors.Sort((CrittersActor x, CrittersActor y) => x.gameObject.name.CompareTo(y.gameObject.name));
		this.UpdatePool<CrittersActor>(ref this.actorPools, this.bagPrefab, CrittersActor.CrittersActorType.Bag, gameObject.transform, 80, this.persistentActors);
		this.UpdatePool<CrittersActor>(ref this.actorPools, this.cagePrefab, CrittersActor.CrittersActorType.Cage, gameObject.transform, this.poolCount, this.persistentActors);
		this.UpdatePool<CrittersActor>(ref this.actorPools, this.foodPrefab, CrittersActor.CrittersActorType.Food, gameObject.transform, this.poolCount, this.persistentActors);
		this.UpdatePool<CrittersActor>(ref this.actorPools, this.creaturePrefab, CrittersActor.CrittersActorType.Creature, gameObject.transform, this.poolCount, this.persistentActors);
		this.UpdatePool<CrittersActor>(ref this.actorPools, this.noisePrefab, CrittersActor.CrittersActorType.LoudNoise, gameObject.transform, this.poolCount, this.persistentActors);
		this.UpdatePool<CrittersActor>(ref this.actorPools, this.grabberPrefab, CrittersActor.CrittersActorType.Grabber, gameObject.transform, this.poolCount, this.persistentActors);
		this.UpdatePool<CrittersActor>(ref this.actorPools, this.foodSpawnerPrefab, CrittersActor.CrittersActorType.FoodSpawner, gameObject.transform, this.poolCount, this.persistentActors);
		this.UpdatePool<CrittersActor>(ref this.actorPools, this.bodyAttachPointPrefab, CrittersActor.CrittersActorType.BodyAttachPoint, gameObject.transform, 40, this.persistentActors);
		this.UpdatePool<CrittersActor>(ref this.actorPools, null, CrittersActor.CrittersActorType.AttachPoint, gameObject.transform, 0, this.persistentActors);
		this.UpdatePool<CrittersActor>(ref this.actorPools, this.stunBombPrefab, CrittersActor.CrittersActorType.StunBomb, gameObject.transform, this.poolCount, this.persistentActors);
		this.UpdatePool<CrittersActor>(ref this.actorPools, this.noiseMakerPrefab, CrittersActor.CrittersActorType.NoiseMaker, gameObject.transform, this.poolCount, this.persistentActors);
		this.UpdatePool<CrittersActor>(ref this.actorPools, this.stickyTrapPrefab, CrittersActor.CrittersActorType.StickyTrap, gameObject.transform, this.poolCount, this.persistentActors);
		this.UpdatePool<CrittersActor>(ref this.actorPools, this.stickyGooPrefab, CrittersActor.CrittersActorType.StickyGoo, gameObject.transform, this.poolCount, this.persistentActors);
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00070878 File Offset: 0x0006EA78
	public void UpdatePool<T>(ref Dictionary<CrittersActor.CrittersActorType, List<T>> dict, GameObject prefab, CrittersActor.CrittersActorType crittersActorType, Transform parent, int poolAmount, List<CrittersActor> sceneActors) where T : CrittersActor
	{
		int num = 0;
		for (int i = 0; i < sceneActors.Count; i++)
		{
			if (sceneActors[i].crittersActorType == crittersActorType)
			{
				dict[crittersActorType].Add((T)((object)sceneActors[i]));
				sceneActors[i].actorId = this.universalActorId;
				this.actorById.Add(this.universalActorId, sceneActors[i]);
				this.allActors.Add(sceneActors[i]);
				this.universalActorId++;
				num++;
				if (sceneActors[i].enabled)
				{
					if (crittersActorType == CrittersActor.CrittersActorType.Creature)
					{
						CrittersManager.RegisterCritter(sceneActors[i] as CrittersPawn);
					}
					else
					{
						CrittersManager.RegisterActor(sceneActors[i]);
					}
				}
			}
		}
		for (int j = 0; j < poolAmount - num; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
			gameObject.transform.parent = parent;
			gameObject.name += j.ToString();
			gameObject.SetActive(false);
			T component = gameObject.GetComponent<T>();
			dict[crittersActorType].Add(component);
			component.actorId = this.universalActorId;
			component.SetDefaultParent(parent);
			this.actorById.Add(this.universalActorId, component);
			this.allActors.Add(component);
			this.universalActorId++;
		}
		this.poolIndexDict[crittersActorType] = 0;
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00070A0C File Offset: 0x0006EC0C
	public void TriggerEvent(CrittersManager.CritterEvent eventType, int sourceActor, Vector3 position, Quaternion rotation)
	{
		Action<CrittersManager.CritterEvent, int, Vector3, Quaternion> onCritterEventReceived = this.OnCritterEventReceived;
		if (onCritterEventReceived != null)
		{
			onCritterEventReceived(eventType, sourceActor, position, rotation);
		}
		if (!this.LocalAuthority() || !NetworkSystem.Instance.InRoom)
		{
			return;
		}
		base.SendRPC("RemoteReceivedCritterEvent", RpcTarget.Others, new object[]
		{
			eventType,
			sourceActor,
			position,
			rotation
		});
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x000318E0 File Offset: 0x0002FAE0
	public void TriggerEvent(CrittersManager.CritterEvent eventType, int sourceActor, Vector3 position)
	{
		this.TriggerEvent(eventType, sourceActor, position, Quaternion.identity);
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00070A7C File Offset: 0x0006EC7C
	[PunRPC]
	public void RemoteReceivedCritterEvent(CrittersManager.CritterEvent eventType, int sourceActor, Vector3 position, Quaternion rotation, PhotonMessageInfo info)
	{
		if (!this.localInZone)
		{
			return;
		}
		if (!this.SenderIsOwner(info))
		{
			this.OwnerSentError(info);
			return;
		}
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
		{
			return;
		}
		float num = 10000f;
		if (!position.IsValid(num) || !rotation.IsValid())
		{
			return;
		}
		if (!this.critterEventCallLimit.CheckCallTime(Time.time))
		{
			return;
		}
		Action<CrittersManager.CritterEvent, int, Vector3, Quaternion> onCritterEventReceived = this.OnCritterEventReceived;
		if (onCritterEventReceived == null)
		{
			return;
		}
		onCritterEventReceived(eventType, sourceActor, position, rotation);
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x000318F0 File Offset: 0x0002FAF0
	public static bool ValidateDataType<T>(object obj, out T dataAsType)
	{
		if (obj is T)
		{
			dataAsType = (T)((object)obj);
			return true;
		}
		dataAsType = default(T);
		return false;
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00070B00 File Offset: 0x0006ED00
	public void CheckValidRemoteActorRelease(int releasedActorID, bool keepWorldPosition, Quaternion rotation, Vector3 position, Vector3 velocity, Vector3 angularVelocity, PhotonMessageInfo info)
	{
		CrittersActor crittersActor;
		if (!this.actorById.TryGetValue(releasedActorID, out crittersActor))
		{
			return;
		}
		CrittersActor crittersActor2 = this.TopLevelCritterGrabber(crittersActor);
		ref rotation.SetValueSafe(rotation);
		ref position.SetValueSafe(position);
		ref velocity.SetValueSafe(velocity);
		ref angularVelocity.SetValueSafe(angularVelocity);
		if (crittersActor2 != null && crittersActor2 is CrittersGrabber && crittersActor2.isOnPlayer && crittersActor2.rigPlayerId == info.Sender.ActorNumber)
		{
			crittersActor.Released(keepWorldPosition, rotation, position, velocity, angularVelocity);
		}
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00070B88 File Offset: 0x0006ED88
	private void CheckValidRemoteActorGrab(int actorBeingGrabbedActorID, int grabbingActorID, Quaternion offsetRotation, Vector3 offsetPosition, bool isGrabDisabled, PhotonMessageInfo info)
	{
		CrittersActor crittersActor;
		CrittersActor crittersActor2;
		if (!this.actorById.TryGetValue(actorBeingGrabbedActorID, out crittersActor) || !this.actorById.TryGetValue(grabbingActorID, out crittersActor2))
		{
			return;
		}
		ref offsetRotation.SetValueSafe(offsetRotation);
		ref offsetPosition.SetValueSafe(offsetPosition);
		if ((crittersActor.transform.position - crittersActor2.transform.position).magnitude > this.maxGrabDistance || offsetPosition.magnitude > this.maxGrabDistance)
		{
			return;
		}
		if (((crittersActor2.crittersActorType == CrittersActor.CrittersActorType.Grabber && crittersActor2.isOnPlayer && crittersActor2.rigPlayerId == info.Sender.ActorNumber) || crittersActor2.crittersActorType != CrittersActor.CrittersActorType.Grabber) && crittersActor.AllowGrabbingActor(crittersActor2))
		{
			crittersActor.GrabbedBy(crittersActor2, true, offsetRotation, offsetPosition, isGrabDisabled);
		}
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00070C4C File Offset: 0x0006EE4C
	private CrittersActor TopLevelCritterGrabber(CrittersActor baseActor)
	{
		CrittersActor crittersActor = null;
		this.actorById.TryGetValue(baseActor.parentActorId, out crittersActor);
		while (crittersActor != null && crittersActor.parentActorId != -1)
		{
			this.actorById.TryGetValue(crittersActor.parentActorId, out crittersActor);
		}
		return crittersActor;
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x00070C98 File Offset: 0x0006EE98
	public static CapsuleCollider DuplicateCapsuleCollider(Transform targetTransform, CapsuleCollider sourceCollider)
	{
		if (sourceCollider == null)
		{
			return null;
		}
		CapsuleCollider capsuleCollider = new GameObject().AddComponent<CapsuleCollider>();
		capsuleCollider.transform.rotation = sourceCollider.transform.rotation;
		capsuleCollider.transform.position = sourceCollider.transform.position;
		capsuleCollider.transform.localScale = sourceCollider.transform.lossyScale;
		capsuleCollider.radius = sourceCollider.radius;
		capsuleCollider.height = sourceCollider.height;
		capsuleCollider.center = sourceCollider.center;
		capsuleCollider.gameObject.layer = targetTransform.gameObject.layer;
		capsuleCollider.transform.SetParent(targetTransform.transform);
		return capsuleCollider;
	}

	// Token: 0x060001DA RID: 474 RVA: 0x00070D48 File Offset: 0x0006EF48
	private void HandleZonesAndOwnership()
	{
		bool flag = this.localInZone;
		this.localInZone = ZoneManagement.IsInZone(GTZone.critters);
		this.CheckOwnership();
		if (!this.LocalAuthority() && this.localInZone && NetworkSystem.Instance.InRoom && this.guard.actualOwner != null && (!this.hasNewlyInitialized || !flag) && Time.time > this.lastRequest + this.initRequestCooldown)
		{
			this.lastRequest = Time.time;
			this.hasNewlyInitialized = true;
			base.SendRPC("RequestDataInitialization", this.guard.actualOwner, Array.Empty<object>());
		}
		if (flag && !this.localInZone)
		{
			this.ResetRoom();
			this.poolParent.gameObject.SetActive(false);
			this.crittersPool.poolParent.gameObject.SetActive(false);
		}
		if (!flag && this.localInZone)
		{
			this.poolParent.gameObject.SetActive(true);
			this.crittersPool.poolParent.gameObject.SetActive(true);
		}
	}

	// Token: 0x060001DB RID: 475 RVA: 0x00070E50 File Offset: 0x0006F050
	private void CheckOwnership()
	{
		if (!PhotonNetwork.InRoom && base.IsMine)
		{
			if (this.guard.actualOwner == null || !this.guard.actualOwner.Equals(NetworkSystem.Instance.LocalPlayer))
			{
				this.guard.SetOwnership(NetworkSystem.Instance.LocalPlayer, false, false);
			}
			return;
		}
		if (this.allRigs == null && !VRRigCache.isInitialized)
		{
			return;
		}
		if (this.allRigs == null)
		{
			this.allRigs = new List<VRRig>(VRRigCache.Instance.GetAllRigs());
		}
		if (!this.LocalAuthority())
		{
			return;
		}
		if (this.localInZone)
		{
			return;
		}
		int num = int.MaxValue;
		NetPlayer netPlayer = null;
		for (int i = 0; i < this.allRigs.Count; i++)
		{
			NetPlayer creator = this.allRigs[i].creator;
			if (creator != null && this.allRigs[i].zoneEntity.currentZone == GTZone.critters && creator.ActorNumber < num)
			{
				netPlayer = creator;
				num = creator.ActorNumber;
			}
		}
		if (netPlayer == null)
		{
			return;
		}
		this.guard.TransferOwnership(netPlayer, "");
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00070F60 File Offset: 0x0006F160
	public bool LocalAuthority()
	{
		return !NetworkSystem.Instance.InRoom || (!(this.guard == null) && ((this.guard.actualOwner != null && this.guard.isTrulyMine) || (base.Owner != null && base.Owner.IsLocal) || this.guard.currentState == NetworkingState.IsOwner));
	}

	// Token: 0x060001DD RID: 477 RVA: 0x00070FCC File Offset: 0x0006F1CC
	private bool SenderIsOwner(PhotonMessageInfo info)
	{
		return (this.guard.actualOwner != null || base.Owner != null) && info.Sender != null && !this.LocalAuthority() && ((this.guard.actualOwner != null && this.guard.actualOwner.ActorNumber == info.Sender.ActorNumber) || (base.Owner != null && base.Owner.ActorNumber == info.Sender.ActorNumber));
	}

	// Token: 0x060001DE RID: 478 RVA: 0x00031912 File Offset: 0x0002FB12
	private void OwnerSentError(PhotonMessageInfo info)
	{
		NetPlayer owner = base.Owner;
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0003191B File Offset: 0x0002FB1B
	public void OnOwnershipTransferred(NetPlayer toPlayer, NetPlayer fromPlayer)
	{
		NetPlayer localPlayer = NetworkSystem.Instance.LocalPlayer;
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x00030498 File Offset: 0x0002E698
	public bool OnOwnershipRequest(NetPlayer fromPlayer)
	{
		return false;
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnMyOwnerLeft()
	{
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x00030498 File Offset: 0x0002E698
	public bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer)
	{
		return false;
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnMyCreatorLeft()
	{
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x00030709 File Offset: 0x0002E909
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x00030715 File Offset: 0x0002E915
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}

	// Token: 0x040001E9 RID: 489
	public CritterIndex creatureIndex;

	// Token: 0x040001EA RID: 490
	public static volatile CrittersManager instance;

	// Token: 0x040001EC RID: 492
	public LayerMask movementLayers;

	// Token: 0x040001ED RID: 493
	public LayerMask objectLayers;

	// Token: 0x040001EE RID: 494
	public LayerMask containerLayer;

	// Token: 0x040001EF RID: 495
	[ReadOnly]
	public List<CrittersActor> crittersActors;

	// Token: 0x040001F0 RID: 496
	[ReadOnly]
	public List<CrittersActor> allActors;

	// Token: 0x040001F1 RID: 497
	[ReadOnly]
	public List<CrittersPawn> crittersPawns;

	// Token: 0x040001F2 RID: 498
	[ReadOnly]
	public List<CrittersActor> despawnableActors;

	// Token: 0x040001F3 RID: 499
	[ReadOnly]
	public List<CrittersActor> newlyDisabledActors;

	// Token: 0x040001F4 RID: 500
	[ReadOnly]
	public List<CrittersRigActorSetup> rigActorSetups;

	// Token: 0x040001F5 RID: 501
	[ReadOnly]
	public List<CrittersActorSpawner> actorSpawners;

	// Token: 0x040001F6 RID: 502
	[NonSerialized]
	private List<CrittersActor> persistentActors = new List<CrittersActor>();

	// Token: 0x040001F7 RID: 503
	public Dictionary<int, CrittersActor> actorById;

	// Token: 0x040001F8 RID: 504
	public Dictionary<CrittersPawn, List<CrittersActor>> awareOfActors;

	// Token: 0x040001F9 RID: 505
	public Dictionary<VRRig, CrittersRigActorSetup> rigSetupByRig;

	// Token: 0x040001FA RID: 506
	private int allActorsCount;

	// Token: 0x040001FB RID: 507
	public bool intialized;

	// Token: 0x040001FC RID: 508
	private List<int> updatesToSend;

	// Token: 0x040001FD RID: 509
	public int actorsPerInitializationCall = 5;

	// Token: 0x040001FE RID: 510
	public float actorsInitializationCallCooldown = 0.2f;

	// Token: 0x040001FF RID: 511
	public Transform poolParent;

	// Token: 0x04000200 RID: 512
	public List<object> objList;

	// Token: 0x04000201 RID: 513
	public double spawnDelay;

	// Token: 0x04000202 RID: 514
	private double lastSpawnTime;

	// Token: 0x04000203 RID: 515
	public float softJointGracePeriod = 0.1f;

	// Token: 0x04000204 RID: 516
	private List<CrittersRegion> _spawnRegions;

	// Token: 0x04000205 RID: 517
	private int _currentRegionIndex = -1;

	// Token: 0x04000206 RID: 518
	private static CrittersActorGrabber _rightGrabber;

	// Token: 0x04000207 RID: 519
	private static CrittersActorGrabber _leftGrabber;

	// Token: 0x04000208 RID: 520
	public float springForce = 1000f;

	// Token: 0x04000209 RID: 521
	public float springAngularForce = 100f;

	// Token: 0x0400020A RID: 522
	public float damperForce = 10f;

	// Token: 0x0400020B RID: 523
	public float damperAngularForce = 1f;

	// Token: 0x0400020C RID: 524
	public float lightMass = 0.05f;

	// Token: 0x0400020D RID: 525
	public float heavyMass = 2f;

	// Token: 0x0400020E RID: 526
	public float overlapDistanceMax = 0.01f;

	// Token: 0x0400020F RID: 527
	public float fastThrowThreshold = 3f;

	// Token: 0x04000210 RID: 528
	public float fastThrowMultiplier = 1.5f;

	// Token: 0x04000211 RID: 529
	private Dictionary<CrittersActor.CrittersActorType, int> poolIndexDict;

	// Token: 0x04000212 RID: 530
	public CrittersManager.AllowGrabbingFlags privateRoomGrabbingFlags;

	// Token: 0x04000213 RID: 531
	public CrittersManager.AllowGrabbingFlags publicRoomGrabbingFlags;

	// Token: 0x04000214 RID: 532
	public float MaxAttachSpeed = 0.04f;

	// Token: 0x04000215 RID: 533
	private float binDimensionXMin;

	// Token: 0x04000216 RID: 534
	private float binDimensionZMin;

	// Token: 0x04000217 RID: 535
	public Transform crittersRange;

	// Token: 0x04000218 RID: 536
	public int totalBinsApproximate = 400;

	// Token: 0x04000219 RID: 537
	private float xLength;

	// Token: 0x0400021A RID: 538
	private float zLength;

	// Token: 0x0400021B RID: 539
	private int binXCount;

	// Token: 0x0400021C RID: 540
	private int binZCount;

	// Token: 0x0400021D RID: 541
	private float individualBinSide;

	// Token: 0x0400021E RID: 542
	private List<CrittersActor>[] actorBins;

	// Token: 0x0400021F RID: 543
	private bool[] priorityBins;

	// Token: 0x04000220 RID: 544
	private Dictionary<CrittersActor, int> actorBinIndices;

	// Token: 0x04000221 RID: 545
	private List<CrittersActor> nearbyActors;

	// Token: 0x04000222 RID: 546
	private List<NetPlayer> playersToUpdate;

	// Token: 0x04000223 RID: 547
	public CrittersPool crittersPool;

	// Token: 0x04000224 RID: 548
	private int lowPriorityActorsPerFrame = 5;

	// Token: 0x04000225 RID: 549
	private int lowPriorityIndex;

	// Token: 0x04000226 RID: 550
	private int spawnerIndex;

	// Token: 0x04000227 RID: 551
	private int despawnIndex;

	// Token: 0x04000228 RID: 552
	private List<CrittersActor> lowPriorityPawnsToProcess;

	// Token: 0x04000229 RID: 553
	private Dictionary<CrittersActor.CrittersActorType, float> despawnDecayValue;

	// Token: 0x0400022A RID: 554
	public float decayRate = 60f;

	// Token: 0x0400022B RID: 555
	private CrittersActor.CrittersActorType[] actorTypes;

	// Token: 0x0400022C RID: 556
	public float maxGrabDistance = 25f;

	// Token: 0x0400022D RID: 557
	public RequestableOwnershipGuard guard;

	// Token: 0x0400022E RID: 558
	private List<VRRig> allRigs;

	// Token: 0x0400022F RID: 559
	private bool localInZone;

	// Token: 0x04000230 RID: 560
	private List<int> updatingPlayers;

	// Token: 0x04000231 RID: 561
	private bool hasNewlyInitialized;

	// Token: 0x04000232 RID: 562
	private float initRequestCooldown = 10f;

	// Token: 0x04000233 RID: 563
	private float lastRequest;

	// Token: 0x04000234 RID: 564
	public int poolCount = 100;

	// Token: 0x04000235 RID: 565
	public int despawnThreshold = 20;

	// Token: 0x04000236 RID: 566
	private Dictionary<CrittersActor.CrittersActorType, int> poolCounts;

	// Token: 0x04000237 RID: 567
	private Dictionary<CrittersActor.CrittersActorType, List<CrittersActor>> actorPools;

	// Token: 0x04000238 RID: 568
	public GameObject foodPrefab;

	// Token: 0x04000239 RID: 569
	public GameObject creaturePrefab;

	// Token: 0x0400023A RID: 570
	public GameObject noisePrefab;

	// Token: 0x0400023B RID: 571
	public GameObject grabberPrefab;

	// Token: 0x0400023C RID: 572
	public GameObject cagePrefab;

	// Token: 0x0400023D RID: 573
	public GameObject foodSpawnerPrefab;

	// Token: 0x0400023E RID: 574
	public GameObject stunBombPrefab;

	// Token: 0x0400023F RID: 575
	public GameObject bodyAttachPointPrefab;

	// Token: 0x04000240 RID: 576
	public GameObject bagPrefab;

	// Token: 0x04000241 RID: 577
	public GameObject noiseMakerPrefab;

	// Token: 0x04000242 RID: 578
	public GameObject stickyTrapPrefab;

	// Token: 0x04000243 RID: 579
	public GameObject stickyGooPrefab;

	// Token: 0x04000244 RID: 580
	public int universalActorId;

	// Token: 0x04000245 RID: 581
	public int rigActorId;

	// Token: 0x04000247 RID: 583
	private CallLimiter critterEventCallLimit = new CallLimiter(10, 0.5f, 0.5f);

	// Token: 0x02000052 RID: 82
	[Flags]
	public enum AllowGrabbingFlags
	{
		// Token: 0x04000249 RID: 585
		None = 0,
		// Token: 0x0400024A RID: 586
		OutOfHands = 1,
		// Token: 0x0400024B RID: 587
		FromBags = 2,
		// Token: 0x0400024C RID: 588
		EntireBag = 4
	}

	// Token: 0x02000053 RID: 83
	public enum CritterEvent
	{
		// Token: 0x0400024E RID: 590
		StunExplosion,
		// Token: 0x0400024F RID: 591
		NoiseMakerTriggered,
		// Token: 0x04000250 RID: 592
		StickyDeployed,
		// Token: 0x04000251 RID: 593
		StickyTriggered
	}
}
