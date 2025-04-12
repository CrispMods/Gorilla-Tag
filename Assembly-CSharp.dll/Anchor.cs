using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02000357 RID: 855
[RequireComponent(typeof(OVRSpatialAnchor))]
public class Anchor : MonoBehaviour
{
	// Token: 0x060013D3 RID: 5075 RVA: 0x000B87EC File Offset: 0x000B69EC
	private void Awake()
	{
		this._anchorMenu.SetActive(false);
		this._renderers = base.GetComponentsInChildren<MeshRenderer>();
		this._canvas.worldCamera = Camera.main;
		this._selectedButton = this._buttonList[0];
		this._selectedButton.OnSelect(null);
		this._spatialAnchor = base.GetComponent<OVRSpatialAnchor>();
		this._icon = base.GetComponent<Transform>().FindChildRecursive("Sphere").gameObject;
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x000B8868 File Offset: 0x000B6A68
	private static string ConvertUuidToString(Guid guid)
	{
		byte[] array = guid.ToByteArray();
		StringBuilder stringBuilder = new StringBuilder(array.Length * 2 + 4);
		for (int i = 0; i < array.Length; i++)
		{
			if (3 < i && i < 11 && i % 2 == 0)
			{
				stringBuilder.Append("-");
			}
			stringBuilder.AppendFormat("{0:x2}", array[i]);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060013D5 RID: 5077 RVA: 0x0003C545 File Offset: 0x0003A745
	private IEnumerator Start()
	{
		while (this._spatialAnchor && !this._spatialAnchor.Created)
		{
			yield return null;
		}
		if (this._spatialAnchor)
		{
			this._anchorName.text = Anchor.ConvertUuidToString(this._spatialAnchor.Uuid);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		yield break;
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x0003C554 File Offset: 0x0003A754
	private void Update()
	{
		this.BillboardPanel(this._canvas.transform);
		this.BillboardPanel(this._pivot);
		this.HandleMenuNavigation();
		this.BillboardPanel(this._icon.transform);
	}

	// Token: 0x060013D7 RID: 5079 RVA: 0x0003C58A File Offset: 0x0003A78A
	public void OnSaveLocalButtonPressed()
	{
		if (!this._spatialAnchor)
		{
			return;
		}
		this._spatialAnchor.Save(delegate(OVRSpatialAnchor anchor, bool success)
		{
			if (!success)
			{
				return;
			}
			this.ShowSaveIcon = true;
			this.SaveUuidToPlayerPrefs(anchor.Uuid);
		});
	}

	// Token: 0x060013D8 RID: 5080 RVA: 0x000B88CC File Offset: 0x000B6ACC
	private void SaveUuidToPlayerPrefs(Guid uuid)
	{
		if (!PlayerPrefs.HasKey("numUuids"))
		{
			PlayerPrefs.SetInt("numUuids", 0);
		}
		int @int = PlayerPrefs.GetInt("numUuids");
		PlayerPrefs.SetString("uuid" + @int.ToString(), uuid.ToString());
		PlayerPrefs.SetInt("numUuids", @int + 1);
	}

	// Token: 0x060013D9 RID: 5081 RVA: 0x00036006 File Offset: 0x00034206
	public void OnHideButtonPressed()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060013DA RID: 5082 RVA: 0x0003C5B1 File Offset: 0x0003A7B1
	public void OnEraseButtonPressed()
	{
		if (!this._spatialAnchor)
		{
			return;
		}
		this._spatialAnchor.Erase(delegate(OVRSpatialAnchor anchor, bool success)
		{
			if (success)
			{
				this._saveIcon.SetActive(false);
			}
		});
	}

	// Token: 0x1700023C RID: 572
	// (set) Token: 0x060013DB RID: 5083 RVA: 0x0003C5D8 File Offset: 0x0003A7D8
	public bool ShowSaveIcon
	{
		set
		{
			this._saveIcon.SetActive(value);
		}
	}

	// Token: 0x060013DC RID: 5084 RVA: 0x000B8930 File Offset: 0x000B6B30
	public void OnHoverStart()
	{
		if (this._isHovered)
		{
			return;
		}
		this._isHovered = true;
		MeshRenderer[] renderers = this._renderers;
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].material.SetColor("_EmissionColor", Color.yellow);
		}
		this._labelImage.color = this._labelHighlightColor;
	}

	// Token: 0x060013DD RID: 5085 RVA: 0x000B898C File Offset: 0x000B6B8C
	public void OnHoverEnd()
	{
		if (!this._isHovered)
		{
			return;
		}
		this._isHovered = false;
		MeshRenderer[] renderers = this._renderers;
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].material.SetColor("_EmissionColor", Color.clear);
		}
		if (this._isSelected)
		{
			this._labelImage.color = this._labelSelectedColor;
			return;
		}
		this._labelImage.color = this._labelBaseColor;
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x000B8A00 File Offset: 0x000B6C00
	public void OnSelect()
	{
		if (this._isSelected)
		{
			this._anchorMenu.SetActive(false);
			this._isSelected = false;
			this._selectedButton = null;
			if (this._isHovered)
			{
				this._labelImage.color = this._labelHighlightColor;
				return;
			}
			this._labelImage.color = this._labelBaseColor;
			return;
		}
		else
		{
			this._anchorMenu.SetActive(true);
			this._isSelected = true;
			this._menuIndex = -1;
			this.NavigateToIndexInMenu(true);
			if (this._isHovered)
			{
				this._labelImage.color = this._labelHighlightColor;
				return;
			}
			this._labelImage.color = this._labelSelectedColor;
			return;
		}
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x000B8AA8 File Offset: 0x000B6CA8
	private void BillboardPanel(Transform panel)
	{
		panel.LookAt(new Vector3(panel.position.x * 2f - Camera.main.transform.position.x, panel.position.y * 2f - Camera.main.transform.position.y, panel.position.z * 2f - Camera.main.transform.position.z), Vector3.up);
	}

	// Token: 0x060013E0 RID: 5088 RVA: 0x000B8B38 File Offset: 0x000B6D38
	private void HandleMenuNavigation()
	{
		if (!this._isSelected)
		{
			return;
		}
		if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp, OVRInput.Controller.Active))
		{
			this.NavigateToIndexInMenu(false);
		}
		if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown, OVRInput.Controller.Active))
		{
			this.NavigateToIndexInMenu(true);
		}
		if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.Active))
		{
			this._selectedButton.OnSubmit(null);
		}
	}

	// Token: 0x060013E1 RID: 5089 RVA: 0x000B8B9C File Offset: 0x000B6D9C
	private void NavigateToIndexInMenu(bool moveNext)
	{
		if (moveNext)
		{
			this._menuIndex++;
			if (this._menuIndex > this._buttonList.Count - 1)
			{
				this._menuIndex = 0;
			}
		}
		else
		{
			this._menuIndex--;
			if (this._menuIndex < 0)
			{
				this._menuIndex = this._buttonList.Count - 1;
			}
		}
		if (this._selectedButton)
		{
			this._selectedButton.OnDeselect(null);
		}
		this._selectedButton = this._buttonList[this._menuIndex];
		this._selectedButton.OnSelect(null);
	}

	// Token: 0x040015FD RID: 5629
	public const string NumUuidsPlayerPref = "numUuids";

	// Token: 0x040015FE RID: 5630
	[SerializeField]
	[FormerlySerializedAs("canvas_")]
	private Canvas _canvas;

	// Token: 0x040015FF RID: 5631
	[SerializeField]
	[FormerlySerializedAs("pivot_")]
	private Transform _pivot;

	// Token: 0x04001600 RID: 5632
	[SerializeField]
	[FormerlySerializedAs("anchorMenu_")]
	private GameObject _anchorMenu;

	// Token: 0x04001601 RID: 5633
	private bool _isSelected;

	// Token: 0x04001602 RID: 5634
	private bool _isHovered;

	// Token: 0x04001603 RID: 5635
	[SerializeField]
	[FormerlySerializedAs("anchorName_")]
	private TextMeshProUGUI _anchorName;

	// Token: 0x04001604 RID: 5636
	[SerializeField]
	[FormerlySerializedAs("saveIcon_")]
	private GameObject _saveIcon;

	// Token: 0x04001605 RID: 5637
	[SerializeField]
	[FormerlySerializedAs("labelImage_")]
	private Image _labelImage;

	// Token: 0x04001606 RID: 5638
	[SerializeField]
	[FormerlySerializedAs("labelBaseColor_")]
	private Color _labelBaseColor;

	// Token: 0x04001607 RID: 5639
	[SerializeField]
	[FormerlySerializedAs("labelHighlightColor_")]
	private Color _labelHighlightColor;

	// Token: 0x04001608 RID: 5640
	[SerializeField]
	[FormerlySerializedAs("labelSelectedColor_")]
	private Color _labelSelectedColor;

	// Token: 0x04001609 RID: 5641
	[SerializeField]
	[FormerlySerializedAs("uiManager_")]
	private AnchorUIManager _uiManager;

	// Token: 0x0400160A RID: 5642
	[SerializeField]
	[FormerlySerializedAs("renderers_")]
	private MeshRenderer[] _renderers;

	// Token: 0x0400160B RID: 5643
	private int _menuIndex;

	// Token: 0x0400160C RID: 5644
	[SerializeField]
	[FormerlySerializedAs("buttonList_")]
	private List<Button> _buttonList;

	// Token: 0x0400160D RID: 5645
	private Button _selectedButton;

	// Token: 0x0400160E RID: 5646
	private OVRSpatialAnchor _spatialAnchor;

	// Token: 0x0400160F RID: 5647
	private GameObject _icon;
}
