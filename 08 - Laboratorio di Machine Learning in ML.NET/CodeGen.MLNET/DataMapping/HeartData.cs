using Microsoft.ML.Runtime.Api;

namespace CodeGen.MLNET.DataMapping
{
    public class HeartData
    {
        //age,sex,cp,trestbps,chol,fbs,restecg,thalach,exang,oldpeak,slope,ca,thal,num      
        // "Age","Sex", "Cp", "TrestBps", "Chol", "Fbs", "Restecg", "Thalach", "Exang", "OldPeak", "Slope", "Ca", "Thal", "Num" };
        [Column("0")]

        public float Age; //Eta

        [Column("1")]

        public float Sex; //Sesso

        [Column("2")]

        public float Cp; //Dolore Petto

        [Column("3")]

        public float TrestBps; // Pressione a riposo

        [Column("4")]

        public float Chol; //Colesterolo

        [Column("5")]

        public float Fbs; //Zuccheri nel sangue

        [Column("6")]

        public float Restecg; // Elettrocardiogramma 
                              // Value 0: normal --
                              // Value 1: having ST-T wave abnormality (T wave inversions and/or ST elevation or depression of > 0.05 mV) 
                              // Value 2: showing probable or definite left ventricular hypertrophy by Estes' criteria

        [Column("7")]

        public float Thalach; //battito cardiaco piu alto

        [Column("8")]

        public float Exang; // esercizio produce soffocamento affanno

        [Column("9")]

        public float OldPeak; // picco a riposo 

        [Column("10")]

        public float Slope; // pendenza del picco 

        [Column("11")]

        public float Ca;

        [Column("12")]

        public float Thal;

        [Column("13")]
        [ColumnName("Label")]
        public float Label;

  

    }
}
