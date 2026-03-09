namespace StringCalculatorApp
{
    public class StringCalculator
    {
        public int Calculate(string arg)
        {
            if (string.IsNullOrEmpty(arg))
            {
                return 0;
            }

            var (numbersPart, delimiters) = ParseInput(arg);
            var numbers = SplitNumbers(numbersPart, delimiters);
            var integers = ParseNumbers(numbers);

            ValidateNoNegatives(integers);

            return SumIgnoringLargeNumbers(integers);
        }

        private (string numbersPart, List<string> delimiters) ParseInput(string arg)
        {
            if (!arg.StartsWith("//"))
            {
                return (arg, new List<string> { ",", "\n" });
            }

            int newLineIndex = arg.IndexOf('\n');
            string delimiterPart = arg.Substring(2, newLineIndex - 2);
            string numbersPart = arg.Substring(newLineIndex + 1);

            var delimiters = ParseDelimiters(delimiterPart);
            return (numbersPart, delimiters);
        }

        private List<string> ParseDelimiters(string delimiterPart)
        {
            if (!delimiterPart.StartsWith("["))
            {
                return new List<string> { delimiterPart };
            }

            var delimiters = new List<string>();
            int i = 0;

            while (i < delimiterPart.Length)
            {
                if (delimiterPart[i] == '[')
                {
                    int endBracketIndex = delimiterPart.IndexOf(']', i);
                    string delimiter = delimiterPart.Substring(i + 1, endBracketIndex - i - 1);
                    delimiters.Add(delimiter);
                    i = endBracketIndex + 1;
                }
                else
                {
                    i++;
                }
            }

            return delimiters.OrderByDescending(d => d.Length).ToList();
        }

        private string[] SplitNumbers(string numbersPart, List<string> delimiters)
        {
            string normalized = numbersPart;

            foreach (var delimiter in delimiters)
            {
                normalized = normalized.Replace(delimiter, ",");
            }

            return normalized.Split(",", StringSplitOptions.RemoveEmptyEntries);
        }

        private List<int> ParseNumbers(string[] numbers)
        {
            var result = new List<int>();

            foreach (var number in numbers)
            {
                result.Add(int.Parse(number));
            }

            return result;
        }

        private void ValidateNoNegatives(List<int> numbers)
        {
            var negatives = numbers.Where(n => n < 0).ToList();

            if (negatives.Any())
            {
                throw new ArgumentException($"Negative numbers are not allowed");
            }
        }

        private int SumIgnoringLargeNumbers(List<int> numbers)
        {
            int sum = 0;

            foreach (var number in numbers)
            {
                if (number <= 1000)
                {
                    sum += number;
                }
            }

            return sum;
        }
    }
}