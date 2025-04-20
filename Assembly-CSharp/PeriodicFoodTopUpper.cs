using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class PeriodicFoodTopUpper : MonoBehaviour
{
	// Token: 0x0600031A RID: 794 RVA: 0x00032645 File Offset: 0x00030845
	private void Awake()
	{
		this.food = base.GetComponentInParent<CrittersFood>();
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00076B78 File Offset: 0x00074D78
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

	// Token: 0x040003B3 RID: 947
	private CrittersFood food;

	// Token: 0x040003B4 RID: 948
	private float timeFoodEmpty;

	// Token: 0x040003B5 RID: 949
	private bool waitingToRefill;

	// Token: 0x040003B6 RID: 950
	public float waitToRefill = 10f;

	// Token: 0x040003B7 RID: 951
	public GameObject foodObject;
}
