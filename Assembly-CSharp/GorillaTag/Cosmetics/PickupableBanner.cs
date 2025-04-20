using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C5B RID: 3163
	public class PickupableBanner : PickupableVariant
	{
		// Token: 0x06004F1B RID: 20251 RVA: 0x001B5B8C File Offset: 0x001B3D8C
		protected internal override void Pickup()
		{
			UnityEvent onPickup = this.OnPickup;
			if (onPickup != null)
			{
				onPickup.Invoke();
			}
			this.rb.isKinematic = true;
			this.rb.velocity = Vector3.zero;
			if (this.holdableParent != null)
			{
				base.transform.parent = this.holdableParent.transform;
			}
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			base.transform.localScale = Vector3.one;
			this.scale = 1f;
			this.placedOnFloorTime = -1f;
			this.placedOnFloor = false;
			if (this.interactionPoint != null)
			{
				this.interactionPoint.enabled = true;
			}
			base.enabled = false;
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x001B5C58 File Offset: 0x001B3E58
		protected internal override void Release(HoldableObject holdable, Vector3 startPosition, Vector3 velocity, float playerScale)
		{
			this.holdableParent = holdable;
			base.transform.parent = null;
			base.transform.position = startPosition;
			base.transform.localScale = Vector3.one * playerScale;
			this.rb.isKinematic = false;
			this.rb.useGravity = true;
			this.rb.velocity = velocity;
			if (!this.allowPickupFromGround && this.interactionPoint != null)
			{
				this.interactionPoint.enabled = false;
			}
			this.scale = playerScale;
			base.enabled = true;
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x001B5CF0 File Offset: 0x001B3EF0
		private void FixedUpdate()
		{
			if (this.placedOnFloor && Time.time - this.placedOnFloorTime > this.autoPickupAfterSeconds)
			{
				this.Pickup();
				this.placedOnFloorTime = -1f;
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(this.raycastOrigin.position, Vector3.down, out raycastHit, 0.1f * this.scale, this.floorLayerMask.value, QueryTriggerInteraction.Ignore))
			{
				UnityEvent onPlaced = this.OnPlaced;
				if (onPlaced != null)
				{
					onPlaced.Invoke();
				}
				this.placedOnFloor = true;
				this.placedOnFloorTime = Time.time;
				this.rb.isKinematic = true;
				this.rb.useGravity = false;
				this.rb.velocity = Vector3.zero;
				Vector3 normal = raycastHit.normal;
				base.transform.position = raycastHit.point + raycastHit.normal * this.placementOffset;
				Quaternion rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(base.transform.forward, normal).normalized, normal);
				base.transform.rotation = rotation;
			}
		}

		// Token: 0x040051C2 RID: 20930
		[SerializeField]
		private InteractionPoint interactionPoint;

		// Token: 0x040051C3 RID: 20931
		[SerializeField]
		private Rigidbody rb;

		// Token: 0x040051C4 RID: 20932
		[SerializeField]
		private bool allowPickupFromGround = true;

		// Token: 0x040051C5 RID: 20933
		[SerializeField]
		private LayerMask floorLayerMask;

		// Token: 0x040051C6 RID: 20934
		[SerializeField]
		private float placementOffset;

		// Token: 0x040051C7 RID: 20935
		[SerializeField]
		private Transform raycastOrigin;

		// Token: 0x040051C8 RID: 20936
		[SerializeField]
		private float autoPickupAfterSeconds;

		// Token: 0x040051C9 RID: 20937
		public UnityEvent OnPickup;

		// Token: 0x040051CA RID: 20938
		public UnityEvent OnPlaced;

		// Token: 0x040051CB RID: 20939
		private bool placedOnFloor;

		// Token: 0x040051CC RID: 20940
		private float placedOnFloorTime = -1f;

		// Token: 0x040051CD RID: 20941
		private VRRig cachedLocalRig;

		// Token: 0x040051CE RID: 20942
		private HoldableObject holdableParent;

		// Token: 0x040051CF RID: 20943
		private double throwSettledTime = -1.0;

		// Token: 0x040051D0 RID: 20944
		private int landingSide;

		// Token: 0x040051D1 RID: 20945
		private float scale;
	}
}
