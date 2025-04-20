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
	// Token: 0x02000B11 RID: 2833
	public class PlayFabTitleDataCache : MonoBehaviour
	{
		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06004707 RID: 18183 RVA: 0x0005E46B File Offset: 0x0005C66B
		// (set) Token: 0x06004708 RID: 18184 RVA: 0x0005E472 File Offset: 0x0005C672
		public static PlayFabTitleDataCache Instance { get; private set; }

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06004709 RID: 18185 RVA: 0x0005E47A File Offset: 0x0005C67A
		private static string FilePath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "TitleDataCache.json");
			}
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x001888B8 File Offset: 0x00186AB8
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

		// Token: 0x0600470B RID: 18187 RVA: 0x0005E48B File Offset: 0x0005C68B
		private void Awake()
		{
			if (PlayFabTitleDataCache.Instance != null)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			PlayFabTitleDataCache.Instance = this;
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x0005E4A7 File Offset: 0x0005C6A7
		private void Start()
		{
			this.UpdateData();
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x0018892C File Offset: 0x00186B2C
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

		// Token: 0x0600470E RID: 18190 RVA: 0x0018899C File Offset: 0x00186B9C
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

		// Token: 0x0600470F RID: 18191 RVA: 0x0005E4AF File Offset: 0x0005C6AF
		public void UpdateData()
		{
			this.updateDataCoroutine = base.StartCoroutine(this.UpdateDataCo());
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x0005E4C3 File Offset: 0x0005C6C3
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

		// Token: 0x06004711 RID: 18193 RVA: 0x001889E4 File Offset: 0x00186BE4
		private void LoadKey()
		{
			TextAsset textAsset = Resources.Load<TextAsset>("title_data_key");
			this.titleDataKey = textAsset.text;
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x00188A08 File Offset: 0x00186C08
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

		// Token: 0x06004713 RID: 18195 RVA: 0x00188A60 File Offset: 0x00186C60
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

		// Token: 0x04004855 RID: 18517
		public PlayFabTitleDataCache.DataUpdate OnTitleDataUpdate;

		// Token: 0x04004856 RID: 18518
		private const string FileName = "TitleDataCache.json";

		// Token: 0x04004857 RID: 18519
		private readonly List<PlayFabTitleDataCache.DataRequest> requests = new List<PlayFabTitleDataCache.DataRequest>();

		// Token: 0x04004858 RID: 18520
		private Dictionary<string, string> titleData = new Dictionary<string, string>();

		// Token: 0x04004859 RID: 18521
		private string titleDataKey;

		// Token: 0x0400485A RID: 18522
		private bool isDataUpToDate;

		// Token: 0x0400485B RID: 18523
		private Coroutine updateDataCoroutine;

		// Token: 0x02000B12 RID: 2834
		[Serializable]
		public sealed class DataUpdate : UnityEvent<string>
		{
		}

		// Token: 0x02000B13 RID: 2835
		private class DataRequest
		{
			// Token: 0x17000748 RID: 1864
			// (get) Token: 0x06004717 RID: 18199 RVA: 0x0005E4F8 File Offset: 0x0005C6F8
			// (set) Token: 0x06004718 RID: 18200 RVA: 0x0005E500 File Offset: 0x0005C700
			public string Name { get; set; }

			// Token: 0x17000749 RID: 1865
			// (get) Token: 0x06004719 RID: 18201 RVA: 0x0005E509 File Offset: 0x0005C709
			// (set) Token: 0x0600471A RID: 18202 RVA: 0x0005E511 File Offset: 0x0005C711
			public Action<string> Callback { get; set; }

			// Token: 0x1700074A RID: 1866
			// (get) Token: 0x0600471B RID: 18203 RVA: 0x0005E51A File Offset: 0x0005C71A
			// (set) Token: 0x0600471C RID: 18204 RVA: 0x0005E522 File Offset: 0x0005C722
			public Action<PlayFabError> ErrorCallback { get; set; }
		}
	}
}
