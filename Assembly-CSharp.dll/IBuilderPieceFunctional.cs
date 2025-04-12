using System;

// Token: 0x020004C9 RID: 1225
public interface IBuilderPieceFunctional
{
	// Token: 0x06001DBA RID: 7610
	void OnStateChanged(byte newState, NetPlayer instigator, int timeStamp);

	// Token: 0x06001DBB RID: 7611
	void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp);

	// Token: 0x06001DBC RID: 7612
	bool IsStateValid(byte state);

	// Token: 0x06001DBD RID: 7613
	void FunctionalPieceUpdate();

	// Token: 0x06001DBE RID: 7614 RVA: 0x0002F834 File Offset: 0x0002DA34
	void FunctionalPieceFixedUpdate()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001DBF RID: 7615 RVA: 0x000434D7 File Offset: 0x000416D7
	float GetInteractionDistace()
	{
		return 2.5f;
	}
}
