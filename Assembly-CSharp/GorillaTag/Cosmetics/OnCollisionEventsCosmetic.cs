using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C4A RID: 3146
	public class OnCollisionEventsCosmetic : MonoBehaviour
	{
		// Token: 0x06004E81 RID: 20097 RVA: 0x00181B6F File Offset: 0x0017FD6F
		private bool IsMyItem()
		{
			return this._rig != null && this._rig.isOfflineVRRig;
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x00181B8C File Offset: 0x0017FD8C
		private void Awake()
		{
			this._rig = base.GetComponentInParent<VRRig>();
			this.parentTransferable = base.GetComponentInParent<TransferrableObject>();
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x00181BA8 File Offset: 0x0017FDA8
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

		// Token: 0x06004E84 RID: 20100 RVA: 0x00181C45 File Offset: 0x0017FE45
		private bool IsTagValid(GameObject obj, OnCollisionEventsCosmetic.Listener listener)
		{
			return listener.collisionTagsList.Contains(obj.tag);
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x00181C58 File Offset: 0x0017FE58
		private bool IsInCollisionLayer(GameObject obj, OnCollisionEventsCosmetic.Listener listener)
		{
			return (listener.collisionLayerMask.value & 1 << obj.layer) != 0;
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x00181C74 File Offset: 0x0017FE74
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

		// Token: 0x06004E87 RID: 20103 RVA: 0x00181CB0 File Offset: 0x0017FEB0
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

		// Token: 0x06004E88 RID: 20104 RVA: 0x00181CEC File Offset: 0x0017FEEC
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

		// Token: 0x04005208 RID: 21000
		public OnCollisionEventsCosmetic.Listener[] eventListeners = new OnCollisionEventsCosmetic.Listener[0];

		// Token: 0x04005209 RID: 21001
		private VRRig _rig;

		// Token: 0x0400520A RID: 21002
		private TransferrableObject parentTransferable;

		// Token: 0x02000C4B RID: 3147
		[Serializable]
		public class Listener
		{
			// Token: 0x0400520B RID: 21003
			public LayerMask collisionLayerMask;

			// Token: 0x0400520C RID: 21004
			public List<string> collisionTagsList = new List<string>();

			// Token: 0x0400520D RID: 21005
			public OnCollisionEventsCosmetic.EventType eventType;

			// Token: 0x0400520E RID: 21006
			public UnityEvent<bool, Collision> listenerComponent;

			// Token: 0x0400520F RID: 21007
			public bool syncForEveryoneInRoom = true;

			// Token: 0x04005210 RID: 21008
			[Tooltip("Fire these events only when the item is held in hand, only works if there is a transferable component somewhere on the object or its parent.")]
			public bool fireOnlyWhileHeld = true;
		}

		// Token: 0x02000C4C RID: 3148
		public enum EventType
		{
			// Token: 0x04005212 RID: 21010
			CollisionEnter,
			// Token: 0x04005213 RID: 21011
			CollisionStay,
			// Token: 0x04005214 RID: 21012
			CollisionExit
		}
	}
}
