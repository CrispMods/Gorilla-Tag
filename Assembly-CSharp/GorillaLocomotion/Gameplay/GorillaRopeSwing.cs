using System;
using System.Collections.Generic;
using System.Linq;
using GorillaExtensions;
using GorillaLocomotion.Climbing;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B80 RID: 2944
	public class GorillaRopeSwing : MonoBehaviour, IBuilderPieceComponent
	{
		// Token: 0x060049B7 RID: 18871 RVA: 0x0005FEEC File Offset: 0x0005E0EC
		private void EdRecalculateId()
		{
			this.CalculateId(true);
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060049B8 RID: 18872 RVA: 0x0005FEF5 File Offset: 0x0005E0F5
		// (set) Token: 0x060049B9 RID: 18873 RVA: 0x0005FEFD File Offset: 0x0005E0FD
		public bool isIdle { get; private set; }

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060049BA RID: 18874 RVA: 0x0005FF06 File Offset: 0x0005E106
		// (set) Token: 0x060049BB RID: 18875 RVA: 0x0005FF0E File Offset: 0x0005E10E
		public bool isFullyIdle { get; private set; }

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060049BC RID: 18876 RVA: 0x0005FF17 File Offset: 0x0005E117
		public bool SupportsMovingAtRuntime
		{
			get
			{
				return this.supportMovingAtRuntime;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060049BD RID: 18877 RVA: 0x0005FF1F File Offset: 0x0005E11F
		public bool hasPlayers
		{
			get
			{
				return this.localPlayerOn || this.remotePlayers.Count > 0;
			}
		}

		// Token: 0x060049BE RID: 18878 RVA: 0x0019AB18 File Offset: 0x00198D18
		private void Awake()
		{
			base.transform.rotation = Quaternion.identity;
			this.scaleFactor = (base.transform.lossyScale.x + base.transform.lossyScale.y + base.transform.lossyScale.z) / 3f;
			this.SetIsIdle(true, false);
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x0005FF39 File Offset: 0x0005E139
		private void Start()
		{
			if (!this.useStaticId)
			{
				this.CalculateId(false);
			}
			RopeSwingManager.Register(this);
			this.started = true;
		}

		// Token: 0x060049C0 RID: 18880 RVA: 0x0005FF57 File Offset: 0x0005E157
		private void OnDestroy()
		{
			if (RopeSwingManager.instance != null)
			{
				RopeSwingManager.Unregister(this);
			}
		}

		// Token: 0x060049C1 RID: 18881 RVA: 0x0019AB7C File Offset: 0x00198D7C
		private void OnEnable()
		{
			base.transform.rotation = Quaternion.identity;
			this.scaleFactor = (base.transform.lossyScale.x + base.transform.lossyScale.y + base.transform.lossyScale.z) / 3f;
			this.SetIsIdle(true, true);
			VectorizedCustomRopeSimulation.Register(this);
			GorillaRopeSwingUpdateManager.RegisterRopeSwing(this);
		}

		// Token: 0x060049C2 RID: 18882 RVA: 0x0005FF6C File Offset: 0x0005E16C
		private void OnDisable()
		{
			if (!this.isIdle)
			{
				this.SetIsIdle(true, true);
			}
			VectorizedCustomRopeSimulation.Unregister(this);
			GorillaRopeSwingUpdateManager.UnregisterRopeSwing(this);
		}

		// Token: 0x060049C3 RID: 18883 RVA: 0x0019ABEC File Offset: 0x00198DEC
		internal void CalculateId(bool force = false)
		{
			Transform transform = base.transform;
			int staticHash = TransformUtils.GetScenePath(transform).GetStaticHash();
			int staticHash2 = base.GetType().Name.GetStaticHash();
			int num = StaticHash.Compute(staticHash, staticHash2);
			if (this.useStaticId)
			{
				if (string.IsNullOrEmpty(this.staticId) || force)
				{
					Vector3 position = transform.position;
					int i = StaticHash.Compute(position.x, position.y, position.z);
					int instanceID = transform.GetInstanceID();
					int num2 = StaticHash.Compute(num, i, instanceID);
					this.staticId = string.Format("#ID_{0:X8}", num2);
				}
				this.ropeId = this.staticId.GetStaticHash();
				return;
			}
			this.ropeId = (Application.isPlaying ? num : 0);
		}

		// Token: 0x060049C4 RID: 18884 RVA: 0x0019ACA8 File Offset: 0x00198EA8
		public void InvokeUpdate()
		{
			if (this.isIdle)
			{
				this.isFullyIdle = true;
			}
			if (!this.isIdle)
			{
				int num = -1;
				if (this.localPlayerOn)
				{
					num = this.localPlayerBoneIndex;
				}
				else if (this.remotePlayers.Count > 0)
				{
					num = this.remotePlayers.First<KeyValuePair<int, int>>().Value;
				}
				if (num >= 0 && VectorizedCustomRopeSimulation.instance.GetNodeVelocity(this, num).magnitude > 2f && !this.ropeCreakSFX.isPlaying && Mathf.RoundToInt(Time.time) % 5 == 0)
				{
					this.ropeCreakSFX.GTPlay();
				}
				if (this.localPlayerOn)
				{
					float num2 = MathUtils.Linear(this.velocityTracker.GetLatestVelocity(true).magnitude / this.scaleFactor, 0f, 10f, -0.07f, 0.5f);
					if (num2 > 0f)
					{
						GorillaTagger.Instance.DoVibration(this.localPlayerXRNode, num2, Time.deltaTime);
					}
				}
				Transform bone = this.GetBone(this.lastNodeCheckIndex);
				Vector3 nodeVelocity = VectorizedCustomRopeSimulation.instance.GetNodeVelocity(this, this.lastNodeCheckIndex);
				if (Physics.SphereCastNonAlloc(bone.position, 0.2f * this.scaleFactor, nodeVelocity.normalized, this.nodeHits, 0.4f * this.scaleFactor, this.wallLayerMask, QueryTriggerInteraction.Ignore) > 0)
				{
					this.SetVelocity(this.lastNodeCheckIndex, Vector3.zero, false, default(PhotonMessageInfoWrapped));
				}
				if (nodeVelocity.magnitude <= 0.35f)
				{
					this.potentialIdleTimer += Time.deltaTime;
				}
				else
				{
					this.potentialIdleTimer = 0f;
				}
				if (this.potentialIdleTimer >= 2f)
				{
					this.SetIsIdle(true, false);
					this.potentialIdleTimer = 0f;
				}
				this.lastNodeCheckIndex++;
				if (this.lastNodeCheckIndex > this.nodes.Length)
				{
					this.lastNodeCheckIndex = 2;
				}
			}
			if (this.hasMonkeBlockParent && this.supportMovingAtRuntime)
			{
				base.transform.rotation = Quaternion.Euler(0f, base.transform.parent.rotation.eulerAngles.y, 0f);
			}
		}

		// Token: 0x060049C5 RID: 18885 RVA: 0x0019AEDC File Offset: 0x001990DC
		private void SetIsIdle(bool idle, bool resetPos = false)
		{
			this.isIdle = idle;
			this.ropeCreakSFX.gameObject.SetActive(!idle);
			if (idle)
			{
				this.ToggleVelocityTracker(false, 0, default(Vector3));
				if (resetPos)
				{
					Vector3 vector = Vector3.zero;
					for (int i = 0; i < this.nodes.Length; i++)
					{
						this.nodes[i].transform.localRotation = Quaternion.identity;
						this.nodes[i].transform.localPosition = vector;
						vector += new Vector3(0f, -1f, 0f);
					}
					return;
				}
			}
			else
			{
				this.isFullyIdle = false;
			}
		}

		// Token: 0x060049C6 RID: 18886 RVA: 0x0005FF8A File Offset: 0x0005E18A
		public Transform GetBone(int index)
		{
			if (index >= this.nodes.Length)
			{
				return this.nodes.Last<Transform>();
			}
			return this.nodes[index];
		}

		// Token: 0x060049C7 RID: 18887 RVA: 0x0019AF84 File Offset: 0x00199184
		public int GetBoneIndex(Transform r)
		{
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] == r)
				{
					return i;
				}
			}
			return this.nodes.Length - 1;
		}

		// Token: 0x060049C8 RID: 18888 RVA: 0x0019AFC0 File Offset: 0x001991C0
		public void AttachLocalPlayer(XRNode xrNode, Transform grabbedBone, Vector3 offset, Vector3 velocity)
		{
			int boneIndex = this.GetBoneIndex(grabbedBone);
			this.localPlayerBoneIndex = boneIndex;
			velocity /= this.scaleFactor;
			velocity *= this.settings.inheritVelocityMultiplier;
			if (GorillaTagger.hasInstance && GorillaTagger.Instance.offlineVRRig)
			{
				GorillaTagger.Instance.offlineVRRig.grabbedRopeIndex = this.ropeId;
				GorillaTagger.Instance.offlineVRRig.grabbedRopeBoneIndex = boneIndex;
				GorillaTagger.Instance.offlineVRRig.grabbedRopeIsLeft = (xrNode == XRNode.LeftHand);
				GorillaTagger.Instance.offlineVRRig.grabbedRopeOffset = offset;
				GorillaTagger.Instance.offlineVRRig.grabbedRopeIsPhotonView = false;
			}
			this.RefreshAllBonesMass();
			List<Vector3> list = new List<Vector3>();
			if (this.remotePlayers.Count <= 0)
			{
				foreach (Transform transform in this.nodes)
				{
					list.Add(transform.position);
				}
			}
			velocity.y = 0f;
			if (Time.time - this.lastGrabTime > 1f && (this.remotePlayers.Count == 0 || velocity.magnitude > 2.5f))
			{
				RopeSwingManager.instance.SendSetVelocity_RPC(this.ropeId, boneIndex, velocity, true);
			}
			this.lastGrabTime = Time.time;
			this.ropeCreakSFX.transform.parent = this.GetBone(Math.Max(0, boneIndex - 3)).transform;
			this.ropeCreakSFX.transform.localPosition = Vector3.zero;
			this.localPlayerOn = true;
			this.localPlayerXRNode = xrNode;
			this.ToggleVelocityTracker(true, boneIndex, offset);
		}

		// Token: 0x060049C9 RID: 18889 RVA: 0x0005FFAB File Offset: 0x0005E1AB
		public void DetachLocalPlayer()
		{
			if (GorillaTagger.hasInstance && GorillaTagger.Instance.offlineVRRig)
			{
				GorillaTagger.Instance.offlineVRRig.grabbedRopeIndex = -1;
			}
			this.localPlayerOn = false;
			this.localPlayerBoneIndex = 0;
			this.RefreshAllBonesMass();
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x0019B15C File Offset: 0x0019935C
		private void ToggleVelocityTracker(bool enable, int boneIndex = 0, Vector3 offset = default(Vector3))
		{
			if (enable)
			{
				this.velocityTracker.transform.SetParent(this.GetBone(boneIndex));
				this.velocityTracker.transform.localPosition = offset;
				this.velocityTracker.ResetState();
			}
			this.velocityTracker.gameObject.SetActive(enable);
			if (enable)
			{
				this.velocityTracker.Tick();
			}
		}

		// Token: 0x060049CB RID: 18891 RVA: 0x0019B1C0 File Offset: 0x001993C0
		private void RefreshAllBonesMass()
		{
			int num = 0;
			foreach (KeyValuePair<int, int> keyValuePair in this.remotePlayers)
			{
				if (keyValuePair.Value > num)
				{
					num = keyValuePair.Value;
				}
			}
			if (this.localPlayerBoneIndex > num)
			{
				num = this.localPlayerBoneIndex;
			}
			VectorizedCustomRopeSimulation.instance.SetMassForPlayers(this, this.hasPlayers, num);
		}

		// Token: 0x060049CC RID: 18892 RVA: 0x0019B244 File Offset: 0x00199444
		public bool AttachRemotePlayer(int playerId, int boneIndex, Transform offsetTransform, Vector3 offset)
		{
			Transform bone = this.GetBone(boneIndex);
			if (bone == null)
			{
				return false;
			}
			offsetTransform.SetParent(bone.transform);
			offsetTransform.localPosition = offset;
			offsetTransform.localRotation = Quaternion.identity;
			if (this.remotePlayers.ContainsKey(playerId))
			{
				Debug.LogError("already on the list!");
				return false;
			}
			this.remotePlayers.Add(playerId, boneIndex);
			this.RefreshAllBonesMass();
			return true;
		}

		// Token: 0x060049CD RID: 18893 RVA: 0x0005FFE9 File Offset: 0x0005E1E9
		public void DetachRemotePlayer(int playerId)
		{
			this.remotePlayers.Remove(playerId);
			this.RefreshAllBonesMass();
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x0019B2B4 File Offset: 0x001994B4
		public void SetVelocity(int boneIndex, Vector3 velocity, bool wholeRope, PhotonMessageInfoWrapped info)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			float num = 10000f;
			if (!velocity.IsValid(num))
			{
				return;
			}
			velocity.x = Mathf.Clamp(velocity.x, -100f, 100f);
			velocity.y = Mathf.Clamp(velocity.y, -100f, 100f);
			velocity.z = Mathf.Clamp(velocity.z, -100f, 100f);
			boneIndex = Mathf.Clamp(boneIndex, 0, this.nodes.Length);
			Transform bone = this.GetBone(boneIndex);
			if (!bone)
			{
				return;
			}
			if (info.Sender != null && !info.Sender.IsLocal)
			{
				VRRig vrrig = GorillaGameManager.StaticFindRigForPlayer(info.Sender);
				if (!vrrig || Vector3.Distance(bone.position, vrrig.transform.position) > 5f)
				{
					return;
				}
			}
			this.SetIsIdle(false, false);
			if (bone)
			{
				VectorizedCustomRopeSimulation.instance.SetVelocity(this, velocity, wholeRope, boneIndex);
			}
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x0019B3BC File Offset: 0x001995BC
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.monkeBlockParent = base.GetComponentInParent<BuilderPiece>();
			this.hasMonkeBlockParent = (this.monkeBlockParent != null);
			int num = StaticHash.Compute(pieceType, pieceId);
			this.staticId = string.Format("#ID_{0:X8}", num);
			this.ropeId = this.staticId.GetStaticHash();
			GorillaRopeSwing gorillaRopeSwing;
			if (this.started && !RopeSwingManager.instance.TryGetRope(this.ropeId, out gorillaRopeSwing))
			{
				RopeSwingManager.Register(this);
			}
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x0005FFFE File Offset: 0x0005E1FE
		public void OnPieceDestroy()
		{
			RopeSwingManager.Unregister(this);
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x0019B438 File Offset: 0x00199638
		public void OnPiecePlacementDeserialized()
		{
			VectorizedCustomRopeSimulation.Unregister(this);
			base.transform.rotation = Quaternion.identity;
			this.scaleFactor = (base.transform.lossyScale.x + base.transform.lossyScale.y + base.transform.lossyScale.z) / 3f;
			this.SetIsIdle(true, true);
			VectorizedCustomRopeSimulation.Register(this);
			if (this.monkeBlockParent != null)
			{
				this.supportMovingAtRuntime = this.IsAttachedToMovingPiece();
			}
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x00060006 File Offset: 0x0005E206
		public void OnPieceActivate()
		{
			if (this.monkeBlockParent != null)
			{
				this.supportMovingAtRuntime = this.IsAttachedToMovingPiece();
			}
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x0019B4C4 File Offset: 0x001996C4
		private bool IsAttachedToMovingPiece()
		{
			return this.monkeBlockParent.attachIndex >= 0 && this.monkeBlockParent.attachIndex < this.monkeBlockParent.gridPlanes.Count && this.monkeBlockParent.gridPlanes[this.monkeBlockParent.attachIndex].GetMovingParentGrid() != null;
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x00060022 File Offset: 0x0005E222
		public void OnPieceDeactivate()
		{
			this.supportMovingAtRuntime = false;
		}

		// Token: 0x04004C05 RID: 19461
		public int ropeId;

		// Token: 0x04004C06 RID: 19462
		public string staticId;

		// Token: 0x04004C07 RID: 19463
		public bool useStaticId;

		// Token: 0x04004C08 RID: 19464
		public const float ropeBitGenOffset = 1f;

		// Token: 0x04004C09 RID: 19465
		[SerializeField]
		private GameObject prefabRopeBit;

		// Token: 0x04004C0A RID: 19466
		[SerializeField]
		private bool supportMovingAtRuntime;

		// Token: 0x04004C0B RID: 19467
		public Transform[] nodes = Array.Empty<Transform>();

		// Token: 0x04004C0C RID: 19468
		private Dictionary<int, int> remotePlayers = new Dictionary<int, int>();

		// Token: 0x04004C0D RID: 19469
		[NonSerialized]
		public float lastGrabTime;

		// Token: 0x04004C0E RID: 19470
		[SerializeField]
		private AudioSource ropeCreakSFX;

		// Token: 0x04004C0F RID: 19471
		public GorillaVelocityTracker velocityTracker;

		// Token: 0x04004C10 RID: 19472
		private bool localPlayerOn;

		// Token: 0x04004C11 RID: 19473
		private int localPlayerBoneIndex;

		// Token: 0x04004C12 RID: 19474
		private XRNode localPlayerXRNode;

		// Token: 0x04004C13 RID: 19475
		private const float MAX_VELOCITY_FOR_IDLE = 0.5f;

		// Token: 0x04004C14 RID: 19476
		private const float TIME_FOR_IDLE = 2f;

		// Token: 0x04004C17 RID: 19479
		private float potentialIdleTimer;

		// Token: 0x04004C18 RID: 19480
		[SerializeField]
		private int ropeLength = 8;

		// Token: 0x04004C19 RID: 19481
		[SerializeField]
		private GorillaRopeSwingSettings settings;

		// Token: 0x04004C1A RID: 19482
		private bool hasMonkeBlockParent;

		// Token: 0x04004C1B RID: 19483
		private BuilderPiece monkeBlockParent;

		// Token: 0x04004C1C RID: 19484
		[NonSerialized]
		public int ropeDataStartIndex;

		// Token: 0x04004C1D RID: 19485
		[NonSerialized]
		public int ropeDataIndexOffset;

		// Token: 0x04004C1E RID: 19486
		[SerializeField]
		private LayerMask wallLayerMask;

		// Token: 0x04004C1F RID: 19487
		private RaycastHit[] nodeHits = new RaycastHit[1];

		// Token: 0x04004C20 RID: 19488
		private float scaleFactor = 1f;

		// Token: 0x04004C21 RID: 19489
		private bool started;

		// Token: 0x04004C22 RID: 19490
		private int lastNodeCheckIndex = 2;
	}
}
