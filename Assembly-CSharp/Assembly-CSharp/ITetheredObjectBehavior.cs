using System;
using UnityEngine;

// Token: 0x0200037D RID: 893
internal interface ITetheredObjectBehavior
{
	// Token: 0x060014F2 RID: 5362
	void DbgClear();

	// Token: 0x060014F3 RID: 5363
	void EnableDistanceConstraints(bool v, float playerScale);

	// Token: 0x060014F4 RID: 5364
	void EnableDynamics(bool enable, bool collider, bool kinematic);

	// Token: 0x060014F5 RID: 5365
	bool IsEnabled();

	// Token: 0x060014F6 RID: 5366
	void ReParent();

	// Token: 0x060014F7 RID: 5367
	bool ReturnStep();

	// Token: 0x060014F8 RID: 5368
	void TriggerEnter(Collider other, ref Vector3 force, ref Vector3 collisionPt, ref bool transferOwnership);
}
