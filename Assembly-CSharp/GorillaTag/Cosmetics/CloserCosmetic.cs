using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C65 RID: 3173
	public class CloserCosmetic : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06004F68 RID: 20328 RVA: 0x00063DB6 File Offset: 0x00061FB6
		// (set) Token: 0x06004F69 RID: 20329 RVA: 0x00063DBE File Offset: 0x00061FBE
		public bool TickRunning { get; set; }

		// Token: 0x06004F6A RID: 20330 RVA: 0x001B7858 File Offset: 0x001B5A58
		private void OnEnable()
		{
			TickSystem<object>.AddCallbackTarget(this);
			this.localRotA = this.sideA.transform.localRotation;
			this.localRotB = this.sideB.transform.localRotation;
			this.fingerValue = 0f;
			this.UpdateState(CloserCosmetic.State.Opening);
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x0003368B File Offset: 0x0003188B
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x001B78AC File Offset: 0x001B5AAC
		public void Tick()
		{
			switch (this.currentState)
			{
			case CloserCosmetic.State.Closing:
				this.Closing();
				return;
			case CloserCosmetic.State.Opening:
				this.Opening();
				break;
			case CloserCosmetic.State.None:
				break;
			default:
				return;
			}
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x00063DC7 File Offset: 0x00061FC7
		public void Close(bool leftHand, float fingerFlexValue)
		{
			this.UpdateState(CloserCosmetic.State.Closing);
			this.fingerValue = fingerFlexValue;
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x00063DD7 File Offset: 0x00061FD7
		public void Open(bool leftHand, float fingerFlexValue)
		{
			this.UpdateState(CloserCosmetic.State.Opening);
			this.fingerValue = fingerFlexValue;
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x001B78E0 File Offset: 0x001B5AE0
		private void Closing()
		{
			float t = this.useFingerFlexValueAsStrength ? Mathf.Clamp01(this.fingerValue) : 1f;
			Quaternion b = Quaternion.Euler(this.maxRotationB);
			Quaternion quaternion = Quaternion.Slerp(this.localRotB, b, t);
			this.sideB.transform.localRotation = quaternion;
			Quaternion b2 = Quaternion.Euler(this.maxRotationA);
			Quaternion quaternion2 = Quaternion.Slerp(this.localRotA, b2, t);
			this.sideA.transform.localRotation = quaternion2;
			if (Quaternion.Angle(this.sideB.transform.localRotation, quaternion) < 0.1f && Quaternion.Angle(this.sideA.transform.localRotation, quaternion2) < 0.1f)
			{
				this.UpdateState(CloserCosmetic.State.None);
			}
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x001B79A4 File Offset: 0x001B5BA4
		private void Opening()
		{
			float t = this.useFingerFlexValueAsStrength ? Mathf.Clamp01(this.fingerValue) : 1f;
			Quaternion quaternion = Quaternion.Slerp(this.sideB.transform.localRotation, this.localRotB, t);
			this.sideB.transform.localRotation = quaternion;
			Quaternion quaternion2 = Quaternion.Slerp(this.sideA.transform.localRotation, this.localRotA, t);
			this.sideA.transform.localRotation = quaternion2;
			if (Quaternion.Angle(this.sideB.transform.localRotation, quaternion) < 0.1f && Quaternion.Angle(this.sideA.transform.localRotation, quaternion2) < 0.1f)
			{
				this.UpdateState(CloserCosmetic.State.None);
			}
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x00063DE7 File Offset: 0x00061FE7
		private void UpdateState(CloserCosmetic.State newState)
		{
			this.currentState = newState;
		}

		// Token: 0x04005238 RID: 21048
		[SerializeField]
		private GameObject sideA;

		// Token: 0x04005239 RID: 21049
		[SerializeField]
		private GameObject sideB;

		// Token: 0x0400523A RID: 21050
		[SerializeField]
		private Vector3 maxRotationA;

		// Token: 0x0400523B RID: 21051
		[SerializeField]
		private Vector3 maxRotationB;

		// Token: 0x0400523C RID: 21052
		[SerializeField]
		private bool useFingerFlexValueAsStrength;

		// Token: 0x0400523D RID: 21053
		private Quaternion localRotA;

		// Token: 0x0400523E RID: 21054
		private Quaternion localRotB;

		// Token: 0x0400523F RID: 21055
		private CloserCosmetic.State currentState;

		// Token: 0x04005240 RID: 21056
		private float fingerValue;

		// Token: 0x02000C66 RID: 3174
		private enum State
		{
			// Token: 0x04005243 RID: 21059
			Closing,
			// Token: 0x04005244 RID: 21060
			Opening,
			// Token: 0x04005245 RID: 21061
			None
		}
	}
}
