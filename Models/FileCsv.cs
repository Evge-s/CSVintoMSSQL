using System;
using System.ComponentModel.DataAnnotations;

namespace CSV_Base.Models
{
    public class FileCsv
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime UploadDate { get; set; }

        [Required]
        public string Path { get; set; }
    }
}
