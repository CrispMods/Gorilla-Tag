using System;
using System.Collections.Generic;
using GorillaLocomotion;
using Valve.VR;

namespace UnityEngine.XR.Interaction.Toolkit
{
	// Token: 0x02000A1B RID: 2587
	public class GorillaSnapTurn : LocomotionProvider
	{
		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060040BA RID: 16570 RVA: 0x00133D9B File Offset: 0x00131F9B
		// (set) Token: 0x060040BB RID: 16571 RVA: 0x00133DA3 File Offset: 0x00131FA3
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

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060040BC RID: 16572 RVA: 0x00133DAC File Offset: 0x00131FAC
		// (set) Token: 0x060040BD RID: 16573 RVA: 0x00133DB4 File Offset: 0x00131FB4
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

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060040BE RID: 16574 RVA: 0x00133DBD File Offset: 0x00131FBD
		// (set) Token: 0x060040BF RID: 16575 RVA: 0x00133DC5 File Offset: 0x00131FC5
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

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060040C0 RID: 16576 RVA: 0x00133DCE File Offset: 0x00131FCE
		// (set) Token: 0x060040C1 RID: 16577 RVA: 0x00133DD6 File Offset: 0x00131FD6
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

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060040C2 RID: 16578 RVA: 0x00133DDF File Offset: 0x00131FDF
		// (set) Token: 0x060040C3 RID: 16579 RVA: 0x00133DE7 File Offset: 0x00131FE7
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

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x00133DF0 File Offset: 0x00131FF0
		// (set) Token: 0x060040C5 RID: 16581 RVA: 0x00133DF8 File Offset: 0x00131FF8
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

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060040C6 RID: 16582 RVA: 0x00133E01 File Offset: 0x00132001
		// (set) Token: 0x060040C7 RID: 16583 RVA: 0x00133E09 File Offset: 0x00132009
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

		// Token: 0x060040C8 RID: 16584 RVA: 0x00133E14 File Offset: 0x00132014
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

		// Token: 0x060040C9 RID: 16585 RVA: 0x00133F44 File Offset: 0x00132144
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

		// Token: 0x060040CA RID: 16586 RVA: 0x00133FC1 File Offset: 0x001321C1
		internal void FakeStartTurn(bool isLeft)
		{
			this.StartTurn(isLeft ? (-this.m_TurnAmount) : this.m_TurnAmount);
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x00133FDC File Offset: 0x001321DC
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

		// Token: 0x060040CC RID: 16588 RVA: 0x00134038 File Offset: 0x00132238
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

		// Token: 0x060040CD RID: 16589 RVA: 0x001340CB File Offset: 0x001322CB
		public float ConvertedTurnFactor(float newTurnSpeed)
		{
			return Mathf.Max(0.75f, 0.5f + newTurnSpeed / 10f * 1.5f);
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x001340EA File Offset: 0x001322EA
		public void SetTurningOverride(ISnapTurnOverride caller)
		{
			if (!this.turningOverriders.Contains(caller))
			{
				this.turningOverriders.Add(caller);
			}
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x00134107 File Offset: 0x00132307
		public void UnsetTurningOverride(ISnapTurnOverride caller)
		{
			if (this.turningOverriders.Contains(caller))
			{
				this.turningOverriders.Remove(caller);
			}
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x00134124 File Offset: 0x00132324
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

		// Token: 0x040041F4 RID: 16884
		private static readonly InputFeatureUsage<Vector2>[] m_Vec2UsageList = new InputFeatureUsage<Vector2>[]
		{
			CommonUsages.primary2DAxis,
			CommonUsages.secondary2DAxis
		};

		// Token: 0x040041F5 RID: 16885
		[SerializeField]
		[Tooltip("The 2D Input Axis on the primary devices that will be used to trigger a snap turn.")]
		private GorillaSnapTurn.InputAxes m_TurnUsage;

		// Token: 0x040041F6 RID: 16886
		[SerializeField]
		[Tooltip("A list of controllers that allow Snap Turn.  If an XRController is not enabled, or does not have input actions enabled.  Snap Turn will not work.")]
		private List<XRController> m_Controllers = new List<XRController>();

		// Token: 0x040041F7 RID: 16887
		[SerializeField]
		[Tooltip("The number of degrees clockwise to rotate when snap turning clockwise.")]
		private float m_TurnAmount = 45f;

		// Token: 0x040041F8 RID: 16888
		[SerializeField]
		[Tooltip("The amount of time that the system will wait before starting another snap turn.")]
		private float m_DebounceTime = 0.5f;

		// Token: 0x040041F9 RID: 16889
		[SerializeField]
		[Tooltip("The deadzone that the controller movement will have to be above to trigger a snap turn.")]
		private float m_DeadZone = 0.75f;

		// Token: 0x040041FA RID: 16890
		private float m_CurrentTurnAmount;

		// Token: 0x040041FB RID: 16891
		private float m_TimeStarted;

		// Token: 0x040041FC RID: 16892
		private bool m_AxisReset;

		// Token: 0x040041FD RID: 16893
		public float turnSpeed = 1f;

		// Token: 0x040041FE RID: 16894
		private HashSet<ISnapTurnOverride> turningOverriders = new HashSet<ISnapTurnOverride>();

		// Token: 0x040041FF RID: 16895
		private List<bool> m_ControllersWereActive = new List<bool>();

		// Token: 0x04004200 RID: 16896
		private string m_TurnType = "";

		// Token: 0x04004201 RID: 16897
		private int m_TurnFactor = 1;

		// Token: 0x02000A1C RID: 2588
		public enum InputAxes
		{
			// Token: 0x04004203 RID: 16899
			Primary2DAxis,
			// Token: 0x04004204 RID: 16900
			Secondary2DAxis
		}
	}
}
