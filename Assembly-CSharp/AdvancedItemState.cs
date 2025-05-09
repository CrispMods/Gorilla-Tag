﻿using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000404 RID: 1028
[Serializable]
public class AdvancedItemState
{
	// Token: 0x0600193C RID: 6460 RVA: 0x00041106 File Offset: 0x0003F306
	public void Encode()
	{
		this._encodedValue = this.EncodeData();
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x000D00F0 File Offset: 0x000CE2F0
	public void Decode()
	{
		AdvancedItemState advancedItemState = this.DecodeData(this._encodedValue);
		this.index = advancedItemState.index;
		this.preData = advancedItemState.preData;
		this.limitAxis = advancedItemState.limitAxis;
		this.reverseGrip = advancedItemState.reverseGrip;
		this.angle = advancedItemState.angle;
	}

	// Token: 0x0600193E RID: 6462 RVA: 0x000D0148 File Offset: 0x000CE348
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

	// Token: 0x0600193F RID: 6463 RVA: 0x000D019C File Offset: 0x000CE39C
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

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06001940 RID: 6464 RVA: 0x00041114 File Offset: 0x0003F314
	private float EncodedDeltaRotation
	{
		get
		{
			return this.GetEncodedDeltaRotation();
		}
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x0004111C File Offset: 0x0003F31C
	public float GetEncodedDeltaRotation()
	{
		return Mathf.Abs(Mathf.Atan2(this.angleVectorWhereUpIsStandard.x, this.angleVectorWhereUpIsStandard.y)) / 3.1415927f;
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x000D01F8 File Offset: 0x000CE3F8
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

	// Token: 0x06001943 RID: 6467 RVA: 0x000D02AC File Offset: 0x000CE4AC
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

	// Token: 0x06001944 RID: 6468 RVA: 0x000D03C8 File Offset: 0x000CE5C8
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

	// Token: 0x04001C13 RID: 7187
	private int _encodedValue;

	// Token: 0x04001C14 RID: 7188
	public Vector2 angleVectorWhereUpIsStandard;

	// Token: 0x04001C15 RID: 7189
	public Quaternion deltaRotation;

	// Token: 0x04001C16 RID: 7190
	public int index;

	// Token: 0x04001C17 RID: 7191
	public AdvancedItemState.PreData preData;

	// Token: 0x04001C18 RID: 7192
	public LimitAxis limitAxis;

	// Token: 0x04001C19 RID: 7193
	public bool reverseGrip;

	// Token: 0x04001C1A RID: 7194
	public float angle;

	// Token: 0x02000405 RID: 1029
	[Serializable]
	public class PreData
	{
		// Token: 0x04001C1B RID: 7195
		public float distAlongLine;

		// Token: 0x04001C1C RID: 7196
		public AdvancedItemState.PointType pointType;
	}

	// Token: 0x02000406 RID: 1030
	public enum PointType
	{
		// Token: 0x04001C1E RID: 7198
		Standard,
		// Token: 0x04001C1F RID: 7199
		DistanceBased
	}
}
