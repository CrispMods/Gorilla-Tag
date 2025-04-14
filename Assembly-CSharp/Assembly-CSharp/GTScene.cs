using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020008A3 RID: 2211
[Serializable]
public class GTScene : IEquatable<GTScene>
{
	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x0600356F RID: 13679 RVA: 0x000FE898 File Offset: 0x000FCA98
	public string alias
	{
		get
		{
			return this._alias;
		}
	}

	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x06003570 RID: 13680 RVA: 0x000FE8A0 File Offset: 0x000FCAA0
	public string name
	{
		get
		{
			return this._name;
		}
	}

	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06003571 RID: 13681 RVA: 0x000FE8A8 File Offset: 0x000FCAA8
	public string path
	{
		get
		{
			return this._path;
		}
	}

	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x06003572 RID: 13682 RVA: 0x000FE8B0 File Offset: 0x000FCAB0
	public string guid
	{
		get
		{
			return this._guid;
		}
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06003573 RID: 13683 RVA: 0x000FE8B8 File Offset: 0x000FCAB8
	public int buildIndex
	{
		get
		{
			return this._buildIndex;
		}
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x06003574 RID: 13684 RVA: 0x000FE8C0 File Offset: 0x000FCAC0
	public bool includeInBuild
	{
		get
		{
			return this._includeInBuild;
		}
	}

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06003575 RID: 13685 RVA: 0x000FE8C8 File Offset: 0x000FCAC8
	public bool isLoaded
	{
		get
		{
			return SceneManager.GetSceneByBuildIndex(this._buildIndex).isLoaded;
		}
	}

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06003576 RID: 13686 RVA: 0x000FE8E8 File Offset: 0x000FCAE8
	public bool hasAlias
	{
		get
		{
			return !string.IsNullOrWhiteSpace(this._alias);
		}
	}

	// Token: 0x06003577 RID: 13687 RVA: 0x000FE8F8 File Offset: 0x000FCAF8
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

	// Token: 0x06003578 RID: 13688 RVA: 0x000FE969 File Offset: 0x000FCB69
	public override int GetHashCode()
	{
		return this._guid.GetHashCode();
	}

	// Token: 0x06003579 RID: 13689 RVA: 0x000FE976 File Offset: 0x000FCB76
	public override string ToString()
	{
		return this.ToJson(false);
	}

	// Token: 0x0600357A RID: 13690 RVA: 0x000FE97F File Offset: 0x000FCB7F
	public bool Equals(GTScene other)
	{
		return this._guid.Equals(other._guid) && this._name == other._name && this._path == other._path;
	}

	// Token: 0x0600357B RID: 13691 RVA: 0x000FE9BC File Offset: 0x000FCBBC
	public override bool Equals(object obj)
	{
		GTScene gtscene = obj as GTScene;
		return gtscene != null && this.Equals(gtscene);
	}

	// Token: 0x0600357C RID: 13692 RVA: 0x000FE9DC File Offset: 0x000FCBDC
	public static bool operator ==(GTScene x, GTScene y)
	{
		return x.Equals(y);
	}

	// Token: 0x0600357D RID: 13693 RVA: 0x000FE9E5 File Offset: 0x000FCBE5
	public static bool operator !=(GTScene x, GTScene y)
	{
		return !x.Equals(y);
	}

	// Token: 0x0600357E RID: 13694 RVA: 0x000FE9F1 File Offset: 0x000FCBF1
	public void LoadAsync()
	{
		if (this.isLoaded)
		{
			return;
		}
		SceneManager.LoadSceneAsync(this._buildIndex, LoadSceneMode.Additive);
	}

	// Token: 0x0600357F RID: 13695 RVA: 0x000FEA09 File Offset: 0x000FCC09
	public void UnloadAsync()
	{
		if (!this.isLoaded)
		{
			return;
		}
		SceneManager.UnloadSceneAsync(this._buildIndex, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
	}

	// Token: 0x06003580 RID: 13696 RVA: 0x00043175 File Offset: 0x00041375
	public static GTScene FromAsset(object sceneAsset)
	{
		return null;
	}

	// Token: 0x06003581 RID: 13697 RVA: 0x00043175 File Offset: 0x00041375
	public static GTScene From(object editorBuildSettingsScene)
	{
		return null;
	}

	// Token: 0x040037D4 RID: 14292
	[SerializeField]
	private string _alias;

	// Token: 0x040037D5 RID: 14293
	[SerializeField]
	private string _name;

	// Token: 0x040037D6 RID: 14294
	[SerializeField]
	private string _path;

	// Token: 0x040037D7 RID: 14295
	[SerializeField]
	private string _guid;

	// Token: 0x040037D8 RID: 14296
	[SerializeField]
	private int _buildIndex;

	// Token: 0x040037D9 RID: 14297
	[SerializeField]
	private bool _includeInBuild;
}
