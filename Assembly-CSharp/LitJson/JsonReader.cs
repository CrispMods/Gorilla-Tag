using System;
using System.Collections.Generic;
using System.IO;

namespace LitJson
{
	// Token: 0x02000952 RID: 2386
	public class JsonReader
	{
		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06003A08 RID: 14856 RVA: 0x0010AEA4 File Offset: 0x001090A4
		// (set) Token: 0x06003A09 RID: 14857 RVA: 0x0010AEB1 File Offset: 0x001090B1
		public bool AllowComments
		{
			get
			{
				return this.lexer.AllowComments;
			}
			set
			{
				this.lexer.AllowComments = value;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06003A0A RID: 14858 RVA: 0x0010AEBF File Offset: 0x001090BF
		// (set) Token: 0x06003A0B RID: 14859 RVA: 0x0010AECC File Offset: 0x001090CC
		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.lexer.AllowSingleQuotedStrings;
			}
			set
			{
				this.lexer.AllowSingleQuotedStrings = value;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06003A0C RID: 14860 RVA: 0x0010AEDA File Offset: 0x001090DA
		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06003A0D RID: 14861 RVA: 0x0010AEE2 File Offset: 0x001090E2
		public bool EndOfJson
		{
			get
			{
				return this.end_of_json;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06003A0E RID: 14862 RVA: 0x0010AEEA File Offset: 0x001090EA
		public JsonToken Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06003A0F RID: 14863 RVA: 0x0010AEF2 File Offset: 0x001090F2
		public object Value
		{
			get
			{
				return this.token_value;
			}
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x0010AEFA File Offset: 0x001090FA
		static JsonReader()
		{
			JsonReader.PopulateParseTable();
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x0010AF01 File Offset: 0x00109101
		public JsonReader(string json_text) : this(new StringReader(json_text), true)
		{
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x0010AF10 File Offset: 0x00109110
		public JsonReader(TextReader reader) : this(reader, false)
		{
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x0010AF1C File Offset: 0x0010911C
		private JsonReader(TextReader reader, bool owned)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.read_started = false;
			this.automaton_stack = new Stack<int>();
			this.automaton_stack.Push(65553);
			this.automaton_stack.Push(65543);
			this.lexer = new Lexer(reader);
			this.end_of_input = false;
			this.end_of_json = false;
			this.reader = reader;
			this.reader_is_owned = owned;
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x0010AFA8 File Offset: 0x001091A8
		private static void PopulateParseTable()
		{
			JsonReader.parse_table = new Dictionary<int, IDictionary<int, int[]>>();
			JsonReader.TableAddRow(ParserToken.Array);
			JsonReader.TableAddCol(ParserToken.Array, 91, new int[]
			{
				91,
				65549
			});
			JsonReader.TableAddRow(ParserToken.ArrayPrime);
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 34, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 91, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 93, new int[]
			{
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 123, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65537, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65538, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65539, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65540, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddRow(ParserToken.Object);
			JsonReader.TableAddCol(ParserToken.Object, 123, new int[]
			{
				123,
				65545
			});
			JsonReader.TableAddRow(ParserToken.ObjectPrime);
			JsonReader.TableAddCol(ParserToken.ObjectPrime, 34, new int[]
			{
				65546,
				65547,
				125
			});
			JsonReader.TableAddCol(ParserToken.ObjectPrime, 125, new int[]
			{
				125
			});
			JsonReader.TableAddRow(ParserToken.Pair);
			JsonReader.TableAddCol(ParserToken.Pair, 34, new int[]
			{
				65552,
				58,
				65550
			});
			JsonReader.TableAddRow(ParserToken.PairRest);
			JsonReader.TableAddCol(ParserToken.PairRest, 44, new int[]
			{
				44,
				65546,
				65547
			});
			JsonReader.TableAddCol(ParserToken.PairRest, 125, new int[]
			{
				65554
			});
			JsonReader.TableAddRow(ParserToken.String);
			JsonReader.TableAddCol(ParserToken.String, 34, new int[]
			{
				34,
				65541,
				34
			});
			JsonReader.TableAddRow(ParserToken.Text);
			JsonReader.TableAddCol(ParserToken.Text, 91, new int[]
			{
				65548
			});
			JsonReader.TableAddCol(ParserToken.Text, 123, new int[]
			{
				65544
			});
			JsonReader.TableAddRow(ParserToken.Value);
			JsonReader.TableAddCol(ParserToken.Value, 34, new int[]
			{
				65552
			});
			JsonReader.TableAddCol(ParserToken.Value, 91, new int[]
			{
				65548
			});
			JsonReader.TableAddCol(ParserToken.Value, 123, new int[]
			{
				65544
			});
			JsonReader.TableAddCol(ParserToken.Value, 65537, new int[]
			{
				65537
			});
			JsonReader.TableAddCol(ParserToken.Value, 65538, new int[]
			{
				65538
			});
			JsonReader.TableAddCol(ParserToken.Value, 65539, new int[]
			{
				65539
			});
			JsonReader.TableAddCol(ParserToken.Value, 65540, new int[]
			{
				65540
			});
			JsonReader.TableAddRow(ParserToken.ValueRest);
			JsonReader.TableAddCol(ParserToken.ValueRest, 44, new int[]
			{
				44,
				65550,
				65551
			});
			JsonReader.TableAddCol(ParserToken.ValueRest, 93, new int[]
			{
				65554
			});
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x0010B321 File Offset: 0x00109521
		private static void TableAddCol(ParserToken row, int col, params int[] symbols)
		{
			JsonReader.parse_table[(int)row].Add(col, symbols);
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x0010B335 File Offset: 0x00109535
		private static void TableAddRow(ParserToken rule)
		{
			JsonReader.parse_table.Add((int)rule, new Dictionary<int, int[]>());
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x0010B348 File Offset: 0x00109548
		private void ProcessNumber(string number)
		{
			double num;
			if ((number.IndexOf('.') != -1 || number.IndexOf('e') != -1 || number.IndexOf('E') != -1) && double.TryParse(number, out num))
			{
				this.token = JsonToken.Double;
				this.token_value = num;
				return;
			}
			int num2;
			if (int.TryParse(number, out num2))
			{
				this.token = JsonToken.Int;
				this.token_value = num2;
				return;
			}
			long num3;
			if (long.TryParse(number, out num3))
			{
				this.token = JsonToken.Long;
				this.token_value = num3;
				return;
			}
			this.token = JsonToken.Int;
			this.token_value = 0;
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x0010B3E4 File Offset: 0x001095E4
		private void ProcessSymbol()
		{
			if (this.current_symbol == 91)
			{
				this.token = JsonToken.ArrayStart;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 93)
			{
				this.token = JsonToken.ArrayEnd;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 123)
			{
				this.token = JsonToken.ObjectStart;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 125)
			{
				this.token = JsonToken.ObjectEnd;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 34)
			{
				if (this.parser_in_string)
				{
					this.parser_in_string = false;
					this.parser_return = true;
					return;
				}
				if (this.token == JsonToken.None)
				{
					this.token = JsonToken.String;
				}
				this.parser_in_string = true;
				return;
			}
			else
			{
				if (this.current_symbol == 65541)
				{
					this.token_value = this.lexer.StringValue;
					return;
				}
				if (this.current_symbol == 65539)
				{
					this.token = JsonToken.Boolean;
					this.token_value = false;
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65540)
				{
					this.token = JsonToken.Null;
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65537)
				{
					this.ProcessNumber(this.lexer.StringValue);
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65546)
				{
					this.token = JsonToken.PropertyName;
					return;
				}
				if (this.current_symbol == 65538)
				{
					this.token = JsonToken.Boolean;
					this.token_value = true;
					this.parser_return = true;
				}
				return;
			}
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x0010B556 File Offset: 0x00109756
		private bool ReadToken()
		{
			if (this.end_of_input)
			{
				return false;
			}
			this.lexer.NextToken();
			if (this.lexer.EndOfInput)
			{
				this.Close();
				return false;
			}
			this.current_input = this.lexer.Token;
			return true;
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x0010B595 File Offset: 0x00109795
		public void Close()
		{
			if (this.end_of_input)
			{
				return;
			}
			this.end_of_input = true;
			this.end_of_json = true;
			if (this.reader_is_owned)
			{
				this.reader.Close();
			}
			this.reader = null;
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x0010B5C8 File Offset: 0x001097C8
		public bool Read()
		{
			if (this.end_of_input)
			{
				return false;
			}
			if (this.end_of_json)
			{
				this.end_of_json = false;
				this.automaton_stack.Clear();
				this.automaton_stack.Push(65553);
				this.automaton_stack.Push(65543);
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.token = JsonToken.None;
			this.token_value = null;
			if (!this.read_started)
			{
				this.read_started = true;
				if (!this.ReadToken())
				{
					return false;
				}
			}
			while (!this.parser_return)
			{
				this.current_symbol = this.automaton_stack.Pop();
				this.ProcessSymbol();
				if (this.current_symbol == this.current_input)
				{
					if (!this.ReadToken())
					{
						if (this.automaton_stack.Peek() != 65553)
						{
							throw new JsonException("Input doesn't evaluate to proper JSON text");
						}
						return this.parser_return;
					}
				}
				else
				{
					int[] array;
					try
					{
						array = JsonReader.parse_table[this.current_symbol][this.current_input];
					}
					catch (KeyNotFoundException inner_exception)
					{
						throw new JsonException((ParserToken)this.current_input, inner_exception);
					}
					if (array[0] != 65554)
					{
						for (int i = array.Length - 1; i >= 0; i--)
						{
							this.automaton_stack.Push(array[i]);
						}
					}
				}
			}
			if (this.automaton_stack.Peek() == 65553)
			{
				this.end_of_json = true;
			}
			return true;
		}

		// Token: 0x04003B55 RID: 15189
		private static IDictionary<int, IDictionary<int, int[]>> parse_table;

		// Token: 0x04003B56 RID: 15190
		private Stack<int> automaton_stack;

		// Token: 0x04003B57 RID: 15191
		private int current_input;

		// Token: 0x04003B58 RID: 15192
		private int current_symbol;

		// Token: 0x04003B59 RID: 15193
		private bool end_of_json;

		// Token: 0x04003B5A RID: 15194
		private bool end_of_input;

		// Token: 0x04003B5B RID: 15195
		private Lexer lexer;

		// Token: 0x04003B5C RID: 15196
		private bool parser_in_string;

		// Token: 0x04003B5D RID: 15197
		private bool parser_return;

		// Token: 0x04003B5E RID: 15198
		private bool read_started;

		// Token: 0x04003B5F RID: 15199
		private TextReader reader;

		// Token: 0x04003B60 RID: 15200
		private bool reader_is_owned;

		// Token: 0x04003B61 RID: 15201
		private object token_value;

		// Token: 0x04003B62 RID: 15202
		private JsonToken token;
	}
}
