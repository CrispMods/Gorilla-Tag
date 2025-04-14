using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A48 RID: 2632
	public class ButtonController : Interactable
	{
		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600418A RID: 16778 RVA: 0x00136D96 File Offset: 0x00134F96
		public override int ValidToolTagsMask
		{
			get
			{
				return this._toolTagsMask;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x00136D9E File Offset: 0x00134F9E
		public Vector3 LocalButtonDirection
		{
			get
			{
				return this._localButtonDirection;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x0600418C RID: 16780 RVA: 0x00136DA6 File Offset: 0x00134FA6
		// (set) Token: 0x0600418D RID: 16781 RVA: 0x00136DAE File Offset: 0x00134FAE
		public InteractableState CurrentButtonState { get; private set; }

		// Token: 0x0600418E RID: 16782 RVA: 0x00136DB8 File Offset: 0x00134FB8
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

		// Token: 0x0600418F RID: 16783 RVA: 0x00136E28 File Offset: 0x00135028
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

		// Token: 0x06004190 RID: 16784 RVA: 0x00136E98 File Offset: 0x00135098
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

		// Token: 0x06004191 RID: 16785 RVA: 0x001370C0 File Offset: 0x001352C0
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

		// Token: 0x06004192 RID: 16786 RVA: 0x00137158 File Offset: 0x00135358
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

		// Token: 0x06004193 RID: 16787 RVA: 0x001371A4 File Offset: 0x001353A4
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

		// Token: 0x06004194 RID: 16788 RVA: 0x001371F8 File Offset: 0x001353F8
		private bool PassEntryTest(InteractableTool collidingTool, Vector3 buttonDirection)
		{
			return Vector3.Dot(collidingTool.Velocity.normalized, buttonDirection) >= 0.8f;
		}

		// Token: 0x06004195 RID: 16789 RVA: 0x00137224 File Offset: 0x00135424
		private bool PassPerpTest(InteractableTool collidingTool, Vector3 buttonDirection)
		{
			Vector3 vector = collidingTool.ToolTransform.right;
			if (collidingTool.IsRightHandedTool)
			{
				vector = -vector;
			}
			return Vector3.Dot(vector, buttonDirection) >= 0.5f;
		}

		// Token: 0x040042AE RID: 17070
		private const float ENTRY_DOT_THRESHOLD = 0.8f;

		// Token: 0x040042AF RID: 17071
		private const float PERP_DOT_THRESHOLD = 0.5f;

		// Token: 0x040042B0 RID: 17072
		[SerializeField]
		private GameObject _proximityZone;

		// Token: 0x040042B1 RID: 17073
		[SerializeField]
		private GameObject _contactZone;

		// Token: 0x040042B2 RID: 17074
		[SerializeField]
		private GameObject _actionZone;

		// Token: 0x040042B3 RID: 17075
		[SerializeField]
		private ButtonController.ContactTest[] _contactTests;

		// Token: 0x040042B4 RID: 17076
		[SerializeField]
		private Transform _buttonPlaneCenter;

		// Token: 0x040042B5 RID: 17077
		[SerializeField]
		private bool _makeSureToolIsOnPositiveSide = true;

		// Token: 0x040042B6 RID: 17078
		[SerializeField]
		private Vector3 _localButtonDirection = Vector3.down;

		// Token: 0x040042B7 RID: 17079
		[SerializeField]
		private InteractableToolTags[] _allValidToolsTags = new InteractableToolTags[]
		{
			InteractableToolTags.All
		};

		// Token: 0x040042B8 RID: 17080
		private int _toolTagsMask;

		// Token: 0x040042B9 RID: 17081
		[SerializeField]
		private bool _allowMultipleNearFieldInteraction;

		// Token: 0x040042BB RID: 17083
		private Dictionary<InteractableTool, InteractableState> _toolToState = new Dictionary<InteractableTool, InteractableState>();

		// Token: 0x02000A49 RID: 2633
		public enum ContactTest
		{
			// Token: 0x040042BD RID: 17085
			PerpenTest,
			// Token: 0x040042BE RID: 17086
			BackwardsPress
		}
	}
}
