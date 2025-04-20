using System;
using UnityEngine;

// Token: 0x02000388 RID: 904
internal interface ITetheredObjectBehavior
{
	// Token: 0x0600153B RID: 5435
	void DbgClear();

	// Token: 0x0600153C RID: 5436
	void EnableDistanceConstraints(bool v, float playerScale);

	// Token: 0x0600153D RID: 5437
	void EnableDynamics(bool enable, bool collider, bool kinematic);

	// Token: 0x0600153E RID: 5438
	bool IsEnabled();

	// Token: 0x0600153F RID: 5439
	void ReParent();

	// Token: 0x06001540 RID: 5440
	bool ReturnStep();

	// Token: 0x06001541 RID: 5441
	void TriggerEnter(Collider other, ref Vector3 force, ref Vector3 collisionPt, ref bool transferOwnership);
}
