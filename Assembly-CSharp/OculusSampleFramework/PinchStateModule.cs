using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A8C RID: 2700
	public class PinchStateModule
	{
		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06004366 RID: 17254 RVA: 0x0005BE7C File Offset: 0x0005A07C
		public bool PinchUpAndDownOnFocusedObject
		{
			get
			{
				return this._currPinchState == PinchStateModule.PinchState.PinchUp && this._firstFocusedInteractable != null;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06004367 RID: 17255 RVA: 0x0005BE95 File Offset: 0x0005A095
		public bool PinchSteadyOnFocusedObject
		{
			get
			{
				return this._currPinchState == PinchStateModule.PinchState.PinchStay && this._firstFocusedInteractable != null;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06004368 RID: 17256 RVA: 0x0005BEAE File Offset: 0x0005A0AE
		public bool PinchDownOnFocusedObject
		{
			get
			{
				return this._currPinchState == PinchStateModule.PinchState.PinchDown && this._firstFocusedInteractable != null;
			}
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x0005BEC7 File Offset: 0x0005A0C7
		public PinchStateModule()
		{
			this._currPinchState = PinchStateModule.PinchState.None;
			this._firstFocusedInteractable = null;
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x00177E6C File Offset: 0x0017606C
		public void UpdateState(OVRHand hand, Interactable currFocusedInteractable)
		{
			float fingerPinchStrength = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
			bool flag = Mathf.Abs(1f - fingerPinchStrength) < Mathf.Epsilon;
			switch (this._currPinchState)
			{
			case PinchStateModule.PinchState.PinchDown:
				this._currPinchState = (flag ? PinchStateModule.PinchState.PinchStay : PinchStateModule.PinchState.PinchUp);
				if (this._firstFocusedInteractable != currFocusedInteractable)
				{
					this._firstFocusedInteractable = null;
					return;
				}
				break;
			case PinchStateModule.PinchState.PinchStay:
				if (!flag)
				{
					this._currPinchState = PinchStateModule.PinchState.PinchUp;
				}
				if (currFocusedInteractable != this._firstFocusedInteractable)
				{
					this._firstFocusedInteractable = null;
					return;
				}
				break;
			case PinchStateModule.PinchState.PinchUp:
				if (!flag)
				{
					this._currPinchState = PinchStateModule.PinchState.None;
					this._firstFocusedInteractable = null;
					return;
				}
				this._currPinchState = PinchStateModule.PinchState.PinchDown;
				if (currFocusedInteractable != this._firstFocusedInteractable)
				{
					this._firstFocusedInteractable = null;
					return;
				}
				break;
			default:
				if (flag)
				{
					this._currPinchState = PinchStateModule.PinchState.PinchDown;
					this._firstFocusedInteractable = currFocusedInteractable;
				}
				break;
			}
		}

		// Token: 0x0400441A RID: 17434
		private const float PINCH_STRENGTH_THRESHOLD = 1f;

		// Token: 0x0400441B RID: 17435
		private PinchStateModule.PinchState _currPinchState;

		// Token: 0x0400441C RID: 17436
		private Interactable _firstFocusedInteractable;

		// Token: 0x02000A8D RID: 2701
		private enum PinchState
		{
			// Token: 0x0400441E RID: 17438
			None,
			// Token: 0x0400441F RID: 17439
			PinchDown,
			// Token: 0x04004420 RID: 17440
			PinchStay,
			// Token: 0x04004421 RID: 17441
			PinchUp
		}
	}
}
