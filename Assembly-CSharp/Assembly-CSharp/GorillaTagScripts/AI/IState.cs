using System;

namespace GorillaTagScripts.AI
{
	// Token: 0x02000A14 RID: 2580
	public interface IState
	{
		// Token: 0x06004097 RID: 16535
		void Tick();

		// Token: 0x06004098 RID: 16536
		void OnEnter();

		// Token: 0x06004099 RID: 16537
		void OnExit();
	}
}
