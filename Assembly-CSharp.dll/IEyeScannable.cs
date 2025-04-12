using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008D RID: 141
public interface IEyeScannable
{
	// Token: 0x1700003A RID: 58
	// (get) Token: 0x0600039B RID: 923
	int scannableId { get; }

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x0600039C RID: 924
	Vector3 Position { get; }

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x0600039D RID: 925
	Bounds Bounds { get; }

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600039E RID: 926
	IList<KeyValueStringPair> Entries { get; }

	// Token: 0x0600039F RID: 927
	void OnEnable();

	// Token: 0x060003A0 RID: 928
	void OnDisable();

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x060003A1 RID: 929
	// (remove) Token: 0x060003A2 RID: 930
	event Action OnDataChange;
}
