using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200035D RID: 861
public class StartMenu : MonoBehaviour
{
	// Token: 0x0600140D RID: 5133 RVA: 0x000B9184 File Offset: 0x000B7384
	private void Start()
	{
		DebugUIBuilder.instance.AddLabel("Select Sample Scene", 0);
		int sceneCountInBuildSettings = SceneManager.sceneCountInBuildSettings;
		for (int i = 0; i < sceneCountInBuildSettings; i++)
		{
			string scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(i);
			int sceneIndex = i;
			DebugUIBuilder.instance.AddButton(Path.GetFileNameWithoutExtension(scenePathByBuildIndex), delegate
			{
				this.LoadScene(sceneIndex);
			}, -1, 0, false);
		}
		DebugUIBuilder.instance.Show();
	}

	// Token: 0x0600140E RID: 5134 RVA: 0x0003C812 File Offset: 0x0003AA12
	private void LoadScene(int idx)
	{
		DebugUIBuilder.instance.Hide();
		Debug.Log("Load scene: " + idx.ToString());
		SceneManager.LoadScene(idx);
	}

	// Token: 0x0400162A RID: 5674
	public OVROverlay overlay;

	// Token: 0x0400162B RID: 5675
	public OVROverlay text;

	// Token: 0x0400162C RID: 5676
	public OVRCameraRig vrRig;
}
