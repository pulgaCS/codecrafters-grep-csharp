using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern) {
    int inputIndex = 0;
    int patternIndex = 0;

    while (inputIndex < inputLine.Length && patternIndex < pattern.Length) {
        if (pattern[patternIndex] == '\\') {
            patternIndex++;
            if (patternIndex >= pattern.Length) {
                throw new ArgumentException($"Invalid pattern: {pattern}");
            }
            if (pattern[patternIndex] == 'd') {
                if (!char.IsDigit(inputLine[inputIndex])) {
                    return false;
                }
            }
            else if (pattern[patternIndex] == 'w') {
                if (!char.IsLetterOrDigit(inputLine[inputIndex])) {
                    return false;
                }
            }
            else if (pattern[patternIndex] == 's') {
                if (!char.IsWhiteSpace(inputLine[inputIndex])) {
                    return false;
                }
            }
            else {
                throw new ArgumentException($"Invalid escape sequence: \\{pattern[patternIndex]}");
            }
        }
        else if (pattern[patternIndex] == '[') {
            bool invert = false;
            if (pattern[patternIndex + 1] == '^') {
                invert = true;
                patternIndex++;
            }
            patternIndex++;
            bool match = false;
            while (pattern[patternIndex] != ']') {
                if (pattern[patternIndex] == '\\') {
                    patternIndex++;
                    if (patternIndex >= pattern.Length) {
                        throw new ArgumentException($"Invalid pattern: {pattern}");
                    }
                    if (pattern[patternIndex] == inputLine[inputIndex]) {
                        match = true;
                        break;
                    }
                }
                else if (pattern[patternIndex] == inputLine[inputIndex]) {
                    match = true;
                    break;
                }
                patternIndex++;
            }
            if (invert) {
                match = !match;
            }
            if (!match) {
                return false;
            }
        }
        else {
            if (pattern[patternIndex] != inputLine[inputIndex]) {
                return false;
            }
        }
        inputIndex++;
        patternIndex++;
    }

    return inputIndex == inputLine.Length && patternIndex == pattern.Length;
}

if (args.Length < 2 || args[0] != "-E") {
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine;
using (StreamReader reader = new StreamReader(Console.OpenStandardInput())) {
    inputLine = reader.ReadLine();
}

if (MatchPattern(inputLine, pattern)) {
    Environment.Exit(0);
}
else {
    Environment.Exit(1);
}
