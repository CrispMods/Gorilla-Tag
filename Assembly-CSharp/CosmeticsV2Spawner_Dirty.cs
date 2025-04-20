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

// Token: 0x0200018D RID: 397
public class CosmeticsV2Spawner_Dirty : IDelayedExecListener, ITickSystemTick
{
	// Token: 0x170000FB RID: 251
	// (get) Token: 0x060009EE RID: 2542 RVA: 0x00036FF7 File Offset: 0x000351F7
	// (set) Token: 0x060009EF RID: 2543 RVA: 0x00036FFE File Offset: 0x000351FE
	[OnEnterPlay_Set(false)]
	public static bool startedAllPartsInstantiated { get; private set; }

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x060009F0 RID: 2544 RVA: 0x00037006 File Offset: 0x00035206
	// (set) Token: 0x060009F1 RID: 2545 RVA: 0x0003700D File Offset: 0x0003520D
	[OnEnterPlay_Set(false)]
	public static bool allPartsInstantiated { get; private set; }

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x060009F2 RID: 2546 RVA: 0x00037015 File Offset: 0x00035215
	// (set) Token: 0x060009F3 RID: 2547 RVA: 0x0003701C File Offset: 0x0003521C
	[OnEnterPlay_Set(false)]
	public static bool completed { get; private set; }

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x060009F4 RID: 2548 RVA: 0x00037024 File Offset: 0x00035224
	// (set) Token: 0x060009F5 RID: 2549 RVA: 0x0003702C File Offset: 0x0003522C
	public bool TickRunning { get; set; }

	// Token: 0x060009F6 RID: 2550 RVA: 0x00037035 File Offset: 0x00035235
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

	// Token: 0x060009F7 RID: 2551 RVA: 0x0003706E File Offset: 0x0003526E
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

	// Token: 0x060009F8 RID: 2552 RVA: 0x0009588C File Offset: 0x00093A8C
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
		CosmeticsV2Spawner_Dirty.g_gorillaPlayer = UnityEngine.Object.FindObjectOfType<GTPlayer>();
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

	// Token: 0x060009F9 RID: 2553 RVA: 0x00095B58 File Offset: 0x00093D58
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

	// Token: 0x060009FA RID: 2554 RVA: 0x00095BB8 File Offset: 0x00093DB8
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

	// Token: 0x060009FB RID: 2555 RVA: 0x00095C70 File Offset: 0x00093E70
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

	// Token: 0x060009FC RID: 2556 RVA: 0x000962EC File Offset: 0x000944EC
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

	// Token: 0x060009FD RID: 2557 RVA: 0x0009642C File Offset: 0x0009462C
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

	// Token: 0x060009FE RID: 2558 RVA: 0x0009658C File Offset: 0x0009478C
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

	// Token: 0x060009FF RID: 2559 RVA: 0x000965D0 File Offset: 0x000947D0
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

	// Token: 0x06000A00 RID: 2560 RVA: 0x00096814 File Offset: 0x00094A14
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

	// Token: 0x06000A01 RID: 2561 RVA: 0x00096994 File Offset: 0x00094B94
	private void _DelayedStatusCheck()
	{
		int count = CosmeticsV2Spawner_Dirty._g_loadOpInfos.Count;
		Debug.Log(ZString.Concat<string, string, string, string, double, string, int, string, int, string>("CosmeticsV2Spawner_Dirty", ".", "_DelayedStatusCheck", ": Load progress ", (double)CosmeticsV2Spawner_Dirty._g_loadOpsCountCompleted / (double)count * 100.0, "% (", CosmeticsV2Spawner_Dirty._g_loadOpsCountCompleted, "/", count, ")."));
		if (CosmeticsV2Spawner_Dirty._g_loadOpsCountCompleted < count)
		{
			GTDelayedExec.Add(this, 2f, -100);
		}
	}

	// Token: 0x04000BED RID: 3053
	private static CosmeticsV2Spawner_Dirty _instance;

	// Token: 0x04000BEE RID: 3054
	public static Action OnPostInstantiateAllPrefabs;

	// Token: 0x04000BEF RID: 3055
	public static Action OnPostInstantiateAllPrefabs2;

	// Token: 0x04000BF3 RID: 3059
	[OnEnterPlay_SetNull]
	private static Transform _gDeactivatedSpawnParent;

	// Token: 0x04000BF4 RID: 3060
	[OnEnterPlay_Set(0)]
	private static int _g_loadOpsCountCompleted = 0;

	// Token: 0x04000BF5 RID: 3061
	private const int _k_maxActiveLoadOps = 1000000;

	// Token: 0x04000BF6 RID: 3062
	private const int _k_maxTotalLoadOps = 1000000;

	// Token: 0x04000BF7 RID: 3063
	private const int _k_delayedStatusCheckContextId = -100;

	// Token: 0x04000BF8 RID: 3064
	[OnEnterPlay_Clear]
	private static readonly List<CosmeticsV2Spawner_Dirty.LoadOpInfo> _g_loadOpInfos = new List<CosmeticsV2Spawner_Dirty.LoadOpInfo>(100000);

	// Token: 0x04000BF9 RID: 3065
	[OnEnterPlay_Clear]
	private static readonly Dictionary<AsyncOperationHandle<GameObject>, int> _g_loadOp_to_index = new Dictionary<AsyncOperationHandle<GameObject>, int>(100000);

	// Token: 0x04000BFA RID: 3066
	[OnEnterPlay_SetNull]
	private static SnowballMaker _gSnowballMakerLeft;

	// Token: 0x04000BFB RID: 3067
	[OnEnterPlay_Clear]
	private static readonly List<SnowballThrowable> _gSnowballMakerLeft_throwables = new List<SnowballThrowable>(20);

	// Token: 0x04000BFC RID: 3068
	[OnEnterPlay_SetNull]
	private static SnowballMaker _gSnowballMakerRight;

	// Token: 0x04000BFD RID: 3069
	[OnEnterPlay_Clear]
	private static readonly List<SnowballThrowable> _gSnowballMakerRight_throwables = new List<SnowballThrowable>(20);

	// Token: 0x04000BFE RID: 3070
	[OnEnterPlay_SetNull]
	private static GTPlayer g_gorillaPlayer;

	// Token: 0x04000BFF RID: 3071
	[OnEnterPlay_SetNull]
	private static Transform[] g_allInstantiatedParts;

	// Token: 0x04000C00 RID: 3072
	private static Stopwatch k_stopwatch = new Stopwatch();

	// Token: 0x04000C01 RID: 3073
	[OnEnterPlay_Clear]
	private static readonly List<CosmeticsV2Spawner_Dirty.VRRigData> _gVRRigDatas = new List<CosmeticsV2Spawner_Dirty.VRRigData>(11);

	// Token: 0x04000C02 RID: 3074
	private bool _shouldTick;

	// Token: 0x0200018E RID: 398
	private struct LoadOpInfo
	{
		// Token: 0x06000A04 RID: 2564 RVA: 0x00096A68 File Offset: 0x00094C68
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

		// Token: 0x04000C04 RID: 3076
		public bool isStarted;

		// Token: 0x04000C05 RID: 3077
		public AsyncOperationHandle<GameObject> loadOp;

		// Token: 0x04000C06 RID: 3078
		public GameObject resultGObj;

		// Token: 0x04000C07 RID: 3079
		public readonly CosmeticAttachInfo attachInfo;

		// Token: 0x04000C08 RID: 3080
		public readonly CosmeticPart part;

		// Token: 0x04000C09 RID: 3081
		public readonly int partIndex;

		// Token: 0x04000C0A RID: 3082
		public readonly CosmeticInfoV2 cosmeticInfoV2;

		// Token: 0x04000C0B RID: 3083
		public readonly int vrRigIndex;
	}

	// Token: 0x0200018F RID: 399
	private struct VRRigData
	{
		// Token: 0x06000A05 RID: 2565 RVA: 0x00096AB4 File Offset: 0x00094CB4
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

		// Token: 0x04000C0C RID: 3084
		public readonly VRRig vrRig;

		// Token: 0x04000C0D RID: 3085
		public readonly Transform[] boneXforms;

		// Token: 0x04000C0E RID: 3086
		public readonly BodyDockPositions bdPositionsComp;

		// Token: 0x04000C0F RID: 3087
		public readonly List<GameObject> vrRig_cosmetics;

		// Token: 0x04000C10 RID: 3088
		public readonly List<GameObject> vrRig_override;

		// Token: 0x04000C11 RID: 3089
		public readonly Transform parentOfDeactivatedHoldables;

		// Token: 0x04000C12 RID: 3090
		public readonly List<TransferrableObject> bdPositions_allObjects;

		// Token: 0x04000C13 RID: 3091
		public int bdPositions_allObjects_length;

		// Token: 0x04000C14 RID: 3092
		public readonly List<GameObject> bdPositions_leftHandThrowables;

		// Token: 0x04000C15 RID: 3093
		public readonly List<GameObject> bdPositions_rightHandThrowables;
	}
}
