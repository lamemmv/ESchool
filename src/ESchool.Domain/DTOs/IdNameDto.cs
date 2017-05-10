namespace ESchool.Domain.DTOs
{
    public class IdNameDto
    {
        public IdNameDto(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
