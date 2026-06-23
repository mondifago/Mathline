using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Parsing
{
    public static class Splitter
    {
        public static List<string> Split(string input)
        {
            var equations = new List<string>();

            if (input is null) return equations;

            foreach (string line in input.Split('\n'))
            {
                string trimmed = line.Trim();  
                if (trimmed.Length > 0)
                    equations.Add(trimmed);
            }

            return equations;
        }
    }
}
