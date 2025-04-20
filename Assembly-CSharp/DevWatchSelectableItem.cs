using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200008F RID: 143
public class DevWatchSelectableItem : MonoBehaviour
{
	// Token: 0x06000395 RID: 917 RVA: 0x00032BFC File Offset: 0x00030DFC
	public void Init(NetworkObject obj)
	{
		this.SelectedObject = obj;
		this.ItemName.text = obj.name;
		this.Button.onClick.AddListener(delegate()
		{
			this.OnSelected(this.ItemName.text, this.SelectedObject);
		});
	}

	// Token: 0x04000424 RID: 1060
	public Button Button;

	// Token: 0x04000425 RID: 1061
	public TextMeshProUGUI ItemName;

	// Token: 0x04000426 RID: 1062
	public NetworkObject SelectedObject;

	// Token: 0x04000427 RID: 1063
	public Action<string, NetworkObject> OnSelected;
}
