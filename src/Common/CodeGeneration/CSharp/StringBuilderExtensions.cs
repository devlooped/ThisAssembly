using System.Text;

namespace CodeGeneration.CSharp;

static class StringBuilderExtensions
{
    public static StringBuilder AppendQuoted(this StringBuilder @this, string? str)
    {
        if (str is null)
        {
            return @this.Append("null");
        }

        _ = @this.Append('@').Append('"');
        var length = str.Length;
        var pos = 0;
        while (pos < length)
        {
            if (str[pos] == '"')
            {
                _ = @this.Append('"').Append('"');
                pos++;
            }
            else
            {
                var doubleQuotePos = str.IndexOf('"', pos);
                if (doubleQuotePos < 0)
                {
                    _ = @this.Append(str, pos, length - pos);
                    pos = length;
                }
                else
                {
                    _ = @this.Append(str, pos, doubleQuotePos - pos);
                    pos = doubleQuotePos + 1;
                }
            }
        }

        return @this.Append('"');
    }
}