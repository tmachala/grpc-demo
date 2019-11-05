using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GrpcChat.Server.Helpers
{
    public static class MentionHelpers
    {
        public static IEnumerable<string> Parse(string message)
        {
            var matches = Regex.Matches(message, Pattern);

            // Trim the leading '@'
            return matches.Select(m => m.Value.Substring(1));
        }

        // Allows for user names in this shape: @alice, @bob-99 etc.
        // Supported special characters: -_.#/\
        private const string Pattern = @"\@[\w|-|_|\.|#|/|\\]+";
    }
}
