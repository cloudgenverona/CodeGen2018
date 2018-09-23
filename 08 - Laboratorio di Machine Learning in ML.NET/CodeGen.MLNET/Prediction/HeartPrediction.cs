using Microsoft.ML.Runtime.Api;

namespace CodeGen.MLNET.Prediction
{
    public class HeartPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint ExpectedDigit; // <-- This is the predicted value

        //[ColumnName("Score")]
        //public float[] Score;
    }
}
