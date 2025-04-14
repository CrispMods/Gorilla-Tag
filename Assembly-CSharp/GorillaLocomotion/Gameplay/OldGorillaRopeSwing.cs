using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B5B RID: 2907
	public class OldGorillaRopeSwing : MonoBehaviourPun
	{
		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x060048AF RID: 18607 RVA: 0x00160658 File Offset: 0x0015E858
		// (set) Token: 0x060048B0 RID: 18608 RVA: 0x00160660 File Offset: 0x0015E860
		public bool isIdle { get; private set; }

		// Token: 0x060048B1 RID: 18609 RVA: 0x00160669 File Offset: 0x0015E869
		private void Awake()
		{
			this.SetIsIdle(true);
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x00160672 File Offset: 0x0015E872
		private void OnDisable()
		{
			if (!this.isIdle)
			{
				this.SetIsIdle(true);
			}
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x00160684 File Offset: 0x0015E884
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

		// Token: 0x060048B4 RID: 18612 RVA: 0x00160830 File Offset: 0x0015EA30
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

		// Token: 0x060048B5 RID: 18613 RVA: 0x0016089C File Offset: 0x0015EA9C
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

		// Token: 0x060048B6 RID: 18614 RVA: 0x001608EB File Offset: 0x0015EAEB
		public Rigidbody GetBone(int index)
		{
			if (index >= this.bones.Length)
			{
				return this.bones.Last<Rigidbody>();
			}
			return this.bones[index];
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x0016090C File Offset: 0x0015EB0C
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

		// Token: 0x060048B8 RID: 18616 RVA: 0x00160948 File Offset: 0x0015EB48
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

		// Token: 0x060048B9 RID: 18617 RVA: 0x00160ACB File Offset: 0x0015ECCB
		public void DetachLocalPlayer()
		{
			if (GorillaTagger.hasInstance && GorillaTagger.Instance.offlineVRRig)
			{
				GorillaTagger.Instance.offlineVRRig.grabbedRopeIndex = -1;
			}
			this.localPlayerOn = false;
			this.localGrabbedRigid = null;
		}

		// Token: 0x060048BA RID: 18618 RVA: 0x00160B04 File Offset: 0x0015ED04
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

		// Token: 0x060048BB RID: 18619 RVA: 0x00160B6B File Offset: 0x0015ED6B
		public void DetachRemotePlayer(int playerId)
		{
			this.remotePlayers.Remove(playerId);
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x00160B7C File Offset: 0x0015ED7C
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

		// Token: 0x060048BD RID: 18621 RVA: 0x00160BE0 File Offset: 0x0015EDE0
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

		// Token: 0x04004B58 RID: 19288
		public const float kPlayerMass = 0.8f;

		// Token: 0x04004B59 RID: 19289
		public const float ropeBitGenOffset = 1f;

		// Token: 0x04004B5A RID: 19290
		public const float MAX_ROPE_SPEED = 15f;

		// Token: 0x04004B5B RID: 19291
		[SerializeField]
		private GameObject prefabRopeBit;

		// Token: 0x04004B5C RID: 19292
		public Rigidbody[] bones = Array.Empty<Rigidbody>();

		// Token: 0x04004B5D RID: 19293
		private Dictionary<int, int> remotePlayers = new Dictionary<int, int>();

		// Token: 0x04004B5E RID: 19294
		[NonSerialized]
		public float lastGrabTime;

		// Token: 0x04004B5F RID: 19295
		[SerializeField]
		private AudioSource ropeCreakSFX;

		// Token: 0x04004B60 RID: 19296
		private bool localPlayerOn;

		// Token: 0x04004B61 RID: 19297
		private XRNode localPlayerXRNode;

		// Token: 0x04004B62 RID: 19298
		private Rigidbody localGrabbedRigid;

		// Token: 0x04004B63 RID: 19299
		private const float MAX_VELOCITY_FOR_IDLE = 0.1f;

		// Token: 0x04004B64 RID: 19300
		private const float TIME_FOR_IDLE = 2f;

		// Token: 0x04004B66 RID: 19302
		private float potentialIdleTimer;

		// Token: 0x04004B67 RID: 19303
		[Header("Config")]
		[SerializeField]
		private int ropeLength = 8;

		// Token: 0x04004B68 RID: 19304
		[SerializeField]
		private GorillaRopeSwingSettings settings;
	}
}
