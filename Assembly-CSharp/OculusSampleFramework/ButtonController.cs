using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A45 RID: 2629
	public class ButtonController : Interactable
	{
		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x0600417E RID: 16766 RVA: 0x001367CE File Offset: 0x001349CE
		public override int ValidToolTagsMask
		{
			get
			{
				return this._toolTagsMask;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600417F RID: 16767 RVA: 0x001367D6 File Offset: 0x001349D6
		public Vector3 LocalButtonDirection
		{
			get
			{
				return this._localButtonDirection;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06004180 RID: 16768 RVA: 0x001367DE File Offset: 0x001349DE
		// (set) Token: 0x06004181 RID: 16769 RVA: 0x001367E6 File Offset: 0x001349E6
		public InteractableState CurrentButtonState { get; private set; }

		// Token: 0x06004182 RID: 16770 RVA: 0x001367F0 File Offset: 0x001349F0
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

		// Token: 0x06004183 RID: 16771 RVA: 0x00136860 File Offset: 0x00134A60
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

		// Token: 0x06004184 RID: 16772 RVA: 0x001368D0 File Offset: 0x00134AD0
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

		// Token: 0x06004185 RID: 16773 RVA: 0x00136AF8 File Offset: 0x00134CF8
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

		// Token: 0x06004186 RID: 16774 RVA: 0x00136B90 File Offset: 0x00134D90
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

		// Token: 0x06004187 RID: 16775 RVA: 0x00136BDC File Offset: 0x00134DDC
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

		// Token: 0x06004188 RID: 16776 RVA: 0x00136C30 File Offset: 0x00134E30
		private bool PassEntryTest(InteractableTool collidingTool, Vector3 buttonDirection)
		{
			return Vector3.Dot(collidingTool.Velocity.normalized, buttonDirection) >= 0.8f;
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x00136C5C File Offset: 0x00134E5C
		private bool PassPerpTest(InteractableTool collidingTool, Vector3 buttonDirection)
		{
			Vector3 vector = collidingTool.ToolTransform.right;
			if (collidingTool.IsRightHandedTool)
			{
				vector = -vector;
			}
			return Vector3.Dot(vector, buttonDirection) >= 0.5f;
		}

		// Token: 0x0400429C RID: 17052
		private const float ENTRY_DOT_THRESHOLD = 0.8f;

		// Token: 0x0400429D RID: 17053
		private const float PERP_DOT_THRESHOLD = 0.5f;

		// Token: 0x0400429E RID: 17054
		[SerializeField]
		private GameObject _proximityZone;

		// Token: 0x0400429F RID: 17055
		[SerializeField]
		private GameObject _contactZone;

		// Token: 0x040042A0 RID: 17056
		[SerializeField]
		private GameObject _actionZone;

		// Token: 0x040042A1 RID: 17057
		[SerializeField]
		private ButtonController.ContactTest[] _contactTests;

		// Token: 0x040042A2 RID: 17058
		[SerializeField]
		private Transform _buttonPlaneCenter;

		// Token: 0x040042A3 RID: 17059
		[SerializeField]
		private bool _makeSureToolIsOnPositiveSide = true;

		// Token: 0x040042A4 RID: 17060
		[SerializeField]
		private Vector3 _localButtonDirection = Vector3.down;

		// Token: 0x040042A5 RID: 17061
		[SerializeField]
		private InteractableToolTags[] _allValidToolsTags = new InteractableToolTags[]
		{
			InteractableToolTags.All
		};

		// Token: 0x040042A6 RID: 17062
		private int _toolTagsMask;

		// Token: 0x040042A7 RID: 17063
		[SerializeField]
		private bool _allowMultipleNearFieldInteraction;

		// Token: 0x040042A9 RID: 17065
		private Dictionary<InteractableTool, InteractableState> _toolToState = new Dictionary<InteractableTool, InteractableState>();

		// Token: 0x02000A46 RID: 2630
		public enum ContactTest
		{
			// Token: 0x040042AB RID: 17067
			PerpenTest,
			// Token: 0x040042AC RID: 17068
			BackwardsPress
		}
	}
}
