using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeGen.Core.Model;
using Microsoft.ML.Models;

namespace CodeGen.Core
{
    public static class MachineLearningCore
    {
        /// <summary>
        /// Get the prediction by request.
        /// I use the model created before
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TPrediction"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<TPrediction> PredictionByModel<TData, TPrediction>(PredictionModelRequest<TData> request) where TData : class where TPrediction : class, new()
        {
            var model = await PredictionModel.ReadAsync<TData, TPrediction>(request.ModelFilePath);
            return model.Predict(request.ObjectToPredict);
        }


        /// <summary>
        /// Train and Save model by TrainRequest obj.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TPrediction"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<bool> TrainAndSaveModel<TData, TPrediction>(TrainRequest request) where TData : class where TPrediction : class, new()
        {
            if (File.Exists(request.ModelFilePath))
            {
                File.Delete(request.ModelFilePath);
            }
      
            var model = await CreateModelWithPipeline<TData, TPrediction>(request);

            await model.WriteAsync(request.ModelFilePath);

            return true;
        }


        /// <summary>
        /// Create the pipeline with data send in request
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TPrediction"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        private static async Task<PredictionModel> CreateModelWithPipeline<TData, TPrediction>(TrainRequest request) where TData : class where TPrediction : class, new()
        {

            List<string> properties = GetPropertiesFromDataSetByType<TData>();

            try
            {
                // Pipeline build.
                var learningPipeline = new LearningPipeline
                {
                    new TextLoader(request.FilePath).CreateFrom<TData>(useHeader: request.UseHeader, separator: request.Separator),
                    new ColumnConcatenator("Features", properties.ToArray()),
                    request.Algorythm,
                };
                
                var model = learningPipeline.Train<TData, TPrediction>(); 

                return model;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// Get all props from dataset.
        /// Important for the columnconcatenator in Features column
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        private static List<string> GetPropertiesFromDataSetByType<TData>() where TData : class
        {
            var properties = typeof(TData).GetFields().Select(o => o.Name).ToList();
            properties.Remove("Label");
            return properties;
        }

    }
}
