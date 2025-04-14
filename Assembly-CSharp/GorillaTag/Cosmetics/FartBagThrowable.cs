using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C3F RID: 3135
	public class FartBagThrowable : MonoBehaviour, IProjectile
	{
		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06004E3A RID: 20026 RVA: 0x00180A2D File Offset: 0x0017EC2D
		// (set) Token: 0x06004E3B RID: 20027 RVA: 0x00180A35 File Offset: 0x0017EC35
		public TransferrableObject ParentTransferable { get; set; }

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06004E3C RID: 20028 RVA: 0x00180A40 File Offset: 0x0017EC40
		// (remove) Token: 0x06004E3D RID: 20029 RVA: 0x00180A78 File Offset: 0x0017EC78
		public event Action<IProjectile> OnDeflated;

		// Token: 0x06004E3E RID: 20030 RVA: 0x00180AB0 File Offset: 0x0017ECB0
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

		// Token: 0x06004E3F RID: 20031 RVA: 0x00180B0F File Offset: 0x0017ED0F
		private void Update()
		{
			if (Time.time - this.timeCreated > this.forceDestroyAfterSec)
			{
				this.DeflateLocal();
			}
		}

		// Token: 0x06004E40 RID: 20032 RVA: 0x00180B2C File Offset: 0x0017ED2C
		public void Launch(Vector3 startPosition, Quaternion startRotation, Vector3 velocity, float scale)
		{
			base.transform.position = startPosition;
			base.transform.rotation = startRotation;
			base.transform.localScale = Vector3.one * scale;
			this.rigidbody.velocity = velocity;
			this.timeCreated = Time.time;
			this.InitialPhotonEvent();
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x00180B88 File Offset: 0x0017ED88
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

		// Token: 0x06004E42 RID: 20034 RVA: 0x00180C5C File Offset: 0x0017EE5C
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

		// Token: 0x06004E43 RID: 20035 RVA: 0x00180CE4 File Offset: 0x0017EEE4
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

		// Token: 0x06004E44 RID: 20036 RVA: 0x00180D8C File Offset: 0x0017EF8C
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

		// Token: 0x06004E45 RID: 20037 RVA: 0x00180DFC File Offset: 0x0017EFFC
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

		// Token: 0x06004E46 RID: 20038 RVA: 0x00180EAC File Offset: 0x0017F0AC
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

		// Token: 0x06004E47 RID: 20039 RVA: 0x00180F5A File Offset: 0x0017F15A
		private void DisableObject()
		{
			Action<IProjectile> onDeflated = this.OnDeflated;
			if (onDeflated != null)
			{
				onDeflated(this);
			}
			this.deflated = false;
		}

		// Token: 0x06004E48 RID: 20040 RVA: 0x00180F78 File Offset: 0x0017F178
		private void OnDestroy()
		{
			if (this._events != null)
			{
				this._events.Activate -= this.DeflateEvent;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x040051BB RID: 20923
		[SerializeField]
		private GameObject deflationEffect;

		// Token: 0x040051BC RID: 20924
		[SerializeField]
		private float destroyWhenDeflateDelay = 3f;

		// Token: 0x040051BD RID: 20925
		[SerializeField]
		private float forceDestroyAfterSec = 10f;

		// Token: 0x040051BE RID: 20926
		[SerializeField]
		private float placementOffset = 0.2f;

		// Token: 0x040051BF RID: 20927
		[SerializeField]
		private UpdateBlendShapeCosmetic updateBlendShapeCosmetic;

		// Token: 0x040051C0 RID: 20928
		[SerializeField]
		private LayerMask floorLayerMask;

		// Token: 0x040051C1 RID: 20929
		[SerializeField]
		private LayerMask handLayerMask;

		// Token: 0x040051C2 RID: 20930
		[SerializeField]
		private Rigidbody rigidbody;

		// Token: 0x040051C3 RID: 20931
		private bool placedOnFloor;

		// Token: 0x040051C4 RID: 20932
		private float placedOnFloorTime;

		// Token: 0x040051C5 RID: 20933
		private float timeCreated;

		// Token: 0x040051C6 RID: 20934
		private bool deflated;

		// Token: 0x040051C7 RID: 20935
		private Vector3 handContactPoint;

		// Token: 0x040051C8 RID: 20936
		private Vector3 handNormalVector;

		// Token: 0x040051C9 RID: 20937
		private CallLimiter callLimiter = new CallLimiter(10, 2f, 0.5f);

		// Token: 0x040051CC RID: 20940
		private RubberDuckEvents _events;
	}
}
