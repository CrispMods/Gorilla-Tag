using System;
using GorillaExtensions;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C49 RID: 3145
	public class DiceHoldable : TransferrableObject
	{
		// Token: 0x06004E9E RID: 20126 RVA: 0x001B0A60 File Offset: 0x001AEC60
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
				this._events.Activate += this.OnDiceEvent;
			}
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x001B0B30 File Offset: 0x001AED30
		internal override void OnDisable()
		{
			base.OnDisable();
			if (this._events != null)
			{
				this._events.Activate -= this.OnDiceEvent;
				UnityEngine.Object.Destroy(this._events);
				this._events = null;
			}
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x001B0B88 File Offset: 0x001AED88
		private void OnDiceEvent(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
		{
			GorillaNot.IncrementRPCCall(info, "OnDiceEvent");
			if (sender != target)
			{
				return;
			}
			if (info.senderID != this.ownerRig.creator.ActorNumber)
			{
				return;
			}
			if ((bool)args[0])
			{
				Vector3 position = base.transform.position;
				Vector3 forward = base.transform.forward;
				Vector3 vector = (Vector3)args[1];
				ref position.SetValueSafe(vector);
				vector = (Vector3)args[2];
				ref forward.SetValueSafe(vector);
				float playerScale = ((float)args[3]).ClampSafe(0.01f, 1f);
				int landingSide = Mathf.Clamp((int)args[4], 1, 20);
				double finite = ((double)args[5]).GetFinite();
				this.ThrowDiceLocal(position, forward, playerScale, landingSide, finite);
				return;
			}
			this.dicePhysics.EndThrow();
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x001B0C5C File Offset: 0x001AEE5C
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			if (this.dicePhysics.enabled)
			{
				if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
				{
					object[] args = new object[]
					{
						false
					};
					this._events.Activate.RaiseOthers(args);
				}
				base.transform.position = this.dicePhysics.transform.position;
				base.transform.rotation = this.dicePhysics.transform.rotation;
				this.dicePhysics.EndThrow();
				if (grabbingHand == EquipmentInteractor.instance.leftHand && this.currentState == TransferrableObject.PositionState.OnLeftArm)
				{
					this.canAutoGrabLeft = false;
					this.interpState = TransferrableObject.InterpolateState.None;
					this.currentState = TransferrableObject.PositionState.InLeftHand;
				}
				else if (grabbingHand == EquipmentInteractor.instance.rightHand && this.currentState == TransferrableObject.PositionState.OnRightArm)
				{
					this.canAutoGrabRight = false;
					this.interpState = TransferrableObject.InterpolateState.None;
					this.currentState = TransferrableObject.PositionState.InLeftHand;
				}
			}
			base.OnGrab(pointGrabbed, grabbingHand);
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x001B0D74 File Offset: 0x001AEF74
		public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
		{
			if (!base.OnRelease(zoneReleased, releasingHand))
			{
				return false;
			}
			if (zoneReleased == null)
			{
				Vector3 position = base.transform.position;
				Vector3 vector = (releasingHand == EquipmentInteractor.instance.leftHand) ? GTPlayer.Instance.leftInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false) : GTPlayer.Instance.rightInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false);
				int randomSide = this.dicePhysics.GetRandomSide();
				double num = PhotonNetwork.InRoom ? PhotonNetwork.Time : -1.0;
				float scale = GTPlayer.Instance.scale;
				if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
				{
					object[] args = new object[]
					{
						true,
						position,
						vector,
						scale,
						randomSide,
						num
					};
					this._events.Activate.RaiseOthers(args);
				}
				this.ThrowDiceLocal(position, vector, scale, randomSide, num);
			}
			return true;
		}

		// Token: 0x06004EA3 RID: 20131 RVA: 0x00063732 File Offset: 0x00061932
		private void ThrowDiceLocal(Vector3 startPosition, Vector3 throwVelocity, float playerScale, int landingSide, double startTime)
		{
			this.dicePhysics.StartThrow(this, startPosition, throwVelocity, playerScale, landingSide, startTime);
		}

		// Token: 0x040050BB RID: 20667
		[SerializeField]
		private DicePhysics dicePhysics;

		// Token: 0x040050BC RID: 20668
		private RubberDuckEvents _events;
	}
}
