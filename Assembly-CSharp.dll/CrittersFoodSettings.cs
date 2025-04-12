using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class CrittersFoodSettings : CrittersActorSettings
{
	// Token: 0x06000163 RID: 355 RVA: 0x0006C430 File Offset: 0x0006A630
	public override void UpdateActorSettings()
	{
		base.UpdateActorSettings();
		CrittersFood crittersFood = (CrittersFood)this.parentActor;
		crittersFood.maxFood = this._maxFood;
		crittersFood.currentFood = this._currentFood;
		crittersFood.startingSize = this._startingSize;
		crittersFood.currentSize = this._currentSize;
		crittersFood.food = this._food;
		crittersFood.disableWhenEmpty = this._disableWhenEmpty;
		crittersFood.SpawnData(this._maxFood, this._currentFood, this._startingSize);
	}

	// Token: 0x040001AB RID: 427
	public float _maxFood;

	// Token: 0x040001AC RID: 428
	public float _currentFood;

	// Token: 0x040001AD RID: 429
	public float _startingSize;

	// Token: 0x040001AE RID: 430
	public float _currentSize;

	// Token: 0x040001AF RID: 431
	public Transform _food;

	// Token: 0x040001B0 RID: 432
	public bool _disableWhenEmpty;
}
