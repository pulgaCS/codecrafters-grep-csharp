using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern) {
    int inputIndex = 0, patternIndex = 0;
    while (inputIndex < inputLine.Length && patternIndex < pattern.Length) {
        if (pattern[patternIndex] == '\\') {
            // Handle escape sequences
            patternIndex++;
            if (patternIndex >= pattern.Length) return false; // Pattern ends with a backslash
            switch (pattern[patternIndex]) {
                case 'd':
                    if (!char.IsDigit(inputLine[inputIndex])) return false;
                    break;
                case 'w':
                    if (!char.IsLetterOrDigit(inputLine[inputIndex])) return false;
                    break;
                default:
                    // Handle other escape sequences if needed
                    return false;
            }
        }
        else if (pattern[patternIndex] == '[') {
            // Handle character classes
            bool match = false;
            bool negate = patternIndex + 1 < pattern.Length && pattern[patternIndex + 1] == '^';
            int charClassStart = negate ? patternIndex + 2 : patternIndex + 1;
            int charClassEnd = pattern.IndexOf(']', charClassStart);
            if (charClassEnd == -1) return false; // No closing bracket found
            string charClass = pattern.Substring(charClassStart, charClassEnd - charClassStart);
            if (negate) {
                match = !charClass.Contains(inputLine[inputIndex]);
            }
            else {
                match = charClass.Contains(inputLine[inputIndex]);
            }
            patternIndex = charClassEnd + 1;
        }
        else {
            // Handle literal characters
            if (pattern[patternIndex] != inputLine[inputIndex]) return false;
        }
        inputIndex++;
        patternIndex++;
    }
    // Check if entire pattern was matched
    return inputIndex == inputLine.Length && patternIndex == pattern.Length;
}


if (args.Length < 2 || args[0] != "-E") {
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadToEnd();

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

if (MatchPattern(inputLine, pattern)) {
    Environment.Exit(0);
}
else {
    Environment.Exit(1);
}