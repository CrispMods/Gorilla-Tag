using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C2D RID: 3117
	[RequireComponent(typeof(TransferrableObject))]
	public class SeedPacketHoldable : MonoBehaviour
	{
		// Token: 0x06004DCA RID: 19914 RVA: 0x0017D71A File Offset: 0x0017B91A
		private void Awake()
		{
			this.transferrableObject = base.GetComponent<TransferrableObject>();
			this.flowerEffectHash = PoolUtils.GameObjHashCode(this.flowerEffectPrefab);
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x0017D73C File Offset: 0x0017B93C
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

		// Token: 0x06004DCC RID: 19916 RVA: 0x0017D804 File Offset: 0x0017BA04
		private void OnDisable()
		{
			if (this._events != null)
			{
				this._events.Activate -= this.SyncTriggerEffect;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x0017D853 File Offset: 0x0017BA53
		private void OnDestroy()
		{
			this.pooledObjects.Clear();
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x0017D860 File Offset: 0x0017BA60
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

		// Token: 0x06004DCF RID: 19919 RVA: 0x0017D919 File Offset: 0x0017BB19
		private void StartPouring()
		{
			if (this.particles)
			{
				this.particles.Play();
			}
			this.isPouring = true;
			this.pouringStartedTime = Time.time;
		}

		// Token: 0x06004DD0 RID: 19920 RVA: 0x0017D948 File Offset: 0x0017BB48
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

		// Token: 0x06004DD1 RID: 19921 RVA: 0x0017D9A4 File Offset: 0x0017BBA4
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

		// Token: 0x06004DD2 RID: 19922 RVA: 0x0017DA08 File Offset: 0x0017BC08
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

		// Token: 0x040050CC RID: 20684
		[SerializeField]
		private float cooldown;

		// Token: 0x040050CD RID: 20685
		[SerializeField]
		private ParticleSystem particles;

		// Token: 0x040050CE RID: 20686
		[SerializeField]
		private float pouringAngle;

		// Token: 0x040050CF RID: 20687
		[SerializeField]
		private float pouringRaycastDistance = 5f;

		// Token: 0x040050D0 RID: 20688
		[SerializeField]
		private LayerMask raycastLayerMask;

		// Token: 0x040050D1 RID: 20689
		[SerializeField]
		private float placeEffectDelayMultiplier = 10f;

		// Token: 0x040050D2 RID: 20690
		[SerializeField]
		private GameObject flowerEffectPrefab;

		// Token: 0x040050D3 RID: 20691
		private List<OnTriggerEventsHandlerCosmetic> pooledObjects = new List<OnTriggerEventsHandlerCosmetic>();

		// Token: 0x040050D4 RID: 20692
		private CallLimiter callLimiter = new CallLimiter(10, 3f, 0.5f);

		// Token: 0x040050D5 RID: 20693
		private int flowerEffectHash;

		// Token: 0x040050D6 RID: 20694
		private Vector3 hitPoint;

		// Token: 0x040050D7 RID: 20695
		private TransferrableObject transferrableObject;

		// Token: 0x040050D8 RID: 20696
		private bool isPouring = true;

		// Token: 0x040050D9 RID: 20697
		private float pouringStartedTime;

		// Token: 0x040050DA RID: 20698
		private RubberDuckEvents _events;
	}
}
