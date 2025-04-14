using System;
using UnityEngine;

// Token: 0x020008A6 RID: 2214
[Serializable]
public class SceneObject : IEquatable<SceneObject>
{
	// Token: 0x060035AD RID: 13741 RVA: 0x000FE809 File Offset: 0x000FCA09
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

	// Token: 0x060035AE RID: 13742 RVA: 0x000FE843 File Offset: 0x000FCA43
	public SceneObject(int classID, ulong fileID)
	{
		this.classID = classID;
		this.fileID = fileID;
		this.typeString = UnityYaml.ClassIDToType[classID].AssemblyQualifiedName;
	}

	// Token: 0x060035AF RID: 13743 RVA: 0x000FE86F File Offset: 0x000FCA6F
	public bool Equals(SceneObject other)
	{
		return this.fileID == other.fileID && this.classID == other.classID;
	}

	// Token: 0x060035B0 RID: 13744 RVA: 0x000FE890 File Offset: 0x000FCA90
	public override bool Equals(object obj)
	{
		SceneObject sceneObject = obj as SceneObject;
		return sceneObject != null && this.Equals(sceneObject);
	}

	// Token: 0x060035B1 RID: 13745 RVA: 0x000FE8B0 File Offset: 0x000FCAB0
	public override int GetHashCode()
	{
		int i = this.classID;
		int i2 = StaticHash.Compute((long)this.fileID);
		return StaticHash.Compute(i, i2);
	}

	// Token: 0x060035B2 RID: 13746 RVA: 0x000FE8D5 File Offset: 0x000FCAD5
	public static bool operator ==(SceneObject x, SceneObject y)
	{
		return x.Equals(y);
	}

	// Token: 0x060035B3 RID: 13747 RVA: 0x000FE8DE File Offset: 0x000FCADE
	public static bool operator !=(SceneObject x, SceneObject y)
	{
		return !x.Equals(y);
	}

	// Token: 0x040037CE RID: 14286
	public int classID;

	// Token: 0x040037CF RID: 14287
	public ulong fileID;

	// Token: 0x040037D0 RID: 14288
	[SerializeField]
	public string typeString;

	// Token: 0x040037D1 RID: 14289
	public string json;
}
