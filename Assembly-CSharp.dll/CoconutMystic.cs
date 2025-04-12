using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

// Token: 0x020003D8 RID: 984
public class CoconutMystic : MonoBehaviour
{
	// Token: 0x060017CB RID: 6091 RVA: 0x0003F25A File Offset: 0x0003D45A
	private void Awake()
	{
		this.rig = base.GetComponentInParent<VRRig>();
	}

	// Token: 0x060017CC RID: 6092 RVA: 0x0003F268 File Offset: 0x0003D468
	private void OnEnable()
	{
		PhotonNetwork.NetworkingClient.EventReceived += this.OnPhotonEvent;
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x0003F280 File Offset: 0x0003D480
	private void OnDisable()
	{
		PhotonNetwork.NetworkingClient.EventReceived -= this.OnPhotonEvent;
	}

	// Token: 0x060017CE RID: 6094 RVA: 0x000C8074 File Offset: 0x000C6274
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

	// Token: 0x060017CF RID: 6095 RVA: 0x000C8114 File Offset: 0x000C6314
	public void UpdateLabel()
	{
		bool flag = this.geodeItem.currentState == TransferrableObject.PositionState.InLeftHand;
		this.label.rectTransform.localRotation = Quaternion.Euler(0f, flag ? 270f : 90f, 0f);
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x000C8160 File Offset: 0x000C6360
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

	// Token: 0x04001A62 RID: 6754
	public VRRig rig;

	// Token: 0x04001A63 RID: 6755
	public GeodeItem geodeItem;

	// Token: 0x04001A64 RID: 6756
	public SoundBankPlayer soundPlayer;

	// Token: 0x04001A65 RID: 6757
	public ParticleSystem breakEffect;

	// Token: 0x04001A66 RID: 6758
	public RandomStrings answers;

	// Token: 0x04001A67 RID: 6759
	public TMP_Text label;

	// Token: 0x04001A68 RID: 6760
	public bool distinct;

	// Token: 0x04001A69 RID: 6761
	private static readonly int kUpdateLabelEvent = "CoconutMystic.UpdateLabel".GetStaticHash();
}
