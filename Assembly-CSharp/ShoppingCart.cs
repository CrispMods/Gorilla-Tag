using System;
using UnityEngine;

// Token: 0x02000425 RID: 1061
public class ShoppingCart : MonoBehaviour
{
	// Token: 0x06001A3F RID: 6719 RVA: 0x00041B56 File Offset: 0x0003FD56
	public void Awake()
	{
		if (ShoppingCart.instance == null)
		{
			ShoppingCart.instance = this;
			return;
		}
		if (ShoppingCart.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x00030607 File Offset: 0x0002E807
	private void Start()
	{
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x00030607 File Offset: 0x0002E807
	private void Update()
	{
	}

	// Token: 0x04001D16 RID: 7446
	[OnEnterPlay_SetNull]
	public static volatile ShoppingCart instance;
}
