using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A8A RID: 2698
	public abstract class InteractableTool : MonoBehaviour
	{
		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x0600434A RID: 17226 RVA: 0x00039243 File Offset: 0x00037443
		public Transform ToolTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x0600434B RID: 17227 RVA: 0x0005BE27 File Offset: 0x0005A027
		// (set) Token: 0x0600434C RID: 17228 RVA: 0x0005BE2F File Offset: 0x0005A02F
		public bool IsRightHandedTool { get; set; }

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x0600434D RID: 17229
		public abstract InteractableToolTags ToolTags { get; }

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x0600434E RID: 17230
		public abstract ToolInputState ToolInputState { get; }

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x0600434F RID: 17231
		public abstract bool IsFarFieldTool { get; }

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06004350 RID: 17232 RVA: 0x0005BE38 File Offset: 0x0005A038
		// (set) Token: 0x06004351 RID: 17233 RVA: 0x0005BE40 File Offset: 0x0005A040
		public Vector3 Velocity { get; protected set; }

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06004352 RID: 17234 RVA: 0x0005BE49 File Offset: 0x0005A049
		// (set) Token: 0x06004353 RID: 17235 RVA: 0x0005BE51 File Offset: 0x0005A051
		public Vector3 InteractionPosition { get; protected set; }

		// Token: 0x06004354 RID: 17236 RVA: 0x0005BE5A File Offset: 0x0005A05A
		public List<InteractableCollisionInfo> GetCurrentIntersectingObjects()
		{
			return this._currentIntersectingObjects;
		}

		// Token: 0x06004355 RID: 17237
		public abstract List<InteractableCollisionInfo> GetNextIntersectingObjects();

		// Token: 0x06004356 RID: 17238
		public abstract void FocusOnInteractable(Interactable focusedInteractable, ColliderZone colliderZone);

		// Token: 0x06004357 RID: 17239
		public abstract void DeFocus();

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06004358 RID: 17240
		// (set) Token: 0x06004359 RID: 17241
		public abstract bool EnableState { get; set; }

		// Token: 0x0600435A RID: 17242
		public abstract void Initialize();

		// Token: 0x0600435B RID: 17243 RVA: 0x0005BE62 File Offset: 0x0005A062
		public KeyValuePair<Interactable, InteractableCollisionInfo> GetFirstCurrentCollisionInfo()
		{
			return this._currInteractableToCollisionInfos.First<KeyValuePair<Interactable, InteractableCollisionInfo>>();
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x0005BE6F File Offset: 0x0005A06F
		public void ClearAllCurrentCollisionInfos()
		{
			this._currInteractableToCollisionInfos.Clear();
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x00177B40 File Offset: 0x00175D40
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

		// Token: 0x0600435E RID: 17246 RVA: 0x00177BEC File Offset: 0x00175DEC
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

		// Token: 0x04004414 RID: 17428
		protected List<InteractableCollisionInfo> _currentIntersectingObjects = new List<InteractableCollisionInfo>();

		// Token: 0x04004415 RID: 17429
		private List<Interactable> _addedInteractables = new List<Interactable>();

		// Token: 0x04004416 RID: 17430
		private List<Interactable> _removedInteractables = new List<Interactable>();

		// Token: 0x04004417 RID: 17431
		private List<Interactable> _remainingInteractables = new List<Interactable>();

		// Token: 0x04004418 RID: 17432
		private Dictionary<Interactable, InteractableCollisionInfo> _currInteractableToCollisionInfos = new Dictionary<Interactable, InteractableCollisionInfo>();

		// Token: 0x04004419 RID: 17433
		private Dictionary<Interactable, InteractableCollisionInfo> _prevInteractableToCollisionInfos = new Dictionary<Interactable, InteractableCollisionInfo>();
	}
}
