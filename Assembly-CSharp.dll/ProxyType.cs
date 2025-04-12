using System;
using System.Globalization;
using System.Reflection;

// Token: 0x020008A8 RID: 2216
public class ProxyType : Type
{
	// Token: 0x06003590 RID: 13712 RVA: 0x00052703 File Offset: 0x00050903
	public ProxyType()
	{
	}

	// Token: 0x06003591 RID: 13713 RVA: 0x0005271B File Offset: 0x0005091B
	public ProxyType(string typeName)
	{
		this._typeName = typeName;
	}

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06003592 RID: 13714 RVA: 0x0005273A File Offset: 0x0005093A
	public override string Name
	{
		get
		{
			return this._typeName;
		}
	}

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06003593 RID: 13715 RVA: 0x00052742 File Offset: 0x00050942
	public override string FullName
	{
		get
		{
			return ProxyType.kPrefix + this._typeName;
		}
	}

	// Token: 0x06003594 RID: 13716 RVA: 0x0013F0F4 File Offset: 0x0013D2F4
	public static ProxyType Parse(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			throw new ArgumentNullException("input");
		}
		input = input.Trim();
		if (!input.Contains(ProxyType.kPrefix, StringComparison.InvariantCultureIgnoreCase))
		{
			return ProxyType.kInvalidType;
		}
		if (!input.StartsWith(ProxyType.kPrefix, StringComparison.InvariantCultureIgnoreCase))
		{
			return ProxyType.kInvalidType;
		}
		if (input.Contains(','))
		{
			input = input.Split(',', StringSplitOptions.None)[0];
		}
		string text = input.Split('.', StringSplitOptions.None)[1].Trim();
		if (string.IsNullOrWhiteSpace(text))
		{
			return ProxyType.kInvalidType;
		}
		return new ProxyType(text);
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x00052754 File Offset: 0x00050954
	public override string ToString()
	{
		return base.ToString() + "." + this._typeName;
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x0005276C File Offset: 0x0005096C
	public override object[] GetCustomAttributes(bool inherit)
	{
		return this._self.GetCustomAttributes(inherit);
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x0005277A File Offset: 0x0005097A
	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		return this._self.GetCustomAttributes(attributeType, inherit);
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x00052789 File Offset: 0x00050989
	public override bool IsDefined(Type attributeType, bool inherit)
	{
		return this._self.IsDefined(attributeType, inherit);
	}

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x06003599 RID: 13721 RVA: 0x00052798 File Offset: 0x00050998
	public override Module Module
	{
		get
		{
			return this._self.Module;
		}
	}

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x0600359A RID: 13722 RVA: 0x000527A5 File Offset: 0x000509A5
	public override string Namespace
	{
		get
		{
			return this._self.Namespace;
		}
	}

	// Token: 0x0600359B RID: 13723 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	protected override TypeAttributes GetAttributeFlagsImpl()
	{
		return TypeAttributes.NotPublic;
	}

	// Token: 0x0600359C RID: 13724 RVA: 0x00037F8B File Offset: 0x0003618B
	protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		return null;
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x000527B2 File Offset: 0x000509B2
	public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
	{
		return this._self.GetConstructors(bindingAttr);
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x000527C0 File Offset: 0x000509C0
	public override Type GetElementType()
	{
		return this._self.GetElementType();
	}

	// Token: 0x0600359F RID: 13727 RVA: 0x000527CD File Offset: 0x000509CD
	public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
	{
		return this._self.GetEvent(name, bindingAttr);
	}

	// Token: 0x060035A0 RID: 13728 RVA: 0x000527DC File Offset: 0x000509DC
	public override EventInfo[] GetEvents(BindingFlags bindingAttr)
	{
		return this._self.GetEvents(bindingAttr);
	}

	// Token: 0x060035A1 RID: 13729 RVA: 0x000527EA File Offset: 0x000509EA
	public override FieldInfo GetField(string name, BindingFlags bindingAttr)
	{
		return this._self.GetField(name, bindingAttr);
	}

	// Token: 0x060035A2 RID: 13730 RVA: 0x000527F9 File Offset: 0x000509F9
	public override FieldInfo[] GetFields(BindingFlags bindingAttr)
	{
		return this._self.GetFields(bindingAttr);
	}

	// Token: 0x060035A3 RID: 13731 RVA: 0x00052807 File Offset: 0x00050A07
	public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
	{
		return this._self.GetMembers(bindingAttr);
	}

	// Token: 0x060035A4 RID: 13732 RVA: 0x00037F8B File Offset: 0x0003618B
	protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		return null;
	}

	// Token: 0x060035A5 RID: 13733 RVA: 0x00052815 File Offset: 0x00050A15
	public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
	{
		return this._self.GetMethods(bindingAttr);
	}

	// Token: 0x060035A6 RID: 13734 RVA: 0x00052823 File Offset: 0x00050A23
	public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
	{
		return this._self.GetProperties(bindingAttr);
	}

	// Token: 0x060035A7 RID: 13735 RVA: 0x0013F180 File Offset: 0x0013D380
	public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
	{
		return this._self.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
	}

	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x060035A8 RID: 13736 RVA: 0x00052831 File Offset: 0x00050A31
	public override Type UnderlyingSystemType
	{
		get
		{
			return this._self.UnderlyingSystemType;
		}
	}

	// Token: 0x060035A9 RID: 13737 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	protected override bool IsArrayImpl()
	{
		return false;
	}

	// Token: 0x060035AA RID: 13738 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	protected override bool IsByRefImpl()
	{
		return false;
	}

	// Token: 0x060035AB RID: 13739 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	protected override bool IsCOMObjectImpl()
	{
		return false;
	}

	// Token: 0x060035AC RID: 13740 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	protected override bool IsPointerImpl()
	{
		return false;
	}

	// Token: 0x060035AD RID: 13741 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	protected override bool IsPrimitiveImpl()
	{
		return false;
	}

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x060035AE RID: 13742 RVA: 0x0005283E File Offset: 0x00050A3E
	public override Assembly Assembly
	{
		get
		{
			return this._self.Assembly;
		}
	}

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x060035AF RID: 13743 RVA: 0x0005284B File Offset: 0x00050A4B
	public override string AssemblyQualifiedName
	{
		get
		{
			return this._self.AssemblyQualifiedName.Replace("ProxyType", this.FullName);
		}
	}

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x060035B0 RID: 13744 RVA: 0x00052868 File Offset: 0x00050A68
	public override Type BaseType
	{
		get
		{
			return this._self.BaseType;
		}
	}

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x060035B1 RID: 13745 RVA: 0x00052875 File Offset: 0x00050A75
	public override Guid GUID
	{
		get
		{
			return this._self.GUID;
		}
	}

	// Token: 0x060035B2 RID: 13746 RVA: 0x00037F8B File Offset: 0x0003618B
	protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
	{
		return null;
	}

	// Token: 0x060035B3 RID: 13747 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	protected override bool HasElementTypeImpl()
	{
		return false;
	}

	// Token: 0x060035B4 RID: 13748 RVA: 0x00052882 File Offset: 0x00050A82
	public override Type GetNestedType(string name, BindingFlags bindingAttr)
	{
		return this._self.GetNestedType(name, bindingAttr);
	}

	// Token: 0x060035B5 RID: 13749 RVA: 0x00052891 File Offset: 0x00050A91
	public override Type[] GetNestedTypes(BindingFlags bindingAttr)
	{
		return this._self.GetNestedTypes(bindingAttr);
	}

	// Token: 0x060035B6 RID: 13750 RVA: 0x0005289F File Offset: 0x00050A9F
	public override Type GetInterface(string name, bool ignoreCase)
	{
		return this._self.GetInterface(name, ignoreCase);
	}

	// Token: 0x060035B7 RID: 13751 RVA: 0x000528AE File Offset: 0x00050AAE
	public override Type[] GetInterfaces()
	{
		return this._self.GetInterfaces();
	}

	// Token: 0x040037DC RID: 14300
	private Type _self = typeof(ProxyType);

	// Token: 0x040037DD RID: 14301
	private readonly string _typeName;

	// Token: 0x040037DE RID: 14302
	private static readonly string kPrefix = "ProxyType.";

	// Token: 0x040037DF RID: 14303
	private static InvalidType kInvalidType = new InvalidType();
}
