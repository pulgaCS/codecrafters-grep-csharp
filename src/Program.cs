using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern) {
    for (int i = 0; i <= inputLine.Length - pattern.Length; i++) {
        if (MatchFromIndex(inputLine, pattern, i)) {
            return true;
        }
    }
    return false;
}

static bool MatchFromIndex(string inputLine, string pattern, int index) {
    int inputIndex = index;
    int patternIndex = 0;

    while (patternIndex < pattern.Length && inputIndex < inputLine.Length) {
        char patternChar = pattern[patternIndex];

        if (patternChar == '\\') {
            patternIndex++;
            if (patternIndex >= pattern.Length) {
                return false;
            }
            char escChar = pattern[patternIndex];
            if (!MatchEscapedCharacter(inputLine, ref inputIndex, escChar)) {
                return false;
            }
        }
        else if (patternChar == '[') {
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
            if (inputLine[inputIndex] != patternChar) {
                return false;
            }
        }

        patternIndex++;
        inputIndex++;
    }

    return patternIndex == pattern.Length;
}

static bool MatchEscapedCharacter(string inputLine, ref int inputIndex, char escChar) {
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
    inputIndex++;
    return true;
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
