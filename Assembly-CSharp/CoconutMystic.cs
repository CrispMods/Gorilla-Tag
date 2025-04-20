using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

// Token: 0x020003E3 RID: 995
public class CoconutMystic : MonoBehaviour
{
	// Token: 0x06001815 RID: 6165 RVA: 0x00040544 File Offset: 0x0003E744
	private void Awake()
	{
		this.rig = base.GetComponentInParent<VRRig>();
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x00040552 File Offset: 0x0003E752
	private void OnEnable()
	{
		PhotonNetwork.NetworkingClient.EventReceived += this.OnPhotonEvent;
	}

	// Token: 0x06001817 RID: 6167 RVA: 0x0004056A File Offset: 0x0003E76A
	private void OnDisable()
	{
		PhotonNetwork.NetworkingClient.EventReceived -= this.OnPhotonEvent;
	}

	// Token: 0x06001818 RID: 6168 RVA: 0x000CA89C File Offset: 0x000C8A9C
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

	// Token: 0x06001819 RID: 6169 RVA: 0x000CA93C File Offset: 0x000C8B3C
	public void UpdateLabel()
	{
		bool flag = this.geodeItem.currentState == TransferrableObject.PositionState.InLeftHand;
		this.label.rectTransform.localRotation = Quaternion.Euler(0f, flag ? 270f : 90f, 0f);
	}

	// Token: 0x0600181A RID: 6170 RVA: 0x000CA988 File Offset: 0x000C8B88
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

	// Token: 0x04001AAA RID: 6826
	public VRRig rig;

	// Token: 0x04001AAB RID: 6827
	public GeodeItem geodeItem;

	// Token: 0x04001AAC RID: 6828
	public SoundBankPlayer soundPlayer;

	// Token: 0x04001AAD RID: 6829
	public ParticleSystem breakEffect;

	// Token: 0x04001AAE RID: 6830
	public RandomStrings answers;

	// Token: 0x04001AAF RID: 6831
	public TMP_Text label;

	// Token: 0x04001AB0 RID: 6832
	public bool distinct;

	// Token: 0x04001AB1 RID: 6833
	private static readonly int kUpdateLabelEvent = "CoconutMystic.UpdateLabel".GetStaticHash();
}
