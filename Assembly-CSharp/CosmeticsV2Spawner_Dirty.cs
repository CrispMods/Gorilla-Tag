using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cysharp.Text;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaNetworking;
using GorillaNetworking.Store;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000182 RID: 386
public class CosmeticsV2Spawner_Dirty : IDelayedExecListener, ITickSystemTick
{
	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x060009A2 RID: 2466 RVA: 0x000356B7 File Offset: 0x000338B7
	// (set) Token: 0x060009A3 RID: 2467 RVA: 0x000356BE File Offset: 0x000338BE
	[OnEnterPlay_Set(false)]
	public static bool startedAllPartsInstantiated { get; private set; }

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x060009A4 RID: 2468 RVA: 0x000356C6 File Offset: 0x000338C6
	// (set) Token: 0x060009A5 RID: 2469 RVA: 0x000356CD File Offset: 0x000338CD
	[OnEnterPlay_Set(false)]
	public static bool allPartsInstantiated { get; private set; }

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x060009A6 RID: 2470 RVA: 0x000356D5 File Offset: 0x000338D5
	// (set) Token: 0x060009A7 RID: 2471 RVA: 0x000356DC File Offset: 0x000338DC
	[OnEnterPlay_Set(false)]
	public static bool completed { get; private set; }

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x060009A8 RID: 2472 RVA: 0x000356E4 File Offset: 0x000338E4
	// (set) Token: 0x060009A9 RID: 2473 RVA: 0x000356EC File Offset: 0x000338EC
	public bool TickRunning { get; set; }

	// Token: 0x060009AA RID: 2474 RVA: 0x000356F5 File Offset: 0x000338F5
	void ITickSystemTick.Tick()
	{
		this._shouldTick = false;
		if (CosmeticsV2Spawner_Dirty._g_loadOp_to_index.Count < CosmeticsV2Spawner_Dirty._g_loadOpInfos.Count)
		{
			this._shouldTick = true;
			CosmeticsV2Spawner_Dirty._Step2_UpdateLoadOpStarting();
		}
		if (!this._shouldTick)
		{
			TickSystem<object>.RemoveTickCallback(this);
		}
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0003572E File Offset: 0x0003392E
	void IDelayedExecListener.OnDelayedAction(int contextId)
	{
		if (contextId >= 0 && contextId < 1000000)
		{
			CosmeticsV2Spawner_Dirty._RetryDownload(contextId);
			return;
		}
		if (contextId == -100)
		{
			this._DelayedStatusCheck();
			return;
		}
		if (contextId == -Mathf.Abs("_Step5_InitializeVRRigsAndCosmeticsControllerFinalize".GetHashCode()))
		{
			CosmeticsV2Spawner_Dirty._Step5_InitializeVRRigsAndCosmeticsControllerFinalize();
		}
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00035768 File Offset: 0x00033968
	public static void StartInstantiatingPrefabs()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		if (CosmeticsV2Spawner_Dirty.startedAllPartsInstantiated || CosmeticsV2Spawner_Dirty.allPartsInstantiated)
		{
			Debug.LogError("CosmeticsV2Spawner_Dirty.StartInstantiatingPrefabs: All parts already started instantiated. Check `startedAllPartsInstantiated` before calling this.");
			return;
		}
		if (CosmeticsV2Spawner_Dirty._instance == null)
		{
			CosmeticsV2Spawner_Dirty._instance = new CosmeticsV2Spawner_Dirty();
		}
		CosmeticsV2Spawner_Dirty.k_stopwatch.Restart();
		CosmeticsV2Spawner_Dirty.g_gorillaPlayer = Object.FindObjectOfType<GTPlayer>();
		foreach (SnowballMaker snowballMaker in CosmeticsV2Spawner_Dirty.g_gorillaPlayer.GetComponentsInChildren<SnowballMaker>(true))
		{
			if (snowballMaker.isLeftHand)
			{
				CosmeticsV2Spawner_Dirty._gSnowballMakerLeft = snowballMaker;
			}
			else
			{
				CosmeticsV2Spawner_Dirty._gSnowballMakerRight = snowballMaker;
			}
		}
		if (!CosmeticsController.hasInstance)
		{
			Debug.LogError("(should never happen) cannot instantiate prefabs before cosmetics controller instance is available.");
			return;
		}
		if (!CosmeticsController.instance.v2_allCosmeticsInfoAssetRef.IsValid())
		{
			Debug.LogError("(should never happen) cannot load prefabs before v2_allCosmeticsInfoAssetRef is loaded.");
			return;
		}
		AllCosmeticsArraySO allCosmeticsArraySO = CosmeticsController.instance.v2_allCosmeticsInfoAssetRef.Asset as AllCosmeticsArraySO;
		if (allCosmeticsArraySO == null)
		{
			Debug.LogError("(should never happen) v2_allCosmeticsInfoAssetRef is valid but null.");
			return;
		}
		Transform[] boneXforms;
		string str;
		if (!GTHardCodedBones.TryGetBoneXforms(VRRig.LocalRig, out boneXforms, out str))
		{
			Debug.LogError("CosmeticsV2Spawner_Dirty: Error getting bone Transforms from local VRRig: " + str, VRRig.LocalRig);
			return;
		}
		CosmeticsV2Spawner_Dirty._gVRRigDatas.Add(new CosmeticsV2Spawner_Dirty.VRRigData(VRRig.LocalRig, boneXforms));
		int vrRigIndex = 0;
		foreach (VRRig vrRig in VRRigCache.Instance.GetAllRigs())
		{
			Transform[] boneXforms2;
			if (!GTHardCodedBones.TryGetBoneXforms(vrRig, out boneXforms2, out str))
			{
				Debug.LogError("CosmeticsV2Spawner_Dirty: Error getting bone Transforms from cached VRRig: " + str, VRRig.LocalRig);
				return;
			}
			CosmeticsV2Spawner_Dirty._gVRRigDatas.Add(new CosmeticsV2Spawner_Dirty.VRRigData(vrRig, boneXforms2));
		}
		CosmeticsV2Spawner_Dirty._gDeactivatedSpawnParent = GlobalDeactivatedSpawnRoot.GetOrCreate();
		GTDelayedExec.Add(CosmeticsV2Spawner_Dirty._instance, 2f, -100);
		int num = 0;
		int num2 = 0;
		foreach (GTDirectAssetRef<CosmeticSO> gtdirectAssetRef in allCosmeticsArraySO.sturdyAssetRefs)
		{
			CosmeticInfoV2 info = gtdirectAssetRef.obj.info;
			if (info.hasHoldableParts)
			{
				for (int j = 0; j < CosmeticsV2Spawner_Dirty._gVRRigDatas.Count; j++)
				{
					for (int k = 0; k < info.holdableParts.Length; k++)
					{
						CosmeticsV2Spawner_Dirty.AddEachAttachInfoToLoadOpInfosList(info.holdableParts[k], k, info, j, ref num);
					}
				}
			}
			if (info.hasFunctionalParts)
			{
				for (int l = 0; l < CosmeticsV2Spawner_Dirty._gVRRigDatas.Count; l++)
				{
					for (int m = 0; m < info.functionalParts.Length; m++)
					{
						CosmeticsV2Spawner_Dirty.AddEachAttachInfoToLoadOpInfosList(info.functionalParts[m], m, info, l, ref num);
					}
				}
			}
			if (info.hasFirstPersonViewParts)
			{
				for (int n = 0; n < info.firstPersonViewParts.Length; n++)
				{
					CosmeticsV2Spawner_Dirty.AddEachAttachInfoToLoadOpInfosList(info.firstPersonViewParts[n], n, info, vrRigIndex, ref num2);
				}
			}
		}
		TickSystem<object>.AddTickCallback(CosmeticsV2Spawner_Dirty._instance);
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00035A34 File Offset: 0x00033C34
	private static void AddEachAttachInfoToLoadOpInfosList(CosmeticPart part, int partIndex, CosmeticInfoV2 cosmeticInfo, int vrRigIndex, ref int partCount)
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		for (int i = 0; i < part.attachAnchors.Length; i++)
		{
			CosmeticsV2Spawner_Dirty.LoadOpInfo item = new CosmeticsV2Spawner_Dirty.LoadOpInfo(part.attachAnchors[i], part, partIndex, cosmeticInfo, vrRigIndex);
			CosmeticsV2Spawner_Dirty._g_loadOpInfos.Add(item);
			partCount++;
			if (part.partType == ECosmeticPartType.Holdable && i == 0)
			{
				break;
			}
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00035A94 File Offset: 0x00033C94
	private static void _Step2_UpdateLoadOpStarting()
	{
		int num = CosmeticsV2Spawner_Dirty._g_loadOp_to_index.Count - CosmeticsV2Spawner_Dirty._g_loadOpsCountCompleted;
		while (CosmeticsV2Spawner_Dirty._g_loadOp_to_index.Count < CosmeticsV2Spawner_Dirty._g_loadOpInfos.Count && num < 1000000)
		{
			num++;
			int count = CosmeticsV2Spawner_Dirty._g_loadOp_to_index.Count;
			CosmeticsV2Spawner_Dirty.LoadOpInfo loadOpInfo = CosmeticsV2Spawner_Dirty._g_loadOpInfos[count];
			loadOpInfo.loadOp = loadOpInfo.part.prefabAssetRef.InstantiateAsync(CosmeticsV2Spawner_Dirty._gDeactivatedSpawnParent, false);
			loadOpInfo.isStarted = true;
			CosmeticsV2Spawner_Dirty._g_loadOp_to_index.Add(loadOpInfo.loadOp, count);
			loadOpInfo.loadOp.Completed += CosmeticsV2Spawner_Dirty._Step3_HandleLoadOpCompleted;
			CosmeticsV2Spawner_Dirty._g_loadOpInfos[count] = loadOpInfo;
		}
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x00035B4C File Offset: 0x00033D4C
	private static void _Step3_HandleLoadOpCompleted(AsyncOperationHandle<GameObject> loadOp)
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		int num;
		if (!CosmeticsV2Spawner_Dirty._g_loadOp_to_index.TryGetValue(loadOp, out num))
		{
			throw new Exception("(this should never happen) could not find LoadOpInfo in `_g_loadOpInfos`.");
		}
		CosmeticsV2Spawner_Dirty.LoadOpInfo loadOpInfo = CosmeticsV2Spawner_Dirty._g_loadOpInfos[num];
		if (loadOp.Status == AsyncOperationStatus.Failed)
		{
			Debug.Log("CosmeticsV2Spawner_Dirty: Failed to load a part for cosmetic \"" + loadOpInfo.cosmeticInfoV2.displayName + "\"! waiting for 10 seconds before trying again.");
			GTDelayedExec.Add(CosmeticsV2Spawner_Dirty._instance, 10f, num);
			return;
		}
		CosmeticsV2Spawner_Dirty._g_loadOpsCountCompleted++;
		ECosmeticSelectSide ecosmeticSelectSide = loadOpInfo.attachInfo.selectSide;
		string name = loadOpInfo.cosmeticInfoV2.playFabID;
		if (ecosmeticSelectSide != ECosmeticSelectSide.Both)
		{
			string playFabID = loadOpInfo.cosmeticInfoV2.playFabID;
			string arg;
			if (ecosmeticSelectSide != ECosmeticSelectSide.Left)
			{
				if (ecosmeticSelectSide != ECosmeticSelectSide.Right)
				{
					arg = "";
				}
				else
				{
					arg = " RIGHT.";
				}
			}
			else
			{
				arg = " LEFT.";
			}
			name = ZString.Concat<string, string>(playFabID, arg);
		}
		loadOpInfo.resultGObj = loadOp.Result;
		loadOpInfo.resultGObj.SetActive(false);
		Transform transform = loadOpInfo.resultGObj.transform;
		Transform transform2 = transform;
		CosmeticPart[] holdableParts = loadOpInfo.cosmeticInfoV2.holdableParts;
		if (holdableParts != null && holdableParts.Length > 0)
		{
			TransferrableObject componentInChildren = loadOpInfo.resultGObj.GetComponentInChildren<TransferrableObject>(true);
			if (componentInChildren && componentInChildren.gameObject != loadOpInfo.resultGObj)
			{
				transform2 = componentInChildren.transform;
				transform2.gameObject.SetActive(false);
				loadOpInfo.resultGObj.SetActive(true);
			}
		}
		if (loadOpInfo.cosmeticInfoV2.isThrowable)
		{
			SnowballThrowable componentInChildren2 = loadOpInfo.resultGObj.GetComponentInChildren<SnowballThrowable>(true);
			if (componentInChildren2 && componentInChildren2.gameObject != loadOpInfo.resultGObj)
			{
				transform2 = componentInChildren2.transform;
				transform2.gameObject.SetActive(false);
				loadOpInfo.resultGObj.SetActive(true);
			}
		}
		transform2.name = name;
		CosmeticsV2Spawner_Dirty.VRRigData vrrigData = (loadOpInfo.vrRigIndex != -1) ? CosmeticsV2Spawner_Dirty._gVRRigDatas[loadOpInfo.vrRigIndex] : default(CosmeticsV2Spawner_Dirty.VRRigData);
		Transform transform3;
		switch (loadOpInfo.part.partType)
		{
		case ECosmeticPartType.Holdable:
			transform3 = ((loadOpInfo.attachInfo.parentBone != GTHardCodedBones.EBone.body_AnchorFront_StowSlot) ? vrrigData.parentOfDeactivatedHoldables : vrrigData.boneXforms[(int)loadOpInfo.attachInfo.parentBone]);
			goto IL_284;
		case ECosmeticPartType.Functional:
			transform3 = vrrigData.boneXforms[(int)loadOpInfo.attachInfo.parentBone];
			goto IL_284;
		case ECosmeticPartType.FirstPerson:
			transform3 = CosmeticsV2Spawner_Dirty.g_gorillaPlayer.CosmeticsHeadTarget;
			goto IL_284;
		}
		throw new ArgumentOutOfRangeException("partType", "unhandled part type.");
		IL_284:
		Transform transform4 = transform3;
		if (transform4)
		{
			transform.SetParent(transform4, false);
			transform.localPosition = loadOpInfo.attachInfo.offset.pos;
			Transform transform5 = transform;
			XformOffset offset = loadOpInfo.attachInfo.offset;
			transform5.localRotation = offset.rot;
			transform.localScale = loadOpInfo.attachInfo.offset.scale;
		}
		else
		{
			Debug.LogError(string.Concat(new string[]
			{
				string.Format("Bone transform not found for cosmetic part type {0}. Cosmetic: ", loadOpInfo.part.partType),
				"\"",
				loadOpInfo.cosmeticInfoV2.displayName,
				"\",",
				string.Format("part: \"{0}\"", loadOpInfo.part.prefabAssetRef.RuntimeKey)
			}));
		}
		switch (loadOpInfo.part.partType)
		{
		case ECosmeticPartType.Holdable:
		{
			vrrigData.vrRig_cosmetics.Add(transform2.gameObject);
			HoldableObject componentInChildren3 = loadOpInfo.resultGObj.GetComponentInChildren<HoldableObject>(true);
			SnowballThrowable snowballThrowable = componentInChildren3 as SnowballThrowable;
			if (snowballThrowable != null)
			{
				CosmeticsV2Spawner_Dirty.AddPartToThrowableLists(loadOpInfo, snowballThrowable);
				goto IL_5D6;
			}
			TransferrableObject transferrableObject = componentInChildren3 as TransferrableObject;
			if (transferrableObject == null)
			{
				if (componentInChildren3 != null)
				{
					throw new Exception("Encountered unexpected HoldableObject derived type on cosmetic part: \"" + loadOpInfo.cosmeticInfoV2.displayName + "\"");
				}
				goto IL_5D6;
			}
			else
			{
				vrrigData.bdPositions_allObjects.Add(transferrableObject);
				string text = loadOpInfo.cosmeticInfoV2.playFabID;
				int[] array;
				if (CosmeticsLegacyV1Info.TryGetBodyDockAllObjectsIndexes(text, out array))
				{
					if (loadOpInfo.partIndex < array.Length && loadOpInfo.partIndex >= 0)
					{
						transferrableObject.myIndex = array[loadOpInfo.partIndex];
					}
				}
				else if (text.Length >= 5 && text[0] == 'L')
				{
					if (text[1] != 'M')
					{
						throw new Exception("(this should never happen) A TransferrableObject cosmetic added sometime after 2024-06 does not use the expected PlayFabID format where the string starts with \"LM\" and ends with \".\". Path: " + transform2.GetPathQ());
					}
					string text2 = text;
					text = ((text2[text2.Length - 1] == '.') ? text : (text + "."));
					int num2 = 224;
					transferrableObject.myIndex = num2 + CosmeticIDUtils.PlayFabIdToIndexInCategory(text);
				}
				else
				{
					transferrableObject.myIndex = -2;
					if (!(text == "STICKABLE TARGET"))
					{
						Debug.LogError(string.Concat(new string[]
						{
							"Cosmetic \"",
							loadOpInfo.cosmeticInfoV2.displayName,
							"\" cannot derive `TransferrableObject.myIndex` from playFabId \"",
							text,
							"\" and so will not be included in `BodyDockPositions.allObjects` array."
						}));
					}
				}
				vrrigData.bdPositions_allObjects_length = math.max(transferrableObject.myIndex + 1, vrrigData.bdPositions_allObjects_length);
				ProjectileWeapon projectileWeapon = transferrableObject as ProjectileWeapon;
				if (projectileWeapon != null && loadOpInfo.cosmeticInfoV2.playFabID == "Slingshot")
				{
					vrrigData.vrRig.projectileWeapon = projectileWeapon;
					goto IL_5D6;
				}
				goto IL_5D6;
			}
			break;
		}
		case ECosmeticPartType.Functional:
			vrrigData.vrRig_cosmetics.Add(transform2.gameObject);
			goto IL_5D6;
		case ECosmeticPartType.FirstPerson:
			vrrigData.vrRig_override.Add(transform2.gameObject);
			goto IL_5D6;
		}
		throw new ArgumentOutOfRangeException("Unexpected ECosmeticPartType value encountered: " + string.Format("{0}, ", loadOpInfo.part.partType) + string.Format("int: {0}.", (int)loadOpInfo.part.partType));
		IL_5D6:
		if (loadOpInfo.vrRigIndex > -1)
		{
			CosmeticsV2Spawner_Dirty._gVRRigDatas[loadOpInfo.vrRigIndex] = vrrigData;
		}
		CosmeticRefRegistry cosmeticReferences = CosmeticsV2Spawner_Dirty._gVRRigDatas[loadOpInfo.vrRigIndex].vrRig.cosmeticReferences;
		CosmeticRefTarget[] componentsInChildren = loadOpInfo.resultGObj.GetComponentsInChildren<CosmeticRefTarget>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			cosmeticReferences.Register(componentsInChildren[i].id, componentsInChildren[i].gameObject);
		}
		CosmeticsV2Spawner_Dirty._g_loadOpInfos[num] = loadOpInfo;
		if (CosmeticsV2Spawner_Dirty._g_loadOpsCountCompleted < CosmeticsV2Spawner_Dirty._g_loadOpInfos.Count)
		{
			return;
		}
		CosmeticsV2Spawner_Dirty._Step4_PopulateAllArrays();
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x000361C8 File Offset: 0x000343C8
	private static void _RetryDownload(int loadOpIndex)
	{
		if (loadOpIndex < 0 || loadOpIndex >= CosmeticsV2Spawner_Dirty._g_loadOpInfos.Count)
		{
			Debug.LogError("(should never happen) Unexpected! While trying to recover from a failed download, the value " + string.Format("{0}={1} was out of range of ", "loadOpIndex", loadOpIndex) + string.Format("{0}.Count={1}.", "_g_loadOpInfos", CosmeticsV2Spawner_Dirty._g_loadOpInfos.Count));
			return;
		}
		CosmeticsV2Spawner_Dirty.LoadOpInfo loadOpInfo = CosmeticsV2Spawner_Dirty._g_loadOpInfos[loadOpIndex];
		if (!CosmeticsV2Spawner_Dirty._g_loadOp_to_index.Remove(loadOpInfo.loadOp))
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"(should never happen) Unexpected! Could not find the loadOp to remove it in the _g_loadOp_to_index. If you see this message then comparison does not work the way I thought and we need a different way to store/retrieve loadOpInfos. Happened while trying to retry failed download prefab part of cosmetic \"",
				loadOpInfo.cosmeticInfoV2.displayName,
				"\" with guid \"",
				loadOpInfo.part.prefabAssetRef.AssetGUID,
				"\"."
			}));
		}
		Debug.Log("Retrying prefab part of cosmetic \"{loadOpInfo.cosmeticInfoV2.displayName}\" with guid \"" + loadOpInfo.part.prefabAssetRef.AssetGUID + "\".");
		loadOpInfo.loadOp = loadOpInfo.part.prefabAssetRef.InstantiateAsync(CosmeticsV2Spawner_Dirty._gDeactivatedSpawnParent, false);
		CosmeticsV2Spawner_Dirty._g_loadOpInfos[loadOpIndex] = loadOpInfo;
		CosmeticsV2Spawner_Dirty._g_loadOp_to_index[loadOpInfo.loadOp] = loadOpIndex;
		loadOpInfo.loadOp.Completed += CosmeticsV2Spawner_Dirty._Step3_HandleLoadOpCompleted;
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x00036308 File Offset: 0x00034508
	private static void AddPartToThrowableLists(CosmeticsV2Spawner_Dirty.LoadOpInfo loadOpInfo, SnowballThrowable throwable)
	{
		CosmeticsV2Spawner_Dirty.VRRigData vrrigData = CosmeticsV2Spawner_Dirty._gVRRigDatas[loadOpInfo.vrRigIndex];
		EHandedness handednessFromBone = GTHardCodedBones.GetHandednessFromBone(loadOpInfo.attachInfo.parentBone);
		bool flag = vrrigData.vrRig == CosmeticsV2Spawner_Dirty._gVRRigDatas[0].vrRig;
		switch (handednessFromBone)
		{
		case EHandedness.None:
			throw new ArgumentException(string.Concat(new string[]
			{
				"Encountered throwable cosmetic \"",
				loadOpInfo.cosmeticInfoV2.displayName,
				"\" where handedness ",
				string.Format("could not be determined from bone `{0}`. ", loadOpInfo.attachInfo.parentBone),
				"Path: \"",
				throwable.transform.GetPath(),
				"\""
			}));
		case EHandedness.Left:
			CosmeticsV2Spawner_Dirty.ResizeAndSetAtIndex<GameObject>(vrrigData.bdPositions_leftHandThrowables, throwable.gameObject, throwable.throwableMakerIndex);
			if (flag)
			{
				CosmeticsV2Spawner_Dirty.ResizeAndSetAtIndex<SnowballThrowable>(CosmeticsV2Spawner_Dirty._gSnowballMakerLeft_throwables, throwable, throwable.throwableMakerIndex);
				return;
			}
			break;
		case EHandedness.Right:
			CosmeticsV2Spawner_Dirty.ResizeAndSetAtIndex<GameObject>(vrrigData.bdPositions_rightHandThrowables, throwable.gameObject, throwable.throwableMakerIndex);
			if (flag)
			{
				CosmeticsV2Spawner_Dirty.ResizeAndSetAtIndex<SnowballThrowable>(CosmeticsV2Spawner_Dirty._gSnowballMakerRight_throwables, throwable, throwable.throwableMakerIndex);
				return;
			}
			break;
		default:
			throw new ArgumentOutOfRangeException("Unexpected ECosmeticSelectSide value encountered: " + string.Format("{0}, ", handednessFromBone) + string.Format("int: {0}.", (int)handednessFromBone));
		}
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x00036468 File Offset: 0x00034668
	private static void ResizeAndSetAtIndex<T>(List<T> list, T item, int index)
	{
		if (index >= list.Count)
		{
			int num = index - list.Count + 1;
			for (int i = 0; i < num; i++)
			{
				list.Add(default(T));
			}
		}
		list[index] = item;
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x000364AC File Offset: 0x000346AC
	private static void _Step4_PopulateAllArrays()
	{
		if (CosmeticsV2Spawner_Dirty.allPartsInstantiated)
		{
			Debug.LogError("_Step4_PopulateAllArrays: (should never happen) CALLED MORE THAN ONCE!");
			return;
		}
		foreach (CosmeticsV2Spawner_Dirty.LoadOpInfo loadOpInfo in CosmeticsV2Spawner_Dirty._g_loadOpInfos)
		{
			ISpawnable[] componentsInChildren = loadOpInfo.resultGObj.GetComponentsInChildren<ISpawnable>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				try
				{
					componentsInChildren[i].IsSpawned = true;
					componentsInChildren[i].CosmeticSelectedSide = loadOpInfo.attachInfo.selectSide;
					componentsInChildren[i].OnSpawn(CosmeticsV2Spawner_Dirty._gVRRigDatas[loadOpInfo.vrRigIndex].vrRig);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}
		CosmeticsV2Spawner_Dirty._gSnowballMakerLeft.SetupThrowables(CosmeticsV2Spawner_Dirty._gSnowballMakerLeft_throwables.ToArray());
		CosmeticsV2Spawner_Dirty._gSnowballMakerRight.SetupThrowables(CosmeticsV2Spawner_Dirty._gSnowballMakerRight_throwables.ToArray());
		foreach (CosmeticsV2Spawner_Dirty.VRRigData vrrigData in CosmeticsV2Spawner_Dirty._gVRRigDatas)
		{
			vrrigData.vrRig.cosmetics = vrrigData.vrRig_cosmetics.ToArray();
			vrrigData.vrRig.overrideCosmetics = vrrigData.vrRig_override.ToArray();
			vrrigData.bdPositionsComp.leftHandThrowables = vrrigData.bdPositions_leftHandThrowables.ToArray();
			vrrigData.bdPositionsComp.rightHandThrowables = vrrigData.bdPositions_rightHandThrowables.ToArray();
			vrrigData.bdPositionsComp._allObjects = new TransferrableObject[vrrigData.bdPositions_allObjects_length];
			foreach (TransferrableObject transferrableObject in vrrigData.bdPositions_allObjects)
			{
				if (transferrableObject.myIndex >= 0 && transferrableObject.myIndex < vrrigData.bdPositions_allObjects_length)
				{
					vrrigData.bdPositionsComp._allObjects[transferrableObject.myIndex] = transferrableObject;
				}
			}
		}
		CosmeticsV2Spawner_Dirty.allPartsInstantiated = true;
		GTDelayedExec.Add(CosmeticsV2Spawner_Dirty._instance, 1f, -Mathf.Abs("_Step5_InitializeVRRigsAndCosmeticsControllerFinalize".GetHashCode()));
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x000366F0 File Offset: 0x000348F0
	private static void _Step5_InitializeVRRigsAndCosmeticsControllerFinalize()
	{
		CosmeticsController.instance.UpdateWardrobeModelsAndButtons();
		try
		{
			Action onPostInstantiateAllPrefabs = CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs;
			if (onPostInstantiateAllPrefabs != null)
			{
				onPostInstantiateAllPrefabs();
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		try
		{
			CosmeticsController.instance.InitializeCosmeticStands();
		}
		catch (Exception exception2)
		{
			Debug.LogException(exception2);
		}
		try
		{
			Action onPostInstantiateAllPrefabs2 = CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs2;
			if (onPostInstantiateAllPrefabs2 != null)
			{
				onPostInstantiateAllPrefabs2();
			}
		}
		catch (Exception exception3)
		{
			Debug.LogException(exception3);
		}
		try
		{
			CosmeticsController.instance.UpdateWornCosmetics(false);
		}
		catch (Exception exception4)
		{
			Debug.LogException(exception4);
		}
		foreach (CosmeticsV2Spawner_Dirty.VRRigData vrrigData in CosmeticsV2Spawner_Dirty._gVRRigDatas)
		{
			try
			{
				if (vrrigData.bdPositionsComp.isActiveAndEnabled)
				{
					vrrigData.bdPositionsComp.RefreshTransferrableItems();
				}
			}
			catch (Exception exception5)
			{
				Debug.LogException(exception5, vrrigData.vrRig);
			}
		}
		try
		{
			StoreController.instance.InitalizeCosmeticStands();
		}
		catch (Exception exception6)
		{
			Debug.LogException(exception6);
		}
		CosmeticsV2Spawner_Dirty.completed = true;
		CosmeticsV2Spawner_Dirty.k_stopwatch.Stop();
		Debug.Log("_Step5_InitializeVRRigsAndCosmeticsControllerFinalize" + string.Format(": Done instantiating cosmetics in {0:0.0000} seconds.", (double)CosmeticsV2Spawner_Dirty.k_stopwatch.ElapsedMilliseconds / 1000.0));
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x00036870 File Offset: 0x00034A70
	private void _DelayedStatusCheck()
	{
		int count = CosmeticsV2Spawner_Dirty._g_loadOpInfos.Count;
		Debug.Log(ZString.Concat<string, string, string, string, double, string, int, string, int, string>("CosmeticsV2Spawner_Dirty", ".", "_DelayedStatusCheck", ": Load progress ", (double)CosmeticsV2Spawner_Dirty._g_loadOpsCountCompleted / (double)count * 100.0, "% (", CosmeticsV2Spawner_Dirty._g_loadOpsCountCompleted, "/", count, ")."));
		if (CosmeticsV2Spawner_Dirty._g_loadOpsCountCompleted < count)
		{
			GTDelayedExec.Add(this, 2f, -100);
		}
	}

	// Token: 0x04000BA7 RID: 2983
	private static CosmeticsV2Spawner_Dirty _instance;

	// Token: 0x04000BA8 RID: 2984
	public static Action OnPostInstantiateAllPrefabs;

	// Token: 0x04000BA9 RID: 2985
	public static Action OnPostInstantiateAllPrefabs2;

	// Token: 0x04000BAD RID: 2989
	[OnEnterPlay_SetNull]
	private static Transform _gDeactivatedSpawnParent;

	// Token: 0x04000BAE RID: 2990
	[OnEnterPlay_Set(0)]
	private static int _g_loadOpsCountCompleted = 0;

	// Token: 0x04000BAF RID: 2991
	private const int _k_maxActiveLoadOps = 1000000;

	// Token: 0x04000BB0 RID: 2992
	private const int _k_maxTotalLoadOps = 1000000;

	// Token: 0x04000BB1 RID: 2993
	private const int _k_delayedStatusCheckContextId = -100;

	// Token: 0x04000BB2 RID: 2994
	[OnEnterPlay_Clear]
	private static readonly List<CosmeticsV2Spawner_Dirty.LoadOpInfo> _g_loadOpInfos = new List<CosmeticsV2Spawner_Dirty.LoadOpInfo>(100000);

	// Token: 0x04000BB3 RID: 2995
	[OnEnterPlay_Clear]
	private static readonly Dictionary<AsyncOperationHandle<GameObject>, int> _g_loadOp_to_index = new Dictionary<AsyncOperationHandle<GameObject>, int>(100000);

	// Token: 0x04000BB4 RID: 2996
	[OnEnterPlay_SetNull]
	private static SnowballMaker _gSnowballMakerLeft;

	// Token: 0x04000BB5 RID: 2997
	[OnEnterPlay_Clear]
	private static readonly List<SnowballThrowable> _gSnowballMakerLeft_throwables = new List<SnowballThrowable>(20);

	// Token: 0x04000BB6 RID: 2998
	[OnEnterPlay_SetNull]
	private static SnowballMaker _gSnowballMakerRight;

	// Token: 0x04000BB7 RID: 2999
	[OnEnterPlay_Clear]
	private static readonly List<SnowballThrowable> _gSnowballMakerRight_throwables = new List<SnowballThrowable>(20);

	// Token: 0x04000BB8 RID: 3000
	[OnEnterPlay_SetNull]
	private static GTPlayer g_gorillaPlayer;

	// Token: 0x04000BB9 RID: 3001
	[OnEnterPlay_SetNull]
	private static Transform[] g_allInstantiatedParts;

	// Token: 0x04000BBA RID: 3002
	private static Stopwatch k_stopwatch = new Stopwatch();

	// Token: 0x04000BBB RID: 3003
	[OnEnterPlay_Clear]
	private static readonly List<CosmeticsV2Spawner_Dirty.VRRigData> _gVRRigDatas = new List<CosmeticsV2Spawner_Dirty.VRRigData>(11);

	// Token: 0x04000BBC RID: 3004
	private bool _shouldTick;

	// Token: 0x02000183 RID: 387
	private struct LoadOpInfo
	{
		// Token: 0x060009B8 RID: 2488 RVA: 0x00036944 File Offset: 0x00034B44
		public LoadOpInfo(CosmeticAttachInfo attachInfo, CosmeticPart part, int partIndex, CosmeticInfoV2 cosmeticInfoV2, int vrRigIndex)
		{
			this.isStarted = false;
			this.loadOp = default(AsyncOperationHandle<GameObject>);
			this.resultGObj = null;
			this.attachInfo = attachInfo;
			this.part = part;
			this.partIndex = partIndex;
			this.cosmeticInfoV2 = cosmeticInfoV2;
			this.vrRigIndex = vrRigIndex;
		}

		// Token: 0x04000BBE RID: 3006
		public bool isStarted;

		// Token: 0x04000BBF RID: 3007
		public AsyncOperationHandle<GameObject> loadOp;

		// Token: 0x04000BC0 RID: 3008
		public GameObject resultGObj;

		// Token: 0x04000BC1 RID: 3009
		public readonly CosmeticAttachInfo attachInfo;

		// Token: 0x04000BC2 RID: 3010
		public readonly CosmeticPart part;

		// Token: 0x04000BC3 RID: 3011
		public readonly int partIndex;

		// Token: 0x04000BC4 RID: 3012
		public readonly CosmeticInfoV2 cosmeticInfoV2;

		// Token: 0x04000BC5 RID: 3013
		public readonly int vrRigIndex;
	}

	// Token: 0x02000184 RID: 388
	private struct VRRigData
	{
		// Token: 0x060009B9 RID: 2489 RVA: 0x00036990 File Offset: 0x00034B90
		public VRRigData(VRRig vrRig, Transform[] boneXforms)
		{
			this.vrRig = vrRig;
			this.boneXforms = boneXforms;
			if (!vrRig.transform.TryFindByPath("./**/Holdables", out this.parentOfDeactivatedHoldables, false))
			{
				Debug.LogError("Could not find parent for deactivated holdables. Falling back to VRRig transform: \"" + vrRig.transform.GetPath() + "\"");
			}
			this.bdPositionsComp = vrRig.GetComponentInChildren<BodyDockPositions>(true);
			this.vrRig_cosmetics = new List<GameObject>(500);
			this.vrRig_override = new List<GameObject>(500);
			this.bdPositions_leftHandThrowables = new List<GameObject>(20);
			this.bdPositions_rightHandThrowables = new List<GameObject>(20);
			this.bdPositions_allObjects = new List<TransferrableObject>(20);
			this.bdPositions_allObjects_length = 0;
		}

		// Token: 0x04000BC6 RID: 3014
		public readonly VRRig vrRig;

		// Token: 0x04000BC7 RID: 3015
		public readonly Transform[] boneXforms;

		// Token: 0x04000BC8 RID: 3016
		public readonly BodyDockPositions bdPositionsComp;

		// Token: 0x04000BC9 RID: 3017
		public readonly List<GameObject> vrRig_cosmetics;

		// Token: 0x04000BCA RID: 3018
		public readonly List<GameObject> vrRig_override;

		// Token: 0x04000BCB RID: 3019
		public readonly Transform parentOfDeactivatedHoldables;

		// Token: 0x04000BCC RID: 3020
		public readonly List<TransferrableObject> bdPositions_allObjects;

		// Token: 0x04000BCD RID: 3021
		public int bdPositions_allObjects_length;

		// Token: 0x04000BCE RID: 3022
		public readonly List<GameObject> bdPositions_leftHandThrowables;

		// Token: 0x04000BCF RID: 3023
		public readonly List<GameObject> bdPositions_rightHandThrowables;
	}
}
