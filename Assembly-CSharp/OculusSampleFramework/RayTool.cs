using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A8E RID: 2702
	public class RayTool : InteractableTool
	{
		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x0600436B RID: 17259 RVA: 0x00039846 File Offset: 0x00037A46
		public override InteractableToolTags ToolTags
		{
			get
			{
				return InteractableToolTags.Ray;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x0600436C RID: 17260 RVA: 0x0005BEDD File Offset: 0x0005A0DD
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

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x0600436D RID: 17261 RVA: 0x00039846 File Offset: 0x00037A46
		public override bool IsFarFieldTool
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x0600436E RID: 17262 RVA: 0x0005BF0D File Offset: 0x0005A10D
		// (set) Token: 0x0600436F RID: 17263 RVA: 0x0005BF1A File Offset: 0x0005A11A
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

		// Token: 0x06004370 RID: 17264 RVA: 0x0005BF28 File Offset: 0x0005A128
		public override void Initialize()
		{
			InteractableToolsInputRouter.Instance.RegisterInteractableTool(this);
			this._rayToolView.InteractableTool = this;
			this._coneAngleReleaseDegrees = this._coneAngleDegrees * 1.2f;
			this._initialized = true;
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x0005BF5A File Offset: 0x0005A15A
		private void OnDestroy()
		{
			if (InteractableToolsInputRouter.Instance != null)
			{
				InteractableToolsInputRouter.Instance.UnregisterInteractableTool(this);
			}
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x00177F38 File Offset: 0x00176138
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

		// Token: 0x06004373 RID: 17267 RVA: 0x0005BF74 File Offset: 0x0005A174
		private Vector3 GetRayCastOrigin()
		{
			return base.transform.position + 0.8f * base.transform.forward;
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x00178018 File Offset: 0x00176218
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

		// Token: 0x06004375 RID: 17269 RVA: 0x00178124 File Offset: 0x00176324
		private bool HasRayReleasedInteractable(Interactable focusedInteractable)
		{
			Vector3 position = base.transform.position;
			Vector3 forward = base.transform.forward;
			float num = Mathf.Cos(this._coneAngleReleaseDegrees * 0.017453292f);
			Vector3 lhs = focusedInteractable.transform.position - position;
			lhs.Normalize();
			return Vector3.Dot(lhs, forward) < num;
		}

		// Token: 0x06004376 RID: 17270 RVA: 0x00178180 File Offset: 0x00176380
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

		// Token: 0x06004377 RID: 17271 RVA: 0x001781C0 File Offset: 0x001763C0
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

		// Token: 0x06004378 RID: 17272 RVA: 0x00178270 File Offset: 0x00176470
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

		// Token: 0x06004379 RID: 17273 RVA: 0x0005BF9B File Offset: 0x0005A19B
		public override void FocusOnInteractable(Interactable focusedInteractable, ColliderZone colliderZone)
		{
			this._rayToolView.SetFocusedInteractable(focusedInteractable);
			this._focusedInteractable = focusedInteractable;
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x0005BFB0 File Offset: 0x0005A1B0
		public override void DeFocus()
		{
			this._rayToolView.SetFocusedInteractable(null);
			this._focusedInteractable = null;
		}

		// Token: 0x04004422 RID: 17442
		private const float MINIMUM_RAY_CAST_DISTANCE = 0.8f;

		// Token: 0x04004423 RID: 17443
		private const float COLLIDER_RADIUS = 0.01f;

		// Token: 0x04004424 RID: 17444
		private const int NUM_MAX_PRIMARY_HITS = 10;

		// Token: 0x04004425 RID: 17445
		private const int NUM_MAX_SECONDARY_HITS = 25;

		// Token: 0x04004426 RID: 17446
		private const int NUM_COLLIDERS_TO_TEST = 20;

		// Token: 0x04004427 RID: 17447
		[SerializeField]
		private RayToolView _rayToolView;

		// Token: 0x04004428 RID: 17448
		[Range(0f, 45f)]
		[SerializeField]
		private float _coneAngleDegrees = 20f;

		// Token: 0x04004429 RID: 17449
		[SerializeField]
		private float _farFieldMaxDistance = 5f;

		// Token: 0x0400442A RID: 17450
		private PinchStateModule _pinchStateModule = new PinchStateModule();

		// Token: 0x0400442B RID: 17451
		private Interactable _focusedInteractable;

		// Token: 0x0400442C RID: 17452
		private Collider[] _collidersOverlapped = new Collider[20];

		// Token: 0x0400442D RID: 17453
		private Interactable _currInteractableCastedAgainst;

		// Token: 0x0400442E RID: 17454
		private float _coneAngleReleaseDegrees;

		// Token: 0x0400442F RID: 17455
		private RaycastHit[] _primaryHits = new RaycastHit[10];

		// Token: 0x04004430 RID: 17456
		private Collider[] _secondaryOverlapResults = new Collider[25];

		// Token: 0x04004431 RID: 17457
		private bool _initialized;
	}
}
