using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x02000955 RID: 2389
	public class JsonWriter
	{
		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06003A1D RID: 14877 RVA: 0x0010B734 File Offset: 0x00109934
		// (set) Token: 0x06003A1E RID: 14878 RVA: 0x0010B73C File Offset: 0x0010993C
		public int IndentValue
		{
			get
			{
				return this.indent_value;
			}
			set
			{
				this.indentation = this.indentation / this.indent_value * value;
				this.indent_value = value;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06003A1F RID: 14879 RVA: 0x0010B75A File Offset: 0x0010995A
		// (set) Token: 0x06003A20 RID: 14880 RVA: 0x0010B762 File Offset: 0x00109962
		public bool PrettyPrint
		{
			get
			{
				return this.pretty_print;
			}
			set
			{
				this.pretty_print = value;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06003A21 RID: 14881 RVA: 0x0010B76B File Offset: 0x0010996B
		public TextWriter TextWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06003A22 RID: 14882 RVA: 0x0010B773 File Offset: 0x00109973
		// (set) Token: 0x06003A23 RID: 14883 RVA: 0x0010B77B File Offset: 0x0010997B
		public bool Validate
		{
			get
			{
				return this.validate;
			}
			set
			{
				this.validate = value;
			}
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x0010B790 File Offset: 0x00109990
		public JsonWriter()
		{
			this.inst_string_builder = new StringBuilder();
			this.writer = new StringWriter(this.inst_string_builder);
			this.Init();
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x0010B7BA File Offset: 0x001099BA
		public JsonWriter(StringBuilder sb) : this(new StringWriter(sb))
		{
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x0010B7C8 File Offset: 0x001099C8
		public JsonWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.Init();
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x0010B7EC File Offset: 0x001099EC
		private void DoValidation(Condition cond)
		{
			if (!this.context.ExpectingValue)
			{
				this.context.Count++;
			}
			if (!this.validate)
			{
				return;
			}
			if (this.has_reached_end)
			{
				throw new JsonException("A complete JSON symbol has already been written");
			}
			switch (cond)
			{
			case Condition.InArray:
				if (!this.context.InArray)
				{
					throw new JsonException("Can't close an array here");
				}
				break;
			case Condition.InObject:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't close an object here");
				}
				break;
			case Condition.NotAProperty:
				if (this.context.InObject && !this.context.ExpectingValue)
				{
					throw new JsonException("Expected a property");
				}
				break;
			case Condition.Property:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't add a property here");
				}
				break;
			case Condition.Value:
				if (!this.context.InArray && (!this.context.InObject || !this.context.ExpectingValue))
				{
					throw new JsonException("Can't add a value here");
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x0010B910 File Offset: 0x00109B10
		private void Init()
		{
			this.has_reached_end = false;
			this.hex_seq = new char[4];
			this.indentation = 0;
			this.indent_value = 4;
			this.pretty_print = false;
			this.validate = true;
			this.ctx_stack = new Stack<WriterContext>();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x0010B974 File Offset: 0x00109B74
		private static void IntToHex(int n, char[] hex)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = n % 16;
				if (num < 10)
				{
					hex[3 - i] = (char)(48 + num);
				}
				else
				{
					hex[3 - i] = (char)(65 + (num - 10));
				}
				n >>= 4;
			}
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x0010B9B5 File Offset: 0x00109BB5
		private void Indent()
		{
			if (this.pretty_print)
			{
				this.indentation += this.indent_value;
			}
		}

		// Token: 0x06003A2C RID: 14892 RVA: 0x0010B9D4 File Offset: 0x00109BD4
		private void Put(string str)
		{
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				for (int i = 0; i < this.indentation; i++)
				{
					this.writer.Write(' ');
				}
			}
			this.writer.Write(str);
		}

		// Token: 0x06003A2D RID: 14893 RVA: 0x0010BA20 File Offset: 0x00109C20
		private void PutNewline()
		{
			this.PutNewline(true);
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x0010BA2C File Offset: 0x00109C2C
		private void PutNewline(bool add_comma)
		{
			if (add_comma && !this.context.ExpectingValue && this.context.Count > 1)
			{
				this.writer.Write(',');
			}
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				this.writer.Write('\n');
			}
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x0010BA88 File Offset: 0x00109C88
		private void PutString(string str)
		{
			this.Put(string.Empty);
			this.writer.Write('"');
			int length = str.Length;
			int i = 0;
			while (i < length)
			{
				char c = str[i];
				switch (c)
				{
				case '\b':
					this.writer.Write("\\b");
					break;
				case '\t':
					this.writer.Write("\\t");
					break;
				case '\n':
					this.writer.Write("\\n");
					break;
				case '\v':
					goto IL_E4;
				case '\f':
					this.writer.Write("\\f");
					break;
				case '\r':
					this.writer.Write("\\r");
					break;
				default:
					if (c != '"' && c != '\\')
					{
						goto IL_E4;
					}
					this.writer.Write('\\');
					this.writer.Write(str[i]);
					break;
				}
				IL_141:
				i++;
				continue;
				IL_E4:
				if (str[i] >= ' ' && str[i] <= '~')
				{
					this.writer.Write(str[i]);
					goto IL_141;
				}
				JsonWriter.IntToHex((int)str[i], this.hex_seq);
				this.writer.Write("\\u");
				this.writer.Write(this.hex_seq);
				goto IL_141;
			}
			this.writer.Write('"');
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x0010BBEE File Offset: 0x00109DEE
		private void Unindent()
		{
			if (this.pretty_print)
			{
				this.indentation -= this.indent_value;
			}
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x0010BC0B File Offset: 0x00109E0B
		public override string ToString()
		{
			if (this.inst_string_builder == null)
			{
				return string.Empty;
			}
			return this.inst_string_builder.ToString();
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x0010BC28 File Offset: 0x00109E28
		public void Reset()
		{
			this.has_reached_end = false;
			this.ctx_stack.Clear();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
			if (this.inst_string_builder != null)
			{
				this.inst_string_builder.Remove(0, this.inst_string_builder.Length);
			}
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x0010BC83 File Offset: 0x00109E83
		public void Write(bool boolean)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(boolean ? "true" : "false");
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x0010BCB3 File Offset: 0x00109EB3
		public void Write(decimal number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x0010BCE0 File Offset: 0x00109EE0
		public void Write(double number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string text = Convert.ToString(number, JsonWriter.number_format);
			this.Put(text);
			if (text.IndexOf('.') == -1 && text.IndexOf('E') == -1)
			{
				this.writer.Write(".0");
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x0010BD3F File Offset: 0x00109F3F
		public void Write(int number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x0010BD6B File Offset: 0x00109F6B
		public void Write(long number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x0010BD97 File Offset: 0x00109F97
		public void Write(string str)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			if (str == null)
			{
				this.Put("null");
			}
			else
			{
				this.PutString(str);
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x0010BDC9 File Offset: 0x00109FC9
		public void Write(ulong number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x0010BDF8 File Offset: 0x00109FF8
		public void WriteArrayEnd()
		{
			this.DoValidation(Condition.InArray);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("]");
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x0010BE64 File Offset: 0x0010A064
		public void WriteArrayStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("[");
			this.context = new WriterContext();
			this.context.InArray = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x0010BEB8 File Offset: 0x0010A0B8
		public void WriteObjectEnd()
		{
			this.DoValidation(Condition.InObject);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("}");
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x0010BF24 File Offset: 0x0010A124
		public void WriteObjectStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("{");
			this.context = new WriterContext();
			this.context.InObject = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x0010BF78 File Offset: 0x0010A178
		public void WritePropertyName(string property_name)
		{
			this.DoValidation(Condition.Property);
			this.PutNewline();
			this.PutString(property_name);
			if (this.pretty_print)
			{
				if (property_name.Length > this.context.Padding)
				{
					this.context.Padding = property_name.Length;
				}
				for (int i = this.context.Padding - property_name.Length; i >= 0; i--)
				{
					this.writer.Write(' ');
				}
				this.writer.Write(": ");
			}
			else
			{
				this.writer.Write(':');
			}
			this.context.ExpectingValue = true;
		}

		// Token: 0x04003B6E RID: 15214
		private static NumberFormatInfo number_format = NumberFormatInfo.InvariantInfo;

		// Token: 0x04003B6F RID: 15215
		private WriterContext context;

		// Token: 0x04003B70 RID: 15216
		private Stack<WriterContext> ctx_stack;

		// Token: 0x04003B71 RID: 15217
		private bool has_reached_end;

		// Token: 0x04003B72 RID: 15218
		private char[] hex_seq;

		// Token: 0x04003B73 RID: 15219
		private int indentation;

		// Token: 0x04003B74 RID: 15220
		private int indent_value;

		// Token: 0x04003B75 RID: 15221
		private StringBuilder inst_string_builder;

		// Token: 0x04003B76 RID: 15222
		private bool pretty_print;

		// Token: 0x04003B77 RID: 15223
		private bool validate;

		// Token: 0x04003B78 RID: 15224
		private TextWriter writer;
	}
}
