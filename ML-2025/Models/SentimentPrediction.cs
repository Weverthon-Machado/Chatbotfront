using Microsoft.ML.Data;

namespace ML_2025.Models
{
    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }

        // 🔥 Propriedade necessária para compatibilidade com o backend
        public bool Prediction => PredictedLabel;

        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
