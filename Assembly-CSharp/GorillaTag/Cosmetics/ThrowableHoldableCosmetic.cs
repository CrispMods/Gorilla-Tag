using System;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaTag.Shared.Scripts;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C2F RID: 3119
	public class ThrowableHoldableCosmetic : TransferrableObject
	{
		// Token: 0x06004DE3 RID: 19939 RVA: 0x0017DC20 File Offset: 0x0017BE20
		internal override void OnEnable()
		{
			base.OnEnable();
			if (this._events == null)
			{
				this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
				NetPlayer netPlayer = (base.myOnlineRig != null) ? base.myOnlineRig.creator : ((base.myRig != null) ? ((base.myRig.creator != null) ? base.myRig.creator : NetworkSystem.Instance.LocalPlayer) : null);
				if (netPlayer != null)
				{
					this._events.Init(netPlayer);
				}
			}
			if (this._events != null)
			{
				this._events.Activate += this.OnThrowEvent;
			}
		}

		// Token: 0x06004DE4 RID: 19940 RVA: 0x0017DCE2 File Offset: 0x0017BEE2
		protected override void Awake()
		{
			base.Awake();
			this.firecrackerProjectileHash = PoolUtils.GameObjHashCode(this.firecrackerProjectilePrefab);
			this.playersEffect = base.GetComponentInChildren<CosmeticEffectsOnPlayers>();
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x0017DD07 File Offset: 0x0017BF07
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			if (!this.disableWhenThrown.gameObject.activeSelf)
			{
				return;
			}
			base.OnGrab(pointGrabbed, grabbingHand);
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x0017DD24 File Offset: 0x0017BF24
		public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
		{
			if (!base.OnRelease(zoneReleased, releasingHand))
			{
				return false;
			}
			if (VRRigCache.Instance.localRig.Rig != this.ownerRig)
			{
				return false;
			}
			Vector3 position = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			Vector3 vector = (releasingHand == EquipmentInteractor.instance.leftHand) ? GTPlayer.Instance.leftInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false) : GTPlayer.Instance.rightInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false);
			float scale = GTPlayer.Instance.scale;
			if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
			{
				this._events.Activate.RaiseOthers(new object[]
				{
					position,
					rotation,
					vector,
					scale
				});
			}
			this.OnThrowLocal(position, rotation, vector, scale);
			return true;
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x0017DE30 File Offset: 0x0017C030
		internal override void OnDisable()
		{
			base.OnDisable();
			if (this._events != null)
			{
				this._events.Activate -= this.OnThrowEvent;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x0017DE88 File Offset: 0x0017C088
		private void OnThrowEvent(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
		{
			if (sender != target)
			{
				return;
			}
			if (args.Length != 4)
			{
				return;
			}
			if (info.senderID != this.ownerRig.creator.ActorNumber)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "OnThrowEvent");
			if (this.firecrackerCallLimiter.CheckCallTime(Time.time))
			{
				object obj = args[0];
				if (obj is Vector3)
				{
					Vector3 vector = (Vector3)obj;
					obj = args[1];
					if (obj is Quaternion)
					{
						Quaternion rotation = (Quaternion)obj;
						obj = args[2];
						if (obj is Vector3)
						{
							Vector3 vector2 = (Vector3)obj;
							obj = args[3];
							if (obj is float)
							{
								float value = (float)obj;
								vector2 = this.targetRig.ClampVelocityRelativeToPlayerSafe(vector2, 40f);
								float playerScale = value.ClampSafe(0.01f, 1f);
								if (!rotation.IsValid())
								{
									return;
								}
								float num = 10000f;
								if (!vector.IsValid(num) || !this.targetRig.IsPositionInRange(vector, 4f))
								{
									return;
								}
								this.OnThrowLocal(vector, rotation, vector2, playerScale);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x0017DF94 File Offset: 0x0017C194
		private void OnThrowLocal(Vector3 startPos, Quaternion rotation, Vector3 velocity, float playerScale)
		{
			this.disableWhenThrown.SetActive(false);
			IProjectile component = ObjectPools.instance.Instantiate(this.firecrackerProjectileHash).GetComponent<IProjectile>();
			FirecrackerProjectile firecrackerProjectile = component as FirecrackerProjectile;
			if (firecrackerProjectile != null)
			{
				FirecrackerProjectile firecrackerProjectile2 = firecrackerProjectile;
				firecrackerProjectile2.OnHitComplete = (Action<FirecrackerProjectile>)Delegate.Combine(firecrackerProjectile2.OnHitComplete, new Action<FirecrackerProjectile>(this.HitComplete));
				FirecrackerProjectile firecrackerProjectile3 = firecrackerProjectile;
				firecrackerProjectile3.OnHitStart = (Action<FirecrackerProjectile, Vector3>)Delegate.Combine(firecrackerProjectile3.OnHitStart, new Action<FirecrackerProjectile, Vector3>(this.HitStart));
			}
			else
			{
				FartBagThrowable fartBagThrowable = component as FartBagThrowable;
				if (fartBagThrowable != null)
				{
					fartBagThrowable.OnDeflated += this.HitComplete;
					fartBagThrowable.ParentTransferable = this;
				}
			}
			component.Launch(startPos, rotation, velocity, playerScale);
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x0017E041 File Offset: 0x0017C241
		private void HitStart(FirecrackerProjectile firecracker, Vector3 contactPos)
		{
			if (firecracker == null)
			{
				return;
			}
			if (this.playersEffect == null)
			{
				return;
			}
			this.playersEffect.ApplyAllEffectsByDistance(contactPos);
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x0017E068 File Offset: 0x0017C268
		private void HitComplete(IProjectile projectile)
		{
			if (projectile == null)
			{
				return;
			}
			this.disableWhenThrown.SetActive(true);
			FirecrackerProjectile firecrackerProjectile = projectile as FirecrackerProjectile;
			if (firecrackerProjectile != null)
			{
				FirecrackerProjectile firecrackerProjectile2 = firecrackerProjectile;
				firecrackerProjectile2.OnHitStart = (Action<FirecrackerProjectile, Vector3>)Delegate.Remove(firecrackerProjectile2.OnHitStart, new Action<FirecrackerProjectile, Vector3>(this.HitStart));
				FirecrackerProjectile firecrackerProjectile3 = firecrackerProjectile;
				firecrackerProjectile3.OnHitComplete = (Action<FirecrackerProjectile>)Delegate.Remove(firecrackerProjectile3.OnHitComplete, new Action<FirecrackerProjectile>(this.HitComplete));
				ObjectPools.instance.Destroy(firecrackerProjectile.gameObject);
				return;
			}
			FartBagThrowable fartBagThrowable = projectile as FartBagThrowable;
			if (fartBagThrowable != null)
			{
				fartBagThrowable.OnDeflated -= this.HitComplete;
				ObjectPools.instance.Destroy(fartBagThrowable.gameObject);
			}
		}

		// Token: 0x040050E7 RID: 20711
		[SerializeField]
		private GameObject firecrackerProjectilePrefab;

		// Token: 0x040050E8 RID: 20712
		[SerializeField]
		private GameObject disableWhenThrown;

		// Token: 0x040050E9 RID: 20713
		private CallLimiter firecrackerCallLimiter = new CallLimiter(10, 3f, 0.5f);

		// Token: 0x040050EA RID: 20714
		private CosmeticEffectsOnPlayers playersEffect;

		// Token: 0x040050EB RID: 20715
		private int firecrackerProjectileHash;

		// Token: 0x040050EC RID: 20716
		private RubberDuckEvents _events;
	}
}
