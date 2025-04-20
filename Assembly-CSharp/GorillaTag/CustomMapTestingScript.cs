using System;
using System.Collections;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BDF RID: 3039
	public class CustomMapTestingScript : GorillaPressableButton
	{
		// Token: 0x06004CFF RID: 19711 RVA: 0x000628C9 File Offset: 0x00060AC9
		public override void ButtonActivation()
		{
			base.ButtonActivation();
			base.StartCoroutine(this.ButtonPressed_Local());
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x000628DE File Offset: 0x00060ADE
		private IEnumerator ButtonPressed_Local()
		{
			this.isOn = true;
			this.UpdateColor();
			yield return new WaitForSeconds(this.debounceTime);
			this.isOn = false;
			this.UpdateColor();
			yield break;
		}
	}
}
