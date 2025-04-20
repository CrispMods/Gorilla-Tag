using System;
using System.Collections.Generic;
using System.Reflection;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.Animations.Rigging;

// Token: 0x020003B6 RID: 950
public class CalibrationCube : MonoBehaviour
{
	// Token: 0x06001639 RID: 5689 RVA: 0x0003F070 File Offset: 0x0003D270
	private void Awake()
	{
		this.calibratedLength = this.baseLength;
	}

	// Token: 0x0600163A RID: 5690 RVA: 0x000C1E44 File Offset: 0x000C0044
	private void Start()
	{
		try
		{
			this.OnCollisionExit(null);
		}
		catch
		{
		}
	}

	// Token: 0x0600163B RID: 5691 RVA: 0x00030607 File Offset: 0x0002E807
	private void OnTriggerEnter(Collider other)
	{
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x00030607 File Offset: 0x0002E807
	private void OnTriggerExit(Collider other)
	{
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x000C1E70 File Offset: 0x000C0070
	public void RecalibrateSize(bool pressed)
	{
		this.lastCalibratedLength = this.calibratedLength;
		this.calibratedLength = (this.rightController.transform.position - this.leftController.transform.position).magnitude;
		this.calibratedLength = ((this.calibratedLength > this.maxLength) ? this.maxLength : ((this.calibratedLength < this.minLength) ? this.minLength : this.calibratedLength));
		float d = this.calibratedLength / this.lastCalibratedLength;
		Vector3 localScale = this.playerBody.transform.localScale;
		this.playerBody.GetComponentInChildren<RigBuilder>().Clear();
		this.playerBody.transform.localScale = new Vector3(1f, 1f, 1f);
		this.playerBody.GetComponentInChildren<TransformReset>().ResetTransforms();
		this.playerBody.transform.localScale = d * localScale;
		this.playerBody.GetComponentInChildren<RigBuilder>().Build();
		this.playerBody.GetComponentInChildren<VRRig>().SetHeadBodyOffset();
		GorillaPlaySpace.Instance.bodyColliderOffset *= d;
		GorillaPlaySpace.Instance.bodyCollider.gameObject.transform.localScale *= d;
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x00030607 File Offset: 0x0002E807
	private void OnCollisionEnter(Collision collision)
	{
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x000C1FCC File Offset: 0x000C01CC
	private void OnCollisionExit(Collision collision)
	{
		try
		{
			bool flag = false;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				AssemblyName name = assemblies[i].GetName();
				if (!this.calibrationPresetsTest3[0].Contains(name.Name))
				{
					flag = true;
				}
			}
			if (!flag || Application.platform == RuntimePlatform.Android)
			{
				GorillaComputer.instance.includeUpdatedServerSynchTest = 0;
			}
		}
		catch
		{
		}
	}

	// Token: 0x04001868 RID: 6248
	public PrimaryButtonWatcher watcher;

	// Token: 0x04001869 RID: 6249
	public GameObject rightController;

	// Token: 0x0400186A RID: 6250
	public GameObject leftController;

	// Token: 0x0400186B RID: 6251
	public GameObject playerBody;

	// Token: 0x0400186C RID: 6252
	private float calibratedLength;

	// Token: 0x0400186D RID: 6253
	private float lastCalibratedLength;

	// Token: 0x0400186E RID: 6254
	public float minLength = 1f;

	// Token: 0x0400186F RID: 6255
	public float maxLength = 2.5f;

	// Token: 0x04001870 RID: 6256
	public float baseLength = 1.61f;

	// Token: 0x04001871 RID: 6257
	public string[] calibrationPresets;

	// Token: 0x04001872 RID: 6258
	public string[] calibrationPresetsTest;

	// Token: 0x04001873 RID: 6259
	public string[] calibrationPresetsTest2;

	// Token: 0x04001874 RID: 6260
	public string[] calibrationPresetsTest3;

	// Token: 0x04001875 RID: 6261
	public string[] calibrationPresetsTest4;

	// Token: 0x04001876 RID: 6262
	public string outputstring;

	// Token: 0x04001877 RID: 6263
	private List<string> stringList = new List<string>();
}
