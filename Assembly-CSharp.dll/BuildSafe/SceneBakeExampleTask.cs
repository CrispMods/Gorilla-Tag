using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuildSafe
{
	// Token: 0x02000A30 RID: 2608
	public class SceneBakeExampleTask : SceneBakeTask
	{
		// Token: 0x0600412A RID: 16682 RVA: 0x0016E2D8 File Offset: 0x0016C4D8
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

		// Token: 0x0600412B RID: 16683 RVA: 0x0016E308 File Offset: 0x0016C508
		private static void DuplicateAndRecolor(GameObject target)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(target);
			gameObject.transform.position = UnityEngine.Random.insideUnitSphere * 4f;
			MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
			component.material = new Material(component.sharedMaterial)
			{
				color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f)
			};
		}
	}
}
