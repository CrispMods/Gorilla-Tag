using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000323 RID: 803
public class SceneSampler : MonoBehaviour
{
	// Token: 0x06001315 RID: 4885 RVA: 0x0005D330 File Offset: 0x0005B530
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06001316 RID: 4886 RVA: 0x0005D340 File Offset: 0x0005B540
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

	// Token: 0x04001515 RID: 5397
	private int currentSceneIndex;

	// Token: 0x04001516 RID: 5398
	public GameObject displayText;
}
