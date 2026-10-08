using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.DTO_s
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
        public int Stock { get; set; }
        public decimal Rating { get; set; }
    }
}
