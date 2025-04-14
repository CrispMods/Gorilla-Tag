using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C39 RID: 3129
	public class ControllerButtonEvent : MonoBehaviour, ISpawnable
	{
		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06004E1F RID: 19999 RVA: 0x0017F6EE File Offset: 0x0017D8EE
		// (set) Token: 0x06004E20 RID: 20000 RVA: 0x0017F6F6 File Offset: 0x0017D8F6
		public bool IsSpawned { get; set; }

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06004E21 RID: 20001 RVA: 0x0017F6FF File Offset: 0x0017D8FF
		// (set) Token: 0x06004E22 RID: 20002 RVA: 0x0017F707 File Offset: 0x0017D907
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004E23 RID: 20003 RVA: 0x0017F710 File Offset: 0x0017D910
		public void OnSpawn(VRRig rig)
		{
			this.myRig = rig;
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnDespawn()
		{
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x0017F719 File Offset: 0x0017D919
		private bool IsMyItem()
		{
			return this.myRig != null && this.myRig.isOfflineVRRig;
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x0017F736 File Offset: 0x0017D936
		private void Awake()
		{
			this.triggerLastValue = 0f;
			this.gripLastValue = 0f;
			this.primaryLastValue = false;
			this.secondaryLastValue = false;
			this.frameCounter = 0;
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x0017F764 File Offset: 0x0017D964
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

		// Token: 0x0400514C RID: 20812
		[SerializeField]
		private float gripValue = 0.75f;

		// Token: 0x0400514D RID: 20813
		[SerializeField]
		private float gripReleaseValue = 0.01f;

		// Token: 0x0400514E RID: 20814
		[SerializeField]
		private float triggerValue = 0.75f;

		// Token: 0x0400514F RID: 20815
		[SerializeField]
		private float triggerReleaseValue = 0.01f;

		// Token: 0x04005150 RID: 20816
		[SerializeField]
		private ControllerButtonEvent.ButtonType buttonType;

		// Token: 0x04005151 RID: 20817
		[Tooltip("How many frames should pass to trigger a press stayed button")]
		[SerializeField]
		private int frameInterval = 20;

		// Token: 0x04005152 RID: 20818
		public UnityEvent<bool, float> onButtonPressed;

		// Token: 0x04005153 RID: 20819
		public UnityEvent<bool, float> onButtonReleased;

		// Token: 0x04005154 RID: 20820
		public UnityEvent<bool, float> onButtonPressStayed;

		// Token: 0x04005155 RID: 20821
		private float triggerLastValue;

		// Token: 0x04005156 RID: 20822
		private float gripLastValue;

		// Token: 0x04005157 RID: 20823
		private bool primaryLastValue;

		// Token: 0x04005158 RID: 20824
		private bool secondaryLastValue;

		// Token: 0x04005159 RID: 20825
		private int frameCounter;

		// Token: 0x0400515A RID: 20826
		private bool inLeftHand;

		// Token: 0x0400515B RID: 20827
		private VRRig myRig;

		// Token: 0x02000C3A RID: 3130
		private enum ButtonType
		{
			// Token: 0x0400515F RID: 20831
			trigger,
			// Token: 0x04005160 RID: 20832
			primary,
			// Token: 0x04005161 RID: 20833
			secondary,
			// Token: 0x04005162 RID: 20834
			grip
		}
	}
}
