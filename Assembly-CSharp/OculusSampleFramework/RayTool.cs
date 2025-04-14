using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A61 RID: 2657
	public class RayTool : InteractableTool
	{
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06004226 RID: 16934 RVA: 0x000444E2 File Offset: 0x000426E2
		public override InteractableToolTags ToolTags
		{
			get
			{
				return InteractableToolTags.Ray;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06004227 RID: 16935 RVA: 0x001385C9 File Offset: 0x001367C9
		public override ToolInputState ToolInputState
		{
			get
			{
				if (this._pinchStateModule.PinchDownOnFocusedObject)
				{
					return ToolInputState.PrimaryInputDown;
				}
				if (this._pinchStateModule.PinchSteadyOnFocusedObject)
				{
					return ToolInputState.PrimaryInputDownStay;
				}
				if (this._pinchStateModule.PinchUpAndDownOnFocusedObject)
				{
					return ToolInputState.PrimaryInputUp;
				}
				return ToolInputState.Inactive;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06004228 RID: 16936 RVA: 0x000444E2 File Offset: 0x000426E2
		public override bool IsFarFieldTool
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06004229 RID: 16937 RVA: 0x001385F9 File Offset: 0x001367F9
		// (set) Token: 0x0600422A RID: 16938 RVA: 0x00138606 File Offset: 0x00136806
		public override bool EnableState
		{
			get
			{
				return this._rayToolView.EnableState;
			}
			set
			{
				this._rayToolView.EnableState = value;
			}
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x00138614 File Offset: 0x00136814
		public override void Initialize()
		{
			InteractableToolsInputRouter.Instance.RegisterInteractableTool(this);
			this._rayToolView.InteractableTool = this;
			this._coneAngleReleaseDegrees = this._coneAngleDegrees * 1.2f;
			this._initialized = true;
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x00138646 File Offset: 0x00136846
		private void OnDestroy()
		{
			if (InteractableToolsInputRouter.Instance != null)
			{
				InteractableToolsInputRouter.Instance.UnregisterInteractableTool(this);
			}
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x00138660 File Offset: 0x00136860
		private void Update()
		{
			if (!HandsManager.Instance || !HandsManager.Instance.IsInitialized() || !this._initialized)
			{
				return;
			}
			OVRHand ovrhand = base.IsRightHandedTool ? HandsManager.Instance.RightHand : HandsManager.Instance.LeftHand;
			Transform pointerPose = ovrhand.PointerPose;
			base.transform.position = pointerPose.position;
			base.transform.rotation = pointerPose.rotation;
			Vector3 interactionPosition = base.InteractionPosition;
			Vector3 position = base.transform.position;
			base.Velocity = (position - interactionPosition) / Time.deltaTime;
			base.InteractionPosition = position;
			this._pinchStateModule.UpdateState(ovrhand, this._focusedInteractable);
			this._rayToolView.ToolActivateState = (this._pinchStateModule.PinchSteadyOnFocusedObject || this._pinchStateModule.PinchDownOnFocusedObject);
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x0013873F File Offset: 0x0013693F
		private Vector3 GetRayCastOrigin()
		{
			return base.transform.position + 0.8f * base.transform.forward;
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x00138768 File Offset: 0x00136968
		public override List<InteractableCollisionInfo> GetNextIntersectingObjects()
		{
			if (!this._initialized)
			{
				return this._currentIntersectingObjects;
			}
			if (this._currInteractableCastedAgainst != null && this.HasRayReleasedInteractable(this._currInteractableCastedAgainst))
			{
				this._currInteractableCastedAgainst = null;
			}
			if (this._currInteractableCastedAgainst == null)
			{
				this._currentIntersectingObjects.Clear();
				this._currInteractableCastedAgainst = this.FindTargetInteractable();
				if (this._currInteractableCastedAgainst != null)
				{
					int num = Physics.OverlapSphereNonAlloc(this._currInteractableCastedAgainst.transform.position, 0.01f, this._collidersOverlapped);
					for (int i = 0; i < num; i++)
					{
						ColliderZone component = this._collidersOverlapped[i].GetComponent<ColliderZone>();
						if (component != null)
						{
							Interactable parentInteractable = component.ParentInteractable;
							if (!(parentInteractable == null) && !(parentInteractable != this._currInteractableCastedAgainst))
							{
								InteractableCollisionInfo item = new InteractableCollisionInfo(component, component.CollisionDepth, this);
								this._currentIntersectingObjects.Add(item);
							}
						}
					}
					if (this._currentIntersectingObjects.Count == 0)
					{
						this._currInteractableCastedAgainst = null;
					}
				}
			}
			return this._currentIntersectingObjects;
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x00138874 File Offset: 0x00136A74
		private bool HasRayReleasedInteractable(Interactable focusedInteractable)
		{
			Vector3 position = base.transform.position;
			Vector3 forward = base.transform.forward;
			float num = Mathf.Cos(this._coneAngleReleaseDegrees * 0.017453292f);
			Vector3 lhs = focusedInteractable.transform.position - position;
			lhs.Normalize();
			return Vector3.Dot(lhs, forward) < num;
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x001388D0 File Offset: 0x00136AD0
		private Interactable FindTargetInteractable()
		{
			Vector3 rayCastOrigin = this.GetRayCastOrigin();
			Vector3 forward = base.transform.forward;
			Interactable interactable = this.FindPrimaryRaycastHit(rayCastOrigin, forward);
			if (interactable == null)
			{
				interactable = this.FindInteractableViaConeTest(rayCastOrigin, forward);
			}
			return interactable;
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x00138910 File Offset: 0x00136B10
		private Interactable FindPrimaryRaycastHit(Vector3 rayOrigin, Vector3 rayDirection)
		{
			Interactable interactable = null;
			int num = Physics.RaycastNonAlloc(new Ray(rayOrigin, rayDirection), this._primaryHits, float.PositiveInfinity);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = this._primaryHits[i];
				ColliderZone component = raycastHit.transform.GetComponent<ColliderZone>();
				if (component != null)
				{
					Interactable parentInteractable = component.ParentInteractable;
					if (!(parentInteractable == null) && (parentInteractable.ValidToolTagsMask & (int)this.ToolTags) != 0)
					{
						float magnitude = (parentInteractable.transform.position - rayOrigin).magnitude;
						if (interactable == null || magnitude < num2)
						{
							interactable = parentInteractable;
							num2 = magnitude;
						}
					}
				}
			}
			return interactable;
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x001389C0 File Offset: 0x00136BC0
		private Interactable FindInteractableViaConeTest(Vector3 rayOrigin, Vector3 rayDirection)
		{
			Interactable interactable = null;
			float num = 0f;
			float num2 = Mathf.Cos(this._coneAngleDegrees * 0.017453292f);
			float num3 = Mathf.Tan(0.017453292f * this._coneAngleDegrees * 0.5f) * this._farFieldMaxDistance;
			int num4 = Physics.OverlapBoxNonAlloc(rayOrigin + rayDirection * this._farFieldMaxDistance * 0.5f, new Vector3(num3, num3, this._farFieldMaxDistance * 0.5f), this._secondaryOverlapResults, base.transform.rotation);
			for (int i = 0; i < num4; i++)
			{
				ColliderZone component = this._secondaryOverlapResults[i].GetComponent<ColliderZone>();
				if (component != null)
				{
					Interactable parentInteractable = component.ParentInteractable;
					if (!(parentInteractable == null) && (parentInteractable.ValidToolTagsMask & (int)this.ToolTags) != 0)
					{
						Vector3 vector = parentInteractable.transform.position - rayOrigin;
						float magnitude = vector.magnitude;
						vector /= magnitude;
						if (Vector3.Dot(vector, rayDirection) >= num2 && (interactable == null || magnitude < num))
						{
							interactable = parentInteractable;
							num = magnitude;
						}
					}
				}
			}
			return interactable;
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x00138AE3 File Offset: 0x00136CE3
		public override void FocusOnInteractable(Interactable focusedInteractable, ColliderZone colliderZone)
		{
			this._rayToolView.SetFocusedInteractable(focusedInteractable);
			this._focusedInteractable = focusedInteractable;
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x00138AF8 File Offset: 0x00136CF8
		public override void DeFocus()
		{
			this._rayToolView.SetFocusedInteractable(null);
			this._focusedInteractable = null;
		}

		// Token: 0x04004328 RID: 17192
		private const float MINIMUM_RAY_CAST_DISTANCE = 0.8f;

		// Token: 0x04004329 RID: 17193
		private const float COLLIDER_RADIUS = 0.01f;

		// Token: 0x0400432A RID: 17194
		private const int NUM_MAX_PRIMARY_HITS = 10;

		// Token: 0x0400432B RID: 17195
		private const int NUM_MAX_SECONDARY_HITS = 25;

		// Token: 0x0400432C RID: 17196
		private const int NUM_COLLIDERS_TO_TEST = 20;

		// Token: 0x0400432D RID: 17197
		[SerializeField]
		private RayToolView _rayToolView;

		// Token: 0x0400432E RID: 17198
		[Range(0f, 45f)]
		[SerializeField]
		private float _coneAngleDegrees = 20f;

		// Token: 0x0400432F RID: 17199
		[SerializeField]
		private float _farFieldMaxDistance = 5f;

		// Token: 0x04004330 RID: 17200
		private PinchStateModule _pinchStateModule = new PinchStateModule();

		// Token: 0x04004331 RID: 17201
		private Interactable _focusedInteractable;

		// Token: 0x04004332 RID: 17202
		private Collider[] _collidersOverlapped = new Collider[20];

		// Token: 0x04004333 RID: 17203
		private Interactable _currInteractableCastedAgainst;

		// Token: 0x04004334 RID: 17204
		private float _coneAngleReleaseDegrees;

		// Token: 0x04004335 RID: 17205
		private RaycastHit[] _primaryHits = new RaycastHit[10];

		// Token: 0x04004336 RID: 17206
		private Collider[] _secondaryOverlapResults = new Collider[25];

		// Token: 0x04004337 RID: 17207
		private bool _initialized;
	}
}
