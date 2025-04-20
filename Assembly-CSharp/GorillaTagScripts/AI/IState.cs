using System;

namespace GorillaTagScripts.AI
{
	// Token: 0x02000A3E RID: 2622
	public interface IState
	{
		// Token: 0x060041D0 RID: 16848
		void Tick();

		// Token: 0x060041D1 RID: 16849
		void OnEnter();

		// Token: 0x060041D2 RID: 16850
		void OnExit();
	}
}
