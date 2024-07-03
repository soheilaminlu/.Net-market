using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace api.Dto
{
    public class UpdateStockRequest
    {

          [MaxLength(10 , ErrorMessage ="Symbol cannot over the 10 character")]
          public string Symbol { get; set; } = string.Empty;
          
        public string CompanyName {get; set; } = string.Empty;
        

        [Required]
        [Range(1 , 100000)]
        public decimal Purchase { get; set; }
        
          [Required]
        [Range(0.001 , 100)]
        public decimal LastDiv {get; set;}


   [MaxLength(10 , ErrorMessage ="Industry cannot over the 10 character")]
        public string Industry {get; set; } = string.Empty;


          [Range(0 , 50000000)]
        public long MarketCap {get;set; }
    }
}