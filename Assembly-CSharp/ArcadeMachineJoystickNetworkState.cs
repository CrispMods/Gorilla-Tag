using System;
using Fusion;
using Photon.Pun;

// Token: 0x0200000C RID: 12
[NetworkBehaviourWeaved(0)]
public class ArcadeMachineJoystickNetworkState : NetworkComponent
{
	// Token: 0x06000030 RID: 48 RVA: 0x000306CE File Offset: 0x0002E8CE
	private new void Awake()
	{
		this.joystick = base.GetComponent<ArcadeMachineJoystick>();
	}

	// Token: 0x06000031 RID: 49 RVA: 0x000306DC File Offset: 0x0002E8DC
	public override void ReadDataFusion()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000032 RID: 50 RVA: 0x000306DC File Offset: 0x0002E8DC
	public override void WriteDataFusion()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000033 RID: 51 RVA: 0x000306E3 File Offset: 0x0002E8E3
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.joystick.ReadDataPUN(stream, info);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x000306F2 File Offset: 0x0002E8F2
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.joystick.WriteDataPUN(stream, info);
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00030709 File Offset: 0x0002E909
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00030715 File Offset: 0x0002E915
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}

	// Token: 0x04000018 RID: 24
	private ArcadeMachineJoystick joystick;
}
