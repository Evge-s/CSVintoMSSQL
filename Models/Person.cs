using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CSV_Base.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.Date), DisplayName("Date of birthday")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public bool Married { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public decimal Salary { get; set; }
    }
}
