using GeoJSON.Net;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleApp.RouteService.Dto
{
    public class DirectionService
    {
        public DirectionService(string apiKey)
        {
            ApiKey = apiKey;
        }

        public Model.Route GetRoute(MultiPoint locations)
        {
            var geoJson = /*"{\"coordinates\":[[8.681495,49.41461],[8.686507,49.41943],[8.687872,49.420318]]}";*/ GeometryToGeoJson(locations);
            List<List<double>> coordinate = new List<List<double>>();
            foreach (var geometry in locations.Geometries)
            {
                coordinate.Add(new List<double>(new[] { ((Point)geometry).X, ((Point)geometry).Y }));
            }
            var requestGeometry = JsonConvert.SerializeObject( new RequestGeometry(coordinate));

            
            var baseAddress = new Uri("https://api.openrouteservice.org/v2/directions/driving-car/geojson");
            var httpClient = WebRequest.CreateHttp(baseAddress);
            httpClient.KeepAlive = false;
            httpClient.Method = "POST";
            httpClient.ContentType = "application/json";
            httpClient.Headers.Add("Authorization", ApiKey);
            var requestStream = httpClient.GetRequestStream();
            requestStream.Write(Encoding.UTF8.GetBytes(requestGeometry));
            requestStream.Flush();
            var response = new StreamReader(((HttpWebResponse)httpClient.GetResponse()).GetResponseStream()).ReadToEnd();
            Root root = JsonConvert.DeserializeObject<Root>(response);
            var coordinates = new List<Coordinate>();

            root.features[0].geometry.coordinates.ForEach(o => coordinates.Add(new Coordinate(o[0], o[1])));
            return new Model.Route( new LineString(coordinates.ToArray()));
        }
        public string ApiKey { get; }
        private string GeometryToGeoJson(NetTopologySuite.Geometries.Geometry geometry)
        {

            MultiPoint locations1 = new MultiPoint(new[] { new Point(8.681495, 49.41461), new Point(8.686507, 49.41943), new Point(8.687872, 49.420318) });

            string geoJson;

            var serializer = GeoJsonSerializer.Create();
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                serializer.Serialize(jsonWriter, geometry);
                geoJson = stringWriter.ToString();
            }
            return geoJson;
        }
    }
}
