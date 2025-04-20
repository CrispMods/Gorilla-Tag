using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C7B RID: 3195
	public class OnCollisionEventsCosmetic : MonoBehaviour
	{
		// Token: 0x06004FE1 RID: 20449 RVA: 0x0006428D File Offset: 0x0006248D
		private bool IsMyItem()
		{
			return this._rig != null && this._rig.isOfflineVRRig;
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x000642AA File Offset: 0x000624AA
		private void Awake()
		{
			this._rig = base.GetComponentInParent<VRRig>();
			this.parentTransferable = base.GetComponentInParent<TransferrableObject>();
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x001BA014 File Offset: 0x001B8214
		private void FireEvents(Collision other, OnCollisionEventsCosmetic.Listener listener)
		{
			if (!listener.syncForEveryoneInRoom && !this.IsMyItem())
			{
				return;
			}
			if (this.parentTransferable && listener.fireOnlyWhileHeld && !this.parentTransferable.InHand())
			{
				return;
			}
			if (listener.collisionTagsList.Count > 0 && !this.IsTagValid(other.gameObject, listener))
			{
				return;
			}
			if (!this.IsInCollisionLayer(other.gameObject, listener))
			{
				return;
			}
			bool arg = this.parentTransferable && this.parentTransferable.InLeftHand();
			UnityEvent<bool, Collision> listenerComponent = listener.listenerComponent;
			if (listenerComponent == null)
			{
				return;
			}
			listenerComponent.Invoke(arg, other);
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x000642C4 File Offset: 0x000624C4
		private bool IsTagValid(GameObject obj, OnCollisionEventsCosmetic.Listener listener)
		{
			return listener.collisionTagsList.Contains(obj.tag);
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x000642D7 File Offset: 0x000624D7
		private bool IsInCollisionLayer(GameObject obj, OnCollisionEventsCosmetic.Listener listener)
		{
			return (listener.collisionLayerMask.value & 1 << obj.layer) != 0;
		}

		// Token: 0x06004FE6 RID: 20454 RVA: 0x001BA0B4 File Offset: 0x001B82B4
		private void OnCollisionEnter(Collision other)
		{
			for (int i = 0; i < this.eventListeners.Length; i++)
			{
				OnCollisionEventsCosmetic.Listener listener = this.eventListeners[i];
				if (listener.eventType == OnCollisionEventsCosmetic.EventType.CollisionEnter)
				{
					this.FireEvents(other, listener);
				}
			}
		}

		// Token: 0x06004FE7 RID: 20455 RVA: 0x001BA0F0 File Offset: 0x001B82F0
		private void OnCollisionExit(Collision other)
		{
			for (int i = 0; i < this.eventListeners.Length; i++)
			{
				OnCollisionEventsCosmetic.Listener listener = this.eventListeners[i];
				if (listener.eventType == OnCollisionEventsCosmetic.EventType.CollisionExit)
				{
					this.FireEvents(other, listener);
				}
			}
		}

		// Token: 0x06004FE8 RID: 20456 RVA: 0x001BA12C File Offset: 0x001B832C
		private void OnCollisionStay(Collision other)
		{
			for (int i = 0; i < this.eventListeners.Length; i++)
			{
				OnCollisionEventsCosmetic.Listener listener = this.eventListeners[i];
				if (listener.eventType == OnCollisionEventsCosmetic.EventType.CollisionStay)
				{
					this.FireEvents(other, listener);
				}
			}
		}

		// Token: 0x04005314 RID: 21268
		public OnCollisionEventsCosmetic.Listener[] eventListeners = new OnCollisionEventsCosmetic.Listener[0];

		// Token: 0x04005315 RID: 21269
		private VRRig _rig;

		// Token: 0x04005316 RID: 21270
		private TransferrableObject parentTransferable;

		// Token: 0x02000C7C RID: 3196
		[Serializable]
		public class Listener
		{
			// Token: 0x04005317 RID: 21271
			public LayerMask collisionLayerMask;

			// Token: 0x04005318 RID: 21272
			public List<string> collisionTagsList = new List<string>();

			// Token: 0x04005319 RID: 21273
			public OnCollisionEventsCosmetic.EventType eventType;

			// Token: 0x0400531A RID: 21274
			public UnityEvent<bool, Collision> listenerComponent;

			// Token: 0x0400531B RID: 21275
			public bool syncForEveryoneInRoom = true;

			// Token: 0x0400531C RID: 21276
			[Tooltip("Fire these events only when the item is held in hand, only works if there is a transferable component somewhere on the object or its parent.")]
			public bool fireOnlyWhileHeld = true;
		}

		// Token: 0x02000C7D RID: 3197
		public enum EventType
		{
			// Token: 0x0400531E RID: 21278
			CollisionEnter,
			// Token: 0x0400531F RID: 21279
			CollisionStay,
			// Token: 0x04005320 RID: 21280
			CollisionExit
		}
	}
}
