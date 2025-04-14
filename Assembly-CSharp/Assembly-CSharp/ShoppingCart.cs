using System;
using UnityEngine;

// Token: 0x0200041A RID: 1050
public class ShoppingCart : MonoBehaviour
{
	// Token: 0x060019F5 RID: 6645 RVA: 0x0007FA31 File Offset: 0x0007DC31
	public void Awake()
	{
		if (ShoppingCart.instance == null)
		{
			ShoppingCart.instance = this;
			return;
		}
		if (ShoppingCart.instance != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060019F6 RID: 6646 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Start()
	{
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Update()
	{
	}

	// Token: 0x04001CCE RID: 7374
	[OnEnterPlay_SetNull]
	public static volatile ShoppingCart instance;
}
