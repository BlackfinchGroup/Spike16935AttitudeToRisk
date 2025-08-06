// See https://aka.ms/new-console-template for more information
using Spike16935AttitudeToRisk;

Console.WriteLine("Everyone feels differently about taking risks with their money. These questions will help you work out what’s right for you.");
Console.WriteLine("");

var repo = new QuestionSetRepository();
var questionSet = repo.GetQuestionSetById(Guid.Parse("7bd9eb71-270e-444b-89de-eda8f4db8e47"));

var totalWeighting = 0;

while (questionSet != null)
{
    Console.WriteLine(questionSet.Title);
    for (int i = 0; i < questionSet.Questions.Count; i++) // Corrected loop syntax
    {
        var q = questionSet.Questions[i]; // Access the question at index i
        Console.WriteLine("{0}. {1}", i+1, q.Text); // Print the question text
    }
    Console.Write("Please select an option (1-{0}): ", questionSet.Questions.Count);
    var input = Console.ReadLine();

    if (int.TryParse(input, out int choice) && choice >= 1 && choice <= questionSet.Questions.Count)
    {
        var selectedQuestion = questionSet.Questions[choice - 1];
        totalWeighting += selectedQuestion.Weighting;

        if (questionSet.NextQuestionSetId != null)
            questionSet = repo.GetQuestionSetById(questionSet.NextQuestionSetId.Value);
        else
            questionSet = null;
    }   
    else
    {
        Console.WriteLine("Invalid input, please enter a number between 1 and 5.");
    }
}

Console.WriteLine("Total weighting is {0}.", totalWeighting);

Console.ReadLine();
