using System.ComponentModel.DataAnnotations.Schema;

namespace RS3Bot.Abstractions.Model
{
    public class ExpGain
    {
        public int Skill { get; set; }
        public double Amount { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public CurrentTask CurrentTask { get; set; }
        public int CurrentTaskId { get; set; }
    }
}