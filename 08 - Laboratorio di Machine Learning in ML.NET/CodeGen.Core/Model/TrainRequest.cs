using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;

namespace CodeGen.Core.Model
{
    public class TrainRequest
    {
        public ILearningPipelineItem Algorythm { get; set; }
        public string FilePath { get; set; }
        public string ModelFilePath { get; set; }
        public bool UseHeader { get; set; }
        public char Separator { get; set; }
        public string PredictedLabel { get; set; }
    }
}
