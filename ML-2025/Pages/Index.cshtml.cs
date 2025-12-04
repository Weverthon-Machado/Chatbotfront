using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.ML;
using ML_2025.Models;
using System.Text;

public class IndexModel : PageModel
{
    private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;
    private readonly string logReqPath = "logs_requisicoes.csv";
    private readonly string logFeedbackPath = "logs_feedback.csv";

    public IndexModel(PredictionEngine<SentimentData, SentimentPrediction> predictionEngine)
    {
        _predictionEngine = predictionEngine;
    }

    [BindProperty]
    public string InputText { get; set; } = string.Empty;

    public SentimentPrediction? PredictionResult { get; set; }

    public void OnGet()
    {
    }

    // ================================
    //  Função: adicionar linha ao CSV
    // ================================
    private void AppendCsv(string path, string header, string line)
    {
        if (!System.IO.File.Exists(path))
            System.IO.File.WriteAllText(path, header + "\n", Encoding.UTF8);

        System.IO.File.AppendAllText(path, line + "\n", Encoding.UTF8);
    }

    // ================================
    // Função principal da IA (POST)
    // ================================
    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(InputText))
            return Page();

        var inputData = new SentimentData { Text = InputText };
        PredictionResult = _predictionEngine.Predict(inputData);

        string resposta = PredictionResult.Score >= 0 ? "Mensagem positiva!" : "Mensagem negativa!";

        string line = $"\"{DateTime.Now:O}\",\"{InputText.Replace("\"", "'")}\",\"{resposta}\",{PredictionResult.Probability},\"{PredictionResult.PredictedLabel}\"";

        AppendCsv(
            logReqPath,
            "data,mensagem,resposta,probabilidade,predicao",
            line
        );

        return new JsonResult(new
        {
            botText = resposta,
            id = Guid.NewGuid().ToString(),
            probability = PredictionResult.Probability,
            prediction = PredictionResult.PredictedLabel
        });
    }

    // =====================================
    //   ENDPOINT PARA RECEBER FEEDBACK
    // =====================================
    public IActionResult OnPostFeedback([FromBody] FeedbackModel feedback)
    {
        if (feedback == null)
            return BadRequest();

        if (feedback.Util)
        {
            string line = $"\"{DateTime.Now:O}\",\"{feedback.Mensagem}\",\"{feedback.Resposta}\",true";

            AppendCsv(
                logFeedbackPath,
                "data,mensagem,resposta,util",
                line
            );
        }

        return new JsonResult(new { status = "feedback salvo" });
    }
}

// MODEL DO FEEDBACK
public class FeedbackModel
{
    public string Mensagem { get; set; }
    public string Resposta { get; set; }
    public bool Util { get; set; }
}
