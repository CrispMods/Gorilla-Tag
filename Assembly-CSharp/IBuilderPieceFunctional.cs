using System;

// Token: 0x020004D6 RID: 1238
public interface IBuilderPieceFunctional
{
	// Token: 0x06001E10 RID: 7696
	void OnStateChanged(byte newState, NetPlayer instigator, int timeStamp);

	// Token: 0x06001E11 RID: 7697
	void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp);

	// Token: 0x06001E12 RID: 7698
	bool IsStateValid(byte state);

	// Token: 0x06001E13 RID: 7699
	void FunctionalPieceUpdate();

	// Token: 0x06001E14 RID: 7700 RVA: 0x000306DC File Offset: 0x0002E8DC
	void FunctionalPieceFixedUpdate()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x00044876 File Offset: 0x00042A76
	float GetInteractionDistace()
	{
		return 2.5f;
	}
}
