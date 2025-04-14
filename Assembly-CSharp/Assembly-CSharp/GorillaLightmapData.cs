using System;
using UnityEngine;

// Token: 0x0200057A RID: 1402
public class GorillaLightmapData : MonoBehaviour
{
	// Token: 0x06002282 RID: 8834 RVA: 0x000AB9E4 File Offset: 0x000A9BE4
	public void Awake()
	{
		this.lights = new Color[this.lightTextures.Length][];
		this.dirs = new Color[this.dirTextures.Length][];
		for (int i = 0; i < this.dirTextures.Length; i++)
		{
			float value = Random.value;
			Debug.Log(value.ToString() + " before load " + Time.realtimeSinceStartup.ToString());
			this.dirs[i] = this.dirTextures[i].GetPixels();
			this.lights[i] = this.lightTextures[i].GetPixels();
			Debug.Log(value.ToString() + " after load " + Time.realtimeSinceStartup.ToString());
		}
	}

	// Token: 0x04002618 RID: 9752
	[SerializeField]
	public Texture2D[] dirTextures;

	// Token: 0x04002619 RID: 9753
	[SerializeField]
	public Texture2D[] lightTextures;

	// Token: 0x0400261A RID: 9754
	public Color[][] lights;

	// Token: 0x0400261B RID: 9755
	public Color[][] dirs;

	// Token: 0x0400261C RID: 9756
	public bool done;
}
