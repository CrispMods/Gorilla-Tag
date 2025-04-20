using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BAD RID: 2989
	public class DestroyOnAwake : MonoBehaviour
	{
		// Token: 0x06004BDE RID: 19422 RVA: 0x001A30A8 File Offset: 0x001A12A8
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

		// Token: 0x06004BDF RID: 19423 RVA: 0x001A30A8 File Offset: 0x001A12A8
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

		// Token: 0x06004BE0 RID: 19424 RVA: 0x001A30A8 File Offset: 0x001A12A8
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
