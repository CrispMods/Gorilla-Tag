using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B80 RID: 2944
	public class DestroyOnAwake : MonoBehaviour
	{
		// Token: 0x06004A93 RID: 19091 RVA: 0x001696B0 File Offset: 0x001678B0
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

		// Token: 0x06004A94 RID: 19092 RVA: 0x001696E0 File Offset: 0x001678E0
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

		// Token: 0x06004A95 RID: 19093 RVA: 0x00169710 File Offset: 0x00167910
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
