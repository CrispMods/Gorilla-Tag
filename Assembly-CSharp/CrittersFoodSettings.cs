using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class CrittersFoodSettings : CrittersActorSettings
{
	// Token: 0x0600017B RID: 379 RVA: 0x0006E800 File Offset: 0x0006CA00
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

	// Token: 0x040001D0 RID: 464
	public float _maxFood;

	// Token: 0x040001D1 RID: 465
	public float _currentFood;

	// Token: 0x040001D2 RID: 466
	public float _startingSize;

	// Token: 0x040001D3 RID: 467
	public float _currentSize;

	// Token: 0x040001D4 RID: 468
	public Transform _food;

	// Token: 0x040001D5 RID: 469
	public bool _disableWhenEmpty;
}
