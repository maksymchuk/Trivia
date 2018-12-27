using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TriviaFinal
{
    class Program
    {
        static void Main(string[] args)
        {
            var pathToFiles = @"C:\Class 9\QsandAs.txt";
            var pathToOutFiles = @"C:\Class 9\Results.txt";
            //створюється Ліст щоб зберігати всі питання прочитані з файлу
            Console.WriteLine("Please enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine($"Greetings, {name}, we are going to play a Trivia game today! Are you excited?");
            Console.WriteLine();
            Console.WriteLine("There will be 5 questions. Please provide answer for each question by entering 1/2/3/4 and by pressing Enter key. Let's go!");
            Console.WriteLine("Press Enter to continue ");
            Console.ReadLine();
            Console.WriteLine();

            if (!File.Exists(pathToFiles))
            {
                Console.WriteLine("The file was not found " + pathToFiles);
                return;
            }

            var questions = LoadQuestionsFromFile(pathToFiles);
            RandomizeQuestions(questions);
            var correctCount = AskQuestions(questions);
            SaveResultsToFile(pathToOutFiles, name, correctCount);
        }

        private static void SaveResultsToFile(string pathToOutFiles, string name, int correctCount)
        {
            using (StreamWriter sw = new StreamWriter(pathToOutFiles, true))
            {
                sw.WriteLine($"{DateTime.Now.ToString()} {name} {(correctCount / 5) * 100}%");

            }
        }

        private static int AskQuestions(List<Question> questions)
        {
            int correctCount = 0;
            try
            {
                for (int f = 0; f < 5; f++)
                {
                    Console.WriteLine(questions[f].QuestionText + " \n 1." + questions[f].Choices[0] + "\n 2." + questions[f].Choices[1] + "\n 3." + questions[f].Choices[2] + "\n 4." + questions[f].Choices[3]);
                    Console.WriteLine();
                    Console.WriteLine("\nEnter your choice - 1/2/3/4");
                    var answer = Console.ReadLine();

                    int inputAnswer = int.Parse(answer);


                    if ((inputAnswer - 1) == questions[f].Answer)
                    {
                        Console.WriteLine("Correct\n");
                        correctCount++;


                    }


                    else
                    {
                        Console.WriteLine("Incorrect\n");
                    }
                }
            }
            catch (FormatException fe)
            {
                Console.WriteLine("Please use the correct format\n");
            }

            return correctCount;
        }

        private static void RandomizeQuestions(List<Question> questions)
        {
            var rand = new Random();

            for (int i = 0; i < questions.Count; i++)
            {
                var currentq = questions[i];
                var randomIndexq = rand.Next(questions.Count);
                questions[i] = questions[randomIndexq];
                questions[randomIndexq] = currentq;

            }
        }

        private static List<Question> LoadQuestionsFromFile(string pathToFile)
        {
            var questions = new List<Question>(); 
            //читає файл якщо він існує 
            using (var text = new StreamReader(pathToFile))
            {
                string lines;
                Question question;


                while ((lines = text.ReadLine()) != null)
                {
                    //ініціалізація обєкта
                    question = new Question()
                    {
                        //лінія яка була тільки що прочитана в філду
                        QuestionText = lines,
                        //еррей стрінгів з прочитаниї чойсіс після питання
                        Choices = new string[]
                        {
                                text.ReadLine(),
                                text.ReadLine(),
                                text.ReadLine(),
                                text.ReadLine()
                        }
                    };

                    //перевіряє кожен чойз чи він починається з ^ і індекс чойза з еррея зберігає в філду
                    for (int i = 0; i < 4; i++)
                    {
                        if (question.Choices[i].StartsWith("^"))
                        {
                            question.Choices[i] = question.Choices[i].Substring(1);
                            question.Answer = i;
                            break;
                        }
                    }

                    questions.Add(question);



                    var rand1 = new Random();
                    for (int i = 0; i < 4; i++)
                    {



                        var currenta = question.Choices[i];


                        var randomIndexa = rand1.Next(question.Choices.Length);


                        question.Choices[i] = question.Choices[randomIndexa];
                        question.Choices[randomIndexa] = currenta;
                        if (question.Answer == i)
                        {
                            question.Answer = randomIndexa;
                        }
                        else if (question.Answer == randomIndexa)
                        {
                            question.Answer = i;
                        }


                    }
                }
                return questions;

            }
        }
    }
}
