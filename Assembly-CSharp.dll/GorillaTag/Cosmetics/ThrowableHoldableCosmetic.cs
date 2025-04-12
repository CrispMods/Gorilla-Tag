using System;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaTag.Shared.Scripts;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C32 RID: 3122
	public class ThrowableHoldableCosmetic : TransferrableObject
	{
		// Token: 0x06004DEF RID: 19951 RVA: 0x001AE674 File Offset: 0x001AC874
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

		// Token: 0x06004DF0 RID: 19952 RVA: 0x000621D0 File Offset: 0x000603D0
		protected override void Awake()
		{
			base.Awake();
			this.firecrackerProjectileHash = PoolUtils.GameObjHashCode(this.firecrackerProjectilePrefab);
			this.playersEffect = base.GetComponentInChildren<CosmeticEffectsOnPlayers>();
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x000621F5 File Offset: 0x000603F5
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			if (!this.disableWhenThrown.gameObject.activeSelf)
			{
				return;
			}
			base.OnGrab(pointGrabbed, grabbingHand);
		}

		// Token: 0x06004DF2 RID: 19954 RVA: 0x001AE738 File Offset: 0x001AC938
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

		// Token: 0x06004DF3 RID: 19955 RVA: 0x001AE844 File Offset: 0x001ACA44
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

		// Token: 0x06004DF4 RID: 19956 RVA: 0x001AE89C File Offset: 0x001ACA9C
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

		// Token: 0x06004DF5 RID: 19957 RVA: 0x001AE9A8 File Offset: 0x001ACBA8
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

		// Token: 0x06004DF6 RID: 19958 RVA: 0x00062212 File Offset: 0x00060412
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

		// Token: 0x06004DF7 RID: 19959 RVA: 0x001AEA58 File Offset: 0x001ACC58
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

		// Token: 0x040050F9 RID: 20729
		[SerializeField]
		private GameObject firecrackerProjectilePrefab;

		// Token: 0x040050FA RID: 20730
		[SerializeField]
		private GameObject disableWhenThrown;

		// Token: 0x040050FB RID: 20731
		private CallLimiter firecrackerCallLimiter = new CallLimiter(10, 3f, 0.5f);

		// Token: 0x040050FC RID: 20732
		private CosmeticEffectsOnPlayers playersEffect;

		// Token: 0x040050FD RID: 20733
		private int firecrackerProjectileHash;

		// Token: 0x040050FE RID: 20734
		private RubberDuckEvents _events;
	}
}
