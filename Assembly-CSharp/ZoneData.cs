using System;
using UnityEngine;

// Token: 0x02000220 RID: 544
[Serializable]
public class ZoneData
{
	// Token: 0x0400100A RID: 4106
	public GTZone zone;

	// Token: 0x0400100B RID: 4107
	public string sceneName;

	// Token: 0x0400100C RID: 4108
	public float CameraFarClipPlane = 500f;

	// Token: 0x0400100D RID: 4109
	public GameObject[] rootGameObjects;

	// Token: 0x0400100E RID: 4110
	[NonSerialized]
	public bool active;
}
