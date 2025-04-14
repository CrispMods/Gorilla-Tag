using System;
using System.Collections.Generic;
using Cysharp.Text;
using GorillaNetworking;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x020003D1 RID: 977
public class HeadModel : MonoBehaviour, IDelayedExecListener
{
	// Token: 0x0600177B RID: 6011 RVA: 0x00072610 File Offset: 0x00070810
	protected void Awake()
	{
		this._mannequinRenderer = base.GetComponentInChildren<Renderer>(true);
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x0007261F File Offset: 0x0007081F
	public void SetCosmeticActive(string playFabId, bool forRightSide = false)
	{
		this._ClearCurrent();
		this._AddPreviewCosmetic(playFabId, forRightSide);
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x00072630 File Offset: 0x00070830
	public void SetCosmeticActiveArray(string[] playFabIds, bool[] forRightSideArray)
	{
		this._ClearCurrent();
		for (int i = 0; i < playFabIds.Length; i++)
		{
			this._AddPreviewCosmetic(playFabIds[i], forRightSideArray[i]);
		}
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x00072660 File Offset: 0x00070860
	private void _AddPreviewCosmetic(string playFabId, bool forRightSide)
	{
		CosmeticInfoV2 cosmeticInfoV;
		if (!CosmeticsController.instance.TryGetCosmeticInfoV2(playFabId, out cosmeticInfoV))
		{
			if (!(playFabId == "null") && !(playFabId == "NOTHING") && !(playFabId == "Slingshot"))
			{
				Debug.LogError(ZString.Concat<string, string, string>("HeadModel._AddPreviewCosmetic: Cosmetic id \"", playFabId, "\" not found in `CosmeticsController`."), this);
			}
			return;
		}
		if (cosmeticInfoV.hideWardrobeMannequin)
		{
			this._mannequinRenderer.enabled = false;
		}
		foreach (CosmeticPart cosmeticPart in cosmeticInfoV.wardrobeParts)
		{
			foreach (CosmeticAttachInfo cosmeticAttachInfo in cosmeticPart.attachAnchors)
			{
				if ((!forRightSide || !(cosmeticAttachInfo.selectSide == ECosmeticSelectSide.Left)) && (forRightSide || !(cosmeticAttachInfo.selectSide == ECosmeticSelectSide.Right)))
				{
					HeadModel._CosmeticPartLoadInfo cosmeticPartLoadInfo = new HeadModel._CosmeticPartLoadInfo
					{
						playFabId = playFabId,
						prefabAssetRef = cosmeticPart.prefabAssetRef,
						attachInfo = cosmeticAttachInfo,
						loadOp = cosmeticPart.prefabAssetRef.InstantiateAsync(base.transform, false),
						xform = null
					};
					cosmeticPartLoadInfo.loadOp.Completed += this._HandleLoadOpOnCompleted;
					this._loadOp_to_partInfoIndex[cosmeticPartLoadInfo.loadOp] = this._currentPartLoadInfos.Count;
					this._currentPartLoadInfos.Add(cosmeticPartLoadInfo);
				}
			}
		}
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x000727E8 File Offset: 0x000709E8
	private void _HandleLoadOpOnCompleted(AsyncOperationHandle<GameObject> loadOp)
	{
		int num;
		if (!this._loadOp_to_partInfoIndex.TryGetValue(loadOp, out num))
		{
			if (loadOp.Status == AsyncOperationStatus.Succeeded && loadOp.Result)
			{
				Object.Destroy(loadOp.Result);
			}
			return;
		}
		HeadModel._CosmeticPartLoadInfo cosmeticPartLoadInfo = this._currentPartLoadInfos[num];
		if (loadOp.Status == AsyncOperationStatus.Failed)
		{
			Debug.Log("HeadModel: Failed to load a part for cosmetic \"" + cosmeticPartLoadInfo.playFabId + "\"! Waiting for 10 seconds before trying again.", this);
			GTDelayedExec.Add(this, 10f, num);
			return;
		}
		cosmeticPartLoadInfo.xform = loadOp.Result.transform;
		cosmeticPartLoadInfo.xform.localPosition = cosmeticPartLoadInfo.attachInfo.offset.pos;
		cosmeticPartLoadInfo.xform.localRotation = cosmeticPartLoadInfo.attachInfo.offset.rot;
		cosmeticPartLoadInfo.xform.localScale = cosmeticPartLoadInfo.attachInfo.offset.scale;
		cosmeticPartLoadInfo.xform.gameObject.SetActive(true);
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x000728E4 File Offset: 0x00070AE4
	void IDelayedExecListener.OnDelayedAction(int partLoadInfosIndex)
	{
		if (partLoadInfosIndex < 0 || partLoadInfosIndex >= this._currentPartLoadInfos.Count)
		{
			return;
		}
		HeadModel._CosmeticPartLoadInfo cosmeticPartLoadInfo = this._currentPartLoadInfos[partLoadInfosIndex];
		if (cosmeticPartLoadInfo.loadOp.Status != AsyncOperationStatus.Failed)
		{
			return;
		}
		cosmeticPartLoadInfo.loadOp.Completed += this._HandleLoadOpOnCompleted;
		cosmeticPartLoadInfo.loadOp = cosmeticPartLoadInfo.prefabAssetRef.InstantiateAsync(base.transform, false);
		this._loadOp_to_partInfoIndex[cosmeticPartLoadInfo.loadOp] = partLoadInfosIndex;
	}

	// Token: 0x06001781 RID: 6017 RVA: 0x0007296C File Offset: 0x00070B6C
	protected void _ClearCurrent()
	{
		for (int i = 0; i < this._currentPartLoadInfos.Count; i++)
		{
			Object.Destroy(this._currentPartLoadInfos[i].loadOp.Result);
		}
		this._EnsureCapacityAndClear<AsyncOperationHandle, int>(this._loadOp_to_partInfoIndex);
		this._EnsureCapacityAndClear<HeadModel._CosmeticPartLoadInfo>(this._currentPartLoadInfos);
		this._mannequinRenderer.enabled = true;
	}

	// Token: 0x06001782 RID: 6018 RVA: 0x000729D1 File Offset: 0x00070BD1
	private void _EnsureCapacityAndClear<T>(List<T> list)
	{
		if (list.Count > list.Capacity)
		{
			list.Capacity = list.Count;
		}
		list.Clear();
	}

	// Token: 0x06001783 RID: 6019 RVA: 0x000729F3 File Offset: 0x00070BF3
	private void _EnsureCapacityAndClear<T1, T2>(Dictionary<T1, T2> dict)
	{
		dict.EnsureCapacity(dict.Count);
		dict.Clear();
	}

	// Token: 0x04001A30 RID: 6704
	[DebugReadout]
	protected readonly List<HeadModel._CosmeticPartLoadInfo> _currentPartLoadInfos = new List<HeadModel._CosmeticPartLoadInfo>(1);

	// Token: 0x04001A31 RID: 6705
	[DebugReadout]
	private readonly Dictionary<AsyncOperationHandle, int> _loadOp_to_partInfoIndex = new Dictionary<AsyncOperationHandle, int>(1);

	// Token: 0x04001A32 RID: 6706
	private Renderer _mannequinRenderer;

	// Token: 0x04001A33 RID: 6707
	public GameObject[] cosmetics;

	// Token: 0x020003D2 RID: 978
	protected struct _CosmeticPartLoadInfo
	{
		// Token: 0x04001A34 RID: 6708
		public string playFabId;

		// Token: 0x04001A35 RID: 6709
		public GTAssetRef<GameObject> prefabAssetRef;

		// Token: 0x04001A36 RID: 6710
		public CosmeticAttachInfo attachInfo;

		// Token: 0x04001A37 RID: 6711
		public AsyncOperationHandle<GameObject> loadOp;

		// Token: 0x04001A38 RID: 6712
		public Transform xform;
	}
}
