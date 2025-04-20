using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GorillaTag
{
	// Token: 0x02000BC1 RID: 3009
	[DefaultExecutionOrder(2000)]
	public class StaticLodManager : MonoBehaviour, IGorillaSliceableSimple
	{
		// Token: 0x06004C18 RID: 19480 RVA: 0x000620BD File Offset: 0x000602BD
		private void Awake()
		{
			StaticLodManager.gorillaInteractableLayer = UnityLayer.GorillaInteractable;
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x000620C6 File Offset: 0x000602C6
		public void OnEnable()
		{
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
			this.mainCamera = Camera.main;
			this.hasMainCamera = (this.mainCamera != null);
		}

		// Token: 0x06004C1A RID: 19482 RVA: 0x000320C8 File Offset: 0x000302C8
		public void OnDisable()
		{
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		}

		// Token: 0x06004C1B RID: 19483 RVA: 0x001A3500 File Offset: 0x001A1700
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

		// Token: 0x06004C1C RID: 19484 RVA: 0x001A3880 File Offset: 0x001A1A80
		public static void Unregister(int lodGroupIndex)
		{
			StaticLodManager.groupMonoBehaviours[lodGroupIndex] = null;
			StaticLodManager.groupInfos[lodGroupIndex] = default(StaticLodManager.GroupInfo);
			StaticLodManager.freeSlots.Push(lodGroupIndex);
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x001A38B8 File Offset: 0x001A1AB8
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

		// Token: 0x06004C1E RID: 19486 RVA: 0x001A38F0 File Offset: 0x001A1AF0
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

		// Token: 0x06004C21 RID: 19489 RVA: 0x00032105 File Offset: 0x00030305
		bool IGorillaSliceableSimple.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x04004D2D RID: 19757
		[OnEnterPlay_Clear]
		private static readonly List<StaticLodGroup> groupMonoBehaviours = new List<StaticLodGroup>(32);

		// Token: 0x04004D2E RID: 19758
		[DebugReadout]
		[OnEnterPlay_Clear]
		private static readonly List<StaticLodManager.GroupInfo> groupInfos = new List<StaticLodManager.GroupInfo>(32);

		// Token: 0x04004D2F RID: 19759
		[OnEnterPlay_Clear]
		private static readonly Stack<int> freeSlots = new Stack<int>();

		// Token: 0x04004D30 RID: 19760
		private static UnityLayer gorillaInteractableLayer;

		// Token: 0x04004D31 RID: 19761
		private Camera mainCamera;

		// Token: 0x04004D32 RID: 19762
		private bool hasMainCamera;

		// Token: 0x02000BC2 RID: 3010
		private struct GroupInfo
		{
			// Token: 0x04004D33 RID: 19763
			public bool isLoaded;

			// Token: 0x04004D34 RID: 19764
			public bool componentEnabled;

			// Token: 0x04004D35 RID: 19765
			public Vector3 center;

			// Token: 0x04004D36 RID: 19766
			public float radiusSq;

			// Token: 0x04004D37 RID: 19767
			public bool uiEnabled;

			// Token: 0x04004D38 RID: 19768
			public float uiEnableDistanceSq;

			// Token: 0x04004D39 RID: 19769
			public Text[] uiTexts;

			// Token: 0x04004D3A RID: 19770
			public TextMeshPro[] uiTMPs;

			// Token: 0x04004D3B RID: 19771
			public bool collidersEnabled;

			// Token: 0x04004D3C RID: 19772
			public float collisionEnableDistanceSq;

			// Token: 0x04004D3D RID: 19773
			public Collider[] interactableColliders;
		}
	}
}
