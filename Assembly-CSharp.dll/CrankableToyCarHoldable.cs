using System;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class CrankableToyCarHoldable : TransferrableObject
{
	// Token: 0x0600085E RID: 2142 RVA: 0x00034ED8 File Offset: 0x000330D8
	protected override void Start()
	{
		base.Start();
		this.crank.SetOnCrankedCallback(new Action<float>(this.OnCranked));
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0008C4DC File Offset: 0x0008A6DC
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

	// Token: 0x06000860 RID: 2144 RVA: 0x00034EF7 File Offset: 0x000330F7
	internal override void OnDisable()
	{
		base.OnDisable();
		if (this._events != null)
		{
			this._events.Dispose();
		}
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0008C5C8 File Offset: 0x0008A7C8
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

	// Token: 0x06000862 RID: 2146 RVA: 0x0008C61C File Offset: 0x0008A81C
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

	// Token: 0x06000863 RID: 2147 RVA: 0x0008C730 File Offset: 0x0008A930
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

	// Token: 0x06000864 RID: 2148 RVA: 0x00034F18 File Offset: 0x00033118
	private void DeployCarLocal(Vector3 launchPos, Quaternion launchRot, Vector3 releaseVel, float lifetime, bool isRemote = false)
	{
		if (!this.disabledWhileDeployed.activeSelf)
		{
			return;
		}
		this.deployedCar.Deploy(this, launchPos, launchRot, releaseVel, lifetime, isRemote);
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x0008C860 File Offset: 0x0008AA60
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

	// Token: 0x06000866 RID: 2150 RVA: 0x00034F3B File Offset: 0x0003313B
	public void OnCarDeployed()
	{
		this.itemState |= TransferrableObject.ItemStates.State0;
		this.deployablePart.SetActive(true);
		this.disabledWhileDeployed.SetActive(false);
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x00034F63 File Offset: 0x00033163
	public void OnCarReturned()
	{
		this.itemState &= (TransferrableObject.ItemStates)(-2);
		this.deployablePart.SetActive(false);
		this.disabledWhileDeployed.SetActive(true);
		this.clickSound.RestartSequence();
	}

	// Token: 0x040009EE RID: 2542
	[SerializeField]
	private TransferrableObjectHoldablePart_Crank crank;

	// Token: 0x040009EF RID: 2543
	[SerializeField]
	private CrankableToyCarDeployed deployedCar;

	// Token: 0x040009F0 RID: 2544
	[SerializeField]
	private GameObject deployablePart;

	// Token: 0x040009F1 RID: 2545
	[SerializeField]
	private GameObject disabledWhileDeployed;

	// Token: 0x040009F2 RID: 2546
	[SerializeField]
	private float crankAnglePerClick;

	// Token: 0x040009F3 RID: 2547
	[SerializeField]
	private float maxCrankStrength;

	// Token: 0x040009F4 RID: 2548
	[SerializeField]
	private float minClickPitch;

	// Token: 0x040009F5 RID: 2549
	[SerializeField]
	private float maxClickPitch;

	// Token: 0x040009F6 RID: 2550
	[SerializeField]
	private float minLifetime;

	// Token: 0x040009F7 RID: 2551
	[SerializeField]
	private float maxLifetime;

	// Token: 0x040009F8 RID: 2552
	[SerializeField]
	private SoundBankPlayer clickSound;

	// Token: 0x040009F9 RID: 2553
	[SerializeField]
	private SoundBankPlayer overCrankedSound;

	// Token: 0x040009FA RID: 2554
	[SerializeField]
	private float crankHapticStrength = 0.1f;

	// Token: 0x040009FB RID: 2555
	[SerializeField]
	private float crankHapticDuration = 0.05f;

	// Token: 0x040009FC RID: 2556
	[SerializeField]
	private float overcrankHapticStrength = 0.8f;

	// Token: 0x040009FD RID: 2557
	[SerializeField]
	private float overcrankHapticDuration = 0.05f;

	// Token: 0x040009FE RID: 2558
	private float currentCrankStrength;

	// Token: 0x040009FF RID: 2559
	private float currentCrankClickAmount;

	// Token: 0x04000A00 RID: 2560
	private RubberDuckEvents _events;
}
