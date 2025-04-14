using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

// Token: 0x020003D8 RID: 984
public class CoconutMystic : MonoBehaviour
{
	// Token: 0x060017C8 RID: 6088 RVA: 0x00073D44 File Offset: 0x00071F44
	private void Awake()
	{
		this.rig = base.GetComponentInParent<VRRig>();
	}

	// Token: 0x060017C9 RID: 6089 RVA: 0x00073D52 File Offset: 0x00071F52
	private void OnEnable()
	{
		PhotonNetwork.NetworkingClient.EventReceived += this.OnPhotonEvent;
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x00073D6A File Offset: 0x00071F6A
	private void OnDisable()
	{
		PhotonNetwork.NetworkingClient.EventReceived -= this.OnPhotonEvent;
	}

	// Token: 0x060017CB RID: 6091 RVA: 0x00073D84 File Offset: 0x00071F84
	private void OnPhotonEvent(EventData evData)
	{
		if (evData.Code != 176)
		{
			return;
		}
		object[] array = (object[])evData.CustomData;
		object obj = array[0];
		if (!(obj is int))
		{
			return;
		}
		int num = (int)obj;
		if (num != CoconutMystic.kUpdateLabelEvent)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(evData.Sender);
		NetPlayer owningNetPlayer = this.rig.OwningNetPlayer;
		if (player != owningNetPlayer)
		{
			return;
		}
		int index = (int)array[1];
		this.label.text = this.answers.GetItem(index);
		this.soundPlayer.Play();
		this.breakEffect.Play();
	}

	// Token: 0x060017CC RID: 6092 RVA: 0x00073E24 File Offset: 0x00072024
	public void UpdateLabel()
	{
		bool flag = this.geodeItem.currentState == TransferrableObject.PositionState.InLeftHand;
		this.label.rectTransform.localRotation = Quaternion.Euler(0f, flag ? 270f : 90f, 0f);
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x00073E70 File Offset: 0x00072070
	public void ShowAnswer()
	{
		this.answers.distinct = this.distinct;
		this.label.text = this.answers.NextItem();
		this.soundPlayer.Play();
		this.breakEffect.Play();
		object eventContent = new object[]
		{
			CoconutMystic.kUpdateLabelEvent,
			this.answers.lastItemIndex
		};
		PhotonNetwork.RaiseEvent(176, eventContent, RaiseEventOptions.Default, SendOptions.SendReliable);
	}

	// Token: 0x04001A61 RID: 6753
	public VRRig rig;

	// Token: 0x04001A62 RID: 6754
	public GeodeItem geodeItem;

	// Token: 0x04001A63 RID: 6755
	public SoundBankPlayer soundPlayer;

	// Token: 0x04001A64 RID: 6756
	public ParticleSystem breakEffect;

	// Token: 0x04001A65 RID: 6757
	public RandomStrings answers;

	// Token: 0x04001A66 RID: 6758
	public TMP_Text label;

	// Token: 0x04001A67 RID: 6759
	public bool distinct;

	// Token: 0x04001A68 RID: 6760
	private static readonly int kUpdateLabelEvent = "CoconutMystic.UpdateLabel".GetStaticHash();
}
