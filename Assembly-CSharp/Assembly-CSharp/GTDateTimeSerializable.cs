using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000185 RID: 389
[Serializable]
public struct GTDateTimeSerializable : ISerializationCallbackReceiver
{
	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x060009BC RID: 2492 RVA: 0x00036D62 File Offset: 0x00034F62
	// (set) Token: 0x060009BD RID: 2493 RVA: 0x00036D6A File Offset: 0x00034F6A
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

	// Token: 0x060009BE RID: 2494 RVA: 0x00036D84 File Offset: 0x00034F84
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		this._dateTimeString = GTDateTimeSerializable.FormatDateTime(this._dateTime);
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x00036D98 File Offset: 0x00034F98
	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		DateTime dateTime;
		if (GTDateTimeSerializable.TryParseDateTime(this._dateTimeString, out dateTime))
		{
			this._dateTime = dateTime;
		}
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00036DBC File Offset: 0x00034FBC
	public GTDateTimeSerializable(int dummyValue)
	{
		DateTime now = DateTime.Now;
		this._dateTime = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0);
		this._dateTimeString = GTDateTimeSerializable.FormatDateTime(this._dateTime);
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x00036E04 File Offset: 0x00035004
	private static string FormatDateTime(DateTime dateTime)
	{
		return dateTime.ToString("yyyy-MM-dd HH:mm");
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x00036E14 File Offset: 0x00035014
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

	// Token: 0x04000BD1 RID: 3025
	[HideInInspector]
	[SerializeField]
	private string _dateTimeString;

	// Token: 0x04000BD2 RID: 3026
	private DateTime _dateTime;
}
