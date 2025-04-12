using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C37 RID: 3127
	public class CloserCosmetic : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06004E14 RID: 19988 RVA: 0x00062391 File Offset: 0x00060591
		// (set) Token: 0x06004E15 RID: 19989 RVA: 0x00062399 File Offset: 0x00060599
		public bool TickRunning { get; set; }

		// Token: 0x06004E16 RID: 19990 RVA: 0x001AF774 File Offset: 0x001AD974
		private void OnEnable()
		{
			TickSystem<object>.AddCallbackTarget(this);
			this.localRotA = this.sideA.transform.localRotation;
			this.localRotB = this.sideB.transform.localRotation;
			this.fingerValue = 0f;
			this.UpdateState(CloserCosmetic.State.Opening);
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x00032484 File Offset: 0x00030684
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x001AF7C8 File Offset: 0x001AD9C8
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

		// Token: 0x06004E19 RID: 19993 RVA: 0x000623A2 File Offset: 0x000605A2
		public void Close(bool leftHand, float fingerFlexValue)
		{
			this.UpdateState(CloserCosmetic.State.Closing);
			this.fingerValue = fingerFlexValue;
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x000623B2 File Offset: 0x000605B2
		public void Open(bool leftHand, float fingerFlexValue)
		{
			this.UpdateState(CloserCosmetic.State.Opening);
			this.fingerValue = fingerFlexValue;
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x001AF7FC File Offset: 0x001AD9FC
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

		// Token: 0x06004E1C RID: 19996 RVA: 0x001AF8C0 File Offset: 0x001ADAC0
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

		// Token: 0x06004E1D RID: 19997 RVA: 0x000623C2 File Offset: 0x000605C2
		private void UpdateState(CloserCosmetic.State newState)
		{
			this.currentState = newState;
		}

		// Token: 0x0400513E RID: 20798
		[SerializeField]
		private GameObject sideA;

		// Token: 0x0400513F RID: 20799
		[SerializeField]
		private GameObject sideB;

		// Token: 0x04005140 RID: 20800
		[SerializeField]
		private Vector3 maxRotationA;

		// Token: 0x04005141 RID: 20801
		[SerializeField]
		private Vector3 maxRotationB;

		// Token: 0x04005142 RID: 20802
		[SerializeField]
		private bool useFingerFlexValueAsStrength;

		// Token: 0x04005143 RID: 20803
		private Quaternion localRotA;

		// Token: 0x04005144 RID: 20804
		private Quaternion localRotB;

		// Token: 0x04005145 RID: 20805
		private CloserCosmetic.State currentState;

		// Token: 0x04005146 RID: 20806
		private float fingerValue;

		// Token: 0x02000C38 RID: 3128
		private enum State
		{
			// Token: 0x04005149 RID: 20809
			Closing,
			// Token: 0x0400514A RID: 20810
			Opening,
			// Token: 0x0400514B RID: 20811
			None
		}
	}
}
