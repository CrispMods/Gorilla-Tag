using System;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000371 RID: 881
public class UiSceneMenu : MonoBehaviour
{
	// Token: 0x06001476 RID: 5238 RVA: 0x000BC228 File Offset: 0x000BA428
	private void Awake()
	{
		this.m_activeScene = SceneManager.GetActiveScene();
		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			string scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(i);
			this.CreateLabel(i, scenePathByBuildIndex);
		}
	}

	// Token: 0x06001477 RID: 5239 RVA: 0x000BC260 File Offset: 0x000BA460
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

	// Token: 0x06001478 RID: 5240 RVA: 0x0003DC99 File Offset: 0x0003BE99
	private bool InputPrevScene()
	{
		return this.KeyboardPrevScene() || this.ThumbstickPrevScene(OVRInput.Controller.LTouch) || this.ThumbstickPrevScene(OVRInput.Controller.RTouch);
	}

	// Token: 0x06001479 RID: 5241 RVA: 0x0003DCB5 File Offset: 0x0003BEB5
	private bool InputNextScene()
	{
		return this.KeyboardNextScene() || this.ThumbstickNextScene(OVRInput.Controller.LTouch) || this.ThumbstickNextScene(OVRInput.Controller.RTouch);
	}

	// Token: 0x0600147A RID: 5242 RVA: 0x0003DCD1 File Offset: 0x0003BED1
	private bool KeyboardPrevScene()
	{
		return Input.GetKeyDown(KeyCode.UpArrow);
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x0003DCDD File Offset: 0x0003BEDD
	private bool KeyboardNextScene()
	{
		return Input.GetKeyDown(KeyCode.DownArrow);
	}

	// Token: 0x0600147C RID: 5244 RVA: 0x0003DCE9 File Offset: 0x0003BEE9
	private bool ThumbstickPrevScene(OVRInput.Controller controller)
	{
		return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller).y >= 0.9f && this.GetLastThumbstickValue(controller).y < 0.9f;
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x0003DD13 File Offset: 0x0003BF13
	private bool ThumbstickNextScene(OVRInput.Controller controller)
	{
		return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller).y <= -0.9f && this.GetLastThumbstickValue(controller).y > -0.9f;
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x0003DD3D File Offset: 0x0003BF3D
	private Vector2 GetLastThumbstickValue(OVRInput.Controller controller)
	{
		if (controller != OVRInput.Controller.LTouch)
		{
			return UiSceneMenu.s_lastThumbstickR;
		}
		return UiSceneMenu.s_lastThumbstickL;
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x0003DD4E File Offset: 0x0003BF4E
	private void ChangeScene(int nextScene)
	{
		SceneManager.LoadScene(nextScene);
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x000BC2CC File Offset: 0x000BA4CC
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

	// Token: 0x040016A3 RID: 5795
	[Header("Settings")]
	[SerializeField]
	private VerticalLayoutGroup m_layoutGroup;

	// Token: 0x040016A4 RID: 5796
	[SerializeField]
	private TextMeshProUGUI m_labelPf;

	// Token: 0x040016A5 RID: 5797
	private static Vector2 s_lastThumbstickL;

	// Token: 0x040016A6 RID: 5798
	private static Vector2 s_lastThumbstickR;

	// Token: 0x040016A7 RID: 5799
	private Scene m_activeScene;
}
