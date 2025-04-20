using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C5D RID: 3165
	[RequireComponent(typeof(TransferrableObject))]
	public class SeedPacketHoldable : MonoBehaviour
	{
		// Token: 0x06004F22 RID: 20258 RVA: 0x00063A8B File Offset: 0x00061C8B
		private void Awake()
		{
			this.transferrableObject = base.GetComponent<TransferrableObject>();
			this.flowerEffectHash = PoolUtils.GameObjHashCode(this.flowerEffectPrefab);
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x001B5E08 File Offset: 0x001B4008
		private void OnEnable()
		{
			if (this._events == null)
			{
				this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
				NetPlayer netPlayer = (this.transferrableObject.myOnlineRig != null) ? this.transferrableObject.myOnlineRig.creator : ((this.transferrableObject.myRig != null) ? (this.transferrableObject.myRig.creator ?? NetworkSystem.Instance.LocalPlayer) : null);
				if (netPlayer != null)
				{
					this._events.Init(netPlayer);
				}
			}
			if (this._events != null)
			{
				this._events.Activate += this.SyncTriggerEffect;
			}
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x001B5ED0 File Offset: 0x001B40D0
		private void OnDisable()
		{
			if (this._events != null)
			{
				this._events.Activate -= this.SyncTriggerEffect;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x00063AAA File Offset: 0x00061CAA
		private void OnDestroy()
		{
			this.pooledObjects.Clear();
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x001B5F20 File Offset: 0x001B4120
		private void Update()
		{
			if (!this.transferrableObject.InHand())
			{
				return;
			}
			if (!this.isPouring && Vector3.Angle(base.transform.up, Vector3.down) <= this.pouringAngle)
			{
				this.StartPouring();
				RaycastHit raycastHit;
				if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, this.pouringRaycastDistance, this.raycastLayerMask))
				{
					this.hitPoint = raycastHit.point;
					base.Invoke("SpawnEffect", raycastHit.distance * this.placeEffectDelayMultiplier);
				}
			}
			if (this.isPouring && Time.time - this.pouringStartedTime >= this.cooldown)
			{
				this.isPouring = false;
			}
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x00063AB7 File Offset: 0x00061CB7
		private void StartPouring()
		{
			if (this.particles)
			{
				this.particles.Play();
			}
			this.isPouring = true;
			this.pouringStartedTime = Time.time;
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x001B5FDC File Offset: 0x001B41DC
		private void SpawnEffect()
		{
			GameObject gameObject = ObjectPools.instance.Instantiate(this.flowerEffectHash);
			gameObject.transform.position = this.hitPoint;
			OnTriggerEventsHandlerCosmetic onTriggerEventsHandlerCosmetic;
			if (gameObject.TryGetComponent<OnTriggerEventsHandlerCosmetic>(out onTriggerEventsHandlerCosmetic))
			{
				this.pooledObjects.Add(onTriggerEventsHandlerCosmetic);
				onTriggerEventsHandlerCosmetic.onTriggerEntered.AddListener(new UnityAction<OnTriggerEventsHandlerCosmetic>(this.SyncTriggerEffectForOthers));
			}
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x001B6038 File Offset: 0x001B4238
		private void SyncTriggerEffectForOthers(OnTriggerEventsHandlerCosmetic onTriggerEventsHandlerCosmeticTriggerEvent)
		{
			int num = this.pooledObjects.IndexOf(onTriggerEventsHandlerCosmeticTriggerEvent);
			if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
			{
				this._events.Activate.RaiseOthers(new object[]
				{
					num
				});
			}
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x001B609C File Offset: 0x001B429C
		private void SyncTriggerEffect(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
		{
			if (sender != target)
			{
				return;
			}
			if (args.Length != 1)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "SyncTriggerEffect");
			if (!this.callLimiter.CheckCallTime(Time.time))
			{
				return;
			}
			int num = (int)args[0];
			if (num < 0 && num >= this.pooledObjects.Count)
			{
				return;
			}
			this.pooledObjects[num].ToggleEffects();
		}

		// Token: 0x040051D2 RID: 20946
		[SerializeField]
		private float cooldown;

		// Token: 0x040051D3 RID: 20947
		[SerializeField]
		private ParticleSystem particles;

		// Token: 0x040051D4 RID: 20948
		[SerializeField]
		private float pouringAngle;

		// Token: 0x040051D5 RID: 20949
		[SerializeField]
		private float pouringRaycastDistance = 5f;

		// Token: 0x040051D6 RID: 20950
		[SerializeField]
		private LayerMask raycastLayerMask;

		// Token: 0x040051D7 RID: 20951
		[SerializeField]
		private float placeEffectDelayMultiplier = 10f;

		// Token: 0x040051D8 RID: 20952
		[SerializeField]
		private GameObject flowerEffectPrefab;

		// Token: 0x040051D9 RID: 20953
		private List<OnTriggerEventsHandlerCosmetic> pooledObjects = new List<OnTriggerEventsHandlerCosmetic>();

		// Token: 0x040051DA RID: 20954
		private CallLimiter callLimiter = new CallLimiter(10, 3f, 0.5f);

		// Token: 0x040051DB RID: 20955
		private int flowerEffectHash;

		// Token: 0x040051DC RID: 20956
		private Vector3 hitPoint;

		// Token: 0x040051DD RID: 20957
		private TransferrableObject transferrableObject;

		// Token: 0x040051DE RID: 20958
		private bool isPouring = true;

		// Token: 0x040051DF RID: 20959
		private float pouringStartedTime;

		// Token: 0x040051E0 RID: 20960
		private RubberDuckEvents _events;
	}
}
