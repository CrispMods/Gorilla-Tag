using System;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class PeriodicFoodTopUpper : MonoBehaviour
{
	// Token: 0x060002ED RID: 749 RVA: 0x00031512 File Offset: 0x0002F712
	private void Awake()
	{
		this.food = base.GetComponentInParent<CrittersFood>();
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00074450 File Offset: 0x00072650
	private void Update()
	{
		if (!CrittersManager.instance.LocalAuthority())
		{
			return;
		}
		if (!this.waitingToRefill && this.food.currentFood == 0f)
		{
			this.waitingToRefill = true;
			this.timeFoodEmpty = Time.time;
		}
		if (this.waitingToRefill && Time.time > this.timeFoodEmpty + this.waitToRefill)
		{
			this.waitingToRefill = false;
			this.food.Initialize();
		}
	}

	// Token: 0x04000382 RID: 898
	private CrittersFood food;

	// Token: 0x04000383 RID: 899
	private float timeFoodEmpty;

	// Token: 0x04000384 RID: 900
	private bool waitingToRefill;

	// Token: 0x04000385 RID: 901
	public float waitToRefill = 10f;

	// Token: 0x04000386 RID: 902
	public GameObject foodObject;
}
