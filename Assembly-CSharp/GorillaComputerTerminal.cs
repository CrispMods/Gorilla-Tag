using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200065D RID: 1629
public class GorillaComputerTerminal : MonoBehaviour, IBuildValidation
{
	// Token: 0x0600285F RID: 10335 RVA: 0x000C6874 File Offset: 0x000C4A74
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

	// Token: 0x06002860 RID: 10336 RVA: 0x000C68D2 File Offset: 0x000C4AD2
	private void OnEnable()
	{
		if (GorillaComputer.instance == null)
		{
			base.StartCoroutine(this.<OnEnable>g__OnEnable_Local|4_0());
			return;
		}
		this.Init();
	}

	// Token: 0x06002861 RID: 10337 RVA: 0x000C68F8 File Offset: 0x000C4AF8
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

	// Token: 0x06002862 RID: 10338 RVA: 0x000C69B0 File Offset: 0x000C4BB0
	private void OnDisable()
	{
		GameEvents.ScreenTextChangedEvent.RemoveListener(new UnityAction<string>(this.OnScreenTextChanged));
		GameEvents.FunctionSelectTextChangedEvent.RemoveListener(new UnityAction<string>(this.OnFunctionTextChanged));
		GameEvents.ScreenTextMaterialsEvent.RemoveListener(new UnityAction<Material[]>(this.OnMaterialsChanged));
	}

	// Token: 0x06002863 RID: 10339 RVA: 0x000C69FF File Offset: 0x000C4BFF
	public void OnScreenTextChanged(string text)
	{
		this.myScreenText.text = text;
	}

	// Token: 0x06002864 RID: 10340 RVA: 0x000C6A0D File Offset: 0x000C4C0D
	public void OnFunctionTextChanged(string text)
	{
		this.myFunctionText.text = text;
	}

	// Token: 0x06002865 RID: 10341 RVA: 0x000C6A1B File Offset: 0x000C4C1B
	private void OnMaterialsChanged(Material[] materials)
	{
		this.monitorMesh.materials = materials;
	}

	// Token: 0x06002867 RID: 10343 RVA: 0x000C6A29 File Offset: 0x000C4C29
	[CompilerGenerated]
	private IEnumerator <OnEnable>g__OnEnable_Local|4_0()
	{
		yield return new WaitUntil(() => GorillaComputer.instance != null);
		yield return null;
		this.Init();
		yield break;
	}

	// Token: 0x04002D3C RID: 11580
	public TextMeshPro myScreenText;

	// Token: 0x04002D3D RID: 11581
	public TextMeshPro myFunctionText;

	// Token: 0x04002D3E RID: 11582
	public MeshRenderer monitorMesh;
}
