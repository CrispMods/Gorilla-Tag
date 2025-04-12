using System;
using GorillaNetworking;
using Photon.Pun;

// Token: 0x020005A9 RID: 1449
public class GroupJoinButton : GorillaPressableButton
{
	// Token: 0x060023F1 RID: 9201 RVA: 0x0004748E File Offset: 0x0004568E
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		if (this.inPrivate)
		{
			GorillaComputer.instance.OnGroupJoinButtonPress(this.gameModeIndex, this.friendCollider);
		}
	}

	// Token: 0x060023F2 RID: 9202 RVA: 0x000474B6 File Offset: 0x000456B6
	public void Update()
	{
		this.inPrivate = (PhotonNetwork.InRoom && !PhotonNetwork.CurrentRoom.IsVisible);
		if (!this.inPrivate)
		{
			this.isOn = true;
		}
	}

	// Token: 0x040027EE RID: 10222
	public int gameModeIndex;

	// Token: 0x040027EF RID: 10223
	public GorillaFriendCollider friendCollider;

	// Token: 0x040027F0 RID: 10224
	public bool inPrivate;
}
