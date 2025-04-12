using System;
using System.Collections.Generic;
using System.Linq;
using GorillaExtensions;
using GorillaLocomotion.Climbing;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B56 RID: 2902
	public class GorillaRopeSwing : MonoBehaviour, IBuilderPieceComponent
	{
		// Token: 0x06004878 RID: 18552 RVA: 0x0005E4B4 File Offset: 0x0005C6B4
		private void EdRecalculateId()
		{
			this.CalculateId(true);
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06004879 RID: 18553 RVA: 0x0005E4BD File Offset: 0x0005C6BD
		// (set) Token: 0x0600487A RID: 18554 RVA: 0x0005E4C5 File Offset: 0x0005C6C5
		public bool isIdle { get; private set; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x0600487B RID: 18555 RVA: 0x0005E4CE File Offset: 0x0005C6CE
		// (set) Token: 0x0600487C RID: 18556 RVA: 0x0005E4D6 File Offset: 0x0005C6D6
		public bool isFullyIdle { get; private set; }

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x0600487D RID: 18557 RVA: 0x0005E4DF File Offset: 0x0005C6DF
		public bool SupportsMovingAtRuntime
		{
			get
			{
				return this.supportMovingAtRuntime;
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x0600487E RID: 18558 RVA: 0x0005E4E7 File Offset: 0x0005C6E7
		public bool hasPlayers
		{
			get
			{
				return this.localPlayerOn || this.remotePlayers.Count > 0;
			}
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x00193B00 File Offset: 0x00191D00
		private void Awake()
		{
			base.transform.rotation = Quaternion.identity;
			this.scaleFactor = (base.transform.lossyScale.x + base.transform.lossyScale.y + base.transform.lossyScale.z) / 3f;
			this.SetIsIdle(true, false);
		}

		// Token: 0x06004880 RID: 18560 RVA: 0x0005E501 File Offset: 0x0005C701
		private void Start()
		{
			if (!this.useStaticId)
			{
				this.CalculateId(false);
			}
			RopeSwingManager.Register(this);
			this.started = true;
		}

		// Token: 0x06004881 RID: 18561 RVA: 0x0005E51F File Offset: 0x0005C71F
		private void OnDestroy()
		{
			if (RopeSwingManager.instance != null)
			{
				RopeSwingManager.Unregister(this);
			}
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x00193B64 File Offset: 0x00191D64
		private void OnEnable()
		{
			base.transform.rotation = Quaternion.identity;
			this.scaleFactor = (base.transform.lossyScale.x + base.transform.lossyScale.y + base.transform.lossyScale.z) / 3f;
			this.SetIsIdle(true, true);
			VectorizedCustomRopeSimulation.Register(this);
			GorillaRopeSwingUpdateManager.RegisterRopeSwing(this);
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x0005E534 File Offset: 0x0005C734
		private void OnDisable()
		{
			if (!this.isIdle)
			{
				this.SetIsIdle(true, true);
			}
			VectorizedCustomRopeSimulation.Unregister(this);
			GorillaRopeSwingUpdateManager.UnregisterRopeSwing(this);
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x00193BD4 File Offset: 0x00191DD4
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

		// Token: 0x06004885 RID: 18565 RVA: 0x00193C90 File Offset: 0x00191E90
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

		// Token: 0x06004886 RID: 18566 RVA: 0x00193EC4 File Offset: 0x001920C4
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

		// Token: 0x06004887 RID: 18567 RVA: 0x0005E552 File Offset: 0x0005C752
		public Transform GetBone(int index)
		{
			if (index >= this.nodes.Length)
			{
				return this.nodes.Last<Transform>();
			}
			return this.nodes[index];
		}

		// Token: 0x06004888 RID: 18568 RVA: 0x00193F6C File Offset: 0x0019216C
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

		// Token: 0x06004889 RID: 18569 RVA: 0x00193FA8 File Offset: 0x001921A8
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

		// Token: 0x0600488A RID: 18570 RVA: 0x0005E573 File Offset: 0x0005C773
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

		// Token: 0x0600488B RID: 18571 RVA: 0x00194144 File Offset: 0x00192344
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

		// Token: 0x0600488C RID: 18572 RVA: 0x001941A8 File Offset: 0x001923A8
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

		// Token: 0x0600488D RID: 18573 RVA: 0x0019422C File Offset: 0x0019242C
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

		// Token: 0x0600488E RID: 18574 RVA: 0x0005E5B1 File Offset: 0x0005C7B1
		public void DetachRemotePlayer(int playerId)
		{
			this.remotePlayers.Remove(playerId);
			this.RefreshAllBonesMass();
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x0019429C File Offset: 0x0019249C
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

		// Token: 0x06004890 RID: 18576 RVA: 0x001943A4 File Offset: 0x001925A4
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

		// Token: 0x06004891 RID: 18577 RVA: 0x0005E5C6 File Offset: 0x0005C7C6
		public void OnPieceDestroy()
		{
			RopeSwingManager.Unregister(this);
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x00194420 File Offset: 0x00192620
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

		// Token: 0x06004893 RID: 18579 RVA: 0x0005E5CE File Offset: 0x0005C7CE
		public void OnPieceActivate()
		{
			if (this.monkeBlockParent != null)
			{
				this.supportMovingAtRuntime = this.IsAttachedToMovingPiece();
			}
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x001944AC File Offset: 0x001926AC
		private bool IsAttachedToMovingPiece()
		{
			return this.monkeBlockParent.attachIndex >= 0 && this.monkeBlockParent.attachIndex < this.monkeBlockParent.gridPlanes.Count && this.monkeBlockParent.gridPlanes[this.monkeBlockParent.attachIndex].GetMovingParentGrid() != null;
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x0005E5EA File Offset: 0x0005C7EA
		public void OnPieceDeactivate()
		{
			this.supportMovingAtRuntime = false;
		}

		// Token: 0x04004B21 RID: 19233
		public int ropeId;

		// Token: 0x04004B22 RID: 19234
		public string staticId;

		// Token: 0x04004B23 RID: 19235
		public bool useStaticId;

		// Token: 0x04004B24 RID: 19236
		public const float ropeBitGenOffset = 1f;

		// Token: 0x04004B25 RID: 19237
		[SerializeField]
		private GameObject prefabRopeBit;

		// Token: 0x04004B26 RID: 19238
		[SerializeField]
		private bool supportMovingAtRuntime;

		// Token: 0x04004B27 RID: 19239
		public Transform[] nodes = Array.Empty<Transform>();

		// Token: 0x04004B28 RID: 19240
		private Dictionary<int, int> remotePlayers = new Dictionary<int, int>();

		// Token: 0x04004B29 RID: 19241
		[NonSerialized]
		public float lastGrabTime;

		// Token: 0x04004B2A RID: 19242
		[SerializeField]
		private AudioSource ropeCreakSFX;

		// Token: 0x04004B2B RID: 19243
		public GorillaVelocityTracker velocityTracker;

		// Token: 0x04004B2C RID: 19244
		private bool localPlayerOn;

		// Token: 0x04004B2D RID: 19245
		private int localPlayerBoneIndex;

		// Token: 0x04004B2E RID: 19246
		private XRNode localPlayerXRNode;

		// Token: 0x04004B2F RID: 19247
		private const float MAX_VELOCITY_FOR_IDLE = 0.5f;

		// Token: 0x04004B30 RID: 19248
		private const float TIME_FOR_IDLE = 2f;

		// Token: 0x04004B33 RID: 19251
		private float potentialIdleTimer;

		// Token: 0x04004B34 RID: 19252
		[SerializeField]
		private int ropeLength = 8;

		// Token: 0x04004B35 RID: 19253
		[SerializeField]
		private GorillaRopeSwingSettings settings;

		// Token: 0x04004B36 RID: 19254
		private bool hasMonkeBlockParent;

		// Token: 0x04004B37 RID: 19255
		private BuilderPiece monkeBlockParent;

		// Token: 0x04004B38 RID: 19256
		[NonSerialized]
		public int ropeDataStartIndex;

		// Token: 0x04004B39 RID: 19257
		[NonSerialized]
		public int ropeDataIndexOffset;

		// Token: 0x04004B3A RID: 19258
		[SerializeField]
		private LayerMask wallLayerMask;

		// Token: 0x04004B3B RID: 19259
		private RaycastHit[] nodeHits = new RaycastHit[1];

		// Token: 0x04004B3C RID: 19260
		private float scaleFactor = 1f;

		// Token: 0x04004B3D RID: 19261
		private bool started;

		// Token: 0x04004B3E RID: 19262
		private int lastNodeCheckIndex = 2;
	}
}
