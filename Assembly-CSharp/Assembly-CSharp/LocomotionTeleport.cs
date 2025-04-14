using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020002C2 RID: 706
public class LocomotionTeleport : MonoBehaviour
{
	// Token: 0x06001102 RID: 4354 RVA: 0x0005265B File Offset: 0x0005085B
	public void EnableMovement(bool ready, bool aim, bool pre, bool post)
	{
		this.EnableMovementDuringReady = ready;
		this.EnableMovementDuringAim = aim;
		this.EnableMovementDuringPreTeleport = pre;
		this.EnableMovementDuringPostTeleport = post;
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x0005267A File Offset: 0x0005087A
	public void EnableRotation(bool ready, bool aim, bool pre, bool post)
	{
		this.EnableRotationDuringReady = ready;
		this.EnableRotationDuringAim = aim;
		this.EnableRotationDuringPreTeleport = pre;
		this.EnableRotationDuringPostTeleport = post;
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06001104 RID: 4356 RVA: 0x00052699 File Offset: 0x00050899
	// (set) Token: 0x06001105 RID: 4357 RVA: 0x000526A1 File Offset: 0x000508A1
	public LocomotionTeleport.States CurrentState { get; private set; }

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x06001106 RID: 4358 RVA: 0x000526AC File Offset: 0x000508AC
	// (remove) Token: 0x06001107 RID: 4359 RVA: 0x000526E4 File Offset: 0x000508E4
	public event Action<bool, Vector3?, Quaternion?, Quaternion?> UpdateTeleportDestination;

	// Token: 0x06001108 RID: 4360 RVA: 0x00052719 File Offset: 0x00050919
	public void OnUpdateTeleportDestination(bool isValidDestination, Vector3? position, Quaternion? rotation, Quaternion? landingRotation)
	{
		if (this.UpdateTeleportDestination != null)
		{
			this.UpdateTeleportDestination(isValidDestination, position, rotation, landingRotation);
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06001109 RID: 4361 RVA: 0x00052733 File Offset: 0x00050933
	public Quaternion DestinationRotation
	{
		get
		{
			return this._teleportDestination.OrientationIndicator.rotation;
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x0600110A RID: 4362 RVA: 0x00052745 File Offset: 0x00050945
	// (set) Token: 0x0600110B RID: 4363 RVA: 0x0005274D File Offset: 0x0005094D
	public LocomotionController LocomotionController { get; private set; }

	// Token: 0x0600110C RID: 4364 RVA: 0x00052758 File Offset: 0x00050958
	public bool AimCollisionTest(Vector3 start, Vector3 end, LayerMask aimCollisionLayerMask, out RaycastHit hitInfo)
	{
		Vector3 a = end - start;
		float magnitude = a.magnitude;
		Vector3 direction = a / magnitude;
		switch (this.AimCollisionType)
		{
		case LocomotionTeleport.AimCollisionTypes.Point:
			return Physics.Raycast(start, direction, out hitInfo, magnitude, aimCollisionLayerMask, QueryTriggerInteraction.Ignore);
		case LocomotionTeleport.AimCollisionTypes.Sphere:
		{
			float radius;
			if (this.UseCharacterCollisionData)
			{
				radius = this.LocomotionController.CharacterController.radius;
			}
			else
			{
				radius = this.AimCollisionRadius;
			}
			return Physics.SphereCast(start, radius, direction, out hitInfo, magnitude, aimCollisionLayerMask, QueryTriggerInteraction.Ignore);
		}
		case LocomotionTeleport.AimCollisionTypes.Capsule:
		{
			float num;
			float num2;
			if (this.UseCharacterCollisionData)
			{
				CapsuleCollider characterController = this.LocomotionController.CharacterController;
				num = characterController.height;
				num2 = characterController.radius;
			}
			else
			{
				num = this.AimCollisionHeight;
				num2 = this.AimCollisionRadius;
			}
			return Physics.CapsuleCast(start + new Vector3(0f, num2, 0f), start + new Vector3(0f, num + num2, 0f), num2, direction, out hitInfo, magnitude, aimCollisionLayerMask, QueryTriggerInteraction.Ignore);
		}
		default:
			throw new Exception();
		}
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x00052864 File Offset: 0x00050A64
	[Conditional("DEBUG_TELEPORT_STATES")]
	protected void LogState(string msg)
	{
		Debug.Log(Time.frameCount.ToString() + ": " + msg);
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x00052890 File Offset: 0x00050A90
	protected void CreateNewTeleportDestination()
	{
		this.TeleportDestinationPrefab.gameObject.SetActive(false);
		TeleportDestination teleportDestination = Object.Instantiate<TeleportDestination>(this.TeleportDestinationPrefab);
		teleportDestination.LocomotionTeleport = this;
		teleportDestination.gameObject.layer = this.TeleportDestinationLayer;
		this._teleportDestination = teleportDestination;
		this._teleportDestination.LocomotionTeleport = this;
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x000528E5 File Offset: 0x00050AE5
	private void DeactivateDestination()
	{
		this._teleportDestination.OnDeactivated();
	}

	// Token: 0x06001110 RID: 4368 RVA: 0x000528F2 File Offset: 0x00050AF2
	public void RecycleTeleportDestination(TeleportDestination oldDestination)
	{
		if (oldDestination == this._teleportDestination)
		{
			this.CreateNewTeleportDestination();
		}
		Object.Destroy(oldDestination.gameObject);
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x00052913 File Offset: 0x00050B13
	private void EnableMotion(bool enableLinear, bool enableRotation)
	{
		this.LocomotionController.PlayerController.EnableLinearMovement = enableLinear;
		this.LocomotionController.PlayerController.EnableRotation = enableRotation;
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x00052937 File Offset: 0x00050B37
	private void Awake()
	{
		this.LocomotionController = base.GetComponent<LocomotionController>();
		this.CreateNewTeleportDestination();
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x0005294B File Offset: 0x00050B4B
	public virtual void OnEnable()
	{
		this.CurrentState = LocomotionTeleport.States.Ready;
		base.StartCoroutine(this.ReadyStateCoroutine());
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x00019C59 File Offset: 0x00017E59
	public virtual void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x06001115 RID: 4373 RVA: 0x00052964 File Offset: 0x00050B64
	// (remove) Token: 0x06001116 RID: 4374 RVA: 0x0005299C File Offset: 0x00050B9C
	public event Action EnterStateReady;

	// Token: 0x06001117 RID: 4375 RVA: 0x000529D1 File Offset: 0x00050BD1
	protected IEnumerator ReadyStateCoroutine()
	{
		yield return null;
		this.CurrentState = LocomotionTeleport.States.Ready;
		this.EnableMotion(this.EnableMovementDuringReady, this.EnableRotationDuringReady);
		if (this.EnterStateReady != null)
		{
			this.EnterStateReady();
		}
		while (this.CurrentIntention != LocomotionTeleport.TeleportIntentions.Aim)
		{
			yield return null;
		}
		yield return null;
		base.StartCoroutine(this.AimStateCoroutine());
		yield break;
	}

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x06001118 RID: 4376 RVA: 0x000529E0 File Offset: 0x00050BE0
	// (remove) Token: 0x06001119 RID: 4377 RVA: 0x00052A18 File Offset: 0x00050C18
	public event Action EnterStateAim;

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x0600111A RID: 4378 RVA: 0x00052A50 File Offset: 0x00050C50
	// (remove) Token: 0x0600111B RID: 4379 RVA: 0x00052A88 File Offset: 0x00050C88
	public event Action<LocomotionTeleport.AimData> UpdateAimData;

	// Token: 0x0600111C RID: 4380 RVA: 0x00052ABD File Offset: 0x00050CBD
	public void OnUpdateAimData(LocomotionTeleport.AimData aimData)
	{
		if (this.UpdateAimData != null)
		{
			this.UpdateAimData(aimData);
		}
	}

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x0600111D RID: 4381 RVA: 0x00052AD4 File Offset: 0x00050CD4
	// (remove) Token: 0x0600111E RID: 4382 RVA: 0x00052B0C File Offset: 0x00050D0C
	public event Action ExitStateAim;

	// Token: 0x0600111F RID: 4383 RVA: 0x00052B41 File Offset: 0x00050D41
	protected IEnumerator AimStateCoroutine()
	{
		this.CurrentState = LocomotionTeleport.States.Aim;
		this.EnableMotion(this.EnableMovementDuringAim, this.EnableRotationDuringAim);
		if (this.EnterStateAim != null)
		{
			this.EnterStateAim();
		}
		this._teleportDestination.gameObject.SetActive(true);
		while (this.CurrentIntention == LocomotionTeleport.TeleportIntentions.Aim)
		{
			yield return null;
		}
		if (this.ExitStateAim != null)
		{
			this.ExitStateAim();
		}
		yield return null;
		if ((this.CurrentIntention == LocomotionTeleport.TeleportIntentions.PreTeleport || this.CurrentIntention == LocomotionTeleport.TeleportIntentions.Teleport) && this._teleportDestination.IsValidDestination)
		{
			base.StartCoroutine(this.PreTeleportStateCoroutine());
		}
		else
		{
			base.StartCoroutine(this.CancelAimStateCoroutine());
		}
		yield break;
	}

	// Token: 0x14000035 RID: 53
	// (add) Token: 0x06001120 RID: 4384 RVA: 0x00052B50 File Offset: 0x00050D50
	// (remove) Token: 0x06001121 RID: 4385 RVA: 0x00052B88 File Offset: 0x00050D88
	public event Action EnterStateCancelAim;

	// Token: 0x06001122 RID: 4386 RVA: 0x00052BBD File Offset: 0x00050DBD
	protected IEnumerator CancelAimStateCoroutine()
	{
		this.CurrentState = LocomotionTeleport.States.CancelAim;
		if (this.EnterStateCancelAim != null)
		{
			this.EnterStateCancelAim();
		}
		this.DeactivateDestination();
		yield return null;
		base.StartCoroutine(this.ReadyStateCoroutine());
		yield break;
	}

	// Token: 0x14000036 RID: 54
	// (add) Token: 0x06001123 RID: 4387 RVA: 0x00052BCC File Offset: 0x00050DCC
	// (remove) Token: 0x06001124 RID: 4388 RVA: 0x00052C04 File Offset: 0x00050E04
	public event Action EnterStatePreTeleport;

	// Token: 0x06001125 RID: 4389 RVA: 0x00052C39 File Offset: 0x00050E39
	protected IEnumerator PreTeleportStateCoroutine()
	{
		this.CurrentState = LocomotionTeleport.States.PreTeleport;
		this.EnableMotion(this.EnableMovementDuringPreTeleport, this.EnableRotationDuringPreTeleport);
		if (this.EnterStatePreTeleport != null)
		{
			this.EnterStatePreTeleport();
		}
		while (this.CurrentIntention == LocomotionTeleport.TeleportIntentions.PreTeleport || this.IsPreTeleportRequested)
		{
			yield return null;
		}
		if (this._teleportDestination.IsValidDestination)
		{
			base.StartCoroutine(this.TeleportingStateCoroutine());
		}
		else
		{
			base.StartCoroutine(this.CancelTeleportStateCoroutine());
		}
		yield break;
	}

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06001126 RID: 4390 RVA: 0x00052C48 File Offset: 0x00050E48
	// (remove) Token: 0x06001127 RID: 4391 RVA: 0x00052C80 File Offset: 0x00050E80
	public event Action EnterStateCancelTeleport;

	// Token: 0x06001128 RID: 4392 RVA: 0x00052CB5 File Offset: 0x00050EB5
	protected IEnumerator CancelTeleportStateCoroutine()
	{
		this.CurrentState = LocomotionTeleport.States.CancelTeleport;
		if (this.EnterStateCancelTeleport != null)
		{
			this.EnterStateCancelTeleport();
		}
		this.DeactivateDestination();
		yield return null;
		base.StartCoroutine(this.ReadyStateCoroutine());
		yield break;
	}

	// Token: 0x14000038 RID: 56
	// (add) Token: 0x06001129 RID: 4393 RVA: 0x00052CC4 File Offset: 0x00050EC4
	// (remove) Token: 0x0600112A RID: 4394 RVA: 0x00052CFC File Offset: 0x00050EFC
	public event Action EnterStateTeleporting;

	// Token: 0x0600112B RID: 4395 RVA: 0x00052D31 File Offset: 0x00050F31
	protected IEnumerator TeleportingStateCoroutine()
	{
		this.CurrentState = LocomotionTeleport.States.Teleporting;
		this.EnableMotion(false, false);
		if (this.EnterStateTeleporting != null)
		{
			this.EnterStateTeleporting();
		}
		while (this.IsTransitioning)
		{
			yield return null;
		}
		yield return null;
		base.StartCoroutine(this.PostTeleportStateCoroutine());
		yield break;
	}

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x0600112C RID: 4396 RVA: 0x00052D40 File Offset: 0x00050F40
	// (remove) Token: 0x0600112D RID: 4397 RVA: 0x00052D78 File Offset: 0x00050F78
	public event Action EnterStatePostTeleport;

	// Token: 0x0600112E RID: 4398 RVA: 0x00052DAD File Offset: 0x00050FAD
	protected IEnumerator PostTeleportStateCoroutine()
	{
		this.CurrentState = LocomotionTeleport.States.PostTeleport;
		this.EnableMotion(this.EnableMovementDuringPostTeleport, this.EnableRotationDuringPostTeleport);
		if (this.EnterStatePostTeleport != null)
		{
			this.EnterStatePostTeleport();
		}
		while (this.IsPostTeleportRequested)
		{
			yield return null;
		}
		this.DeactivateDestination();
		yield return null;
		base.StartCoroutine(this.ReadyStateCoroutine());
		yield break;
	}

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x0600112F RID: 4399 RVA: 0x00052DBC File Offset: 0x00050FBC
	// (remove) Token: 0x06001130 RID: 4400 RVA: 0x00052DF4 File Offset: 0x00050FF4
	public event Action<Transform, Vector3, Quaternion> Teleported;

	// Token: 0x06001131 RID: 4401 RVA: 0x00052E2C File Offset: 0x0005102C
	public void DoTeleport()
	{
		CapsuleCollider characterController = this.LocomotionController.CharacterController;
		Transform transform = characterController.transform;
		Vector3 position = this._teleportDestination.OrientationIndicator.position;
		position.y += characterController.height * 0.5f;
		Quaternion landingRotation = this._teleportDestination.LandingRotation;
		if (this.Teleported != null)
		{
			this.Teleported(transform, position, landingRotation);
		}
		transform.position = position;
		transform.rotation = landingRotation;
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x00052EA4 File Offset: 0x000510A4
	public Vector3 GetCharacterPosition()
	{
		return this.LocomotionController.CharacterController.transform.position;
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x00052EBC File Offset: 0x000510BC
	public Quaternion GetHeadRotationY()
	{
		Quaternion result = Quaternion.identity;
		InputDevice deviceAtXRNode = InputDevices.GetDeviceAtXRNode(XRNode.Head);
		if (deviceAtXRNode.isValid)
		{
			deviceAtXRNode.TryGetFeatureValue(CommonUsages.deviceRotation, out result);
		}
		Vector3 eulerAngles = result.eulerAngles;
		eulerAngles.x = 0f;
		eulerAngles.z = 0f;
		result = Quaternion.Euler(eulerAngles);
		return result;
	}

	// Token: 0x06001134 RID: 4404 RVA: 0x00052F18 File Offset: 0x00051118
	public void DoWarp(Vector3 startPos, float positionPercent)
	{
		Vector3 position = this._teleportDestination.OrientationIndicator.position;
		position.y += this.LocomotionController.CharacterController.height / 2f;
		Transform transform = this.LocomotionController.CharacterController.transform;
		Vector3 position2 = Vector3.Lerp(startPos, position, positionPercent);
		transform.position = position2;
	}

	// Token: 0x04001303 RID: 4867
	[Tooltip("Allow linear movement prior to the teleport system being activated.")]
	public bool EnableMovementDuringReady = true;

	// Token: 0x04001304 RID: 4868
	[Tooltip("Allow linear movement while the teleport system is in the process of aiming for a teleport target.")]
	public bool EnableMovementDuringAim = true;

	// Token: 0x04001305 RID: 4869
	[Tooltip("Allow linear movement while the teleport system is in the process of configuring the landing orientation.")]
	public bool EnableMovementDuringPreTeleport = true;

	// Token: 0x04001306 RID: 4870
	[Tooltip("Allow linear movement after the teleport has occurred but before the system has returned to the ready state.")]
	public bool EnableMovementDuringPostTeleport = true;

	// Token: 0x04001307 RID: 4871
	[Tooltip("Allow rotation prior to the teleport system being activated.")]
	public bool EnableRotationDuringReady = true;

	// Token: 0x04001308 RID: 4872
	[Tooltip("Allow rotation while the teleport system is in the process of aiming for a teleport target.")]
	public bool EnableRotationDuringAim = true;

	// Token: 0x04001309 RID: 4873
	[Tooltip("Allow rotation while the teleport system is in the process of configuring the landing orientation.")]
	public bool EnableRotationDuringPreTeleport = true;

	// Token: 0x0400130A RID: 4874
	[Tooltip("Allow rotation after the teleport has occurred but before the system has returned to the ready state.")]
	public bool EnableRotationDuringPostTeleport = true;

	// Token: 0x0400130C RID: 4876
	[NonSerialized]
	public TeleportAimHandler AimHandler;

	// Token: 0x0400130D RID: 4877
	[Tooltip("This prefab will be instantiated as needed and updated to match the current aim target.")]
	public TeleportDestination TeleportDestinationPrefab;

	// Token: 0x0400130E RID: 4878
	[Tooltip("TeleportDestinationPrefab will be instantiated into this layer.")]
	public int TeleportDestinationLayer;

	// Token: 0x04001310 RID: 4880
	[NonSerialized]
	public TeleportInputHandler InputHandler;

	// Token: 0x04001311 RID: 4881
	[NonSerialized]
	public LocomotionTeleport.TeleportIntentions CurrentIntention;

	// Token: 0x04001312 RID: 4882
	[NonSerialized]
	public bool IsPreTeleportRequested;

	// Token: 0x04001313 RID: 4883
	[NonSerialized]
	public bool IsTransitioning;

	// Token: 0x04001314 RID: 4884
	[NonSerialized]
	public bool IsPostTeleportRequested;

	// Token: 0x04001315 RID: 4885
	private TeleportDestination _teleportDestination;

	// Token: 0x04001317 RID: 4887
	[Tooltip("When aiming at possible destinations, the aim collision type determines which shape to use for collision tests.")]
	public LocomotionTeleport.AimCollisionTypes AimCollisionType;

	// Token: 0x04001318 RID: 4888
	[Tooltip("Use the character collision radius/height/skinwidth for sphere/capsule collision tests.")]
	public bool UseCharacterCollisionData;

	// Token: 0x04001319 RID: 4889
	[Tooltip("Radius of the sphere or capsule used for collision testing when aiming to possible teleport destinations. Ignored if UseCharacterCollisionData is true.")]
	public float AimCollisionRadius;

	// Token: 0x0400131A RID: 4890
	[Tooltip("Height of the capsule used for collision testing when aiming to possible teleport destinations. Ignored if UseCharacterCollisionData is true.")]
	public float AimCollisionHeight;

	// Token: 0x020002C3 RID: 707
	public enum States
	{
		// Token: 0x04001326 RID: 4902
		Ready,
		// Token: 0x04001327 RID: 4903
		Aim,
		// Token: 0x04001328 RID: 4904
		CancelAim,
		// Token: 0x04001329 RID: 4905
		PreTeleport,
		// Token: 0x0400132A RID: 4906
		CancelTeleport,
		// Token: 0x0400132B RID: 4907
		Teleporting,
		// Token: 0x0400132C RID: 4908
		PostTeleport
	}

	// Token: 0x020002C4 RID: 708
	public enum TeleportIntentions
	{
		// Token: 0x0400132E RID: 4910
		None,
		// Token: 0x0400132F RID: 4911
		Aim,
		// Token: 0x04001330 RID: 4912
		PreTeleport,
		// Token: 0x04001331 RID: 4913
		Teleport
	}

	// Token: 0x020002C5 RID: 709
	public enum AimCollisionTypes
	{
		// Token: 0x04001333 RID: 4915
		Point,
		// Token: 0x04001334 RID: 4916
		Sphere,
		// Token: 0x04001335 RID: 4917
		Capsule
	}

	// Token: 0x020002C6 RID: 710
	public class AimData
	{
		// Token: 0x06001136 RID: 4406 RVA: 0x00052FB6 File Offset: 0x000511B6
		public AimData()
		{
			this.Points = new List<Vector3>();
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x00052FC9 File Offset: 0x000511C9
		// (set) Token: 0x06001138 RID: 4408 RVA: 0x00052FD1 File Offset: 0x000511D1
		public List<Vector3> Points { get; private set; }

		// Token: 0x06001139 RID: 4409 RVA: 0x00052FDA File Offset: 0x000511DA
		public void Reset()
		{
			this.Points.Clear();
			this.TargetValid = false;
			this.Destination = null;
		}

		// Token: 0x04001336 RID: 4918
		public RaycastHit TargetHitInfo;

		// Token: 0x04001337 RID: 4919
		public bool TargetValid;

		// Token: 0x04001338 RID: 4920
		public Vector3? Destination;

		// Token: 0x04001339 RID: 4921
		public float Radius;
	}
}
