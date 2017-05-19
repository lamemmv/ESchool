using System;

namespace ESchool.Data.Entities.Files
{
    public class Blob : BaseEntity
    {
        public string FileName { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string ContentType { get; set; }

        public string Path { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
