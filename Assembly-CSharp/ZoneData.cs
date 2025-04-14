using System;
using UnityEngine;

// Token: 0x02000215 RID: 533
[Serializable]
public class ZoneData
{
	// Token: 0x04000FC4 RID: 4036
	public GTZone zone;

	// Token: 0x04000FC5 RID: 4037
	public string sceneName;

	// Token: 0x04000FC6 RID: 4038
	public float CameraFarClipPlane = 500f;

	// Token: 0x04000FC7 RID: 4039
	public GameObject[] rootGameObjects;

	// Token: 0x04000FC8 RID: 4040
	[NonSerialized]
	public bool active;
}
