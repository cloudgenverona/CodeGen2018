using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeGen.Core;
using CodeGen.Core.Model;
using CodeGen.MLNET.DataMapping;
using CodeGen.MLNET.Prediction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.Trainers;

namespace CodeGen.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartController : ControllerBase
    {       
        
        [HttpPost("TrainAndSaveModel", Name = "TrainAndSaveModel")]
        public async Task<IActionResult> TrainAndSaveModel([FromBody] TrainRequest trainModel)
        {

            var dateString = DateTime.Now.ToString("dd_MM_yyyy_HHmm");
            // @"../CodeGen.MLNET/Data/heart-dataset.csv", "../CodeGen.MLNET/Data/heart-model-"+ dateString + ".zip",',');
            var model = await MachineLearningCore.TrainAndSaveModel<HeartData, HeartPrediction>(trainModel);
            return Ok(true);
        }

        [HttpPost("PredictionByModel", Name = "GetByRegistration")]
        public async Task<IActionResult> PredictionByModel([FromBody]PredictionModelRequest<HeartData> request)
        {
            var model = await MachineLearningCore.PredictionByModel<HeartData, HeartPrediction>(request);
            return Ok(model.ExpectedDigit);
        }
    }
}
