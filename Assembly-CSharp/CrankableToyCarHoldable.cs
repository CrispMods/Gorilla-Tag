using System;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class CrankableToyCarHoldable : TransferrableObject
{
	// Token: 0x060008A0 RID: 2208 RVA: 0x0003614E File Offset: 0x0003434E
	protected override void Start()
	{
		base.Start();
		this.crank.SetOnCrankedCallback(new Action<float>(this.OnCranked));
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x0008EE64 File Offset: 0x0008D064
	internal override void OnEnable()
	{
		base.OnEnable();
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this._events == null)
		{
			this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
		}
		NetPlayer netPlayer = (base.myOnlineRig != null) ? base.myOnlineRig.creator : ((base.myRig != null) ? ((base.myRig.creator != null) ? base.myRig.creator : NetworkSystem.Instance.LocalPlayer) : null);
		if (netPlayer != null && this._events != null)
		{
			this._events.Init(netPlayer);
			this._events.Activate += this.OnDeployRPC;
		}
		else
		{
			Debug.LogError("Failed to get a reference to the Photon Player needed to hook up the cosmetic event");
		}
		this.itemState &= (TransferrableObject.ItemStates)(-2);
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x0003616D File Offset: 0x0003436D
	internal override void OnDisable()
	{
		base.OnDisable();
		if (this._events != null)
		{
			this._events.Dispose();
		}
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x0008EF50 File Offset: 0x0008D150
	protected override void LateUpdateReplicated()
	{
		base.LateUpdateReplicated();
		if (this.itemState.HasFlag(TransferrableObject.ItemStates.State0))
		{
			if (!this.deployablePart.activeSelf)
			{
				this.OnCarDeployed();
				return;
			}
		}
		else if (this.deployablePart.activeSelf)
		{
			this.OnCarReturned();
		}
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0008EFA4 File Offset: 0x0008D1A4
	private void OnCranked(float deltaAngle)
	{
		this.currentCrankStrength += Mathf.Abs(deltaAngle);
		this.currentCrankClickAmount += deltaAngle;
		if (Mathf.Abs(this.currentCrankClickAmount) > this.crankAnglePerClick)
		{
			if (this.currentCrankStrength >= this.maxCrankStrength)
			{
				this.overCrankedSound.Play();
				VRRig ownerRig = this.ownerRig;
				if (ownerRig != null && ownerRig.isLocal)
				{
					GorillaTagger.Instance.StartVibration(base.InRightHand(), this.overcrankHapticStrength, this.overcrankHapticDuration);
				}
			}
			else
			{
				float value = Mathf.Lerp(this.minClickPitch, this.maxClickPitch, Mathf.InverseLerp(0f, this.maxCrankStrength, this.currentCrankStrength));
				SoundBankPlayer soundBankPlayer = this.clickSound;
				float? pitchOverride = new float?(value);
				soundBankPlayer.Play(null, pitchOverride);
				VRRig ownerRig2 = this.ownerRig;
				if (ownerRig2 != null && ownerRig2.isLocal)
				{
					GorillaTagger.Instance.StartVibration(base.InRightHand(), this.crankHapticStrength, this.crankHapticDuration);
				}
			}
			this.currentCrankClickAmount = 0f;
		}
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0008F0B8 File Offset: 0x0008D2B8
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		if (VRRigCache.Instance.localRig.Rig != this.ownerRig)
		{
			return false;
		}
		if (this.currentCrankStrength == 0f)
		{
			return true;
		}
		bool isRightHand = releasingHand == EquipmentInteractor.instance.rightHand;
		GorillaVelocityTracker interactPointVelocityTracker = GTPlayer.Instance.GetInteractPointVelocityTracker(isRightHand);
		Vector3 vector = base.transform.TransformPoint(Vector3.zero);
		Quaternion rotation = base.transform.rotation;
		Vector3 averageVelocity = interactPointVelocityTracker.GetAverageVelocity(true, 0.15f, false);
		float num = Mathf.Lerp(this.minLifetime, this.maxLifetime, Mathf.Clamp01(Mathf.InverseLerp(0f, this.maxCrankStrength, this.currentCrankStrength)));
		this.DeployCarLocal(vector, rotation, averageVelocity, num, false);
		if (PhotonNetwork.InRoom)
		{
			this._events.Activate.RaiseOthers(new object[]
			{
				BitPackUtils.PackWorldPosForNetwork(vector),
				BitPackUtils.PackQuaternionForNetwork(rotation),
				BitPackUtils.PackWorldPosForNetwork(averageVelocity * 100f),
				num
			});
		}
		this.currentCrankStrength = 0f;
		return true;
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0003618E File Offset: 0x0003438E
	private void DeployCarLocal(Vector3 launchPos, Quaternion launchRot, Vector3 releaseVel, float lifetime, bool isRemote = false)
	{
		if (!this.disabledWhileDeployed.activeSelf)
		{
			return;
		}
		this.deployedCar.Deploy(this, launchPos, launchRot, releaseVel, lifetime, isRemote);
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0008F1E8 File Offset: 0x0008D3E8
	private void OnDeployRPC(int sender, int receiver, object[] args, PhotonMessageInfoWrapped info)
	{
		if (!this || sender != receiver || info.senderID != this.ownerRig.creator.ActorNumber)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "OnDeployRPC");
		Vector3 launchPos = BitPackUtils.UnpackWorldPosFromNetwork((long)args[0]);
		Quaternion launchRot = BitPackUtils.UnpackQuaternionFromNetwork((int)args[1]);
		Vector3 releaseVel = BitPackUtils.UnpackWorldPosFromNetwork((long)args[2]) / 100f;
		float lifetime = (float)args[3];
		float num = 10000f;
		if (launchPos.IsValid(num) && launchRot.IsValid())
		{
			float num2 = 10000f;
			if (releaseVel.IsValid(num2))
			{
				this.DeployCarLocal(launchPos, launchRot, releaseVel, lifetime, true);
				return;
			}
		}
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x000361B1 File Offset: 0x000343B1
	public void OnCarDeployed()
	{
		this.itemState |= TransferrableObject.ItemStates.State0;
		this.deployablePart.SetActive(true);
		this.disabledWhileDeployed.SetActive(false);
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x000361D9 File Offset: 0x000343D9
	public void OnCarReturned()
	{
		this.itemState &= (TransferrableObject.ItemStates)(-2);
		this.deployablePart.SetActive(false);
		this.disabledWhileDeployed.SetActive(true);
		this.clickSound.RestartSequence();
	}

	// Token: 0x04000A30 RID: 2608
	[SerializeField]
	private TransferrableObjectHoldablePart_Crank crank;

	// Token: 0x04000A31 RID: 2609
	[SerializeField]
	private CrankableToyCarDeployed deployedCar;

	// Token: 0x04000A32 RID: 2610
	[SerializeField]
	private GameObject deployablePart;

	// Token: 0x04000A33 RID: 2611
	[SerializeField]
	private GameObject disabledWhileDeployed;

	// Token: 0x04000A34 RID: 2612
	[SerializeField]
	private float crankAnglePerClick;

	// Token: 0x04000A35 RID: 2613
	[SerializeField]
	private float maxCrankStrength;

	// Token: 0x04000A36 RID: 2614
	[SerializeField]
	private float minClickPitch;

	// Token: 0x04000A37 RID: 2615
	[SerializeField]
	private float maxClickPitch;

	// Token: 0x04000A38 RID: 2616
	[SerializeField]
	private float minLifetime;

	// Token: 0x04000A39 RID: 2617
	[SerializeField]
	private float maxLifetime;

	// Token: 0x04000A3A RID: 2618
	[SerializeField]
	private SoundBankPlayer clickSound;

	// Token: 0x04000A3B RID: 2619
	[SerializeField]
	private SoundBankPlayer overCrankedSound;

	// Token: 0x04000A3C RID: 2620
	[SerializeField]
	private float crankHapticStrength = 0.1f;

	// Token: 0x04000A3D RID: 2621
	[SerializeField]
	private float crankHapticDuration = 0.05f;

	// Token: 0x04000A3E RID: 2622
	[SerializeField]
	private float overcrankHapticStrength = 0.8f;

	// Token: 0x04000A3F RID: 2623
	[SerializeField]
	private float overcrankHapticDuration = 0.05f;

	// Token: 0x04000A40 RID: 2624
	private float currentCrankStrength;

	// Token: 0x04000A41 RID: 2625
	private float currentCrankClickAmount;

	// Token: 0x04000A42 RID: 2626
	private RubberDuckEvents _events;
}
