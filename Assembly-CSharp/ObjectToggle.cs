using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006CA RID: 1738
public class ObjectToggle : MonoBehaviour
{
	// Token: 0x06002B07 RID: 11015 RVA: 0x000D545A File Offset: 0x000D365A
	public void Toggle(bool initialState = true)
	{
		if (this._toggled == null)
		{
			if (initialState)
			{
				this.Enable();
				return;
			}
			this.Disable();
			return;
		}
		else
		{
			if (this._toggled.Value)
			{
				this.Disable();
				return;
			}
			this.Enable();
			return;
		}
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x000D5494 File Offset: 0x000D3694
	public void Enable()
	{
		if (this.objectsToToggle == null)
		{
			return;
		}
		for (int i = 0; i < this.objectsToToggle.Count; i++)
		{
			GameObject gameObject = this.objectsToToggle[i];
			if (!(gameObject == null))
			{
				if (this._ignoreHierarchyState)
				{
					gameObject.SetActive(true);
				}
				else if (!gameObject.activeInHierarchy)
				{
					gameObject.SetActive(true);
				}
			}
		}
		this._toggled = new bool?(true);
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x000D5504 File Offset: 0x000D3704
	public void Disable()
	{
		if (this.objectsToToggle == null)
		{
			return;
		}
		for (int i = 0; i < this.objectsToToggle.Count; i++)
		{
			GameObject gameObject = this.objectsToToggle[i];
			if (!(gameObject == null))
			{
				if (this._ignoreHierarchyState)
				{
					gameObject.SetActive(false);
				}
				else if (gameObject.activeInHierarchy)
				{
					gameObject.SetActive(false);
				}
			}
		}
		this._toggled = new bool?(false);
	}

	// Token: 0x04003090 RID: 12432
	public List<GameObject> objectsToToggle = new List<GameObject>();

	// Token: 0x04003091 RID: 12433
	[SerializeField]
	private bool _ignoreHierarchyState;

	// Token: 0x04003092 RID: 12434
	[NonSerialized]
	private bool? _toggled;
}
