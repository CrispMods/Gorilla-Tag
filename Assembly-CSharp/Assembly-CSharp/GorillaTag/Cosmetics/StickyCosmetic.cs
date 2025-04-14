using System;
using GorillaExtensions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C5D RID: 3165
	public class StickyCosmetic : MonoBehaviour
	{
		// Token: 0x06004EDC RID: 20188 RVA: 0x00183577 File Offset: 0x00181777
		private void Start()
		{
			this.endRigidbody.isKinematic = false;
			this.endRigidbody.useGravity = false;
			this.UpdateState(StickyCosmetic.ObjectState.Idle);
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x00183598 File Offset: 0x00181798
		public void Extend()
		{
			if (this.currentState == StickyCosmetic.ObjectState.Idle || this.currentState == StickyCosmetic.ObjectState.Extending)
			{
				this.UpdateState(StickyCosmetic.ObjectState.Extending);
			}
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x001835B2 File Offset: 0x001817B2
		public void Retract()
		{
			this.UpdateState(StickyCosmetic.ObjectState.Retracting);
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x001835BC File Offset: 0x001817BC
		private void Extend_Internal()
		{
			if (this.endRigidbody.isKinematic)
			{
				return;
			}
			this.rayLength = Mathf.Lerp(0f, this.maxObjectLength, this.blendShapeCosmetic.GetBlendValue() / this.blendShapeCosmetic.maxBlendShapeWeight);
			this.endRigidbody.MovePosition(this.startPosition.position + this.startPosition.forward * this.rayLength);
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x00183638 File Offset: 0x00181838
		private void Retract_Internal()
		{
			this.endRigidbody.isKinematic = false;
			Vector3 position = Vector3.MoveTowards(this.endRigidbody.position, this.startPosition.position, this.retractSpeed * Time.fixedDeltaTime);
			this.endRigidbody.MovePosition(position);
		}

		// Token: 0x06004EE1 RID: 20193 RVA: 0x00183688 File Offset: 0x00181888
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

		// Token: 0x06004EE2 RID: 20194 RVA: 0x00183868 File Offset: 0x00181A68
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

		// Token: 0x04005285 RID: 21125
		[SerializeField]
		private UpdateBlendShapeCosmetic blendShapeCosmetic;

		// Token: 0x04005286 RID: 21126
		[SerializeField]
		private LayerMask collisionLayers;

		// Token: 0x04005287 RID: 21127
		[SerializeField]
		private Transform rayOrigin;

		// Token: 0x04005288 RID: 21128
		[SerializeField]
		private float maxObjectLength = 0.7f;

		// Token: 0x04005289 RID: 21129
		[SerializeField]
		private float autoRetractThreshold = 1f;

		// Token: 0x0400528A RID: 21130
		[SerializeField]
		private float retractSpeed = 5f;

		// Token: 0x0400528B RID: 21131
		[Tooltip("If extended but not stuck, retract automatically after X seconds")]
		[SerializeField]
		private float retractAfterSecond = 2f;

		// Token: 0x0400528C RID: 21132
		[SerializeField]
		private Transform startPosition;

		// Token: 0x0400528D RID: 21133
		[SerializeField]
		private Rigidbody endRigidbody;

		// Token: 0x0400528E RID: 21134
		[SerializeField]
		private Transform endPositionParent;

		// Token: 0x0400528F RID: 21135
		public UnityEvent onStick;

		// Token: 0x04005290 RID: 21136
		public UnityEvent onUnstick;

		// Token: 0x04005291 RID: 21137
		private StickyCosmetic.ObjectState currentState;

		// Token: 0x04005292 RID: 21138
		private float rayLength;

		// Token: 0x04005293 RID: 21139
		private bool stick;

		// Token: 0x04005294 RID: 21140
		private StickyCosmetic.ObjectState lastState;

		// Token: 0x04005295 RID: 21141
		private float extendingStartedTime;

		// Token: 0x02000C5E RID: 3166
		private enum ObjectState
		{
			// Token: 0x04005297 RID: 21143
			Extending,
			// Token: 0x04005298 RID: 21144
			Retracting,
			// Token: 0x04005299 RID: 21145
			Stuck,
			// Token: 0x0400529A RID: 21146
			JustRetracted,
			// Token: 0x0400529B RID: 21147
			Idle,
			// Token: 0x0400529C RID: 21148
			AutoUnstuck,
			// Token: 0x0400529D RID: 21149
			AutoRetract
		}
	}
}
