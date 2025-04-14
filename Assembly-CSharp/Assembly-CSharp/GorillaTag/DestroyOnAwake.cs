using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B83 RID: 2947
	public class DestroyOnAwake : MonoBehaviour
	{
		// Token: 0x06004A9F RID: 19103 RVA: 0x00169C78 File Offset: 0x00167E78
		protected void Awake()
		{
			try
			{
				Object.Destroy(base.gameObject);
			}
			catch
			{
			}
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x00169CA8 File Offset: 0x00167EA8
		protected void OnEnable()
		{
			try
			{
				Object.Destroy(base.gameObject);
			}
			catch
			{
			}
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x00169CD8 File Offset: 0x00167ED8
		protected void Update()
		{
			try
			{
				Object.Destroy(base.gameObject);
			}
			catch
			{
			}
		}
	}
}
