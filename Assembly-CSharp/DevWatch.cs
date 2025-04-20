using System;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200008D RID: 141
public class DevWatch : MonoBehaviour
{
	// Token: 0x0600038C RID: 908 RVA: 0x00079830 File Offset: 0x00077A30
	private void Awake()
	{
		this.SearchButton.SearchEvent.AddListener(new UnityAction(this.SearchItems));
		this.TakeOwnershipButton.onClick.AddListener(new UnityAction(this.TakeOwneshipOfItem));
		this.DestroyObjectButton.onClick.AddListener(new UnityAction(this.TryDestroyItem));
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00079894 File Offset: 0x00077A94
	public void SearchItems()
	{
		this.FoundNetworkObjects.Clear();
		RaycastHit[] array = Physics.SphereCastAll(new Ray(this.RayCastStartPos.position, this.RayCastDirection.position - this.RayCastStartPos.position), 0.3f, 100f);
		if (array.Length != 0)
		{
			foreach (RaycastHit raycastHit in array)
			{
				NetworkObject item;
				if (raycastHit.collider.gameObject.TryGetComponent<NetworkObject>(out item))
				{
					this.FoundNetworkObjects.Add(item);
				}
			}
		}
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00079928 File Offset: 0x00077B28
	public void Cleanup()
	{
		this.FoundNetworkObjects.Clear();
		if (this.Items.Count > 0)
		{
			for (int i = this.Items.Count - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(this.Items[i]);
			}
		}
		this.Items.Clear();
		this.Panel1.SetActive(true);
		this.Panel2.SetActive(false);
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00032B92 File Offset: 0x00030D92
	public void ItemSelected(DevWatchSelectableItem item)
	{
		this.Panel1.SetActive(false);
		this.Panel2.SetActive(true);
		this.SelectedItem = item;
		this.SelectedItemName.text = item.ItemName.text;
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00030607 File Offset: 0x0002E807
	public void TryDestroyItem()
	{
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00030607 File Offset: 0x0002E807
	public void TakeOwneshipOfItem()
	{
	}

	// Token: 0x04000416 RID: 1046
	public DevWatchButton SearchButton;

	// Token: 0x04000417 RID: 1047
	public GameObject Panel1;

	// Token: 0x04000418 RID: 1048
	public GameObject Panel2;

	// Token: 0x04000419 RID: 1049
	public DevWatchSelectableItem SelectableItemPrefab;

	// Token: 0x0400041A RID: 1050
	public List<DevWatchSelectableItem> Items;

	// Token: 0x0400041B RID: 1051
	public Transform RayCastStartPos;

	// Token: 0x0400041C RID: 1052
	public Transform RayCastDirection;

	// Token: 0x0400041D RID: 1053
	public Transform ItemsFoundContainer;

	// Token: 0x0400041E RID: 1054
	public Button TakeOwnershipButton;

	// Token: 0x0400041F RID: 1055
	public Button DestroyObjectButton;

	// Token: 0x04000420 RID: 1056
	public List<NetworkObject> FoundNetworkObjects = new List<NetworkObject>();

	// Token: 0x04000421 RID: 1057
	public TextMeshProUGUI SelectedItemName;

	// Token: 0x04000422 RID: 1058
	public DevWatchSelectableItem SelectedItem;
}
