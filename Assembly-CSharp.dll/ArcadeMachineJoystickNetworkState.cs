using System;
using Fusion;
using Photon.Pun;

// Token: 0x0200000C RID: 12
[NetworkBehaviourWeaved(0)]
public class ArcadeMachineJoystickNetworkState : NetworkComponent
{
	// Token: 0x06000030 RID: 48 RVA: 0x0002F826 File Offset: 0x0002DA26
	private new void Awake()
	{
		this.joystick = base.GetComponent<ArcadeMachineJoystick>();
	}

	// Token: 0x06000031 RID: 49 RVA: 0x0002F834 File Offset: 0x0002DA34
	public override void ReadDataFusion()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000032 RID: 50 RVA: 0x0002F834 File Offset: 0x0002DA34
	public override void WriteDataFusion()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000033 RID: 51 RVA: 0x0002F83B File Offset: 0x0002DA3B
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.joystick.ReadDataPUN(stream, info);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x0002F84A File Offset: 0x0002DA4A
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.joystick.WriteDataPUN(stream, info);
	}

	// Token: 0x06000036 RID: 54 RVA: 0x0002F861 File Offset: 0x0002DA61
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x0002F86D File Offset: 0x0002DA6D
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}

	// Token: 0x04000018 RID: 24
	private ArcadeMachineJoystick joystick;
}
