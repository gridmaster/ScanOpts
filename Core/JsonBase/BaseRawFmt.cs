﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.JsonBase
{
    public class BaseRawFmt
    {
        public decimal raw { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string fmt { get; set; }
    }
}