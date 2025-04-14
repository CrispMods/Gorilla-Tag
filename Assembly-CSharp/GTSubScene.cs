using System;
using UnityEngine;

// Token: 0x020008A2 RID: 2210
public class GTSubScene : ScriptableObject
{
	// Token: 0x0600357A RID: 13690 RVA: 0x000FE484 File Offset: 0x000FC684
	public void SwitchToScene(int index)
	{
		this.scenes[index].LoadAsync();
	}

	// Token: 0x0600357B RID: 13691 RVA: 0x000FE494 File Offset: 0x000FC694
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

	// Token: 0x0600357C RID: 13692 RVA: 0x000FE4D4 File Offset: 0x000FC6D4
	public void LoadAll()
	{
		for (int i = 0; i < this.scenes.Length; i++)
		{
			this.scenes[i].LoadAsync();
		}
	}

	// Token: 0x0600357D RID: 13693 RVA: 0x000FE504 File Offset: 0x000FC704
	public void UnloadAll()
	{
		for (int i = 0; i < this.scenes.Length; i++)
		{
			this.scenes[i].UnloadAsync();
		}
	}

	// Token: 0x040037C8 RID: 14280
	[DragDropScenes]
	public GTScene[] scenes = new GTScene[0];
}
