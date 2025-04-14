using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020000BD RID: 189
public class FingerFlagWearable : MonoBehaviour, ISpawnable
{
	// Token: 0x1700005C RID: 92
	// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0001D22D File Offset: 0x0001B42D
	// (set) Token: 0x060004E1 RID: 1249 RVA: 0x0001D235 File Offset: 0x0001B435
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0001D23E File Offset: 0x0001B43E
	// (set) Token: 0x060004E3 RID: 1251 RVA: 0x0001D246 File Offset: 0x0001B446
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x060004E4 RID: 1252 RVA: 0x0001D24F File Offset: 0x0001B44F
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = base.GetComponentInParent<VRRig>(true);
		if (!this.myRig)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x0001D278 File Offset: 0x0001B478
	protected void OnEnable()
	{
		int num = this.attachedToLeftHand ? 1 : 2;
		this.stateBitIndex = VRRig.WearablePackedStatesBitWriteInfos[num].index;
		this.OnExtendStateChanged(false);
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x0001D2B0 File Offset: 0x0001B4B0
	private void UpdateLocal()
	{
		int node = this.attachedToLeftHand ? 4 : 5;
		bool flag = ControllerInputPoller.GripFloat((XRNode)node) > 0.25f;
		bool flag2 = ControllerInputPoller.PrimaryButtonPress((XRNode)node);
		bool flag3 = ControllerInputPoller.SecondaryButtonPress((XRNode)node);
		bool flag4 = flag && (flag2 || flag3);
		this.networkedExtended = flag4;
		if (PhotonNetwork.InRoom && this.myRig)
		{
			this.myRig.WearablePackedStates = GTBitOps.WriteBit(this.myRig.WearablePackedStates, this.stateBitIndex, this.networkedExtended);
		}
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x0001D330 File Offset: 0x0001B530
	private void UpdateShared()
	{
		if (this.extended != this.networkedExtended)
		{
			this.extended = this.networkedExtended;
			this.OnExtendStateChanged(true);
		}
		bool flag = this.fullyRetracted;
		this.fullyRetracted = (this.extended && this.retractExtendTime <= 0f);
		if (flag != this.fullyRetracted)
		{
			Transform[] array = this.clothRigidbodies;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(!this.fullyRetracted);
			}
		}
		this.UpdateAnimation();
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0001D3BE File Offset: 0x0001B5BE
	private void UpdateReplicated()
	{
		if (this.myRig != null && !this.myRig.isOfflineVRRig)
		{
			this.networkedExtended = GTBitOps.ReadBit(this.myRig.WearablePackedStates, this.stateBitIndex);
		}
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x0001D3F7 File Offset: 0x0001B5F7
	public bool IsMyItem()
	{
		return this.myRig != null && this.myRig.isOfflineVRRig;
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x0001D414 File Offset: 0x0001B614
	protected void LateUpdate()
	{
		if (this.IsMyItem())
		{
			this.UpdateLocal();
		}
		else
		{
			this.UpdateReplicated();
		}
		this.UpdateShared();
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x0001D434 File Offset: 0x0001B634
	private void UpdateAnimation()
	{
		float num = this.extended ? this.extendSpeed : (-this.retractSpeed);
		this.retractExtendTime = Mathf.Clamp01(this.retractExtendTime + Time.deltaTime * num);
		this.animator.SetFloat(this.retractExtendTimeAnimParam, this.retractExtendTime);
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x0001D48C File Offset: 0x0001B68C
	private void OnExtendStateChanged(bool playAudio)
	{
		this.audioSource.clip = (this.extended ? this.extendAudioClip : this.retractAudioClip);
		if (playAudio)
		{
			this.audioSource.GTPlay();
		}
		if (this.IsMyItem() && GorillaTagger.Instance)
		{
			GorillaTagger.Instance.StartVibration(this.attachedToLeftHand, this.extended ? this.extendVibrationDuration : this.retractVibrationDuration, this.extended ? this.extendVibrationStrength : this.retractVibrationStrength);
		}
	}

	// Token: 0x040005A8 RID: 1448
	[Header("Wearable Settings")]
	public bool attachedToLeftHand = true;

	// Token: 0x040005A9 RID: 1449
	[Header("Bones")]
	public Transform pinkyRingBone;

	// Token: 0x040005AA RID: 1450
	public Transform thumbRingBone;

	// Token: 0x040005AB RID: 1451
	public Transform[] clothBones;

	// Token: 0x040005AC RID: 1452
	public Transform[] clothRigidbodies;

	// Token: 0x040005AD RID: 1453
	[Header("Animation")]
	public Animator animator;

	// Token: 0x040005AE RID: 1454
	public float extendSpeed = 1.5f;

	// Token: 0x040005AF RID: 1455
	public float retractSpeed = 2.25f;

	// Token: 0x040005B0 RID: 1456
	[Header("Audio")]
	public AudioSource audioSource;

	// Token: 0x040005B1 RID: 1457
	public AudioClip extendAudioClip;

	// Token: 0x040005B2 RID: 1458
	public AudioClip retractAudioClip;

	// Token: 0x040005B3 RID: 1459
	[Header("Vibration")]
	public float extendVibrationDuration = 0.05f;

	// Token: 0x040005B4 RID: 1460
	public float extendVibrationStrength = 0.2f;

	// Token: 0x040005B5 RID: 1461
	public float retractVibrationDuration = 0.05f;

	// Token: 0x040005B6 RID: 1462
	public float retractVibrationStrength = 0.2f;

	// Token: 0x040005B7 RID: 1463
	private readonly int retractExtendTimeAnimParam = Animator.StringToHash("retractExtendTime");

	// Token: 0x040005B8 RID: 1464
	private bool networkedExtended;

	// Token: 0x040005B9 RID: 1465
	private bool extended;

	// Token: 0x040005BA RID: 1466
	private bool fullyRetracted;

	// Token: 0x040005BB RID: 1467
	private float retractExtendTime;

	// Token: 0x040005BC RID: 1468
	private InputDevice inputDevice;

	// Token: 0x040005BD RID: 1469
	private VRRig myRig;

	// Token: 0x040005BE RID: 1470
	private int stateBitIndex;
}
