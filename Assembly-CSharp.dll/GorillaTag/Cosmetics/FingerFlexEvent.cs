using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C43 RID: 3139
	public class FingerFlexEvent : MonoBehaviour
	{
		// Token: 0x06004E56 RID: 20054 RVA: 0x000625E6 File Offset: 0x000607E6
		private void Awake()
		{
			this._rig = base.GetComponentInParent<VRRig>();
			this.parentTransferable = base.GetComponentInParent<TransferrableObject>();
		}

		// Token: 0x06004E57 RID: 20055 RVA: 0x00062600 File Offset: 0x00060800
		private bool IsMyItem()
		{
			return this._rig != null && this._rig.isOfflineVRRig;
		}

		// Token: 0x06004E58 RID: 20056 RVA: 0x001B1648 File Offset: 0x001AF848
		private void Update()
		{
			for (int i = 0; i < this.eventListeners.Length; i++)
			{
				FingerFlexEvent.Listener listener = this.eventListeners[i];
				this.FireEvents(listener);
			}
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x001B1678 File Offset: 0x001AF878
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

		// Token: 0x06004E5A RID: 20058 RVA: 0x001B175C File Offset: 0x001AF95C
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

		// Token: 0x06004E5B RID: 20059 RVA: 0x001B17D4 File Offset: 0x001AF9D4
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

		// Token: 0x06004E5C RID: 20060 RVA: 0x0006261D File Offset: 0x0006081D
		private bool FingerFlexValidation(bool isLeftHand)
		{
			return (!this.parentTransferable.InLeftHand() || isLeftHand) && (this.parentTransferable.InLeftHand() || !isLeftHand);
		}

		// Token: 0x040051DF RID: 20959
		[SerializeField]
		private FingerFlexEvent.FingerType fingerType = FingerFlexEvent.FingerType.Index;

		// Token: 0x040051E0 RID: 20960
		public FingerFlexEvent.Listener[] eventListeners = new FingerFlexEvent.Listener[0];

		// Token: 0x040051E1 RID: 20961
		private VRRig _rig;

		// Token: 0x040051E2 RID: 20962
		private TransferrableObject parentTransferable;

		// Token: 0x02000C44 RID: 3140
		[Serializable]
		public class Listener
		{
			// Token: 0x040051E3 RID: 20963
			public FingerFlexEvent.EventType eventType;

			// Token: 0x040051E4 RID: 20964
			public UnityEvent<bool, float> listenerComponent;

			// Token: 0x040051E5 RID: 20965
			public float fingerFlexValue = 0.75f;

			// Token: 0x040051E6 RID: 20966
			public float fingerReleaseValue = 0.01f;

			// Token: 0x040051E7 RID: 20967
			[Tooltip("How many frames should pass to fire a finger flex stayed event")]
			public int frameInterval = 20;

			// Token: 0x040051E8 RID: 20968
			[Tooltip("This event will be fired for everyone in the room (synced) by default unless you uncheck this box so that it will be fired only for the local player.")]
			public bool syncForEveryoneInRoom = true;

			// Token: 0x040051E9 RID: 20969
			[Tooltip("Fire these events only when the item is held in hand, only works if there is a transferable component somewhere on the object or its parent.")]
			public bool fireOnlyWhileHeld = true;

			// Token: 0x040051EA RID: 20970
			internal int frameCounter;

			// Token: 0x040051EB RID: 20971
			internal float fingerRightLastValue;

			// Token: 0x040051EC RID: 20972
			internal float fingerLeftLastValue;
		}

		// Token: 0x02000C45 RID: 3141
		public enum EventType
		{
			// Token: 0x040051EE RID: 20974
			OnFingerFlexed,
			// Token: 0x040051EF RID: 20975
			OnFingerReleased,
			// Token: 0x040051F0 RID: 20976
			OnFingerFlexStayed
		}

		// Token: 0x02000C46 RID: 3142
		private enum FingerType
		{
			// Token: 0x040051F2 RID: 20978
			Thumb,
			// Token: 0x040051F3 RID: 20979
			Index,
			// Token: 0x040051F4 RID: 20980
			Middle
		}
	}
}
