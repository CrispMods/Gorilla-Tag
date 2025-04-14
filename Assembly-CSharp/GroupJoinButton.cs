using System;
using GorillaNetworking;
using Photon.Pun;

// Token: 0x020005A8 RID: 1448
public class GroupJoinButton : GorillaPressableButton
{
	// Token: 0x060023E9 RID: 9193 RVA: 0x000B3126 File Offset: 0x000B1326
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		if (this.inPrivate)
		{
			GorillaComputer.instance.OnGroupJoinButtonPress(this.gameModeIndex, this.friendCollider);
		}
	}

	// Token: 0x060023EA RID: 9194 RVA: 0x000B314E File Offset: 0x000B134E
	public void Update()
	{
		this.inPrivate = (PhotonNetwork.InRoom && !PhotonNetwork.CurrentRoom.IsVisible);
		if (!this.inPrivate)
		{
			this.isOn = true;
		}
	}

	// Token: 0x040027E8 RID: 10216
	public int gameModeIndex;

	// Token: 0x040027E9 RID: 10217
	public GorillaFriendCollider friendCollider;

	// Token: 0x040027EA RID: 10218
	public bool inPrivate;
}
