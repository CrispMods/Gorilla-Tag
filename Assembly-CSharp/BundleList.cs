using System;

// Token: 0x020003C5 RID: 965
internal class BundleList
{
	// Token: 0x06001741 RID: 5953 RVA: 0x00071ADC File Offset: 0x0006FCDC
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

	// Token: 0x06001742 RID: 5954 RVA: 0x00071C7C File Offset: 0x0006FE7C
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

	// Token: 0x06001743 RID: 5955 RVA: 0x00071CCB File Offset: 0x0006FECB
	public BundleData ActiveBundle()
	{
		return this.data[this.activeBundleIdx];
	}

	// Token: 0x040019F5 RID: 6645
	private int activeBundleIdx;

	// Token: 0x040019F6 RID: 6646
	public BundleData[] data;
}
