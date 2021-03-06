﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace TTBWeb_Asp.net.Models
{
    [Table("movements")]
    public class Movement
    {
        [Key]
        public int id { get; set; }
        public String Name { get; set; }
        public String Type { get; set; }
        public Movement()
        {

        }
        public Movement(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
