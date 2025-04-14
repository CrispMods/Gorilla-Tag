using System;
using GorillaExtensions;
using Photon.Pun;

// Token: 0x02000065 RID: 101
public struct CritterAppearance
{
	// Token: 0x06000285 RID: 645 RVA: 0x00010AC7 File Offset: 0x0000ECC7
	public CritterAppearance(string hatName, float size = 1f)
	{
		this.hatName = hatName;
		this.size = size;
	}

	// Token: 0x06000286 RID: 646 RVA: 0x00010AD8 File Offset: 0x0000ECD8
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

	// Token: 0x06000287 RID: 647 RVA: 0x00010B2F File Offset: 0x0000ED2F
	public static int DataLength()
	{
		return 2;
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00010B34 File Offset: 0x0000ED34
	public static bool ValidateData(object[] data)
	{
		float num;
		return data.Length == CritterAppearance.DataLength() && CrittersManager.ValidateDataType<float>(data[1], out num) && num >= 0f && !float.IsNaN(num) && !float.IsInfinity(num);
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00010B78 File Offset: 0x0000ED78
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

	// Token: 0x0600028A RID: 650 RVA: 0x00010BD0 File Offset: 0x0000EDD0
	public static CritterAppearance ReadFromPhotonStream(PhotonStream data)
	{
		string text = (string)data.ReceiveNext();
		float num = (float)data.ReceiveNext();
		return new CritterAppearance(text, num);
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00010BFA File Offset: 0x0000EDFA
	public override string ToString()
	{
		return string.Format("Size: {0} Hat: {1}", this.size, this.hatName);
	}

	// Token: 0x04000335 RID: 821
	public float size;

	// Token: 0x04000336 RID: 822
	public string hatName;
}
