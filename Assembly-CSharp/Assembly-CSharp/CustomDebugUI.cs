using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002FD RID: 765
public class CustomDebugUI : MonoBehaviour
{
	// Token: 0x06001241 RID: 4673 RVA: 0x0005752A File Offset: 0x0005572A
	private void Awake()
	{
		CustomDebugUI.instance = this;
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x00057534 File Offset: 0x00055734
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

	// Token: 0x06001243 RID: 4675 RVA: 0x00057598 File Offset: 0x00055798
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

	// Token: 0x0400143B RID: 5179
	[SerializeField]
	private RectTransform textPrefab;

	// Token: 0x0400143C RID: 5180
	public static CustomDebugUI instance;

	// Token: 0x0400143D RID: 5181
	private const BindingFlags privateFlags = BindingFlags.Instance | BindingFlags.NonPublic;
}
