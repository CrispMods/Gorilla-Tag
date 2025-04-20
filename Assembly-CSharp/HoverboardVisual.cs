using System;
using GorillaExtensions;
using GorillaLocomotion;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020005D3 RID: 1491
public class HoverboardVisual : MonoBehaviour, ICallBack
{
	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x0600250A RID: 9482 RVA: 0x0004917C File Offset: 0x0004737C
	// (set) Token: 0x0600250B RID: 9483 RVA: 0x00049184 File Offset: 0x00047384
	public Color boardColor { get; private set; }

	// Token: 0x0600250C RID: 9484 RVA: 0x00105014 File Offset: 0x00103214
	private void Awake()
	{
		Material[] sharedMaterials = this.boardMesh.sharedMaterials;
		this.colorMaterial = new Material(sharedMaterials[1]);
		sharedMaterials[1] = this.colorMaterial;
		this.boardMesh.sharedMaterials = sharedMaterials;
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x0600250D RID: 9485 RVA: 0x0004918D File Offset: 0x0004738D
	// (set) Token: 0x0600250E RID: 9486 RVA: 0x00049195 File Offset: 0x00047395
	public bool IsHeld { get; private set; }

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x0600250F RID: 9487 RVA: 0x0004919E File Offset: 0x0004739E
	// (set) Token: 0x06002510 RID: 9488 RVA: 0x000491A6 File Offset: 0x000473A6
	public bool IsLeftHanded { get; private set; }

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x06002511 RID: 9489 RVA: 0x000491AF File Offset: 0x000473AF
	// (set) Token: 0x06002512 RID: 9490 RVA: 0x000491B7 File Offset: 0x000473B7
	public Vector3 NominalLocalPosition { get; private set; }

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x06002513 RID: 9491 RVA: 0x000491C0 File Offset: 0x000473C0
	// (set) Token: 0x06002514 RID: 9492 RVA: 0x000491C8 File Offset: 0x000473C8
	public Quaternion NominalLocalRotation { get; private set; }

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x06002515 RID: 9493 RVA: 0x000491D1 File Offset: 0x000473D1
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

	// Token: 0x06002516 RID: 9494 RVA: 0x00105050 File Offset: 0x00103250
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

	// Token: 0x06002517 RID: 9495 RVA: 0x00049211 File Offset: 0x00047411
	public void SetNotHeld(bool isLeftHanded)
	{
		this.IsLeftHanded = isLeftHanded;
		this.SetNotHeld();
	}

	// Token: 0x06002518 RID: 9496 RVA: 0x0010515C File Offset: 0x0010335C
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

	// Token: 0x06002519 RID: 9497 RVA: 0x00105264 File Offset: 0x00103464
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

	// Token: 0x0600251A RID: 9498 RVA: 0x00049220 File Offset: 0x00047420
	public void PlayGrindHaptic()
	{
		if (this.IsHeld)
		{
			GorillaTagger.Instance.StartVibration(this.IsLeftHanded, this.grindHapticStrength, this.grindHapticDuration);
		}
	}

	// Token: 0x0600251B RID: 9499 RVA: 0x00049246 File Offset: 0x00047446
	public void PlayCarveHaptic(float carveForce)
	{
		if (this.IsHeld)
		{
			GorillaTagger.Instance.StartVibration(this.IsLeftHanded, carveForce * this.carveHapticStrength, this.carveHapticDuration);
		}
	}

	// Token: 0x0600251C RID: 9500 RVA: 0x0004926E File Offset: 0x0004746E
	public void ProxyGrabHandle(bool isLeftHand)
	{
		EquipmentInteractor.instance.UpdateHandEquipment(this.handlePosition, isLeftHand);
	}

	// Token: 0x0600251D RID: 9501 RVA: 0x00049283 File Offset: 0x00047483
	public void DropFreeBoard()
	{
		FreeHoverboardManager.instance.SendDropBoardRPC(base.transform.position, base.transform.rotation, this.velocityEstimator.linearVelocity, this.velocityEstimator.angularVelocity, this.boardColor);
	}

	// Token: 0x0600251E RID: 9502 RVA: 0x000492C1 File Offset: 0x000474C1
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

	// Token: 0x0600251F RID: 9503 RVA: 0x000492FA File Offset: 0x000474FA
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

	// Token: 0x04002930 RID: 10544
	[SerializeField]
	private VRRig parentRig;

	// Token: 0x04002931 RID: 10545
	[SerializeField]
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04002932 RID: 10546
	[SerializeField]
	[FormerlySerializedAs("audio")]
	private HoverboardAudio hoverboardAudio;

	// Token: 0x04002933 RID: 10547
	[SerializeField]
	private HoverboardHandle handlePosition;

	// Token: 0x04002934 RID: 10548
	[SerializeField]
	private float grindHapticStrength;

	// Token: 0x04002935 RID: 10549
	[SerializeField]
	private float grindHapticDuration;

	// Token: 0x04002936 RID: 10550
	[SerializeField]
	private float carveHapticStrength;

	// Token: 0x04002937 RID: 10551
	[SerializeField]
	private float carveHapticDuration;

	// Token: 0x04002938 RID: 10552
	[SerializeField]
	private MeshRenderer boardMesh;

	// Token: 0x04002939 RID: 10553
	[SerializeField]
	private InteractionPoint handleInteractionPoint;

	// Token: 0x0400293A RID: 10554
	[SerializeField]
	private TextMeshPro racePositionReadout;

	// Token: 0x0400293B RID: 10555
	[SerializeField]
	private TextMeshPro raceLapsReadout;

	// Token: 0x0400293C RID: 10556
	private Material colorMaterial;

	// Token: 0x04002942 RID: 10562
	private Vector3 interpolatedLocalPosition;

	// Token: 0x04002943 RID: 10563
	private Quaternion interpolatedLocalRotation;

	// Token: 0x04002944 RID: 10564
	[SerializeField]
	private float lerpIntoHandDuration;

	// Token: 0x04002945 RID: 10565
	private float positionLerpSpeed;

	// Token: 0x04002946 RID: 10566
	private float rotationLerpSpeed;

	// Token: 0x04002947 RID: 10567
	private bool isCallbackActive;
}
