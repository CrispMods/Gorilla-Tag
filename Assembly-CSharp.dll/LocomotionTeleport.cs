using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020002C2 RID: 706
public class LocomotionTeleport : MonoBehaviour
{
	// Token: 0x06001102 RID: 4354 RVA: 0x0003AA67 File Offset: 0x00038C67
	public void EnableMovement(bool ready, bool aim, bool pre, bool post)
	{
		this.EnableMovementDuringReady = ready;
		this.EnableMovementDuringAim = aim;
		this.EnableMovementDuringPreTeleport = pre;
		this.EnableMovementDuringPostTeleport = post;
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x0003AA86 File Offset: 0x00038C86
	public void EnableRotation(bool ready, bool aim, bool pre, bool post)
	{
		this.EnableRotationDuringReady = ready;
		this.EnableRotationDuringAim = aim;
		this.EnableRotationDuringPreTeleport = pre;
		this.EnableRotationDuringPostTeleport = post;
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06001104 RID: 4356 RVA: 0x0003AAA5 File Offset: 0x00038CA5
	// (set) Token: 0x06001105 RID: 4357 RVA: 0x0003AAAD File Offset: 0x00038CAD
	public LocomotionTeleport.States CurrentState { get; private set; }

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x06001106 RID: 4358 RVA: 0x000AAEA0 File Offset: 0x000A90A0
	// (remove) Token: 0x06001107 RID: 4359 RVA: 0x000AAED8 File Offset: 0x000A90D8
	public event Action<bool, Vector3?, Quaternion?, Quaternion?> UpdateTeleportDestination;

	// Token: 0x06001108 RID: 4360 RVA: 0x0003AAB6 File Offset: 0x00038CB6
	public void OnUpdateTeleportDestination(bool isValidDestination, Vector3? position, Quaternion? rotation, Quaternion? landingRotation)
	{
		if (this.UpdateTeleportDestination != null)
		{
			this.UpdateTeleportDestination(isValidDestination, position, rotation, landingRotation);
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06001109 RID: 4361 RVA: 0x0003AAD0 File Offset: 0x00038CD0
	public Quaternion DestinationRotation
	{
		get
		{
			return this._teleportDestination.OrientationIndicator.rotation;
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x0600110A RID: 4362 RVA: 0x0003AAE2 File Offset: 0x00038CE2
	// (set) Token: 0x0600110B RID: 4363 RVA: 0x0003AAEA File Offset: 0x00038CEA
	public LocomotionController LocomotionController { get; private set; }

	// Token: 0x0600110C RID: 4364 RVA: 0x000AAF10 File Offset: 0x000A9110
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

	// Token: 0x0600110D RID: 4365 RVA: 0x000AB01C File Offset: 0x000A921C
	[Conditional("DEBUG_TELEPORT_STATES")]
	protected void LogState(string msg)
	{
		Debug.Log(Time.frameCount.ToString() + ": " + msg);
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x000AB048 File Offset: 0x000A9248
	protected void CreateNewTeleportDestination()
	{
		this.TeleportDestinationPrefab.gameObject.SetActive(false);
		TeleportDestination teleportDestination = UnityEngine.Object.Instantiate<TeleportDestination>(this.TeleportDestinationPrefab);
		teleportDestination.LocomotionTeleport = this;
		teleportDestination.gameObject.layer = this.TeleportDestinationLayer;
		this._teleportDestination = teleportDestination;
		this._teleportDestination.LocomotionTeleport = this;
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x0003AAF3 File Offset: 0x00038CF3
	private void DeactivateDestination()
	{
		this._teleportDestination.OnDeactivated();
	}

	// Token: 0x06001110 RID: 4368 RVA: 0x0003AB00 File Offset: 0x00038D00
	public void RecycleTeleportDestination(TeleportDestination oldDestination)
	{
		if (oldDestination == this._teleportDestination)
		{
			this.CreateNewTeleportDestination();
		}
		UnityEngine.Object.Destroy(oldDestination.gameObject);
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x0003AB21 File Offset: 0x00038D21
	private void EnableMotion(bool enableLinear, bool enableRotation)
	{
		this.LocomotionController.PlayerController.EnableLinearMovement = enableLinear;
		this.LocomotionController.PlayerController.EnableRotation = enableRotation;
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x0003AB45 File Offset: 0x00038D45
	private void Awake()
	{
		this.LocomotionController = base.GetComponent<LocomotionController>();
		this.CreateNewTeleportDestination();
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x0003AB59 File Offset: 0x00038D59
	public virtual void OnEnable()
	{
		this.CurrentState = LocomotionTeleport.States.Ready;
		base.StartCoroutine(this.ReadyStateCoroutine());
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x0003242F File Offset: 0x0003062F
	public virtual void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x06001115 RID: 4373 RVA: 0x000AB0A0 File Offset: 0x000A92A0
	// (remove) Token: 0x06001116 RID: 4374 RVA: 0x000AB0D8 File Offset: 0x000A92D8
	public event Action EnterStateReady;

	// Token: 0x06001117 RID: 4375 RVA: 0x0003AB6F File Offset: 0x00038D6F
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
	// (add) Token: 0x06001118 RID: 4376 RVA: 0x000AB110 File Offset: 0x000A9310
	// (remove) Token: 0x06001119 RID: 4377 RVA: 0x000AB148 File Offset: 0x000A9348
	public event Action EnterStateAim;

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x0600111A RID: 4378 RVA: 0x000AB180 File Offset: 0x000A9380
	// (remove) Token: 0x0600111B RID: 4379 RVA: 0x000AB1B8 File Offset: 0x000A93B8
	public event Action<LocomotionTeleport.AimData> UpdateAimData;

	// Token: 0x0600111C RID: 4380 RVA: 0x0003AB7E File Offset: 0x00038D7E
	public void OnUpdateAimData(LocomotionTeleport.AimData aimData)
	{
		if (this.UpdateAimData != null)
		{
			this.UpdateAimData(aimData);
		}
	}

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x0600111D RID: 4381 RVA: 0x000AB1F0 File Offset: 0x000A93F0
	// (remove) Token: 0x0600111E RID: 4382 RVA: 0x000AB228 File Offset: 0x000A9428
	public event Action ExitStateAim;

	// Token: 0x0600111F RID: 4383 RVA: 0x0003AB94 File Offset: 0x00038D94
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
	// (add) Token: 0x06001120 RID: 4384 RVA: 0x000AB260 File Offset: 0x000A9460
	// (remove) Token: 0x06001121 RID: 4385 RVA: 0x000AB298 File Offset: 0x000A9498
	public event Action EnterStateCancelAim;

	// Token: 0x06001122 RID: 4386 RVA: 0x0003ABA3 File Offset: 0x00038DA3
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
	// (add) Token: 0x06001123 RID: 4387 RVA: 0x000AB2D0 File Offset: 0x000A94D0
	// (remove) Token: 0x06001124 RID: 4388 RVA: 0x000AB308 File Offset: 0x000A9508
	public event Action EnterStatePreTeleport;

	// Token: 0x06001125 RID: 4389 RVA: 0x0003ABB2 File Offset: 0x00038DB2
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
	// (add) Token: 0x06001126 RID: 4390 RVA: 0x000AB340 File Offset: 0x000A9540
	// (remove) Token: 0x06001127 RID: 4391 RVA: 0x000AB378 File Offset: 0x000A9578
	public event Action EnterStateCancelTeleport;

	// Token: 0x06001128 RID: 4392 RVA: 0x0003ABC1 File Offset: 0x00038DC1
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
	// (add) Token: 0x06001129 RID: 4393 RVA: 0x000AB3B0 File Offset: 0x000A95B0
	// (remove) Token: 0x0600112A RID: 4394 RVA: 0x000AB3E8 File Offset: 0x000A95E8
	public event Action EnterStateTeleporting;

	// Token: 0x0600112B RID: 4395 RVA: 0x0003ABD0 File Offset: 0x00038DD0
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
	// (add) Token: 0x0600112C RID: 4396 RVA: 0x000AB420 File Offset: 0x000A9620
	// (remove) Token: 0x0600112D RID: 4397 RVA: 0x000AB458 File Offset: 0x000A9658
	public event Action EnterStatePostTeleport;

	// Token: 0x0600112E RID: 4398 RVA: 0x0003ABDF File Offset: 0x00038DDF
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
	// (add) Token: 0x0600112F RID: 4399 RVA: 0x000AB490 File Offset: 0x000A9690
	// (remove) Token: 0x06001130 RID: 4400 RVA: 0x000AB4C8 File Offset: 0x000A96C8
	public event Action<Transform, Vector3, Quaternion> Teleported;

	// Token: 0x06001131 RID: 4401 RVA: 0x000AB500 File Offset: 0x000A9700
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

	// Token: 0x06001132 RID: 4402 RVA: 0x0003ABEE File Offset: 0x00038DEE
	public Vector3 GetCharacterPosition()
	{
		return this.LocomotionController.CharacterController.transform.position;
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x000AB578 File Offset: 0x000A9778
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

	// Token: 0x06001134 RID: 4404 RVA: 0x000AB5D4 File Offset: 0x000A97D4
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
		// Token: 0x06001136 RID: 4406 RVA: 0x0003AC45 File Offset: 0x00038E45
		public AimData()
		{
			this.Points = new List<Vector3>();
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x0003AC58 File Offset: 0x00038E58
		// (set) Token: 0x06001138 RID: 4408 RVA: 0x0003AC60 File Offset: 0x00038E60
		public List<Vector3> Points { get; private set; }

		// Token: 0x06001139 RID: 4409 RVA: 0x0003AC69 File Offset: 0x00038E69
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
