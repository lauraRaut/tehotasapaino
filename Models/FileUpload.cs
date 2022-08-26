using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace Tehotasapaino.Models
{
    //Model for response status and message after performing X operation.
    public class FileUpload
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsResponse { get; set; }
    }

    //Single File Model; To use when uploading a single file at a time. 
    public class SingleFileModel : FileUpload
    {
        [Required(ErrorMessage = "Please enter file name")]
       public string FileName { get; set; }


    }





}
