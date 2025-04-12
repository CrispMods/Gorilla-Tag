using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A64 RID: 2660
	public class RayTool : InteractableTool
	{
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06004232 RID: 16946 RVA: 0x00038586 File Offset: 0x00036786
		public override InteractableToolTags ToolTags
		{
			get
			{
				return InteractableToolTags.Ray;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06004233 RID: 16947 RVA: 0x0005A4DB File Offset: 0x000586DB
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

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06004234 RID: 16948 RVA: 0x00038586 File Offset: 0x00036786
		public override bool IsFarFieldTool
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06004235 RID: 16949 RVA: 0x0005A50B File Offset: 0x0005870B
		// (set) Token: 0x06004236 RID: 16950 RVA: 0x0005A518 File Offset: 0x00058718
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

		// Token: 0x06004237 RID: 16951 RVA: 0x0005A526 File Offset: 0x00058726
		public override void Initialize()
		{
			InteractableToolsInputRouter.Instance.RegisterInteractableTool(this);
			this._rayToolView.InteractableTool = this;
			this._coneAngleReleaseDegrees = this._coneAngleDegrees * 1.2f;
			this._initialized = true;
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x0005A558 File Offset: 0x00058758
		private void OnDestroy()
		{
			if (InteractableToolsInputRouter.Instance != null)
			{
				InteractableToolsInputRouter.Instance.UnregisterInteractableTool(this);
			}
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x001710B4 File Offset: 0x0016F2B4
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

		// Token: 0x0600423A RID: 16954 RVA: 0x0005A572 File Offset: 0x00058772
		private Vector3 GetRayCastOrigin()
		{
			return base.transform.position + 0.8f * base.transform.forward;
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x00171194 File Offset: 0x0016F394
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

		// Token: 0x0600423C RID: 16956 RVA: 0x001712A0 File Offset: 0x0016F4A0
		private bool HasRayReleasedInteractable(Interactable focusedInteractable)
		{
			Vector3 position = base.transform.position;
			Vector3 forward = base.transform.forward;
			float num = Mathf.Cos(this._coneAngleReleaseDegrees * 0.017453292f);
			Vector3 lhs = focusedInteractable.transform.position - position;
			lhs.Normalize();
			return Vector3.Dot(lhs, forward) < num;
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x001712FC File Offset: 0x0016F4FC
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

		// Token: 0x0600423E RID: 16958 RVA: 0x0017133C File Offset: 0x0016F53C
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

		// Token: 0x0600423F RID: 16959 RVA: 0x001713EC File Offset: 0x0016F5EC
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

		// Token: 0x06004240 RID: 16960 RVA: 0x0005A599 File Offset: 0x00058799
		public override void FocusOnInteractable(Interactable focusedInteractable, ColliderZone colliderZone)
		{
			this._rayToolView.SetFocusedInteractable(focusedInteractable);
			this._focusedInteractable = focusedInteractable;
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x0005A5AE File Offset: 0x000587AE
		public override void DeFocus()
		{
			this._rayToolView.SetFocusedInteractable(null);
			this._focusedInteractable = null;
		}

		// Token: 0x0400433A RID: 17210
		private const float MINIMUM_RAY_CAST_DISTANCE = 0.8f;

		// Token: 0x0400433B RID: 17211
		private const float COLLIDER_RADIUS = 0.01f;

		// Token: 0x0400433C RID: 17212
		private const int NUM_MAX_PRIMARY_HITS = 10;

		// Token: 0x0400433D RID: 17213
		private const int NUM_MAX_SECONDARY_HITS = 25;

		// Token: 0x0400433E RID: 17214
		private const int NUM_COLLIDERS_TO_TEST = 20;

		// Token: 0x0400433F RID: 17215
		[SerializeField]
		private RayToolView _rayToolView;

		// Token: 0x04004340 RID: 17216
		[Range(0f, 45f)]
		[SerializeField]
		private float _coneAngleDegrees = 20f;

		// Token: 0x04004341 RID: 17217
		[SerializeField]
		private float _farFieldMaxDistance = 5f;

		// Token: 0x04004342 RID: 17218
		private PinchStateModule _pinchStateModule = new PinchStateModule();

		// Token: 0x04004343 RID: 17219
		private Interactable _focusedInteractable;

		// Token: 0x04004344 RID: 17220
		private Collider[] _collidersOverlapped = new Collider[20];

		// Token: 0x04004345 RID: 17221
		private Interactable _currInteractableCastedAgainst;

		// Token: 0x04004346 RID: 17222
		private float _coneAngleReleaseDegrees;

		// Token: 0x04004347 RID: 17223
		private RaycastHit[] _primaryHits = new RaycastHit[10];

		// Token: 0x04004348 RID: 17224
		private Collider[] _secondaryOverlapResults = new Collider[25];

		// Token: 0x04004349 RID: 17225
		private bool _initialized;
	}
}
