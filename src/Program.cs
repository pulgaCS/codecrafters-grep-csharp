using System;

static bool MatchPattern(string inputLine, string pattern) {
    int inputIndex = 0;
    int patternIndex = 0;

    while (inputIndex < inputLine.Length && patternIndex < pattern.Length) {
        char pChar = pattern[patternIndex];

        if (pChar == '\\') {
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
        else if (pChar == '[') {
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
            if (inputIndex >= inputLine.Length || inputLine[inputIndex] != pChar) {
                return false;
            }
            inputIndex++;
            patternIndex++;
        }
    }

    return inputIndex == inputLine.Length && patternIndex == pattern.Length;
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
