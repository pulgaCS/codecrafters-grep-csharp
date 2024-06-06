using System;

static bool MatchPattern(string inputLine, string pattern) {
    int inputIndex = 0;
    int patternIndex = 0;

    while (patternIndex < pattern.Length) {
        if (pattern[patternIndex] == '\') {
            if (patternIndex + 1 >= pattern.Length) {
                throw new ArgumentException($"Invalid escape sequence at end of pattern: {pattern}");
            }

            char nextPatternChar = pattern[patternIndex + 1];

            if (nextPatternChar == 'd') {
                if (inputIndex >= inputLine.Length || !char.IsDigit(inputLine[inputIndex])) {
                    return false;
                }
                inputIndex++;
                patternIndex += 2;
            }
            else if (nextPatternChar == 'w') {
                if (inputIndex >= inputLine.Length || !char.IsLetterOrDigit(inputLine[inputIndex])) {
                    return false;
                }
                inputIndex++;
                patternIndex += 2;
            }
            else {
                throw new ArgumentException($"Unhandled escape sequence: \\{nextPatternChar}");
            }
        }
        else if (pattern[patternIndex] == '[') {
            int closingBracket = pattern.IndexOf(']', patternIndex);
            if (closingBracket == -1) {
                throw new ArgumentException($"Unclosed character class in pattern: {pattern}");
            }

            string charClass = pattern.Substring(patternIndex + 1, closingBracket - patternIndex - 1);
            bool isNegated = charClass.StartsWith("^");
            string charSet = isNegated ? charClass.Substring(1) : charClass;

            if (inputIndex >= inputLine.Length ||
                isNegated == charSet.Contains(inputLine[inputIndex])) {
                return false;
            }
            inputIndex++;
            patternIndex = closingBracket + 1;
        }
        else {
            if (inputIndex >= inputLine.Length || inputLine[inputIndex] != pattern[patternIndex]) {
                return false;
            }
            inputIndex++;
            patternIndex++;
        }
    }

    // Ensure all characters in the pattern and input line are consumed
    return patternIndex == pattern.Length && (inputIndex == inputLine.Length || char.IsWhiteSpace(inputLine[inputIndex]));
}

if (args.Length < 2 || args[0] != "-E") {
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadToEnd().Trim();

// Debug log
Console.WriteLine("Logs from your program will appear here!");

if (MatchPattern(inputLine, pattern)) {
    Environment.Exit(0);
}
else {
    Environment.Exit(1);
}
