using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C7E RID: 3198
	public class OnTriggerEventsCosmetic : MonoBehaviour
	{
		// Token: 0x06004FEB RID: 20459 RVA: 0x00064328 File Offset: 0x00062528
		private bool IsMyItem()
		{
			return this._rig != null && this._rig.isOfflineVRRig;
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x00064345 File Offset: 0x00062545
		private void Awake()
		{
			this._rig = base.GetComponentInParent<VRRig>();
			this.parentTransferable = base.GetComponentInParent<TransferrableObject>();
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x001BA168 File Offset: 0x001B8368
		private void FireEvents(Collider other, OnTriggerEventsCosmetic.Listener listener)
		{
			if (!listener.syncForEveryoneInRoom && !this.IsMyItem())
			{
				return;
			}
			if (this.parentTransferable && listener.fireOnlyWhileHeld && !this.parentTransferable.InHand())
			{
				return;
			}
			if (listener.triggerTagsList.Count > 0 && !this.IsTagValid(other.gameObject, listener))
			{
				return;
			}
			if ((1 << other.gameObject.layer & listener.triggerLayerMask) == 0)
			{
				return;
			}
			bool arg = this.parentTransferable && this.parentTransferable.InLeftHand();
			UnityEvent<bool, Collider> listenerComponent = listener.listenerComponent;
			if (listenerComponent == null)
			{
				return;
			}
			listenerComponent.Invoke(arg, other);
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x0006435F File Offset: 0x0006255F
		private bool IsTagValid(GameObject obj, OnTriggerEventsCosmetic.Listener listener)
		{
			return listener.triggerTagsList.Contains(obj.tag);
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x001BA214 File Offset: 0x001B8414
		private void OnTriggerEnter(Collider other)
		{
			for (int i = 0; i < this.eventListeners.Length; i++)
			{
				OnTriggerEventsCosmetic.Listener listener = this.eventListeners[i];
				if (listener.eventType == OnTriggerEventsCosmetic.EventType.TriggerEnter)
				{
					this.FireEvents(other, listener);
				}
			}
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x001BA250 File Offset: 0x001B8450
		private void OnTriggerExit(Collider other)
		{
			for (int i = 0; i < this.eventListeners.Length; i++)
			{
				OnTriggerEventsCosmetic.Listener listener = this.eventListeners[i];
				if (listener.eventType == OnTriggerEventsCosmetic.EventType.TriggerExit)
				{
					this.FireEvents(other, listener);
				}
			}
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x001BA28C File Offset: 0x001B848C
		private void OnTriggerStay(Collider other)
		{
			for (int i = 0; i < this.eventListeners.Length; i++)
			{
				OnTriggerEventsCosmetic.Listener listener = this.eventListeners[i];
				if (listener.eventType == OnTriggerEventsCosmetic.EventType.TriggerStay)
				{
					this.FireEvents(other, listener);
				}
			}
		}

		// Token: 0x04005321 RID: 21281
		public OnTriggerEventsCosmetic.Listener[] eventListeners = new OnTriggerEventsCosmetic.Listener[0];

		// Token: 0x04005322 RID: 21282
		private VRRig _rig;

		// Token: 0x04005323 RID: 21283
		private TransferrableObject parentTransferable;

		// Token: 0x02000C7F RID: 3199
		[Serializable]
		public class Listener
		{
			// Token: 0x04005324 RID: 21284
			public LayerMask triggerLayerMask;

			// Token: 0x04005325 RID: 21285
			public List<string> triggerTagsList = new List<string>();

			// Token: 0x04005326 RID: 21286
			public OnTriggerEventsCosmetic.EventType eventType;

			// Token: 0x04005327 RID: 21287
			public UnityEvent<bool, Collider> listenerComponent;

			// Token: 0x04005328 RID: 21288
			public bool syncForEveryoneInRoom = true;

			// Token: 0x04005329 RID: 21289
			[Tooltip("Fire these events only when the item is held in hand, only works if there is a transferable component somewhere on the object or its parent.")]
			public bool fireOnlyWhileHeld = true;
		}

		// Token: 0x02000C80 RID: 3200
		public enum EventType
		{
			// Token: 0x0400532B RID: 21291
			TriggerEnter,
			// Token: 0x0400532C RID: 21292
			TriggerStay,
			// Token: 0x0400532D RID: 21293
			TriggerExit
		}
	}
}
