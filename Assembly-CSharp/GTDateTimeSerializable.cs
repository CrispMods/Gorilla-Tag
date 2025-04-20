using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000190 RID: 400
[Serializable]
public struct GTDateTimeSerializable : ISerializationCallbackReceiver
{
	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000A06 RID: 2566 RVA: 0x000370A7 File Offset: 0x000352A7
	// (set) Token: 0x06000A07 RID: 2567 RVA: 0x000370AF File Offset: 0x000352AF
	public DateTime dateTime
	{
		get
		{
			return this._dateTime;
		}
		set
		{
			this._dateTime = value;
			this._dateTimeString = GTDateTimeSerializable.FormatDateTime(this._dateTime);
		}
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x000370C9 File Offset: 0x000352C9
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		this._dateTimeString = GTDateTimeSerializable.FormatDateTime(this._dateTime);
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x00096B64 File Offset: 0x00094D64
	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		DateTime dateTime;
		if (GTDateTimeSerializable.TryParseDateTime(this._dateTimeString, out dateTime))
		{
			this._dateTime = dateTime;
		}
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x00096B88 File Offset: 0x00094D88
	public GTDateTimeSerializable(int dummyValue)
	{
		DateTime now = DateTime.Now;
		this._dateTime = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0);
		this._dateTimeString = GTDateTimeSerializable.FormatDateTime(this._dateTime);
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x000370DC File Offset: 0x000352DC
	private static string FormatDateTime(DateTime dateTime)
	{
		return dateTime.ToString("yyyy-MM-dd HH:mm");
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x00096BD0 File Offset: 0x00094DD0
	private static bool TryParseDateTime(string value, out DateTime result)
	{
		if (DateTime.TryParseExact(value, new string[]
		{
			"yyyy-MM-dd HH:mm",
			"yyyy-MM-dd",
			"yyyy-MM"
		}, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
		{
			DateTime dateTime = result;
			if (dateTime.Hour == 0 && dateTime.Minute == 0)
			{
				result = result.AddHours(11.0);
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000C16 RID: 3094
	[HideInInspector]
	[SerializeField]
	private string _dateTimeString;

	// Token: 0x04000C17 RID: 3095
	private DateTime _dateTime;
}
