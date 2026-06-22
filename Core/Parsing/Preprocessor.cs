using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Parsing
{
    public static class Preprocessor
    {
        private static readonly Dictionary<char, char> Superscripts = new()
        {
            ['⁰'] = '0',
            ['¹'] = '1',
            ['²'] = '2',
            ['³'] = '3',
            ['⁴'] = '4',
            ['⁵'] = '5',
            ['⁶'] = '6',
            ['⁷'] = '7',
            ['⁸'] = '8',
            ['⁹'] = '9'
        };

        public static string Canonicalize(string input)
        {
            var sb = new StringBuilder(input.Length);
            bool inSuperscript = false;

            foreach (char c in input)
            {
                if (Superscripts.TryGetValue(c, out char digit))
                {
                    if (!inSuperscript)
                    {
                        sb.Append('^');         
                        inSuperscript = true;
                    }
                    sb.Append(digit);
                    continue;
                }

                inSuperscript = false;

                switch (c)
                {
                    case '×': sb.Append('*'); break;
                    case '−':                        // U+2212 minus sign
                    case '–':                        // en dash
                    case '—': sb.Append('-'); break; // em dash
                    case '÷': sb.Append('/'); break; 
                    default: sb.Append(c); break;
                }
            }

            return sb.ToString();
        }
    }
}
