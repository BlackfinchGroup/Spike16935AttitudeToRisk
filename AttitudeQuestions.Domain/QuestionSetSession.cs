using AttitudeQuestions.Domain.Shared;

namespace AttitudeQuestions.Domain;

public class QuestionSetSession : AggregateRoot<Guid>
{
    public QuestionSetSession(Guid id, Guid questionSetId) : base(id)
    {
        QuestionSetId = questionSetId;
    }
    public Guid QuestionSetId { get; private set; }
    public List<QuestionAnswers> Answers { get; private set; } = new List<QuestionAnswers>();
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}

public class QuestionAnswers
{
    public Guid QuestionId { get; set; }
}