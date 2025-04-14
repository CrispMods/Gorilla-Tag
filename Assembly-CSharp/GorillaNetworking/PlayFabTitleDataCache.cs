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
	// Token: 0x02000AE4 RID: 2788
	public class PlayFabTitleDataCache : MonoBehaviour
	{
		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060045BF RID: 17855 RVA: 0x0014B5A0 File Offset: 0x001497A0
		// (set) Token: 0x060045C0 RID: 17856 RVA: 0x0014B5A7 File Offset: 0x001497A7
		public static PlayFabTitleDataCache Instance { get; private set; }

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060045C1 RID: 17857 RVA: 0x0014B5AF File Offset: 0x001497AF
		private static string FilePath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "TitleDataCache.json");
			}
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x0014B5C0 File Offset: 0x001497C0
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

		// Token: 0x060045C3 RID: 17859 RVA: 0x0014B633 File Offset: 0x00149833
		private void Awake()
		{
			if (PlayFabTitleDataCache.Instance != null)
			{
				Object.Destroy(this);
				return;
			}
			PlayFabTitleDataCache.Instance = this;
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x0014B64F File Offset: 0x0014984F
		private void Start()
		{
			this.UpdateData();
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x0014B658 File Offset: 0x00149858
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

		// Token: 0x060045C6 RID: 17862 RVA: 0x0014B6C8 File Offset: 0x001498C8
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

		// Token: 0x060045C7 RID: 17863 RVA: 0x0014B710 File Offset: 0x00149910
		public void UpdateData()
		{
			this.updateDataCoroutine = base.StartCoroutine(this.UpdateDataCo());
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x0014B724 File Offset: 0x00149924
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

		// Token: 0x060045C9 RID: 17865 RVA: 0x0014B734 File Offset: 0x00149934
		private void LoadKey()
		{
			TextAsset textAsset = Resources.Load<TextAsset>("title_data_key");
			this.titleDataKey = textAsset.text;
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x0014B758 File Offset: 0x00149958
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

		// Token: 0x060045CB RID: 17867 RVA: 0x0014B7B0 File Offset: 0x001499B0
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

		// Token: 0x04004760 RID: 18272
		public PlayFabTitleDataCache.DataUpdate OnTitleDataUpdate;

		// Token: 0x04004761 RID: 18273
		private const string FileName = "TitleDataCache.json";

		// Token: 0x04004762 RID: 18274
		private readonly List<PlayFabTitleDataCache.DataRequest> requests = new List<PlayFabTitleDataCache.DataRequest>();

		// Token: 0x04004763 RID: 18275
		private Dictionary<string, string> titleData = new Dictionary<string, string>();

		// Token: 0x04004764 RID: 18276
		private string titleDataKey;

		// Token: 0x04004765 RID: 18277
		private bool isDataUpToDate;

		// Token: 0x04004766 RID: 18278
		private Coroutine updateDataCoroutine;

		// Token: 0x02000AE5 RID: 2789
		[Serializable]
		public sealed class DataUpdate : UnityEvent<string>
		{
		}

		// Token: 0x02000AE6 RID: 2790
		private class DataRequest
		{
			// Token: 0x1700072C RID: 1836
			// (get) Token: 0x060045CF RID: 17871 RVA: 0x0014B875 File Offset: 0x00149A75
			// (set) Token: 0x060045D0 RID: 17872 RVA: 0x0014B87D File Offset: 0x00149A7D
			public string Name { get; set; }

			// Token: 0x1700072D RID: 1837
			// (get) Token: 0x060045D1 RID: 17873 RVA: 0x0014B886 File Offset: 0x00149A86
			// (set) Token: 0x060045D2 RID: 17874 RVA: 0x0014B88E File Offset: 0x00149A8E
			public Action<string> Callback { get; set; }

			// Token: 0x1700072E RID: 1838
			// (get) Token: 0x060045D3 RID: 17875 RVA: 0x0014B897 File Offset: 0x00149A97
			// (set) Token: 0x060045D4 RID: 17876 RVA: 0x0014B89F File Offset: 0x00149A9F
			public Action<PlayFabError> ErrorCallback { get; set; }
		}
	}
}
