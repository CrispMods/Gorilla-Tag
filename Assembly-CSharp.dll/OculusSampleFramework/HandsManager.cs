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
		// (get) Token: 0x060041A2 RID: 16802 RVA: 0x0005A06A File Offset: 0x0005826A
		// (set) Token: 0x060041A3 RID: 16803 RVA: 0x0005A074 File Offset: 0x00058274
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
		// (get) Token: 0x060041A4 RID: 16804 RVA: 0x0005A07F File Offset: 0x0005827F
		// (set) Token: 0x060041A5 RID: 16805 RVA: 0x0005A089 File Offset: 0x00058289
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
		// (get) Token: 0x060041A6 RID: 16806 RVA: 0x0005A094 File Offset: 0x00058294
		// (set) Token: 0x060041A7 RID: 16807 RVA: 0x0005A09E File Offset: 0x0005829E
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
		// (get) Token: 0x060041A8 RID: 16808 RVA: 0x0005A0A9 File Offset: 0x000582A9
		// (set) Token: 0x060041A9 RID: 16809 RVA: 0x0005A0B3 File Offset: 0x000582B3
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
		// (get) Token: 0x060041AA RID: 16810 RVA: 0x0005A0BE File Offset: 0x000582BE
		// (set) Token: 0x060041AB RID: 16811 RVA: 0x0005A0C8 File Offset: 0x000582C8
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
		// (get) Token: 0x060041AC RID: 16812 RVA: 0x0005A0D3 File Offset: 0x000582D3
		// (set) Token: 0x060041AD RID: 16813 RVA: 0x0005A0DD File Offset: 0x000582DD
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
		// (get) Token: 0x060041AE RID: 16814 RVA: 0x0005A0E8 File Offset: 0x000582E8
		// (set) Token: 0x060041AF RID: 16815 RVA: 0x0005A0F2 File Offset: 0x000582F2
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
		// (get) Token: 0x060041B0 RID: 16816 RVA: 0x0005A0FD File Offset: 0x000582FD
		// (set) Token: 0x060041B1 RID: 16817 RVA: 0x0005A107 File Offset: 0x00058307
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
		// (get) Token: 0x060041B2 RID: 16818 RVA: 0x0005A112 File Offset: 0x00058312
		// (set) Token: 0x060041B3 RID: 16819 RVA: 0x0005A11C File Offset: 0x0005831C
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
		// (get) Token: 0x060041B4 RID: 16820 RVA: 0x0005A127 File Offset: 0x00058327
		// (set) Token: 0x060041B5 RID: 16821 RVA: 0x0005A131 File Offset: 0x00058331
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
		// (get) Token: 0x060041B6 RID: 16822 RVA: 0x0005A13C File Offset: 0x0005833C
		// (set) Token: 0x060041B7 RID: 16823 RVA: 0x0005A143 File Offset: 0x00058343
		public static HandsManager Instance { get; private set; }

		// Token: 0x060041B8 RID: 16824 RVA: 0x0016FCD0 File Offset: 0x0016DED0
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

		// Token: 0x060041B9 RID: 16825 RVA: 0x0016FDDC File Offset: 0x0016DFDC
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

		// Token: 0x060041BA RID: 16826 RVA: 0x0005A14B File Offset: 0x0005834B
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

		// Token: 0x060041BB RID: 16827 RVA: 0x0005A15A File Offset: 0x0005835A
		public void SwitchVisualization()
		{
			if (!this._leftSkeletonVisual || !this._rightSkeletonVisual)
			{
				return;
			}
			this.VisualMode = (this.VisualMode + 1) % (HandsManager.HandsVisualMode)3;
			this.SetToCurrentVisualMode();
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x0016FE58 File Offset: 0x0016E058
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

		// Token: 0x060041BD RID: 16829 RVA: 0x0016FF78 File Offset: 0x0016E178
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

		// Token: 0x060041BE RID: 16830 RVA: 0x0016FFC4 File Offset: 0x0016E1C4
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
