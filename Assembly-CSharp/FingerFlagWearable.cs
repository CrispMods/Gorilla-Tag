using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020000C7 RID: 199
public class FingerFlagWearable : MonoBehaviour, ISpawnable
{
	// Token: 0x17000061 RID: 97
	// (get) Token: 0x0600051C RID: 1308 RVA: 0x00033CD3 File Offset: 0x00031ED3
	// (set) Token: 0x0600051D RID: 1309 RVA: 0x00033CDB File Offset: 0x00031EDB
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x0600051E RID: 1310 RVA: 0x00033CE4 File Offset: 0x00031EE4
	// (set) Token: 0x0600051F RID: 1311 RVA: 0x00033CEC File Offset: 0x00031EEC
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000520 RID: 1312 RVA: 0x00033CF5 File Offset: 0x00031EF5
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = base.GetComponentInParent<VRRig>(true);
		if (!this.myRig)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x000805F4 File Offset: 0x0007E7F4
	protected void OnEnable()
	{
		int num = this.attachedToLeftHand ? 1 : 2;
		this.stateBitIndex = VRRig.WearablePackedStatesBitWriteInfos[num].index;
		this.OnExtendStateChanged(false);
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0008062C File Offset: 0x0007E82C
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

	// Token: 0x06000524 RID: 1316 RVA: 0x000806AC File Offset: 0x0007E8AC
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

	// Token: 0x06000525 RID: 1317 RVA: 0x00033D1D File Offset: 0x00031F1D
	private void UpdateReplicated()
	{
		if (this.myRig != null && !this.myRig.isOfflineVRRig)
		{
			this.networkedExtended = GTBitOps.ReadBit(this.myRig.WearablePackedStates, this.stateBitIndex);
		}
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00033D56 File Offset: 0x00031F56
	public bool IsMyItem()
	{
		return this.myRig != null && this.myRig.isOfflineVRRig;
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00033D73 File Offset: 0x00031F73
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

	// Token: 0x06000528 RID: 1320 RVA: 0x0008073C File Offset: 0x0007E93C
	private void UpdateAnimation()
	{
		float num = this.extended ? this.extendSpeed : (-this.retractSpeed);
		this.retractExtendTime = Mathf.Clamp01(this.retractExtendTime + Time.deltaTime * num);
		this.animator.SetFloat(this.retractExtendTimeAnimParam, this.retractExtendTime);
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00080794 File Offset: 0x0007E994
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

	// Token: 0x040005E8 RID: 1512
	[Header("Wearable Settings")]
	public bool attachedToLeftHand = true;

	// Token: 0x040005E9 RID: 1513
	[Header("Bones")]
	public Transform pinkyRingBone;

	// Token: 0x040005EA RID: 1514
	public Transform thumbRingBone;

	// Token: 0x040005EB RID: 1515
	public Transform[] clothBones;

	// Token: 0x040005EC RID: 1516
	public Transform[] clothRigidbodies;

	// Token: 0x040005ED RID: 1517
	[Header("Animation")]
	public Animator animator;

	// Token: 0x040005EE RID: 1518
	public float extendSpeed = 1.5f;

	// Token: 0x040005EF RID: 1519
	public float retractSpeed = 2.25f;

	// Token: 0x040005F0 RID: 1520
	[Header("Audio")]
	public AudioSource audioSource;

	// Token: 0x040005F1 RID: 1521
	public AudioClip extendAudioClip;

	// Token: 0x040005F2 RID: 1522
	public AudioClip retractAudioClip;

	// Token: 0x040005F3 RID: 1523
	[Header("Vibration")]
	public float extendVibrationDuration = 0.05f;

	// Token: 0x040005F4 RID: 1524
	public float extendVibrationStrength = 0.2f;

	// Token: 0x040005F5 RID: 1525
	public float retractVibrationDuration = 0.05f;

	// Token: 0x040005F6 RID: 1526
	public float retractVibrationStrength = 0.2f;

	// Token: 0x040005F7 RID: 1527
	private readonly int retractExtendTimeAnimParam = Animator.StringToHash("retractExtendTime");

	// Token: 0x040005F8 RID: 1528
	private bool networkedExtended;

	// Token: 0x040005F9 RID: 1529
	private bool extended;

	// Token: 0x040005FA RID: 1530
	private bool fullyRetracted;

	// Token: 0x040005FB RID: 1531
	private float retractExtendTime;

	// Token: 0x040005FC RID: 1532
	private InputDevice inputDevice;

	// Token: 0x040005FD RID: 1533
	private VRRig myRig;

	// Token: 0x040005FE RID: 1534
	private int stateBitIndex;
}
