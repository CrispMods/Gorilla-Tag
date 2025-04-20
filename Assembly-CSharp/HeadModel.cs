using System;
using System.Collections.Generic;
using Cysharp.Text;
using GorillaNetworking;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x020003DC RID: 988
public class HeadModel : MonoBehaviour, IDelayedExecListener
{
	// Token: 0x060017C8 RID: 6088 RVA: 0x00040177 File Offset: 0x0003E377
	protected void Awake()
	{
		this._mannequinRenderer = base.GetComponentInChildren<Renderer>(true);
	}

	// Token: 0x060017C9 RID: 6089 RVA: 0x00040186 File Offset: 0x0003E386
	public void SetCosmeticActive(string playFabId, bool forRightSide = false)
	{
		this._ClearCurrent();
		this._AddPreviewCosmetic(playFabId, forRightSide);
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x000C9538 File Offset: 0x000C7738
	public void SetCosmeticActiveArray(string[] playFabIds, bool[] forRightSideArray)
	{
		this._ClearCurrent();
		for (int i = 0; i < playFabIds.Length; i++)
		{
			this._AddPreviewCosmetic(playFabIds[i], forRightSideArray[i]);
		}
	}

	// Token: 0x060017CB RID: 6091 RVA: 0x000C9568 File Offset: 0x000C7768
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

	// Token: 0x060017CC RID: 6092 RVA: 0x000C96F0 File Offset: 0x000C78F0
	private void _HandleLoadOpOnCompleted(AsyncOperationHandle<GameObject> loadOp)
	{
		int num;
		if (!this._loadOp_to_partInfoIndex.TryGetValue(loadOp, out num))
		{
			if (loadOp.Status == AsyncOperationStatus.Succeeded && loadOp.Result)
			{
				UnityEngine.Object.Destroy(loadOp.Result);
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

	// Token: 0x060017CD RID: 6093 RVA: 0x000C97EC File Offset: 0x000C79EC
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

	// Token: 0x060017CE RID: 6094 RVA: 0x000C9874 File Offset: 0x000C7A74
	protected void _ClearCurrent()
	{
		for (int i = 0; i < this._currentPartLoadInfos.Count; i++)
		{
			UnityEngine.Object.Destroy(this._currentPartLoadInfos[i].loadOp.Result);
		}
		this._EnsureCapacityAndClear<AsyncOperationHandle, int>(this._loadOp_to_partInfoIndex);
		this._EnsureCapacityAndClear<HeadModel._CosmeticPartLoadInfo>(this._currentPartLoadInfos);
		this._mannequinRenderer.enabled = true;
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x00040196 File Offset: 0x0003E396
	private void _EnsureCapacityAndClear<T>(List<T> list)
	{
		if (list.Count > list.Capacity)
		{
			list.Capacity = list.Count;
		}
		list.Clear();
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x000401B8 File Offset: 0x0003E3B8
	private void _EnsureCapacityAndClear<T1, T2>(Dictionary<T1, T2> dict)
	{
		dict.EnsureCapacity(dict.Count);
		dict.Clear();
	}

	// Token: 0x04001A79 RID: 6777
	[DebugReadout]
	protected readonly List<HeadModel._CosmeticPartLoadInfo> _currentPartLoadInfos = new List<HeadModel._CosmeticPartLoadInfo>(1);

	// Token: 0x04001A7A RID: 6778
	[DebugReadout]
	private readonly Dictionary<AsyncOperationHandle, int> _loadOp_to_partInfoIndex = new Dictionary<AsyncOperationHandle, int>(1);

	// Token: 0x04001A7B RID: 6779
	private Renderer _mannequinRenderer;

	// Token: 0x04001A7C RID: 6780
	public GameObject[] cosmetics;

	// Token: 0x020003DD RID: 989
	protected struct _CosmeticPartLoadInfo
	{
		// Token: 0x04001A7D RID: 6781
		public string playFabId;

		// Token: 0x04001A7E RID: 6782
		public GTAssetRef<GameObject> prefabAssetRef;

		// Token: 0x04001A7F RID: 6783
		public CosmeticAttachInfo attachInfo;

		// Token: 0x04001A80 RID: 6784
		public AsyncOperationHandle<GameObject> loadOp;

		// Token: 0x04001A81 RID: 6785
		public Transform xform;
	}
}
