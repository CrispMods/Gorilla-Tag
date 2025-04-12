using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C50 RID: 3152
	public class OnTriggerEventsCosmetic : MonoBehaviour
	{
		// Token: 0x06004E97 RID: 20119 RVA: 0x00062903 File Offset: 0x00060B03
		private bool IsMyItem()
		{
			return this._rig != null && this._rig.isOfflineVRRig;
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x00062920 File Offset: 0x00060B20
		private void Awake()
		{
			this._rig = base.GetComponentInParent<VRRig>();
			this.parentTransferable = base.GetComponentInParent<TransferrableObject>();
		}

		// Token: 0x06004E99 RID: 20121 RVA: 0x001B2084 File Offset: 0x001B0284
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

		// Token: 0x06004E9A RID: 20122 RVA: 0x0006293A File Offset: 0x00060B3A
		private bool IsTagValid(GameObject obj, OnTriggerEventsCosmetic.Listener listener)
		{
			return listener.triggerTagsList.Contains(obj.tag);
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x001B2130 File Offset: 0x001B0330
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

		// Token: 0x06004E9C RID: 20124 RVA: 0x001B216C File Offset: 0x001B036C
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

		// Token: 0x06004E9D RID: 20125 RVA: 0x001B21A8 File Offset: 0x001B03A8
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

		// Token: 0x04005227 RID: 21031
		public OnTriggerEventsCosmetic.Listener[] eventListeners = new OnTriggerEventsCosmetic.Listener[0];

		// Token: 0x04005228 RID: 21032
		private VRRig _rig;

		// Token: 0x04005229 RID: 21033
		private TransferrableObject parentTransferable;

		// Token: 0x02000C51 RID: 3153
		[Serializable]
		public class Listener
		{
			// Token: 0x0400522A RID: 21034
			public LayerMask triggerLayerMask;

			// Token: 0x0400522B RID: 21035
			public List<string> triggerTagsList = new List<string>();

			// Token: 0x0400522C RID: 21036
			public OnTriggerEventsCosmetic.EventType eventType;

			// Token: 0x0400522D RID: 21037
			public UnityEvent<bool, Collider> listenerComponent;

			// Token: 0x0400522E RID: 21038
			public bool syncForEveryoneInRoom = true;

			// Token: 0x0400522F RID: 21039
			[Tooltip("Fire these events only when the item is held in hand, only works if there is a transferable component somewhere on the object or its parent.")]
			public bool fireOnlyWhileHeld = true;
		}

		// Token: 0x02000C52 RID: 3154
		public enum EventType
		{
			// Token: 0x04005231 RID: 21041
			TriggerEnter,
			// Token: 0x04005232 RID: 21042
			TriggerStay,
			// Token: 0x04005233 RID: 21043
			TriggerExit
		}
	}
}
