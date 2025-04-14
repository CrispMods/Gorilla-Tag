using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002FD RID: 765
public class CustomDebugUI : MonoBehaviour
{
	// Token: 0x0600123E RID: 4670 RVA: 0x000571A6 File Offset: 0x000553A6
	private void Awake()
	{
		CustomDebugUI.instance = this;
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x000571B0 File Offset: 0x000553B0
	public RectTransform AddTextField(string label, int targetCanvas = 0)
	{
		RectTransform component = Object.Instantiate<RectTransform>(this.textPrefab).GetComponent<RectTransform>();
		component.GetComponentInChildren<InputField>().text = label;
		DebugUIBuilder obj = DebugUIBuilder.instance;
		typeof(DebugUIBuilder).GetMethod("AddRect", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, new object[]
		{
			component,
			targetCanvas
		});
		return component;
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x00057214 File Offset: 0x00055414
	public void RemoveFromCanvas(RectTransform element, int targetCanvas = 0)
	{
		DebugUIBuilder obj = DebugUIBuilder.instance;
		FieldInfo field = typeof(DebugUIBuilder).GetField("insertedElements", BindingFlags.Instance | BindingFlags.NonPublic);
		MethodInfo method = typeof(DebugUIBuilder).GetMethod("Relayout", BindingFlags.Instance | BindingFlags.NonPublic);
		List<RectTransform>[] array = (List<RectTransform>[])field.GetValue(obj);
		if (targetCanvas > -1 && targetCanvas < array.Length - 1)
		{
			array[targetCanvas].Remove(element);
			element.SetParent(null);
			method.Invoke(obj, new object[0]);
		}
	}

	// Token: 0x0400143A RID: 5178
	[SerializeField]
	private RectTransform textPrefab;

	// Token: 0x0400143B RID: 5179
	public static CustomDebugUI instance;

	// Token: 0x0400143C RID: 5180
	private const BindingFlags privateFlags = BindingFlags.Instance | BindingFlags.NonPublic;
}
