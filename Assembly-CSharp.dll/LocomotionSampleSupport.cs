using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000305 RID: 773
public class LocomotionSampleSupport : MonoBehaviour
{
	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06001261 RID: 4705 RVA: 0x0003B9B4 File Offset: 0x00039BB4
	private LocomotionTeleport TeleportController
	{
		get
		{
			return this.lc.GetComponent<LocomotionTeleport>();
		}
	}

	// Token: 0x06001262 RID: 4706 RVA: 0x000AF9D8 File Offset: 0x000ADBD8
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

	// Token: 0x06001263 RID: 4707 RVA: 0x000AFA90 File Offset: 0x000ADC90
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

	// Token: 0x06001264 RID: 4708 RVA: 0x0003B9C1 File Offset: 0x00039BC1
	[Conditional("DEBUG_LOCOMOTION_PANEL")]
	private static void Log(string msg)
	{
		Debug.Log(msg);
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x000AFAE8 File Offset: 0x000ADCE8
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

	// Token: 0x06001266 RID: 4710 RVA: 0x0003B9C9 File Offset: 0x00039BC9
	protected void ActivateHandlers<TInput, TAim, TTarget, TOrientation, TTransition>() where TInput : TeleportInputHandler where TAim : TeleportAimHandler where TTarget : TeleportTargetHandler where TOrientation : TeleportOrientationHandler where TTransition : TeleportTransition
	{
		this.ActivateInput<TInput>();
		this.ActivateAim<TAim>();
		this.ActivateTarget<TTarget>();
		this.ActivateOrientation<TOrientation>();
		this.ActivateTransition<TTransition>();
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x0003B9E9 File Offset: 0x00039BE9
	protected void ActivateInput<TActivate>() where TActivate : TeleportInputHandler
	{
		this.ActivateCategory<TeleportInputHandler, TActivate>();
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x0003B9F2 File Offset: 0x00039BF2
	protected void ActivateAim<TActivate>() where TActivate : TeleportAimHandler
	{
		this.ActivateCategory<TeleportAimHandler, TActivate>();
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x0003B9FB File Offset: 0x00039BFB
	protected void ActivateTarget<TActivate>() where TActivate : TeleportTargetHandler
	{
		this.ActivateCategory<TeleportTargetHandler, TActivate>();
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x0003BA04 File Offset: 0x00039C04
	protected void ActivateOrientation<TActivate>() where TActivate : TeleportOrientationHandler
	{
		this.ActivateCategory<TeleportOrientationHandler, TActivate>();
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x0003BA0D File Offset: 0x00039C0D
	protected void ActivateTransition<TActivate>() where TActivate : TeleportTransition
	{
		this.ActivateCategory<TeleportTransition, TActivate>();
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x0003BA16 File Offset: 0x00039C16
	protected TActivate ActivateCategory<TCategory, TActivate>() where TCategory : MonoBehaviour where TActivate : MonoBehaviour
	{
		return LocomotionSampleSupport.ActivateCategory<TCategory, TActivate>(this.lc.gameObject);
	}

	// Token: 0x0600126D RID: 4717 RVA: 0x0003BA28 File Offset: 0x00039C28
	protected void UpdateToggle(Toggle toggle, bool enabled)
	{
		if (enabled != toggle.isOn)
		{
			toggle.isOn = enabled;
		}
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x0003BA3A File Offset: 0x00039C3A
	private void SetupNonCap()
	{
		TeleportInputHandlerTouch component = this.TeleportController.GetComponent<TeleportInputHandlerTouch>();
		component.InputMode = TeleportInputHandlerTouch.InputModes.SeparateButtonsForAimAndTeleport;
		component.AimButton = OVRInput.RawButton.A;
		component.TeleportButton = OVRInput.RawButton.A;
	}

	// Token: 0x0600126F RID: 4719 RVA: 0x000AFC28 File Offset: 0x000ADE28
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

	// Token: 0x06001270 RID: 4720 RVA: 0x0003BA5B File Offset: 0x00039C5B
	protected GameObject AddInstance(GameObject template, string label)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(template);
		gameObject.transform.SetParent(base.transform, false);
		gameObject.name = label;
		return gameObject;
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x000AFCC8 File Offset: 0x000ADEC8
	private void SetupNodeTeleport()
	{
		this.SetupTeleportDefaults();
		this.SetupNonCap();
		this.lc.PlayerController.RotationEitherThumbstick = true;
		this.TeleportController.EnableRotation(true, false, false, true);
		this.ActivateHandlers<TeleportInputHandlerTouch, TeleportAimHandlerLaser, TeleportTargetHandlerNode, TeleportOrientationHandlerThumbstick, TeleportTransitionBlink>();
		this.TeleportController.GetComponent<TeleportInputHandlerTouch>().AimingController = OVRInput.Controller.RTouch;
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x000AFD18 File Offset: 0x000ADF18
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

	// Token: 0x06001273 RID: 4723 RVA: 0x0003BA7C File Offset: 0x00039C7C
	private void SetupWalkOnly()
	{
		this.SetupTeleportDefaults();
		this.TeleportController.enabled = false;
		this.lc.PlayerController.EnableLinearMovement = true;
		this.lc.PlayerController.RotationEitherThumbstick = false;
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x000AFD8C File Offset: 0x000ADF8C
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

	// Token: 0x04001455 RID: 5205
	private LocomotionController lc;

	// Token: 0x04001456 RID: 5206
	private bool inMenu;
}
