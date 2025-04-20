using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A95 RID: 2709
	public class SelectionCylinder : MonoBehaviour
	{
		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060043A8 RID: 17320 RVA: 0x0005C1DC File Offset: 0x0005A3DC
		// (set) Token: 0x060043A9 RID: 17321 RVA: 0x00178954 File Offset: 0x00176B54
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

		// Token: 0x060043AA RID: 17322 RVA: 0x001789B8 File Offset: 0x00176BB8
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

		// Token: 0x060043AB RID: 17323 RVA: 0x00178A5C File Offset: 0x00176C5C
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

		// Token: 0x060043AC RID: 17324 RVA: 0x00178A9C File Offset: 0x00176C9C
		private void AffectSelectionColor(Color[] newColors)
		{
			int num = newColors.Length;
			for (int i = 0; i < num; i++)
			{
				this._selectionMaterials[i].SetColor(SelectionCylinder._colorId, newColors[i]);
			}
		}

		// Token: 0x04004453 RID: 17491
		[SerializeField]
		private MeshRenderer _selectionMeshRenderer;

		// Token: 0x04004454 RID: 17492
		private static int _colorId = Shader.PropertyToID("_Color");

		// Token: 0x04004455 RID: 17493
		private Material[] _selectionMaterials;

		// Token: 0x04004456 RID: 17494
		private Color[] _defaultSelectionColors;

		// Token: 0x04004457 RID: 17495
		private Color[] _highlightColors;

		// Token: 0x04004458 RID: 17496
		private SelectionCylinder.SelectionState _currSelectionState;

		// Token: 0x02000A96 RID: 2710
		public enum SelectionState
		{
			// Token: 0x0400445A RID: 17498
			Off,
			// Token: 0x0400445B RID: 17499
			Selected,
			// Token: 0x0400445C RID: 17500
			Highlighted
		}
	}
}
