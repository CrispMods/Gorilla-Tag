using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000323 RID: 803
public class SceneSampler : MonoBehaviour
{
	// Token: 0x06001312 RID: 4882 RVA: 0x0005CFAC File Offset: 0x0005B1AC
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06001313 RID: 4883 RVA: 0x0005CFBC File Offset: 0x0005B1BC
	private void Update()
	{
		bool active = OVRInput.GetActiveController() == OVRInput.Controller.Touch || OVRInput.GetActiveController() == OVRInput.Controller.LTouch || OVRInput.GetActiveController() == OVRInput.Controller.RTouch;
		this.displayText.SetActive(active);
		if (OVRInput.GetUp(OVRInput.Button.Start, OVRInput.Controller.Active))
		{
			this.currentSceneIndex++;
			if (this.currentSceneIndex >= SceneManager.sceneCountInBuildSettings)
			{
				this.currentSceneIndex = 0;
			}
			SceneManager.LoadScene(this.currentSceneIndex);
		}
		Vector3 vector = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch) + Vector3.up * 0.09f;
		this.displayText.transform.position = vector;
		this.displayText.transform.rotation = Quaternion.LookRotation(vector - Camera.main.transform.position);
	}

	// Token: 0x04001514 RID: 5396
	private int currentSceneIndex;

	// Token: 0x04001515 RID: 5397
	public GameObject displayText;
}
