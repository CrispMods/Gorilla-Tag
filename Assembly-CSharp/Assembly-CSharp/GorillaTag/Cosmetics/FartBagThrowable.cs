﻿using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C42 RID: 3138
	public class FartBagThrowable : MonoBehaviour, IProjectile
	{
		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06004E46 RID: 20038 RVA: 0x00180FF5 File Offset: 0x0017F1F5
		// (set) Token: 0x06004E47 RID: 20039 RVA: 0x00180FFD File Offset: 0x0017F1FD
		public TransferrableObject ParentTransferable { get; set; }

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06004E48 RID: 20040 RVA: 0x00181008 File Offset: 0x0017F208
		// (remove) Token: 0x06004E49 RID: 20041 RVA: 0x00181040 File Offset: 0x0017F240
		public event Action<IProjectile> OnDeflated;

		// Token: 0x06004E4A RID: 20042 RVA: 0x00181078 File Offset: 0x0017F278
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

		// Token: 0x06004E4B RID: 20043 RVA: 0x001810D7 File Offset: 0x0017F2D7
		private void Update()
		{
			if (Time.time - this.timeCreated > this.forceDestroyAfterSec)
			{
				this.DeflateLocal();
			}
		}

		// Token: 0x06004E4C RID: 20044 RVA: 0x001810F4 File Offset: 0x0017F2F4
		public void Launch(Vector3 startPosition, Quaternion startRotation, Vector3 velocity, float scale)
		{
			base.transform.position = startPosition;
			base.transform.rotation = startRotation;
			base.transform.localScale = Vector3.one * scale;
			this.rigidbody.velocity = velocity;
			this.timeCreated = Time.time;
			this.InitialPhotonEvent();
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x00181150 File Offset: 0x0017F350
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

		// Token: 0x06004E4E RID: 20046 RVA: 0x00181224 File Offset: 0x0017F424
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

		// Token: 0x06004E4F RID: 20047 RVA: 0x001812AC File Offset: 0x0017F4AC
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

		// Token: 0x06004E50 RID: 20048 RVA: 0x00181354 File Offset: 0x0017F554
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

		// Token: 0x06004E51 RID: 20049 RVA: 0x001813C4 File Offset: 0x0017F5C4
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

		// Token: 0x06004E52 RID: 20050 RVA: 0x00181474 File Offset: 0x0017F674
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

		// Token: 0x06004E53 RID: 20051 RVA: 0x00181522 File Offset: 0x0017F722
		private void DisableObject()
		{
			Action<IProjectile> onDeflated = this.OnDeflated;
			if (onDeflated != null)
			{
				onDeflated(this);
			}
			this.deflated = false;
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x00181540 File Offset: 0x0017F740
		private void OnDestroy()
		{
			if (this._events != null)
			{
				this._events.Activate -= this.DeflateEvent;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x040051CD RID: 20941
		[SerializeField]
		private GameObject deflationEffect;

		// Token: 0x040051CE RID: 20942
		[SerializeField]
		private float destroyWhenDeflateDelay = 3f;

		// Token: 0x040051CF RID: 20943
		[SerializeField]
		private float forceDestroyAfterSec = 10f;

		// Token: 0x040051D0 RID: 20944
		[SerializeField]
		private float placementOffset = 0.2f;

		// Token: 0x040051D1 RID: 20945
		[SerializeField]
		private UpdateBlendShapeCosmetic updateBlendShapeCosmetic;

		// Token: 0x040051D2 RID: 20946
		[SerializeField]
		private LayerMask floorLayerMask;

		// Token: 0x040051D3 RID: 20947
		[SerializeField]
		private LayerMask handLayerMask;

		// Token: 0x040051D4 RID: 20948
		[SerializeField]
		private Rigidbody rigidbody;

		// Token: 0x040051D5 RID: 20949
		private bool placedOnFloor;

		// Token: 0x040051D6 RID: 20950
		private float placedOnFloorTime;

		// Token: 0x040051D7 RID: 20951
		private float timeCreated;

		// Token: 0x040051D8 RID: 20952
		private bool deflated;

		// Token: 0x040051D9 RID: 20953
		private Vector3 handContactPoint;

		// Token: 0x040051DA RID: 20954
		private Vector3 handNormalVector;

		// Token: 0x040051DB RID: 20955
		private CallLimiter callLimiter = new CallLimiter(10, 2f, 0.5f);

		// Token: 0x040051DE RID: 20958
		private RubberDuckEvents _events;
	}
}
