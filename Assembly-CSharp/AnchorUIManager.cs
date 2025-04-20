using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02000364 RID: 868
[RequireComponent(typeof(SpatialAnchorLoader))]
public class AnchorUIManager : MonoBehaviour
{
	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06001434 RID: 5172 RVA: 0x0003D8E7 File Offset: 0x0003BAE7
	public Anchor AnchorPrefab
	{
		get
		{
			return this._anchorPrefab;
		}
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x0003D8EF File Offset: 0x0003BAEF
	private void Awake()
	{
		if (AnchorUIManager.Instance == null)
		{
			AnchorUIManager.Instance = this;
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x000BB56C File Offset: 0x000B976C
	private void Start()
	{
		this._raycastOrigin = this._trackedDevice;
		this._mode = AnchorUIManager.AnchorMode.Select;
		this.StartSelectMode();
		this._menuIndex = 0;
		this._selectedButton = this._buttonList[0];
		this._selectedButton.OnSelect(null);
		this._lineRenderer.startWidth = 0.005f;
		this._lineRenderer.endWidth = 0.005f;
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x000BB5D8 File Offset: 0x000B97D8
	private void Update()
	{
		if (this._drawRaycast)
		{
			this.ControllerRaycast();
		}
		if (this._selectedAnchor == null)
		{
			this._selectedButton.OnSelect(null);
			this._isFocused = true;
		}
		this.HandleMenuNavigation();
		if (OVRInput.GetDown(OVRInput.RawButton.A, OVRInput.Controller.Active))
		{
			AnchorUIManager.PrimaryPressDelegate primaryPressDelegate = this._primaryPressDelegate;
			if (primaryPressDelegate == null)
			{
				return;
			}
			primaryPressDelegate();
		}
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x0003D90B File Offset: 0x0003BB0B
	public void OnCreateModeButtonPressed()
	{
		this.ToggleCreateMode();
		this._createModeButton.SetActive(!this._createModeButton.activeSelf);
		this._selectModeButton.SetActive(!this._selectModeButton.activeSelf);
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x0003D945 File Offset: 0x0003BB45
	public void OnLoadAnchorsButtonPressed()
	{
		base.GetComponent<SpatialAnchorLoader>().LoadAnchorsByUuid();
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x0003D952 File Offset: 0x0003BB52
	private void ToggleCreateMode()
	{
		if (this._mode == AnchorUIManager.AnchorMode.Select)
		{
			this._mode = AnchorUIManager.AnchorMode.Create;
			this.EndSelectMode();
			this.StartPlacementMode();
			return;
		}
		this._mode = AnchorUIManager.AnchorMode.Select;
		this.EndPlacementMode();
		this.StartSelectMode();
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x0003D984 File Offset: 0x0003BB84
	private void StartPlacementMode()
	{
		this.ShowAnchorPreview();
		this._primaryPressDelegate = new AnchorUIManager.PrimaryPressDelegate(this.PlaceAnchor);
	}

	// Token: 0x0600143C RID: 5180 RVA: 0x0003D99E File Offset: 0x0003BB9E
	private void EndPlacementMode()
	{
		this.HideAnchorPreview();
		this._primaryPressDelegate = null;
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x0003D9AD File Offset: 0x0003BBAD
	private void StartSelectMode()
	{
		this.ShowRaycastLine();
		this._primaryPressDelegate = new AnchorUIManager.PrimaryPressDelegate(this.SelectAnchor);
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x0003D9C7 File Offset: 0x0003BBC7
	private void EndSelectMode()
	{
		this.HideRaycastLine();
		this._primaryPressDelegate = null;
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x000BB638 File Offset: 0x000B9838
	private void HandleMenuNavigation()
	{
		if (!this._isFocused)
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

	// Token: 0x06001440 RID: 5184 RVA: 0x000BB69C File Offset: 0x000B989C
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
		this._selectedButton.OnDeselect(null);
		this._selectedButton = this._buttonList[this._menuIndex];
		this._selectedButton.OnSelect(null);
	}

	// Token: 0x06001441 RID: 5185 RVA: 0x0003D9D6 File Offset: 0x0003BBD6
	private void ShowAnchorPreview()
	{
		this._placementPreview.SetActive(true);
	}

	// Token: 0x06001442 RID: 5186 RVA: 0x0003D9E4 File Offset: 0x0003BBE4
	private void HideAnchorPreview()
	{
		this._placementPreview.SetActive(false);
	}

	// Token: 0x06001443 RID: 5187 RVA: 0x0003D9F2 File Offset: 0x0003BBF2
	private void PlaceAnchor()
	{
		UnityEngine.Object.Instantiate<Anchor>(this._anchorPrefab, this._anchorPlacementTransform.position, this._anchorPlacementTransform.rotation);
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x0003DA16 File Offset: 0x0003BC16
	private void ShowRaycastLine()
	{
		this._drawRaycast = true;
		this._lineRenderer.gameObject.SetActive(true);
	}

	// Token: 0x06001445 RID: 5189 RVA: 0x0003DA30 File Offset: 0x0003BC30
	private void HideRaycastLine()
	{
		this._drawRaycast = false;
		this._lineRenderer.gameObject.SetActive(false);
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x000BB734 File Offset: 0x000B9934
	private void ControllerRaycast()
	{
		Ray ray = new Ray(this._raycastOrigin.position, this._raycastOrigin.TransformDirection(Vector3.forward));
		this._lineRenderer.SetPosition(0, this._raycastOrigin.position);
		this._lineRenderer.SetPosition(1, this._raycastOrigin.position + this._raycastOrigin.TransformDirection(Vector3.forward) * 10f);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity))
		{
			Anchor component = raycastHit.collider.GetComponent<Anchor>();
			if (component != null)
			{
				this._lineRenderer.SetPosition(1, raycastHit.point);
				this.HoverAnchor(component);
				return;
			}
		}
		this.UnhoverAnchor();
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x0003DA4A File Offset: 0x0003BC4A
	private void HoverAnchor(Anchor anchor)
	{
		this._hoveredAnchor = anchor;
		this._hoveredAnchor.OnHoverStart();
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x0003DA5E File Offset: 0x0003BC5E
	private void UnhoverAnchor()
	{
		if (this._hoveredAnchor == null)
		{
			return;
		}
		this._hoveredAnchor.OnHoverEnd();
		this._hoveredAnchor = null;
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x000BB7F4 File Offset: 0x000B99F4
	private void SelectAnchor()
	{
		if (this._hoveredAnchor != null)
		{
			if (this._selectedAnchor != null)
			{
				this._selectedAnchor.OnSelect();
				this._selectedAnchor = null;
			}
			this._selectedAnchor = this._hoveredAnchor;
			this._selectedAnchor.OnSelect();
			this._selectedButton.OnDeselect(null);
			this._isFocused = false;
			return;
		}
		if (this._selectedAnchor != null)
		{
			this._selectedAnchor.OnSelect();
			this._selectedAnchor = null;
			this._selectedButton.OnSelect(null);
			this._isFocused = true;
		}
	}

	// Token: 0x0400165A RID: 5722
	public static AnchorUIManager Instance;

	// Token: 0x0400165B RID: 5723
	[SerializeField]
	[FormerlySerializedAs("createModeButton_")]
	private GameObject _createModeButton;

	// Token: 0x0400165C RID: 5724
	[SerializeField]
	[FormerlySerializedAs("selectModeButton_")]
	private GameObject _selectModeButton;

	// Token: 0x0400165D RID: 5725
	[SerializeField]
	[FormerlySerializedAs("trackedDevice_")]
	private Transform _trackedDevice;

	// Token: 0x0400165E RID: 5726
	private Transform _raycastOrigin;

	// Token: 0x0400165F RID: 5727
	private bool _drawRaycast;

	// Token: 0x04001660 RID: 5728
	[SerializeField]
	[FormerlySerializedAs("lineRenderer_")]
	private LineRenderer _lineRenderer;

	// Token: 0x04001661 RID: 5729
	private Anchor _hoveredAnchor;

	// Token: 0x04001662 RID: 5730
	private Anchor _selectedAnchor;

	// Token: 0x04001663 RID: 5731
	private AnchorUIManager.AnchorMode _mode = AnchorUIManager.AnchorMode.Select;

	// Token: 0x04001664 RID: 5732
	[SerializeField]
	[FormerlySerializedAs("buttonList_")]
	private List<Button> _buttonList;

	// Token: 0x04001665 RID: 5733
	private int _menuIndex;

	// Token: 0x04001666 RID: 5734
	private Button _selectedButton;

	// Token: 0x04001667 RID: 5735
	[SerializeField]
	private Anchor _anchorPrefab;

	// Token: 0x04001668 RID: 5736
	[SerializeField]
	[FormerlySerializedAs("placementPreview_")]
	private GameObject _placementPreview;

	// Token: 0x04001669 RID: 5737
	[SerializeField]
	[FormerlySerializedAs("anchorPlacementTransform_")]
	private Transform _anchorPlacementTransform;

	// Token: 0x0400166A RID: 5738
	private AnchorUIManager.PrimaryPressDelegate _primaryPressDelegate;

	// Token: 0x0400166B RID: 5739
	private bool _isFocused = true;

	// Token: 0x02000365 RID: 869
	public enum AnchorMode
	{
		// Token: 0x0400166D RID: 5741
		Create,
		// Token: 0x0400166E RID: 5742
		Select
	}

	// Token: 0x02000366 RID: 870
	// (Invoke) Token: 0x0600144C RID: 5196
	private delegate void PrimaryPressDelegate();
}
