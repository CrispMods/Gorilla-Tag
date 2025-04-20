using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A72 RID: 2674
	public class ButtonController : Interactable
	{
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060042C3 RID: 17091 RVA: 0x0005B9B0 File Offset: 0x00059BB0
		public override int ValidToolTagsMask
		{
			get
			{
				return this._toolTagsMask;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060042C4 RID: 17092 RVA: 0x0005B9B8 File Offset: 0x00059BB8
		public Vector3 LocalButtonDirection
		{
			get
			{
				return this._localButtonDirection;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060042C5 RID: 17093 RVA: 0x0005B9C0 File Offset: 0x00059BC0
		// (set) Token: 0x060042C6 RID: 17094 RVA: 0x0005B9C8 File Offset: 0x00059BC8
		public InteractableState CurrentButtonState { get; private set; }

		// Token: 0x060042C7 RID: 17095 RVA: 0x0017666C File Offset: 0x0017486C
		protected override void Awake()
		{
			base.Awake();
			foreach (InteractableToolTags interactableToolTags in this._allValidToolsTags)
			{
				this._toolTagsMask |= (int)interactableToolTags;
			}
			this._proximityZoneCollider = this._proximityZone.GetComponent<ColliderZone>();
			this._contactZoneCollider = this._contactZone.GetComponent<ColliderZone>();
			this._actionZoneCollider = this._actionZone.GetComponent<ColliderZone>();
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x001766DC File Offset: 0x001748DC
		private void FireInteractionEventsOnDepth(InteractableCollisionDepth oldDepth, InteractableTool collidingTool, InteractionType interactionType)
		{
			switch (oldDepth)
			{
			case InteractableCollisionDepth.Proximity:
				this.OnProximityZoneEvent(new ColliderZoneArgs(base.ProximityCollider, (float)Time.frameCount, collidingTool, interactionType));
				return;
			case InteractableCollisionDepth.Contact:
				this.OnContactZoneEvent(new ColliderZoneArgs(base.ContactCollider, (float)Time.frameCount, collidingTool, interactionType));
				return;
			case InteractableCollisionDepth.Action:
				this.OnActionZoneEvent(new ColliderZoneArgs(base.ActionCollider, (float)Time.frameCount, collidingTool, interactionType));
				return;
			default:
				return;
			}
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x0017674C File Offset: 0x0017494C
		public override void UpdateCollisionDepth(InteractableTool interactableTool, InteractableCollisionDepth oldCollisionDepth, InteractableCollisionDepth newCollisionDepth)
		{
			bool isFarFieldTool = interactableTool.IsFarFieldTool;
			if (!isFarFieldTool && !this._allowMultipleNearFieldInteraction && this._toolToState.Keys.Count > 0 && !this._toolToState.ContainsKey(interactableTool))
			{
				return;
			}
			InteractableState currentButtonState = this.CurrentButtonState;
			Vector3 vector = base.transform.TransformDirection(this._localButtonDirection);
			bool validContact = this.IsValidContact(interactableTool, vector) || interactableTool.IsFarFieldTool;
			bool toolIsInProximity = newCollisionDepth >= InteractableCollisionDepth.Proximity;
			bool flag = newCollisionDepth == InteractableCollisionDepth.Contact;
			bool flag2 = newCollisionDepth == InteractableCollisionDepth.Action;
			bool flag3 = oldCollisionDepth != newCollisionDepth;
			if (flag3)
			{
				this.FireInteractionEventsOnDepth(oldCollisionDepth, interactableTool, InteractionType.Exit);
				this.FireInteractionEventsOnDepth(newCollisionDepth, interactableTool, InteractionType.Enter);
			}
			else
			{
				this.FireInteractionEventsOnDepth(newCollisionDepth, interactableTool, InteractionType.Stay);
			}
			InteractableState interactableState = currentButtonState;
			if (interactableTool.IsFarFieldTool)
			{
				interactableState = (flag ? InteractableState.ContactState : (flag2 ? InteractableState.ActionState : InteractableState.Default));
			}
			else
			{
				Plane plane = new Plane(-vector, this._buttonPlaneCenter.position);
				bool onPositiveSideOfInteractable = !this._makeSureToolIsOnPositiveSide || plane.GetSide(interactableTool.InteractionPosition);
				interactableState = this.GetUpcomingStateNearField(currentButtonState, newCollisionDepth, flag2, flag, toolIsInProximity, validContact, onPositiveSideOfInteractable);
			}
			if (interactableState != InteractableState.Default)
			{
				this._toolToState[interactableTool] = interactableState;
			}
			else
			{
				this._toolToState.Remove(interactableTool);
			}
			if (isFarFieldTool || this._allowMultipleNearFieldInteraction)
			{
				foreach (InteractableState interactableState2 in this._toolToState.Values)
				{
					if (interactableState < interactableState2)
					{
						interactableState = interactableState2;
					}
				}
			}
			if (currentButtonState != interactableState)
			{
				this.CurrentButtonState = interactableState;
				InteractionType interactionType = (!flag3) ? InteractionType.Stay : ((newCollisionDepth == InteractableCollisionDepth.None) ? InteractionType.Exit : InteractionType.Enter);
				ColliderZone collider;
				switch (this.CurrentButtonState)
				{
				case InteractableState.ProximityState:
					collider = base.ProximityCollider;
					break;
				case InteractableState.ContactState:
					collider = base.ContactCollider;
					break;
				case InteractableState.ActionState:
					collider = base.ActionCollider;
					break;
				default:
					collider = null;
					break;
				}
				Interactable.InteractableStateArgsEvent interactableStateChanged = this.InteractableStateChanged;
				if (interactableStateChanged == null)
				{
					return;
				}
				interactableStateChanged.Invoke(new InteractableStateArgs(this, interactableTool, this.CurrentButtonState, currentButtonState, new ColliderZoneArgs(collider, (float)Time.frameCount, interactableTool, interactionType)));
			}
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x00176974 File Offset: 0x00174B74
		private InteractableState GetUpcomingStateNearField(InteractableState oldState, InteractableCollisionDepth newCollisionDepth, bool toolIsInActionZone, bool toolIsInContactZone, bool toolIsInProximity, bool validContact, bool onPositiveSideOfInteractable)
		{
			InteractableState result = oldState;
			switch (oldState)
			{
			case InteractableState.Default:
				if (validContact && onPositiveSideOfInteractable && newCollisionDepth > InteractableCollisionDepth.Proximity)
				{
					result = ((newCollisionDepth == InteractableCollisionDepth.Action) ? InteractableState.ActionState : InteractableState.ContactState);
				}
				else if (toolIsInProximity)
				{
					result = InteractableState.ProximityState;
				}
				break;
			case InteractableState.ProximityState:
				if (newCollisionDepth < InteractableCollisionDepth.Proximity)
				{
					result = InteractableState.Default;
				}
				else if (validContact && onPositiveSideOfInteractable && newCollisionDepth > InteractableCollisionDepth.Proximity)
				{
					result = ((newCollisionDepth == InteractableCollisionDepth.Action) ? InteractableState.ActionState : InteractableState.ContactState);
				}
				break;
			case InteractableState.ContactState:
				if (newCollisionDepth < InteractableCollisionDepth.Contact)
				{
					result = (toolIsInProximity ? InteractableState.ProximityState : InteractableState.Default);
				}
				else if (toolIsInActionZone && validContact && onPositiveSideOfInteractable)
				{
					result = InteractableState.ActionState;
				}
				break;
			case InteractableState.ActionState:
				if (!toolIsInActionZone)
				{
					if (toolIsInContactZone)
					{
						result = InteractableState.ContactState;
					}
					else if (toolIsInProximity)
					{
						result = InteractableState.ProximityState;
					}
					else
					{
						result = InteractableState.Default;
					}
				}
				break;
			}
			return result;
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x00176A0C File Offset: 0x00174C0C
		public void ForceResetButton()
		{
			InteractableState currentButtonState = this.CurrentButtonState;
			this.CurrentButtonState = InteractableState.Default;
			Interactable.InteractableStateArgsEvent interactableStateChanged = this.InteractableStateChanged;
			if (interactableStateChanged == null)
			{
				return;
			}
			interactableStateChanged.Invoke(new InteractableStateArgs(this, null, this.CurrentButtonState, currentButtonState, new ColliderZoneArgs(base.ContactCollider, (float)Time.frameCount, null, InteractionType.Exit)));
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x00176A58 File Offset: 0x00174C58
		private bool IsValidContact(InteractableTool collidingTool, Vector3 buttonDirection)
		{
			if (this._contactTests == null || collidingTool.IsFarFieldTool)
			{
				return true;
			}
			ButtonController.ContactTest[] contactTests = this._contactTests;
			for (int i = 0; i < contactTests.Length; i++)
			{
				if (contactTests[i] == ButtonController.ContactTest.BackwardsPress)
				{
					if (!this.PassEntryTest(collidingTool, buttonDirection))
					{
						return false;
					}
				}
				else if (!this.PassPerpTest(collidingTool, buttonDirection))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x00176AAC File Offset: 0x00174CAC
		private bool PassEntryTest(InteractableTool collidingTool, Vector3 buttonDirection)
		{
			return Vector3.Dot(collidingTool.Velocity.normalized, buttonDirection) >= 0.8f;
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x00176AD8 File Offset: 0x00174CD8
		private bool PassPerpTest(InteractableTool collidingTool, Vector3 buttonDirection)
		{
			Vector3 vector = collidingTool.ToolTransform.right;
			if (collidingTool.IsRightHandedTool)
			{
				vector = -vector;
			}
			return Vector3.Dot(vector, buttonDirection) >= 0.5f;
		}

		// Token: 0x04004396 RID: 17302
		private const float ENTRY_DOT_THRESHOLD = 0.8f;

		// Token: 0x04004397 RID: 17303
		private const float PERP_DOT_THRESHOLD = 0.5f;

		// Token: 0x04004398 RID: 17304
		[SerializeField]
		private GameObject _proximityZone;

		// Token: 0x04004399 RID: 17305
		[SerializeField]
		private GameObject _contactZone;

		// Token: 0x0400439A RID: 17306
		[SerializeField]
		private GameObject _actionZone;

		// Token: 0x0400439B RID: 17307
		[SerializeField]
		private ButtonController.ContactTest[] _contactTests;

		// Token: 0x0400439C RID: 17308
		[SerializeField]
		private Transform _buttonPlaneCenter;

		// Token: 0x0400439D RID: 17309
		[SerializeField]
		private bool _makeSureToolIsOnPositiveSide = true;

		// Token: 0x0400439E RID: 17310
		[SerializeField]
		private Vector3 _localButtonDirection = Vector3.down;

		// Token: 0x0400439F RID: 17311
		[SerializeField]
		private InteractableToolTags[] _allValidToolsTags = new InteractableToolTags[]
		{
			InteractableToolTags.All
		};

		// Token: 0x040043A0 RID: 17312
		private int _toolTagsMask;

		// Token: 0x040043A1 RID: 17313
		[SerializeField]
		private bool _allowMultipleNearFieldInteraction;

		// Token: 0x040043A3 RID: 17315
		private Dictionary<InteractableTool, InteractableState> _toolToState = new Dictionary<InteractableTool, InteractableState>();

		// Token: 0x02000A73 RID: 2675
		public enum ContactTest
		{
			// Token: 0x040043A5 RID: 17317
			PerpenTest,
			// Token: 0x040043A6 RID: 17318
			BackwardsPress
		}
	}
}
