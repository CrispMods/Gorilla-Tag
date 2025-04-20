using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000308 RID: 776
public class CustomDebugUI : MonoBehaviour
{
	// Token: 0x0600128A RID: 4746 RVA: 0x0003CB80 File Offset: 0x0003AD80
	private void Awake()
	{
		CustomDebugUI.instance = this;
	}

	// Token: 0x0600128B RID: 4747 RVA: 0x000B1754 File Offset: 0x000AF954
	public RectTransform AddTextField(string label, int targetCanvas = 0)
	{
		RectTransform component = UnityEngine.Object.Instantiate<RectTransform>(this.textPrefab).GetComponent<RectTransform>();
		component.GetComponentInChildren<InputField>().text = label;
		DebugUIBuilder obj = DebugUIBuilder.instance;
		typeof(DebugUIBuilder).GetMethod("AddRect", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, new object[]
		{
			component,
			targetCanvas
		});
		return component;
	}

	// Token: 0x0600128C RID: 4748 RVA: 0x000B17B8 File Offset: 0x000AF9B8
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

	// Token: 0x04001482 RID: 5250
	[SerializeField]
	private RectTransform textPrefab;

	// Token: 0x04001483 RID: 5251
	public static CustomDebugUI instance;

	// Token: 0x04001484 RID: 5252
	private const BindingFlags privateFlags = BindingFlags.Instance | BindingFlags.NonPublic;
}
