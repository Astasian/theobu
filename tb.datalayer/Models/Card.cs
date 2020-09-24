using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tb.datalayer.Models
{
    public class Card
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public List<string> Tabus { get; set; }

        [JsonIgnore]
        public virtual Language Language { get; set; }

        [Required]
        public int LanguageId { get; set; }

    }
}
