using System;
using System.IO;

class YourGrep {
    static void Main(string[] args) {
        // Check if there are enough arguments
        if (args.Length < 1) {
            Console.WriteLine("Usage: YourGrep <pattern>");
            Environment.Exit(2);
        }

        // Extract the pattern from the arguments
        string pattern = args[0];

        // Read lines from standard input
        string inputLine;
        while ((inputLine = Console.ReadLine()) != null) {
            // Check if the line matches the pattern
            if (MatchesPattern(inputLine, pattern)) {
                // Print the matching line
                Console.WriteLine(inputLine);
            }
        }
    }

    static bool MatchesPattern(string inputLine, string pattern) {
        // Handle specific patterns
        if (pattern == @"\d") {
            // Check if the line contains any digit
            foreach (char c in inputLine) {
                if (char.IsDigit(c)) {
                    return true;
                }
            }
        }
        else if (pattern == @"\w") {
            // Check if the line contains any word character
            foreach (char c in inputLine) {
                if (char.IsLetterOrDigit(c)) {
                    return true;
                }
            }
        }
        else if (pattern == ".") {
            // Check if the line contains any character (non-empty line)
            return inputLine.Length > 0;
        }
        else if (pattern.StartsWith("^") && pattern.Length > 1) {
            // Check if the line starts with the specified pattern
            return inputLine.StartsWith(pattern.Substring(1));
        }
        else if (pattern.EndsWith("$") && pattern.Length > 1) {
            // Check if the line ends with the specified pattern
            return inputLine.EndsWith(pattern.Substring(0, pattern.Length - 1));
        }
        else if (pattern.StartsWith("[") && pattern.EndsWith("]")) {
            // Handle character class patterns like [abcd]
            string charClass = pattern.Substring(1, pattern.Length - 2);
            foreach (char c in inputLine) {
                if (charClass.Contains(c)) {
                    return true;
                }
            }
        }
        else if (pattern.StartsWith("[^") && pattern.EndsWith("]")) {
            // Handle negated character class patterns like [^xyz]
            string negatedCharClass = pattern.Substring(2, pattern.Length - 3);
            foreach (char c in inputLine) {
                if (!negatedCharClass.Contains(c)) {
                    return true;
                }
            }
        }
        else {
            // Default to simple substring matching
            return inputLine.Contains(pattern);
        }

        return false; // No match found
    }
}
