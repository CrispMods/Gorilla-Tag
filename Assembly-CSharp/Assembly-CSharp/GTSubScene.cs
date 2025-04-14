using System;
using UnityEngine;

// Token: 0x020008A5 RID: 2213
public class GTSubScene : ScriptableObject
{
	// Token: 0x06003586 RID: 13702 RVA: 0x000FEA4C File Offset: 0x000FCC4C
	public void SwitchToScene(int index)
	{
		this.scenes[index].LoadAsync();
	}

	// Token: 0x06003587 RID: 13703 RVA: 0x000FEA5C File Offset: 0x000FCC5C
	public void SwitchToScene(GTScene scene)
	{
		for (int i = 0; i < this.scenes.Length; i++)
		{
			GTScene gtscene = this.scenes[i];
			if (!(scene == gtscene))
			{
				gtscene.UnloadAsync();
			}
		}
		scene.LoadAsync();
	}

	// Token: 0x06003588 RID: 13704 RVA: 0x000FEA9C File Offset: 0x000FCC9C
	public void LoadAll()
	{
		for (int i = 0; i < this.scenes.Length; i++)
		{
			this.scenes[i].LoadAsync();
		}
	}

	// Token: 0x06003589 RID: 13705 RVA: 0x000FEACC File Offset: 0x000FCCCC
	public void UnloadAll()
	{
		for (int i = 0; i < this.scenes.Length; i++)
		{
			this.scenes[i].UnloadAsync();
		}
	}

	// Token: 0x040037DA RID: 14298
	[DragDropScenes]
	public GTScene[] scenes = new GTScene[0];
}
