using EmotionAnalyzerML.Models;
using EmotionAnalyzerML.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmotionAnalyzer.API.Controllers
{
    // This controller handles emotion prediction requests.
    [ApiController]
    [Route("api/emotion")]
    public class EmotionController : ControllerBase
    {
        private readonly EmotionPredictionService _predictionService;
        private readonly LoadedModelService _loadedModelService;

        public EmotionController(EmotionPredictionService predictionService, LoadedModelService loadedModelService)
        {
            _predictionService = predictionService;
            _loadedModelService = loadedModelService;
        }

        // POST api/emotion/predict
        [HttpPost("predict")]
        public IActionResult Predict(
            [FromBody] PredictionRequest request)
        {
            // Validate the request
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest(new
                {
                    message = "Text is required."
                });
            }

            // Check if the model is loaded
            if (!_loadedModelService.IsLoaded)
            {
                return BadRequest(new
                {
                    message = "Model is not loaded."
                });
            }

            // Perform the prediction
            var result = _predictionService.Predict(request.Text);

            return Ok(result);
        }
    }
}