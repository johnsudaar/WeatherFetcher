using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Globalization;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace Weather
{
  public class Previsions
  {

    private static Prediction[] aggregateResults(Weatherdata data) {
      List<Prediction> predictions = new List<Prediction>();
      DateTime last = new DateTime();
      foreach(Time t in data.Forecast.Time) {
        DateTime date = DateTime.Parse(t.From);
        double temp = Double.Parse(t.Temperature.Value, CultureInfo.InvariantCulture);
        if(predictions.Count == 0) {
          predictions.Add(new Prediction(temp, temp, date.ToString("MM/dd/yyyy")));
          last = date;
        } else if (last.Date != date.Date) {
          predictions.Add(new Prediction(temp, temp, date.ToString("MM/dd/yyyy")));
          last = date;
        } else if (predictions.Last().min > temp) {
          predictions.Last().min = temp;
        } else if (predictions.Last().max < temp) {
          predictions.Last().max = temp;
        }
      }
      return predictions.ToArray();
    }

    public static Prediction[] FiveDayPrediction(string location, string appID){
      WebRequest request = WebRequest.Create("http://api.openweathermap.org/data/2.5/forecast?q="+location+"&mode=xml&units=metric&APPID="+appID);
      WebResponse response = request.GetResponse();
      Stream stream = response.GetResponseStream();
      StreamReader reader = new StreamReader(stream);

      XmlSerializer deserializer = new XmlSerializer(typeof(Weatherdata));
      object obj = deserializer.Deserialize(reader);
      Weatherdata data = (Weatherdata)obj;
      reader.Close();
      response.Close();
      return aggregateResults(data);
    }
  }

  public class Prediction
  {
    public Prediction(double mi, double ma, string d){
      this.min = mi;
      this.max = ma;
      this.date = d;
    }

    public double min {get;set;}
    public double max {get;set;}
    public string date {get;set;}
  }
}
