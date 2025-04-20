using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A78 RID: 2680
	public class HandsManager : MonoBehaviour
	{
		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x060042DB RID: 17115 RVA: 0x0005BA6C File Offset: 0x00059C6C
		// (set) Token: 0x060042DC RID: 17116 RVA: 0x0005BA76 File Offset: 0x00059C76
		public OVRHand RightHand
		{
			get
			{
				return this._hand[1];
			}
			private set
			{
				this._hand[1] = value;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x060042DD RID: 17117 RVA: 0x0005BA81 File Offset: 0x00059C81
		// (set) Token: 0x060042DE RID: 17118 RVA: 0x0005BA8B File Offset: 0x00059C8B
		public OVRSkeleton RightHandSkeleton
		{
			get
			{
				return this._handSkeleton[1];
			}
			private set
			{
				this._handSkeleton[1] = value;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x060042DF RID: 17119 RVA: 0x0005BA96 File Offset: 0x00059C96
		// (set) Token: 0x060042E0 RID: 17120 RVA: 0x0005BAA0 File Offset: 0x00059CA0
		public OVRSkeletonRenderer RightHandSkeletonRenderer
		{
			get
			{
				return this._handSkeletonRenderer[1];
			}
			private set
			{
				this._handSkeletonRenderer[1] = value;
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x060042E1 RID: 17121 RVA: 0x0005BAAB File Offset: 0x00059CAB
		// (set) Token: 0x060042E2 RID: 17122 RVA: 0x0005BAB5 File Offset: 0x00059CB5
		public OVRMesh RightHandMesh
		{
			get
			{
				return this._handMesh[1];
			}
			private set
			{
				this._handMesh[1] = value;
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x060042E3 RID: 17123 RVA: 0x0005BAC0 File Offset: 0x00059CC0
		// (set) Token: 0x060042E4 RID: 17124 RVA: 0x0005BACA File Offset: 0x00059CCA
		public OVRMeshRenderer RightHandMeshRenderer
		{
			get
			{
				return this._handMeshRenderer[1];
			}
			private set
			{
				this._handMeshRenderer[1] = value;
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x060042E5 RID: 17125 RVA: 0x0005BAD5 File Offset: 0x00059CD5
		// (set) Token: 0x060042E6 RID: 17126 RVA: 0x0005BADF File Offset: 0x00059CDF
		public OVRHand LeftHand
		{
			get
			{
				return this._hand[0];
			}
			private set
			{
				this._hand[0] = value;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x060042E7 RID: 17127 RVA: 0x0005BAEA File Offset: 0x00059CEA
		// (set) Token: 0x060042E8 RID: 17128 RVA: 0x0005BAF4 File Offset: 0x00059CF4
		public OVRSkeleton LeftHandSkeleton
		{
			get
			{
				return this._handSkeleton[0];
			}
			private set
			{
				this._handSkeleton[0] = value;
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x060042E9 RID: 17129 RVA: 0x0005BAFF File Offset: 0x00059CFF
		// (set) Token: 0x060042EA RID: 17130 RVA: 0x0005BB09 File Offset: 0x00059D09
		public OVRSkeletonRenderer LeftHandSkeletonRenderer
		{
			get
			{
				return this._handSkeletonRenderer[0];
			}
			private set
			{
				this._handSkeletonRenderer[0] = value;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x060042EB RID: 17131 RVA: 0x0005BB14 File Offset: 0x00059D14
		// (set) Token: 0x060042EC RID: 17132 RVA: 0x0005BB1E File Offset: 0x00059D1E
		public OVRMesh LeftHandMesh
		{
			get
			{
				return this._handMesh[0];
			}
			private set
			{
				this._handMesh[0] = value;
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x060042ED RID: 17133 RVA: 0x0005BB29 File Offset: 0x00059D29
		// (set) Token: 0x060042EE RID: 17134 RVA: 0x0005BB33 File Offset: 0x00059D33
		public OVRMeshRenderer LeftHandMeshRenderer
		{
			get
			{
				return this._handMeshRenderer[0];
			}
			private set
			{
				this._handMeshRenderer[0] = value;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x060042EF RID: 17135 RVA: 0x0005BB3E File Offset: 0x00059D3E
		// (set) Token: 0x060042F0 RID: 17136 RVA: 0x0005BB45 File Offset: 0x00059D45
		public static HandsManager Instance { get; private set; }

		// Token: 0x060042F1 RID: 17137 RVA: 0x00176B54 File Offset: 0x00174D54
		private void Awake()
		{
			if (HandsManager.Instance && HandsManager.Instance != this)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			HandsManager.Instance = this;
			this.LeftHand = this._leftHand.GetComponent<OVRHand>();
			this.LeftHandSkeleton = this._leftHand.GetComponent<OVRSkeleton>();
			this.LeftHandSkeletonRenderer = this._leftHand.GetComponent<OVRSkeletonRenderer>();
			this.LeftHandMesh = this._leftHand.GetComponent<OVRMesh>();
			this.LeftHandMeshRenderer = this._leftHand.GetComponent<OVRMeshRenderer>();
			this.RightHand = this._rightHand.GetComponent<OVRHand>();
			this.RightHandSkeleton = this._rightHand.GetComponent<OVRSkeleton>();
			this.RightHandSkeletonRenderer = this._rightHand.GetComponent<OVRSkeletonRenderer>();
			this.RightHandMesh = this._rightHand.GetComponent<OVRMesh>();
			this.RightHandMeshRenderer = this._rightHand.GetComponent<OVRMeshRenderer>();
			this._leftMeshRenderer = this.LeftHand.GetComponent<SkinnedMeshRenderer>();
			this._rightMeshRenderer = this.RightHand.GetComponent<SkinnedMeshRenderer>();
			base.StartCoroutine(this.FindSkeletonVisualGameObjects());
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x00176C60 File Offset: 0x00174E60
		private void Update()
		{
			HandsManager.HandsVisualMode visualMode = this.VisualMode;
			if (visualMode > HandsManager.HandsVisualMode.Skeleton)
			{
				if (visualMode != HandsManager.HandsVisualMode.Both)
				{
					this._currentHandAlpha = 1f;
				}
				else
				{
					this._currentHandAlpha = 0.6f;
				}
			}
			else
			{
				this._currentHandAlpha = 1f;
			}
			this._rightMeshRenderer.sharedMaterial.SetFloat(this.HandAlphaId, this._currentHandAlpha);
			this._leftMeshRenderer.sharedMaterial.SetFloat(this.HandAlphaId, this._currentHandAlpha);
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x0005BB4D File Offset: 0x00059D4D
		private IEnumerator FindSkeletonVisualGameObjects()
		{
			while (!this._leftSkeletonVisual || !this._rightSkeletonVisual)
			{
				if (!this._leftSkeletonVisual)
				{
					Transform transform = this.LeftHand.transform.Find("SkeletonRenderer");
					if (transform)
					{
						this._leftSkeletonVisual = transform.gameObject;
					}
				}
				if (!this._rightSkeletonVisual)
				{
					Transform transform2 = this.RightHand.transform.Find("SkeletonRenderer");
					if (transform2)
					{
						this._rightSkeletonVisual = transform2.gameObject;
					}
				}
				yield return null;
			}
			this.SetToCurrentVisualMode();
			yield break;
		}

		// Token: 0x060042F4 RID: 17140 RVA: 0x0005BB5C File Offset: 0x00059D5C
		public void SwitchVisualization()
		{
			if (!this._leftSkeletonVisual || !this._rightSkeletonVisual)
			{
				return;
			}
			this.VisualMode = (this.VisualMode + 1) % (HandsManager.HandsVisualMode)3;
			this.SetToCurrentVisualMode();
		}

		// Token: 0x060042F5 RID: 17141 RVA: 0x00176CDC File Offset: 0x00174EDC
		private void SetToCurrentVisualMode()
		{
			switch (this.VisualMode)
			{
			case HandsManager.HandsVisualMode.Mesh:
				this.RightHandMeshRenderer.enabled = true;
				this._rightMeshRenderer.enabled = true;
				this._rightSkeletonVisual.gameObject.SetActive(false);
				this.LeftHandMeshRenderer.enabled = true;
				this._leftMeshRenderer.enabled = true;
				this._leftSkeletonVisual.gameObject.SetActive(false);
				return;
			case HandsManager.HandsVisualMode.Skeleton:
				this.RightHandMeshRenderer.enabled = false;
				this._rightMeshRenderer.enabled = false;
				this._rightSkeletonVisual.gameObject.SetActive(true);
				this.LeftHandMeshRenderer.enabled = false;
				this._leftMeshRenderer.enabled = false;
				this._leftSkeletonVisual.gameObject.SetActive(true);
				return;
			case HandsManager.HandsVisualMode.Both:
				this.RightHandMeshRenderer.enabled = true;
				this._rightMeshRenderer.enabled = true;
				this._rightSkeletonVisual.gameObject.SetActive(true);
				this.LeftHandMeshRenderer.enabled = true;
				this._leftMeshRenderer.enabled = true;
				this._leftSkeletonVisual.gameObject.SetActive(true);
				return;
			default:
				return;
			}
		}

		// Token: 0x060042F6 RID: 17142 RVA: 0x00176DFC File Offset: 0x00174FFC
		public static List<OVRBoneCapsule> GetCapsulesPerBone(OVRSkeleton skeleton, OVRSkeleton.BoneId boneId)
		{
			List<OVRBoneCapsule> list = new List<OVRBoneCapsule>();
			IList<OVRBoneCapsule> capsules = skeleton.Capsules;
			for (int i = 0; i < capsules.Count; i++)
			{
				if (capsules[i].BoneIndex == (short)boneId)
				{
					list.Add(capsules[i]);
				}
			}
			return list;
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x00176E48 File Offset: 0x00175048
		public bool IsInitialized()
		{
			return this.LeftHandSkeleton && this.LeftHandSkeleton.IsInitialized && this.RightHandSkeleton && this.RightHandSkeleton.IsInitialized && this.LeftHandMesh && this.LeftHandMesh.IsInitialized && this.RightHandMesh && this.RightHandMesh.IsInitialized;
		}

		// Token: 0x040043B2 RID: 17330
		private const string SKELETON_VISUALIZER_NAME = "SkeletonRenderer";

		// Token: 0x040043B3 RID: 17331
		[SerializeField]
		private GameObject _leftHand;

		// Token: 0x040043B4 RID: 17332
		[SerializeField]
		private GameObject _rightHand;

		// Token: 0x040043B5 RID: 17333
		public HandsManager.HandsVisualMode VisualMode;

		// Token: 0x040043B6 RID: 17334
		private OVRHand[] _hand = new OVRHand[2];

		// Token: 0x040043B7 RID: 17335
		private OVRSkeleton[] _handSkeleton = new OVRSkeleton[2];

		// Token: 0x040043B8 RID: 17336
		private OVRSkeletonRenderer[] _handSkeletonRenderer = new OVRSkeletonRenderer[2];

		// Token: 0x040043B9 RID: 17337
		private OVRMesh[] _handMesh = new OVRMesh[2];

		// Token: 0x040043BA RID: 17338
		private OVRMeshRenderer[] _handMeshRenderer = new OVRMeshRenderer[2];

		// Token: 0x040043BB RID: 17339
		private SkinnedMeshRenderer _leftMeshRenderer;

		// Token: 0x040043BC RID: 17340
		private SkinnedMeshRenderer _rightMeshRenderer;

		// Token: 0x040043BD RID: 17341
		private GameObject _leftSkeletonVisual;

		// Token: 0x040043BE RID: 17342
		private GameObject _rightSkeletonVisual;

		// Token: 0x040043BF RID: 17343
		private float _currentHandAlpha = 1f;

		// Token: 0x040043C0 RID: 17344
		private int HandAlphaId = Shader.PropertyToID("_HandAlpha");

		// Token: 0x02000A79 RID: 2681
		public enum HandsVisualMode
		{
			// Token: 0x040043C3 RID: 17347
			Mesh,
			// Token: 0x040043C4 RID: 17348
			Skeleton,
			// Token: 0x040043C5 RID: 17349
			Both
		}
	}
}
