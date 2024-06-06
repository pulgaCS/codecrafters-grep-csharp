using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern) {
    int inputPos = 0;
    int patternPos = 0;

    while (patternPos < pattern.Length && inputPos < inputLine.Length) {
        char patternChar = pattern[patternPos];

        if (patternChar == '\\') {
            patternPos++;
            if (patternPos >= pattern.Length) throw new ArgumentException($"Unhandled pattern: {pattern}");
            patternChar = pattern[patternPos];

            switch (patternChar) {
                case 'd':
                if (!char.IsDigit(inputLine[inputPos])) return false;
                break;
                case 'w':
                if (!char.IsLetterOrDigit(inputLine[inputPos])) return false;
                break;
                default:
                throw new ArgumentException($"Unhandled pattern: \\{patternChar}");
            }
        }
        else if (patternChar == '[') {
            int closingBracket = pattern.IndexOf(']', patternPos);
            if (closingBracket == -1) throw new ArgumentException($"Unhandled pattern: {pattern}");

            string charClass = pattern.Substring(patternPos + 1, closingBracket - patternPos - 1);
            bool negate = charClass.StartsWith("^");
            if (negate) charClass = charClass.Substring(1);

            bool match = charClass.Contains(inputLine[inputPos]);
            if (negate) match = !match;
            if (!match) return false;

            patternPos = closingBracket;
        }
        else {
            if (patternChar != inputLine[inputPos]) return false;
        }

        patternPos++;
        inputPos++;
    }

    return patternPos == pattern.Length && inputPos <= inputLine.Length;
}

if (args.Length < 2 || args[0] != "-E") {
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadToEnd().Trim(); // Trim to remove any extra newlines

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

if (MatchPattern(inputLine, pattern)) {
    Environment.Exit(0);
}
else {
    Environment.Exit(1);
}
