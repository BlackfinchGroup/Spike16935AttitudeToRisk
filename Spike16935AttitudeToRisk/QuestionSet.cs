using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spike16935AttitudeToRisk;

internal class QuestionSet
{
    public Guid Id { get; set; }
    public List<Question> Questions { get; set; }
    public string Title { get; internal set; }
    public Guid? NextQuestionSetId { get; set; }
}


internal class Question
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public int Weighting { get; set; }
}
