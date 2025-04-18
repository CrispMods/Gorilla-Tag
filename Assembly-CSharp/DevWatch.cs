﻿using System;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000086 RID: 134
public class DevWatch : MonoBehaviour
{
	// Token: 0x0600035A RID: 858 RVA: 0x0001550C File Offset: 0x0001370C
	private void Awake()
	{
		this.SearchButton.SearchEvent.AddListener(new UnityAction(this.SearchItems));
		this.TakeOwnershipButton.onClick.AddListener(new UnityAction(this.TakeOwneshipOfItem));
		this.DestroyObjectButton.onClick.AddListener(new UnityAction(this.TryDestroyItem));
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00015570 File Offset: 0x00013770
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

	// Token: 0x0600035C RID: 860 RVA: 0x00015604 File Offset: 0x00013804
	public void Cleanup()
	{
		this.FoundNetworkObjects.Clear();
		if (this.Items.Count > 0)
		{
			for (int i = this.Items.Count - 1; i >= 0; i--)
			{
				Object.Destroy(this.Items[i]);
			}
		}
		this.Items.Clear();
		this.Panel1.SetActive(true);
		this.Panel2.SetActive(false);
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00015676 File Offset: 0x00013876
	public void ItemSelected(DevWatchSelectableItem item)
	{
		this.Panel1.SetActive(false);
		this.Panel2.SetActive(true);
		this.SelectedItem = item;
		this.SelectedItemName.text = item.ItemName.text;
	}

	// Token: 0x0600035E RID: 862 RVA: 0x000023F4 File Offset: 0x000005F4
	public void TryDestroyItem()
	{
	}

	// Token: 0x0600035F RID: 863 RVA: 0x000023F4 File Offset: 0x000005F4
	public void TakeOwneshipOfItem()
	{
	}

	// Token: 0x040003E2 RID: 994
	public DevWatchButton SearchButton;

	// Token: 0x040003E3 RID: 995
	public GameObject Panel1;

	// Token: 0x040003E4 RID: 996
	public GameObject Panel2;

	// Token: 0x040003E5 RID: 997
	public DevWatchSelectableItem SelectableItemPrefab;

	// Token: 0x040003E6 RID: 998
	public List<DevWatchSelectableItem> Items;

	// Token: 0x040003E7 RID: 999
	public Transform RayCastStartPos;

	// Token: 0x040003E8 RID: 1000
	public Transform RayCastDirection;

	// Token: 0x040003E9 RID: 1001
	public Transform ItemsFoundContainer;

	// Token: 0x040003EA RID: 1002
	public Button TakeOwnershipButton;

	// Token: 0x040003EB RID: 1003
	public Button DestroyObjectButton;

	// Token: 0x040003EC RID: 1004
	public List<NetworkObject> FoundNetworkObjects = new List<NetworkObject>();

	// Token: 0x040003ED RID: 1005
	public TextMeshProUGUI SelectedItemName;

	// Token: 0x040003EE RID: 1006
	public DevWatchSelectableItem SelectedItem;
}
