using AttitudeQuestions.Domain;
using AttitudeQuestions.Infrastructure.Shared.Persistence;

namespace AttitudeQuestions.API;

public static class DataSeeder
{
    public static async Task SeedDevelopmentDataAsync(WebApplication webApplication)
    {
        AsyncServiceScope serviceScope = webApplication.Services.CreateAsyncScope();
        QuestionDbContext questionDbContext = serviceScope.ServiceProvider.GetRequiredService<QuestionDbContext>();

        await questionDbContext.Database.EnsureCreatedAsync();

        AddQuestionSet(questionDbContext, QuestionOneSet);
        AddQuestionSet(questionDbContext, QuestionTwoSet);
        AddQuestionSet(questionDbContext, QuestionThreeSet);
        AddQuestionSet(questionDbContext, QuestionFourSet);
        AddQuestionSet(questionDbContext, QuestionFiveSet);

        await questionDbContext.SaveChangesAsync();
    }

    private static void AddQuestionSet(QuestionDbContext questionDbContext, QuestionSet questionSet)
    {
        if (questionDbContext.QuestionSets.FirstOrDefault(x => x.Id == questionSet.Id) is null)
        {
            questionDbContext.QuestionSets.Add(questionSet);
        }
    }

    private static readonly Guid QuestionOneId = Guid.Parse("7bd9eb71-270e-444b-89de-eda8f4db8e47");
    private static readonly Guid QuestionTwoId = Guid.Parse("b0b8423c-428a-494a-91fc-d3587e77a00d");
    private static readonly Guid QuestionThreeId = Guid.Parse("b808743c-6553-4c17-b2ca-effd618cd706");
    private static readonly Guid QuestionFourId = Guid.Parse("d540fd90-cf68-4a24-a680-e869d1c7efa1");
    private static readonly Guid QuestionFiveId = Guid.Parse("a6678d48-967b-4176-b224-e635a516e559");

    private static QuestionSet QuestionOneSet =>
        new(QuestionOneId,
            "When you think about investing, how do you feel?",
            string.Empty,
            [            
                new Question(Guid.NewGuid(), "I’d rather not take any risks at all", 1),
                new Question(Guid.NewGuid(), "I’m a little cautious, but open to small ups and downs", 2),
                new Question(Guid.NewGuid(), "I’m happy to take a bit of risk if the reward seems fair", 3),
                new Question(Guid.NewGuid(), "I understand there’ll be ups and downs and I’m comfortable with that", 4),
                new Question(Guid.NewGuid(), "I’m happy to take more risk if it means a better chance of growth", 5)
            ],
            QuestionTwoId);

    private static QuestionSet QuestionTwoSet =>
        new(QuestionTwoId,
            "Imagine your investment dropped in value by 10% in a year. What would you do?",
            string.Empty,
            [

                new Question(Guid.NewGuid(), "Sell straight away, I couldn’t cope with the loss", 1),
                new Question(Guid.NewGuid(), "Feel very uncomfortable and think about pulling out", 2),
                new Question(Guid.NewGuid(), "Wait and see what happens", 3),
                new Question(Guid.NewGuid(), "Stay invested and trust things will bounce back", 4),
                new Question(Guid.NewGuid(), "Invest more, it could be a good time to buy", 5)
            ],
            QuestionThreeId);

    private static QuestionSet QuestionThreeSet =>
        new(QuestionThreeId,
            "What’s your goal with this investment?",
            string.Empty,
            [

                new Question(Guid.NewGuid(), "Keep my money safe, even if it grows slowly", 1 ),
                new Question(Guid.NewGuid(), "Grow my money steadily, but keep risk low", 2 ),
                new Question(Guid.NewGuid(), "Find a balance between growth and risk", 3 ),
                new Question(Guid.NewGuid(), "Aim for strong growth, accepting short-term ups and downs", 4 ),
                new Question(Guid.NewGuid(), "Maximise long-term growth, even with bigger swings along the way",  5 ),
            ],
            QuestionFourId);

    private static QuestionSet QuestionFourSet =>
        new(QuestionFourId,
            "How long do you plan to keep this money invested?",
            string.Empty,
            [

                new Question(Guid.NewGuid(), "Less than 3 years", 1),
                new Question(Guid.NewGuid(), "3–5 years", 2),
                new Question(Guid.NewGuid(), "5–10 years", 3),
                new Question(Guid.NewGuid(), "10–15 years", 4),
                new Question(Guid.NewGuid(), "15+ years", 5),
            ],
            QuestionFiveId);

    private static QuestionSet QuestionFiveSet =>
        new(QuestionFiveId,
            "Which best describes your experience with investing?",
            string.Empty,
            [

                new Question(Guid.NewGuid(), "I’ve never invested before", 1),
                new Question(Guid.NewGuid(), "I’ve tried a savings account or cash ISA", 2),
                new Question(Guid.NewGuid(), "I’ve done some investing, but only with low risk", 3),
                new Question(Guid.NewGuid(), "I’ve invested in funds or shares and understand they go up and down", 4),
                new Question(Guid.NewGuid(), "I regularly invest and understand risk levels well", 5),
            ],
            null);
}