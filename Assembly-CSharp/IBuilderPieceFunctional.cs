using System;

// Token: 0x020004C9 RID: 1225
public interface IBuilderPieceFunctional
{
	// Token: 0x06001DB7 RID: 7607
	void OnStateChanged(byte newState, NetPlayer instigator, int timeStamp);

	// Token: 0x06001DB8 RID: 7608
	void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp);

	// Token: 0x06001DB9 RID: 7609
	bool IsStateValid(byte state);

	// Token: 0x06001DBA RID: 7610
	void FunctionalPieceUpdate();

	// Token: 0x06001DBB RID: 7611 RVA: 0x00002628 File Offset: 0x00000828
	void FunctionalPieceFixedUpdate()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001DBC RID: 7612 RVA: 0x0009171F File Offset: 0x0008F91F
	float GetInteractionDistace()
	{
		return 2.5f;
	}
}
