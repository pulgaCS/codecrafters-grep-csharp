static bool MatchPattern(string inputLine, string pattern) {
    int inputIndex = 0;
    int patternIndex = 0;

    while (inputIndex < inputLine.Length && patternIndex < pattern.Length) {
        if (pattern[patternIndex] == '\\') {
            // Handling escape characters
            patternIndex++;
            if (patternIndex >= pattern.Length) {
                throw new ArgumentException("Invalid pattern: pattern ends with escape character");
            }
            if (inputLine[inputIndex] != pattern[patternIndex]) {
                return false;
            }
        }
        else if (pattern[patternIndex] == '.') {
            // Match any character
            inputIndex++;
        }
        else if (pattern[patternIndex] == '[') {
            bool isInverted = false;
            if (patternIndex + 1 < pattern.Length && pattern[patternIndex + 1] == '^') {
                isInverted = true;
                patternIndex++;
            }
            patternIndex++; // Move past '[' or '^'
            bool matched = false;
            while (pattern[patternIndex] != ']') {
                if (patternIndex >= pattern.Length) {
                    throw new ArgumentException("Invalid pattern: missing ']'");
                }
                char currentPatternChar = pattern[patternIndex];
                if (patternIndex + 1 < pattern.Length && pattern[patternIndex + 1] == '-') {
                    if (patternIndex + 2 >= pattern.Length) {
                        throw new ArgumentException("Invalid pattern: missing range end");
                    }
                    char rangeEnd = pattern[patternIndex + 2];
                    if (inputLine[inputIndex] >= currentPatternChar && inputLine[inputIndex] <= rangeEnd) {
                        matched = true;
                        break;
                    }
                    patternIndex += 2; // Move past '-'
                }
                else {
                    if (inputLine[inputIndex] == currentPatternChar) {
                        matched = true;
                        break;
                    }
                }
                patternIndex++;
            }
            if (isInverted) {
                matched = !matched;
            }
            if (!matched) {
                return false;
            }
        }
        else if (pattern[patternIndex] == '\\') {
            // Handling escaped characters
            patternIndex++;
            if (patternIndex >= pattern.Length) {
                throw new ArgumentException("Invalid pattern: pattern ends with escape character");
            }
            if (inputLine[inputIndex] != pattern[patternIndex]) {
                return false;
            }
        }
        else {
            // Normal character comparison
            if (inputLine[inputIndex] != pattern[patternIndex]) {
                return false;
            }
        }
        inputIndex++;
        patternIndex++;
    }

    return inputIndex == inputLine.Length && patternIndex == pattern.Length;
}
