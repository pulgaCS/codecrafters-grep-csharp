//using System;
//using System.IO;

//static bool MatchPattern(string inputLine, string pattern) {
//    if (pattern.Length == 1) {
//        return inputLine.Contains(pattern);
//    }
//    else if (pattern == @"\d") {
//        foreach (char c in inputLine) {
//            if (char.IsDigit(c)) {
//                return true;
//            }
//        }
//        return false;
//    }
//    else if (pattern == @"\w") {
//        foreach (char c in inputLine) {
//            if (char.IsLetterOrDigit(c)) {
//                return true;
//            }
//        }
//        return false;
//    }
//    else if (pattern.Length > 2 && pattern[0] == '[' && pattern[pattern.Length - 1] == ']') {
//        if (pattern[1] == '^') {
//            foreach (char c in inputLine) {
//                if (!pattern.Substring(2, pattern.Length - 3).Contains(c)) {
//                    return true;
//                }
//            }
//            return false;
//        }
//        else {
//            foreach (char c in inputLine) {
//                if (pattern.Substring(1, pattern.Length - 2).Contains(c)) {
//                    return true;
//                }
//            }
//            return false;
//        }
//    }
//    else {
//        throw new ArgumentException($"Unhandled pattern: {pattern}");
//    }
//}

//if (args.Length < 2 || args[0] != "-E") {
//    Console.WriteLine("Expected first argument to be '-E'");
//    Environment.Exit(2);
//}

//string pattern = args[1];
//string inputLine = Console.In.ReadToEnd();

//// You can use print statements as follows for debugging, they'll be visible when running tests.
//Console.WriteLine("Logs from your program will appear here!");

//if (MatchPattern(inputLine, pattern)) {
//    Environment.Exit(0);
//}
//else {
//    Environment.Exit(1);
//}

using System;

class Program {
    static bool Matcher(string inputLine, string pattern) {
        int ptr1 = 0;
        int ptr2 = 0;

        if (inputLine == "" && pattern == "")
            return true;
        else if (inputLine == "")
            return false;
        else if (pattern == "")
            return true;

        while (ptr1 < inputLine.Length) {
            if (ptr2 + 1 < pattern.Length && pattern.Substring(ptr2, 2) == "\\d") {
                if (Char.IsDigit(inputLine[ptr1]))
                    return Matcher(inputLine.Substring(ptr1 + 1), pattern.Substring(ptr2 + 2));
                else
                    ptr1++;
            }
            else if (ptr2 + 1 < pattern.Length && pattern.Substring(ptr2, 2) == "\\w") {
                if (Char.IsLetterOrDigit(inputLine[ptr1]))
                    return Matcher(inputLine.Substring(ptr1 + 1), pattern.Substring(ptr2 + 2));
                else
                    ptr1++;
            }
            else if (inputLine[ptr1] == pattern[ptr2])
                return Matcher(inputLine.Substring(ptr1 + 1), pattern.Substring(ptr2 + 1));
            else
                ptr1++;
        }
        return false;
    }

    static bool MatchPattern(string inputLine, string pattern) {
        if (pattern.Length == 1)
            return inputLine.Contains(pattern);
        else if (pattern == "\\d") {
            for (int i = 0; i < 10; i++) {
                if (inputLine.Contains(i.ToString()))
                    return true;
            }
        }
        else if (pattern == "\\w")
            return inputLine.All(char.IsLetterOrDigit);
        else if (pattern.StartsWith("[^") && pattern.EndsWith("]")) {
            string pat = pattern.Substring(2, pattern.Length - 3);
            foreach (char ch in pat) {
                if (inputLine.Contains(ch))
                    return false;
            }
            return true;
        }
        else if (pattern.StartsWith("[") && pattern.EndsWith("]")) {
            string pat = pattern.Substring(1, pattern.Length - 2);
            foreach (char ch in pat) {
                if (inputLine.Contains(ch))
                    return true;
            }
        }
        return false;
    }

    static void Main(string[] args) {
        if (args.Length < 3) {
            Console.WriteLine("Usage: program.exe -E pattern input_line");
            Environment.Exit(1);
        }

        if (args[0] != "-E") {
            Console.WriteLine("Expected first argument to be '-E'");
            Environment.Exit(1);
        }

        string pattern = args[1];
        string inputLine = args[2];

        Console.WriteLine("Logs from your program will appear here!");

        if (MatchPattern(inputLine, pattern))
            Environment.Exit(0);
        else
            Environment.Exit(1);
    }
}
