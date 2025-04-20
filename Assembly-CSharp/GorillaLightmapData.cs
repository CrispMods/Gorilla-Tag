using System;
using UnityEngine;

// Token: 0x02000587 RID: 1415
public class GorillaLightmapData : MonoBehaviour
{
	// Token: 0x060022D8 RID: 8920 RVA: 0x000FB258 File Offset: 0x000F9458
	public void Awake()
	{
		this.lights = new Color[this.lightTextures.Length][];
		this.dirs = new Color[this.dirTextures.Length][];
		for (int i = 0; i < this.dirTextures.Length; i++)
		{
			float value = UnityEngine.Random.value;
			Debug.Log(value.ToString() + " before load " + Time.realtimeSinceStartup.ToString());
			this.dirs[i] = this.dirTextures[i].GetPixels();
			this.lights[i] = this.lightTextures[i].GetPixels();
			Debug.Log(value.ToString() + " after load " + Time.realtimeSinceStartup.ToString());
		}
	}

	// Token: 0x0400266A RID: 9834
	[SerializeField]
	public Texture2D[] dirTextures;

	// Token: 0x0400266B RID: 9835
	[SerializeField]
	public Texture2D[] lightTextures;

	// Token: 0x0400266C RID: 9836
	public Color[][] lights;

	// Token: 0x0400266D RID: 9837
	public Color[][] dirs;

	// Token: 0x0400266E RID: 9838
	public bool done;
}
