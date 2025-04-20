using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006DF RID: 1759
public class ObjectToggle : MonoBehaviour
{
	// Token: 0x06002B9D RID: 11165 RVA: 0x0004D86A File Offset: 0x0004BA6A
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

	// Token: 0x06002B9E RID: 11166 RVA: 0x00121018 File Offset: 0x0011F218
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

	// Token: 0x06002B9F RID: 11167 RVA: 0x00121088 File Offset: 0x0011F288
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

	// Token: 0x0400312D RID: 12589
	public List<GameObject> objectsToToggle = new List<GameObject>();

	// Token: 0x0400312E RID: 12590
	[SerializeField]
	private bool _ignoreHierarchyState;

	// Token: 0x0400312F RID: 12591
	[NonSerialized]
	private bool? _toggled;
}
