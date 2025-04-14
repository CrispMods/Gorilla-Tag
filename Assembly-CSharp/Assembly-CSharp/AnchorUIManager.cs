using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02000359 RID: 857
[RequireComponent(typeof(SpatialAnchorLoader))]
public class AnchorUIManager : MonoBehaviour
{
	// Token: 0x1700023F RID: 575
	// (get) Token: 0x060013EB RID: 5099 RVA: 0x000620DC File Offset: 0x000602DC
	public Anchor AnchorPrefab
	{
		get
		{
			return this._anchorPrefab;
		}
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x000620E4 File Offset: 0x000602E4
	private void Awake()
	{
		if (AnchorUIManager.Instance == null)
		{
			AnchorUIManager.Instance = this;
			return;
		}
		Object.Destroy(this);
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x00062100 File Offset: 0x00060300
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

	// Token: 0x060013EE RID: 5102 RVA: 0x0006216C File Offset: 0x0006036C
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

	// Token: 0x060013EF RID: 5103 RVA: 0x000621CB File Offset: 0x000603CB
	public void OnCreateModeButtonPressed()
	{
		this.ToggleCreateMode();
		this._createModeButton.SetActive(!this._createModeButton.activeSelf);
		this._selectModeButton.SetActive(!this._selectModeButton.activeSelf);
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x00062205 File Offset: 0x00060405
	public void OnLoadAnchorsButtonPressed()
	{
		base.GetComponent<SpatialAnchorLoader>().LoadAnchorsByUuid();
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x00062212 File Offset: 0x00060412
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

	// Token: 0x060013F2 RID: 5106 RVA: 0x00062244 File Offset: 0x00060444
	private void StartPlacementMode()
	{
		this.ShowAnchorPreview();
		this._primaryPressDelegate = new AnchorUIManager.PrimaryPressDelegate(this.PlaceAnchor);
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x0006225E File Offset: 0x0006045E
	private void EndPlacementMode()
	{
		this.HideAnchorPreview();
		this._primaryPressDelegate = null;
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x0006226D File Offset: 0x0006046D
	private void StartSelectMode()
	{
		this.ShowRaycastLine();
		this._primaryPressDelegate = new AnchorUIManager.PrimaryPressDelegate(this.SelectAnchor);
	}

	// Token: 0x060013F5 RID: 5109 RVA: 0x00062287 File Offset: 0x00060487
	private void EndSelectMode()
	{
		this.HideRaycastLine();
		this._primaryPressDelegate = null;
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x00062298 File Offset: 0x00060498
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

	// Token: 0x060013F7 RID: 5111 RVA: 0x000622FC File Offset: 0x000604FC
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

	// Token: 0x060013F8 RID: 5112 RVA: 0x00062391 File Offset: 0x00060591
	private void ShowAnchorPreview()
	{
		this._placementPreview.SetActive(true);
	}

	// Token: 0x060013F9 RID: 5113 RVA: 0x0006239F File Offset: 0x0006059F
	private void HideAnchorPreview()
	{
		this._placementPreview.SetActive(false);
	}

	// Token: 0x060013FA RID: 5114 RVA: 0x000623AD File Offset: 0x000605AD
	private void PlaceAnchor()
	{
		Object.Instantiate<Anchor>(this._anchorPrefab, this._anchorPlacementTransform.position, this._anchorPlacementTransform.rotation);
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x000623D1 File Offset: 0x000605D1
	private void ShowRaycastLine()
	{
		this._drawRaycast = true;
		this._lineRenderer.gameObject.SetActive(true);
	}

	// Token: 0x060013FC RID: 5116 RVA: 0x000623EB File Offset: 0x000605EB
	private void HideRaycastLine()
	{
		this._drawRaycast = false;
		this._lineRenderer.gameObject.SetActive(false);
	}

	// Token: 0x060013FD RID: 5117 RVA: 0x00062408 File Offset: 0x00060608
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

	// Token: 0x060013FE RID: 5118 RVA: 0x000624C7 File Offset: 0x000606C7
	private void HoverAnchor(Anchor anchor)
	{
		this._hoveredAnchor = anchor;
		this._hoveredAnchor.OnHoverStart();
	}

	// Token: 0x060013FF RID: 5119 RVA: 0x000624DB File Offset: 0x000606DB
	private void UnhoverAnchor()
	{
		if (this._hoveredAnchor == null)
		{
			return;
		}
		this._hoveredAnchor.OnHoverEnd();
		this._hoveredAnchor = null;
	}

	// Token: 0x06001400 RID: 5120 RVA: 0x00062500 File Offset: 0x00060700
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

	// Token: 0x04001613 RID: 5651
	public static AnchorUIManager Instance;

	// Token: 0x04001614 RID: 5652
	[SerializeField]
	[FormerlySerializedAs("createModeButton_")]
	private GameObject _createModeButton;

	// Token: 0x04001615 RID: 5653
	[SerializeField]
	[FormerlySerializedAs("selectModeButton_")]
	private GameObject _selectModeButton;

	// Token: 0x04001616 RID: 5654
	[SerializeField]
	[FormerlySerializedAs("trackedDevice_")]
	private Transform _trackedDevice;

	// Token: 0x04001617 RID: 5655
	private Transform _raycastOrigin;

	// Token: 0x04001618 RID: 5656
	private bool _drawRaycast;

	// Token: 0x04001619 RID: 5657
	[SerializeField]
	[FormerlySerializedAs("lineRenderer_")]
	private LineRenderer _lineRenderer;

	// Token: 0x0400161A RID: 5658
	private Anchor _hoveredAnchor;

	// Token: 0x0400161B RID: 5659
	private Anchor _selectedAnchor;

	// Token: 0x0400161C RID: 5660
	private AnchorUIManager.AnchorMode _mode = AnchorUIManager.AnchorMode.Select;

	// Token: 0x0400161D RID: 5661
	[SerializeField]
	[FormerlySerializedAs("buttonList_")]
	private List<Button> _buttonList;

	// Token: 0x0400161E RID: 5662
	private int _menuIndex;

	// Token: 0x0400161F RID: 5663
	private Button _selectedButton;

	// Token: 0x04001620 RID: 5664
	[SerializeField]
	private Anchor _anchorPrefab;

	// Token: 0x04001621 RID: 5665
	[SerializeField]
	[FormerlySerializedAs("placementPreview_")]
	private GameObject _placementPreview;

	// Token: 0x04001622 RID: 5666
	[SerializeField]
	[FormerlySerializedAs("anchorPlacementTransform_")]
	private Transform _anchorPlacementTransform;

	// Token: 0x04001623 RID: 5667
	private AnchorUIManager.PrimaryPressDelegate _primaryPressDelegate;

	// Token: 0x04001624 RID: 5668
	private bool _isFocused = true;

	// Token: 0x0200035A RID: 858
	public enum AnchorMode
	{
		// Token: 0x04001626 RID: 5670
		Create,
		// Token: 0x04001627 RID: 5671
		Select
	}

	// Token: 0x0200035B RID: 859
	// (Invoke) Token: 0x06001403 RID: 5123
	private delegate void PrimaryPressDelegate();
}
