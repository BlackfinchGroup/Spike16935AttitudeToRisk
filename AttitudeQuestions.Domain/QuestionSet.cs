using AttitudeQuestions.Domain.Shared;

namespace AttitudeQuestions.Domain;

// Might need something above questionset to group a related set of questions sets together

public class QuestionSet : AggregateRoot<Guid>
{
    public QuestionSet(Guid id, string title, string subTitle, List<Question> questions, Guid? nextQuestionSetId) : base(id)
    {
        Title = title;
        SubTitle = subTitle;
        Questions = questions;
        NextQuestionSetId = nextQuestionSetId;
    }

    private QuestionSet(): base(Guid.NewGuid())
    {
    }

    public string Title { get; private set; } = null!;
    public string SubTitle { get; private set; } = null!;
    public List<Question> Questions { get; private set; } = [];
    public Guid? NextQuestionSetId { get; private set; }
}
