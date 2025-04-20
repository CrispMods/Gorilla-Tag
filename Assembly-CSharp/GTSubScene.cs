using System;
using UnityEngine;

// Token: 0x020008BE RID: 2238
public class GTSubScene : ScriptableObject
{
	// Token: 0x06003642 RID: 13890 RVA: 0x00053BBE File Offset: 0x00051DBE
	public void SwitchToScene(int index)
	{
		this.scenes[index].LoadAsync();
	}

	// Token: 0x06003643 RID: 13891 RVA: 0x00144614 File Offset: 0x00142814
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

	// Token: 0x06003644 RID: 13892 RVA: 0x00144654 File Offset: 0x00142854
	public void LoadAll()
	{
		for (int i = 0; i < this.scenes.Length; i++)
		{
			this.scenes[i].LoadAsync();
		}
	}

	// Token: 0x06003645 RID: 13893 RVA: 0x00144684 File Offset: 0x00142884
	public void UnloadAll()
	{
		for (int i = 0; i < this.scenes.Length; i++)
		{
			this.scenes[i].UnloadAsync();
		}
	}

	// Token: 0x04003889 RID: 14473
	[DragDropScenes]
	public GTScene[] scenes = new GTScene[0];
}
