using System;
using KID.Model;

// Token: 0x02000705 RID: 1797
public struct SKIDPermissionData
{
	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x06002C9A RID: 11418 RVA: 0x000DC54E File Offset: 0x000DA74E
	public string PermissionName
	{
		get
		{
			return this._permissionName;
		}
	}

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x06002C9B RID: 11419 RVA: 0x000DC556 File Offset: 0x000DA756
	public bool PermissionEnabled
	{
		get
		{
			return this._permissionEnabled;
		}
	}

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x06002C9C RID: 11420 RVA: 0x000DC55E File Offset: 0x000DA75E
	public Permission PermissionData
	{
		get
		{
			return this._kidPermission;
		}
	}

	// Token: 0x06002C9D RID: 11421 RVA: 0x000DC566 File Offset: 0x000DA766
	public SKIDPermissionData(Permission perm, bool enabled, string name)
	{
		this._kidPermission = perm;
		this._permissionEnabled = enabled;
		this._permissionName = name;
	}

	// Token: 0x06002C9E RID: 11422 RVA: 0x000DC57D File Offset: 0x000DA77D
	public void SetPermission(bool isEnabled)
	{
		this._permissionEnabled = isEnabled;
	}

	// Token: 0x06002C9F RID: 11423 RVA: 0x000DC586 File Offset: 0x000DA786
	public static bool operator ==(SKIDPermissionData data, Permission rhs)
	{
		return data._kidPermission == rhs;
	}

	// Token: 0x06002CA0 RID: 11424 RVA: 0x000DC591 File Offset: 0x000DA791
	public static bool operator !=(SKIDPermissionData data, Permission rhs)
	{
		return data._kidPermission != rhs;
	}

	// Token: 0x040031EE RID: 12782
	private string _permissionName;

	// Token: 0x040031EF RID: 12783
	private Permission _kidPermission;

	// Token: 0x040031F0 RID: 12784
	private bool _permissionEnabled;
}
