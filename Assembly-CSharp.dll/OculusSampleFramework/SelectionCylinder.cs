using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A6B RID: 2667
	public class SelectionCylinder : MonoBehaviour
	{
		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600426F RID: 17007 RVA: 0x0005A7DA File Offset: 0x000589DA
		// (set) Token: 0x06004270 RID: 17008 RVA: 0x00171AD0 File Offset: 0x0016FCD0
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

		// Token: 0x06004271 RID: 17009 RVA: 0x00171B34 File Offset: 0x0016FD34
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

		// Token: 0x06004272 RID: 17010 RVA: 0x00171BD8 File Offset: 0x0016FDD8
		private void OnDestroy()
		{
			if (this._selectionMaterials != null)
			{
				foreach (Material material in this._selectionMaterials)
				{
					if (material != null)
					{
						UnityEngine.Object.Destroy(material);
					}
				}
			}
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x00171C18 File Offset: 0x0016FE18
		private void AffectSelectionColor(Color[] newColors)
		{
			int num = newColors.Length;
			for (int i = 0; i < num; i++)
			{
				this._selectionMaterials[i].SetColor(SelectionCylinder._colorId, newColors[i]);
			}
		}

		// Token: 0x0400436B RID: 17259
		[SerializeField]
		private MeshRenderer _selectionMeshRenderer;

		// Token: 0x0400436C RID: 17260
		private static int _colorId = Shader.PropertyToID("_Color");

		// Token: 0x0400436D RID: 17261
		private Material[] _selectionMaterials;

		// Token: 0x0400436E RID: 17262
		private Color[] _defaultSelectionColors;

		// Token: 0x0400436F RID: 17263
		private Color[] _highlightColors;

		// Token: 0x04004370 RID: 17264
		private SelectionCylinder.SelectionState _currSelectionState;

		// Token: 0x02000A6C RID: 2668
		public enum SelectionState
		{
			// Token: 0x04004372 RID: 17266
			Off,
			// Token: 0x04004373 RID: 17267
			Selected,
			// Token: 0x04004374 RID: 17268
			Highlighted
		}
	}
}
