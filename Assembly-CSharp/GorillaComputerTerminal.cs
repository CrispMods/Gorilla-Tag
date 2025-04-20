using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200063C RID: 1596
public class GorillaComputerTerminal : MonoBehaviour, IBuildValidation
{
	// Token: 0x0600278A RID: 10122 RVA: 0x0010D880 File Offset: 0x0010BA80
	public bool BuildValidationCheck()
	{
		if (this.myScreenText == null || this.myFunctionText == null || this.monitorMesh == null)
		{
			Debug.LogErrorFormat(base.gameObject, "gorilla computer terminal {0} is missing screen text, function text, or monitor mesh. this will break lots of computer stuff", new object[]
			{
				base.gameObject.name
			});
			return false;
		}
		return true;
	}

	// Token: 0x0600278B RID: 10123 RVA: 0x0004AEE2 File Offset: 0x000490E2
	private void OnEnable()
	{
		if (GorillaComputer.instance == null)
		{
			base.StartCoroutine(this.<OnEnable>g__OnEnable_Local|4_0());
			return;
		}
		this.Init();
	}

	// Token: 0x0600278C RID: 10124 RVA: 0x0010D8E0 File Offset: 0x0010BAE0
	private void Init()
	{
		GameEvents.ScreenTextChangedEvent.AddListener(new UnityAction<string>(this.OnScreenTextChanged));
		GameEvents.FunctionSelectTextChangedEvent.AddListener(new UnityAction<string>(this.OnFunctionTextChanged));
		GameEvents.ScreenTextMaterialsEvent.AddListener(new UnityAction<Material[]>(this.OnMaterialsChanged));
		this.myScreenText.text = GorillaComputer.instance.screenText.Text;
		this.myFunctionText.text = GorillaComputer.instance.functionSelectText.Text;
		if (GorillaComputer.instance.screenText.currentMaterials != null)
		{
			this.monitorMesh.materials = GorillaComputer.instance.screenText.currentMaterials;
		}
	}

	// Token: 0x0600278D RID: 10125 RVA: 0x0010D998 File Offset: 0x0010BB98
	private void OnDisable()
	{
		GameEvents.ScreenTextChangedEvent.RemoveListener(new UnityAction<string>(this.OnScreenTextChanged));
		GameEvents.FunctionSelectTextChangedEvent.RemoveListener(new UnityAction<string>(this.OnFunctionTextChanged));
		GameEvents.ScreenTextMaterialsEvent.RemoveListener(new UnityAction<Material[]>(this.OnMaterialsChanged));
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x0004AF07 File Offset: 0x00049107
	public void OnScreenTextChanged(string text)
	{
		this.myScreenText.text = text;
	}

	// Token: 0x0600278F RID: 10127 RVA: 0x0004AF15 File Offset: 0x00049115
	public void OnFunctionTextChanged(string text)
	{
		this.myFunctionText.text = text;
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x0004AF23 File Offset: 0x00049123
	private void OnMaterialsChanged(Material[] materials)
	{
		this.monitorMesh.materials = materials;
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x0004AF31 File Offset: 0x00049131
	[CompilerGenerated]
	private IEnumerator <OnEnable>g__OnEnable_Local|4_0()
	{
		yield return new WaitUntil(() => GorillaComputer.instance != null);
		yield return null;
		this.Init();
		yield break;
	}

	// Token: 0x04002CA2 RID: 11426
	public TextMeshPro myScreenText;

	// Token: 0x04002CA3 RID: 11427
	public TextMeshPro myFunctionText;

	// Token: 0x04002CA4 RID: 11428
	public MeshRenderer monitorMesh;
}
