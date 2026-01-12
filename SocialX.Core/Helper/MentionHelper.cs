using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SocialX.Core.Helper
{
    public static class MentionHelper
    {
        
        private static readonly Regex MentionRegex = new Regex(
            @"\B@([a-zA-Z0-9_]{1,15})\b",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static List<string> ExtractUsernames(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return new List<string>();

            return MentionRegex.Matches(content)
                .Select(m => m.Groups[1].Value)
                .Distinct(StringComparer.OrdinalIgnoreCase) 
                .ToList();
        }
    }
}