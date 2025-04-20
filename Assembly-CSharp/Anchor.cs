using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02000362 RID: 866
[RequireComponent(typeof(OVRSpatialAnchor))]
public class Anchor : MonoBehaviour
{
	// Token: 0x0600141C RID: 5148 RVA: 0x000BB084 File Offset: 0x000B9284
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

	// Token: 0x0600141D RID: 5149 RVA: 0x000BB100 File Offset: 0x000B9300
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

	// Token: 0x0600141E RID: 5150 RVA: 0x0003D805 File Offset: 0x0003BA05
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

	// Token: 0x0600141F RID: 5151 RVA: 0x0003D814 File Offset: 0x0003BA14
	private void Update()
	{
		this.BillboardPanel(this._canvas.transform);
		this.BillboardPanel(this._pivot);
		this.HandleMenuNavigation();
		this.BillboardPanel(this._icon.transform);
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x0003D84A File Offset: 0x0003BA4A
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

	// Token: 0x06001421 RID: 5153 RVA: 0x000BB164 File Offset: 0x000B9364
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

	// Token: 0x06001422 RID: 5154 RVA: 0x000372C6 File Offset: 0x000354C6
	public void OnHideButtonPressed()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x0003D871 File Offset: 0x0003BA71
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

	// Token: 0x17000243 RID: 579
	// (set) Token: 0x06001424 RID: 5156 RVA: 0x0003D898 File Offset: 0x0003BA98
	public bool ShowSaveIcon
	{
		set
		{
			this._saveIcon.SetActive(value);
		}
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x000BB1C8 File Offset: 0x000B93C8
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

	// Token: 0x06001426 RID: 5158 RVA: 0x000BB224 File Offset: 0x000B9424
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

	// Token: 0x06001427 RID: 5159 RVA: 0x000BB298 File Offset: 0x000B9498
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

	// Token: 0x06001428 RID: 5160 RVA: 0x000BB340 File Offset: 0x000B9540
	private void BillboardPanel(Transform panel)
	{
		panel.LookAt(new Vector3(panel.position.x * 2f - Camera.main.transform.position.x, panel.position.y * 2f - Camera.main.transform.position.y, panel.position.z * 2f - Camera.main.transform.position.z), Vector3.up);
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x000BB3D0 File Offset: 0x000B95D0
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

	// Token: 0x0600142A RID: 5162 RVA: 0x000BB434 File Offset: 0x000B9634
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

	// Token: 0x04001644 RID: 5700
	public const string NumUuidsPlayerPref = "numUuids";

	// Token: 0x04001645 RID: 5701
	[SerializeField]
	[FormerlySerializedAs("canvas_")]
	private Canvas _canvas;

	// Token: 0x04001646 RID: 5702
	[SerializeField]
	[FormerlySerializedAs("pivot_")]
	private Transform _pivot;

	// Token: 0x04001647 RID: 5703
	[SerializeField]
	[FormerlySerializedAs("anchorMenu_")]
	private GameObject _anchorMenu;

	// Token: 0x04001648 RID: 5704
	private bool _isSelected;

	// Token: 0x04001649 RID: 5705
	private bool _isHovered;

	// Token: 0x0400164A RID: 5706
	[SerializeField]
	[FormerlySerializedAs("anchorName_")]
	private TextMeshProUGUI _anchorName;

	// Token: 0x0400164B RID: 5707
	[SerializeField]
	[FormerlySerializedAs("saveIcon_")]
	private GameObject _saveIcon;

	// Token: 0x0400164C RID: 5708
	[SerializeField]
	[FormerlySerializedAs("labelImage_")]
	private Image _labelImage;

	// Token: 0x0400164D RID: 5709
	[SerializeField]
	[FormerlySerializedAs("labelBaseColor_")]
	private Color _labelBaseColor;

	// Token: 0x0400164E RID: 5710
	[SerializeField]
	[FormerlySerializedAs("labelHighlightColor_")]
	private Color _labelHighlightColor;

	// Token: 0x0400164F RID: 5711
	[SerializeField]
	[FormerlySerializedAs("labelSelectedColor_")]
	private Color _labelSelectedColor;

	// Token: 0x04001650 RID: 5712
	[SerializeField]
	[FormerlySerializedAs("uiManager_")]
	private AnchorUIManager _uiManager;

	// Token: 0x04001651 RID: 5713
	[SerializeField]
	[FormerlySerializedAs("renderers_")]
	private MeshRenderer[] _renderers;

	// Token: 0x04001652 RID: 5714
	private int _menuIndex;

	// Token: 0x04001653 RID: 5715
	[SerializeField]
	[FormerlySerializedAs("buttonList_")]
	private List<Button> _buttonList;

	// Token: 0x04001654 RID: 5716
	private Button _selectedButton;

	// Token: 0x04001655 RID: 5717
	private OVRSpatialAnchor _spatialAnchor;

	// Token: 0x04001656 RID: 5718
	private GameObject _icon;
}
