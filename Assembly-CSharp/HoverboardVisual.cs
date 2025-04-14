using System;
using GorillaExtensions;
using GorillaLocomotion;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020005C5 RID: 1477
public class HoverboardVisual : MonoBehaviour, ICallBack
{
	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x060024A8 RID: 9384 RVA: 0x000B694E File Offset: 0x000B4B4E
	// (set) Token: 0x060024A9 RID: 9385 RVA: 0x000B6956 File Offset: 0x000B4B56
	public Color boardColor { get; private set; }

	// Token: 0x060024AA RID: 9386 RVA: 0x000B6960 File Offset: 0x000B4B60
	private void Awake()
	{
		Material[] sharedMaterials = this.boardMesh.sharedMaterials;
		this.colorMaterial = new Material(sharedMaterials[1]);
		sharedMaterials[1] = this.colorMaterial;
		this.boardMesh.sharedMaterials = sharedMaterials;
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x060024AB RID: 9387 RVA: 0x000B699C File Offset: 0x000B4B9C
	// (set) Token: 0x060024AC RID: 9388 RVA: 0x000B69A4 File Offset: 0x000B4BA4
	public bool IsHeld { get; private set; }

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x060024AD RID: 9389 RVA: 0x000B69AD File Offset: 0x000B4BAD
	// (set) Token: 0x060024AE RID: 9390 RVA: 0x000B69B5 File Offset: 0x000B4BB5
	public bool IsLeftHanded { get; private set; }

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x060024AF RID: 9391 RVA: 0x000B69BE File Offset: 0x000B4BBE
	// (set) Token: 0x060024B0 RID: 9392 RVA: 0x000B69C6 File Offset: 0x000B4BC6
	public Vector3 NominalLocalPosition { get; private set; }

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x060024B1 RID: 9393 RVA: 0x000B69CF File Offset: 0x000B4BCF
	// (set) Token: 0x060024B2 RID: 9394 RVA: 0x000B69D7 File Offset: 0x000B4BD7
	public Quaternion NominalLocalRotation { get; private set; }

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x060024B3 RID: 9395 RVA: 0x000B69E0 File Offset: 0x000B4BE0
	private Transform NominalParentTransform
	{
		get
		{
			if (!this.IsHeld)
			{
				return base.transform.parent;
			}
			return (this.IsLeftHanded ? this.parentRig.leftHand : this.parentRig.rightHand).rigTarget.transform;
		}
	}

	// Token: 0x060024B4 RID: 9396 RVA: 0x000B6A20 File Offset: 0x000B4C20
	public void SetIsHeld(bool isHeldLeftHanded, Vector3 localPosition, Quaternion localRotation, Color boardColor)
	{
		if (!this.isCallbackActive)
		{
			this.parentRig.AddLateUpdateCallback(this);
			this.isCallbackActive = true;
		}
		this.IsHeld = true;
		base.gameObject.SetActive(true);
		this.IsLeftHanded = isHeldLeftHanded;
		this.NominalLocalPosition = localPosition;
		this.NominalLocalRotation = localRotation;
		Transform nominalParentTransform = this.NominalParentTransform;
		this.interpolatedLocalPosition = nominalParentTransform.InverseTransformPoint(base.transform.position);
		this.interpolatedLocalRotation = nominalParentTransform.InverseTransformRotation(base.transform.rotation);
		this.positionLerpSpeed = (this.interpolatedLocalPosition - this.NominalLocalPosition).magnitude / this.lerpIntoHandDuration;
		float num;
		Vector3 vector;
		(Quaternion.Inverse(this.interpolatedLocalRotation) * this.NominalLocalRotation).ToAngleAxis(out num, out vector);
		this.rotationLerpSpeed = num / this.lerpIntoHandDuration;
		if (this.parentRig.isLocal)
		{
			GTPlayer.Instance.SetHoverActive(true);
		}
		this.colorMaterial.color = boardColor;
		this.boardColor = boardColor;
	}

	// Token: 0x060024B5 RID: 9397 RVA: 0x000B6B29 File Offset: 0x000B4D29
	public void SetNotHeld(bool isLeftHanded)
	{
		this.IsLeftHanded = isLeftHanded;
		this.SetNotHeld();
	}

	// Token: 0x060024B6 RID: 9398 RVA: 0x000B6B38 File Offset: 0x000B4D38
	public void SetNotHeld()
	{
		bool isHeld = this.IsHeld;
		base.gameObject.SetActive(false);
		this.IsHeld = false;
		this.interpolatedLocalPosition = base.transform.localPosition;
		this.interpolatedLocalRotation = base.transform.localRotation;
		this.positionLerpSpeed = (this.interpolatedLocalPosition - this.NominalLocalPosition).magnitude / this.lerpIntoHandDuration;
		float num;
		Vector3 vector;
		(Quaternion.Inverse(this.interpolatedLocalRotation) * this.NominalLocalRotation).ToAngleAxis(out num, out vector);
		this.rotationLerpSpeed = num / this.lerpIntoHandDuration;
		if (!isHeld)
		{
			base.transform.position = base.transform.parent.TransformPoint(this.NominalLocalPosition);
			base.transform.rotation = base.transform.parent.TransformRotation(this.NominalLocalRotation);
		}
		if (this.parentRig.isLocal)
		{
			GTPlayer.Instance.SetHoverActive(false);
		}
		this.hoverboardAudio.Stop();
	}

	// Token: 0x060024B7 RID: 9399 RVA: 0x000B6C40 File Offset: 0x000B4E40
	void ICallBack.CallBack()
	{
		Transform nominalParentTransform = this.NominalParentTransform;
		if ((this.interpolatedLocalPosition - this.NominalLocalPosition).IsShorterThan(0.01f))
		{
			base.transform.position = nominalParentTransform.TransformPoint(this.NominalLocalPosition);
			base.transform.rotation = nominalParentTransform.TransformRotation(this.NominalLocalRotation);
			if (!this.IsHeld)
			{
				this.parentRig.RemoveLateUpdateCallback(this);
				this.isCallbackActive = false;
			}
		}
		else
		{
			this.interpolatedLocalPosition = Vector3.MoveTowards(this.interpolatedLocalPosition, this.NominalLocalPosition, this.positionLerpSpeed * Time.deltaTime);
			this.interpolatedLocalRotation = Quaternion.RotateTowards(this.interpolatedLocalRotation, this.NominalLocalRotation, this.rotationLerpSpeed * Time.deltaTime);
			base.transform.position = nominalParentTransform.TransformPoint(this.interpolatedLocalPosition);
			base.transform.rotation = nominalParentTransform.TransformRotation(this.interpolatedLocalRotation);
		}
		if (this.IsHeld)
		{
			if (this.parentRig.isLocal)
			{
				GTPlayer.Instance.SetHoverboardPosRot(base.transform.position, base.transform.rotation);
				return;
			}
			this.hoverboardAudio.UpdateAudioLoop(this.parentRig.LatestVelocity().magnitude, 0f, 0f, 0f);
		}
	}

	// Token: 0x060024B8 RID: 9400 RVA: 0x000B6D96 File Offset: 0x000B4F96
	public void PlayGrindHaptic()
	{
		if (this.IsHeld)
		{
			GorillaTagger.Instance.StartVibration(this.IsLeftHanded, this.grindHapticStrength, this.grindHapticDuration);
		}
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x000B6DBC File Offset: 0x000B4FBC
	public void PlayCarveHaptic(float carveForce)
	{
		if (this.IsHeld)
		{
			GorillaTagger.Instance.StartVibration(this.IsLeftHanded, carveForce * this.carveHapticStrength, this.carveHapticDuration);
		}
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x000B6DE4 File Offset: 0x000B4FE4
	public void ProxyGrabHandle(bool isLeftHand)
	{
		EquipmentInteractor.instance.UpdateHandEquipment(this.handlePosition, isLeftHand);
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x000B6DF9 File Offset: 0x000B4FF9
	public void DropFreeBoard()
	{
		FreeHoverboardManager.instance.SendDropBoardRPC(base.transform.position, base.transform.rotation, this.velocityEstimator.linearVelocity, this.velocityEstimator.angularVelocity, this.boardColor);
	}

	// Token: 0x060024BC RID: 9404 RVA: 0x000B6E37 File Offset: 0x000B5037
	public void SetRaceDisplay(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			this.racePositionReadout.gameObject.SetActive(false);
			return;
		}
		this.racePositionReadout.gameObject.SetActive(true);
		this.racePositionReadout.text = text;
	}

	// Token: 0x060024BD RID: 9405 RVA: 0x000B6E70 File Offset: 0x000B5070
	public void SetRaceLapsDisplay(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			this.raceLapsReadout.gameObject.SetActive(false);
			return;
		}
		this.raceLapsReadout.gameObject.SetActive(true);
		this.raceLapsReadout.text = text;
	}

	// Token: 0x040028D1 RID: 10449
	[SerializeField]
	private VRRig parentRig;

	// Token: 0x040028D2 RID: 10450
	[SerializeField]
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x040028D3 RID: 10451
	[SerializeField]
	[FormerlySerializedAs("audio")]
	private HoverboardAudio hoverboardAudio;

	// Token: 0x040028D4 RID: 10452
	[SerializeField]
	private HoverboardHandle handlePosition;

	// Token: 0x040028D5 RID: 10453
	[SerializeField]
	private float grindHapticStrength;

	// Token: 0x040028D6 RID: 10454
	[SerializeField]
	private float grindHapticDuration;

	// Token: 0x040028D7 RID: 10455
	[SerializeField]
	private float carveHapticStrength;

	// Token: 0x040028D8 RID: 10456
	[SerializeField]
	private float carveHapticDuration;

	// Token: 0x040028D9 RID: 10457
	[SerializeField]
	private MeshRenderer boardMesh;

	// Token: 0x040028DA RID: 10458
	[SerializeField]
	private InteractionPoint handleInteractionPoint;

	// Token: 0x040028DB RID: 10459
	[SerializeField]
	private TextMeshPro racePositionReadout;

	// Token: 0x040028DC RID: 10460
	[SerializeField]
	private TextMeshPro raceLapsReadout;

	// Token: 0x040028DD RID: 10461
	private Material colorMaterial;

	// Token: 0x040028E3 RID: 10467
	private Vector3 interpolatedLocalPosition;

	// Token: 0x040028E4 RID: 10468
	private Quaternion interpolatedLocalRotation;

	// Token: 0x040028E5 RID: 10469
	[SerializeField]
	private float lerpIntoHandDuration;

	// Token: 0x040028E6 RID: 10470
	private float positionLerpSpeed;

	// Token: 0x040028E7 RID: 10471
	private float rotationLerpSpeed;

	// Token: 0x040028E8 RID: 10472
	private bool isCallbackActive;
}
