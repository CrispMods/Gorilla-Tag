using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using LitJson;
using PlayFab;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace GorillaNetworking
{
	// Token: 0x02000AE7 RID: 2791
	public class PlayFabTitleDataCache : MonoBehaviour
	{
		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060045CB RID: 17867 RVA: 0x0014BB68 File Offset: 0x00149D68
		// (set) Token: 0x060045CC RID: 17868 RVA: 0x0014BB6F File Offset: 0x00149D6F
		public static PlayFabTitleDataCache Instance { get; private set; }

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x060045CD RID: 17869 RVA: 0x0014BB77 File Offset: 0x00149D77
		private static string FilePath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "TitleDataCache.json");
			}
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x0014BB88 File Offset: 0x00149D88
		public void GetTitleData(string name, Action<string> callback, Action<PlayFabError> errorCallback)
		{
			if (this.isDataUpToDate && this.titleData.ContainsKey(name))
			{
				callback.SafeInvoke(this.titleData[name]);
				return;
			}
			PlayFabTitleDataCache.DataRequest item = new PlayFabTitleDataCache.DataRequest
			{
				Name = name,
				Callback = callback,
				ErrorCallback = errorCallback
			};
			this.requests.Add(item);
			if (this.isDataUpToDate && this.updateDataCoroutine == null)
			{
				this.UpdateData();
			}
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x0014BBFB File Offset: 0x00149DFB
		private void Awake()
		{
			if (PlayFabTitleDataCache.Instance != null)
			{
				Object.Destroy(this);
				return;
			}
			PlayFabTitleDataCache.Instance = this;
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x0014BC17 File Offset: 0x00149E17
		private void Start()
		{
			this.UpdateData();
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x0014BC20 File Offset: 0x00149E20
		public void LoadDataFromFile()
		{
			try
			{
				if (!File.Exists(PlayFabTitleDataCache.FilePath))
				{
					Debug.LogWarning("Title data file " + PlayFabTitleDataCache.FilePath + " does not exist!");
				}
				else
				{
					string json = File.ReadAllText(PlayFabTitleDataCache.FilePath);
					this.titleData = JsonMapper.ToObject<Dictionary<string, string>>(json);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError(string.Format("Error reading PlayFab title data from file: {0}", arg));
			}
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x0014BC90 File Offset: 0x00149E90
		private void SaveDataToFile(string filepath)
		{
			try
			{
				string contents = JsonMapper.ToJson(this.titleData);
				File.WriteAllText(filepath, contents);
			}
			catch (Exception arg)
			{
				Debug.LogError(string.Format("Error writing PlayFab title data to file: {0}", arg));
			}
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x0014BCD8 File Offset: 0x00149ED8
		public void UpdateData()
		{
			this.updateDataCoroutine = base.StartCoroutine(this.UpdateDataCo());
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x0014BCEC File Offset: 0x00149EEC
		private IEnumerator UpdateDataCo()
		{
			this.LoadDataFromFile();
			this.LoadKey();
			Dictionary<string, string> dictionary = this.titleData;
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>((dictionary != null) ? dictionary.Count : 0);
			if (this.titleData != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in this.titleData)
				{
					string text;
					string text2;
					keyValuePair.Deconstruct(out text, out text2);
					string text3 = text;
					string text4 = text2;
					if (text3 != null)
					{
						dictionary2[text3] = ((text4 != null) ? PlayFabTitleDataCache.MD5(text4) : null);
					}
				}
			}
			string s = JsonMapper.ToJson(new Dictionary<string, object>
			{
				{
					"version",
					Application.version
				},
				{
					"key",
					this.titleDataKey
				},
				{
					"data",
					dictionary2
				}
			});
			Stopwatch sw = Stopwatch.StartNew();
			Dictionary<string, JsonData> dictionary3;
			using (UnityWebRequest www = new UnityWebRequest(PlayFabAuthenticatorSettings.TitleDataApiBaseUrl, "POST"))
			{
				byte[] bytes = new UTF8Encoding(true).GetBytes(s);
				www.uploadHandler = new UploadHandlerRaw(bytes);
				www.downloadHandler = new DownloadHandlerBuffer();
				www.SetRequestHeader("Content-Type", "application/json");
				yield return www.SendWebRequest();
				if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
				{
					Debug.LogError("Failed to get TitleData from the server.\n" + www.error);
					this.ClearRequestWithError(null);
					yield break;
				}
				dictionary3 = JsonMapper.ToObject<Dictionary<string, JsonData>>(www.downloadHandler.text);
			}
			UnityWebRequest www = null;
			Debug.Log(string.Format("TitleData fetched: {0:N5}", sw.Elapsed.TotalSeconds));
			foreach (KeyValuePair<string, JsonData> keyValuePair2 in dictionary3)
			{
				PlayFabTitleDataCache.DataUpdate onTitleDataUpdate = this.OnTitleDataUpdate;
				if (onTitleDataUpdate != null)
				{
					onTitleDataUpdate.Invoke(keyValuePair2.Key);
				}
				if (keyValuePair2.Value == null)
				{
					this.titleData.Remove(keyValuePair2.Key);
				}
				else
				{
					this.titleData.AddOrUpdate(keyValuePair2.Key, JsonMapper.ToJson(keyValuePair2.Value));
				}
			}
			if (dictionary3.Keys.Count > 0)
			{
				this.SaveDataToFile(PlayFabTitleDataCache.FilePath);
			}
			this.requests.RemoveAll(delegate(PlayFabTitleDataCache.DataRequest request)
			{
				string data;
				if (this.titleData.TryGetValue(request.Name, out data))
				{
					request.Callback.SafeInvoke(data);
					return true;
				}
				return false;
			});
			this.ClearRequestWithError(null);
			this.isDataUpToDate = true;
			this.updateDataCoroutine = null;
			yield break;
			yield break;
		}

		// Token: 0x060045D5 RID: 17877 RVA: 0x0014BCFC File Offset: 0x00149EFC
		private void LoadKey()
		{
			TextAsset textAsset = Resources.Load<TextAsset>("title_data_key");
			this.titleDataKey = textAsset.text;
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x0014BD20 File Offset: 0x00149F20
		private static string MD5(string value)
		{
			HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.Default.GetBytes(value);
			byte[] array = hashAlgorithm.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x0014BD78 File Offset: 0x00149F78
		private void ClearRequestWithError(PlayFabError e = null)
		{
			if (e == null)
			{
				e = new PlayFabError();
			}
			foreach (PlayFabTitleDataCache.DataRequest dataRequest in this.requests)
			{
				dataRequest.ErrorCallback.SafeInvoke(e);
			}
			this.requests.Clear();
		}

		// Token: 0x04004772 RID: 18290
		public PlayFabTitleDataCache.DataUpdate OnTitleDataUpdate;

		// Token: 0x04004773 RID: 18291
		private const string FileName = "TitleDataCache.json";

		// Token: 0x04004774 RID: 18292
		private readonly List<PlayFabTitleDataCache.DataRequest> requests = new List<PlayFabTitleDataCache.DataRequest>();

		// Token: 0x04004775 RID: 18293
		private Dictionary<string, string> titleData = new Dictionary<string, string>();

		// Token: 0x04004776 RID: 18294
		private string titleDataKey;

		// Token: 0x04004777 RID: 18295
		private bool isDataUpToDate;

		// Token: 0x04004778 RID: 18296
		private Coroutine updateDataCoroutine;

		// Token: 0x02000AE8 RID: 2792
		[Serializable]
		public sealed class DataUpdate : UnityEvent<string>
		{
		}

		// Token: 0x02000AE9 RID: 2793
		private class DataRequest
		{
			// Token: 0x1700072D RID: 1837
			// (get) Token: 0x060045DB RID: 17883 RVA: 0x0014BE3D File Offset: 0x0014A03D
			// (set) Token: 0x060045DC RID: 17884 RVA: 0x0014BE45 File Offset: 0x0014A045
			public string Name { get; set; }

			// Token: 0x1700072E RID: 1838
			// (get) Token: 0x060045DD RID: 17885 RVA: 0x0014BE4E File Offset: 0x0014A04E
			// (set) Token: 0x060045DE RID: 17886 RVA: 0x0014BE56 File Offset: 0x0014A056
			public Action<string> Callback { get; set; }

			// Token: 0x1700072F RID: 1839
			// (get) Token: 0x060045DF RID: 17887 RVA: 0x0014BE5F File Offset: 0x0014A05F
			// (set) Token: 0x060045E0 RID: 17888 RVA: 0x0014BE67 File Offset: 0x0014A067
			public Action<PlayFabError> ErrorCallback { get; set; }
		}
	}
}
