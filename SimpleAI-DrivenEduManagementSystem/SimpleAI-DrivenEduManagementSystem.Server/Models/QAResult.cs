using Microsoft.ML.Data;

namespace SimpleAI_DrivenEduManagementSystem.Server.Models
{
    public class QAResult
    {
        [ColumnName("Score")]
        public string Answer { get; set; }
    }
}