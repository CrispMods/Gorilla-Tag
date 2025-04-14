using System;
using GorillaExtensions;
using Photon.Pun;

// Token: 0x02000065 RID: 101
public struct CritterAppearance
{
	// Token: 0x06000287 RID: 647 RVA: 0x00010E6B File Offset: 0x0000F06B
	public CritterAppearance(string hatName, float size = 1f)
	{
		this.hatName = hatName;
		this.size = size;
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00010E7C File Offset: 0x0000F07C
	public object[] WriteToRPCData()
	{
		object[] array = new object[]
		{
			this.hatName,
			this.size
		};
		if (this.hatName == null)
		{
			array[0] = string.Empty;
		}
		if (this.size != 0f)
		{
			array[1] = this.size;
		}
		return array;
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00010ED3 File Offset: 0x0000F0D3
	public static int DataLength()
	{
		return 2;
	}

	// Token: 0x0600028A RID: 650 RVA: 0x00010ED8 File Offset: 0x0000F0D8
	public static bool ValidateData(object[] data)
	{
		float num;
		return data != null && data.Length == CritterAppearance.DataLength() && CrittersManager.ValidateDataType<float>(data[1], out num) && num >= 0f && !float.IsNaN(num) && !float.IsInfinity(num);
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00010F20 File Offset: 0x0000F120
	public static CritterAppearance ReadFromRPCData(object[] data)
	{
		string text;
		if (!CrittersManager.ValidateDataType<string>(data[0], out text))
		{
			return new CritterAppearance(string.Empty, 1f);
		}
		float value;
		if (!CrittersManager.ValidateDataType<float>(data[1], out value))
		{
			return new CritterAppearance(string.Empty, 1f);
		}
		return new CritterAppearance((string)data[0], value.GetFinite());
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00010F78 File Offset: 0x0000F178
	public static CritterAppearance ReadFromPhotonStream(PhotonStream data)
	{
		string text = (string)data.ReceiveNext();
		float num = (float)data.ReceiveNext();
		return new CritterAppearance(text, num);
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00010FA2 File Offset: 0x0000F1A2
	public override string ToString()
	{
		return string.Format("Size: {0} Hat: {1}", this.size, this.hatName);
	}

	// Token: 0x04000336 RID: 822
	public float size;

	// Token: 0x04000337 RID: 823
	public string hatName;
}
