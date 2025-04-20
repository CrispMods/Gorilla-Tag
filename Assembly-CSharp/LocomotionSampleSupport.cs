using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000310 RID: 784
public class LocomotionSampleSupport : MonoBehaviour
{
	// Token: 0x17000220 RID: 544
	// (get) Token: 0x060012AA RID: 4778 RVA: 0x0003CC74 File Offset: 0x0003AE74
	private LocomotionTeleport TeleportController
	{
		get
		{
			return this.lc.GetComponent<LocomotionTeleport>();
		}
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x000B2270 File Offset: 0x000B0470
	public void Start()
	{
		this.lc = UnityEngine.Object.FindObjectOfType<LocomotionController>();
		DebugUIBuilder.instance.AddButton("Node Teleport w/ A", new DebugUIBuilder.OnClick(this.SetupNodeTeleport), -1, 0, false);
		DebugUIBuilder.instance.AddButton("Dual-stick teleport", new DebugUIBuilder.OnClick(this.SetupTwoStickTeleport), -1, 0, false);
		DebugUIBuilder.instance.AddButton("L Strafe R Teleport", new DebugUIBuilder.OnClick(this.SetupLeftStrafeRightTeleport), -1, 0, false);
		DebugUIBuilder.instance.AddButton("Walk Only", new DebugUIBuilder.OnClick(this.SetupWalkOnly), -1, 0, false);
		if (UnityEngine.Object.FindObjectOfType<EventSystem>() == null)
		{
			Debug.LogError("Need EventSystem");
		}
		this.SetupTwoStickTeleport();
		Physics.IgnoreLayerCollision(0, 4);
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x000B2328 File Offset: 0x000B0528
	public void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.Active) || OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.Active))
		{
			if (this.inMenu)
			{
				DebugUIBuilder.instance.Hide();
			}
			else
			{
				DebugUIBuilder.instance.Show();
			}
			this.inMenu = !this.inMenu;
		}
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x0003CC81 File Offset: 0x0003AE81
	[Conditional("DEBUG_LOCOMOTION_PANEL")]
	private static void Log(string msg)
	{
		Debug.Log(msg);
	}

	// Token: 0x060012AE RID: 4782 RVA: 0x000B2380 File Offset: 0x000B0580
	public static TActivate ActivateCategory<TCategory, TActivate>(GameObject target) where TCategory : MonoBehaviour where TActivate : MonoBehaviour
	{
		TCategory[] components = target.GetComponents<TCategory>();
		string[] array = new string[7];
		array[0] = "Activate ";
		int num = 1;
		Type typeFromHandle = typeof(TActivate);
		array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
		array[2] = " derived from ";
		int num2 = 3;
		Type typeFromHandle2 = typeof(TCategory);
		array[num2] = ((typeFromHandle2 != null) ? typeFromHandle2.ToString() : null);
		array[4] = "[";
		array[5] = components.Length.ToString();
		array[6] = "]";
		LocomotionSampleSupport.Log(string.Concat(array));
		TActivate result = default(TActivate);
		foreach (TCategory monoBehaviour in components)
		{
			bool flag = monoBehaviour.GetType() == typeof(TActivate);
			string[] array2 = new string[5];
			int num3 = 0;
			Type type = monoBehaviour.GetType();
			array2[num3] = ((type != null) ? type.ToString() : null);
			array2[1] = " is ";
			int num4 = 2;
			Type typeFromHandle3 = typeof(TActivate);
			array2[num4] = ((typeFromHandle3 != null) ? typeFromHandle3.ToString() : null);
			array2[3] = " = ";
			array2[4] = flag.ToString();
			LocomotionSampleSupport.Log(string.Concat(array2));
			if (flag)
			{
				result = (TActivate)((object)monoBehaviour);
			}
			if (monoBehaviour.enabled != flag)
			{
				monoBehaviour.enabled = flag;
			}
		}
		return result;
	}

	// Token: 0x060012AF RID: 4783 RVA: 0x0003CC89 File Offset: 0x0003AE89
	protected void ActivateHandlers<TInput, TAim, TTarget, TOrientation, TTransition>() where TInput : TeleportInputHandler where TAim : TeleportAimHandler where TTarget : TeleportTargetHandler where TOrientation : TeleportOrientationHandler where TTransition : TeleportTransition
	{
		this.ActivateInput<TInput>();
		this.ActivateAim<TAim>();
		this.ActivateTarget<TTarget>();
		this.ActivateOrientation<TOrientation>();
		this.ActivateTransition<TTransition>();
	}

	// Token: 0x060012B0 RID: 4784 RVA: 0x0003CCA9 File Offset: 0x0003AEA9
	protected void ActivateInput<TActivate>() where TActivate : TeleportInputHandler
	{
		this.ActivateCategory<TeleportInputHandler, TActivate>();
	}

	// Token: 0x060012B1 RID: 4785 RVA: 0x0003CCB2 File Offset: 0x0003AEB2
	protected void ActivateAim<TActivate>() where TActivate : TeleportAimHandler
	{
		this.ActivateCategory<TeleportAimHandler, TActivate>();
	}

	// Token: 0x060012B2 RID: 4786 RVA: 0x0003CCBB File Offset: 0x0003AEBB
	protected void ActivateTarget<TActivate>() where TActivate : TeleportTargetHandler
	{
		this.ActivateCategory<TeleportTargetHandler, TActivate>();
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x0003CCC4 File Offset: 0x0003AEC4
	protected void ActivateOrientation<TActivate>() where TActivate : TeleportOrientationHandler
	{
		this.ActivateCategory<TeleportOrientationHandler, TActivate>();
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x0003CCCD File Offset: 0x0003AECD
	protected void ActivateTransition<TActivate>() where TActivate : TeleportTransition
	{
		this.ActivateCategory<TeleportTransition, TActivate>();
	}

	// Token: 0x060012B5 RID: 4789 RVA: 0x0003CCD6 File Offset: 0x0003AED6
	protected TActivate ActivateCategory<TCategory, TActivate>() where TCategory : MonoBehaviour where TActivate : MonoBehaviour
	{
		return LocomotionSampleSupport.ActivateCategory<TCategory, TActivate>(this.lc.gameObject);
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x0003CCE8 File Offset: 0x0003AEE8
	protected void UpdateToggle(Toggle toggle, bool enabled)
	{
		if (enabled != toggle.isOn)
		{
			toggle.isOn = enabled;
		}
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x0003CCFA File Offset: 0x0003AEFA
	private void SetupNonCap()
	{
		TeleportInputHandlerTouch component = this.TeleportController.GetComponent<TeleportInputHandlerTouch>();
		component.InputMode = TeleportInputHandlerTouch.InputModes.SeparateButtonsForAimAndTeleport;
		component.AimButton = OVRInput.RawButton.A;
		component.TeleportButton = OVRInput.RawButton.A;
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x000B24C0 File Offset: 0x000B06C0
	private void SetupTeleportDefaults()
	{
		this.TeleportController.enabled = true;
		this.lc.PlayerController.RotationEitherThumbstick = false;
		this.TeleportController.EnableMovement(false, false, false, false);
		this.TeleportController.EnableRotation(false, false, false, false);
		TeleportInputHandlerTouch component = this.TeleportController.GetComponent<TeleportInputHandlerTouch>();
		component.InputMode = TeleportInputHandlerTouch.InputModes.CapacitiveButtonForAimAndTeleport;
		component.AimButton = OVRInput.RawButton.A;
		component.TeleportButton = OVRInput.RawButton.A;
		component.CapacitiveAimAndTeleportButton = TeleportInputHandlerTouch.AimCapTouchButtons.A;
		component.FastTeleport = false;
		TeleportInputHandlerHMD component2 = this.TeleportController.GetComponent<TeleportInputHandlerHMD>();
		component2.AimButton = OVRInput.RawButton.A;
		component2.TeleportButton = OVRInput.RawButton.A;
		this.TeleportController.GetComponent<TeleportOrientationHandlerThumbstick>().Thumbstick = OVRInput.Controller.LTouch;
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x0003CD1B File Offset: 0x0003AF1B
	protected GameObject AddInstance(GameObject template, string label)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(template);
		gameObject.transform.SetParent(base.transform, false);
		gameObject.name = label;
		return gameObject;
	}

	// Token: 0x060012BA RID: 4794 RVA: 0x000B2560 File Offset: 0x000B0760
	private void SetupNodeTeleport()
	{
		this.SetupTeleportDefaults();
		this.SetupNonCap();
		this.lc.PlayerController.RotationEitherThumbstick = true;
		this.TeleportController.EnableRotation(true, false, false, true);
		this.ActivateHandlers<TeleportInputHandlerTouch, TeleportAimHandlerLaser, TeleportTargetHandlerNode, TeleportOrientationHandlerThumbstick, TeleportTransitionBlink>();
		this.TeleportController.GetComponent<TeleportInputHandlerTouch>().AimingController = OVRInput.Controller.RTouch;
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x000B25B0 File Offset: 0x000B07B0
	private void SetupTwoStickTeleport()
	{
		this.SetupTeleportDefaults();
		this.TeleportController.EnableRotation(true, false, false, true);
		this.TeleportController.EnableMovement(false, false, false, false);
		this.lc.PlayerController.RotationEitherThumbstick = true;
		TeleportInputHandlerTouch component = this.TeleportController.GetComponent<TeleportInputHandlerTouch>();
		component.InputMode = TeleportInputHandlerTouch.InputModes.ThumbstickTeleportForwardBackOnly;
		component.AimingController = OVRInput.Controller.Touch;
		this.ActivateHandlers<TeleportInputHandlerTouch, TeleportAimHandlerParabolic, TeleportTargetHandlerPhysical, TeleportOrientationHandlerThumbstick, TeleportTransitionBlink>();
		this.TeleportController.GetComponent<TeleportOrientationHandlerThumbstick>().Thumbstick = OVRInput.Controller.Touch;
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x0003CD3C File Offset: 0x0003AF3C
	private void SetupWalkOnly()
	{
		this.SetupTeleportDefaults();
		this.TeleportController.enabled = false;
		this.lc.PlayerController.EnableLinearMovement = true;
		this.lc.PlayerController.RotationEitherThumbstick = false;
	}

	// Token: 0x060012BD RID: 4797 RVA: 0x000B2624 File Offset: 0x000B0824
	private void SetupLeftStrafeRightTeleport()
	{
		this.SetupTeleportDefaults();
		this.TeleportController.EnableRotation(true, false, false, true);
		this.TeleportController.EnableMovement(true, false, false, false);
		TeleportInputHandlerTouch component = this.TeleportController.GetComponent<TeleportInputHandlerTouch>();
		component.InputMode = TeleportInputHandlerTouch.InputModes.ThumbstickTeleportForwardBackOnly;
		component.AimingController = OVRInput.Controller.RTouch;
		this.ActivateHandlers<TeleportInputHandlerTouch, TeleportAimHandlerParabolic, TeleportTargetHandlerPhysical, TeleportOrientationHandlerThumbstick, TeleportTransitionBlink>();
		this.TeleportController.GetComponent<TeleportOrientationHandlerThumbstick>().Thumbstick = OVRInput.Controller.RTouch;
	}

	// Token: 0x0400149C RID: 5276
	private LocomotionController lc;

	// Token: 0x0400149D RID: 5277
	private bool inMenu;
}
