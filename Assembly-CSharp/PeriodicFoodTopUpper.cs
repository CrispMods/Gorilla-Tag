using System;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class PeriodicFoodTopUpper : MonoBehaviour
{
	// Token: 0x060002EB RID: 747 RVA: 0x0001232C File Offset: 0x0001052C
	private void Awake()
	{
		this.food = base.GetComponentInParent<CrittersFood>();
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0001233C File Offset: 0x0001053C
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

	// Token: 0x04000381 RID: 897
	private CrittersFood food;

	// Token: 0x04000382 RID: 898
	private float timeFoodEmpty;

	// Token: 0x04000383 RID: 899
	private bool waitingToRefill;

	// Token: 0x04000384 RID: 900
	public float waitToRefill = 10f;

	// Token: 0x04000385 RID: 901
	public GameObject foodObject;
}
