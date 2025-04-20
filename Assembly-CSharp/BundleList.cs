using System;

// Token: 0x020003D0 RID: 976
internal class BundleList
{
	// Token: 0x0600178E RID: 6030 RVA: 0x000C8C68 File Offset: 0x000C6E68
	public void FromJson(string jsonString)
	{
		this.data = JSonHelper.FromJson<BundleData>(jsonString);
		if (this.data.Length == 0)
		{
			return;
		}
		this.activeBundleIdx = 0;
		int majorVersion = this.data[0].majorVersion;
		int minorVersion = this.data[0].minorVersion;
		int minorVersion2 = this.data[0].minorVersion2;
		int gameMajorVersion = NetworkSystemConfig.GameMajorVersion;
		int gameMinorVersion = NetworkSystemConfig.GameMinorVersion;
		int gameMinorVersion2 = NetworkSystemConfig.GameMinorVersion2;
		for (int i = 1; i < this.data.Length; i++)
		{
			this.data[i].isActive = false;
			int num = gameMajorVersion * 1000000 + gameMinorVersion * 1000 + gameMinorVersion2;
			int num2 = this.data[i].majorVersion * 1000000 + this.data[i].minorVersion * 1000 + this.data[i].minorVersion2;
			if (num >= num2 && this.data[i].majorVersion >= majorVersion && this.data[i].minorVersion >= minorVersion && this.data[i].minorVersion2 >= minorVersion2)
			{
				this.activeBundleIdx = i;
				majorVersion = this.data[i].majorVersion;
				minorVersion = this.data[i].minorVersion;
				minorVersion2 = this.data[i].minorVersion2;
				break;
			}
		}
		this.data[this.activeBundleIdx].isActive = true;
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x000C8E08 File Offset: 0x000C7008
	public bool HasSku(string skuName, out int idx)
	{
		if (this.data == null)
		{
			idx = -1;
			return false;
		}
		for (int i = 0; i < this.data.Length; i++)
		{
			if (this.data[i].skuName == skuName)
			{
				idx = i;
				return true;
			}
		}
		idx = -1;
		return false;
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x0003FF14 File Offset: 0x0003E114
	public BundleData ActiveBundle()
	{
		return this.data[this.activeBundleIdx];
	}

	// Token: 0x04001A3E RID: 6718
	private int activeBundleIdx;

	// Token: 0x04001A3F RID: 6719
	public BundleData[] data;
}
