using System;
using GorillaExtensions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C8B RID: 3211
	public class StickyCosmetic : MonoBehaviour
	{
		// Token: 0x06005030 RID: 20528 RVA: 0x000645DE File Offset: 0x000627DE
		private void Start()
		{
			this.endRigidbody.isKinematic = false;
			this.endRigidbody.useGravity = false;
			this.UpdateState(StickyCosmetic.ObjectState.Idle);
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x000645FF File Offset: 0x000627FF
		public void Extend()
		{
			if (this.currentState == StickyCosmetic.ObjectState.Idle || this.currentState == StickyCosmetic.ObjectState.Extending)
			{
				this.UpdateState(StickyCosmetic.ObjectState.Extending);
			}
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x00064619 File Offset: 0x00062819
		public void Retract()
		{
			this.UpdateState(StickyCosmetic.ObjectState.Retracting);
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x001BB104 File Offset: 0x001B9304
		private void Extend_Internal()
		{
			if (this.endRigidbody.isKinematic)
			{
				return;
			}
			this.rayLength = Mathf.Lerp(0f, this.maxObjectLength, this.blendShapeCosmetic.GetBlendValue() / this.blendShapeCosmetic.maxBlendShapeWeight);
			this.endRigidbody.MovePosition(this.startPosition.position + this.startPosition.forward * this.rayLength);
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x001BB180 File Offset: 0x001B9380
		private void Retract_Internal()
		{
			this.endRigidbody.isKinematic = false;
			Vector3 position = Vector3.MoveTowards(this.endRigidbody.position, this.startPosition.position, this.retractSpeed * Time.fixedDeltaTime);
			this.endRigidbody.MovePosition(position);
		}

		// Token: 0x06005035 RID: 20533 RVA: 0x001BB1D0 File Offset: 0x001B93D0
		private void FixedUpdate()
		{
			switch (this.currentState)
			{
			case StickyCosmetic.ObjectState.Extending:
			{
				if (Time.time - this.extendingStartedTime > this.retractAfterSecond)
				{
					this.UpdateState(StickyCosmetic.ObjectState.AutoRetract);
				}
				this.Extend_Internal();
				RaycastHit raycastHit;
				if (Physics.Raycast(this.rayOrigin.position, this.rayOrigin.forward, out raycastHit, this.rayLength, this.collisionLayers))
				{
					this.endRigidbody.isKinematic = true;
					this.endRigidbody.transform.parent = null;
					UnityEvent unityEvent = this.onStick;
					if (unityEvent != null)
					{
						unityEvent.Invoke();
					}
					this.UpdateState(StickyCosmetic.ObjectState.Stuck);
				}
				break;
			}
			case StickyCosmetic.ObjectState.Retracting:
				if (Vector3.Distance(this.endRigidbody.position, this.startPosition.position) <= 0.01f)
				{
					this.endRigidbody.position = this.startPosition.position;
					Transform transform = this.endRigidbody.transform;
					transform.parent = this.endPositionParent;
					transform.localRotation = quaternion.identity;
					transform.localScale = Vector3.one;
					if (this.lastState == StickyCosmetic.ObjectState.AutoUnstuck || this.lastState == StickyCosmetic.ObjectState.AutoRetract)
					{
						this.UpdateState(StickyCosmetic.ObjectState.JustRetracted);
					}
					else
					{
						this.UpdateState(StickyCosmetic.ObjectState.Idle);
					}
				}
				else
				{
					this.Retract_Internal();
				}
				break;
			case StickyCosmetic.ObjectState.Stuck:
				if (this.endRigidbody.isKinematic && (this.endRigidbody.position - this.startPosition.position).IsLongerThan(this.autoRetractThreshold))
				{
					this.UpdateState(StickyCosmetic.ObjectState.AutoUnstuck);
				}
				break;
			case StickyCosmetic.ObjectState.AutoUnstuck:
				this.UpdateState(StickyCosmetic.ObjectState.Retracting);
				break;
			case StickyCosmetic.ObjectState.AutoRetract:
				this.UpdateState(StickyCosmetic.ObjectState.Retracting);
				break;
			}
			Debug.DrawRay(this.rayOrigin.position, this.rayOrigin.forward * this.rayLength, Color.red);
		}

		// Token: 0x06005036 RID: 20534 RVA: 0x001BB3B0 File Offset: 0x001B95B0
		private void UpdateState(StickyCosmetic.ObjectState newState)
		{
			this.lastState = this.currentState;
			if (this.lastState == StickyCosmetic.ObjectState.Stuck && newState != this.currentState)
			{
				this.onUnstick.Invoke();
			}
			if (this.lastState != StickyCosmetic.ObjectState.Extending && newState == StickyCosmetic.ObjectState.Extending)
			{
				this.extendingStartedTime = Time.time;
			}
			this.currentState = newState;
		}

		// Token: 0x0400537F RID: 21375
		[SerializeField]
		private UpdateBlendShapeCosmetic blendShapeCosmetic;

		// Token: 0x04005380 RID: 21376
		[SerializeField]
		private LayerMask collisionLayers;

		// Token: 0x04005381 RID: 21377
		[SerializeField]
		private Transform rayOrigin;

		// Token: 0x04005382 RID: 21378
		[SerializeField]
		private float maxObjectLength = 0.7f;

		// Token: 0x04005383 RID: 21379
		[SerializeField]
		private float autoRetractThreshold = 1f;

		// Token: 0x04005384 RID: 21380
		[SerializeField]
		private float retractSpeed = 5f;

		// Token: 0x04005385 RID: 21381
		[Tooltip("If extended but not stuck, retract automatically after X seconds")]
		[SerializeField]
		private float retractAfterSecond = 2f;

		// Token: 0x04005386 RID: 21382
		[SerializeField]
		private Transform startPosition;

		// Token: 0x04005387 RID: 21383
		[SerializeField]
		private Rigidbody endRigidbody;

		// Token: 0x04005388 RID: 21384
		[SerializeField]
		private Transform endPositionParent;

		// Token: 0x04005389 RID: 21385
		public UnityEvent onStick;

		// Token: 0x0400538A RID: 21386
		public UnityEvent onUnstick;

		// Token: 0x0400538B RID: 21387
		private StickyCosmetic.ObjectState currentState;

		// Token: 0x0400538C RID: 21388
		private float rayLength;

		// Token: 0x0400538D RID: 21389
		private bool stick;

		// Token: 0x0400538E RID: 21390
		private StickyCosmetic.ObjectState lastState;

		// Token: 0x0400538F RID: 21391
		private float extendingStartedTime;

		// Token: 0x02000C8C RID: 3212
		private enum ObjectState
		{
			// Token: 0x04005391 RID: 21393
			Extending,
			// Token: 0x04005392 RID: 21394
			Retracting,
			// Token: 0x04005393 RID: 21395
			Stuck,
			// Token: 0x04005394 RID: 21396
			JustRetracted,
			// Token: 0x04005395 RID: 21397
			Idle,
			// Token: 0x04005396 RID: 21398
			AutoUnstuck,
			// Token: 0x04005397 RID: 21399
			AutoRetract
		}
	}
}
