using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Serialization.Data
{
    [XmlRoot("recipes")]
    public class RecipePage
    {
        [XmlElement("recipes")]
        public Recipe[] recipes { get; set; }
        [XmlIgnore, JsonIgnore]
        public int total { get; set; }
        [XmlIgnore, JsonIgnore]
        public int skip { get; set; }
        [XmlIgnore, JsonIgnore]
        public int limit { get; set; }
    }

    public class Recipe : object
    {
        [XmlIgnore, JsonIgnore]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string RecipeName { get; set; }
        public string[] ingredients { get; set; }
        public string[] instructions { get; set; }
        public int prepTimeMinutes { get; set; }
        public int cookTimeMinutes { get; set; }
        public int servings { get; set; }

        [XmlAttribute]
        public string difficulty { get; set; }

        [XmlAttribute]
        public string cuisine { get; set; }

        [XmlAttribute]
        public int caloriesPerServing { get; set; }
        public string[] tags { get; set; }
        public int userId { get; set; }
        public string image { get; set; }

        [XmlAttribute]
        public float rating { get; set; }

        [XmlAttribute]
        public int reviewCount { get; set; }
        public string[] mealType { get; set; }

        public string FormatProps()
        {
            return $"{RecipeName,-20} Difficulty: {difficulty}, Cuisine: {cuisine}, Calories: {caloriesPerServing}, Tags: {string.Join(", ", tags)}";
        }
    }

}
