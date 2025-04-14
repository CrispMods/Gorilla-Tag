using System;
using GorillaExtensions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C5A RID: 3162
	public class StickyCosmetic : MonoBehaviour
	{
		// Token: 0x06004ED0 RID: 20176 RVA: 0x00182FAF File Offset: 0x001811AF
		private void Start()
		{
			this.endRigidbody.isKinematic = false;
			this.endRigidbody.useGravity = false;
			this.UpdateState(StickyCosmetic.ObjectState.Idle);
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x00182FD0 File Offset: 0x001811D0
		public void Extend()
		{
			if (this.currentState == StickyCosmetic.ObjectState.Idle || this.currentState == StickyCosmetic.ObjectState.Extending)
			{
				this.UpdateState(StickyCosmetic.ObjectState.Extending);
			}
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x00182FEA File Offset: 0x001811EA
		public void Retract()
		{
			this.UpdateState(StickyCosmetic.ObjectState.Retracting);
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x00182FF4 File Offset: 0x001811F4
		private void Extend_Internal()
		{
			if (this.endRigidbody.isKinematic)
			{
				return;
			}
			this.rayLength = Mathf.Lerp(0f, this.maxObjectLength, this.blendShapeCosmetic.GetBlendValue() / this.blendShapeCosmetic.maxBlendShapeWeight);
			this.endRigidbody.MovePosition(this.startPosition.position + this.startPosition.forward * this.rayLength);
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x00183070 File Offset: 0x00181270
		private void Retract_Internal()
		{
			this.endRigidbody.isKinematic = false;
			Vector3 position = Vector3.MoveTowards(this.endRigidbody.position, this.startPosition.position, this.retractSpeed * Time.fixedDeltaTime);
			this.endRigidbody.MovePosition(position);
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x001830C0 File Offset: 0x001812C0
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

		// Token: 0x06004ED6 RID: 20182 RVA: 0x001832A0 File Offset: 0x001814A0
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

		// Token: 0x04005273 RID: 21107
		[SerializeField]
		private UpdateBlendShapeCosmetic blendShapeCosmetic;

		// Token: 0x04005274 RID: 21108
		[SerializeField]
		private LayerMask collisionLayers;

		// Token: 0x04005275 RID: 21109
		[SerializeField]
		private Transform rayOrigin;

		// Token: 0x04005276 RID: 21110
		[SerializeField]
		private float maxObjectLength = 0.7f;

		// Token: 0x04005277 RID: 21111
		[SerializeField]
		private float autoRetractThreshold = 1f;

		// Token: 0x04005278 RID: 21112
		[SerializeField]
		private float retractSpeed = 5f;

		// Token: 0x04005279 RID: 21113
		[Tooltip("If extended but not stuck, retract automatically after X seconds")]
		[SerializeField]
		private float retractAfterSecond = 2f;

		// Token: 0x0400527A RID: 21114
		[SerializeField]
		private Transform startPosition;

		// Token: 0x0400527B RID: 21115
		[SerializeField]
		private Rigidbody endRigidbody;

		// Token: 0x0400527C RID: 21116
		[SerializeField]
		private Transform endPositionParent;

		// Token: 0x0400527D RID: 21117
		public UnityEvent onStick;

		// Token: 0x0400527E RID: 21118
		public UnityEvent onUnstick;

		// Token: 0x0400527F RID: 21119
		private StickyCosmetic.ObjectState currentState;

		// Token: 0x04005280 RID: 21120
		private float rayLength;

		// Token: 0x04005281 RID: 21121
		private bool stick;

		// Token: 0x04005282 RID: 21122
		private StickyCosmetic.ObjectState lastState;

		// Token: 0x04005283 RID: 21123
		private float extendingStartedTime;

		// Token: 0x02000C5B RID: 3163
		private enum ObjectState
		{
			// Token: 0x04005285 RID: 21125
			Extending,
			// Token: 0x04005286 RID: 21126
			Retracting,
			// Token: 0x04005287 RID: 21127
			Stuck,
			// Token: 0x04005288 RID: 21128
			JustRetracted,
			// Token: 0x04005289 RID: 21129
			Idle,
			// Token: 0x0400528A RID: 21130
			AutoUnstuck,
			// Token: 0x0400528B RID: 21131
			AutoRetract
		}
	}
}
