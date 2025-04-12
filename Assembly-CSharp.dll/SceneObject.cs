using System;
using UnityEngine;

// Token: 0x020008A9 RID: 2217
[Serializable]
public class SceneObject : IEquatable<SceneObject>
{
	// Token: 0x060035B9 RID: 13753 RVA: 0x000528D1 File Offset: 0x00050AD1
	public Type GetObjectType()
	{
		if (string.IsNullOrWhiteSpace(this.typeString))
		{
			return null;
		}
		if (this.typeString.Contains("ProxyType"))
		{
			return ProxyType.Parse(this.typeString);
		}
		return Type.GetType(this.typeString);
	}

	// Token: 0x060035BA RID: 13754 RVA: 0x0005290B File Offset: 0x00050B0B
	public SceneObject(int classID, ulong fileID)
	{
		this.classID = classID;
		this.fileID = fileID;
		this.typeString = UnityYaml.ClassIDToType[classID].AssemblyQualifiedName;
	}

	// Token: 0x060035BB RID: 13755 RVA: 0x00052937 File Offset: 0x00050B37
	public bool Equals(SceneObject other)
	{
		return this.fileID == other.fileID && this.classID == other.classID;
	}

	// Token: 0x060035BC RID: 13756 RVA: 0x0013F1A8 File Offset: 0x0013D3A8
	public override bool Equals(object obj)
	{
		SceneObject sceneObject = obj as SceneObject;
		return sceneObject != null && this.Equals(sceneObject);
	}

	// Token: 0x060035BD RID: 13757 RVA: 0x0013F1C8 File Offset: 0x0013D3C8
	public override int GetHashCode()
	{
		int i = this.classID;
		int i2 = StaticHash.Compute((long)this.fileID);
		return StaticHash.Compute(i, i2);
	}

	// Token: 0x060035BE RID: 13758 RVA: 0x00052957 File Offset: 0x00050B57
	public static bool operator ==(SceneObject x, SceneObject y)
	{
		return x.Equals(y);
	}

	// Token: 0x060035BF RID: 13759 RVA: 0x00052960 File Offset: 0x00050B60
	public static bool operator !=(SceneObject x, SceneObject y)
	{
		return !x.Equals(y);
	}

	// Token: 0x040037E0 RID: 14304
	public int classID;

	// Token: 0x040037E1 RID: 14305
	public ulong fileID;

	// Token: 0x040037E2 RID: 14306
	[SerializeField]
	public string typeString;

	// Token: 0x040037E3 RID: 14307
	public string json;
}
