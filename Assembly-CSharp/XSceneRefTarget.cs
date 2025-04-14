using System;
using UnityEngine;

// Token: 0x0200021C RID: 540
public class XSceneRefTarget : MonoBehaviour
{
	// Token: 0x06000C80 RID: 3200 RVA: 0x00042648 File Offset: 0x00040848
	private void Awake()
	{
		this.Register(false);
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x00042651 File Offset: 0x00040851
	private void Reset()
	{
		this.UniqueID = XSceneRefTarget.CreateNewID();
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x0004265E File Offset: 0x0004085E
	private void OnValidate()
	{
		if (!Application.isPlaying)
		{
			this.Register(false);
		}
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x00042670 File Offset: 0x00040870
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

	// Token: 0x06000C84 RID: 3204 RVA: 0x000426BC File Offset: 0x000408BC
	private void OnDestroy()
	{
		XSceneRefGlobalHub.Unregister(this.UniqueID, this);
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x000426CA File Offset: 0x000408CA
	private void AssignNewID()
	{
		this.UniqueID = XSceneRefTarget.CreateNewID();
		this.Register(false);
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x000426E0 File Offset: 0x000408E0
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

	// Token: 0x04000FE3 RID: 4067
	public int UniqueID;

	// Token: 0x04000FE4 RID: 4068
	[NonSerialized]
	private int lastRegisteredID = -1;

	// Token: 0x04000FE5 RID: 4069
	private static DateTime epoch = new DateTime(2024, 1, 1);

	// Token: 0x04000FE6 RID: 4070
	private static int lastAssignedID;
}
