using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000353 RID: 851
[Serializable]
public class Spawnable : ISerializationCallbackReceiver
{
	// Token: 0x060013C7 RID: 5063 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x0006137C File Offset: 0x0005F57C
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

	// Token: 0x060013CA RID: 5066 RVA: 0x000613F0 File Offset: 0x0005F5F0
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

	// Token: 0x040015F1 RID: 5617
	public SimpleResizable ResizablePrefab;

	// Token: 0x040015F2 RID: 5618
	public string ClassificationLabel = "";

	// Token: 0x040015F3 RID: 5619
	[SerializeField]
	private int _editorClassificationIndex;
}
