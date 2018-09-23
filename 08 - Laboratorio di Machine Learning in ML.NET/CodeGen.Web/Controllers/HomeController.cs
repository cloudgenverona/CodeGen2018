using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CodeGen.Core.Model;
using CodeGen.MLNET.DataMapping;
using Microsoft.AspNetCore.Mvc;
using CodeGen.Web.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;

namespace CodeGen.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Predict()
        {
            ViewData["Message"] = "Prediction page.";

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpPost]
        public IActionResult PredictDisease(HeartDataViewModel data)
        {
            var dataForPrediction = CastToPredictionModelRequest(data);
            var prediction = GetPredictionWithData(dataForPrediction);
            return Ok(prediction);
        }


        private string GetPredictionWithData(PredictionModelRequest<HeartData> data)
        {
            string apiUrl = "http://localhost:50632/api/Heart/PredictionByModel";
            var client = new HttpClient();

            var dataString = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(dataString);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PostAsync(apiUrl, byteContent).Result;
            return result.Content.ToString();

        }

        private PredictionModelRequest<HeartData> CastToPredictionModelRequest(HeartDataViewModel data)
        {
            return new PredictionModelRequest<HeartData>()
            {
                ObjectToPredict = CastToHeartData(data),
                ModelFilePath = "../CodeGen.MLNET/Data/heart-model-21_09_2018_0007.zip"
            };
        }

        private HeartData CastToHeartData(HeartDataViewModel data)
        {

            return new HeartData()
            {
                Age = data.Age,
                Ca = data.Ca,
                Chol = data.Chol,
                Cp = data.Cp,
                Exang = data.Exang,
                Fbs = data.Fbs,
                OldPeak = data.Fbs,
                Restecg = data.Restecg,
                Sex = data.Sex,
                TrestBps = data.TrestBps,
                Thalach = data.Thalach,
                Slope = data.Slope,
                Thal = data.Thal,
            };
        }

    }
}
