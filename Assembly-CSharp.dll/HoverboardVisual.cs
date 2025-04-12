using System;
using GorillaExtensions;
using GorillaLocomotion;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020005C6 RID: 1478
public class HoverboardVisual : MonoBehaviour, ICallBack
{
	// Token: 0x170003BA RID: 954
	// (get) Token: 0x060024B0 RID: 9392 RVA: 0x00047D61 File Offset: 0x00045F61
	// (set) Token: 0x060024B1 RID: 9393 RVA: 0x00047D69 File Offset: 0x00045F69
	public Color boardColor { get; private set; }

	// Token: 0x060024B2 RID: 9394 RVA: 0x00102130 File Offset: 0x00100330
	private void Awake()
	{
		Material[] sharedMaterials = this.boardMesh.sharedMaterials;
		this.colorMaterial = new Material(sharedMaterials[1]);
		sharedMaterials[1] = this.colorMaterial;
		this.boardMesh.sharedMaterials = sharedMaterials;
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x060024B3 RID: 9395 RVA: 0x00047D72 File Offset: 0x00045F72
	// (set) Token: 0x060024B4 RID: 9396 RVA: 0x00047D7A File Offset: 0x00045F7A
	public bool IsHeld { get; private set; }

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x060024B5 RID: 9397 RVA: 0x00047D83 File Offset: 0x00045F83
	// (set) Token: 0x060024B6 RID: 9398 RVA: 0x00047D8B File Offset: 0x00045F8B
	public bool IsLeftHanded { get; private set; }

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x060024B7 RID: 9399 RVA: 0x00047D94 File Offset: 0x00045F94
	// (set) Token: 0x060024B8 RID: 9400 RVA: 0x00047D9C File Offset: 0x00045F9C
	public Vector3 NominalLocalPosition { get; private set; }

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x060024B9 RID: 9401 RVA: 0x00047DA5 File Offset: 0x00045FA5
	// (set) Token: 0x060024BA RID: 9402 RVA: 0x00047DAD File Offset: 0x00045FAD
	public Quaternion NominalLocalRotation { get; private set; }

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x060024BB RID: 9403 RVA: 0x00047DB6 File Offset: 0x00045FB6
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

	// Token: 0x060024BC RID: 9404 RVA: 0x0010216C File Offset: 0x0010036C
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

	// Token: 0x060024BD RID: 9405 RVA: 0x00047DF6 File Offset: 0x00045FF6
	public void SetNotHeld(bool isLeftHanded)
	{
		this.IsLeftHanded = isLeftHanded;
		this.SetNotHeld();
	}

	// Token: 0x060024BE RID: 9406 RVA: 0x00102278 File Offset: 0x00100478
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

	// Token: 0x060024BF RID: 9407 RVA: 0x00102380 File Offset: 0x00100580
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

	// Token: 0x060024C0 RID: 9408 RVA: 0x00047E05 File Offset: 0x00046005
	public void PlayGrindHaptic()
	{
		if (this.IsHeld)
		{
			GorillaTagger.Instance.StartVibration(this.IsLeftHanded, this.grindHapticStrength, this.grindHapticDuration);
		}
	}

	// Token: 0x060024C1 RID: 9409 RVA: 0x00047E2B File Offset: 0x0004602B
	public void PlayCarveHaptic(float carveForce)
	{
		if (this.IsHeld)
		{
			GorillaTagger.Instance.StartVibration(this.IsLeftHanded, carveForce * this.carveHapticStrength, this.carveHapticDuration);
		}
	}

	// Token: 0x060024C2 RID: 9410 RVA: 0x00047E53 File Offset: 0x00046053
	public void ProxyGrabHandle(bool isLeftHand)
	{
		EquipmentInteractor.instance.UpdateHandEquipment(this.handlePosition, isLeftHand);
	}

	// Token: 0x060024C3 RID: 9411 RVA: 0x00047E68 File Offset: 0x00046068
	public void DropFreeBoard()
	{
		FreeHoverboardManager.instance.SendDropBoardRPC(base.transform.position, base.transform.rotation, this.velocityEstimator.linearVelocity, this.velocityEstimator.angularVelocity, this.boardColor);
	}

	// Token: 0x060024C4 RID: 9412 RVA: 0x00047EA6 File Offset: 0x000460A6
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

	// Token: 0x060024C5 RID: 9413 RVA: 0x00047EDF File Offset: 0x000460DF
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

	// Token: 0x040028D7 RID: 10455
	[SerializeField]
	private VRRig parentRig;

	// Token: 0x040028D8 RID: 10456
	[SerializeField]
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x040028D9 RID: 10457
	[SerializeField]
	[FormerlySerializedAs("audio")]
	private HoverboardAudio hoverboardAudio;

	// Token: 0x040028DA RID: 10458
	[SerializeField]
	private HoverboardHandle handlePosition;

	// Token: 0x040028DB RID: 10459
	[SerializeField]
	private float grindHapticStrength;

	// Token: 0x040028DC RID: 10460
	[SerializeField]
	private float grindHapticDuration;

	// Token: 0x040028DD RID: 10461
	[SerializeField]
	private float carveHapticStrength;

	// Token: 0x040028DE RID: 10462
	[SerializeField]
	private float carveHapticDuration;

	// Token: 0x040028DF RID: 10463
	[SerializeField]
	private MeshRenderer boardMesh;

	// Token: 0x040028E0 RID: 10464
	[SerializeField]
	private InteractionPoint handleInteractionPoint;

	// Token: 0x040028E1 RID: 10465
	[SerializeField]
	private TextMeshPro racePositionReadout;

	// Token: 0x040028E2 RID: 10466
	[SerializeField]
	private TextMeshPro raceLapsReadout;

	// Token: 0x040028E3 RID: 10467
	private Material colorMaterial;

	// Token: 0x040028E9 RID: 10473
	private Vector3 interpolatedLocalPosition;

	// Token: 0x040028EA RID: 10474
	private Quaternion interpolatedLocalRotation;

	// Token: 0x040028EB RID: 10475
	[SerializeField]
	private float lerpIntoHandDuration;

	// Token: 0x040028EC RID: 10476
	private float positionLerpSpeed;

	// Token: 0x040028ED RID: 10477
	private float rotationLerpSpeed;

	// Token: 0x040028EE RID: 10478
	private bool isCallbackActive;
}
