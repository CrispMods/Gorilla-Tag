using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C4D RID: 3149
	public class OnCollisionEventsCosmetic : MonoBehaviour
	{
		// Token: 0x06004E8D RID: 20109 RVA: 0x00062868 File Offset: 0x00060A68
		private bool IsMyItem()
		{
			return this._rig != null && this._rig.isOfflineVRRig;
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x00062885 File Offset: 0x00060A85
		private void Awake()
		{
			this._rig = base.GetComponentInParent<VRRig>();
			this.parentTransferable = base.GetComponentInParent<TransferrableObject>();
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x001B1F30 File Offset: 0x001B0130
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

		// Token: 0x06004E90 RID: 20112 RVA: 0x0006289F File Offset: 0x00060A9F
		private bool IsTagValid(GameObject obj, OnCollisionEventsCosmetic.Listener listener)
		{
			return listener.collisionTagsList.Contains(obj.tag);
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x000628B2 File Offset: 0x00060AB2
		private bool IsInCollisionLayer(GameObject obj, OnCollisionEventsCosmetic.Listener listener)
		{
			return (listener.collisionLayerMask.value & 1 << obj.layer) != 0;
		}

		// Token: 0x06004E92 RID: 20114 RVA: 0x001B1FD0 File Offset: 0x001B01D0
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

		// Token: 0x06004E93 RID: 20115 RVA: 0x001B200C File Offset: 0x001B020C
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

		// Token: 0x06004E94 RID: 20116 RVA: 0x001B2048 File Offset: 0x001B0248
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

		// Token: 0x0400521A RID: 21018
		public OnCollisionEventsCosmetic.Listener[] eventListeners = new OnCollisionEventsCosmetic.Listener[0];

		// Token: 0x0400521B RID: 21019
		private VRRig _rig;

		// Token: 0x0400521C RID: 21020
		private TransferrableObject parentTransferable;

		// Token: 0x02000C4E RID: 3150
		[Serializable]
		public class Listener
		{
			// Token: 0x0400521D RID: 21021
			public LayerMask collisionLayerMask;

			// Token: 0x0400521E RID: 21022
			public List<string> collisionTagsList = new List<string>();

			// Token: 0x0400521F RID: 21023
			public OnCollisionEventsCosmetic.EventType eventType;

			// Token: 0x04005220 RID: 21024
			public UnityEvent<bool, Collision> listenerComponent;

			// Token: 0x04005221 RID: 21025
			public bool syncForEveryoneInRoom = true;

			// Token: 0x04005222 RID: 21026
			[Tooltip("Fire these events only when the item is held in hand, only works if there is a transferable component somewhere on the object or its parent.")]
			public bool fireOnlyWhileHeld = true;
		}

		// Token: 0x02000C4F RID: 3151
		public enum EventType
		{
			// Token: 0x04005224 RID: 21028
			CollisionEnter,
			// Token: 0x04005225 RID: 21029
			CollisionStay,
			// Token: 0x04005226 RID: 21030
			CollisionExit
		}
	}
}
