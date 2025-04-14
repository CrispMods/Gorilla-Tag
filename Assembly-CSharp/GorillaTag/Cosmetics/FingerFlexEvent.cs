using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C40 RID: 3136
	public class FingerFlexEvent : MonoBehaviour
	{
		// Token: 0x06004E4A RID: 20042 RVA: 0x00181007 File Offset: 0x0017F207
		private void Awake()
		{
			this._rig = base.GetComponentInParent<VRRig>();
			this.parentTransferable = base.GetComponentInParent<TransferrableObject>();
		}

		// Token: 0x06004E4B RID: 20043 RVA: 0x00181021 File Offset: 0x0017F221
		private bool IsMyItem()
		{
			return this._rig != null && this._rig.isOfflineVRRig;
		}

		// Token: 0x06004E4C RID: 20044 RVA: 0x00181040 File Offset: 0x0017F240
		private void Update()
		{
			for (int i = 0; i < this.eventListeners.Length; i++)
			{
				FingerFlexEvent.Listener listener = this.eventListeners[i];
				this.FireEvents(listener);
			}
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x00181070 File Offset: 0x0017F270
		private void FireEvents(FingerFlexEvent.Listener listener)
		{
			if (!listener.syncForEveryoneInRoom && !this.IsMyItem())
			{
				return;
			}
			if (this.parentTransferable && listener.fireOnlyWhileHeld && !this.parentTransferable.InHand())
			{
				return;
			}
			switch (this.fingerType)
			{
			case FingerFlexEvent.FingerType.Thumb:
			{
				float calcT = this._rig.leftThumb.calcT;
				float calcT2 = this._rig.rightThumb.calcT;
				this.FireEvents(listener, calcT, calcT2);
				return;
			}
			case FingerFlexEvent.FingerType.Index:
			{
				float calcT3 = this._rig.leftIndex.calcT;
				float calcT4 = this._rig.rightIndex.calcT;
				this.FireEvents(listener, calcT3, calcT4);
				return;
			}
			case FingerFlexEvent.FingerType.Middle:
			{
				float calcT5 = this._rig.leftMiddle.calcT;
				float calcT6 = this._rig.rightMiddle.calcT;
				this.FireEvents(listener, calcT5, calcT6);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x00181154 File Offset: 0x0017F354
		private void FireEvents(FingerFlexEvent.Listener listener, float leftFinger, float rightFinger)
		{
			if (this.parentTransferable && this.FingerFlexValidation(true))
			{
				this.CheckFingerValue(listener, leftFinger, true, ref listener.fingerLeftLastValue);
				return;
			}
			if (this.parentTransferable && this.FingerFlexValidation(false))
			{
				this.CheckFingerValue(listener, rightFinger, false, ref listener.fingerRightLastValue);
				return;
			}
			this.CheckFingerValue(listener, leftFinger, true, ref listener.fingerLeftLastValue);
			this.CheckFingerValue(listener, rightFinger, false, ref listener.fingerRightLastValue);
		}

		// Token: 0x06004E4F RID: 20047 RVA: 0x001811CC File Offset: 0x0017F3CC
		private void CheckFingerValue(FingerFlexEvent.Listener listener, float fingerValue, bool isLeft, ref float lastValue)
		{
			if (fingerValue > listener.fingerFlexValue)
			{
				listener.frameCounter++;
			}
			switch (listener.eventType)
			{
			case FingerFlexEvent.EventType.OnFingerFlexed:
				if (fingerValue > listener.fingerFlexValue && lastValue < listener.fingerFlexValue)
				{
					UnityEvent<bool, float> listenerComponent = listener.listenerComponent;
					if (listenerComponent != null)
					{
						listenerComponent.Invoke(isLeft, fingerValue);
					}
				}
				break;
			case FingerFlexEvent.EventType.OnFingerReleased:
				if (fingerValue <= listener.fingerReleaseValue && lastValue > listener.fingerReleaseValue)
				{
					UnityEvent<bool, float> listenerComponent2 = listener.listenerComponent;
					if (listenerComponent2 != null)
					{
						listenerComponent2.Invoke(isLeft, fingerValue);
					}
					listener.frameCounter = 0;
				}
				break;
			case FingerFlexEvent.EventType.OnFingerFlexStayed:
				if (fingerValue > listener.fingerFlexValue && lastValue >= listener.fingerFlexValue && listener.frameCounter % listener.frameInterval == 0)
				{
					UnityEvent<bool, float> listenerComponent3 = listener.listenerComponent;
					if (listenerComponent3 != null)
					{
						listenerComponent3.Invoke(isLeft, fingerValue);
					}
					listener.frameCounter = 0;
				}
				break;
			}
			lastValue = fingerValue;
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x001812AE File Offset: 0x0017F4AE
		private bool FingerFlexValidation(bool isLeftHand)
		{
			return (!this.parentTransferable.InLeftHand() || isLeftHand) && (this.parentTransferable.InLeftHand() || !isLeftHand);
		}

		// Token: 0x040051CD RID: 20941
		[SerializeField]
		private FingerFlexEvent.FingerType fingerType = FingerFlexEvent.FingerType.Index;

		// Token: 0x040051CE RID: 20942
		public FingerFlexEvent.Listener[] eventListeners = new FingerFlexEvent.Listener[0];

		// Token: 0x040051CF RID: 20943
		private VRRig _rig;

		// Token: 0x040051D0 RID: 20944
		private TransferrableObject parentTransferable;

		// Token: 0x02000C41 RID: 3137
		[Serializable]
		public class Listener
		{
			// Token: 0x040051D1 RID: 20945
			public FingerFlexEvent.EventType eventType;

			// Token: 0x040051D2 RID: 20946
			public UnityEvent<bool, float> listenerComponent;

			// Token: 0x040051D3 RID: 20947
			public float fingerFlexValue = 0.75f;

			// Token: 0x040051D4 RID: 20948
			public float fingerReleaseValue = 0.01f;

			// Token: 0x040051D5 RID: 20949
			[Tooltip("How many frames should pass to fire a finger flex stayed event")]
			public int frameInterval = 20;

			// Token: 0x040051D6 RID: 20950
			[Tooltip("This event will be fired for everyone in the room (synced) by default unless you uncheck this box so that it will be fired only for the local player.")]
			public bool syncForEveryoneInRoom = true;

			// Token: 0x040051D7 RID: 20951
			[Tooltip("Fire these events only when the item is held in hand, only works if there is a transferable component somewhere on the object or its parent.")]
			public bool fireOnlyWhileHeld = true;

			// Token: 0x040051D8 RID: 20952
			internal int frameCounter;

			// Token: 0x040051D9 RID: 20953
			internal float fingerRightLastValue;

			// Token: 0x040051DA RID: 20954
			internal float fingerLeftLastValue;
		}

		// Token: 0x02000C42 RID: 3138
		public enum EventType
		{
			// Token: 0x040051DC RID: 20956
			OnFingerFlexed,
			// Token: 0x040051DD RID: 20957
			OnFingerReleased,
			// Token: 0x040051DE RID: 20958
			OnFingerFlexStayed
		}

		// Token: 0x02000C43 RID: 3139
		private enum FingerType
		{
			// Token: 0x040051E0 RID: 20960
			Thumb,
			// Token: 0x040051E1 RID: 20961
			Index,
			// Token: 0x040051E2 RID: 20962
			Middle
		}
	}
}
