using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020002CD RID: 717
public class LocomotionTeleport : MonoBehaviour
{
	// Token: 0x0600114B RID: 4427 RVA: 0x0003BD27 File Offset: 0x00039F27
	public void EnableMovement(bool ready, bool aim, bool pre, bool post)
	{
		this.EnableMovementDuringReady = ready;
		this.EnableMovementDuringAim = aim;
		this.EnableMovementDuringPreTeleport = pre;
		this.EnableMovementDuringPostTeleport = post;
	}

	// Token: 0x0600114C RID: 4428 RVA: 0x0003BD46 File Offset: 0x00039F46
	public void EnableRotation(bool ready, bool aim, bool pre, bool post)
	{
		this.EnableRotationDuringReady = ready;
		this.EnableRotationDuringAim = aim;
		this.EnableRotationDuringPreTeleport = pre;
		this.EnableRotationDuringPostTeleport = post;
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x0600114D RID: 4429 RVA: 0x0003BD65 File Offset: 0x00039F65
	// (set) Token: 0x0600114E RID: 4430 RVA: 0x0003BD6D File Offset: 0x00039F6D
	public LocomotionTeleport.States CurrentState { get; private set; }

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x0600114F RID: 4431 RVA: 0x000AD738 File Offset: 0x000AB938
	// (remove) Token: 0x06001150 RID: 4432 RVA: 0x000AD770 File Offset: 0x000AB970
	public event Action<bool, Vector3?, Quaternion?, Quaternion?> UpdateTeleportDestination;

	// Token: 0x06001151 RID: 4433 RVA: 0x0003BD76 File Offset: 0x00039F76
	public void OnUpdateTeleportDestination(bool isValidDestination, Vector3? position, Quaternion? rotation, Quaternion? landingRotation)
	{
		if (this.UpdateTeleportDestination != null)
		{
			this.UpdateTeleportDestination(isValidDestination, position, rotation, landingRotation);
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x06001152 RID: 4434 RVA: 0x0003BD90 File Offset: 0x00039F90
	public Quaternion DestinationRotation
	{
		get
		{
			return this._teleportDestination.OrientationIndicator.rotation;
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06001153 RID: 4435 RVA: 0x0003BDA2 File Offset: 0x00039FA2
	// (set) Token: 0x06001154 RID: 4436 RVA: 0x0003BDAA File Offset: 0x00039FAA
	public LocomotionController LocomotionController { get; private set; }

	// Token: 0x06001155 RID: 4437 RVA: 0x000AD7A8 File Offset: 0x000AB9A8
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

	// Token: 0x06001156 RID: 4438 RVA: 0x000AD8B4 File Offset: 0x000ABAB4
	[Conditional("DEBUG_TELEPORT_STATES")]
	protected void LogState(string msg)
	{
		Debug.Log(Time.frameCount.ToString() + ": " + msg);
	}

	// Token: 0x06001157 RID: 4439 RVA: 0x000AD8E0 File Offset: 0x000ABAE0
	protected void CreateNewTeleportDestination()
	{
		this.TeleportDestinationPrefab.gameObject.SetActive(false);
		TeleportDestination teleportDestination = UnityEngine.Object.Instantiate<TeleportDestination>(this.TeleportDestinationPrefab);
		teleportDestination.LocomotionTeleport = this;
		teleportDestination.gameObject.layer = this.TeleportDestinationLayer;
		this._teleportDestination = teleportDestination;
		this._teleportDestination.LocomotionTeleport = this;
	}

	// Token: 0x06001158 RID: 4440 RVA: 0x0003BDB3 File Offset: 0x00039FB3
	private void DeactivateDestination()
	{
		this._teleportDestination.OnDeactivated();
	}

	// Token: 0x06001159 RID: 4441 RVA: 0x0003BDC0 File Offset: 0x00039FC0
	public void RecycleTeleportDestination(TeleportDestination oldDestination)
	{
		if (oldDestination == this._teleportDestination)
		{
			this.CreateNewTeleportDestination();
		}
		UnityEngine.Object.Destroy(oldDestination.gameObject);
	}

	// Token: 0x0600115A RID: 4442 RVA: 0x0003BDE1 File Offset: 0x00039FE1
	private void EnableMotion(bool enableLinear, bool enableRotation)
	{
		this.LocomotionController.PlayerController.EnableLinearMovement = enableLinear;
		this.LocomotionController.PlayerController.EnableRotation = enableRotation;
	}

	// Token: 0x0600115B RID: 4443 RVA: 0x0003BE05 File Offset: 0x0003A005
	private void Awake()
	{
		this.LocomotionController = base.GetComponent<LocomotionController>();
		this.CreateNewTeleportDestination();
	}

	// Token: 0x0600115C RID: 4444 RVA: 0x0003BE19 File Offset: 0x0003A019
	public virtual void OnEnable()
	{
		this.CurrentState = LocomotionTeleport.States.Ready;
		base.StartCoroutine(this.ReadyStateCoroutine());
	}

	// Token: 0x0600115D RID: 4445 RVA: 0x00033636 File Offset: 0x00031836
	public virtual void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x0600115E RID: 4446 RVA: 0x000AD938 File Offset: 0x000ABB38
	// (remove) Token: 0x0600115F RID: 4447 RVA: 0x000AD970 File Offset: 0x000ABB70
	public event Action EnterStateReady;

	// Token: 0x06001160 RID: 4448 RVA: 0x0003BE2F File Offset: 0x0003A02F
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

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x06001161 RID: 4449 RVA: 0x000AD9A8 File Offset: 0x000ABBA8
	// (remove) Token: 0x06001162 RID: 4450 RVA: 0x000AD9E0 File Offset: 0x000ABBE0
	public event Action EnterStateAim;

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x06001163 RID: 4451 RVA: 0x000ADA18 File Offset: 0x000ABC18
	// (remove) Token: 0x06001164 RID: 4452 RVA: 0x000ADA50 File Offset: 0x000ABC50
	public event Action<LocomotionTeleport.AimData> UpdateAimData;

	// Token: 0x06001165 RID: 4453 RVA: 0x0003BE3E File Offset: 0x0003A03E
	public void OnUpdateAimData(LocomotionTeleport.AimData aimData)
	{
		if (this.UpdateAimData != null)
		{
			this.UpdateAimData(aimData);
		}
	}

	// Token: 0x14000035 RID: 53
	// (add) Token: 0x06001166 RID: 4454 RVA: 0x000ADA88 File Offset: 0x000ABC88
	// (remove) Token: 0x06001167 RID: 4455 RVA: 0x000ADAC0 File Offset: 0x000ABCC0
	public event Action ExitStateAim;

	// Token: 0x06001168 RID: 4456 RVA: 0x0003BE54 File Offset: 0x0003A054
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

	// Token: 0x14000036 RID: 54
	// (add) Token: 0x06001169 RID: 4457 RVA: 0x000ADAF8 File Offset: 0x000ABCF8
	// (remove) Token: 0x0600116A RID: 4458 RVA: 0x000ADB30 File Offset: 0x000ABD30
	public event Action EnterStateCancelAim;

	// Token: 0x0600116B RID: 4459 RVA: 0x0003BE63 File Offset: 0x0003A063
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

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x0600116C RID: 4460 RVA: 0x000ADB68 File Offset: 0x000ABD68
	// (remove) Token: 0x0600116D RID: 4461 RVA: 0x000ADBA0 File Offset: 0x000ABDA0
	public event Action EnterStatePreTeleport;

	// Token: 0x0600116E RID: 4462 RVA: 0x0003BE72 File Offset: 0x0003A072
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

	// Token: 0x14000038 RID: 56
	// (add) Token: 0x0600116F RID: 4463 RVA: 0x000ADBD8 File Offset: 0x000ABDD8
	// (remove) Token: 0x06001170 RID: 4464 RVA: 0x000ADC10 File Offset: 0x000ABE10
	public event Action EnterStateCancelTeleport;

	// Token: 0x06001171 RID: 4465 RVA: 0x0003BE81 File Offset: 0x0003A081
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

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x06001172 RID: 4466 RVA: 0x000ADC48 File Offset: 0x000ABE48
	// (remove) Token: 0x06001173 RID: 4467 RVA: 0x000ADC80 File Offset: 0x000ABE80
	public event Action EnterStateTeleporting;

	// Token: 0x06001174 RID: 4468 RVA: 0x0003BE90 File Offset: 0x0003A090
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

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x06001175 RID: 4469 RVA: 0x000ADCB8 File Offset: 0x000ABEB8
	// (remove) Token: 0x06001176 RID: 4470 RVA: 0x000ADCF0 File Offset: 0x000ABEF0
	public event Action EnterStatePostTeleport;

	// Token: 0x06001177 RID: 4471 RVA: 0x0003BE9F File Offset: 0x0003A09F
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

	// Token: 0x1400003B RID: 59
	// (add) Token: 0x06001178 RID: 4472 RVA: 0x000ADD28 File Offset: 0x000ABF28
	// (remove) Token: 0x06001179 RID: 4473 RVA: 0x000ADD60 File Offset: 0x000ABF60
	public event Action<Transform, Vector3, Quaternion> Teleported;

	// Token: 0x0600117A RID: 4474 RVA: 0x000ADD98 File Offset: 0x000ABF98
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

	// Token: 0x0600117B RID: 4475 RVA: 0x0003BEAE File Offset: 0x0003A0AE
	public Vector3 GetCharacterPosition()
	{
		return this.LocomotionController.CharacterController.transform.position;
	}

	// Token: 0x0600117C RID: 4476 RVA: 0x000ADE10 File Offset: 0x000AC010
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

	// Token: 0x0600117D RID: 4477 RVA: 0x000ADE6C File Offset: 0x000AC06C
	public void DoWarp(Vector3 startPos, float positionPercent)
	{
		Vector3 position = this._teleportDestination.OrientationIndicator.position;
		position.y += this.LocomotionController.CharacterController.height / 2f;
		Transform transform = this.LocomotionController.CharacterController.transform;
		Vector3 position2 = Vector3.Lerp(startPos, position, positionPercent);
		transform.position = position2;
	}

	// Token: 0x0400134A RID: 4938
	[Tooltip("Allow linear movement prior to the teleport system being activated.")]
	public bool EnableMovementDuringReady = true;

	// Token: 0x0400134B RID: 4939
	[Tooltip("Allow linear movement while the teleport system is in the process of aiming for a teleport target.")]
	public bool EnableMovementDuringAim = true;

	// Token: 0x0400134C RID: 4940
	[Tooltip("Allow linear movement while the teleport system is in the process of configuring the landing orientation.")]
	public bool EnableMovementDuringPreTeleport = true;

	// Token: 0x0400134D RID: 4941
	[Tooltip("Allow linear movement after the teleport has occurred but before the system has returned to the ready state.")]
	public bool EnableMovementDuringPostTeleport = true;

	// Token: 0x0400134E RID: 4942
	[Tooltip("Allow rotation prior to the teleport system being activated.")]
	public bool EnableRotationDuringReady = true;

	// Token: 0x0400134F RID: 4943
	[Tooltip("Allow rotation while the teleport system is in the process of aiming for a teleport target.")]
	public bool EnableRotationDuringAim = true;

	// Token: 0x04001350 RID: 4944
	[Tooltip("Allow rotation while the teleport system is in the process of configuring the landing orientation.")]
	public bool EnableRotationDuringPreTeleport = true;

	// Token: 0x04001351 RID: 4945
	[Tooltip("Allow rotation after the teleport has occurred but before the system has returned to the ready state.")]
	public bool EnableRotationDuringPostTeleport = true;

	// Token: 0x04001353 RID: 4947
	[NonSerialized]
	public TeleportAimHandler AimHandler;

	// Token: 0x04001354 RID: 4948
	[Tooltip("This prefab will be instantiated as needed and updated to match the current aim target.")]
	public TeleportDestination TeleportDestinationPrefab;

	// Token: 0x04001355 RID: 4949
	[Tooltip("TeleportDestinationPrefab will be instantiated into this layer.")]
	public int TeleportDestinationLayer;

	// Token: 0x04001357 RID: 4951
	[NonSerialized]
	public TeleportInputHandler InputHandler;

	// Token: 0x04001358 RID: 4952
	[NonSerialized]
	public LocomotionTeleport.TeleportIntentions CurrentIntention;

	// Token: 0x04001359 RID: 4953
	[NonSerialized]
	public bool IsPreTeleportRequested;

	// Token: 0x0400135A RID: 4954
	[NonSerialized]
	public bool IsTransitioning;

	// Token: 0x0400135B RID: 4955
	[NonSerialized]
	public bool IsPostTeleportRequested;

	// Token: 0x0400135C RID: 4956
	private TeleportDestination _teleportDestination;

	// Token: 0x0400135E RID: 4958
	[Tooltip("When aiming at possible destinations, the aim collision type determines which shape to use for collision tests.")]
	public LocomotionTeleport.AimCollisionTypes AimCollisionType;

	// Token: 0x0400135F RID: 4959
	[Tooltip("Use the character collision radius/height/skinwidth for sphere/capsule collision tests.")]
	public bool UseCharacterCollisionData;

	// Token: 0x04001360 RID: 4960
	[Tooltip("Radius of the sphere or capsule used for collision testing when aiming to possible teleport destinations. Ignored if UseCharacterCollisionData is true.")]
	public float AimCollisionRadius;

	// Token: 0x04001361 RID: 4961
	[Tooltip("Height of the capsule used for collision testing when aiming to possible teleport destinations. Ignored if UseCharacterCollisionData is true.")]
	public float AimCollisionHeight;

	// Token: 0x020002CE RID: 718
	public enum States
	{
		// Token: 0x0400136D RID: 4973
		Ready,
		// Token: 0x0400136E RID: 4974
		Aim,
		// Token: 0x0400136F RID: 4975
		CancelAim,
		// Token: 0x04001370 RID: 4976
		PreTeleport,
		// Token: 0x04001371 RID: 4977
		CancelTeleport,
		// Token: 0x04001372 RID: 4978
		Teleporting,
		// Token: 0x04001373 RID: 4979
		PostTeleport
	}

	// Token: 0x020002CF RID: 719
	public enum TeleportIntentions
	{
		// Token: 0x04001375 RID: 4981
		None,
		// Token: 0x04001376 RID: 4982
		Aim,
		// Token: 0x04001377 RID: 4983
		PreTeleport,
		// Token: 0x04001378 RID: 4984
		Teleport
	}

	// Token: 0x020002D0 RID: 720
	public enum AimCollisionTypes
	{
		// Token: 0x0400137A RID: 4986
		Point,
		// Token: 0x0400137B RID: 4987
		Sphere,
		// Token: 0x0400137C RID: 4988
		Capsule
	}

	// Token: 0x020002D1 RID: 721
	public class AimData
	{
		// Token: 0x0600117F RID: 4479 RVA: 0x0003BF05 File Offset: 0x0003A105
		public AimData()
		{
			this.Points = new List<Vector3>();
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x0003BF18 File Offset: 0x0003A118
		// (set) Token: 0x06001181 RID: 4481 RVA: 0x0003BF20 File Offset: 0x0003A120
		public List<Vector3> Points { get; private set; }

		// Token: 0x06001182 RID: 4482 RVA: 0x0003BF29 File Offset: 0x0003A129
		public void Reset()
		{
			this.Points.Clear();
			this.TargetValid = false;
			this.Destination = null;
		}

		// Token: 0x0400137D RID: 4989
		public RaycastHit TargetHitInfo;

		// Token: 0x0400137E RID: 4990
		public bool TargetValid;

		// Token: 0x0400137F RID: 4991
		public Vector3? Destination;

		// Token: 0x04001380 RID: 4992
		public float Radius;
	}
}
