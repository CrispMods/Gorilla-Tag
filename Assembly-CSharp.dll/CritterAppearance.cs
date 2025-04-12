using System;
using GorillaExtensions;
using Photon.Pun;

// Token: 0x02000065 RID: 101
public struct CritterAppearance
{
	// Token: 0x06000287 RID: 647 RVA: 0x00031008 File Offset: 0x0002F208
	public CritterAppearance(string hatName, float size = 1f)
	{
		this.hatName = hatName;
		this.size = size;
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00073178 File Offset: 0x00071378
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

	// Token: 0x06000289 RID: 649 RVA: 0x00031018 File Offset: 0x0002F218
	public static int DataLength()
	{
		return 2;
	}

	// Token: 0x0600028A RID: 650 RVA: 0x000731D0 File Offset: 0x000713D0
	public static bool ValidateData(object[] data)
	{
		float num;
		return data != null && data.Length == CritterAppearance.DataLength() && CrittersManager.ValidateDataType<float>(data[1], out num) && num >= 0f && !float.IsNaN(num) && !float.IsInfinity(num);
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00073218 File Offset: 0x00071418
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

	// Token: 0x0600028C RID: 652 RVA: 0x00073270 File Offset: 0x00071470
	public static CritterAppearance ReadFromPhotonStream(PhotonStream data)
	{
		string text = (string)data.ReceiveNext();
		float num = (float)data.ReceiveNext();
		return new CritterAppearance(text, num);
	}

	// Token: 0x0600028D RID: 653 RVA: 0x0003101B File Offset: 0x0002F21B
	public override string ToString()
	{
		return string.Format("Size: {0} Hat: {1}", this.size, this.hatName);
	}

	// Token: 0x04000336 RID: 822
	public float size;

	// Token: 0x04000337 RID: 823
	public string hatName;
}
