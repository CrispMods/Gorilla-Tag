using System;
using UnityEngine;

// Token: 0x02000227 RID: 551
public class XSceneRefTarget : MonoBehaviour
{
	// Token: 0x06000CCB RID: 3275 RVA: 0x00038F00 File Offset: 0x00037100
	private void Awake()
	{
		this.Register(false);
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x00038F09 File Offset: 0x00037109
	private void Reset()
	{
		this.UniqueID = XSceneRefTarget.CreateNewID();
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x00038F16 File Offset: 0x00037116
	private void OnValidate()
	{
		if (!Application.isPlaying)
		{
			this.Register(false);
		}
	}

	// Token: 0x06000CCE RID: 3278 RVA: 0x000A08C0 File Offset: 0x0009EAC0
	public void Register(bool force = false)
	{
		if (this.UniqueID == this.lastRegisteredID && !force)
		{
			return;
		}
		if (this.lastRegisteredID != -1)
		{
			XSceneRefGlobalHub.Unregister(this.lastRegisteredID, this);
		}
		XSceneRefGlobalHub.Register(this.UniqueID, this);
		this.lastRegisteredID = this.UniqueID;
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x00038F26 File Offset: 0x00037126
	private void OnDestroy()
	{
		XSceneRefGlobalHub.Unregister(this.UniqueID, this);
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x00038F34 File Offset: 0x00037134
	private void AssignNewID()
	{
		this.UniqueID = XSceneRefTarget.CreateNewID();
		this.Register(false);
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x000A090C File Offset: 0x0009EB0C
	public static int CreateNewID()
	{
		int num = (int)((DateTime.Now - XSceneRefTarget.epoch).TotalSeconds * 8.0 % 2147483646.0) + 1;
		if (num <= XSceneRefTarget.lastAssignedID)
		{
			XSceneRefTarget.lastAssignedID++;
			return XSceneRefTarget.lastAssignedID;
		}
		XSceneRefTarget.lastAssignedID = num;
		return num;
	}

	// Token: 0x04001029 RID: 4137
	public int UniqueID;

	// Token: 0x0400102A RID: 4138
	[NonSerialized]
	private int lastRegisteredID = -1;

	// Token: 0x0400102B RID: 4139
	private static DateTime epoch = new DateTime(2024, 1, 1);

	// Token: 0x0400102C RID: 4140
	private static int lastAssignedID;
}
