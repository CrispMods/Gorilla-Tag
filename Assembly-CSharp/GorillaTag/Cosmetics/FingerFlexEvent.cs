using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C71 RID: 3185
	public class FingerFlexEvent : MonoBehaviour
	{
		// Token: 0x06004FAA RID: 20394 RVA: 0x0006400B File Offset: 0x0006220B
		private void Awake()
		{
			this._rig = base.GetComponentInParent<VRRig>();
			this.parentTransferable = base.GetComponentInParent<TransferrableObject>();
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x00064025 File Offset: 0x00062225
		private bool IsMyItem()
		{
			return this._rig != null && this._rig.isOfflineVRRig;
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x001B972C File Offset: 0x001B792C
		private void Update()
		{
			for (int i = 0; i < this.eventListeners.Length; i++)
			{
				FingerFlexEvent.Listener listener = this.eventListeners[i];
				this.FireEvents(listener);
			}
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x001B975C File Offset: 0x001B795C
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

		// Token: 0x06004FAE RID: 20398 RVA: 0x001B9840 File Offset: 0x001B7A40
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

		// Token: 0x06004FAF RID: 20399 RVA: 0x001B98B8 File Offset: 0x001B7AB8
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

		// Token: 0x06004FB0 RID: 20400 RVA: 0x00064042 File Offset: 0x00062242
		private bool FingerFlexValidation(bool isLeftHand)
		{
			return (!this.parentTransferable.InLeftHand() || isLeftHand) && (this.parentTransferable.InLeftHand() || !isLeftHand);
		}

		// Token: 0x040052D9 RID: 21209
		[SerializeField]
		private FingerFlexEvent.FingerType fingerType = FingerFlexEvent.FingerType.Index;

		// Token: 0x040052DA RID: 21210
		public FingerFlexEvent.Listener[] eventListeners = new FingerFlexEvent.Listener[0];

		// Token: 0x040052DB RID: 21211
		private VRRig _rig;

		// Token: 0x040052DC RID: 21212
		private TransferrableObject parentTransferable;

		// Token: 0x02000C72 RID: 3186
		[Serializable]
		public class Listener
		{
			// Token: 0x040052DD RID: 21213
			public FingerFlexEvent.EventType eventType;

			// Token: 0x040052DE RID: 21214
			public UnityEvent<bool, float> listenerComponent;

			// Token: 0x040052DF RID: 21215
			public float fingerFlexValue = 0.75f;

			// Token: 0x040052E0 RID: 21216
			public float fingerReleaseValue = 0.01f;

			// Token: 0x040052E1 RID: 21217
			[Tooltip("How many frames should pass to fire a finger flex stayed event")]
			public int frameInterval = 20;

			// Token: 0x040052E2 RID: 21218
			[Tooltip("This event will be fired for everyone in the room (synced) by default unless you uncheck this box so that it will be fired only for the local player.")]
			public bool syncForEveryoneInRoom = true;

			// Token: 0x040052E3 RID: 21219
			[Tooltip("Fire these events only when the item is held in hand, only works if there is a transferable component somewhere on the object or its parent.")]
			public bool fireOnlyWhileHeld = true;

			// Token: 0x040052E4 RID: 21220
			internal int frameCounter;

			// Token: 0x040052E5 RID: 21221
			internal float fingerRightLastValue;

			// Token: 0x040052E6 RID: 21222
			internal float fingerLeftLastValue;
		}

		// Token: 0x02000C73 RID: 3187
		public enum EventType
		{
			// Token: 0x040052E8 RID: 21224
			OnFingerFlexed,
			// Token: 0x040052E9 RID: 21225
			OnFingerReleased,
			// Token: 0x040052EA RID: 21226
			OnFingerFlexStayed
		}

		// Token: 0x02000C74 RID: 3188
		private enum FingerType
		{
			// Token: 0x040052EC RID: 21228
			Thumb,
			// Token: 0x040052ED RID: 21229
			Index,
			// Token: 0x040052EE RID: 21230
			Middle
		}
	}
}
