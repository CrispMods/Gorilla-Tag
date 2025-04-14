using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C30 RID: 3120
	[RequireComponent(typeof(TransferrableObject))]
	public class SeedPacketHoldable : MonoBehaviour
	{
		// Token: 0x06004DD6 RID: 19926 RVA: 0x0017DCE2 File Offset: 0x0017BEE2
		private void Awake()
		{
			this.transferrableObject = base.GetComponent<TransferrableObject>();
			this.flowerEffectHash = PoolUtils.GameObjHashCode(this.flowerEffectPrefab);
		}

		// Token: 0x06004DD7 RID: 19927 RVA: 0x0017DD04 File Offset: 0x0017BF04
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

		// Token: 0x06004DD8 RID: 19928 RVA: 0x0017DDCC File Offset: 0x0017BFCC
		private void OnDisable()
		{
			if (this._events != null)
			{
				this._events.Activate -= this.SyncTriggerEffect;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x0017DE1B File Offset: 0x0017C01B
		private void OnDestroy()
		{
			this.pooledObjects.Clear();
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x0017DE28 File Offset: 0x0017C028
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

		// Token: 0x06004DDB RID: 19931 RVA: 0x0017DEE1 File Offset: 0x0017C0E1
		private void StartPouring()
		{
			if (this.particles)
			{
				this.particles.Play();
			}
			this.isPouring = true;
			this.pouringStartedTime = Time.time;
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x0017DF10 File Offset: 0x0017C110
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

		// Token: 0x06004DDD RID: 19933 RVA: 0x0017DF6C File Offset: 0x0017C16C
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

		// Token: 0x06004DDE RID: 19934 RVA: 0x0017DFD0 File Offset: 0x0017C1D0
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

		// Token: 0x040050DE RID: 20702
		[SerializeField]
		private float cooldown;

		// Token: 0x040050DF RID: 20703
		[SerializeField]
		private ParticleSystem particles;

		// Token: 0x040050E0 RID: 20704
		[SerializeField]
		private float pouringAngle;

		// Token: 0x040050E1 RID: 20705
		[SerializeField]
		private float pouringRaycastDistance = 5f;

		// Token: 0x040050E2 RID: 20706
		[SerializeField]
		private LayerMask raycastLayerMask;

		// Token: 0x040050E3 RID: 20707
		[SerializeField]
		private float placeEffectDelayMultiplier = 10f;

		// Token: 0x040050E4 RID: 20708
		[SerializeField]
		private GameObject flowerEffectPrefab;

		// Token: 0x040050E5 RID: 20709
		private List<OnTriggerEventsHandlerCosmetic> pooledObjects = new List<OnTriggerEventsHandlerCosmetic>();

		// Token: 0x040050E6 RID: 20710
		private CallLimiter callLimiter = new CallLimiter(10, 3f, 0.5f);

		// Token: 0x040050E7 RID: 20711
		private int flowerEffectHash;

		// Token: 0x040050E8 RID: 20712
		private Vector3 hitPoint;

		// Token: 0x040050E9 RID: 20713
		private TransferrableObject transferrableObject;

		// Token: 0x040050EA RID: 20714
		private bool isPouring = true;

		// Token: 0x040050EB RID: 20715
		private float pouringStartedTime;

		// Token: 0x040050EC RID: 20716
		private RubberDuckEvents _events;
	}
}
