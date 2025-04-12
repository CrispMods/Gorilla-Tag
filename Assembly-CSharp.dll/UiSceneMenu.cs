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
	// Token: 0x0600142D RID: 5165 RVA: 0x000B9990 File Offset: 0x000B7B90
	private void Awake()
	{
		this.m_activeScene = SceneManager.GetActiveScene();
		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			string scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(i);
			this.CreateLabel(i, scenePathByBuildIndex);
		}
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x000B99C8 File Offset: 0x000B7BC8
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

	// Token: 0x0600142F RID: 5167 RVA: 0x0003C9D9 File Offset: 0x0003ABD9
	private bool InputPrevScene()
	{
		return this.KeyboardPrevScene() || this.ThumbstickPrevScene(OVRInput.Controller.LTouch) || this.ThumbstickPrevScene(OVRInput.Controller.RTouch);
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x0003C9F5 File Offset: 0x0003ABF5
	private bool InputNextScene()
	{
		return this.KeyboardNextScene() || this.ThumbstickNextScene(OVRInput.Controller.LTouch) || this.ThumbstickNextScene(OVRInput.Controller.RTouch);
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x0003CA11 File Offset: 0x0003AC11
	private bool KeyboardPrevScene()
	{
		return Input.GetKeyDown(KeyCode.UpArrow);
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x0003CA1D File Offset: 0x0003AC1D
	private bool KeyboardNextScene()
	{
		return Input.GetKeyDown(KeyCode.DownArrow);
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x0003CA29 File Offset: 0x0003AC29
	private bool ThumbstickPrevScene(OVRInput.Controller controller)
	{
		return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller).y >= 0.9f && this.GetLastThumbstickValue(controller).y < 0.9f;
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x0003CA53 File Offset: 0x0003AC53
	private bool ThumbstickNextScene(OVRInput.Controller controller)
	{
		return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller).y <= -0.9f && this.GetLastThumbstickValue(controller).y > -0.9f;
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x0003CA7D File Offset: 0x0003AC7D
	private Vector2 GetLastThumbstickValue(OVRInput.Controller controller)
	{
		if (controller != OVRInput.Controller.LTouch)
		{
			return UiSceneMenu.s_lastThumbstickR;
		}
		return UiSceneMenu.s_lastThumbstickL;
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x0003CA8E File Offset: 0x0003AC8E
	private void ChangeScene(int nextScene)
	{
		SceneManager.LoadScene(nextScene);
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x000B9A34 File Offset: 0x000B7C34
	private void CreateLabel(int sceneIndex, string scenePath)
	{
		string text = Path.GetFileNameWithoutExtension(scenePath);
		text = Regex.Replace(text, "[A-Z]", " $0").Trim();
		if (this.m_activeScene.buildIndex == sceneIndex)
		{
			text = "Open: " + text;
		}
		TextMeshProUGUI textMeshProUGUI = UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.m_labelPf);
		textMeshProUGUI.SetText(string.Format("{0}. {1}", sceneIndex + 1, text), true);
		textMeshProUGUI.transform.SetParent(this.m_layoutGroup.transform, false);
	}

	// Token: 0x0400165C RID: 5724
	[Header("Settings")]
	[SerializeField]
	private VerticalLayoutGroup m_layoutGroup;

	// Token: 0x0400165D RID: 5725
	[SerializeField]
	private TextMeshProUGUI m_labelPf;

	// Token: 0x0400165E RID: 5726
	private static Vector2 s_lastThumbstickL;

	// Token: 0x0400165F RID: 5727
	private static Vector2 s_lastThumbstickR;

	// Token: 0x04001660 RID: 5728
	private Scene m_activeScene;
}
