using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A4E RID: 2638
	public class HandsManager : MonoBehaviour
	{
		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060041A2 RID: 16802 RVA: 0x00137338 File Offset: 0x00135538
		// (set) Token: 0x060041A3 RID: 16803 RVA: 0x00137342 File Offset: 0x00135542
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

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060041A4 RID: 16804 RVA: 0x0013734D File Offset: 0x0013554D
		// (set) Token: 0x060041A5 RID: 16805 RVA: 0x00137357 File Offset: 0x00135557
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

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060041A6 RID: 16806 RVA: 0x00137362 File Offset: 0x00135562
		// (set) Token: 0x060041A7 RID: 16807 RVA: 0x0013736C File Offset: 0x0013556C
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

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060041A8 RID: 16808 RVA: 0x00137377 File Offset: 0x00135577
		// (set) Token: 0x060041A9 RID: 16809 RVA: 0x00137381 File Offset: 0x00135581
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

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060041AA RID: 16810 RVA: 0x0013738C File Offset: 0x0013558C
		// (set) Token: 0x060041AB RID: 16811 RVA: 0x00137396 File Offset: 0x00135596
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

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060041AC RID: 16812 RVA: 0x001373A1 File Offset: 0x001355A1
		// (set) Token: 0x060041AD RID: 16813 RVA: 0x001373AB File Offset: 0x001355AB
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

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060041AE RID: 16814 RVA: 0x001373B6 File Offset: 0x001355B6
		// (set) Token: 0x060041AF RID: 16815 RVA: 0x001373C0 File Offset: 0x001355C0
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

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060041B0 RID: 16816 RVA: 0x001373CB File Offset: 0x001355CB
		// (set) Token: 0x060041B1 RID: 16817 RVA: 0x001373D5 File Offset: 0x001355D5
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

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060041B2 RID: 16818 RVA: 0x001373E0 File Offset: 0x001355E0
		// (set) Token: 0x060041B3 RID: 16819 RVA: 0x001373EA File Offset: 0x001355EA
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

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060041B4 RID: 16820 RVA: 0x001373F5 File Offset: 0x001355F5
		// (set) Token: 0x060041B5 RID: 16821 RVA: 0x001373FF File Offset: 0x001355FF
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

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060041B6 RID: 16822 RVA: 0x0013740A File Offset: 0x0013560A
		// (set) Token: 0x060041B7 RID: 16823 RVA: 0x00137411 File Offset: 0x00135611
		public static HandsManager Instance { get; private set; }

		// Token: 0x060041B8 RID: 16824 RVA: 0x0013741C File Offset: 0x0013561C
		private void Awake()
		{
			if (HandsManager.Instance && HandsManager.Instance != this)
			{
				Object.Destroy(this);
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

		// Token: 0x060041B9 RID: 16825 RVA: 0x00137528 File Offset: 0x00135728
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

		// Token: 0x060041BA RID: 16826 RVA: 0x001375A3 File Offset: 0x001357A3
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

		// Token: 0x060041BB RID: 16827 RVA: 0x001375B2 File Offset: 0x001357B2
		public void SwitchVisualization()
		{
			if (!this._leftSkeletonVisual || !this._rightSkeletonVisual)
			{
				return;
			}
			this.VisualMode = (this.VisualMode + 1) % (HandsManager.HandsVisualMode)3;
			this.SetToCurrentVisualMode();
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x001375E8 File Offset: 0x001357E8
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

		// Token: 0x060041BD RID: 16829 RVA: 0x00137708 File Offset: 0x00135908
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

		// Token: 0x060041BE RID: 16830 RVA: 0x00137754 File Offset: 0x00135954
		public bool IsInitialized()
		{
			return this.LeftHandSkeleton && this.LeftHandSkeleton.IsInitialized && this.RightHandSkeleton && this.RightHandSkeleton.IsInitialized && this.LeftHandMesh && this.LeftHandMesh.IsInitialized && this.RightHandMesh && this.RightHandMesh.IsInitialized;
		}

		// Token: 0x040042CA RID: 17098
		private const string SKELETON_VISUALIZER_NAME = "SkeletonRenderer";

		// Token: 0x040042CB RID: 17099
		[SerializeField]
		private GameObject _leftHand;

		// Token: 0x040042CC RID: 17100
		[SerializeField]
		private GameObject _rightHand;

		// Token: 0x040042CD RID: 17101
		public HandsManager.HandsVisualMode VisualMode;

		// Token: 0x040042CE RID: 17102
		private OVRHand[] _hand = new OVRHand[2];

		// Token: 0x040042CF RID: 17103
		private OVRSkeleton[] _handSkeleton = new OVRSkeleton[2];

		// Token: 0x040042D0 RID: 17104
		private OVRSkeletonRenderer[] _handSkeletonRenderer = new OVRSkeletonRenderer[2];

		// Token: 0x040042D1 RID: 17105
		private OVRMesh[] _handMesh = new OVRMesh[2];

		// Token: 0x040042D2 RID: 17106
		private OVRMeshRenderer[] _handMeshRenderer = new OVRMeshRenderer[2];

		// Token: 0x040042D3 RID: 17107
		private SkinnedMeshRenderer _leftMeshRenderer;

		// Token: 0x040042D4 RID: 17108
		private SkinnedMeshRenderer _rightMeshRenderer;

		// Token: 0x040042D5 RID: 17109
		private GameObject _leftSkeletonVisual;

		// Token: 0x040042D6 RID: 17110
		private GameObject _rightSkeletonVisual;

		// Token: 0x040042D7 RID: 17111
		private float _currentHandAlpha = 1f;

		// Token: 0x040042D8 RID: 17112
		private int HandAlphaId = Shader.PropertyToID("_HandAlpha");

		// Token: 0x02000A4F RID: 2639
		public enum HandsVisualMode
		{
			// Token: 0x040042DB RID: 17115
			Mesh,
			// Token: 0x040042DC RID: 17116
			Skeleton,
			// Token: 0x040042DD RID: 17117
			Both
		}
	}
}
