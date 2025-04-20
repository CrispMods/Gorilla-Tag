using System;
using GorillaNetworking;
using Photon.Pun;

// Token: 0x020005B6 RID: 1462
public class GroupJoinButton : GorillaPressableButton
{
	// Token: 0x06002449 RID: 9289 RVA: 0x00048863 File Offset: 0x00046A63
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		if (this.inPrivate)
		{
			GorillaComputer.instance.OnGroupJoinButtonPress(this.gameModeIndex, this.friendCollider);
		}
	}

	// Token: 0x0600244A RID: 9290 RVA: 0x0004888B File Offset: 0x00046A8B
	public void Update()
	{
		this.inPrivate = (PhotonNetwork.InRoom && !PhotonNetwork.CurrentRoom.IsVisible);
		if (!this.inPrivate)
		{
			this.isOn = true;
		}
	}

	// Token: 0x04002844 RID: 10308
	public int gameModeIndex;

	// Token: 0x04002845 RID: 10309
	public GorillaFriendCollider friendCollider;

	// Token: 0x04002846 RID: 10310
	public bool inPrivate;
}
