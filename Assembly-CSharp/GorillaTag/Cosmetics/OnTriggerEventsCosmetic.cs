using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C4D RID: 3149
	public class OnTriggerEventsCosmetic : MonoBehaviour
	{
		// Token: 0x06004E8B RID: 20107 RVA: 0x00181D5B File Offset: 0x0017FF5B
		private bool IsMyItem()
		{
			return this._rig != null && this._rig.isOfflineVRRig;
		}

		// Token: 0x06004E8C RID: 20108 RVA: 0x00181D78 File Offset: 0x0017FF78
		private void Awake()
		{
			this._rig = base.GetComponentInParent<VRRig>();
			this.parentTransferable = base.GetComponentInParent<TransferrableObject>();
		}

		// Token: 0x06004E8D RID: 20109 RVA: 0x00181D94 File Offset: 0x0017FF94
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

		// Token: 0x06004E8E RID: 20110 RVA: 0x00181E40 File Offset: 0x00180040
		private bool IsTagValid(GameObject obj, OnTriggerEventsCosmetic.Listener listener)
		{
			return listener.triggerTagsList.Contains(obj.tag);
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x00181E54 File Offset: 0x00180054
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

		// Token: 0x06004E90 RID: 20112 RVA: 0x00181E90 File Offset: 0x00180090
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

		// Token: 0x06004E91 RID: 20113 RVA: 0x00181ECC File Offset: 0x001800CC
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

		// Token: 0x04005215 RID: 21013
		public OnTriggerEventsCosmetic.Listener[] eventListeners = new OnTriggerEventsCosmetic.Listener[0];

		// Token: 0x04005216 RID: 21014
		private VRRig _rig;

		// Token: 0x04005217 RID: 21015
		private TransferrableObject parentTransferable;

		// Token: 0x02000C4E RID: 3150
		[Serializable]
		public class Listener
		{
			// Token: 0x04005218 RID: 21016
			public LayerMask triggerLayerMask;

			// Token: 0x04005219 RID: 21017
			public List<string> triggerTagsList = new List<string>();

			// Token: 0x0400521A RID: 21018
			public OnTriggerEventsCosmetic.EventType eventType;

			// Token: 0x0400521B RID: 21019
			public UnityEvent<bool, Collider> listenerComponent;

			// Token: 0x0400521C RID: 21020
			public bool syncForEveryoneInRoom = true;

			// Token: 0x0400521D RID: 21021
			[Tooltip("Fire these events only when the item is held in hand, only works if there is a transferable component somewhere on the object or its parent.")]
			public bool fireOnlyWhileHeld = true;
		}

		// Token: 0x02000C4F RID: 3151
		public enum EventType
		{
			// Token: 0x0400521F RID: 21023
			TriggerEnter,
			// Token: 0x04005220 RID: 21024
			TriggerStay,
			// Token: 0x04005221 RID: 21025
			TriggerExit
		}
	}
}
