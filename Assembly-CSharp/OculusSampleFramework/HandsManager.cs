using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A4B RID: 2635
	public class HandsManager : MonoBehaviour
	{
		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06004196 RID: 16790 RVA: 0x00136D70 File Offset: 0x00134F70
		// (set) Token: 0x06004197 RID: 16791 RVA: 0x00136D7A File Offset: 0x00134F7A
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

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06004198 RID: 16792 RVA: 0x00136D85 File Offset: 0x00134F85
		// (set) Token: 0x06004199 RID: 16793 RVA: 0x00136D8F File Offset: 0x00134F8F
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

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600419A RID: 16794 RVA: 0x00136D9A File Offset: 0x00134F9A
		// (set) Token: 0x0600419B RID: 16795 RVA: 0x00136DA4 File Offset: 0x00134FA4
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

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x0600419C RID: 16796 RVA: 0x00136DAF File Offset: 0x00134FAF
		// (set) Token: 0x0600419D RID: 16797 RVA: 0x00136DB9 File Offset: 0x00134FB9
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

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600419E RID: 16798 RVA: 0x00136DC4 File Offset: 0x00134FC4
		// (set) Token: 0x0600419F RID: 16799 RVA: 0x00136DCE File Offset: 0x00134FCE
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

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060041A0 RID: 16800 RVA: 0x00136DD9 File Offset: 0x00134FD9
		// (set) Token: 0x060041A1 RID: 16801 RVA: 0x00136DE3 File Offset: 0x00134FE3
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

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060041A2 RID: 16802 RVA: 0x00136DEE File Offset: 0x00134FEE
		// (set) Token: 0x060041A3 RID: 16803 RVA: 0x00136DF8 File Offset: 0x00134FF8
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

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060041A4 RID: 16804 RVA: 0x00136E03 File Offset: 0x00135003
		// (set) Token: 0x060041A5 RID: 16805 RVA: 0x00136E0D File Offset: 0x0013500D
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

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060041A6 RID: 16806 RVA: 0x00136E18 File Offset: 0x00135018
		// (set) Token: 0x060041A7 RID: 16807 RVA: 0x00136E22 File Offset: 0x00135022
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

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060041A8 RID: 16808 RVA: 0x00136E2D File Offset: 0x0013502D
		// (set) Token: 0x060041A9 RID: 16809 RVA: 0x00136E37 File Offset: 0x00135037
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

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060041AA RID: 16810 RVA: 0x00136E42 File Offset: 0x00135042
		// (set) Token: 0x060041AB RID: 16811 RVA: 0x00136E49 File Offset: 0x00135049
		public static HandsManager Instance { get; private set; }

		// Token: 0x060041AC RID: 16812 RVA: 0x00136E54 File Offset: 0x00135054
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

		// Token: 0x060041AD RID: 16813 RVA: 0x00136F60 File Offset: 0x00135160
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

		// Token: 0x060041AE RID: 16814 RVA: 0x00136FDB File Offset: 0x001351DB
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

		// Token: 0x060041AF RID: 16815 RVA: 0x00136FEA File Offset: 0x001351EA
		public void SwitchVisualization()
		{
			if (!this._leftSkeletonVisual || !this._rightSkeletonVisual)
			{
				return;
			}
			this.VisualMode = (this.VisualMode + 1) % (HandsManager.HandsVisualMode)3;
			this.SetToCurrentVisualMode();
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x00137020 File Offset: 0x00135220
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

		// Token: 0x060041B1 RID: 16817 RVA: 0x00137140 File Offset: 0x00135340
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

		// Token: 0x060041B2 RID: 16818 RVA: 0x0013718C File Offset: 0x0013538C
		public bool IsInitialized()
		{
			return this.LeftHandSkeleton && this.LeftHandSkeleton.IsInitialized && this.RightHandSkeleton && this.RightHandSkeleton.IsInitialized && this.LeftHandMesh && this.LeftHandMesh.IsInitialized && this.RightHandMesh && this.RightHandMesh.IsInitialized;
		}

		// Token: 0x040042B8 RID: 17080
		private const string SKELETON_VISUALIZER_NAME = "SkeletonRenderer";

		// Token: 0x040042B9 RID: 17081
		[SerializeField]
		private GameObject _leftHand;

		// Token: 0x040042BA RID: 17082
		[SerializeField]
		private GameObject _rightHand;

		// Token: 0x040042BB RID: 17083
		public HandsManager.HandsVisualMode VisualMode;

		// Token: 0x040042BC RID: 17084
		private OVRHand[] _hand = new OVRHand[2];

		// Token: 0x040042BD RID: 17085
		private OVRSkeleton[] _handSkeleton = new OVRSkeleton[2];

		// Token: 0x040042BE RID: 17086
		private OVRSkeletonRenderer[] _handSkeletonRenderer = new OVRSkeletonRenderer[2];

		// Token: 0x040042BF RID: 17087
		private OVRMesh[] _handMesh = new OVRMesh[2];

		// Token: 0x040042C0 RID: 17088
		private OVRMeshRenderer[] _handMeshRenderer = new OVRMeshRenderer[2];

		// Token: 0x040042C1 RID: 17089
		private SkinnedMeshRenderer _leftMeshRenderer;

		// Token: 0x040042C2 RID: 17090
		private SkinnedMeshRenderer _rightMeshRenderer;

		// Token: 0x040042C3 RID: 17091
		private GameObject _leftSkeletonVisual;

		// Token: 0x040042C4 RID: 17092
		private GameObject _rightSkeletonVisual;

		// Token: 0x040042C5 RID: 17093
		private float _currentHandAlpha = 1f;

		// Token: 0x040042C6 RID: 17094
		private int HandAlphaId = Shader.PropertyToID("_HandAlpha");

		// Token: 0x02000A4C RID: 2636
		public enum HandsVisualMode
		{
			// Token: 0x040042C9 RID: 17097
			Mesh,
			// Token: 0x040042CA RID: 17098
			Skeleton,
			// Token: 0x040042CB RID: 17099
			Both
		}
	}
}
