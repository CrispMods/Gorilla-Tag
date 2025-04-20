using System;
using System.Collections.Generic;
using GorillaLocomotion;
using Valve.VR;

namespace UnityEngine.XR.Interaction.Toolkit
{
	// Token: 0x02000A45 RID: 2629
	public class GorillaSnapTurn : LocomotionProvider
	{
		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060041F3 RID: 16883 RVA: 0x0005B258 File Offset: 0x00059458
		// (set) Token: 0x060041F4 RID: 16884 RVA: 0x0005B260 File Offset: 0x00059460
		public GorillaSnapTurn.InputAxes turnUsage
		{
			get
			{
				return this.m_TurnUsage;
			}
			set
			{
				this.m_TurnUsage = value;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x060041F5 RID: 16885 RVA: 0x0005B269 File Offset: 0x00059469
		// (set) Token: 0x060041F6 RID: 16886 RVA: 0x0005B271 File Offset: 0x00059471
		public List<XRController> controllers
		{
			get
			{
				return this.m_Controllers;
			}
			set
			{
				this.m_Controllers = value;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060041F7 RID: 16887 RVA: 0x0005B27A File Offset: 0x0005947A
		// (set) Token: 0x060041F8 RID: 16888 RVA: 0x0005B282 File Offset: 0x00059482
		public float turnAmount
		{
			get
			{
				return this.m_TurnAmount;
			}
			set
			{
				this.m_TurnAmount = value;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060041F9 RID: 16889 RVA: 0x0005B28B File Offset: 0x0005948B
		// (set) Token: 0x060041FA RID: 16890 RVA: 0x0005B293 File Offset: 0x00059493
		public float debounceTime
		{
			get
			{
				return this.m_DebounceTime;
			}
			set
			{
				this.m_DebounceTime = value;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060041FB RID: 16891 RVA: 0x0005B29C File Offset: 0x0005949C
		// (set) Token: 0x060041FC RID: 16892 RVA: 0x0005B2A4 File Offset: 0x000594A4
		public float deadZone
		{
			get
			{
				return this.m_DeadZone;
			}
			set
			{
				this.m_DeadZone = value;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060041FD RID: 16893 RVA: 0x0005B2AD File Offset: 0x000594AD
		// (set) Token: 0x060041FE RID: 16894 RVA: 0x0005B2B5 File Offset: 0x000594B5
		public string turnType
		{
			get
			{
				return this.m_TurnType;
			}
			private set
			{
				this.m_TurnType = value;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060041FF RID: 16895 RVA: 0x0005B2BE File Offset: 0x000594BE
		// (set) Token: 0x06004200 RID: 16896 RVA: 0x0005B2C6 File Offset: 0x000594C6
		public int turnFactor
		{
			get
			{
				return this.m_TurnFactor;
			}
			private set
			{
				this.m_TurnFactor = value;
			}
		}

		// Token: 0x06004201 RID: 16897 RVA: 0x00173DC0 File Offset: 0x00171FC0
		private void Update()
		{
			this.ValidateTurningOverriders();
			if (this.m_Controllers.Count > 0)
			{
				this.EnsureControllerDataListSize();
				InputFeatureUsage<Vector2>[] vec2UsageList = GorillaSnapTurn.m_Vec2UsageList;
				GorillaSnapTurn.InputAxes turnUsage = this.m_TurnUsage;
				for (int i = 0; i < this.m_Controllers.Count; i++)
				{
					XRController xrcontroller = this.m_Controllers[i];
					if (xrcontroller != null && xrcontroller.enableInputActions)
					{
						InputDevice inputDevice = xrcontroller.inputDevice;
						Vector2 vector = (xrcontroller.controllerNode == XRNode.LeftHand) ? SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.GetAxis(SteamVR_Input_Sources.LeftHand) : SteamVR_Actions.gorillaTag_RightJoystick2DAxis.GetAxis(SteamVR_Input_Sources.RightHand);
						if (vector.x > this.deadZone)
						{
							this.StartTurn(this.m_TurnAmount);
						}
						else if (vector.x < -this.deadZone)
						{
							this.StartTurn(-this.m_TurnAmount);
						}
						else
						{
							this.m_AxisReset = true;
						}
					}
				}
			}
			if (Math.Abs(this.m_CurrentTurnAmount) > 0f && base.BeginLocomotion())
			{
				if (base.system.xrRig != null)
				{
					GTPlayer.Instance.Turn(this.m_CurrentTurnAmount);
				}
				this.m_CurrentTurnAmount = 0f;
				base.EndLocomotion();
			}
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x00173EF0 File Offset: 0x001720F0
		private void EnsureControllerDataListSize()
		{
			if (this.m_Controllers.Count != this.m_ControllersWereActive.Count)
			{
				while (this.m_ControllersWereActive.Count < this.m_Controllers.Count)
				{
					this.m_ControllersWereActive.Add(false);
				}
				while (this.m_ControllersWereActive.Count < this.m_Controllers.Count)
				{
					this.m_ControllersWereActive.RemoveAt(this.m_ControllersWereActive.Count - 1);
				}
			}
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x0005B2CF File Offset: 0x000594CF
		internal void FakeStartTurn(bool isLeft)
		{
			this.StartTurn(isLeft ? (-this.m_TurnAmount) : this.m_TurnAmount);
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x00173F70 File Offset: 0x00172170
		private void StartTurn(float amount)
		{
			if (this.m_TimeStarted + this.m_DebounceTime > Time.time && !this.m_AxisReset)
			{
				return;
			}
			if (!base.CanBeginLocomotion())
			{
				return;
			}
			if (this.turningOverriders.Count > 0)
			{
				return;
			}
			this.m_TimeStarted = Time.time;
			this.m_CurrentTurnAmount = amount;
			this.m_AxisReset = false;
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x00173FCC File Offset: 0x001721CC
		public void ChangeTurnMode(string turnMode, int turnSpeedFactor)
		{
			this.turnType = turnMode;
			this.turnFactor = turnSpeedFactor;
			if (turnMode == "SNAP")
			{
				this.m_DebounceTime = 0.5f;
				this.m_TurnAmount = 60f * this.ConvertedTurnFactor((float)turnSpeedFactor);
				return;
			}
			if (!(turnMode == "SMOOTH"))
			{
				this.m_DebounceTime = 0f;
				this.m_TurnAmount = 0f;
				return;
			}
			this.m_DebounceTime = 0f;
			this.m_TurnAmount = 360f * Time.fixedDeltaTime * this.ConvertedTurnFactor((float)turnSpeedFactor);
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x0005B2E9 File Offset: 0x000594E9
		public float ConvertedTurnFactor(float newTurnSpeed)
		{
			return Mathf.Max(0.75f, 0.5f + newTurnSpeed / 10f * 1.5f);
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x0005B308 File Offset: 0x00059508
		public void SetTurningOverride(ISnapTurnOverride caller)
		{
			if (!this.turningOverriders.Contains(caller))
			{
				this.turningOverriders.Add(caller);
			}
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x0005B325 File Offset: 0x00059525
		public void UnsetTurningOverride(ISnapTurnOverride caller)
		{
			if (this.turningOverriders.Contains(caller))
			{
				this.turningOverriders.Remove(caller);
			}
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x00174060 File Offset: 0x00172260
		public void ValidateTurningOverriders()
		{
			foreach (ISnapTurnOverride snapTurnOverride in this.turningOverriders)
			{
				if (snapTurnOverride == null || !snapTurnOverride.TurnOverrideActive())
				{
					this.turningOverriders.Remove(snapTurnOverride);
				}
			}
		}

		// Token: 0x040042DC RID: 17116
		private static readonly InputFeatureUsage<Vector2>[] m_Vec2UsageList = new InputFeatureUsage<Vector2>[]
		{
			CommonUsages.primary2DAxis,
			CommonUsages.secondary2DAxis
		};

		// Token: 0x040042DD RID: 17117
		[SerializeField]
		[Tooltip("The 2D Input Axis on the primary devices that will be used to trigger a snap turn.")]
		private GorillaSnapTurn.InputAxes m_TurnUsage;

		// Token: 0x040042DE RID: 17118
		[SerializeField]
		[Tooltip("A list of controllers that allow Snap Turn.  If an XRController is not enabled, or does not have input actions enabled.  Snap Turn will not work.")]
		private List<XRController> m_Controllers = new List<XRController>();

		// Token: 0x040042DF RID: 17119
		[SerializeField]
		[Tooltip("The number of degrees clockwise to rotate when snap turning clockwise.")]
		private float m_TurnAmount = 45f;

		// Token: 0x040042E0 RID: 17120
		[SerializeField]
		[Tooltip("The amount of time that the system will wait before starting another snap turn.")]
		private float m_DebounceTime = 0.5f;

		// Token: 0x040042E1 RID: 17121
		[SerializeField]
		[Tooltip("The deadzone that the controller movement will have to be above to trigger a snap turn.")]
		private float m_DeadZone = 0.75f;

		// Token: 0x040042E2 RID: 17122
		private float m_CurrentTurnAmount;

		// Token: 0x040042E3 RID: 17123
		private float m_TimeStarted;

		// Token: 0x040042E4 RID: 17124
		private bool m_AxisReset;

		// Token: 0x040042E5 RID: 17125
		public float turnSpeed = 1f;

		// Token: 0x040042E6 RID: 17126
		private HashSet<ISnapTurnOverride> turningOverriders = new HashSet<ISnapTurnOverride>();

		// Token: 0x040042E7 RID: 17127
		private List<bool> m_ControllersWereActive = new List<bool>();

		// Token: 0x040042E8 RID: 17128
		private string m_TurnType = "";

		// Token: 0x040042E9 RID: 17129
		private int m_TurnFactor = 1;

		// Token: 0x02000A46 RID: 2630
		public enum InputAxes
		{
			// Token: 0x040042EB RID: 17131
			Primary2DAxis,
			// Token: 0x040042EC RID: 17132
			Secondary2DAxis
		}
	}
}
