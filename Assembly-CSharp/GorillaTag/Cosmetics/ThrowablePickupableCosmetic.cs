using System;
using GorillaExtensions;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C60 RID: 3168
	public class ThrowablePickupableCosmetic : TransferrableObject
	{
		// Token: 0x06004F45 RID: 20293 RVA: 0x001B6668 File Offset: 0x001B4868
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
				else
				{
					Debug.LogError("Failed to get a reference to the Photon Player needed to hook up the cosmetic event");
				}
			}
			if (this._events != null)
			{
				this._events.Activate += this.OnReleaseEvent;
			}
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x001B6738 File Offset: 0x001B4938
		internal override void OnDisable()
		{
			base.OnDisable();
			if (this._events != null)
			{
				this._events.Activate -= this.OnReleaseEvent;
				UnityEngine.Object.Destroy(this._events);
				this._events = null;
			}
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x001B6790 File Offset: 0x001B4990
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			if (VRRigCache.Instance.localRig.Rig != this.ownerRig)
			{
				return;
			}
			if (this.pickupableVariant.enabled)
			{
				if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null && PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
				{
					this._events.Activate.RaiseOthers(new object[]
					{
						false
					});
				}
				base.transform.position = this.pickupableVariant.transform.position;
				base.transform.rotation = this.pickupableVariant.transform.rotation;
				this.pickupableVariant.Pickup();
			}
			UnityEvent onGrabFromDockPosition = this.OnGrabFromDockPosition;
			if (onGrabFromDockPosition != null)
			{
				onGrabFromDockPosition.Invoke();
			}
			base.OnGrab(pointGrabbed, grabbingHand);
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x001B6894 File Offset: 0x001B4A94
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
			if (this.DistanceToDock() > this.returnToDockDistanceThreshold)
			{
				Vector3 position = base.transform.position;
				Vector3 vector = (releasingHand == EquipmentInteractor.instance.leftHand) ? GTPlayer.Instance.leftInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false) : GTPlayer.Instance.rightInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false);
				float scale = GTPlayer.Instance.scale;
				if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
				{
					this._events.Activate.RaiseOthers(new object[]
					{
						true,
						position,
						vector,
						scale
					});
				}
				this.OnReleaseEventLocal(position, vector, scale);
			}
			else
			{
				UnityEvent onReturnToDockPosition = this.OnReturnToDockPosition;
				if (onReturnToDockPosition != null)
				{
					onReturnToDockPosition.Invoke();
				}
			}
			return true;
		}

		// Token: 0x06004F49 RID: 20297 RVA: 0x001B69B8 File Offset: 0x001B4BB8
		private void OnReleaseEvent(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
		{
			if (sender != target)
			{
				return;
			}
			if (info.senderID != this.ownerRig.creator.ActorNumber)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "OnReleaseEvent");
			if (!this.callLimiter.CheckCallTime(Time.time))
			{
				return;
			}
			object obj = args[0];
			if (obj is bool)
			{
				bool flag = (bool)obj;
				if (flag)
				{
					obj = args[1];
					if (obj is Vector3)
					{
						Vector3 vector = (Vector3)obj;
						obj = args[2];
						if (obj is Vector3)
						{
							Vector3 inVel = (Vector3)obj;
							obj = args[3];
							if (obj is float)
							{
								float value = (float)obj;
								Vector3 position = base.transform.position;
								Vector3 releaseVelocity = base.transform.forward;
								ref position.SetValueSafe(vector);
								if (this.ownerRig.IsPositionInRange(position, 4f))
								{
									releaseVelocity = this.ownerRig.ClampVelocityRelativeToPlayerSafe(inVel, 50f);
									float playerScale = value.ClampSafe(0.01f, 1f);
									this.OnReleaseEventLocal(position, releaseVelocity, playerScale);
									return;
								}
								return;
							}
						}
					}
					return;
				}
				this.pickupableVariant.Pickup();
				return;
			}
		}

		// Token: 0x06004F4A RID: 20298 RVA: 0x00063C42 File Offset: 0x00061E42
		private void OnReleaseEventLocal(Vector3 startPosition, Vector3 releaseVelocity, float playerScale)
		{
			this.pickupableVariant.Release(this, startPosition, releaseVelocity, playerScale);
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x001B6AD4 File Offset: 0x001B4CD4
		private float DistanceToDock()
		{
			float result = 0f;
			if (this.currentState == TransferrableObject.PositionState.OnRightShoulder)
			{
				result = Vector3.Distance(this.ownerRig.myBodyDockPositions.rightBackTransform.position, base.transform.position);
			}
			else if (this.currentState == TransferrableObject.PositionState.OnLeftShoulder)
			{
				result = Vector3.Distance(this.ownerRig.myBodyDockPositions.leftBackTransform.position, base.transform.position);
			}
			else if (this.currentState == TransferrableObject.PositionState.OnRightArm)
			{
				result = Vector3.Distance(this.ownerRig.myBodyDockPositions.rightArmTransform.position, base.transform.position);
			}
			else if (this.currentState == TransferrableObject.PositionState.OnLeftArm)
			{
				result = Vector3.Distance(this.ownerRig.myBodyDockPositions.leftArmTransform.position, base.transform.position);
			}
			else if (this.currentState == TransferrableObject.PositionState.OnChest)
			{
				result = Vector3.Distance(this.ownerRig.myBodyDockPositions.chestTransform.position, base.transform.position);
			}
			return result;
		}

		// Token: 0x040051F3 RID: 20979
		[SerializeField]
		private PickupableVariant pickupableVariant;

		// Token: 0x040051F4 RID: 20980
		[SerializeField]
		private float returnToDockDistanceThreshold = 0.7f;

		// Token: 0x040051F5 RID: 20981
		public UnityEvent OnReturnToDockPosition;

		// Token: 0x040051F6 RID: 20982
		public UnityEvent OnGrabFromDockPosition;

		// Token: 0x040051F7 RID: 20983
		private RubberDuckEvents _events;

		// Token: 0x040051F8 RID: 20984
		private CallLimiter callLimiter = new CallLimiter(10, 2f, 0.5f);
	}
}
