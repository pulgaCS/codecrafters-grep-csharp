using System;
using System.Linq;

public class Pattern {
    public const string DIGIT = @"\d";
    public const string ALNUM = @"\w";
}

public class Program {
    public static bool MatchPattern(string inputLine, string pattern) {
        if (pattern.Length == 1) {
            return inputLine.Contains(pattern);
        }
        else if (pattern == Pattern.DIGIT) {
            return inputLine.Any(char.IsDigit);
        }
        else if (pattern == Pattern.ALNUM) {
            return inputLine.Any(char.IsLetterOrDigit);
        }
        if (inputLine.Length == 0 && pattern.Length == 0) {
            return true;
        }
        if (string.IsNullOrEmpty(pattern)) {
            return true;
        }
        if (string.IsNullOrEmpty(inputLine)) {
            return false;
        }
        if (pattern[0] == inputLine[0]) {
            return MatchPattern(inputLine.Substring(1), pattern.Substring(1));
        }
        else if (pattern.StartsWith(Pattern.DIGIT)) {
            for (int i = 0; i < inputLine.Length; i++) {
                if (char.IsDigit(inputLine[i])) {
                    return MatchPattern(inputLine.Substring(i), pattern.Substring(2));
                }
            }
            return false;
        }
        else if (pattern.StartsWith(Pattern.ALNUM)) {
            if (char.IsLetterOrDigit(inputLine[0])) {
                return MatchPattern(inputLine.Substring(1), pattern.Substring(2));
            }
            else {
                return false;
            }
        }
        else if (pattern[0] == '[' && pattern[pattern.Length - 1] == ']') {
            if (pattern[1] == '^') {
                var chrs = pattern.Substring(2, pattern.Length - 3).ToCharArray();
                foreach (var c in chrs) {
                    if (inputLine.Contains(c)) {
                        return false;
                    }
                }
                return true;
            }
            var characters = pattern.Substring(1, pattern.Length - 2).ToCharArray();
            foreach (var c in characters) {
                if (inputLine.Contains(c)) {
                    return true;
                }
            }
            return false;
        }
        else {
            throw new InvalidOperationException($"Unhandled pattern: {pattern}");
        }
    }

    public static void Main(string[] args) {
        if (args.Length < 3 || args[0] != "-E") {
            Console.WriteLine("Expected first argument to be '-E'");
            Environment.Exit(1);
        }
        string pattern = args[2];
        string inputLine = Console.In.ReadToEnd();
        if (MatchPattern(inputLine, pattern)) {
            Environment.Exit(0);
        }
        else {
            Console.WriteLine("No match found");
            Environment.Exit(1);
        }
    }
}