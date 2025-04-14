using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008D RID: 141
public interface IEyeScannable
{
	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000399 RID: 921
	int scannableId { get; }

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x0600039A RID: 922
	Vector3 Position { get; }

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x0600039B RID: 923
	Bounds Bounds { get; }

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600039C RID: 924
	IList<KeyValueStringPair> Entries { get; }

	// Token: 0x0600039D RID: 925
	void OnEnable();

	// Token: 0x0600039E RID: 926
	void OnDisable();

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x0600039F RID: 927
	// (remove) Token: 0x060003A0 RID: 928
	event Action OnDataChange;
}
