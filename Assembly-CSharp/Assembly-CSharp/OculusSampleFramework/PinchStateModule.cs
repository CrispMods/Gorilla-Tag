using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A62 RID: 2658
	public class PinchStateModule
	{
		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x0600422D RID: 16941 RVA: 0x00138A65 File Offset: 0x00136C65
		public bool PinchUpAndDownOnFocusedObject
		{
			get
			{
				return this._currPinchState == PinchStateModule.PinchState.PinchUp && this._firstFocusedInteractable != null;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600422E RID: 16942 RVA: 0x00138A7E File Offset: 0x00136C7E
		public bool PinchSteadyOnFocusedObject
		{
			get
			{
				return this._currPinchState == PinchStateModule.PinchState.PinchStay && this._firstFocusedInteractable != null;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x0600422F RID: 16943 RVA: 0x00138A97 File Offset: 0x00136C97
		public bool PinchDownOnFocusedObject
		{
			get
			{
				return this._currPinchState == PinchStateModule.PinchState.PinchDown && this._firstFocusedInteractable != null;
			}
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x00138AB0 File Offset: 0x00136CB0
		public PinchStateModule()
		{
			this._currPinchState = PinchStateModule.PinchState.None;
			this._firstFocusedInteractable = null;
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x00138AC8 File Offset: 0x00136CC8
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

		// Token: 0x04004332 RID: 17202
		private const float PINCH_STRENGTH_THRESHOLD = 1f;

		// Token: 0x04004333 RID: 17203
		private PinchStateModule.PinchState _currPinchState;

		// Token: 0x04004334 RID: 17204
		private Interactable _firstFocusedInteractable;

		// Token: 0x02000A63 RID: 2659
		private enum PinchState
		{
			// Token: 0x04004336 RID: 17206
			None,
			// Token: 0x04004337 RID: 17207
			PinchDown,
			// Token: 0x04004338 RID: 17208
			PinchStay,
			// Token: 0x04004339 RID: 17209
			PinchUp
		}
	}
}
