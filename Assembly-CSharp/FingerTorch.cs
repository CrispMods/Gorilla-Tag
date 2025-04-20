using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020000E5 RID: 229
public class FingerTorch : MonoBehaviour, ISpawnable
{
	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060005D4 RID: 1492 RVA: 0x00034541 File Offset: 0x00032741
	// (set) Token: 0x060005D5 RID: 1493 RVA: 0x00034549 File Offset: 0x00032749
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060005D6 RID: 1494 RVA: 0x00034552 File Offset: 0x00032752
	// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0003455A File Offset: 0x0003275A
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x060005D8 RID: 1496 RVA: 0x00034563 File Offset: 0x00032763
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
		if (!this.myRig)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x00083A6C File Offset: 0x00081C6C
	protected void OnEnable()
	{
		int num = this.attachedToLeftHand ? 1 : 2;
		this.stateBitIndex = VRRig.WearablePackedStatesBitWriteInfos[num].index;
		this.OnExtendStateChanged(false);
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x00030607 File Offset: 0x0002E807
	protected void OnDisable()
	{
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x00083AA4 File Offset: 0x00081CA4
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

	// Token: 0x060005DD RID: 1501 RVA: 0x00034585 File Offset: 0x00032785
	private void UpdateShared()
	{
		if (this.extended != this.networkedExtended)
		{
			this.extended = this.networkedExtended;
			this.OnExtendStateChanged(true);
			this.particleFX.SetActive(this.extended);
		}
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x000345B9 File Offset: 0x000327B9
	private void UpdateReplicated()
	{
		if (this.myRig != null && !this.myRig.isOfflineVRRig)
		{
			this.networkedExtended = GTBitOps.ReadBit(this.myRig.WearablePackedStates, this.stateBitIndex);
		}
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x000345F2 File Offset: 0x000327F2
	public bool IsMyItem()
	{
		return this.myRig != null && this.myRig.isOfflineVRRig;
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0003460F File Offset: 0x0003280F
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

	// Token: 0x060005E1 RID: 1505 RVA: 0x00083B24 File Offset: 0x00081D24
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

	// Token: 0x040006D6 RID: 1750
	[Header("Wearable Settings")]
	public bool attachedToLeftHand = true;

	// Token: 0x040006D7 RID: 1751
	[Header("Bones")]
	public Transform pinkyRingBone;

	// Token: 0x040006D8 RID: 1752
	public Transform thumbRingBone;

	// Token: 0x040006D9 RID: 1753
	[Header("Audio")]
	public AudioSource audioSource;

	// Token: 0x040006DA RID: 1754
	public AudioClip extendAudioClip;

	// Token: 0x040006DB RID: 1755
	public AudioClip retractAudioClip;

	// Token: 0x040006DC RID: 1756
	[Header("Vibration")]
	public float extendVibrationDuration = 0.05f;

	// Token: 0x040006DD RID: 1757
	public float extendVibrationStrength = 0.2f;

	// Token: 0x040006DE RID: 1758
	public float retractVibrationDuration = 0.05f;

	// Token: 0x040006DF RID: 1759
	public float retractVibrationStrength = 0.2f;

	// Token: 0x040006E0 RID: 1760
	[Header("Particle FX")]
	public GameObject particleFX;

	// Token: 0x040006E1 RID: 1761
	private bool networkedExtended;

	// Token: 0x040006E2 RID: 1762
	private bool extended;

	// Token: 0x040006E3 RID: 1763
	private InputDevice inputDevice;

	// Token: 0x040006E4 RID: 1764
	private VRRig myRig;

	// Token: 0x040006E5 RID: 1765
	private int stateBitIndex;
}
