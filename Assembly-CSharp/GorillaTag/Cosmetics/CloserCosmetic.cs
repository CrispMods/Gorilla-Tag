using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C34 RID: 3124
	public class CloserCosmetic : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06004E08 RID: 19976 RVA: 0x0017EED9 File Offset: 0x0017D0D9
		// (set) Token: 0x06004E09 RID: 19977 RVA: 0x0017EEE1 File Offset: 0x0017D0E1
		public bool TickRunning { get; set; }

		// Token: 0x06004E0A RID: 19978 RVA: 0x0017EEEC File Offset: 0x0017D0EC
		private void OnEnable()
		{
			TickSystem<object>.AddCallbackTarget(this);
			this.localRotA = this.sideA.transform.localRotation;
			this.localRotB = this.sideB.transform.localRotation;
			this.fingerValue = 0f;
			this.UpdateState(CloserCosmetic.State.Opening);
		}

		// Token: 0x06004E0B RID: 19979 RVA: 0x00019B13 File Offset: 0x00017D13
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
		}

		// Token: 0x06004E0C RID: 19980 RVA: 0x0017EF40 File Offset: 0x0017D140
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

		// Token: 0x06004E0D RID: 19981 RVA: 0x0017EF74 File Offset: 0x0017D174
		public void Close(bool leftHand, float fingerFlexValue)
		{
			this.UpdateState(CloserCosmetic.State.Closing);
			this.fingerValue = fingerFlexValue;
		}

		// Token: 0x06004E0E RID: 19982 RVA: 0x0017EF84 File Offset: 0x0017D184
		public void Open(bool leftHand, float fingerFlexValue)
		{
			this.UpdateState(CloserCosmetic.State.Opening);
			this.fingerValue = fingerFlexValue;
		}

		// Token: 0x06004E0F RID: 19983 RVA: 0x0017EF94 File Offset: 0x0017D194
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

		// Token: 0x06004E10 RID: 19984 RVA: 0x0017F058 File Offset: 0x0017D258
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

		// Token: 0x06004E11 RID: 19985 RVA: 0x0017F11D File Offset: 0x0017D31D
		private void UpdateState(CloserCosmetic.State newState)
		{
			this.currentState = newState;
		}

		// Token: 0x0400512C RID: 20780
		[SerializeField]
		private GameObject sideA;

		// Token: 0x0400512D RID: 20781
		[SerializeField]
		private GameObject sideB;

		// Token: 0x0400512E RID: 20782
		[SerializeField]
		private Vector3 maxRotationA;

		// Token: 0x0400512F RID: 20783
		[SerializeField]
		private Vector3 maxRotationB;

		// Token: 0x04005130 RID: 20784
		[SerializeField]
		private bool useFingerFlexValueAsStrength;

		// Token: 0x04005131 RID: 20785
		private Quaternion localRotA;

		// Token: 0x04005132 RID: 20786
		private Quaternion localRotB;

		// Token: 0x04005133 RID: 20787
		private CloserCosmetic.State currentState;

		// Token: 0x04005134 RID: 20788
		private float fingerValue;

		// Token: 0x02000C35 RID: 3125
		private enum State
		{
			// Token: 0x04005137 RID: 20791
			Closing,
			// Token: 0x04005138 RID: 20792
			Opening,
			// Token: 0x04005139 RID: 20793
			None
		}
	}
}
