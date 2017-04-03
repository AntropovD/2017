using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterPasswordUnderObservation
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = new EnterPasswordUnderObservation();
            try
            {
                task.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    class EnterPasswordUnderObservation
    {
        private IEnumerable<char> mainAlphabet;
        private IEnumerable<char> secretAlphabet;
        private IEnumerable<char> publicAlphabet;
        private string password;
        private Random random = new Random();

        public void Run()
        {
            SetUp();
            Console.WriteLine("###Authentification###");
            var isRunning = true;
            while (isRunning)
            {
                bool success = Try();
                var result = AskContinue();
                switch (result)
                {
                    case 'Y':
                        continue;
                    case 'N':
                        return;                    
                }
            }
        }

        public char AskContinue()
        {            
            char result;
            do
            {
                Console.WriteLine("Repeat?: [Y/N]");
                 result = char.ToUpper((char)Console.ReadLine()[0]);
            } while (!IsPossibleResult(result));
            return result;
        }

        private char[] possibleAnswers = new char[] { 'Y', 'N' };

        public bool IsPossibleResult(char result)
        {
            return possibleAnswers.Contains(result);
        }

        public void SetUp()
        {
            Console.WriteLine("###SetUp###");
            Console.WriteLine("Main alphabet: ");
            mainAlphabet = Console.ReadLine().ToCharArray().Distinct();
            Console.WriteLine("Secret alphabet: ");
            secretAlphabet = Console.ReadLine().ToCharArray().Distinct();
            if (!CheckAlphabets(mainAlphabet, secretAlphabet))
                throw new Exception("Main alphabet should contain secret alphabet");

            publicAlphabet = mainAlphabet.Except(secretAlphabet);
            if (publicAlphabet.Count() == 0)
                throw new Exception("Main and Secret alphabets should not be same");

            Console.WriteLine("Enter Password: ");
            password = Console.ReadLine();
            Console.WriteLine();
        }

        public bool CheckAlphabets(IEnumerable<char> mainAlphabet, IEnumerable<char> secretAlphabet)
        {
            foreach (var letter in secretAlphabet)            
                if (!mainAlphabet.Contains(letter))
                    return false;
            return true;
        }

        public bool Try()
        {
            var hint = GenerateHint();            
            Console.WriteLine("Hint: " + hint);
            Console.WriteLine("Answer: ");
            var answer = Console.ReadLine();
            if (CheckAnswer(hint, answer))
            {
                Console.WriteLine("Access Granted");
                return true;
            }
            else
            {
                Console.WriteLine("Access Denied");
                return false;
            }
        }

        public string GenerateHint()
        {
            var mask = GenerateMask(password.Length);
            var result = new StringBuilder();

            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] == 0)
                    result.Append(GetRandomLetter(publicAlphabet));
                else
                    result.Append(GetRandomLetter(secretAlphabet));
            }
            return result.ToString();
        }

        public char GetRandomLetter(IEnumerable<char> alphabet)
        {
            var index = random.Next(alphabet.Count());
            return alphabet.ToArray()[index];
        }

        private const int maxAttempts = 100;

        public int[] GenerateMask(int onesCount)
        {
            var result = new int[onesCount * 2];
            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                var count = 0;
                for (int i = 0; i < onesCount * 2; i++)
                {
                    var next = random.Next() % 2;
                    result[i] = next;
                    count += next;
                }
                if (count == onesCount)
                    return result;
            }
            throw new Exception("Error: Cannot generate  bitmask");
        }

        public bool CheckAnswer(string hint, string answer)
        {
            var recoveredPassword = new StringBuilder();
            if (answer.Length != hint.Length)
                throw new Exception("Answer length must be equal Hint Length");

            for (int i = 0; i < hint.Length; i++)
            {
                if (secretAlphabet.Contains(hint[i]))
                    recoveredPassword.Append(answer[i]);
            }
            return recoveredPassword.ToString() == password;
        }
    }
}
