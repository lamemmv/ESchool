using System;
using System.Collections.Generic;

namespace ESchool.Domain.DTOs.Files
{
    public class FileResultDto
    {
        public IList<string> FileNames { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public IList<string> ContentTypes { get; set; }
    }
}
