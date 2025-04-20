using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B88 RID: 2952
	public class OldGorillaRopeSwing : MonoBehaviourPun
	{
		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060049FA RID: 18938 RVA: 0x00060256 File Offset: 0x0005E456
		// (set) Token: 0x060049FB RID: 18939 RVA: 0x0006025E File Offset: 0x0005E45E
		public bool isIdle { get; private set; }

		// Token: 0x060049FC RID: 18940 RVA: 0x00060267 File Offset: 0x0005E467
		private void Awake()
		{
			this.SetIsIdle(true);
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x00060270 File Offset: 0x0005E470
		private void OnDisable()
		{
			if (!this.isIdle)
			{
				this.SetIsIdle(true);
			}
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x0019BCC4 File Offset: 0x00199EC4
		private void Update()
		{
			if (this.localPlayerOn && this.localGrabbedRigid)
			{
				float magnitude = this.localGrabbedRigid.velocity.magnitude;
				if (magnitude > 2.5f && !this.ropeCreakSFX.isPlaying && Mathf.RoundToInt(Time.time) % 5 == 0)
				{
					this.ropeCreakSFX.GTPlay();
				}
				float num = MathUtils.Linear(magnitude, 0f, 10f, -0.07f, 0.5f);
				if (num > 0f)
				{
					GorillaTagger.Instance.DoVibration(this.localPlayerXRNode, num, Time.deltaTime);
				}
			}
			if (!this.isIdle)
			{
				if (!this.localPlayerOn && this.remotePlayers.Count == 0)
				{
					foreach (Rigidbody rigidbody in this.bones)
					{
						float magnitude2 = rigidbody.velocity.magnitude;
						float num2 = Time.deltaTime * this.settings.frictionWhenNotHeld;
						if (num2 < magnitude2 - 0.1f)
						{
							rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, Vector3.zero, num2);
						}
					}
				}
				bool flag = false;
				for (int j = 0; j < this.bones.Length; j++)
				{
					if (this.bones[j].velocity.magnitude > 0.1f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.potentialIdleTimer += Time.deltaTime;
				}
				else
				{
					this.potentialIdleTimer = 0f;
				}
				if (this.potentialIdleTimer >= 2f)
				{
					this.SetIsIdle(true);
					this.potentialIdleTimer = 0f;
				}
			}
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x0019BE70 File Offset: 0x0019A070
		private void SetIsIdle(bool idle)
		{
			this.isIdle = idle;
			this.ToggleIsKinematic(idle);
			if (idle)
			{
				for (int i = 0; i < this.bones.Length; i++)
				{
					this.bones[i].velocity = Vector3.zero;
					this.bones[i].angularVelocity = Vector3.zero;
					this.bones[i].transform.localRotation = Quaternion.identity;
				}
			}
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x0019BEDC File Offset: 0x0019A0DC
		private void ToggleIsKinematic(bool kinematic)
		{
			for (int i = 0; i < this.bones.Length; i++)
			{
				this.bones[i].isKinematic = kinematic;
				if (kinematic)
				{
					this.bones[i].interpolation = RigidbodyInterpolation.None;
				}
				else
				{
					this.bones[i].interpolation = RigidbodyInterpolation.Interpolate;
				}
			}
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x00060281 File Offset: 0x0005E481
		public Rigidbody GetBone(int index)
		{
			if (index >= this.bones.Length)
			{
				return this.bones.Last<Rigidbody>();
			}
			return this.bones[index];
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x0019BF2C File Offset: 0x0019A12C
		public int GetBoneIndex(Rigidbody r)
		{
			for (int i = 0; i < this.bones.Length; i++)
			{
				if (this.bones[i] == r)
				{
					return i;
				}
			}
			return this.bones.Length - 1;
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x0019BF68 File Offset: 0x0019A168
		public void AttachLocalPlayer(XRNode xrNode, Rigidbody rigid, Vector3 offset, Vector3 velocity)
		{
			int boneIndex = this.GetBoneIndex(rigid);
			velocity *= this.settings.inheritVelocityMultiplier;
			if (GorillaTagger.hasInstance && GorillaTagger.Instance.offlineVRRig)
			{
				GorillaTagger.Instance.offlineVRRig.grabbedRopeIndex = base.photonView.ViewID;
				GorillaTagger.Instance.offlineVRRig.grabbedRopeBoneIndex = boneIndex;
				GorillaTagger.Instance.offlineVRRig.grabbedRopeIsLeft = (xrNode == XRNode.LeftHand);
				GorillaTagger.Instance.offlineVRRig.grabbedRopeOffset = offset;
			}
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			if (this.remotePlayers.Count <= 0)
			{
				foreach (Rigidbody rigidbody in this.bones)
				{
					list.Add(rigidbody.transform.localEulerAngles);
					list2.Add(rigidbody.velocity);
				}
			}
			if (Time.time - this.lastGrabTime > 1f && (this.remotePlayers.Count == 0 || velocity.magnitude > 2f))
			{
				this.SetVelocity_RPC(boneIndex, velocity, true, list.ToArray(), list2.ToArray());
			}
			this.lastGrabTime = Time.time;
			this.ropeCreakSFX.transform.parent = this.GetBone(Math.Max(0, boneIndex - 2)).transform;
			this.ropeCreakSFX.transform.localPosition = Vector3.zero;
			this.localPlayerOn = true;
			this.localPlayerXRNode = xrNode;
			this.localGrabbedRigid = rigid;
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x000602A2 File Offset: 0x0005E4A2
		public void DetachLocalPlayer()
		{
			if (GorillaTagger.hasInstance && GorillaTagger.Instance.offlineVRRig)
			{
				GorillaTagger.Instance.offlineVRRig.grabbedRopeIndex = -1;
			}
			this.localPlayerOn = false;
			this.localGrabbedRigid = null;
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x0019C0EC File Offset: 0x0019A2EC
		public bool AttachRemotePlayer(int playerId, int boneIndex, Transform offsetTransform, Vector3 offset)
		{
			Rigidbody bone = this.GetBone(boneIndex);
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
			return true;
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x000602DA File Offset: 0x0005E4DA
		public void DetachRemotePlayer(int playerId)
		{
			this.remotePlayers.Remove(playerId);
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x0019C154 File Offset: 0x0019A354
		public void SetVelocity_RPC(int boneIndex, Vector3 velocity, bool wholeRope = true, Vector3[] ropeRotations = null, Vector3[] ropeVelocities = null)
		{
			if (NetworkSystem.Instance.InRoom)
			{
				base.photonView.RPC("SetVelocity", RpcTarget.All, new object[]
				{
					boneIndex,
					velocity,
					wholeRope,
					ropeRotations,
					ropeVelocities
				});
				return;
			}
			this.SetVelocity(boneIndex, velocity, wholeRope, ropeRotations, ropeVelocities);
		}

		// Token: 0x06004A08 RID: 18952 RVA: 0x0019C1B8 File Offset: 0x0019A3B8
		[PunRPC]
		public void SetVelocity(int boneIndex, Vector3 velocity, bool wholeRope = true, Vector3[] ropeRotations = null, Vector3[] ropeVelocities = null)
		{
			this.SetIsIdle(false);
			if (ropeRotations != null && ropeVelocities != null && ropeRotations.Length != 0)
			{
				this.ToggleIsKinematic(true);
				for (int i = 0; i < ropeRotations.Length; i++)
				{
					if (i != 0)
					{
						this.bones[i].transform.localRotation = Quaternion.Euler(ropeRotations[i]);
						this.bones[i].velocity = ropeVelocities[i];
					}
				}
				this.ToggleIsKinematic(false);
			}
			Rigidbody bone = this.GetBone(boneIndex);
			if (bone)
			{
				if (wholeRope)
				{
					int num = 0;
					float maxLength = Mathf.Min(velocity.magnitude, 15f);
					foreach (Rigidbody rigidbody in this.bones)
					{
						Vector3 vector = velocity / (float)boneIndex * (float)num;
						vector = Vector3.ClampMagnitude(vector, maxLength);
						rigidbody.velocity = vector;
						num++;
					}
					return;
				}
				bone.velocity = velocity;
			}
		}

		// Token: 0x04004C4E RID: 19534
		public const float kPlayerMass = 0.8f;

		// Token: 0x04004C4F RID: 19535
		public const float ropeBitGenOffset = 1f;

		// Token: 0x04004C50 RID: 19536
		public const float MAX_ROPE_SPEED = 15f;

		// Token: 0x04004C51 RID: 19537
		[SerializeField]
		private GameObject prefabRopeBit;

		// Token: 0x04004C52 RID: 19538
		public Rigidbody[] bones = Array.Empty<Rigidbody>();

		// Token: 0x04004C53 RID: 19539
		private Dictionary<int, int> remotePlayers = new Dictionary<int, int>();

		// Token: 0x04004C54 RID: 19540
		[NonSerialized]
		public float lastGrabTime;

		// Token: 0x04004C55 RID: 19541
		[SerializeField]
		private AudioSource ropeCreakSFX;

		// Token: 0x04004C56 RID: 19542
		private bool localPlayerOn;

		// Token: 0x04004C57 RID: 19543
		private XRNode localPlayerXRNode;

		// Token: 0x04004C58 RID: 19544
		private Rigidbody localGrabbedRigid;

		// Token: 0x04004C59 RID: 19545
		private const float MAX_VELOCITY_FOR_IDLE = 0.1f;

		// Token: 0x04004C5A RID: 19546
		private const float TIME_FOR_IDLE = 2f;

		// Token: 0x04004C5C RID: 19548
		private float potentialIdleTimer;

		// Token: 0x04004C5D RID: 19549
		[Header("Config")]
		[SerializeField]
		private int ropeLength = 8;

		// Token: 0x04004C5E RID: 19550
		[SerializeField]
		private GorillaRopeSwingSettings settings;
	}
}
