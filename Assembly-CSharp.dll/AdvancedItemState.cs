using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020003F9 RID: 1017
[Serializable]
public class AdvancedItemState
{
	// Token: 0x060018F2 RID: 6386 RVA: 0x0003FE1C File Offset: 0x0003E01C
	public void Encode()
	{
		this._encodedValue = this.EncodeData();
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x000CD8C8 File Offset: 0x000CBAC8
	public void Decode()
	{
		AdvancedItemState advancedItemState = this.DecodeData(this._encodedValue);
		this.index = advancedItemState.index;
		this.preData = advancedItemState.preData;
		this.limitAxis = advancedItemState.limitAxis;
		this.reverseGrip = advancedItemState.reverseGrip;
		this.angle = advancedItemState.angle;
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x000CD920 File Offset: 0x000CBB20
	public Quaternion GetQuaternion()
	{
		Vector3 one = Vector3.one;
		if (this.reverseGrip)
		{
			switch (this.limitAxis)
			{
			case LimitAxis.NoMovement:
				return Quaternion.identity;
			case LimitAxis.YAxis:
				return Quaternion.identity;
			case LimitAxis.XAxis:
			case LimitAxis.ZAxis:
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		return Quaternion.identity;
	}

	// Token: 0x060018F5 RID: 6389 RVA: 0x000CD974 File Offset: 0x000CBB74
	[return: TupleElementNames(new string[]
	{
		"grabPointIndex",
		"YRotation",
		"XRotation",
		"ZRotation"
	})]
	public ValueTuple<int, float, float, float> DecodeAdvancedItemState(int encodedValue)
	{
		int item = encodedValue >> 21 & 255;
		float item2 = (float)(encodedValue >> 14 & 127) / 128f * 360f;
		float item3 = (float)(encodedValue >> 7 & 127) / 128f * 360f;
		float item4 = (float)(encodedValue & 127) / 128f * 360f;
		return new ValueTuple<int, float, float, float>(item, item2, item3, item4);
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x060018F6 RID: 6390 RVA: 0x0003FE2A File Offset: 0x0003E02A
	private float EncodedDeltaRotation
	{
		get
		{
			return this.GetEncodedDeltaRotation();
		}
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x0003FE32 File Offset: 0x0003E032
	public float GetEncodedDeltaRotation()
	{
		return Mathf.Abs(Mathf.Atan2(this.angleVectorWhereUpIsStandard.x, this.angleVectorWhereUpIsStandard.y)) / 3.1415927f;
	}

	// Token: 0x060018F8 RID: 6392 RVA: 0x000CD9D0 File Offset: 0x000CBBD0
	public void DecodeDeltaRotation(float encodedDelta, bool isFlipped)
	{
		float f = encodedDelta * 3.1415927f;
		if (isFlipped)
		{
			this.angleVectorWhereUpIsStandard = new Vector2(-Mathf.Sin(f), Mathf.Cos(f));
		}
		else
		{
			this.angleVectorWhereUpIsStandard = new Vector2(Mathf.Sin(f), Mathf.Cos(f));
		}
		switch (this.limitAxis)
		{
		case LimitAxis.NoMovement:
		case LimitAxis.XAxis:
		case LimitAxis.ZAxis:
			return;
		case LimitAxis.YAxis:
		{
			Vector3 forward = new Vector3(this.angleVectorWhereUpIsStandard.x, 0f, this.angleVectorWhereUpIsStandard.y);
			Vector3 upwards = this.reverseGrip ? Vector3.down : Vector3.up;
			this.deltaRotation = Quaternion.LookRotation(forward, upwards);
			return;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x060018F9 RID: 6393 RVA: 0x000CDA84 File Offset: 0x000CBC84
	public int EncodeData()
	{
		int num = 0;
		if (this.index >= 32 | this.index < 0)
		{
			throw new ArgumentOutOfRangeException(string.Format("Index is invalid {0}", this.index));
		}
		num |= this.index << 25;
		AdvancedItemState.PointType pointType = this.preData.pointType;
		num |= (int)((int)(pointType & (AdvancedItemState.PointType)7) << 22);
		num |= (int)((int)this.limitAxis << 19);
		num |= (this.reverseGrip ? 1 : 0) << 18;
		bool flag = this.angleVectorWhereUpIsStandard.x < 0f;
		if (pointType != AdvancedItemState.PointType.Standard)
		{
			if (pointType != AdvancedItemState.PointType.DistanceBased)
			{
				throw new ArgumentOutOfRangeException();
			}
			int num2 = (int)(this.GetEncodedDeltaRotation() * 512f) & 511;
			num |= (flag ? 1 : 0) << 17;
			num |= num2 << 9;
			int num3 = (int)(this.preData.distAlongLine * 256f) & 255;
			num |= num3;
		}
		else
		{
			int num4 = (int)(this.GetEncodedDeltaRotation() * 65536f) & 65535;
			num |= (flag ? 1 : 0) << 17;
			num |= num4 << 1;
		}
		return num;
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x000CDBA0 File Offset: 0x000CBDA0
	public AdvancedItemState DecodeData(int encoded)
	{
		AdvancedItemState advancedItemState = new AdvancedItemState();
		advancedItemState.index = (encoded >> 25 & 31);
		advancedItemState.limitAxis = (LimitAxis)(encoded >> 19 & 7);
		advancedItemState.reverseGrip = ((encoded >> 18 & 1) == 1);
		AdvancedItemState.PointType pointType = (AdvancedItemState.PointType)(encoded >> 22 & 7);
		if (pointType != AdvancedItemState.PointType.Standard)
		{
			if (pointType != AdvancedItemState.PointType.DistanceBased)
			{
				throw new ArgumentOutOfRangeException();
			}
			advancedItemState.preData = new AdvancedItemState.PreData
			{
				pointType = pointType,
				distAlongLine = (float)(encoded & 255) / 256f
			};
			this.DecodeDeltaRotation((float)(encoded >> 9 & 511) / 512f, (encoded >> 17 & 1) > 0);
		}
		else
		{
			advancedItemState.preData = new AdvancedItemState.PreData
			{
				pointType = pointType
			};
			this.DecodeDeltaRotation((float)(encoded >> 1 & 65535) / 65536f, (encoded >> 17 & 1) > 0);
		}
		return advancedItemState;
	}

	// Token: 0x04001BCB RID: 7115
	private int _encodedValue;

	// Token: 0x04001BCC RID: 7116
	public Vector2 angleVectorWhereUpIsStandard;

	// Token: 0x04001BCD RID: 7117
	public Quaternion deltaRotation;

	// Token: 0x04001BCE RID: 7118
	public int index;

	// Token: 0x04001BCF RID: 7119
	public AdvancedItemState.PreData preData;

	// Token: 0x04001BD0 RID: 7120
	public LimitAxis limitAxis;

	// Token: 0x04001BD1 RID: 7121
	public bool reverseGrip;

	// Token: 0x04001BD2 RID: 7122
	public float angle;

	// Token: 0x020003FA RID: 1018
	[Serializable]
	public class PreData
	{
		// Token: 0x04001BD3 RID: 7123
		public float distAlongLine;

		// Token: 0x04001BD4 RID: 7124
		public AdvancedItemState.PointType pointType;
	}

	// Token: 0x020003FB RID: 1019
	public enum PointType
	{
		// Token: 0x04001BD6 RID: 7126
		Standard,
		// Token: 0x04001BD7 RID: 7127
		DistanceBased
	}
}
