using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spike16935AttitudeToRisk
{
    internal class QuestionSetRepository
    {
        private List<QuestionSet> _questionSets;

        public QuestionSetRepository()
        {
            _questionSets = new List<QuestionSet>()
            {
                QuestionOneSet, QuestionTwoSet, QuestionThreeSet, QuestionFourSet
            };
        }

        public QuestionSet? GetQuestionSetById(Guid id)
        {
            return _questionSets.FirstOrDefault(qs => qs.Id == id);
        }

        private QuestionSet QuestionOneSet =>
            new QuestionSet()
            {
                Id = Guid.Parse("7bd9eb71-270e-444b-89de-eda8f4db8e47"),
                Title = "When you think about investing, how do you feel?",
                NextQuestionSetId = QuestionTwoSet.Id,
                Questions = new List<Question>()
                {
                    new Question() { Id = Guid.NewGuid(), Text = "I’d rather not take any risks at all", Weighting = 1 },
                    new Question() { Id = Guid.NewGuid(), Text = "I’m a little cautious, but open to small ups and downs", Weighting = 2 },
                    new Question() { Id = Guid.NewGuid(), Text = "I’m happy to take a bit of risk if the reward seems fair", Weighting = 3 },
                    new Question() { Id = Guid.NewGuid(), Text = "I understand there’ll be ups and downs and I’m comfortable with that", Weighting = 4 },
                    new Question() { Id = Guid.NewGuid(), Text = "I’m happy to take more risk if it means a better chance of growth", Weighting = 5 },
                }
            };

        private QuestionSet QuestionTwoSet =>
            new QuestionSet()
            {
                Id = Guid.Parse("b0b8423c-428a-494a-91fc-d3587e77a00d"),
                Title = "Imagine your investment dropped in value by 10% in a year. What would you do?",
                NextQuestionSetId = QuestionThreeSet.Id,
                Questions = new List<Question>()
                {
                    new Question() { Id = Guid.NewGuid(), Text = "Sell straight away, I couldn’t cope with the loss", Weighting = 1 },
                    new Question() { Id = Guid.NewGuid(), Text = "Feel very uncomfortable and think about pulling out", Weighting = 1 },
                    new Question() { Id = Guid.NewGuid(), Text = "Wait and see what happens", Weighting = 2 },
                    new Question() { Id = Guid.NewGuid(), Text = "Stay invested and trust things will bounce back", Weighting = 3 },
                    new Question() { Id = Guid.NewGuid(), Text = "Invest more, it could be a good time to buy", Weighting = 4 },
                }
            };

        private QuestionSet QuestionThreeSet =>
            new QuestionSet()
            {
                Id = Guid.Parse("b808743c-6553-4c17-b2ca-effd618cd706"),
                Title = "What’s your goal with this investment?",
                NextQuestionSetId = QuestionFourSet.Id,
                Questions = new List<Question>()
                {
                    new Question() { Id = Guid.NewGuid(), Text = "Keep my money safe, even if it grows slowly", Weighting = 1 },
                    new Question() { Id = Guid.NewGuid(), Text = "Grow my money steadily, but keep risk low", Weighting = 2 },
                    new Question() { Id = Guid.NewGuid(), Text = "Find a balance between growth and risk", Weighting = 3 },
                    new Question() { Id = Guid.NewGuid(), Text = "Aim for strong growth, accepting short-term ups and downs", Weighting = 4 },
                    new Question() { Id = Guid.NewGuid(), Text = "Maximise long-term growth, even with bigger swings along the way", Weighting = 5 },
                }
            };

        private static QuestionSet QuestionFourSet =>
            new QuestionSet()
            {
                Id = Guid.Parse("d540fd90-cf68-4a24-a680-e869d1c7efa1"),
                Title = "How long do you plan to keep this money invested?",
                Questions = new List<Question>()
                {
                    new Question() { Id = Guid.NewGuid(), Text = "Less than 3 years", Weighting = 1 },
                    new Question() { Id = Guid.NewGuid(), Text = "3–5 years", Weighting = 2 },
                    new Question() { Id = Guid.NewGuid(), Text = "5–10 years", Weighting = 3 },
                    new Question() { Id = Guid.NewGuid(), Text = "10–15 years", Weighting = 4 },
                    new Question() { Id = Guid.NewGuid(), Text = "15+ years", Weighting = 5 },
                }
            };

    }
}