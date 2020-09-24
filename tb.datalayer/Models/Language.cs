using Newtonsoft.Json;
using System.Collections.Generic;

namespace tb.datalayer.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }

        [JsonIgnore]
        public virtual List<Card> Cards { get; set; }
    }
}