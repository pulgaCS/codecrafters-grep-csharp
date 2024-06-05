using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern) {
    int inputIndex = 0;
    int patternIndex = 0;

    while (inputIndex < inputLine.Length && patternIndex < pattern.Length) {
        char currentChar = inputLine[inputIndex];
        char currentPatternChar = pattern[patternIndex];

        if (currentPatternChar == '\\') {
            // Handle escaped characters like \d, \w, etc.
            patternIndex++;
            if (patternIndex < pattern.Length) {
                char escapedChar = pattern[patternIndex];
                if (escapedChar == 'd') {
                    if (!char.IsDigit(currentChar))
                        return false;
                }
                else if (escapedChar == 'w') {
                    if (!char.IsLetterOrDigit(currentChar))
                        return false;
                }
                else if (currentChar != escapedChar) {
                    return false;
                }
            }
        }
        else if (currentPatternChar == ' ') {
            // Skip whitespace in the pattern
            patternIndex++;
            continue;
        }
        else if (currentPatternChar == inputLine[inputIndex]) {
            // Match character directly
            inputIndex++;
        }
        else if (currentPatternChar == '.' && patternIndex + 1 < pattern.Length && pattern[patternIndex + 1] == '*') {
            // Handle .* pattern
            patternIndex += 2;
            if (patternIndex == pattern.Length) return true; // .* matches anything
            char nextChar = pattern[patternIndex];
            while (inputIndex < inputLine.Length && inputLine[inputIndex] != nextChar)
                inputIndex++;
        }
        else if (patternIndex + 1 < pattern.Length && pattern[patternIndex + 1] == '*') {
            // Handle character class followed by *
            char classChar = pattern[patternIndex];
            patternIndex += 2;
            while (inputIndex < inputLine.Length && inputLine[inputIndex] == classChar)
                inputIndex++;
        }
        else {
            return false;
        }

        patternIndex++;
    }

    // If the pattern is finished, but there's still input left, return false
    return patternIndex == pattern.Length && inputIndex == inputLine.Length;
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
