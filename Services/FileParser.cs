using CSV_Base.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Base.Services
{
    public class FileParser
    {
        List<Person> people = new List<Person>();
        public List<Person> CsvFileToModel(string filePath, string delimiter = ",")
        {
            string[] lines = File.ReadAllLines(filePath);
          //  string[] data = lines.Skip(1).ToArray();
            string[] temp;           

            for (int i = 0; i < lines.Length; i++)
            {
                temp = lines[i].Split(delimiter);
                people.Add(new Person
                {
                    Name = temp[0],
                    DateOfBirth = Convert.ToDateTime(temp[1]),
                    Married = Convert.ToBoolean(temp[2]),
                    Phone = temp[3],
                    Salary = Convert.ToDecimal(temp[4])
                });
            }
            return people;
        }
    }
}
