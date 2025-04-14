using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuildSafe
{
	// Token: 0x02000A2D RID: 2605
	public class SceneBakeExampleTask : SceneBakeTask
	{
		// Token: 0x0600411E RID: 16670 RVA: 0x00134FA4 File Offset: 0x001331A4
		public override void OnSceneBake(Scene scene, SceneBakeMode mode)
		{
			for (int i = 0; i < 10; i++)
			{
				SceneBakeExampleTask.DuplicateAndRecolor(base.gameObject);
			}
			if (mode != SceneBakeMode.OnBuildPlayer)
			{
			}
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x00134FD4 File Offset: 0x001331D4
		private static void DuplicateAndRecolor(GameObject target)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(target);
			gameObject.transform.position = Random.insideUnitSphere * 4f;
			MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
			component.material = new Material(component.sharedMaterial)
			{
				color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f)
			};
		}
	}
}
