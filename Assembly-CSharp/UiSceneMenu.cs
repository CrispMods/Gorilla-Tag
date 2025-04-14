using System;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000366 RID: 870
public class UiSceneMenu : MonoBehaviour
{
	// Token: 0x0600142A RID: 5162 RVA: 0x00062DC8 File Offset: 0x00060FC8
	private void Awake()
	{
		this.m_activeScene = SceneManager.GetActiveScene();
		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			string scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(i);
			this.CreateLabel(i, scenePathByBuildIndex);
		}
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x00062E00 File Offset: 0x00061000
	private void Update()
	{
		int sceneCountInBuildSettings = SceneManager.sceneCountInBuildSettings;
		if (this.InputPrevScene())
		{
			this.ChangeScene((this.m_activeScene.buildIndex - 1 + sceneCountInBuildSettings) % sceneCountInBuildSettings);
		}
		else if (this.InputNextScene())
		{
			this.ChangeScene((this.m_activeScene.buildIndex + 1) % sceneCountInBuildSettings);
		}
		UiSceneMenu.s_lastThumbstickL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
		UiSceneMenu.s_lastThumbstickR = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x00062E69 File Offset: 0x00061069
	private bool InputPrevScene()
	{
		return this.KeyboardPrevScene() || this.ThumbstickPrevScene(OVRInput.Controller.LTouch) || this.ThumbstickPrevScene(OVRInput.Controller.RTouch);
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x00062E85 File Offset: 0x00061085
	private bool InputNextScene()
	{
		return this.KeyboardNextScene() || this.ThumbstickNextScene(OVRInput.Controller.LTouch) || this.ThumbstickNextScene(OVRInput.Controller.RTouch);
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x00062EA1 File Offset: 0x000610A1
	private bool KeyboardPrevScene()
	{
		return Input.GetKeyDown(KeyCode.UpArrow);
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x00062EAD File Offset: 0x000610AD
	private bool KeyboardNextScene()
	{
		return Input.GetKeyDown(KeyCode.DownArrow);
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x00062EB9 File Offset: 0x000610B9
	private bool ThumbstickPrevScene(OVRInput.Controller controller)
	{
		return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller).y >= 0.9f && this.GetLastThumbstickValue(controller).y < 0.9f;
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x00062EE3 File Offset: 0x000610E3
	private bool ThumbstickNextScene(OVRInput.Controller controller)
	{
		return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller).y <= -0.9f && this.GetLastThumbstickValue(controller).y > -0.9f;
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x00062F0D File Offset: 0x0006110D
	private Vector2 GetLastThumbstickValue(OVRInput.Controller controller)
	{
		if (controller != OVRInput.Controller.LTouch)
		{
			return UiSceneMenu.s_lastThumbstickR;
		}
		return UiSceneMenu.s_lastThumbstickL;
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x00062F1E File Offset: 0x0006111E
	private void ChangeScene(int nextScene)
	{
		SceneManager.LoadScene(nextScene);
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x00062F28 File Offset: 0x00061128
	private void CreateLabel(int sceneIndex, string scenePath)
	{
		string text = Path.GetFileNameWithoutExtension(scenePath);
		text = Regex.Replace(text, "[A-Z]", " $0").Trim();
		if (this.m_activeScene.buildIndex == sceneIndex)
		{
			text = "Open: " + text;
		}
		TextMeshProUGUI textMeshProUGUI = Object.Instantiate<TextMeshProUGUI>(this.m_labelPf);
		textMeshProUGUI.SetText(string.Format("{0}. {1}", sceneIndex + 1, text), true);
		textMeshProUGUI.transform.SetParent(this.m_layoutGroup.transform, false);
	}

	// Token: 0x0400165B RID: 5723
	[Header("Settings")]
	[SerializeField]
	private VerticalLayoutGroup m_layoutGroup;

	// Token: 0x0400165C RID: 5724
	[SerializeField]
	private TextMeshProUGUI m_labelPf;

	// Token: 0x0400165D RID: 5725
	private static Vector2 s_lastThumbstickL;

	// Token: 0x0400165E RID: 5726
	private static Vector2 s_lastThumbstickR;

	// Token: 0x0400165F RID: 5727
	private Scene m_activeScene;
}
