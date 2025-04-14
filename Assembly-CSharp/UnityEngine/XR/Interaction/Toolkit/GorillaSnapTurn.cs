using System;
using System.Collections.Generic;
using GorillaLocomotion;
using Valve.VR;

namespace UnityEngine.XR.Interaction.Toolkit
{
	// Token: 0x02000A18 RID: 2584
	public class GorillaSnapTurn : LocomotionProvider
	{
		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060040AE RID: 16558 RVA: 0x001337D3 File Offset: 0x001319D3
		// (set) Token: 0x060040AF RID: 16559 RVA: 0x001337DB File Offset: 0x001319DB
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

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060040B0 RID: 16560 RVA: 0x001337E4 File Offset: 0x001319E4
		// (set) Token: 0x060040B1 RID: 16561 RVA: 0x001337EC File Offset: 0x001319EC
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

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060040B2 RID: 16562 RVA: 0x001337F5 File Offset: 0x001319F5
		// (set) Token: 0x060040B3 RID: 16563 RVA: 0x001337FD File Offset: 0x001319FD
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

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060040B4 RID: 16564 RVA: 0x00133806 File Offset: 0x00131A06
		// (set) Token: 0x060040B5 RID: 16565 RVA: 0x0013380E File Offset: 0x00131A0E
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

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060040B6 RID: 16566 RVA: 0x00133817 File Offset: 0x00131A17
		// (set) Token: 0x060040B7 RID: 16567 RVA: 0x0013381F File Offset: 0x00131A1F
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

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060040B8 RID: 16568 RVA: 0x00133828 File Offset: 0x00131A28
		// (set) Token: 0x060040B9 RID: 16569 RVA: 0x00133830 File Offset: 0x00131A30
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

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060040BA RID: 16570 RVA: 0x00133839 File Offset: 0x00131A39
		// (set) Token: 0x060040BB RID: 16571 RVA: 0x00133841 File Offset: 0x00131A41
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

		// Token: 0x060040BC RID: 16572 RVA: 0x0013384C File Offset: 0x00131A4C
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

		// Token: 0x060040BD RID: 16573 RVA: 0x0013397C File Offset: 0x00131B7C
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

		// Token: 0x060040BE RID: 16574 RVA: 0x001339F9 File Offset: 0x00131BF9
		internal void FakeStartTurn(bool isLeft)
		{
			this.StartTurn(isLeft ? (-this.m_TurnAmount) : this.m_TurnAmount);
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x00133A14 File Offset: 0x00131C14
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

		// Token: 0x060040C0 RID: 16576 RVA: 0x00133A70 File Offset: 0x00131C70
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

		// Token: 0x060040C1 RID: 16577 RVA: 0x00133B03 File Offset: 0x00131D03
		public float ConvertedTurnFactor(float newTurnSpeed)
		{
			return Mathf.Max(0.75f, 0.5f + newTurnSpeed / 10f * 1.5f);
		}

		// Token: 0x060040C2 RID: 16578 RVA: 0x00133B22 File Offset: 0x00131D22
		public void SetTurningOverride(ISnapTurnOverride caller)
		{
			if (!this.turningOverriders.Contains(caller))
			{
				this.turningOverriders.Add(caller);
			}
		}

		// Token: 0x060040C3 RID: 16579 RVA: 0x00133B3F File Offset: 0x00131D3F
		public void UnsetTurningOverride(ISnapTurnOverride caller)
		{
			if (this.turningOverriders.Contains(caller))
			{
				this.turningOverriders.Remove(caller);
			}
		}

		// Token: 0x060040C4 RID: 16580 RVA: 0x00133B5C File Offset: 0x00131D5C
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

		// Token: 0x040041E2 RID: 16866
		private static readonly InputFeatureUsage<Vector2>[] m_Vec2UsageList = new InputFeatureUsage<Vector2>[]
		{
			CommonUsages.primary2DAxis,
			CommonUsages.secondary2DAxis
		};

		// Token: 0x040041E3 RID: 16867
		[SerializeField]
		[Tooltip("The 2D Input Axis on the primary devices that will be used to trigger a snap turn.")]
		private GorillaSnapTurn.InputAxes m_TurnUsage;

		// Token: 0x040041E4 RID: 16868
		[SerializeField]
		[Tooltip("A list of controllers that allow Snap Turn.  If an XRController is not enabled, or does not have input actions enabled.  Snap Turn will not work.")]
		private List<XRController> m_Controllers = new List<XRController>();

		// Token: 0x040041E5 RID: 16869
		[SerializeField]
		[Tooltip("The number of degrees clockwise to rotate when snap turning clockwise.")]
		private float m_TurnAmount = 45f;

		// Token: 0x040041E6 RID: 16870
		[SerializeField]
		[Tooltip("The amount of time that the system will wait before starting another snap turn.")]
		private float m_DebounceTime = 0.5f;

		// Token: 0x040041E7 RID: 16871
		[SerializeField]
		[Tooltip("The deadzone that the controller movement will have to be above to trigger a snap turn.")]
		private float m_DeadZone = 0.75f;

		// Token: 0x040041E8 RID: 16872
		private float m_CurrentTurnAmount;

		// Token: 0x040041E9 RID: 16873
		private float m_TimeStarted;

		// Token: 0x040041EA RID: 16874
		private bool m_AxisReset;

		// Token: 0x040041EB RID: 16875
		public float turnSpeed = 1f;

		// Token: 0x040041EC RID: 16876
		private HashSet<ISnapTurnOverride> turningOverriders = new HashSet<ISnapTurnOverride>();

		// Token: 0x040041ED RID: 16877
		private List<bool> m_ControllersWereActive = new List<bool>();

		// Token: 0x040041EE RID: 16878
		private string m_TurnType = "";

		// Token: 0x040041EF RID: 16879
		private int m_TurnFactor = 1;

		// Token: 0x02000A19 RID: 2585
		public enum InputAxes
		{
			// Token: 0x040041F1 RID: 16881
			Primary2DAxis,
			// Token: 0x040041F2 RID: 16882
			Secondary2DAxis
		}
	}
}
