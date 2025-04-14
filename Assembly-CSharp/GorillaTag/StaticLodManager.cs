using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GorillaTag
{
	// Token: 0x02000B94 RID: 2964
	[DefaultExecutionOrder(2000)]
	public class StaticLodManager : MonoBehaviour, IGorillaSliceableSimple
	{
		// Token: 0x06004ACD RID: 19149 RVA: 0x00169DAD File Offset: 0x00167FAD
		private void Awake()
		{
			StaticLodManager.gorillaInteractableLayer = UnityLayer.GorillaInteractable;
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x00169DB6 File Offset: 0x00167FB6
		public void OnEnable()
		{
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
			this.mainCamera = Camera.main;
			this.hasMainCamera = (this.mainCamera != null);
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x0000F86B File Offset: 0x0000DA6B
		public void OnDisable()
		{
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x00169DDC File Offset: 0x00167FDC
		public static int Register(StaticLodGroup lodGroup)
		{
			StaticLodGroupExcluder componentInParent = lodGroup.GetComponentInParent<StaticLodGroupExcluder>();
			Text[] array = lodGroup.GetComponentsInChildren<Text>(true);
			List<Text> list = new List<Text>(array.Length);
			foreach (Text text in array)
			{
				StaticLodGroupExcluder componentInParent2 = text.GetComponentInParent<StaticLodGroupExcluder>();
				if (!(componentInParent2 != null) || !(componentInParent2 != componentInParent))
				{
					list.Add(text);
				}
			}
			array = list.ToArray();
			TextMeshPro[] array3 = lodGroup.GetComponentsInChildren<TextMeshPro>(true);
			List<TextMeshPro> list2 = new List<TextMeshPro>(array3.Length);
			foreach (TextMeshPro textMeshPro in array3)
			{
				StaticLodGroupExcluder componentInParent3 = textMeshPro.GetComponentInParent<StaticLodGroupExcluder>();
				if (!(componentInParent3 != null) || !(componentInParent3 != componentInParent))
				{
					list2.Add(textMeshPro);
				}
			}
			array3 = list2.ToArray();
			Collider[] componentsInChildren = lodGroup.GetComponentsInChildren<Collider>(true);
			List<Collider> list3 = new List<Collider>(componentsInChildren.Length);
			foreach (Collider collider in componentsInChildren)
			{
				if (collider.gameObject.IsOnLayer(StaticLodManager.gorillaInteractableLayer))
				{
					StaticLodGroupExcluder componentInParent4 = collider.GetComponentInParent<StaticLodGroupExcluder>();
					if (!(componentInParent4 != null) || !(componentInParent4 != componentInParent))
					{
						list3.Add(collider);
					}
				}
			}
			Bounds bounds;
			if (array.Length != 0)
			{
				bounds = new Bounds(array[0].transform.position, Vector3.one * 0.01f);
			}
			else if (array3.Length != 0)
			{
				bounds = new Bounds(array3[0].transform.position, Vector3.one * 0.01f);
			}
			else if (list3.Count > 0)
			{
				bounds = new Bounds(list3[0].bounds.center, list3[0].bounds.size);
			}
			else
			{
				bounds = new Bounds(lodGroup.transform.position, Vector3.one * 0.01f);
			}
			foreach (Text text2 in array)
			{
				bounds.Encapsulate(text2.transform.position);
			}
			foreach (TextMeshPro textMeshPro2 in array3)
			{
				bounds.Encapsulate(textMeshPro2.transform.position);
			}
			foreach (Collider collider2 in list3)
			{
				bounds.Encapsulate(collider2.bounds);
			}
			StaticLodManager.GroupInfo groupInfo = new StaticLodManager.GroupInfo
			{
				isLoaded = true,
				componentEnabled = lodGroup.isActiveAndEnabled,
				center = bounds.center,
				radiusSq = bounds.extents.sqrMagnitude,
				uiEnabled = true,
				uiEnableDistanceSq = lodGroup.uiFadeDistanceMax * lodGroup.uiFadeDistanceMax,
				uiTexts = array,
				uiTMPs = array3,
				collidersEnabled = true,
				collisionEnableDistanceSq = lodGroup.collisionEnableDistance * lodGroup.collisionEnableDistance,
				interactableColliders = list3.ToArray()
			};
			int count;
			if (StaticLodManager.freeSlots.TryPop(out count))
			{
				StaticLodManager.groupMonoBehaviours[count] = lodGroup;
				StaticLodManager.groupInfos[count] = groupInfo;
			}
			else
			{
				count = StaticLodManager.groupMonoBehaviours.Count;
				StaticLodManager.groupMonoBehaviours.Add(lodGroup);
				StaticLodManager.groupInfos.Add(groupInfo);
			}
			return count;
		}

		// Token: 0x06004AD1 RID: 19153 RVA: 0x0016A15C File Offset: 0x0016835C
		public static void Unregister(int lodGroupIndex)
		{
			StaticLodManager.groupMonoBehaviours[lodGroupIndex] = null;
			StaticLodManager.groupInfos[lodGroupIndex] = default(StaticLodManager.GroupInfo);
			StaticLodManager.freeSlots.Push(lodGroupIndex);
		}

		// Token: 0x06004AD2 RID: 19154 RVA: 0x0016A194 File Offset: 0x00168394
		public static void SetEnabled(int index, bool enable)
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			StaticLodManager.GroupInfo value = StaticLodManager.groupInfos[index];
			value.componentEnabled = enable;
			StaticLodManager.groupInfos[index] = value;
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x0016A1CC File Offset: 0x001683CC
		public void SliceUpdate()
		{
			if (!this.hasMainCamera)
			{
				return;
			}
			Vector3 position = this.mainCamera.transform.position;
			for (int i = 0; i < StaticLodManager.groupInfos.Count; i++)
			{
				StaticLodManager.GroupInfo groupInfo = StaticLodManager.groupInfos[i];
				if (groupInfo.isLoaded && groupInfo.componentEnabled)
				{
					float num = Mathf.Max(0f, (groupInfo.center - position).sqrMagnitude - groupInfo.radiusSq);
					float num2 = groupInfo.uiEnabled ? 0.010000001f : 0f;
					bool flag = num < groupInfo.uiEnableDistanceSq + num2;
					if (flag != groupInfo.uiEnabled)
					{
						for (int j = 0; j < groupInfo.uiTexts.Length; j++)
						{
							Text text = groupInfo.uiTexts[j];
							if (!(text == null))
							{
								text.enabled = flag;
							}
						}
						for (int k = 0; k < groupInfo.uiTMPs.Length; k++)
						{
							TextMeshPro textMeshPro = groupInfo.uiTMPs[k];
							if (!(textMeshPro == null))
							{
								textMeshPro.enabled = flag;
							}
						}
					}
					groupInfo.uiEnabled = flag;
					num2 = (groupInfo.collidersEnabled ? 0.010000001f : 0f);
					bool flag2 = num < groupInfo.collisionEnableDistanceSq + num2;
					if (flag2 != groupInfo.collidersEnabled)
					{
						for (int l = 0; l < groupInfo.interactableColliders.Length; l++)
						{
							if (!(groupInfo.interactableColliders[l] == null))
							{
								groupInfo.interactableColliders[l].enabled = flag2;
							}
						}
					}
					groupInfo.collidersEnabled = flag2;
					StaticLodManager.groupInfos[i] = groupInfo;
				}
			}
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x0000F974 File Offset: 0x0000DB74
		bool IGorillaSliceableSimple.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x04004C37 RID: 19511
		[OnEnterPlay_Clear]
		private static readonly List<StaticLodGroup> groupMonoBehaviours = new List<StaticLodGroup>(32);

		// Token: 0x04004C38 RID: 19512
		[DebugReadout]
		[OnEnterPlay_Clear]
		private static readonly List<StaticLodManager.GroupInfo> groupInfos = new List<StaticLodManager.GroupInfo>(32);

		// Token: 0x04004C39 RID: 19513
		[OnEnterPlay_Clear]
		private static readonly Stack<int> freeSlots = new Stack<int>();

		// Token: 0x04004C3A RID: 19514
		private static UnityLayer gorillaInteractableLayer;

		// Token: 0x04004C3B RID: 19515
		private Camera mainCamera;

		// Token: 0x04004C3C RID: 19516
		private bool hasMainCamera;

		// Token: 0x02000B95 RID: 2965
		private struct GroupInfo
		{
			// Token: 0x04004C3D RID: 19517
			public bool isLoaded;

			// Token: 0x04004C3E RID: 19518
			public bool componentEnabled;

			// Token: 0x04004C3F RID: 19519
			public Vector3 center;

			// Token: 0x04004C40 RID: 19520
			public float radiusSq;

			// Token: 0x04004C41 RID: 19521
			public bool uiEnabled;

			// Token: 0x04004C42 RID: 19522
			public float uiEnableDistanceSq;

			// Token: 0x04004C43 RID: 19523
			public Text[] uiTexts;

			// Token: 0x04004C44 RID: 19524
			public TextMeshPro[] uiTMPs;

			// Token: 0x04004C45 RID: 19525
			public bool collidersEnabled;

			// Token: 0x04004C46 RID: 19526
			public float collisionEnableDistanceSq;

			// Token: 0x04004C47 RID: 19527
			public Collider[] interactableColliders;
		}
	}
}
