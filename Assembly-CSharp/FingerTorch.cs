using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020000DB RID: 219
public class FingerTorch : MonoBehaviour, ISpawnable
{
	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000593 RID: 1427 RVA: 0x00020E0F File Offset: 0x0001F00F
	// (set) Token: 0x06000594 RID: 1428 RVA: 0x00020E17 File Offset: 0x0001F017
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000595 RID: 1429 RVA: 0x00020E20 File Offset: 0x0001F020
	// (set) Token: 0x06000596 RID: 1430 RVA: 0x00020E28 File Offset: 0x0001F028
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000597 RID: 1431 RVA: 0x00020E31 File Offset: 0x0001F031
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
		if (!this.myRig)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x00020E54 File Offset: 0x0001F054
	protected void OnEnable()
	{
		int num = this.attachedToLeftHand ? 1 : 2;
		this.stateBitIndex = VRRig.WearablePackedStatesBitWriteInfos[num].index;
		this.OnExtendStateChanged(false);
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x000023F4 File Offset: 0x000005F4
	protected void OnDisable()
	{
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00020E8C File Offset: 0x0001F08C
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

	// Token: 0x0600059C RID: 1436 RVA: 0x00020F0C File Offset: 0x0001F10C
	private void UpdateShared()
	{
		if (this.extended != this.networkedExtended)
		{
			this.extended = this.networkedExtended;
			this.OnExtendStateChanged(true);
			this.particleFX.SetActive(this.extended);
		}
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x00020F40 File Offset: 0x0001F140
	private void UpdateReplicated()
	{
		if (this.myRig != null && !this.myRig.isOfflineVRRig)
		{
			this.networkedExtended = GTBitOps.ReadBit(this.myRig.WearablePackedStates, this.stateBitIndex);
		}
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00020F79 File Offset: 0x0001F179
	public bool IsMyItem()
	{
		return this.myRig != null && this.myRig.isOfflineVRRig;
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00020F96 File Offset: 0x0001F196
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

	// Token: 0x060005A0 RID: 1440 RVA: 0x00020FB4 File Offset: 0x0001F1B4
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

	// Token: 0x04000695 RID: 1685
	[Header("Wearable Settings")]
	public bool attachedToLeftHand = true;

	// Token: 0x04000696 RID: 1686
	[Header("Bones")]
	public Transform pinkyRingBone;

	// Token: 0x04000697 RID: 1687
	public Transform thumbRingBone;

	// Token: 0x04000698 RID: 1688
	[Header("Audio")]
	public AudioSource audioSource;

	// Token: 0x04000699 RID: 1689
	public AudioClip extendAudioClip;

	// Token: 0x0400069A RID: 1690
	public AudioClip retractAudioClip;

	// Token: 0x0400069B RID: 1691
	[Header("Vibration")]
	public float extendVibrationDuration = 0.05f;

	// Token: 0x0400069C RID: 1692
	public float extendVibrationStrength = 0.2f;

	// Token: 0x0400069D RID: 1693
	public float retractVibrationDuration = 0.05f;

	// Token: 0x0400069E RID: 1694
	public float retractVibrationStrength = 0.2f;

	// Token: 0x0400069F RID: 1695
	[Header("Particle FX")]
	public GameObject particleFX;

	// Token: 0x040006A0 RID: 1696
	private bool networkedExtended;

	// Token: 0x040006A1 RID: 1697
	private bool extended;

	// Token: 0x040006A2 RID: 1698
	private InputDevice inputDevice;

	// Token: 0x040006A3 RID: 1699
	private VRRig myRig;

	// Token: 0x040006A4 RID: 1700
	private int stateBitIndex;
}
