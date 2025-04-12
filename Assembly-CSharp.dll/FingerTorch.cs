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
	// (get) Token: 0x06000595 RID: 1429 RVA: 0x000332DD File Offset: 0x000314DD
	// (set) Token: 0x06000596 RID: 1430 RVA: 0x000332E5 File Offset: 0x000314E5
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000597 RID: 1431 RVA: 0x000332EE File Offset: 0x000314EE
	// (set) Token: 0x06000598 RID: 1432 RVA: 0x000332F6 File Offset: 0x000314F6
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000599 RID: 1433 RVA: 0x000332FF File Offset: 0x000314FF
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
		if (!this.myRig)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0002F75F File Offset: 0x0002D95F
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00081164 File Offset: 0x0007F364
	protected void OnEnable()
	{
		int num = this.attachedToLeftHand ? 1 : 2;
		this.stateBitIndex = VRRig.WearablePackedStatesBitWriteInfos[num].index;
		this.OnExtendStateChanged(false);
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected void OnDisable()
	{
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0008119C File Offset: 0x0007F39C
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

	// Token: 0x0600059E RID: 1438 RVA: 0x00033321 File Offset: 0x00031521
	private void UpdateShared()
	{
		if (this.extended != this.networkedExtended)
		{
			this.extended = this.networkedExtended;
			this.OnExtendStateChanged(true);
			this.particleFX.SetActive(this.extended);
		}
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00033355 File Offset: 0x00031555
	private void UpdateReplicated()
	{
		if (this.myRig != null && !this.myRig.isOfflineVRRig)
		{
			this.networkedExtended = GTBitOps.ReadBit(this.myRig.WearablePackedStates, this.stateBitIndex);
		}
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0003338E File Offset: 0x0003158E
	public bool IsMyItem()
	{
		return this.myRig != null && this.myRig.isOfflineVRRig;
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x000333AB File Offset: 0x000315AB
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

	// Token: 0x060005A2 RID: 1442 RVA: 0x0008121C File Offset: 0x0007F41C
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

	// Token: 0x04000696 RID: 1686
	[Header("Wearable Settings")]
	public bool attachedToLeftHand = true;

	// Token: 0x04000697 RID: 1687
	[Header("Bones")]
	public Transform pinkyRingBone;

	// Token: 0x04000698 RID: 1688
	public Transform thumbRingBone;

	// Token: 0x04000699 RID: 1689
	[Header("Audio")]
	public AudioSource audioSource;

	// Token: 0x0400069A RID: 1690
	public AudioClip extendAudioClip;

	// Token: 0x0400069B RID: 1691
	public AudioClip retractAudioClip;

	// Token: 0x0400069C RID: 1692
	[Header("Vibration")]
	public float extendVibrationDuration = 0.05f;

	// Token: 0x0400069D RID: 1693
	public float extendVibrationStrength = 0.2f;

	// Token: 0x0400069E RID: 1694
	public float retractVibrationDuration = 0.05f;

	// Token: 0x0400069F RID: 1695
	public float retractVibrationStrength = 0.2f;

	// Token: 0x040006A0 RID: 1696
	[Header("Particle FX")]
	public GameObject particleFX;

	// Token: 0x040006A1 RID: 1697
	private bool networkedExtended;

	// Token: 0x040006A2 RID: 1698
	private bool extended;

	// Token: 0x040006A3 RID: 1699
	private InputDevice inputDevice;

	// Token: 0x040006A4 RID: 1700
	private VRRig myRig;

	// Token: 0x040006A5 RID: 1701
	private int stateBitIndex;
}
