using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A60 RID: 2656
	public abstract class InteractableTool : MonoBehaviour
	{
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06004211 RID: 16913 RVA: 0x00037F83 File Offset: 0x00036183
		public Transform ToolTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x0005A425 File Offset: 0x00058625
		// (set) Token: 0x06004213 RID: 16915 RVA: 0x0005A42D File Offset: 0x0005862D
		public bool IsRightHandedTool { get; set; }

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06004214 RID: 16916
		public abstract InteractableToolTags ToolTags { get; }

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06004215 RID: 16917
		public abstract ToolInputState ToolInputState { get; }

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06004216 RID: 16918
		public abstract bool IsFarFieldTool { get; }

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06004217 RID: 16919 RVA: 0x0005A436 File Offset: 0x00058636
		// (set) Token: 0x06004218 RID: 16920 RVA: 0x0005A43E File Offset: 0x0005863E
		public Vector3 Velocity { get; protected set; }

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06004219 RID: 16921 RVA: 0x0005A447 File Offset: 0x00058647
		// (set) Token: 0x0600421A RID: 16922 RVA: 0x0005A44F File Offset: 0x0005864F
		public Vector3 InteractionPosition { get; protected set; }

		// Token: 0x0600421B RID: 16923 RVA: 0x0005A458 File Offset: 0x00058658
		public List<InteractableCollisionInfo> GetCurrentIntersectingObjects()
		{
			return this._currentIntersectingObjects;
		}

		// Token: 0x0600421C RID: 16924
		public abstract List<InteractableCollisionInfo> GetNextIntersectingObjects();

		// Token: 0x0600421D RID: 16925
		public abstract void FocusOnInteractable(Interactable focusedInteractable, ColliderZone colliderZone);

		// Token: 0x0600421E RID: 16926
		public abstract void DeFocus();

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x0600421F RID: 16927
		// (set) Token: 0x06004220 RID: 16928
		public abstract bool EnableState { get; set; }

		// Token: 0x06004221 RID: 16929
		public abstract void Initialize();

		// Token: 0x06004222 RID: 16930 RVA: 0x0005A460 File Offset: 0x00058660
		public KeyValuePair<Interactable, InteractableCollisionInfo> GetFirstCurrentCollisionInfo()
		{
			return this._currInteractableToCollisionInfos.First<KeyValuePair<Interactable, InteractableCollisionInfo>>();
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x0005A46D File Offset: 0x0005866D
		public void ClearAllCurrentCollisionInfos()
		{
			this._currInteractableToCollisionInfos.Clear();
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x00170CBC File Offset: 0x0016EEBC
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

		// Token: 0x06004225 RID: 16933 RVA: 0x00170D68 File Offset: 0x0016EF68
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

		// Token: 0x0400432C RID: 17196
		protected List<InteractableCollisionInfo> _currentIntersectingObjects = new List<InteractableCollisionInfo>();

		// Token: 0x0400432D RID: 17197
		private List<Interactable> _addedInteractables = new List<Interactable>();

		// Token: 0x0400432E RID: 17198
		private List<Interactable> _removedInteractables = new List<Interactable>();

		// Token: 0x0400432F RID: 17199
		private List<Interactable> _remainingInteractables = new List<Interactable>();

		// Token: 0x04004330 RID: 17200
		private Dictionary<Interactable, InteractableCollisionInfo> _currInteractableToCollisionInfos = new Dictionary<Interactable, InteractableCollisionInfo>();

		// Token: 0x04004331 RID: 17201
		private Dictionary<Interactable, InteractableCollisionInfo> _prevInteractableToCollisionInfos = new Dictionary<Interactable, InteractableCollisionInfo>();
	}
}
