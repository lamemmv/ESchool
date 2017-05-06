//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using ESchool.Domain.Entities.Examinations;
//using ESchool.Domain.Enums;
//using Microsoft.EntityFrameworkCore;

//namespace ESchool.Data
//{
//    public sealed class DbTestDataInitializer
//    {
//        private readonly ObjectDbContext _dbContext;

//        public DbTestDataInitializer(ObjectDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public async Task SeedAsync()
//        {
//            await SeedQuestionsAsync();
//        }

//        private async Task SeedQuestionsAsync()
//        {
//            var qtagDbSet = _dbContext.Set<QTag>();
//            var questionDbSet = _dbContext.Set<Question>();
//        }

//        private async Task SeedQuestionsKhoi6Async(DbSet<QTag> qtagDbSet, DbSet<Question> questionDbSet)
//        {
//            string groupName = "Khối 6";
//            string parentQTagName = "Kỹ năng tính toán cơ bản";
//            string qtagNameDaiSo = "Đại số";
//            string qtagNameHinhHoc = "Hình học";
//        }
//    }
//}
