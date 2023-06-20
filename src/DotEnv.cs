using System.Text.RegularExpressions;

namespace AlphaGuardian01
{
    public static class DotEnv
    {
        public static void Load(string content)
        {
            Regex pattern = new Regex(@"(.*?)=(.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            String[] lines = content.Split('\n');

            foreach (var line in lines)
            {
                var parts = pattern.Match(line).Groups;
                /*line.Split(
                '=',
                (char)StringSplitOptions.RemoveEmptyEntries);*/

                if (parts.Count != 3)
                    continue;

                Environment.SetEnvironmentVariable(parts[1].Value.Trim(), parts[2].Value.Trim());
            }
        }

    }
}
