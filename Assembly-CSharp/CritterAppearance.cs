using System;
using GorillaExtensions;
using Photon.Pun;

// Token: 0x0200006B RID: 107
public struct CritterAppearance
{
	// Token: 0x060002B3 RID: 691 RVA: 0x00032172 File Offset: 0x00030372
	public CritterAppearance(string hatName, float size = 1f)
	{
		this.hatName = hatName;
		this.size = size;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x000757C4 File Offset: 0x000739C4
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

	// Token: 0x060002B5 RID: 693 RVA: 0x00032182 File Offset: 0x00030382
	public static int DataLength()
	{
		return 2;
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x0007581C File Offset: 0x00073A1C
	public static bool ValidateData(object[] data)
	{
		float num;
		return data != null && data.Length == CritterAppearance.DataLength() && CrittersManager.ValidateDataType<float>(data[1], out num) && num >= 0f && !float.IsNaN(num) && !float.IsInfinity(num);
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00075864 File Offset: 0x00073A64
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

	// Token: 0x060002B8 RID: 696 RVA: 0x000758BC File Offset: 0x00073ABC
	public static CritterAppearance ReadFromPhotonStream(PhotonStream data)
	{
		string text = (string)data.ReceiveNext();
		float num = (float)data.ReceiveNext();
		return new CritterAppearance(text, num);
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00032185 File Offset: 0x00030385
	public override string ToString()
	{
		return string.Format("Size: {0} Hat: {1}", this.size, this.hatName);
	}

	// Token: 0x04000367 RID: 871
	public float size;

	// Token: 0x04000368 RID: 872
	public string hatName;
}
