using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A68 RID: 2664
	public class SelectionCylinder : MonoBehaviour
	{
		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06004263 RID: 16995 RVA: 0x001392E2 File Offset: 0x001374E2
		// (set) Token: 0x06004264 RID: 16996 RVA: 0x001392EC File Offset: 0x001374EC
		public SelectionCylinder.SelectionState CurrSelectionState
		{
			get
			{
				return this._currSelectionState;
			}
			set
			{
				SelectionCylinder.SelectionState currSelectionState = this._currSelectionState;
				this._currSelectionState = value;
				if (currSelectionState != this._currSelectionState)
				{
					if (this._currSelectionState > SelectionCylinder.SelectionState.Off)
					{
						this._selectionMeshRenderer.enabled = true;
						this.AffectSelectionColor((this._currSelectionState == SelectionCylinder.SelectionState.Selected) ? this._defaultSelectionColors : this._highlightColors);
						return;
					}
					this._selectionMeshRenderer.enabled = false;
				}
			}
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x00139350 File Offset: 0x00137550
		private void Awake()
		{
			this._selectionMaterials = this._selectionMeshRenderer.materials;
			int num = this._selectionMaterials.Length;
			this._defaultSelectionColors = new Color[num];
			this._highlightColors = new Color[num];
			for (int i = 0; i < num; i++)
			{
				this._defaultSelectionColors[i] = this._selectionMaterials[i].GetColor(SelectionCylinder._colorId);
				this._highlightColors[i] = new Color(1f, 1f, 1f, this._defaultSelectionColors[i].a);
			}
			this.CurrSelectionState = SelectionCylinder.SelectionState.Off;
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x001393F4 File Offset: 0x001375F4
		private void OnDestroy()
		{
			if (this._selectionMaterials != null)
			{
				foreach (Material material in this._selectionMaterials)
				{
					if (material != null)
					{
						Object.Destroy(material);
					}
				}
			}
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x00139434 File Offset: 0x00137634
		private void AffectSelectionColor(Color[] newColors)
		{
			int num = newColors.Length;
			for (int i = 0; i < num; i++)
			{
				this._selectionMaterials[i].SetColor(SelectionCylinder._colorId, newColors[i]);
			}
		}

		// Token: 0x04004359 RID: 17241
		[SerializeField]
		private MeshRenderer _selectionMeshRenderer;

		// Token: 0x0400435A RID: 17242
		private static int _colorId = Shader.PropertyToID("_Color");

		// Token: 0x0400435B RID: 17243
		private Material[] _selectionMaterials;

		// Token: 0x0400435C RID: 17244
		private Color[] _defaultSelectionColors;

		// Token: 0x0400435D RID: 17245
		private Color[] _highlightColors;

		// Token: 0x0400435E RID: 17246
		private SelectionCylinder.SelectionState _currSelectionState;

		// Token: 0x02000A69 RID: 2665
		public enum SelectionState
		{
			// Token: 0x04004360 RID: 17248
			Off,
			// Token: 0x04004361 RID: 17249
			Selected,
			// Token: 0x04004362 RID: 17250
			Highlighted
		}
	}
}
