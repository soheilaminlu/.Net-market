using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto
{
    public class CreateCommentDto
    {
     public string? Content {get; set;} = string.Empty;


     [MinLength(5 , ErrorMessage = "Title Must be 5 Character")]
     [MaxLength(80 , ErrorMessage = "Title Max Length is 80 Character")]
     public string? Title  {get; set;} = string.Empty;


    }
}