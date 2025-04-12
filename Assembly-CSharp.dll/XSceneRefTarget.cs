using System;
using UnityEngine;

// Token: 0x0200021C RID: 540
public class XSceneRefTarget : MonoBehaviour
{
	// Token: 0x06000C82 RID: 3202 RVA: 0x00037C40 File Offset: 0x00035E40
	private void Awake()
	{
		this.Register(false);
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x00037C49 File Offset: 0x00035E49
	private void Reset()
	{
		this.UniqueID = XSceneRefTarget.CreateNewID();
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x00037C56 File Offset: 0x00035E56
	private void OnValidate()
	{
		if (!Application.isPlaying)
		{
			this.Register(false);
		}
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x0009E034 File Offset: 0x0009C234
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

	// Token: 0x06000C86 RID: 3206 RVA: 0x00037C66 File Offset: 0x00035E66
	private void OnDestroy()
	{
		XSceneRefGlobalHub.Unregister(this.UniqueID, this);
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x00037C74 File Offset: 0x00035E74
	private void AssignNewID()
	{
		this.UniqueID = XSceneRefTarget.CreateNewID();
		this.Register(false);
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x0009E080 File Offset: 0x0009C280
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

	// Token: 0x04000FE4 RID: 4068
	public int UniqueID;

	// Token: 0x04000FE5 RID: 4069
	[NonSerialized]
	private int lastRegisteredID = -1;

	// Token: 0x04000FE6 RID: 4070
	private static DateTime epoch = new DateTime(2024, 1, 1);

	// Token: 0x04000FE7 RID: 4071
	private static int lastAssignedID;
}
