using System;
using System.Globalization;
using Backtrace.Unity;
using Backtrace.Unity.Model;
using GorillaNetworking;
using PlayFab;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x0200082D RID: 2093
public class BacktraceManager : MonoBehaviour
{
	// Token: 0x06003323 RID: 13091 RVA: 0x000F459E File Offset: 0x000F279E
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

	// Token: 0x06003324 RID: 13092 RVA: 0x000F45B7 File Offset: 0x000F27B7
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

	// Token: 0x04003687 RID: 13959
	public double backtraceSampleRate = 0.01;
}
