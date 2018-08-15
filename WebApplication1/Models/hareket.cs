using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class hareket
    {
        public int id { get; set; }

        public DateTime hislemtarih { get; set; }

        public long hkartno { get; set; }

        public long mtckimlikno { get; set; }

        public decimal hislemtutariyi { get; set; }

        public string hharekettipi{ get; set; }

        public string hislemaciklamasi { get; set; }

        public string hmerchname { get; set; }

        public string iislemadi { get; set; }


    }
}