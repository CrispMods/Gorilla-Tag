using System;
using UnityEngine;

// Token: 0x0200041A RID: 1050
public class ShoppingCart : MonoBehaviour
{
	// Token: 0x060019F5 RID: 6645 RVA: 0x0004086C File Offset: 0x0003EA6C
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

	// Token: 0x060019F6 RID: 6646 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void Start()
	{
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void Update()
	{
	}

	// Token: 0x04001CCE RID: 7374
	[OnEnterPlay_SetNull]
	public static volatile ShoppingCart instance;
}
