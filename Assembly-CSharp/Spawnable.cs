using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200035E RID: 862
[Serializable]
public class Spawnable : ISerializationCallbackReceiver
{
	// Token: 0x06001413 RID: 5139 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x000BACBC File Offset: 0x000B8EBC
	public void OnAfterDeserialize()
	{
		if (this.ClassificationLabel != "")
		{
			this._editorClassificationIndex = Spawnable.<OnAfterDeserialize>g__IndexOf|4_0(this.ClassificationLabel, OVRSceneManager.Classification.List);
			if (this._editorClassificationIndex < 0)
			{
				Debug.LogError("[Spawnable] OnAfterDeserialize() " + this.ClassificationLabel + " not found. The Classification list in OVRSceneManager has likely changed");
				return;
			}
		}
		else
		{
			this._editorClassificationIndex = 0;
		}
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x000BAD1C File Offset: 0x000B8F1C
	[CompilerGenerated]
	internal static int <OnAfterDeserialize>g__IndexOf|4_0(string label, IEnumerable<string> collection)
	{
		int num = 0;
		using (IEnumerator<string> enumerator = collection.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == label)
				{
					return num;
				}
				num++;
			}
		}
		return -1;
	}

	// Token: 0x04001639 RID: 5689
	public SimpleResizable ResizablePrefab;

	// Token: 0x0400163A RID: 5690
	public string ClassificationLabel = "";

	// Token: 0x0400163B RID: 5691
	[SerializeField]
	private int _editorClassificationIndex;
}
