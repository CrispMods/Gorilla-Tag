using System;
using System.Collections.Generic;

// Token: 0x02000220 RID: 544
[Serializable]
public class SerializablePerformanceReport<T>
{
	// Token: 0x04000FF2 RID: 4082
	public string reportDate;

	// Token: 0x04000FF3 RID: 4083
	public string version;

	// Token: 0x04000FF4 RID: 4084
	public List<T> results;
}
