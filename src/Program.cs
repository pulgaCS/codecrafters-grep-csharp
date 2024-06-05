using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern) {
    int inputIndex = 0, patternIndex = 0;
    while (inputIndex < inputLine.Length && patternIndex < pattern.Length) {
        switch (pattern[patternIndex]) {
            case '\\':
                patternIndex++;
                if (pattern[patternIndex] == 'd' && char.IsDigit(inputLine[inputIndex])) {
                    patternIndex++;
                    inputIndex++;
                }
                else if (pattern[patternIndex] == 'w' && char.IsLetterOrDigit(inputLine[inputIndex])) {
                    patternIndex++;
                    inputIndex++;
                }
                else {
                    return false;
                }
                break;
            case '[':
                bool match = false;
                patternIndex++;
                bool negate = pattern[patternIndex] == '^';
                if (negate) patternIndex++;
                while (pattern[patternIndex] != ']') {
                    if ((negate && inputLine[inputIndex] != pattern[patternIndex]) ||
                        (!negate && inputLine[inputIndex] == pattern[patternIndex])) {
                        match = true;
                        break;
                    }
                    patternIndex++;
                }
                if (!match) return false;
                while (pattern[patternIndex] != ']') patternIndex++;
                patternIndex++;
                inputIndex++;
                break;
            default:
                if (inputLine[inputIndex] == pattern[patternIndex]) {
                    inputIndex++;
                    patternIndex++;
                }
                else {
                    return false;
                }
                break;
        }
    }
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