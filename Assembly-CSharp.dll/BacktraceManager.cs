using System;
using System.Globalization;
using Backtrace.Unity;
using Backtrace.Unity.Model;
using GorillaNetworking;
using PlayFab;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000830 RID: 2096
public class BacktraceManager : MonoBehaviour
{
	// Token: 0x0600332F RID: 13103 RVA: 0x00050D79 File Offset: 0x0004EF79
	public virtual void Awake()
	{
		base.GetComponent<BacktraceClient>().BeforeSend = delegate(BacktraceData data)
		{
			if (new Unity.Mathematics.Random((uint)(Time.realtimeSinceStartupAsDouble * 1000.0)).NextDouble() > this.backtraceSampleRate)
			{
				return null;
			}
			return data;
		};
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x00050D92 File Offset: 0x0004EF92
	private void Start()
	{
		PlayFabTitleDataCache.Instance.GetTitleData("BacktraceSampleRate", delegate(string data)
		{
			if (data != null)
			{
				double.TryParse(data.Trim('"'), NumberStyles.Any, CultureInfo.InvariantCulture, out this.backtraceSampleRate);
				Debug.Log(string.Format("Set backtrace sample rate to: {0}", this.backtraceSampleRate));
			}
		}, delegate(PlayFabError e)
		{
			Debug.LogError(string.Format("Error getting Backtrace sample rate: {0}", e));
		});
	}

	// Token: 0x04003699 RID: 13977
	public double backtraceSampleRate = 0.01;
}
