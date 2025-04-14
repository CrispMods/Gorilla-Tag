using System;
using Fusion;
using Photon.Pun;

// Token: 0x0200000C RID: 12
[NetworkBehaviourWeaved(0)]
public class ArcadeMachineJoystickNetworkState : NetworkComponent
{
	// Token: 0x06000030 RID: 48 RVA: 0x0000261A File Offset: 0x0000081A
	private new void Awake()
	{
		this.joystick = base.GetComponent<ArcadeMachineJoystick>();
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002628 File Offset: 0x00000828
	public override void ReadDataFusion()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002628 File Offset: 0x00000828
	public override void WriteDataFusion()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000033 RID: 51 RVA: 0x0000262F File Offset: 0x0000082F
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.joystick.ReadDataPUN(stream, info);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x0000263E File Offset: 0x0000083E
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.joystick.WriteDataPUN(stream, info);
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002655 File Offset: 0x00000855
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002661 File Offset: 0x00000861
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}

	// Token: 0x04000018 RID: 24
	private ArcadeMachineJoystick joystick;
}
