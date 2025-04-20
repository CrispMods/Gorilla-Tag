using System;

namespace BuildSafe
{
	// Token: 0x02000A5D RID: 2653
	public static class SceneView
	{
		// Token: 0x1400007E RID: 126
		// (add) Token: 0x0600426F RID: 17007 RVA: 0x00030607 File Offset: 0x0002E807
		// (remove) Token: 0x06004270 RID: 17008 RVA: 0x00030607 File Offset: 0x0002E807
		public static event Action duringSceneGui
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06004271 RID: 17009 RVA: 0x00030607 File Offset: 0x0002E807
		// (remove) Token: 0x06004272 RID: 17010 RVA: 0x00030607 File Offset: 0x0002E807
		public static event Action duringSceneGuiTick
		{
			add
			{
			}
			remove
			{
			}
		}
	}
}
