using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CCC RID: 3276
	[ExecuteInEditMode]
	public class LatexFormula : MonoBehaviour
	{
		// Token: 0x040054B7 RID: 21687
		public static readonly string BaseUrl = "http://tex.s2cms.ru/svg/f(x) ";

		// Token: 0x040054B8 RID: 21688
		private int m_hash = LatexFormula.BaseUrl.GetHashCode();

		// Token: 0x040054B9 RID: 21689
		[SerializeField]
		private string m_formula = "";

		// Token: 0x040054BA RID: 21690
		private Texture m_texture;
	}
}
