using System;
using UnityEngine;

// Token: 0x0200041A RID: 1050
public class ShoppingCart : MonoBehaviour
{
	// Token: 0x060019F2 RID: 6642 RVA: 0x0007F6AD File Offset: 0x0007D8AD
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

	// Token: 0x060019F3 RID: 6643 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Start()
	{
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Update()
	{
	}

	// Token: 0x04001CCD RID: 7373
	[OnEnterPlay_SetNull]
	public static volatile ShoppingCart instance;
}
