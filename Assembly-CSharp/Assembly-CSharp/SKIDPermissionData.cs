using System;
using KID.Model;

// Token: 0x02000706 RID: 1798
public struct SKIDPermissionData
{
	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x000DC9CE File Offset: 0x000DABCE
	public string PermissionName
	{
		get
		{
			return this._permissionName;
		}
	}

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x000DC9D6 File Offset: 0x000DABD6
	public bool PermissionEnabled
	{
		get
		{
			return this._permissionEnabled;
		}
	}

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06002CA4 RID: 11428 RVA: 0x000DC9DE File Offset: 0x000DABDE
	public Permission PermissionData
	{
		get
		{
			return this._kidPermission;
		}
	}

	// Token: 0x06002CA5 RID: 11429 RVA: 0x000DC9E6 File Offset: 0x000DABE6
	public SKIDPermissionData(Permission perm, bool enabled, string name)
	{
		this._kidPermission = perm;
		this._permissionEnabled = enabled;
		this._permissionName = name;
	}

	// Token: 0x06002CA6 RID: 11430 RVA: 0x000DC9FD File Offset: 0x000DABFD
	public void SetPermission(bool isEnabled)
	{
		this._permissionEnabled = isEnabled;
	}

	// Token: 0x06002CA7 RID: 11431 RVA: 0x000DCA06 File Offset: 0x000DAC06
	public static bool operator ==(SKIDPermissionData data, Permission rhs)
	{
		return data._kidPermission == rhs;
	}

	// Token: 0x06002CA8 RID: 11432 RVA: 0x000DCA11 File Offset: 0x000DAC11
	public static bool operator !=(SKIDPermissionData data, Permission rhs)
	{
		return data._kidPermission != rhs;
	}

	// Token: 0x040031F4 RID: 12788
	private string _permissionName;

	// Token: 0x040031F5 RID: 12789
	private Permission _kidPermission;

	// Token: 0x040031F6 RID: 12790
	private bool _permissionEnabled;
}
