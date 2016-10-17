using System;
using Weather;

class MyAwesomeSoftware
{
  public static void Main(string[] args)
  {
    Prediction[] values = Previsions.FiveDayPrediction("Strasbourg,fr","");

    foreach(Prediction p in values) {
      Console.WriteLine("Date : "+p.date+", min: "+p.min+", max: "+p.max);
    }
  }
}
