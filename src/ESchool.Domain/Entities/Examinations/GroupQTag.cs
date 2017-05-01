namespace ESchool.Domain.Entities.Examinations
{
    public class GroupQTag
    {
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

        public int QTagId { get; set; }

        public virtual QTag QTag { get; set; }
    }
}
