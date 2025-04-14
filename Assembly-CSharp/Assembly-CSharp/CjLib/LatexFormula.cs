using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C9E RID: 3230
	[ExecuteInEditMode]
	public class LatexFormula : MonoBehaviour
	{
		// Token: 0x040053BD RID: 21437
		public static readonly string BaseUrl = "http://tex.s2cms.ru/svg/f(x) ";

		// Token: 0x040053BE RID: 21438
		private int m_hash = LatexFormula.BaseUrl.GetHashCode();

		// Token: 0x040053BF RID: 21439
		[SerializeField]
		private string m_formula = "";

		// Token: 0x040053C0 RID: 21440
		private Texture m_texture;
	}
}
