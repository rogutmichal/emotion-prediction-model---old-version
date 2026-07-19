using EmotionAnalyzerAPI.Services;
using EmotionAnalyzerML.Data;
using EmotionAnalyzerML.Models;
using EmotionAnalyzerML.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EmotionAnalyzer.API.Controllers
{
    // This controller handles model training, evaluation, and status requests.
    [ApiController]
    [Route("api/model")]
    public class ModelController : ControllerBase
    {
        private readonly TrainingService _trainingService;
        private readonly LoadedModelService _loadedModelService;
        private readonly ModelLoader _modelLoader;
        private readonly ModelSettings _modelSettings;
        private readonly ModelEvaluationService _evaluationService;
        private readonly EvaluationStorageService _storage;


        public ModelController(TrainingService trainingService, LoadedModelService loadedModelService, ModelLoader modelLoader, IOptions<ModelSettings> options, EvaluationStorageService storage)
        {
            _trainingService = trainingService;
            _loadedModelService = loadedModelService;
            _modelLoader = modelLoader;
            _modelSettings = options.Value;
            _storage = storage;
        }
        // POST api/model/train
        [HttpPost("train")]
        // This endpoint trains the model using the training data and saves it
        public IActionResult Train()
        {
            try
            {
                var trainData = DataLoader.LoadDataFromFile(_modelSettings.TrainFilePath);

                _trainingService.TrainAndSave(trainData, _modelSettings.ModelPath);


                return Ok(new
                {
                    message = "Model trained successfully",
                    modelPath = _modelSettings.ModelPath
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Training failed",
                    error = ex.Message
                });
            }
        }

        // GET api/model/status
        [HttpGet("status")]
        // This endpoint checks if the model is loaded and returns its status
        public IActionResult Status()
        {
            
            return Ok(new
            {
                loaded = _loadedModelService.IsLoaded,
                message = _loadedModelService.IsLoaded
                    ? "Model is ready"
                    : "Model is not loaded"
            });
        }




        // POST api/model/evaluate
        [HttpPost("evaluate")]
        // This endpoint evaluates the model using the test data and saves the evaluation results
        public IActionResult Evaluate()
        {
            try
            {
                // Check if the model is loaded before evaluation
                if (!_loadedModelService.IsLoaded)
                {
                    return BadRequest("Model is not loaded.");
                }

                // Load test data
                var testData = DataLoader.LoadDataFromFile(_modelSettings.TestFilePath);

                // Evaluate the model using the test data
                var evaluator = new ModelEvaluationService(new Microsoft.ML.MLContext());

                
                var result = evaluator.Evaluate(_loadedModelService.Model, testData, "TEST");

                // Save the evaluation results
                _storage.Save(result);


                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET api/model/evaluation
        [HttpGet("evaluation")]
        // This endpoint retrieves the latest evaluation results
        public IActionResult GetEvaluation()
        {
            // Load the evaluation results from storage
            var result = _storage.Load();

            
            if (result == null)
            {
                return NotFound(
                    new
                    {
                        message = "No evaluation results available."
                    });
            }


            return Ok(result);
        }
    }
    }
