using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C36 RID: 3126
	public class ControllerButtonEvent : MonoBehaviour, ISpawnable
	{
		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06004E13 RID: 19987 RVA: 0x0017F126 File Offset: 0x0017D326
		// (set) Token: 0x06004E14 RID: 19988 RVA: 0x0017F12E File Offset: 0x0017D32E
		public bool IsSpawned { get; set; }

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06004E15 RID: 19989 RVA: 0x0017F137 File Offset: 0x0017D337
		// (set) Token: 0x06004E16 RID: 19990 RVA: 0x0017F13F File Offset: 0x0017D33F
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004E17 RID: 19991 RVA: 0x0017F148 File Offset: 0x0017D348
		public void OnSpawn(VRRig rig)
		{
			this.myRig = rig;
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnDespawn()
		{
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x0017F151 File Offset: 0x0017D351
		private bool IsMyItem()
		{
			return this.myRig != null && this.myRig.isOfflineVRRig;
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x0017F16E File Offset: 0x0017D36E
		private void Awake()
		{
			this.triggerLastValue = 0f;
			this.gripLastValue = 0f;
			this.primaryLastValue = false;
			this.secondaryLastValue = false;
			this.frameCounter = 0;
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x0017F19C File Offset: 0x0017D39C
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

		// Token: 0x0400513A RID: 20794
		[SerializeField]
		private float gripValue = 0.75f;

		// Token: 0x0400513B RID: 20795
		[SerializeField]
		private float gripReleaseValue = 0.01f;

		// Token: 0x0400513C RID: 20796
		[SerializeField]
		private float triggerValue = 0.75f;

		// Token: 0x0400513D RID: 20797
		[SerializeField]
		private float triggerReleaseValue = 0.01f;

		// Token: 0x0400513E RID: 20798
		[SerializeField]
		private ControllerButtonEvent.ButtonType buttonType;

		// Token: 0x0400513F RID: 20799
		[Tooltip("How many frames should pass to trigger a press stayed button")]
		[SerializeField]
		private int frameInterval = 20;

		// Token: 0x04005140 RID: 20800
		public UnityEvent<bool, float> onButtonPressed;

		// Token: 0x04005141 RID: 20801
		public UnityEvent<bool, float> onButtonReleased;

		// Token: 0x04005142 RID: 20802
		public UnityEvent<bool, float> onButtonPressStayed;

		// Token: 0x04005143 RID: 20803
		private float triggerLastValue;

		// Token: 0x04005144 RID: 20804
		private float gripLastValue;

		// Token: 0x04005145 RID: 20805
		private bool primaryLastValue;

		// Token: 0x04005146 RID: 20806
		private bool secondaryLastValue;

		// Token: 0x04005147 RID: 20807
		private int frameCounter;

		// Token: 0x04005148 RID: 20808
		private bool inLeftHand;

		// Token: 0x04005149 RID: 20809
		private VRRig myRig;

		// Token: 0x02000C37 RID: 3127
		private enum ButtonType
		{
			// Token: 0x0400514D RID: 20813
			trigger,
			// Token: 0x0400514E RID: 20814
			primary,
			// Token: 0x0400514F RID: 20815
			secondary,
			// Token: 0x04005150 RID: 20816
			grip
		}
	}
}
