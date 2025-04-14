using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A5D RID: 2653
	public abstract class InteractableTool : MonoBehaviour
	{
		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06004205 RID: 16901 RVA: 0x00042E29 File Offset: 0x00041029
		public Transform ToolTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06004206 RID: 16902 RVA: 0x0013811E File Offset: 0x0013631E
		// (set) Token: 0x06004207 RID: 16903 RVA: 0x00138126 File Offset: 0x00136326
		public bool IsRightHandedTool { get; set; }

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06004208 RID: 16904
		public abstract InteractableToolTags ToolTags { get; }

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06004209 RID: 16905
		public abstract ToolInputState ToolInputState { get; }

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x0600420A RID: 16906
		public abstract bool IsFarFieldTool { get; }

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600420B RID: 16907 RVA: 0x0013812F File Offset: 0x0013632F
		// (set) Token: 0x0600420C RID: 16908 RVA: 0x00138137 File Offset: 0x00136337
		public Vector3 Velocity { get; protected set; }

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x0600420D RID: 16909 RVA: 0x00138140 File Offset: 0x00136340
		// (set) Token: 0x0600420E RID: 16910 RVA: 0x00138148 File Offset: 0x00136348
		public Vector3 InteractionPosition { get; protected set; }

		// Token: 0x0600420F RID: 16911 RVA: 0x00138151 File Offset: 0x00136351
		public List<InteractableCollisionInfo> GetCurrentIntersectingObjects()
		{
			return this._currentIntersectingObjects;
		}

		// Token: 0x06004210 RID: 16912
		public abstract List<InteractableCollisionInfo> GetNextIntersectingObjects();

		// Token: 0x06004211 RID: 16913
		public abstract void FocusOnInteractable(Interactable focusedInteractable, ColliderZone colliderZone);

		// Token: 0x06004212 RID: 16914
		public abstract void DeFocus();

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06004213 RID: 16915
		// (set) Token: 0x06004214 RID: 16916
		public abstract bool EnableState { get; set; }

		// Token: 0x06004215 RID: 16917
		public abstract void Initialize();

		// Token: 0x06004216 RID: 16918 RVA: 0x00138159 File Offset: 0x00136359
		public KeyValuePair<Interactable, InteractableCollisionInfo> GetFirstCurrentCollisionInfo()
		{
			return this._currInteractableToCollisionInfos.First<KeyValuePair<Interactable, InteractableCollisionInfo>>();
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x00138166 File Offset: 0x00136366
		public void ClearAllCurrentCollisionInfos()
		{
			this._currInteractableToCollisionInfos.Clear();
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x00138174 File Offset: 0x00136374
		public virtual void UpdateCurrentCollisionsBasedOnDepth()
		{
			this._currInteractableToCollisionInfos.Clear();
			foreach (InteractableCollisionInfo interactableCollisionInfo in this._currentIntersectingObjects)
			{
				Interactable parentInteractable = interactableCollisionInfo.InteractableCollider.ParentInteractable;
				InteractableCollisionDepth collisionDepth = interactableCollisionInfo.CollisionDepth;
				InteractableCollisionInfo interactableCollisionInfo2 = null;
				if (!this._currInteractableToCollisionInfos.TryGetValue(parentInteractable, out interactableCollisionInfo2))
				{
					this._currInteractableToCollisionInfos[parentInteractable] = interactableCollisionInfo;
				}
				else if (interactableCollisionInfo2.CollisionDepth < collisionDepth)
				{
					interactableCollisionInfo2.InteractableCollider = interactableCollisionInfo.InteractableCollider;
					interactableCollisionInfo2.CollisionDepth = collisionDepth;
				}
			}
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x00138220 File Offset: 0x00136420
		public virtual void UpdateLatestCollisionData()
		{
			this._addedInteractables.Clear();
			this._removedInteractables.Clear();
			this._remainingInteractables.Clear();
			foreach (Interactable interactable in this._currInteractableToCollisionInfos.Keys)
			{
				if (!this._prevInteractableToCollisionInfos.ContainsKey(interactable))
				{
					this._addedInteractables.Add(interactable);
				}
				else
				{
					this._remainingInteractables.Add(interactable);
				}
			}
			foreach (Interactable interactable2 in this._prevInteractableToCollisionInfos.Keys)
			{
				if (!this._currInteractableToCollisionInfos.ContainsKey(interactable2))
				{
					this._removedInteractables.Add(interactable2);
				}
			}
			foreach (Interactable interactable3 in this._removedInteractables)
			{
				interactable3.UpdateCollisionDepth(this, this._prevInteractableToCollisionInfos[interactable3].CollisionDepth, InteractableCollisionDepth.None);
			}
			foreach (Interactable interactable4 in this._addedInteractables)
			{
				InteractableCollisionDepth collisionDepth = this._currInteractableToCollisionInfos[interactable4].CollisionDepth;
				interactable4.UpdateCollisionDepth(this, InteractableCollisionDepth.None, collisionDepth);
			}
			foreach (Interactable interactable5 in this._remainingInteractables)
			{
				InteractableCollisionDepth collisionDepth2 = this._currInteractableToCollisionInfos[interactable5].CollisionDepth;
				InteractableCollisionDepth collisionDepth3 = this._prevInteractableToCollisionInfos[interactable5].CollisionDepth;
				interactable5.UpdateCollisionDepth(this, collisionDepth3, collisionDepth2);
			}
			this._prevInteractableToCollisionInfos = new Dictionary<Interactable, InteractableCollisionInfo>(this._currInteractableToCollisionInfos);
		}

		// Token: 0x0400431A RID: 17178
		protected List<InteractableCollisionInfo> _currentIntersectingObjects = new List<InteractableCollisionInfo>();

		// Token: 0x0400431B RID: 17179
		private List<Interactable> _addedInteractables = new List<Interactable>();

		// Token: 0x0400431C RID: 17180
		private List<Interactable> _removedInteractables = new List<Interactable>();

		// Token: 0x0400431D RID: 17181
		private List<Interactable> _remainingInteractables = new List<Interactable>();

		// Token: 0x0400431E RID: 17182
		private Dictionary<Interactable, InteractableCollisionInfo> _currInteractableToCollisionInfos = new Dictionary<Interactable, InteractableCollisionInfo>();

		// Token: 0x0400431F RID: 17183
		private Dictionary<Interactable, InteractableCollisionInfo> _prevInteractableToCollisionInfos = new Dictionary<Interactable, InteractableCollisionInfo>();
	}
}
