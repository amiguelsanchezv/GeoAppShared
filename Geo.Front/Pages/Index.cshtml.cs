using Geo.Model;
using Geo.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections;

namespace Geo.Front.Pages
{
    public class IndexModel(ILogger<IndexModel> logger) : PageModel
    {
        public void OnGet()
        {
            logger.Log(LogLevel.Information, "Inicio");
            var datosJS = new List<PoliLineas>();
            GeoreferenceFiberCanalAccess geoAccess = new("mongodb://localhost:27017/poc", "poc", "georeference_fiber_canal");
            var datos = geoAccess.Find(g => g.GeometryType.Equals("esriGeometryPolyline")).GetAwaiter().GetResult();
            foreach (var f in datos?.FirstOrDefault()?.Features ?? [])
            {
                var listaCoordendas = new List<LatLng>();
                foreach (var p in f.Geometry.Paths)
                {
                    foreach (var r in p)
                    {
                        var lat = r[1];
                        var lng = r[0];
                        listaCoordendas.Add(new LatLng() { Lat =lat, Lng = lng });
                    }
                }
                datosJS.Add(new PoliLineas() { Coordenadas = listaCoordendas, Id = f.Attributes.NOMBRE_ESP });
            }
            ViewData["PoliLineas"] = datosJS.Take(100);
            var listaCamaras = new List<Camaras>();
            datos = geoAccess.Find(g => g.GeometryType.Equals("esriGeometryPoint")).GetAwaiter().GetResult();
            foreach (var item in datos?.FirstOrDefault()?.Features ?? [])
            {
                listaCamaras.Add(new Camaras()
                {
                    Id = item.Attributes.NOMBRE_ESP + " - " + item.Attributes.CODIGO_ET,
                    Coordenada = new LatLng() { Lat = item.Geometry.Y, Lng = item.Geometry.X }
                });
            }
            ViewData["Camaras"] = listaCamaras.Take(100);
        }
    }
    public class GeoreferenceFiberCanalAccess(string connectionString, string databaseName, string collectionName) : MongoAccess<FiberCanal>(connectionString, databaseName, collectionName)
    {

    }
    public class PoliLineas
    {
        public required string Id { get; set; }
        public required List<LatLng> Coordenadas { get; set; }
    }

    public class Camaras
    {
        public required string Id { get; set; }
        public required LatLng Coordenada { get; set; }
    }

    public class LatLng
    {
        [JsonProperty("lat")]
        public required double Lat { get; set; }
        [JsonProperty("lng")]
        public required double Lng { get; set; }
    }

    [BsonIgnoreExtraElements(true)]
    public class FiberCanal : IEntity
    {
        public ObjectId Id { get; set; }
        [BsonElement("displayFieldName")]
        public required string DisplayFieldName { get; set; }
        [BsonElement("fieldAliases")]
        public required Fieldaliases FieldAliases { get; set; }
        [BsonElement("geometryType")]
        public required string GeometryType { get; set; }
        [BsonElement("spatialReference")]
        public required Spatialreference SpatialReference { get; set; }
        [BsonElement("fields")]
        public required Field[] Fields { get; set; }
        [BsonElement("features")]
        public required Feature[] Features { get; set; }
    }
    [BsonIgnoreExtraElements(true)]
    public class Fieldaliases
    {
        public required string FID { get; set; }
        public required string ID { get; set; }
        public required string ID_TEXT { get; set; }
        public required string ID_TEXTO { get; set; }
        public required string NAME { get; set; }
        public required string OBJECTID { get; set; }
        public required string SPEC_ID { get; set; }
        public required string NOMBRE_ESP { get; set; }
        public required string APERTURA { get; set; }
        public required string CONSTRUCTI { get; set; }
        public required string COLOCACION { get; set; }
        public required string CALCULATED { get; set; }
        public required string CALCULAT1 { get; set; }
        public required string PERDIDA_DB { get; set; }
        public required string PERDIDA_1 { get; set; }
        public required string NUMERO_CAR { get; set; }
        public required string ABCISA_FIN { get; set; }
        public required string ABCISA_INI { get; set; }
        public required string INSTALLED_ { get; set; }
        public required string DESCRIPTIO { get; set; }
        public required string ID_ESPECIF { get; set; }
        public required string CALCULAT2 { get; set; }
        public required string MEASURED_L { get; set; }
        public required string CALCULAT3 { get; set; }
        public required string SEGMENTO { get; set; }
        public required string PROPIETARI { get; set; }
        public required string UBICACION { get; set; }
        public required string ESTADO_CAM { get; set; }
        public required string TYPE { get; set; }
        public required string CODIGO_ET { get; set; }
        public required string LABEL { get; set; }
        public required string ARRENDADOR { get; set; }
    }
    [BsonIgnoreExtraElements(true)]
    public class Spatialreference
    {
        [BsonElement("wkid")]
        public int Wkid { get; set; }
        [BsonElement("latestWkid")]
        public int LatestWkid { get; set; }
    }
    [BsonIgnoreExtraElements(true)]
    public class Field
    {
        [BsonElement("name")]
        public required string Name { get; set; }
        [BsonElement("type")]
        public required string Type { get; set; }
        [BsonElement("alias")]
        public required string Alias { get; set; }
        [BsonElement("length")]
        public int Length { get; set; }
    }
    [BsonIgnoreExtraElements(true)]
    public class Feature
    {
        [BsonElement("attributes")]
        public required Attributes Attributes { get; set; }
        [BsonElement("geometry")]
        public required Geometry Geometry { get; set; }
    }
    [BsonIgnoreExtraElements(true)]
    public class Attributes
    {
        public int FID { get; set; }
        public long ID { get; set; }
        public required string ID_TEXT { get; set; }
        public required string NAME { get; set; }
        public int SPEC_ID { get; set; }
        public required string NOMBRE_ESP { get; set; }
        public required string CONSTRUCTI { get; set; }
        public required string COLOCACION { get; set; }
        [BsonElement("CÓDIGO_ET")]
        public required string CODIGO_ET { get; set; }
        public double CALCULATED { get; set; }
        public double CALCULAT1 { get; set; }
        public double PERDIDA_DB { get; set; }
        public double PERDIDA_1 { get; set; }
        public int NUMERO_CAR { get; set; }
        public int ABCISA_FIN { get; set; }
        public int ABCISA_INI { get; set; }
        public double INSTALLED_ { get; set; }
        public required string DESCRIPTIO { get; set; }
        public int ID_ESPECIF { get; set; }
        public double CALCULAT2 { get; set; }
        public double MEASURED_L { get; set; }
        public double CALCULAT3 { get; set; }
        public required string SEGMENTO { get; set; }
        public required string PROPIETARI { get; set; }
    }
    [BsonIgnoreExtraElements(true)]
    public class Geometry
    {
        [BsonElement("paths")]
        [BsonIgnoreIfNull(true)]
        public required double[][][] Paths { get; set; }
        [BsonElement("x")]
        [BsonIgnoreIfNull(true)]
        public required double X { get; set; }
        [BsonElement("y")]
        [BsonIgnoreIfNull(true)]
        public required double Y { get; set; }
    }
}
