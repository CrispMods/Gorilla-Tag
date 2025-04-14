using System;
using UnityEngine;

// Token: 0x02000579 RID: 1401
public class GorillaLightmapData : MonoBehaviour
{
	// Token: 0x0600227A RID: 8826 RVA: 0x000AB564 File Offset: 0x000A9764
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

	// Token: 0x04002612 RID: 9746
	[SerializeField]
	public Texture2D[] dirTextures;

	// Token: 0x04002613 RID: 9747
	[SerializeField]
	public Texture2D[] lightTextures;

	// Token: 0x04002614 RID: 9748
	public Color[][] lights;

	// Token: 0x04002615 RID: 9749
	public Color[][] dirs;

	// Token: 0x04002616 RID: 9750
	public bool done;
}
