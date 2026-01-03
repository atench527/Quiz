using System;
using System.Collections.Generic;
using System.Text;

namespace QuizApplication
{
    internal static class CsvUtil
    {
        public static string Escape(string value)
        {
            if (value == null) return "";
            bool mustQuote = value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r");
            if (!mustQuote) return value;

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        // Very small CSV parser for a single line (handles quotes)
        public static List<string> SplitLine(string line)
        {
            var result = new List<string>();
            if (line == null) return result;

            var sb = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        // Escaped quote
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            sb.Append('"');
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                else
                {
                    if (c == ',')
                    {
                        result.Add(sb.ToString());
                        sb.Clear();
                    }
                    else if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }

            result.Add(sb.ToString());
            return result;
        }

        public static string JoinPipe(List<string> parts)
        {
            if (parts == null || parts.Count == 0) return "";
            // escape pipe and backslash in a minimal way
            // \| becomes literal |
            // \\ becomes literal \
            var safe = new List<string>();
            foreach (var p in parts)
            {
                var s = p ?? "";
                s = s.Replace("\\", "\\\\").Replace("|", "\\|");
                safe.Add(s);
            }
            return string.Join("|", safe);
        }

        public static List<string> SplitPipe(string text)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(text)) return result;

            var sb = new StringBuilder();
            bool escaping = false;

            foreach (char c in text)
            {
                if (escaping)
                {
                    sb.Append(c);
                    escaping = false;
                }
                else
                {
                    if (c == '\\') escaping = true;
                    else if (c == '|')
                    {
                        result.Add(sb.ToString());
                        sb.Clear();
                    }
                    else sb.Append(c);
                }
            }
            result.Add(sb.ToString());
            return result;
        }
    }
}
