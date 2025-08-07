using AttitudeQuestions.Domain.Shared;

namespace AttitudeQuestions.Domain;

public class QuestionSet : AggregateRoot<Guid>
{
    public QuestionSet(Guid id, string title, string subTitle, List<Question> questions, Guid? nextQuestionSetId) : base(id)
    {
        Title = title;
        SubTitle = subTitle;
        Questions = questions;
        NextQuestionSetId = nextQuestionSetId;
    }

    private QuestionSet(Guid id) : base(id)
    {
    }

    public string Title { get; private set; }
    public string SubTitle { get; private set; }
    public List<Question> Questions { get; private set; }
    public Guid? NextQuestionSetId { get; private set; }
}
