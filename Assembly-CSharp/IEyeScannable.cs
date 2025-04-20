using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000094 RID: 148
public interface IEyeScannable
{
	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060003CB RID: 971
	int scannableId { get; }

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060003CC RID: 972
	Vector3 Position { get; }

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060003CD RID: 973
	Bounds Bounds { get; }

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060003CE RID: 974
	IList<KeyValueStringPair> Entries { get; }

	// Token: 0x060003CF RID: 975
	void OnEnable();

	// Token: 0x060003D0 RID: 976
	void OnDisable();

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x060003D1 RID: 977
	// (remove) Token: 0x060003D2 RID: 978
	event Action OnDataChange;
}
