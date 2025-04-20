using System;
using System.Collections.Generic;

// Token: 0x0200022B RID: 555
[Serializable]
public class SerializablePerformanceReport<T>
{
	// Token: 0x04001037 RID: 4151
	public string reportDate;

	// Token: 0x04001038 RID: 4152
	public string version;

	// Token: 0x04001039 RID: 4153
	public List<T> results;
}
