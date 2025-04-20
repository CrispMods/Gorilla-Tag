using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C70 RID: 3184
	public class FartBagThrowable : MonoBehaviour, IProjectile
	{
		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06004F9A RID: 20378 RVA: 0x00063F83 File Offset: 0x00062183
		// (set) Token: 0x06004F9B RID: 20379 RVA: 0x00063F8B File Offset: 0x0006218B
		public TransferrableObject ParentTransferable { get; set; }

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06004F9C RID: 20380 RVA: 0x001B91DC File Offset: 0x001B73DC
		// (remove) Token: 0x06004F9D RID: 20381 RVA: 0x001B9214 File Offset: 0x001B7414
		public event Action<IProjectile> OnDeflated;

		// Token: 0x06004F9E RID: 20382 RVA: 0x001B924C File Offset: 0x001B744C
		private void OnEnable()
		{
			this.placedOnFloor = false;
			this.deflated = false;
			this.handContactPoint = Vector3.negativeInfinity;
			this.handNormalVector = Vector3.zero;
			this.timeCreated = float.PositiveInfinity;
			this.placedOnFloorTime = float.PositiveInfinity;
			if (this.updateBlendShapeCosmetic)
			{
				this.updateBlendShapeCosmetic.ResetBlend();
			}
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x00063F94 File Offset: 0x00062194
		private void Update()
		{
			if (Time.time - this.timeCreated > this.forceDestroyAfterSec)
			{
				this.DeflateLocal();
			}
		}

		// Token: 0x06004FA0 RID: 20384 RVA: 0x001B92AC File Offset: 0x001B74AC
		public void Launch(Vector3 startPosition, Quaternion startRotation, Vector3 velocity, float scale)
		{
			base.transform.position = startPosition;
			base.transform.rotation = startRotation;
			base.transform.localScale = Vector3.one * scale;
			this.rigidbody.velocity = velocity;
			this.timeCreated = Time.time;
			this.InitialPhotonEvent();
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x001B9308 File Offset: 0x001B7508
		private void InitialPhotonEvent()
		{
			this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
			if (this.ParentTransferable)
			{
				NetPlayer netPlayer = (this.ParentTransferable.myOnlineRig != null) ? this.ParentTransferable.myOnlineRig.creator : ((this.ParentTransferable.myRig != null) ? (this.ParentTransferable.myRig.creator ?? NetworkSystem.Instance.LocalPlayer) : null);
				if (this._events != null && netPlayer != null)
				{
					this._events.Init(netPlayer);
				}
			}
			if (this._events != null)
			{
				this._events.Activate += this.DeflateEvent;
			}
		}

		// Token: 0x06004FA2 RID: 20386 RVA: 0x001B93DC File Offset: 0x001B75DC
		private void OnTriggerEnter(Collider other)
		{
			if ((this.handLayerMask.value & 1 << other.gameObject.layer) != 0)
			{
				if (!this.placedOnFloor)
				{
					return;
				}
				this.handContactPoint = other.ClosestPoint(base.transform.position);
				this.handNormalVector = (this.handContactPoint - base.transform.position).normalized;
				if (Time.time - this.placedOnFloorTime > 0.3f)
				{
					this.Deflate();
				}
			}
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x001B9464 File Offset: 0x001B7664
		private void OnCollisionEnter(Collision other)
		{
			if ((this.floorLayerMask.value & 1 << other.gameObject.layer) != 0)
			{
				this.placedOnFloor = true;
				this.placedOnFloorTime = Time.time;
				Vector3 normal = other.contacts[0].normal;
				base.transform.position = other.contacts[0].point + normal * this.placementOffset;
				Quaternion rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(base.transform.forward, normal).normalized, normal);
				base.transform.rotation = rotation;
			}
		}

		// Token: 0x06004FA4 RID: 20388 RVA: 0x001B950C File Offset: 0x001B770C
		private void Deflate()
		{
			if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
			{
				this._events.Activate.RaiseOthers(new object[]
				{
					this.handContactPoint,
					this.handNormalVector
				});
			}
			this.DeflateLocal();
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x001B957C File Offset: 0x001B777C
		private void DeflateEvent(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
		{
			if (sender != target)
			{
				return;
			}
			if (args.Length != 2)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "DeflateEvent");
			if (this.callLimiter.CheckCallTime(Time.time))
			{
				object obj = args[0];
				if (obj is Vector3)
				{
					Vector3 position = (Vector3)obj;
					obj = args[1];
					if (obj is Vector3)
					{
						Vector3 vector = (Vector3)obj;
						float num = 10000f;
						if (!vector.IsValid(num))
						{
							return;
						}
						num = 10000f;
						if (!position.IsValid(num) || !this.ParentTransferable.targetRig.IsPositionInRange(position, 4f))
						{
							return;
						}
						this.handNormalVector = vector;
						this.handContactPoint = position;
						this.DeflateLocal();
						return;
					}
				}
			}
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x001B962C File Offset: 0x001B782C
		private void DeflateLocal()
		{
			if (this.deflated)
			{
				return;
			}
			GameObject gameObject = ObjectPools.instance.Instantiate(this.deflationEffect, this.handContactPoint);
			gameObject.transform.up = this.handNormalVector;
			gameObject.transform.position = base.transform.position;
			SoundBankPlayer componentInChildren = gameObject.GetComponentInChildren<SoundBankPlayer>();
			if (componentInChildren.soundBank)
			{
				componentInChildren.Play();
			}
			this.placedOnFloor = false;
			this.timeCreated = float.PositiveInfinity;
			if (this.updateBlendShapeCosmetic)
			{
				this.updateBlendShapeCosmetic.FullyBlend();
			}
			this.deflated = true;
			base.Invoke("DisableObject", this.destroyWhenDeflateDelay);
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x00063FB0 File Offset: 0x000621B0
		private void DisableObject()
		{
			Action<IProjectile> onDeflated = this.OnDeflated;
			if (onDeflated != null)
			{
				onDeflated(this);
			}
			this.deflated = false;
		}

		// Token: 0x06004FA8 RID: 20392 RVA: 0x001B96DC File Offset: 0x001B78DC
		private void OnDestroy()
		{
			if (this._events != null)
			{
				this._events.Activate -= this.DeflateEvent;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x040052C7 RID: 21191
		[SerializeField]
		private GameObject deflationEffect;

		// Token: 0x040052C8 RID: 21192
		[SerializeField]
		private float destroyWhenDeflateDelay = 3f;

		// Token: 0x040052C9 RID: 21193
		[SerializeField]
		private float forceDestroyAfterSec = 10f;

		// Token: 0x040052CA RID: 21194
		[SerializeField]
		private float placementOffset = 0.2f;

		// Token: 0x040052CB RID: 21195
		[SerializeField]
		private UpdateBlendShapeCosmetic updateBlendShapeCosmetic;

		// Token: 0x040052CC RID: 21196
		[SerializeField]
		private LayerMask floorLayerMask;

		// Token: 0x040052CD RID: 21197
		[SerializeField]
		private LayerMask handLayerMask;

		// Token: 0x040052CE RID: 21198
		[SerializeField]
		private Rigidbody rigidbody;

		// Token: 0x040052CF RID: 21199
		private bool placedOnFloor;

		// Token: 0x040052D0 RID: 21200
		private float placedOnFloorTime;

		// Token: 0x040052D1 RID: 21201
		private float timeCreated;

		// Token: 0x040052D2 RID: 21202
		private bool deflated;

		// Token: 0x040052D3 RID: 21203
		private Vector3 handContactPoint;

		// Token: 0x040052D4 RID: 21204
		private Vector3 handNormalVector;

		// Token: 0x040052D5 RID: 21205
		private CallLimiter callLimiter = new CallLimiter(10, 2f, 0.5f);

		// Token: 0x040052D8 RID: 21208
		private RubberDuckEvents _events;
	}
}
