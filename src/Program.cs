using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern) {
    int patternIndex = 0;
    int inputIndex = 0;

    while (patternIndex < pattern.Length && inputIndex < inputLine.Length) {
        char currentPatternChar = pattern[patternIndex];

        if (currentPatternChar == '\\') {
            patternIndex++;
            if (patternIndex >= pattern.Length) {
                throw new ArgumentException($"Invalid escape sequence in pattern: {pattern}");
            }
            currentPatternChar = pattern[patternIndex];

            switch (currentPatternChar) {
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
                throw new ArgumentException($"Unhandled escape sequence: \\{currentPatternChar}");
            }
        }
        else if (currentPatternChar == '[') {
            patternIndex++;
            bool negate = pattern[patternIndex] == '^';
            if (negate) {
                patternIndex++;
            }

            int closingBracketIndex = pattern.IndexOf(']', patternIndex);
            if (closingBracketIndex == -1) {
                throw new ArgumentException($"Unmatched '[' in pattern: {pattern}");
            }

            string characterClass = pattern.Substring(patternIndex, closingBracketIndex - patternIndex);
            patternIndex = closingBracketIndex;

            bool matchFound = characterClass.Contains(inputLine[inputIndex]);
            if (negate) {
                matchFound = !matchFound;
            }

            if (!matchFound) {
                return false;
            }
        }
        else {
            if (inputLine[inputIndex] != currentPatternChar) {
                return false;
            }
        }

        patternIndex++;
        inputIndex++;
    }

    // Ensure the whole pattern was matched
    return patternIndex == pattern.Length;
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
