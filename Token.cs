using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rubydotnet;

public class Token : ICloneable
{
    public string Type;
    public string Value;
    public int Index;
    public int Length;
    public bool IsKeyword;

    public Token(string Type, string Value, int Index, int Length, bool IsKeyword = false)
    {
        this.Type = Type;
        this.Value = Value;
        this.Index = Index;
        this.Length = Length;
        this.IsKeyword = IsKeyword;
    }

    public object Clone()
    {
        return new Token(this.Type, this.Value, this.Index, this.Length, IsKeyword);
    }
}
