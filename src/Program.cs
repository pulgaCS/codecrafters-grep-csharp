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
                return false;
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
                return false;
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
                return false;
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

    return patternIndex == pattern.Length;
}

if (args.Length < 2 || args[0] != "-E") {
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadToEnd().Trim();

Console.WriteLine("Logs from your program will appear here!");

if (MatchPattern(inputLine, pattern)) {
    Environment.Exit(0);
}
else {
    Environment.Exit(1);
}
