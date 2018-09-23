using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Runtime.Data;

namespace CodeGen.Core.Model
{
    public class PredictionModelRequest<TData>
    {
        public TData ObjectToPredict { get; set; }
        public string ModelFilePath { get; set; }

    }
}
