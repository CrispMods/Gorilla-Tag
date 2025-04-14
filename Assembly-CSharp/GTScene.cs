using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020008A0 RID: 2208
[Serializable]
public class GTScene : IEquatable<GTScene>
{
	// Token: 0x1700056B RID: 1387
	// (get) Token: 0x06003563 RID: 13667 RVA: 0x000FE2D0 File Offset: 0x000FC4D0
	public string alias
	{
		get
		{
			return this._alias;
		}
	}

	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x06003564 RID: 13668 RVA: 0x000FE2D8 File Offset: 0x000FC4D8
	public string name
	{
		get
		{
			return this._name;
		}
	}

	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x06003565 RID: 13669 RVA: 0x000FE2E0 File Offset: 0x000FC4E0
	public string path
	{
		get
		{
			return this._path;
		}
	}

	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06003566 RID: 13670 RVA: 0x000FE2E8 File Offset: 0x000FC4E8
	public string guid
	{
		get
		{
			return this._guid;
		}
	}

	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x06003567 RID: 13671 RVA: 0x000FE2F0 File Offset: 0x000FC4F0
	public int buildIndex
	{
		get
		{
			return this._buildIndex;
		}
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06003568 RID: 13672 RVA: 0x000FE2F8 File Offset: 0x000FC4F8
	public bool includeInBuild
	{
		get
		{
			return this._includeInBuild;
		}
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x06003569 RID: 13673 RVA: 0x000FE300 File Offset: 0x000FC500
	public bool isLoaded
	{
		get
		{
			return SceneManager.GetSceneByBuildIndex(this._buildIndex).isLoaded;
		}
	}

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x0600356A RID: 13674 RVA: 0x000FE320 File Offset: 0x000FC520
	public bool hasAlias
	{
		get
		{
			return !string.IsNullOrWhiteSpace(this._alias);
		}
	}

	// Token: 0x0600356B RID: 13675 RVA: 0x000FE330 File Offset: 0x000FC530
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

	// Token: 0x0600356C RID: 13676 RVA: 0x000FE3A1 File Offset: 0x000FC5A1
	public override int GetHashCode()
	{
		return this._guid.GetHashCode();
	}

	// Token: 0x0600356D RID: 13677 RVA: 0x000FE3AE File Offset: 0x000FC5AE
	public override string ToString()
	{
		return this.ToJson(false);
	}

	// Token: 0x0600356E RID: 13678 RVA: 0x000FE3B7 File Offset: 0x000FC5B7
	public bool Equals(GTScene other)
	{
		return this._guid.Equals(other._guid) && this._name == other._name && this._path == other._path;
	}

	// Token: 0x0600356F RID: 13679 RVA: 0x000FE3F4 File Offset: 0x000FC5F4
	public override bool Equals(object obj)
	{
		GTScene gtscene = obj as GTScene;
		return gtscene != null && this.Equals(gtscene);
	}

	// Token: 0x06003570 RID: 13680 RVA: 0x000FE414 File Offset: 0x000FC614
	public static bool operator ==(GTScene x, GTScene y)
	{
		return x.Equals(y);
	}

	// Token: 0x06003571 RID: 13681 RVA: 0x000FE41D File Offset: 0x000FC61D
	public static bool operator !=(GTScene x, GTScene y)
	{
		return !x.Equals(y);
	}

	// Token: 0x06003572 RID: 13682 RVA: 0x000FE429 File Offset: 0x000FC629
	public void LoadAsync()
	{
		if (this.isLoaded)
		{
			return;
		}
		SceneManager.LoadSceneAsync(this._buildIndex, LoadSceneMode.Additive);
	}

	// Token: 0x06003573 RID: 13683 RVA: 0x000FE441 File Offset: 0x000FC641
	public void UnloadAsync()
	{
		if (!this.isLoaded)
		{
			return;
		}
		SceneManager.UnloadSceneAsync(this._buildIndex, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
	}

	// Token: 0x06003574 RID: 13684 RVA: 0x00042E31 File Offset: 0x00041031
	public static GTScene FromAsset(object sceneAsset)
	{
		return null;
	}

	// Token: 0x06003575 RID: 13685 RVA: 0x00042E31 File Offset: 0x00041031
	public static GTScene From(object editorBuildSettingsScene)
	{
		return null;
	}

	// Token: 0x040037C2 RID: 14274
	[SerializeField]
	private string _alias;

	// Token: 0x040037C3 RID: 14275
	[SerializeField]
	private string _name;

	// Token: 0x040037C4 RID: 14276
	[SerializeField]
	private string _path;

	// Token: 0x040037C5 RID: 14277
	[SerializeField]
	private string _guid;

	// Token: 0x040037C6 RID: 14278
	[SerializeField]
	private int _buildIndex;

	// Token: 0x040037C7 RID: 14279
	[SerializeField]
	private bool _includeInBuild;
}
