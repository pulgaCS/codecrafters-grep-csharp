using System;
using System.IO;
using System.Collections.Generic;

static bool MatchPattern(string inputLine, string pattern) {
    int patternIndex = 0;
    int inputIndex = 0;
    while (patternIndex < pattern.Length && inputIndex < inputLine.Length) {
        if (pattern[patternIndex] == '\\') {
            patternIndex++;
            if (patternIndex >= pattern.Length) {
                throw new ArgumentException($"Invalid escape sequence in pattern: {pattern}");
            }

            char escChar = pattern[patternIndex];
            switch (escChar) {
                case 'd':
                if (!char.IsDigit(inputLine[inputIndex])) {
                    return false;
                }
                break;
                case 'w':
                if (!char.IsLetterOrDigit(inputLine[inputIndex])) {
                    return false;
                }
                break;
                default:
                throw new ArgumentException($"Unhandled escape sequence: \\{escChar}");
            }
        }
        else if (pattern[patternIndex] == '[') {
            bool negated = (pattern[patternIndex + 1] == '^');
            int start = patternIndex + (negated ? 2 : 1);
            int end = pattern.IndexOf(']', start);
            if (end == -1) {
                throw new ArgumentException($"Unmatched '[' in pattern: {pattern}");
            }
            string charClass = pattern.Substring(start, end - start);
            bool matchFound = charClass.Contains(inputLine[inputIndex]);
            if (negated) matchFound = !matchFound;
            if (!matchFound) return false;
            patternIndex = end;
        }
        else {
            if (pattern[patternIndex] != inputLine[inputIndex]) {
                return false;
            }
        }
        patternIndex++;
        inputIndex++;
    }
    return patternIndex == pattern.Length;
}

if (args.Length < 2 || args[0] != "-E") {
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadToEnd().Trim();

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

if (MatchPattern(inputLine, pattern)) {
    Environment.Exit(0);
}
else {
    Environment.Exit(1);
}
