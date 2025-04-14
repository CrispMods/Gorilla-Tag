using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C9B RID: 3227
	[ExecuteInEditMode]
	public class LatexFormula : MonoBehaviour
	{
		// Token: 0x040053AB RID: 21419
		public static readonly string BaseUrl = "http://tex.s2cms.ru/svg/f(x) ";

		// Token: 0x040053AC RID: 21420
		private int m_hash = LatexFormula.BaseUrl.GetHashCode();

		// Token: 0x040053AD RID: 21421
		[SerializeField]
		private string m_formula = "";

		// Token: 0x040053AE RID: 21422
		private Texture m_texture;
	}
}
