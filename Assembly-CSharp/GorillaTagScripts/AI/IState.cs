using System;

namespace GorillaTagScripts.AI
{
	// Token: 0x02000A11 RID: 2577
	public interface IState
	{
		// Token: 0x0600408B RID: 16523
		void Tick();

		// Token: 0x0600408C RID: 16524
		void OnEnter();

		// Token: 0x0600408D RID: 16525
		void OnExit();
	}
}
