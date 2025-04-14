using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A5F RID: 2655
	public class PinchStateModule
	{
		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06004221 RID: 16929 RVA: 0x0013849D File Offset: 0x0013669D
		public bool PinchUpAndDownOnFocusedObject
		{
			get
			{
				return this._currPinchState == PinchStateModule.PinchState.PinchUp && this._firstFocusedInteractable != null;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06004222 RID: 16930 RVA: 0x001384B6 File Offset: 0x001366B6
		public bool PinchSteadyOnFocusedObject
		{
			get
			{
				return this._currPinchState == PinchStateModule.PinchState.PinchStay && this._firstFocusedInteractable != null;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06004223 RID: 16931 RVA: 0x001384CF File Offset: 0x001366CF
		public bool PinchDownOnFocusedObject
		{
			get
			{
				return this._currPinchState == PinchStateModule.PinchState.PinchDown && this._firstFocusedInteractable != null;
			}
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x001384E8 File Offset: 0x001366E8
		public PinchStateModule()
		{
			this._currPinchState = PinchStateModule.PinchState.None;
			this._firstFocusedInteractable = null;
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x00138500 File Offset: 0x00136700
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

		// Token: 0x04004320 RID: 17184
		private const float PINCH_STRENGTH_THRESHOLD = 1f;

		// Token: 0x04004321 RID: 17185
		private PinchStateModule.PinchState _currPinchState;

		// Token: 0x04004322 RID: 17186
		private Interactable _firstFocusedInteractable;

		// Token: 0x02000A60 RID: 2656
		private enum PinchState
		{
			// Token: 0x04004324 RID: 17188
			None,
			// Token: 0x04004325 RID: 17189
			PinchDown,
			// Token: 0x04004326 RID: 17190
			PinchStay,
			// Token: 0x04004327 RID: 17191
			PinchUp
		}
	}
}
