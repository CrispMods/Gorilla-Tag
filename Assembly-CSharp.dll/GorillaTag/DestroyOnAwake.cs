using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B83 RID: 2947
	public class DestroyOnAwake : MonoBehaviour
	{
		// Token: 0x06004A9F RID: 19103 RVA: 0x0019C090 File Offset: 0x0019A290
		protected void Awake()
		{
			try
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			catch
			{
			}
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x0019C090 File Offset: 0x0019A290
		protected void OnEnable()
		{
			try
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			catch
			{
			}
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x0019C090 File Offset: 0x0019A290
		protected void Update()
		{
			try
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			catch
			{
			}
		}
	}
}
