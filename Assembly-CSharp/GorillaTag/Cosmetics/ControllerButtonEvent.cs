using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C67 RID: 3175
	public class ControllerButtonEvent : MonoBehaviour, ISpawnable
	{
		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06004F73 RID: 20339 RVA: 0x00063DF0 File Offset: 0x00061FF0
		// (set) Token: 0x06004F74 RID: 20340 RVA: 0x00063DF8 File Offset: 0x00061FF8
		public bool IsSpawned { get; set; }

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06004F75 RID: 20341 RVA: 0x00063E01 File Offset: 0x00062001
		// (set) Token: 0x06004F76 RID: 20342 RVA: 0x00063E09 File Offset: 0x00062009
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004F77 RID: 20343 RVA: 0x00063E12 File Offset: 0x00062012
		public void OnSpawn(VRRig rig)
		{
			this.myRig = rig;
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnDespawn()
		{
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x00063E1B File Offset: 0x0006201B
		private bool IsMyItem()
		{
			return this.myRig != null && this.myRig.isOfflineVRRig;
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x00063E38 File Offset: 0x00062038
		private void Awake()
		{
			this.triggerLastValue = 0f;
			this.gripLastValue = 0f;
			this.primaryLastValue = false;
			this.secondaryLastValue = false;
			this.frameCounter = 0;
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x001B7A6C File Offset: 0x001B5C6C
		public void LateUpdate()
		{
			if (!this.IsMyItem())
			{
				return;
			}
			XRNode node = this.inLeftHand ? XRNode.LeftHand : XRNode.RightHand;
			switch (this.buttonType)
			{
			case ControllerButtonEvent.ButtonType.trigger:
			{
				float num = ControllerInputPoller.TriggerFloat(node);
				if (num > this.triggerValue)
				{
					this.frameCounter++;
				}
				if (num > this.triggerValue && this.triggerLastValue < this.triggerValue)
				{
					UnityEvent<bool, float> unityEvent = this.onButtonPressed;
					if (unityEvent != null)
					{
						unityEvent.Invoke(this.inLeftHand, num);
					}
				}
				else if (num <= this.triggerReleaseValue && this.triggerLastValue > this.triggerReleaseValue)
				{
					UnityEvent<bool, float> unityEvent2 = this.onButtonReleased;
					if (unityEvent2 != null)
					{
						unityEvent2.Invoke(this.inLeftHand, num);
					}
					this.frameCounter = 0;
				}
				else if (num > this.triggerValue && this.triggerLastValue >= this.triggerValue && this.frameCounter % this.frameInterval == 0)
				{
					UnityEvent<bool, float> unityEvent3 = this.onButtonPressStayed;
					if (unityEvent3 != null)
					{
						unityEvent3.Invoke(this.inLeftHand, num);
					}
					this.frameCounter = 0;
				}
				this.triggerLastValue = num;
				return;
			}
			case ControllerButtonEvent.ButtonType.primary:
			{
				bool flag = ControllerInputPoller.PrimaryButtonPress(node);
				if (flag)
				{
					this.frameCounter++;
				}
				if (flag && !this.primaryLastValue)
				{
					UnityEvent<bool, float> unityEvent4 = this.onButtonPressed;
					if (unityEvent4 != null)
					{
						unityEvent4.Invoke(this.inLeftHand, 1f);
					}
				}
				else if (!flag && this.primaryLastValue)
				{
					UnityEvent<bool, float> unityEvent5 = this.onButtonReleased;
					if (unityEvent5 != null)
					{
						unityEvent5.Invoke(this.inLeftHand, 0f);
					}
					this.frameCounter = 0;
				}
				else if (flag && this.primaryLastValue && this.frameCounter % this.frameInterval == 0)
				{
					UnityEvent<bool, float> unityEvent6 = this.onButtonPressStayed;
					if (unityEvent6 != null)
					{
						unityEvent6.Invoke(this.inLeftHand, 1f);
					}
					this.frameCounter = 0;
				}
				this.primaryLastValue = flag;
				return;
			}
			case ControllerButtonEvent.ButtonType.secondary:
			{
				bool flag2 = ControllerInputPoller.SecondaryButtonPress(node);
				if (flag2)
				{
					this.frameCounter++;
				}
				if (flag2 && !this.secondaryLastValue)
				{
					UnityEvent<bool, float> unityEvent7 = this.onButtonPressed;
					if (unityEvent7 != null)
					{
						unityEvent7.Invoke(this.inLeftHand, 1f);
					}
				}
				else if (!flag2 && this.secondaryLastValue)
				{
					UnityEvent<bool, float> unityEvent8 = this.onButtonReleased;
					if (unityEvent8 != null)
					{
						unityEvent8.Invoke(this.inLeftHand, 0f);
					}
					this.frameCounter = 0;
				}
				else if (flag2 && this.secondaryLastValue && this.frameCounter % this.frameInterval == 0)
				{
					UnityEvent<bool, float> unityEvent9 = this.onButtonPressStayed;
					if (unityEvent9 != null)
					{
						unityEvent9.Invoke(this.inLeftHand, 1f);
					}
					this.frameCounter = 0;
				}
				this.secondaryLastValue = flag2;
				return;
			}
			case ControllerButtonEvent.ButtonType.grip:
			{
				float num2 = ControllerInputPoller.GripFloat(node);
				if (num2 > this.gripValue)
				{
					this.frameCounter++;
				}
				if (num2 > this.gripValue && this.gripLastValue < this.gripValue)
				{
					UnityEvent<bool, float> unityEvent10 = this.onButtonPressed;
					if (unityEvent10 != null)
					{
						unityEvent10.Invoke(this.inLeftHand, num2);
					}
				}
				else if (num2 <= this.gripReleaseValue && this.gripLastValue > this.gripReleaseValue)
				{
					UnityEvent<bool, float> unityEvent11 = this.onButtonReleased;
					if (unityEvent11 != null)
					{
						unityEvent11.Invoke(this.inLeftHand, num2);
					}
					this.frameCounter = 0;
				}
				else if (num2 > this.gripValue && this.gripLastValue >= this.gripValue && this.frameCounter % this.frameInterval == 0)
				{
					UnityEvent<bool, float> unityEvent12 = this.onButtonPressStayed;
					if (unityEvent12 != null)
					{
						unityEvent12.Invoke(this.inLeftHand, num2);
					}
					this.frameCounter = 0;
				}
				this.gripLastValue = num2;
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x04005246 RID: 21062
		[SerializeField]
		private float gripValue = 0.75f;

		// Token: 0x04005247 RID: 21063
		[SerializeField]
		private float gripReleaseValue = 0.01f;

		// Token: 0x04005248 RID: 21064
		[SerializeField]
		private float triggerValue = 0.75f;

		// Token: 0x04005249 RID: 21065
		[SerializeField]
		private float triggerReleaseValue = 0.01f;

		// Token: 0x0400524A RID: 21066
		[SerializeField]
		private ControllerButtonEvent.ButtonType buttonType;

		// Token: 0x0400524B RID: 21067
		[Tooltip("How many frames should pass to trigger a press stayed button")]
		[SerializeField]
		private int frameInterval = 20;

		// Token: 0x0400524C RID: 21068
		public UnityEvent<bool, float> onButtonPressed;

		// Token: 0x0400524D RID: 21069
		public UnityEvent<bool, float> onButtonReleased;

		// Token: 0x0400524E RID: 21070
		public UnityEvent<bool, float> onButtonPressStayed;

		// Token: 0x0400524F RID: 21071
		private float triggerLastValue;

		// Token: 0x04005250 RID: 21072
		private float gripLastValue;

		// Token: 0x04005251 RID: 21073
		private bool primaryLastValue;

		// Token: 0x04005252 RID: 21074
		private bool secondaryLastValue;

		// Token: 0x04005253 RID: 21075
		private int frameCounter;

		// Token: 0x04005254 RID: 21076
		private bool inLeftHand;

		// Token: 0x04005255 RID: 21077
		private VRRig myRig;

		// Token: 0x02000C68 RID: 3176
		private enum ButtonType
		{
			// Token: 0x04005259 RID: 21081
			trigger,
			// Token: 0x0400525A RID: 21082
			primary,
			// Token: 0x0400525B RID: 21083
			secondary,
			// Token: 0x0400525C RID: 21084
			grip
		}
	}
}
