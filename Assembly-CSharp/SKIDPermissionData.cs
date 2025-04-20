using System;
using KID.Model;

// Token: 0x0200071A RID: 1818
public struct SKIDPermissionData
{
	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x06002D30 RID: 11568 RVA: 0x0004EB9C File Offset: 0x0004CD9C
	public string PermissionName
	{
		get
		{
			return this._permissionName;
		}
	}

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x06002D31 RID: 11569 RVA: 0x0004EBA4 File Offset: 0x0004CDA4
	public bool PermissionEnabled
	{
		get
		{
			return this._permissionEnabled;
		}
	}

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x06002D32 RID: 11570 RVA: 0x0004EBAC File Offset: 0x0004CDAC
	public Permission PermissionData
	{
		get
		{
			return this._kidPermission;
		}
	}

	// Token: 0x06002D33 RID: 11571 RVA: 0x0004EBB4 File Offset: 0x0004CDB4
	public SKIDPermissionData(Permission perm, bool enabled, string name)
	{
		this._kidPermission = perm;
		this._permissionEnabled = enabled;
		this._permissionName = name;
	}

	// Token: 0x06002D34 RID: 11572 RVA: 0x0004EBCB File Offset: 0x0004CDCB
	public void SetPermission(bool isEnabled)
	{
		this._permissionEnabled = isEnabled;
	}

	// Token: 0x06002D35 RID: 11573 RVA: 0x0004EBD4 File Offset: 0x0004CDD4
	public static bool operator ==(SKIDPermissionData data, Permission rhs)
	{
		return data._kidPermission == rhs;
	}

	// Token: 0x06002D36 RID: 11574 RVA: 0x0004EBDF File Offset: 0x0004CDDF
	public static bool operator !=(SKIDPermissionData data, Permission rhs)
	{
		return data._kidPermission != rhs;
	}

	// Token: 0x0400328B RID: 12939
	private string _permissionName;

	// Token: 0x0400328C RID: 12940
	private Permission _kidPermission;

	// Token: 0x0400328D RID: 12941
	private bool _permissionEnabled;
}
