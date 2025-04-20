﻿using System;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x02000261 RID: 609
public static class CustomSerializer
{
	// Token: 0x06000E1C RID: 3612 RVA: 0x0003A1E8 File Offset: 0x000383E8
	public static byte[] ByteSerialize(this object obj)
	{
		return CustomSerializer.Serialize(obj);
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x0003A1F0 File Offset: 0x000383F0
	public static object ByteDeserialize(this byte[] bytes)
	{
		return CustomSerializer.Deserialize(bytes);
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x000A3240 File Offset: 0x000A1440
	public static byte[] Serialize(object obj)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8))
			{
				CustomSerializer.SerializeObject(binaryWriter, obj);
				result = memoryStream.ToArray();
			}
		}
		return result;
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x000A32A0 File Offset: 0x000A14A0
	public static object Deserialize(byte[] data)
	{
		object result;
		using (MemoryStream memoryStream = new MemoryStream(data))
		{
			using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
			{
				result = CustomSerializer.DeserializeObject(binaryReader);
			}
		}
		return result;
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x000A32FC File Offset: 0x000A14FC
	private static void SerializeObject(BinaryWriter writer, object obj)
	{
		string text = obj as string;
		if (text != null)
		{
			writer.Write(1);
			writer.Write(text);
			return;
		}
		if (obj is bool)
		{
			bool value = (bool)obj;
			writer.Write(2);
			writer.Write(value);
			return;
		}
		if (obj is int)
		{
			int value2 = (int)obj;
			writer.Write(3);
			writer.Write(value2);
			return;
		}
		if (obj is float)
		{
			float value3 = (float)obj;
			writer.Write(4);
			writer.Write(value3);
			return;
		}
		if (obj is double)
		{
			double value4 = (double)obj;
			writer.Write(5);
			writer.Write(value4);
			return;
		}
		if (obj is Vector2)
		{
			Vector2 vector = (Vector2)obj;
			writer.Write(6);
			writer.Write(vector.x);
			writer.Write(vector.y);
			return;
		}
		if (obj is Vector3)
		{
			Vector3 vector2 = (Vector3)obj;
			writer.Write(7);
			writer.Write(vector2.x);
			writer.Write(vector2.y);
			writer.Write(vector2.z);
			return;
		}
		object[] array = obj as object[];
		if (array != null)
		{
			writer.Write(8);
			CustomSerializer.SerializeObjectArray(writer, array);
			return;
		}
		if (obj is byte)
		{
			byte value5 = (byte)obj;
			writer.Write(9);
			writer.Write(value5);
			return;
		}
		Enum @enum = obj as Enum;
		if (@enum != null)
		{
			writer.Write(10);
			writer.Write(Convert.ToInt32(@enum));
			writer.Write(@enum.GetType().AssemblyQualifiedName);
			return;
		}
		NetEventOptions netEventOptions = obj as NetEventOptions;
		if (netEventOptions != null)
		{
			writer.Write(11);
			CustomSerializer.SerializeNetEventOptions(writer, netEventOptions);
			return;
		}
		if (obj is Quaternion)
		{
			Quaternion quaternion = (Quaternion)obj;
			writer.Write(12);
			writer.Write(quaternion.x);
			writer.Write(quaternion.y);
			writer.Write(quaternion.z);
			writer.Write(quaternion.w);
			return;
		}
		Debug.LogWarning("<color=blue>type not supported " + obj.GetType().ToString() + "</color>");
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x000A3540 File Offset: 0x000A1740
	private static void SerializeObjectArray(BinaryWriter writer, object[] objects)
	{
		writer.Write(objects.Length);
		foreach (object obj in objects)
		{
			CustomSerializer.SerializeObject(writer, obj);
		}
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x000A3574 File Offset: 0x000A1774
	private static void SerializeNetEventOptions(BinaryWriter writer, NetEventOptions options)
	{
		writer.Write((int)options.Reciever);
		if (options.TargetActors == null)
		{
			writer.Write(0);
		}
		else
		{
			writer.Write(options.TargetActors.Length);
			foreach (int value in options.TargetActors)
			{
				writer.Write(value);
			}
		}
		writer.Write(options.Flags.WebhookFlags);
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x000A35E0 File Offset: 0x000A17E0
	private static object DeserializeObject(BinaryReader reader)
	{
		switch (reader.ReadByte())
		{
		case 0:
			return null;
		case 1:
			return reader.ReadString();
		case 2:
			return reader.ReadBoolean();
		case 3:
			return reader.ReadInt32();
		case 4:
			return reader.ReadSingle();
		case 5:
			return reader.ReadDouble();
		case 6:
		{
			float x = reader.ReadSingle();
			float y = reader.ReadSingle();
			return new Vector2(x, y);
		}
		case 7:
		{
			float x2 = reader.ReadSingle();
			float y2 = reader.ReadSingle();
			float z = reader.ReadSingle();
			return new Vector3(x2, y2, z);
		}
		case 8:
			return CustomSerializer.DeserializeObjectArray(reader);
		case 9:
			return reader.ReadByte();
		case 10:
		{
			int value = reader.ReadInt32();
			return Enum.ToObject(Type.GetType(reader.ReadString()), value);
		}
		case 11:
			return CustomSerializer.DeserializeNetEventOptions(reader);
		case 12:
		{
			float x3 = reader.ReadSingle();
			float y3 = reader.ReadSingle();
			float z2 = reader.ReadSingle();
			float w = reader.ReadSingle();
			return new Quaternion(x3, y3, z2, w);
		}
		default:
			throw new InvalidOperationException("Unsupported type");
		}
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x000A3714 File Offset: 0x000A1914
	private static object[] DeserializeObjectArray(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		object[] array = new object[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = CustomSerializer.DeserializeObject(reader);
		}
		return array;
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x000A3748 File Offset: 0x000A1948
	private static NetEventOptions DeserializeNetEventOptions(BinaryReader reader)
	{
		int reciever = reader.ReadInt32();
		int num = reader.ReadInt32();
		int[] array = null;
		if (num > 0)
		{
			array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadInt32();
			}
		}
		byte flags = reader.ReadByte();
		return new NetEventOptions(reciever, array, flags);
	}
}
