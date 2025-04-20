using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020008BC RID: 2236
[Serializable]
public class GTScene : IEquatable<GTScene>
{
	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x0600362B RID: 13867 RVA: 0x00053ABD File Offset: 0x00051CBD
	public string alias
	{
		get
		{
			return this._alias;
		}
	}

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x0600362C RID: 13868 RVA: 0x00053AC5 File Offset: 0x00051CC5
	public string name
	{
		get
		{
			return this._name;
		}
	}

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x0600362D RID: 13869 RVA: 0x00053ACD File Offset: 0x00051CCD
	public string path
	{
		get
		{
			return this._path;
		}
	}

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x0600362E RID: 13870 RVA: 0x00053AD5 File Offset: 0x00051CD5
	public string guid
	{
		get
		{
			return this._guid;
		}
	}

	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x0600362F RID: 13871 RVA: 0x00053ADD File Offset: 0x00051CDD
	public int buildIndex
	{
		get
		{
			return this._buildIndex;
		}
	}

	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06003630 RID: 13872 RVA: 0x00053AE5 File Offset: 0x00051CE5
	public bool includeInBuild
	{
		get
		{
			return this._includeInBuild;
		}
	}

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06003631 RID: 13873 RVA: 0x00144560 File Offset: 0x00142760
	public bool isLoaded
	{
		get
		{
			return SceneManager.GetSceneByBuildIndex(this._buildIndex).isLoaded;
		}
	}

	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x06003632 RID: 13874 RVA: 0x00053AED File Offset: 0x00051CED
	public bool hasAlias
	{
		get
		{
			return !string.IsNullOrWhiteSpace(this._alias);
		}
	}

	// Token: 0x06003633 RID: 13875 RVA: 0x00144580 File Offset: 0x00142780
	public GTScene(string name, string path, string guid, int buildIndex, bool includeInBuild)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentNullException("name");
		}
		if (string.IsNullOrWhiteSpace(path))
		{
			throw new ArgumentNullException("path");
		}
		if (string.IsNullOrWhiteSpace(guid))
		{
			throw new ArgumentNullException("guid");
		}
		this._name = name;
		this._path = path;
		this._guid = guid;
		this._buildIndex = buildIndex;
		this._includeInBuild = includeInBuild;
	}

	// Token: 0x06003634 RID: 13876 RVA: 0x00053AFD File Offset: 0x00051CFD
	public override int GetHashCode()
	{
		return this._guid.GetHashCode();
	}

	// Token: 0x06003635 RID: 13877 RVA: 0x00053B0A File Offset: 0x00051D0A
	public override string ToString()
	{
		return this.ToJson(false);
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x00053B13 File Offset: 0x00051D13
	public bool Equals(GTScene other)
	{
		return this._guid.Equals(other._guid) && this._name == other._name && this._path == other._path;
	}

	// Token: 0x06003637 RID: 13879 RVA: 0x001445F4 File Offset: 0x001427F4
	public override bool Equals(object obj)
	{
		GTScene gtscene = obj as GTScene;
		return gtscene != null && this.Equals(gtscene);
	}

	// Token: 0x06003638 RID: 13880 RVA: 0x00053B4E File Offset: 0x00051D4E
	public static bool operator ==(GTScene x, GTScene y)
	{
		return x.Equals(y);
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x00053B57 File Offset: 0x00051D57
	public static bool operator !=(GTScene x, GTScene y)
	{
		return !x.Equals(y);
	}

	// Token: 0x0600363A RID: 13882 RVA: 0x00053B63 File Offset: 0x00051D63
	public void LoadAsync()
	{
		if (this.isLoaded)
		{
			return;
		}
		SceneManager.LoadSceneAsync(this._buildIndex, LoadSceneMode.Additive);
	}

	// Token: 0x0600363B RID: 13883 RVA: 0x00053B7B File Offset: 0x00051D7B
	public void UnloadAsync()
	{
		if (!this.isLoaded)
		{
			return;
		}
		SceneManager.UnloadSceneAsync(this._buildIndex, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
	}

	// Token: 0x0600363C RID: 13884 RVA: 0x0003924B File Offset: 0x0003744B
	public static GTScene FromAsset(object sceneAsset)
	{
		return null;
	}

	// Token: 0x0600363D RID: 13885 RVA: 0x0003924B File Offset: 0x0003744B
	public static GTScene From(object editorBuildSettingsScene)
	{
		return null;
	}

	// Token: 0x04003883 RID: 14467
	[SerializeField]
	private string _alias;

	// Token: 0x04003884 RID: 14468
	[SerializeField]
	private string _name;

	// Token: 0x04003885 RID: 14469
	[SerializeField]
	private string _path;

	// Token: 0x04003886 RID: 14470
	[SerializeField]
	private string _guid;

	// Token: 0x04003887 RID: 14471
	[SerializeField]
	private int _buildIndex;

	// Token: 0x04003888 RID: 14472
	[SerializeField]
	private bool _includeInBuild;
}
