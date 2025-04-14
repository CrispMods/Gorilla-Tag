using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006CB RID: 1739
public class ObjectToggle : MonoBehaviour
{
	// Token: 0x06002B0F RID: 11023 RVA: 0x000D58DA File Offset: 0x000D3ADA
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

	// Token: 0x06002B10 RID: 11024 RVA: 0x000D5914 File Offset: 0x000D3B14
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

	// Token: 0x06002B11 RID: 11025 RVA: 0x000D5984 File Offset: 0x000D3B84
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

	// Token: 0x04003096 RID: 12438
	public List<GameObject> objectsToToggle = new List<GameObject>();

	// Token: 0x04003097 RID: 12439
	[SerializeField]
	private bool _ignoreHierarchyState;

	// Token: 0x04003098 RID: 12440
	[NonSerialized]
	private bool? _toggled;
}
