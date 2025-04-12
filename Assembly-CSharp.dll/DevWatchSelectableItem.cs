using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000088 RID: 136
public class DevWatchSelectableItem : MonoBehaviour
{
	// Token: 0x06000365 RID: 869 RVA: 0x00031A99 File Offset: 0x0002FC99
	public void Init(NetworkObject obj)
	{
		this.SelectedObject = obj;
		this.ItemName.text = obj.name;
		this.Button.onClick.AddListener(delegate()
		{
			this.OnSelected(this.ItemName.text, this.SelectedObject);
		});
	}

	// Token: 0x040003F1 RID: 1009
	public Button Button;

	// Token: 0x040003F2 RID: 1010
	public TextMeshProUGUI ItemName;

	// Token: 0x040003F3 RID: 1011
	public NetworkObject SelectedObject;

	// Token: 0x040003F4 RID: 1012
	public Action<string, NetworkObject> OnSelected;
}
