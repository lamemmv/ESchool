using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data
{
    public sealed class DbTestDataInitializer
    {
        private readonly ObjectDbContext _dbContext;

        public DbTestDataInitializer(ObjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SeedAsync()
        {
            await SeedQTagsAsync();
            await SeedQuestionsAsync();

            return 0;
        }

        private async Task<int> SeedQTagsAsync()
        {
            var qtags = new string[] { "Toán", "Vật Lý", "Hóa Học", "Anh Văn", "Địa Lý", "Lịch Sử", "Sinh Học", "Văn Học" };

            var dbSet = _dbContext.Set<QTag>();
            var existingQTags = dbSet.AsNoTracking().Where(t => qtags.Contains(t.Name));

            if (await existingQTags.AnyAsync())
            {
                qtags = qtags.Except(existingQTags.Select(t => t.Name)).ToArray();
            }

            await dbSet.AddRangeAsync(qtags.Select(t => new QTag { Name = t, Description = t }));

            return await _dbContext.SaveChangesAsync();
        }

        private async Task<int> SeedQuestionsAsync()
        {
            var qtagDbSet = _dbContext.Set<QTag>();
            var questionTagDbSet = _dbContext.Set<QuestionTag>();
            var questionDbSet = _dbContext.Set<Question>();

            await SeedQuestionsVanHocAsync(qtagDbSet, questionTagDbSet, questionDbSet);
            await SeedQuestionsSinhHocAsync(qtagDbSet, questionTagDbSet, questionDbSet);
            await SeedQuestionsLichSuAsync(qtagDbSet, questionTagDbSet, questionDbSet);

            return 0;
        }

        private async Task<int> SeedQuestionsVanHocAsync(DbSet<QTag> qtagDbSet, DbSet<QuestionTag> questionTagDbSet, DbSet<Question> questionDbSet)
        {
            var qtagName = "Văn Học";
            var qtag = await qtagDbSet.SingleOrDefaultAsync(t => t.Name.Equals(qtagName, StringComparison.OrdinalIgnoreCase));

            if (qtag != null)
            {
                var questionTags = questionTagDbSet.AsNoTracking().Where(qt => qt.QTagId == qtag.Id);

                if (!await questionTags.AnyAsync())
                {
                    var questions = SeedQuestionsVanHocAndDifficultLevel2();
                    questions.AddRange(SeedQuestionsVanHocAndDifficultLevel3());
                    questions.AddRange(SeedQuestionsVanHocAndDifficultLevel4());
                    questions.AddRange(SeedQuestionsVanHocAndDifficultLevel5());

                    await questionDbSet.AddRangeAsync(questions);

                    return await _dbContext.SaveChangesAsync();
                }
            }

            return 0;
        }

        private async Task<int> SeedQuestionsSinhHocAsync(DbSet<QTag> qtagDbSet, DbSet<QuestionTag> questionTagDbSet, DbSet<Question> questionDbSet)
        {
            var qtagName = "Sinh Học";
            var qtag = await qtagDbSet.SingleOrDefaultAsync(t => t.Name.Equals(qtagName, StringComparison.OrdinalIgnoreCase));

            if (qtag != null)
            {
                var questionTags = questionTagDbSet.AsNoTracking().Where(qt => qt.QTagId == qtag.Id);

                if (!await questionTags.AnyAsync())
                {
                    var questions = SeedQuestionsSinhHocAndDifficultLevel2();
                    questions.AddRange(SeedQuestionsSinhHocAndDifficultLevel3());
                    questions.AddRange(SeedQuestionsSinhHocAndDifficultLevel4());
                    questions.AddRange(SeedQuestionsSinhHocAndDifficultLevel5());

                    await questionDbSet.AddRangeAsync(questions);

                    return await _dbContext.SaveChangesAsync();
                }
            }

            return 0;
        }

        private async Task<int> SeedQuestionsLichSuAsync(DbSet<QTag> qtagDbSet, DbSet<QuestionTag> questionTagDbSet, DbSet<Question> questionDbSet)
        {
            var qtagName = "Lịch Sử";
            var qtag = await qtagDbSet.SingleOrDefaultAsync(t => t.Name.Equals(qtagName, StringComparison.OrdinalIgnoreCase));

            if (qtag != null)
            {
                var questionTags = questionTagDbSet.AsNoTracking().Where(qt => qt.QTagId == qtag.Id);

                if (!await questionTags.AnyAsync())
                {
                    var questions = SeedQuestionsLichSuAndDifficultLevel2();
                    questions.AddRange(SeedQuestionsLichSuAndDifficultLevel3());
                    questions.AddRange(SeedQuestionsLichSuAndDifficultLevel4());
                    questions.AddRange(SeedQuestionsLichSuAndDifficultLevel5());

                    await questionDbSet.AddRangeAsync(questions);

                    return await _dbContext.SaveChangesAsync();
                }
            }

            return 0;
        }

        #region Seed Questions

        private List<Question> SeedQuestionsVanHocAndDifficultLevel2()
        {
            var type = (int)QuestionType.SingleChoice;
            var difficultLevel = 2;

            return new List<Question>
            {
                new Question
                {
                    Content = "<b>Đất Nước</b> của Nguyễn Khoa Điềm là đoạn thơ mang tính:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Hiện thực - trào lộng.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Trữ tình.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Chính luận - trữ tình.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Chính luận.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Thông tin nào sau đây không chính xác khi nói về tiểu sử Nguyễn Khoa Điềm?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Trong cuộc Tổng tiến công Mậu Thân năm 1968 đã bị giặc bắt.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Xuất thân trong gia đình trí thức cách mạng.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tốt nghiệp Đại học Sư phạm Hà Nội năm 1964, sau đó vào miền Nam tham gia chiến đấu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Từng là Tổng thư kí Hội nhà văn Việt Nam (khóa V) và Bộ trưởng Bộ Văn hóa - Thông tin.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<b>Đất Nước</b> của Nguyễn Khoa Điềm thể hiện sâu sắc tư tưởng cốt lõi nào sau đây?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Đất Nước của Lạc Long Quân và Âu Cơ.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Đất Nước của những vương triều trong lịch sử.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Đất Nước của ca dao thần thoại.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Đất Nước của Nhân dân.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Tác phẩm nào sau đây là của Nguyễn Khoa Điềm?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Hoa trên đá.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Trường ca Tiếng hát quan họ.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Đất ngoại ô.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Lời ru trên mặt đất.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đối với suy nghĩ và cảm xúc của Nguyễn Khoa Điềm trong Đất Nước thì người đã <b>làm ra Đất Nước</b> là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Những anh hùng nổi tiếng trong lịch sử.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Những bậc nam nhi - trụ cột trong xã hội.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Vô vàn những người con trai, con gái vô danh, bình dị.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Các vị vua Hùng.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Câu thơ: <b>Biết trồng tre đợi ngày thành gậy<b> trong Đất Nước của Nguyễn Khoa Điềm được lấy ý từ:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Bài thơ Tre Việt Nam của Nguyễn Duy.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Truyền thuyết Thánh Gióng.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Một câu ca dao xưa.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Truyện cổ tích Cây tre trăm đốt.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đoạn trích <b>Đất nước</b> từ trường ca <b>Mặt đường khát vọng</b> của Nguyễn Khoa Điềm cho ta biết quan niệm của tác giả về đất nước là gì?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Là quê hương của những người con ưu tú đã chiến đấu hết mình vì gia đình, bạn bè, Tổ quốc.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Là sự kết hợp giữa truyền thống và hiện đại, giữa khoa học kĩ thuật với văn nghệ trong thời kì mới.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Là sự kết hợp của nhiều phương diện từ lịch sử tới văn hóa, do nhân dân làm ra và làm chủ.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Là sự định đoạt rõ ràng về biên giới địa phận, có người chủ lãnh đạo qua các thời kì lịch sử.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nhận xét nào sau đây về Nguyễn Khoa Điềm không chính xác?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Là Trưởng ban Tư tưởng Văn hóa Trung ương, uỷ viên Bộ Chính trị Ban Chấp hành Trung ương Đảng Cộng sản Việt Nam khoá 9.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Ông đã được giải thưởng Hội Nhà văn Việt Nam với tập thơ Ngôi nhà có ngọn lửa ấm.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Khi Cách mạng tháng Tám bùng nổ, ông là chủ tịch Ủy ban khởi nghĩa của thành phố Huế.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Là con của nhà cách mạng Hải Triều Nguyễn Khoa Văn.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Câu <b>Đất Nước bắt đầu với miếng trầu bây giờ bà ăn</b> trong <b>Đất Nước</b> của Nguyễn Khoa Điềm có ý nghĩa phù hợp với cảm xúc mà tác giả đang thể hiện là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Gợi nhớ câu chuyện cảm động, đầy tình nghĩa là <b>Sự tích trầu cau</b>.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Đất nước gắn với những phong tục lâu đời của dân tộc là tục ăn trầu có từ thời vua Hùng. Hình ảnh bà ăn trầu gợi phong tục và gợi hình đất nước.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Gợi nhớ hình ảnh rất quen thuộc trong đất nước là người bà ăn trầu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Gợi nhớ hình ảnh thân thiết về người bà thân yêu.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Sự hình dung nào không có về <i>Đất Nước</i> trong <b>Đất Nước</b> của Nguyễn Khoa Điềm?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Sự hình dung qua chiều sâu văn hóa - phong tục, lối sống, tính cách của con người Việt Nam.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Sự hình dung qua chiều dài thời gian, lịch sử.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Sự hình dung qua bề rộng của không gian - lãnh thổ địa lí.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Sự hình dung qua thời gian trị vì của các triều đại phong kiến Việt Nam.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Trường ca <b>Mặt đường và khát vọng</b> của Nguyễn Khoa Điềm được hoàn thành ở chiến khu Trị - Thiên vào năm nào?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "1970.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "1974.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "1973.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "1971.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Cảm xúc chính trong <b>Đất Nước</b> của Nguyễn Khoa Điềm là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Ca ngợi Đất Nước đau thương mà anh hùng.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Ca ngợi vẻ đẹp của Đất Nước.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Cảm nhận và lí giải về mối quan hệ giữa Đất và Nước.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Cảm nhận và lí giải về Đất Nước.",
                            DSS = true
                        }
                    }
                }
            };
        }

        private List<Question> SeedQuestionsVanHocAndDifficultLevel3()
        {
            var type = (int)QuestionType.SingleChoice;
            var difficultLevel = 3;

            return new List<Question>
            {
                new Question
                {
                    Content = "Năm sinh, năm mất của nhà thơ Chế Lan Viên là năm nào sau đây?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "1924 - 1985.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "1920 - 1985.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "1922 - 1989.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "1920 - 1989.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Nhà thơ Chế Lan Viên quê ở:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Quy Nhơn.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Thanh Hóa.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Quảng Trị.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Quảng Bình.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Trong bài thơ Tiếng hát con tàu có câu:</p><p><i>Con đã đi nhưng con cần vượt nữa</i></p><p><i>Cho con về gặp lại Mẹ yêu thương</i></p><p>Hiểu như thế nào là đúng nhất về hình ảnh <b>Mẹ yêu thương<b> trong hai câu thơ trên?</p>",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Đó là người mẹ Tây Bắc đã nuôi dưỡng nhà thơ khi đau yếu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Đó là mẹ của nhà thơ.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Đó là nhân dân, đất nước.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Đó là <b>người mẹ</b> tượng trưng của hồn thơ, của cảm hứng sáng tạo.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nhận xét nào sau đây chính xác về nhà thơ Chế Lan Viên?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Có phong cách rõ nét và độc đáo, nổi bật nhất là chất suy tưởng triết lí mang vẻ đẹp trí tuệ và sự đa dạng, phong phú của thế giới hình ảnh thơ.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Gây được ấn tượng khá đặc biệt bằng một chất giọng trong sáng mà tha thiết, sâu lắng mà tài hoa.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Có giọng điệu riêng rất dễ nhận ra, đó là giọng tâm tình ngọt ngào tha thiết, giọng của tình thương mến.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Có một hồn thơ phóng khoáng, hồn hậu, đầy lãng mạn.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Hai câu thơ <b>Khi ta ở chỉ là nơi đất ở/ Khi ta đi đất đã hóa tâm hồn</b> (Tiếng hát con tàu - Chế Lan Viên) gợi ra những suy tưởng nào sau đây?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "<b>Đất</b> mang tâm hồn cố nhân. ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Từ vật chất, thô sơ (đất) đã huyển hóa thành tinh thần, cao quý (tâm hồn).",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "<b>Đất</b> trở thành một phần tâm hồn ta.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Cả A, B, C đều đúng",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Nhận xét nào sau đây không chính xác về Chế Lan Viên?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Thơ của ông chú trọng về nhạc điệu, ông đã khởi đầu một lối thơ chỉ dùng toàn vần bằng.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Ông tên thật là Phan Ngọc Hoan, sinh năm 1920 tại Cam An, Cam Lộ, Quảng Trị.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Phong cách thơ Chế Lan Viên rất rõ nét và độc đáo, nổi bật nhất là chất suy tưởng triết lí mang vẻ đẹp trí tuệ và sự đa dạng, phong phú của hình ảnh thơ được sáng tạo bởi một ngòi bút thông minh, tài hoa.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Sau Cách mạng, thơ ông đã <b>đến với cuộc sống nhân dân và đất nước, thấm nhuần ánh sáng của cách mạng</b> và có những thay đổi rõ rệt.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Bài thơ <b>Tiếng hát con tàu</b> của Chế Lan Viên được sáng tác vào thời điểm cụ thể nào sau đây?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Trong thời kì kháng chiến chống Pháp (1946 - 1954).",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Trong thời kì xây dựng xã hội chủ nghĩa ở miền Bắc.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Trong thời kì có cuộc vận động nhân dân miền xuôi lên Tây Bắc xây dựng kinh tế miền núi vào những năm 1958 - 1960.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Trong thời gian nhà thơ đi thực tế ở Tây Bắc.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Cử chỉ, hành động nào thật cảm động mà Chế Lan Viên đã nhớ đến khi nghĩ về anh du kích Tây Bắc?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Đêm rét chung chăn.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Trao lại chiếc áo nâu một đời vá rách.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Chia sẻ phần cơm ít ỏi của mình trong những ngày bị địch bao vây.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Che chắn cho đồng đội khi công đồn.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Hiểu như thế nào là phù hợp nhất với đối tượng trữ tình mà nhà thơ gọi là <b>anh</b> ở hai khổ thơ đầu trong bài <b>Tiếng hát con tàu</b> của Chế Lan Viên?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Những con người đi xây dựng cuộc sống mới ở Tây Bắc những năm 1958 - 1960.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Đó chính là nhà thơ.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Những con người đi trên chuyến tàu hỏa về Tây Bắc.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Cách phân đôi của chủ thể trữ tình, tự đối thoại dưới hình thức như lời thuyết phục người khác.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Giá trị nội dung tiêu biểu nhất trong bài thơ <b>Tiếng hát con tàu</b> của Chế Lan Viên là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Thể hiện khát vọng về với những kỉ niệm sâu nặng nghĩa tình trong cuộc kháng chiến chống Pháp, về với ngọn nguồn cho cảm hứng sáng tạo nghệ thuật chân chính của mình.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Thể hiện lòng căm thù trước tội ác của thực dân Pháp đối với nhân dân Tây Bắc anh dũng, kiên cường.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Thể hiện niềm vui trước công cuộc xây dựng kinh tế mới ở Tây Bắc những năm 1958 - 1960.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Thể hiện hình ảnh cuộc kháng chiến chống Pháp.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trong bài thơ <b>Tiếng hát con tàu</b> (Chế Lan Viên), hình ảnh nào sau đây được nhà thơ dùng để thể hiện sự hạnh phúc của mình khi được <b>gặp lại nhân dân</b>?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Thuyền và biển.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Nai về suối cũ.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Ánh sáng và phù sa.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Cả A, B, C đều đúng.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Bài thơ <b>Tiếng hát con tàu</b> được rút ra từ tập thơ:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Điêu tàn.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Ánh sáng và phù sa.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Những bài thơ đánh giặc.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Hái theo mùa.",
                            DSS = false
                        }
                    }
                }
            };
        }

        private List<Question> SeedQuestionsVanHocAndDifficultLevel4()
        {
            var type = (int)QuestionType.SingleChoice;
            var difficultLevel = 4;

            return new List<Question>
            {
                new Question
                {
                    Content = "Phong cách ngôn ngữ hành chính có các đặc trưng cơ bản nào?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tính công vụ.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Tính khuôn mẫu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tính minh xác.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tất cả các đặc trưng.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Ngôn ngữ khoa học là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Ngôn ngữ có sự hỗ trợ của các kí hiệu, công thức, bảng biểu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Ngôn ngữ được dùng trong giao tiếp thuộc lĩnh vực khoa học.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Ngôn ngữ có yêu cầu cao về phát âm, diến đạt.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Cả ba đáp án.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Để hoạt động giao tiếp thuận lợi, các nhân vật gao tiếp phải:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Không được giữ khoảng cách trong giao tiếp.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Đổi vai và luân phiên lượt lời với nhau.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Hiểu rõ đối tượng đang giao tiếp.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Có chiến lược giao tiếp phù hợp.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Để tạo hàm ý, người ta dùng những cách thức nào?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Chủ ý vi phạm phương châm về lượng trong giao tiếp (nói thừa hoặc thiếu thông tin).",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Chủ ý vi phạm phương châm về cách thức (nói vòng vo, mập mờ).",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Sử dụng các hành động nói gián tiếp.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Cả 3 phương án trên.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Bài <b>Khái quát văn học Việt Nam từ CM-8 - 1945 đến hết thế kỉ XX</b> thuộc văn bản khoa học nào?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Văn bản khoa học phổ cập.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Văn bản khoa học chuyên sâu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Văn bản khoa học giáo khoa.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Văn bản khác.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Biểu hiện rõ nhất của tính khuôn mẫu của một số văn bản hành chính là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Kết cấu ba phần.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Các thông tin được in sẵn theo mẫu chung.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Người viết không được tự ý thêm bớt thông tin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Không thể thay đổi kết cấu.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đặc điểm nào sau đây sẽ quyết định đến vai (vị thế giao tiếp) của nhân vật giao tiếp?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Trình độ văn hóa.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Độ tuổi, giới tính.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Nghề nghiệp, vốn sống.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tất cả các đặc điểm.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Phép điệp nào sau đây không thuộc tu từ ngữ âm?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Điệp vần.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Điệp âm.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Điệp cú pháp.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Điệp thanh.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tiếng Việt là gì?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Là tiếng nói và chữ viết của dân tộc Việt, được dùng làm ngôn ngữ giao tiếp chung.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Là tiếng nói của người Việt.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Là chữ viết của người Việt.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Là chữ viết của dân tộc.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Không pha tạp, lai căng tiếng Việt có nghĩa là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Không sử dụng bất cứ tiếng nước ngoài nào trong nói năng, giao tiếp.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Chỉ sử dụng ngôn ngữ nước ngoài khi giao tiếp với người nước ngoài.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Không sử dụng tùy tiện, không cần thiết những yếu tố của ngôn ngữ khác.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Chỉ sử dụng trong giao tiếp những từ ngữ nước ngoài đã quen thuộc, thông dụng.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Chủ ý vi phạm phương châm về lượng trong giao tiếp nghĩa là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Nói thiếu lượng thông tin cần thiết (1).",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Nói thừa lượng thông tin cần thiết (2).",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Cung cấp quá nhiều thông tin trong một câu nói (3).",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Đáp án 1 và 3.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Chiến lược giao tiếp bao gồm:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Việc lựa chọn đề tài, nội dung giao tiếp.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Việc lựa chọn phương tiện ngôn ngữ, cách thức giao tiếp.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Thứ tự nói (viết).",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tất cả các đáp án.",
                            DSS = true
                        }
                    }
                }
            };
        }

        private List<Question> SeedQuestionsVanHocAndDifficultLevel5()
        {
            var type = (int)QuestionType.SingleChoice;
            var difficultLevel = 5;

            return new List<Question>
            {
                new Question
                {
                    Content = "Bút pháp tiêu biểu của bài thơ Tây Tiến là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Trào phúng.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Hiện thực.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Lãng mạn.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Lãng mạn.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Phải hiểu thế nào về việc Quang Dũng viết về cái chết của người lính Tây Tiến theo cách sau:</p><p><i>Áo bào thay chiếu anh về đất</i></p><p><i>Sông Mã gầm lên khúc độc hành</i></p>",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nội dung nào sau đây đúng với bài thơ <b>Tây Tiến</b> của Quang Dũng?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Bài thơ thể hiện khát vọng về với những sâu nặng nghĩa tình trong cuộc kháng chiến chống Pháp, về với ngọn nguồn cảm hứng sáng tác.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Bài thơ là một bản quyết tâm thư, là lời thề hành động của chiến sĩ trẻ, đồng thời thể hiện khát khao rạo rực, mong được về với cuộc sống tự do.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Bài thơ là cảm xúc và suy tư về đất nước đau thương nhưng anh dũng kiên cường đứng lên chiến đấu và chiến thắng trong kháng chiến chống thực dân Pháp.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Bài thơ là bức tranh hoang vu, kỳ vĩ, hấp dẫn của thiên nhiên Tây Bắc, là nỗi nhớ khôn nguôi, là khúc hoài niệm, là một dư âm không dứt về cuộc đời chiến binh.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Câu thơ: <b>Đêm mơ Hà Nội dáng kiều thơm</b> trong bài <b>Tây Tiến</b> của Quang Dũng diễn tả:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Khát khao mãnh liệt được trở về gặp mặt người yêu của người lính.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Một cách tinh tế, chân thực tâm lí của những người lính trẻ thủ đô hào hoa, mơ mộng.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Sự yếu lòng của người lính Tây Tiến khi làm nhiệm vụ ở vùng biên cương hẻo lánh, luôn nhung nhớ về dáng hình người yêu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tâm trạng xót thương cho người yêu đang mòn mỏi đợi chờ của những người lính trong đoàn quân Tây Tiến.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Hai chữ <b>về đất</b> trong câu: <b>Áo bào thay chiếu anh về đất</b> không gợi ý liên tưởng nào sau đây?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Sự thanh thản, ung dung của người lính sau khi đã tận trung với nước.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Cách nói giảm để tránh sự đau thương.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Sự hi sinh âm thầm không ai biết đến.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Sự hi sinh của người lính là hóa thân vào non sông đất nước.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tác phẩm nào không ra đời cùng năm với bài thơ <b>Tây Tiến</b> của Quang Dũng?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Đồng chí (Chính Hữu).",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Bên kia sông Đuống (Hoàng Cầm).",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Đôi mắt (Nam Cao).",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Việt Bắc (Tố Hữu).",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Hình ảnh trong câu thơ: <b>Kìa em xiêm áo tự bao giờ</b> là hình ảnh của:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Người yêu trong tâm tưởng, mong nhớ của lính Tây Tiến.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Các cô gái dân tộc nơi đoàn quân Tây Tiến đóng quân.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "<b>Dáng Kiều thơm<b> của những kiều nữ Hà Nội chập chờn hiện về trong <b>đêm mơ</b> của lính Tây Tiến.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Người sơn nữ mà tình cờ lính Tây Tiến gặp được trên đường hành quân giữa núi rừng Tây Bắc.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đoàn quân Tây Tiến được thành lập vào năm nào?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "1946",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "1945",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "1947",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "1948",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Địa danh nào không được nhắc tới trong bài thơ Tây Tiến của Quang Dũng?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Châu Mộc.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Mường Hịch.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Nà Ngần.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Pha Luông.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nhà thơ Quang Dũng sinh năm:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "1923.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "1921.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "1925.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "1920.",
                            DSS = false
                        }
                    }
                }
            };
        }

        private List<Question> SeedQuestionsSinhHocAndDifficultLevel2()
        {
            var type = (int)QuestionType.SingleChoice;
            var difficultLevel = 2;

            return new List<Question>
            {
                new Question
                {
                    Content = "Giả sử một gen được cấu tạo từ 3 loại nuclêôtit: A, T, G thì trên mạch gốc của gen này có thể có tối đa bao nhiêu loại mã bộ ba?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "6 loại mã bộ ba.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "3 loại mã bộ ba.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "27 loại mã bộ ba.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "9 loại mã bộ ba.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Ở sinh vật nhân thực, trình tự nuclêôtit trong vùng mã hóa của gen nhưng không mã hóa axit amin được gọi là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Đoạn intron.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Đoạn êxôn.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Gen phân mảnh.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Vùng vận hành.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Vùng điều hoà là vùng:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Quy định trình tự sắp xếp các axit amin trong phân tử prôtêin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Mang tín hiệu khởi động và kiểm soát quá trình phiên mã.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Mang thông tin mã hoá các axit amin.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Mang tín hiệu kết thúc phiên mã.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trong 64 bộ ba mã di truyền, có 3 bộ ba không mã hoá cho axit amin nào. Các bộ ba đó là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "UGU, UAA, UAG.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "UUG, UGA, UAG.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "UAG, UAA, UGA.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "UUG, UAA, UGA.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trong quá trình nhân đôi ADN, vì sao trên mỗi chạc tái bản có một mạch được tổng hợp liên tục còn mạch kia được tổng hợp gián đoạn?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Vì enzim ADN polimeraza chỉ tổng hợp mạch mới theo chiều 5’→3’.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Vì enzim ADN polimeraza chỉ tác dụng lên một mạch.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Vì enzim ADN polimeraza chỉ tác dụng lên mạch khuôn 3’→5’.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Vì enzim ADN polimeraza chỉ tác dụng lên mạch khuôn 5’→3’.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Mã di truyền có tính đặc hiệu, tức là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tất cả các loài đều dùng chung một bộ mã di truyền.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Mã mở đầu là AUG, mã kết thúc là UAA, UAG, UGA.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Nhiều bộ ba cùng xác định một axit amin.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Một bộ ba mã hoá chỉ mã hoá cho một loại axit amin.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tất cả các loài sinh vật đều có chung một bộ mã di truyền, trừ một vài ngoại lệ, điều này biểu hiện đặc điểm gì của mã di truyền?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Mã di truyền có tính đặc hiệu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Mã di truyền có tính thoái hóa.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Mã di truyền có tính phổ biến.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Mã di truyền luôn là mã bộ ba.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Gen không phân mảnh có:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Cả exôn và intrôn.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Vùng mã hoá không liên tục.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Vùng mã hoá liên tục.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Các đoạn intrôn.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Một đoạn của phân tử ADN mang thông tin mã hoá cho một chuỗi pôlipeptit hay một phân tử ARN được gọi là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Codon.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Gen.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Anticodon.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Mã di truyền.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Quá trình nhân đôi ADN được thực hiện theo nguyên tắc gì?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Hai mạch được tổng hợp theo nguyên tắc bổ sung song song liên tục.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Một mạch được tổng hợp gián đoạn, một mạch được tổng hợp liên tục.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Nguyên tắc bổ sung và nguyên tắc bán bảo toàn.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Mạch liên tục hướng vào, mạch gián đoạn hướng ra chạc ba tái bản.",
                            DSS = false
                        }
                    }
                }
            };
        }

        private List<Question> SeedQuestionsSinhHocAndDifficultLevel3()
        {
            var type = (int)QuestionType.SingleChoice;
            var difficultLevel = 3;

            return new List<Question>
            {
                new Question
                {
                    Content = "Bản chất của mã di truyền là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Trình tự sắp xếp các nulêôtit trong gen quy định trình tự sắp xếp các axit amin trong prôtêin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Các axit amin đựơc mã hoá trong gen.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Ba nuclêôtit liền kề cùng loại hay khác loại đều mã hoá cho một axit amin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Một bộ ba mã hoá cho một axit amin.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Vùng kết thúc của gen là vùng:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Mang tín hiệu khởi động và kiểm soát quá trình phiên mã.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Mang tín hiệu kết thúc phiên mã.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Quy định trình tự sắp xếp các aa trong phân tử prôtêin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Mang thông tin mã hoá các aa.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Mã di truyền mang tính thoái hoá, tức là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Nhiều bộ ba khác nhau cùng mã hoá cho một loại axit amin",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Tất cả các loài đều dùng chung nhiều bộ mã di truyền",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tất cả các loài đều dùng chung một bộ mã di truyền",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Một bộ ba mã di truyền chỉ mã hoá cho một axit amin",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Mã di truyền có tính phổ biến, tức là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tất cả các loài đều dùng chung nhiều bộ mã di truyền.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Nhiều bộ ba cùng xác định một axit amin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Một bô ba mã di truyền chỉ mã hoá cho một axit amin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tất cả các loài đều dùng chung một bộ mã di truyền, trừ một vài loài ngoại lệ.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Mỗi ADN con sau nhân đôi đều có một mạch của ADN mẹ, mạch còn lại được hình thành từ các nuclêôtit tự do. Đây là cơ sở của nguyên tắc:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Bổ sung.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Bán bảo toàn.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Bổ sung và bảo toàn.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Bổ sung và bán bảo toàn.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Mỗi gen mã hoá prôtêin điển hình gồm các vùng theo trình tự là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Vùng điều hoà, vùng vận hành, vùng mã hoá.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Vùng điều hoà, vùng mã hoá, vùng kết thúc.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Vùng điều hoà, vùng vận hành, vùng kết thúc.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Vùng vận hành, vùng mã hoá, vùng kết thúc.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Gen là một đoạn của phân tử ADN",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Mang thông tin mã hoá chuỗi polipeptit hay phân tử ARN.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Mang thông tin di truyền của các loài.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Mang thông tin cấu trúc của phân tử prôtêin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Chứa các bộ 3 mã hoá các axit amin.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Vùng nào của gen quyết định cấu trúc phân tử protêin do nó quy định tổng hợp?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Vùng kết thúc.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Vùng điều hòa.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Vùng mã hóa.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Cả ba vùng của gen.",
                            DSS = false
                        }
                    }
                }
            };
        }

        private List<Question> SeedQuestionsSinhHocAndDifficultLevel4()
        {
            var type = (int)QuestionType.SingleChoice;
            var difficultLevel = 4;

            return new List<Question>
            {
                new Question
                {
                    Content = "Trong quá trình nhân đôi ADN, các đoạn Okazaki được nối lại với nhau thành mạch liên tục nhờ enzim nối, enzim nối đó là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "ADN giraza.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "ADN pôlimeraza.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Hêlicaza",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "ADN ligaza",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Một gen có 480 ađênin và 3120 liên kết hiđrô. Gen đó có số lượng nuclêôtit là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "1800.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "2400.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "3000.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "2040.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Intron là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Đoạn gen mã hóa axit amin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Đoạn gen không mã hóa axit amin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Gen phân mảnh xen kẽ với các êxôn.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Đoạn gen mang tính hiệu kết thúc phiên mã.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Vai trò của enzim ADN pôlimeraza trong quá trình nhân đôi ADN là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tháo xoắn phân tử ADN.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Lắp ráp các nuclêôtit tự do theo nguyên tắc bổ sung với mỗi mạch khuôn của ADN.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Bẻ gãy các liên kết hiđrô giữa hai mạch của ADN.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Nối các đoạn Okazaki với nhau.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Vùng mã hoá của gen là vùng:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Mang tín hiệu khởi động và kiểm soát phiên mã.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Mang tín hiệu kết thúc phiên mã.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Mang tín hiệu mã hoá các axit amin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Mang bộ ba mở đầu và bộ ba kết thúc.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nhiều bộ ba khác nhau có thể cùng mã hóa một axit amin trừ AUG và UGG, điều này biểu hiện đặc điểm gì của mã di truyền?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Mã di truyền có tính phổ biến.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Mã di truyền có tính đặc hiệu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Mã di truyền luôn là mã bộ ba.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Mã di truyền có tính thoái hóa.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đơn vị mang thông tin di truyền trong ADN được gọi là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Nuclêôtit.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Bộ ba mã hóa.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Triplet.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Gen.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đơn vị mã hoá thông tin di truyền trên ADN được gọi là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Gen.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Codon.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Triplet.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Axit amin.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Mã di truyền là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Mã bộ một, tức là cứ một nuclêôtit xác định một loại axit amin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Mã bộ bốn, tức là cứ bốn nuclêôtit xác định một loại axit amin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Mã bộ ba, tức là cứ ba nuclêôtit xác định một loại axit amin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Mã bộ hai, tức là cứ hai nuclêôtit xác định một loại axit amin.",
                            DSS = false
                        }
                    }
                }
            };
        }

        private List<Question> SeedQuestionsSinhHocAndDifficultLevel5()
        {
            var type = (int)QuestionType.SingleChoice;
            var difficultLevel = 5;

            return new List<Question>
            {
                new Question
                {
                    Content = "Quá trình phiên mã ở vi khuẩn E.coli xảy ra trong",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Ribôxôm.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Tế bào chất.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Nhân tế bào.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Ti thể.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Làm khuôn mẫu cho quá trình phiên mã là nhiệm vụ của:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Mạch mã hoá.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "mARN.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Mạch mã gốc.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "tARN.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đơn vị được sử dụng để giải mã cho thông tin di truyền nằm trong chuỗi polipeptit là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Anticodon.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Axit amin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Codon.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Triplet.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đặc điểm nào dưới đây thuộc về cấu trúc của mARN?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "mARN có cấu trúc mạch kép, dạng vòng, gồm 4 loại đơn phân A, T, G, X.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "mARN có cấu trúc mạch kép, gồm 4 loại đơn phân A, T, G, X.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "mARN có cấu trúc mạch đơn, gồm 4 loại đơn phân A, U, G, X.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "mARN có cấu trúc mạch đơn, dạng thẳng, gồm 4 loại đơn phân A, U, G, X.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Quá trình phiên mã xảy ra ở",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Sinh vật nhân chuẩn, vi khuẩn.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Sinh vật có ADN mạch kép.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Sinh vật nhân chuẩn, vi rút.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Vi rút, vi khuẩn.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trong quá trình dịch mã, mARN thường gắn với một nhóm ribôxôm gọi là poliribôxôm giúp",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tăng hiệu suất tổng hợp prôtêin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Điều hoà sự tổng hợp prôtêin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tổng hợp các prôtêin cùng loại.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tổng hợp được nhiều loại prôtêin.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đối mã đặc hiệu trên phân tử tARN được gọi là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Codon.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Axit amin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Anticodon.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Triplet.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "ARN được tổng hợp từ mạch nào của gen?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Từ mạch có chiều 5’ → 3’.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = " Từ cả hai mạch đơn.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Khi thì từ mạch 1, khi thì từ mạch 2.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Từ mạch mang mã gốc.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Loại axit nuclêic tham gia vào thành phần cấu tạo nên ribôxôm là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "rARN.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "mARN.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "tARN.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "ADN.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Ở cấp độ phân tử nguyên tắc khuôn mẫu được thể hiện trong cơ chế",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tự sao, tổng hợp ARN, dịch mã.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Tổng hợp ADN, dịch mã.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tự sao, tổng hợp ARN.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tổng hợp ADN, ARN.",
                            DSS = false
                        }
                    }
                }
            };
        }

        private List<Question> SeedQuestionsLichSuAndDifficultLevel2()
        {
            var type = (int)QuestionType.SingleChoice;
            var difficultLevel = 2;

            return new List<Question>
            {
                new Question
                {
                    Content = "Đầu năm 1945, Chiến tranh thế giới thứ hai bước vào giai đoạn cuối thắng lợi thuộc về",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Phe Đồng minh.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Các lực lượng dân chủ tiến bộ.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Mĩ và Liên Xô.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Anh và Pháp.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Hội nghị cấp cao ở Ianta sau Chiến tranh thế giới thứ hai kéo dài",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "8 ngày.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "9 ngày.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "10 ngày.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "11 ngày.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đại diện cho Liên Xô tham dự Hội nghị cấp cao ở Ianta là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Thủ tướng Stalin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Chủ tịch Hội đồng Bộ trưởng Stalin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tổng thống Stalin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Chủ tịch Ủy ban Quân đội Stalin.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nước nào sau đây không có mặt ở Hội nghị cấp cao ở Ianta?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Anh.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Mĩ.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Pháp.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Liên Xô.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Để nhanh chóng kết thúc chiến tranh ở châu Á, Hội nghị Ianta đã",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Phân công Pháp và Anh phản công tiến đánh Nhật Bản.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Quyết định Liên Xô tham chiến chống Nhật trước khi chiến tranh kết thúc ở châu Âu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Quyết định Liên Xô tham chiến chống Nhật khi chiến tranh đang diễn ra ở châu Âu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Quyết định Liên Xô tham chiến chống Nhật sau khi chiến tranh kết thúc ở châu Âu.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Phạm vi nào không thuộc ảnh hưởng của Liên Xô?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Đông Đức.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Đông Âu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Đông Bec – Lin.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tây Đức.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Theo thỏa thuận tại Hội nghị Ianta thì hai nước trở thành trung lập là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Pháp và Phần Lan.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Áo và Phần Lan",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Áo và Hà Lan.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Phần Lan và Thổ Nhĩ Kì.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Theo những quyết định của Hội nghị Ianta về phân chia khu vực chiếm đóng, Mĩ không có quyền lợi ở:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Italia.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Nhật Bản.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Trung Quốc.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Bắc Triều Tiên.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Hội nghị Postđam diễn ra vào",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "17/7/1945.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "18/7/1945.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "19/7/1945.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "21/7/1945.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tham dự Hội nghị Postđam gồm bao nhiêu nước?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "3",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "4",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "6",
                            DSS = false
                        }
                    }
                }
            };
        }

        private List<Question> SeedQuestionsLichSuAndDifficultLevel3()
        {
            var type = (int)QuestionType.SingleChoice;
            var difficultLevel = 3;

            return new List<Question>
            {
                new Question
                {
                    Content = "Liên hợp quốc là cơ quan",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "An ninh, đối ngoại của các nước thắng trận.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "dDuy trì hòa binh, an ninh ở cấp độ khu vực.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Được thành lập từ ngày 24/10/1945.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Quyền lực, mang tính quốc tế sâu sắc.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trụ sở của Liên hợp quốc đặt tại",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Paris.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "London.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "New York.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Đức.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Bản Hiến chương Liên hợp quốc có hiệu lực từ ngày",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "24/10/1945",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "25/10/1945",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "26/10/1945",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "27/10/1945",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Để thông qua bản Hiến chương và tuyên bố thành lập Liên hợp quốc, Hội nghị tại Xan Phranxixcô đã diễn ra với sự tham gia của",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "45 nước.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "50 nước.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "55 nước.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "60 nước.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nguyên tắc cơ bản nhất để chỉ đạo hoạt động của Liên hợp quốc là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tôn trọng toàn vẹn lãnh thổ và độc lập chính trị của tất cả các nước.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Không can thiệp vào công việc nội bộ của bất kì nước nào.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Chung sống hòa bình và có sự nhất trí giữa 5 cường quốc lớn.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tôn trọng quyền bình đẳng và chủ quyền giữa các quốc gia và quyền tự quyết của các dân tộc.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Câu nào sau đây sai khi nói về Đại hội đồng Liên hợp quốc?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Là cơ quan lớn nhất, đứng đầu Liên hợp Quốc, giám sát các hoạt động của Hội đồng bảo an.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Họp mỗi năm một kì để thảo luận các công việc thuộc phạm vi Hiến chương quy định.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Đối với những vấn đề quan trọng, Hội nghị quyết định theo nguyên tắc đa số hai phần ba hoặc quá bán.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Hội nghị dành cho tất cả các nước thành viên.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đâu là nhận xét sai khi nói về Hội đồng bảo an Liên hợp quốc?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Là cơ quan chính trị, quan trọng nhất, hoạt động thường xuyên.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Chịu trách nhiệm duy trì hòa bình và an ninh thế giới.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Chịu sự giám sát và chi phối của Đại hội đồng.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Có 5 Ủy viên thường trực.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Ban thư kí Liên hợp có nhiệm kì",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "3 năm.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "2 năm.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "1 năm.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "5 năm.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Ban thư kí do ai bầu?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Hội đồng bảo an",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Đại hội đồng.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tổng thư kí.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Ban quản thác.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Việt Nam gia nhập Liên hợp vào ngày",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "21/9/1976",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "20/9/1977",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "21/9/1977",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "20/9/1976",
                            DSS = false
                        }
                    }
                }
            };
        }

        private List<Question> SeedQuestionsLichSuAndDifficultLevel4()
        {
            var type = (int)QuestionType.SingleChoice;
            var difficultLevel = 4;

            return new List<Question>
            {
                new Question
                {
                    Content = "Việt Nam là thành viên thứ mấy của Liên hợp quốc?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "149.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "150.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "151.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "152.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Việt Nam là ủy viên không thường trực của Hội đồng bảo an Liên hợp quốc vào năm",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "2006.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "2007.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "2008.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "2009.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nhiệm kì mà Việt Nam đảm nhiệm khi là Ủy viên không thường trực của Hội đồng bảo an là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "1 năm.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "2 năm.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "3 năm.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "4 năm.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đâu là tên viết tắt của tổ chức Liên hợp quốc?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "UNP.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "UN.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "LAO.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "IFC.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Năm 1991, số thành viên của Liên hợp quốc là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "168.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "191.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "172.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "194.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đến ngày 31/5/2000, Liên hợp quốc có bao nhiêu hội viên?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "188.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "191.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "168.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "172.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "ECOSOC là tên gọi của",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Hội đồng hàng không.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Hội đồng kinh tế và xã hội.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Hội đồng lương thực nông nghiệp.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Ban thư kí Liên hợp quốc.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trật tự hai cực Ianta đã chi phối đến",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Kinh tế.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Quân sự.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tư tưởng.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tất cả ý trên.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Liên hợp quốc có mấy cơ quan chủ yếu?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "4.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "5.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "6.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "7.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Hạn chế lớn nhất của tổ chức Liên hợp quốc hiện nay là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Quan liêu, tham nhũng ngày càng gia tăng.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Hệ thống nội bộ chia rẻ.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Chưa giải quyết tốt các vấn đề dịch bệnh, thiên tai, viện trợ kinh tế đối với các nước thành viên nghèo khó.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Chưa có những quyết định phù hợp đối với những sự việc ở Trung Đông, châu Âu, Irắc, ...",
                            DSS = false
                        }
                    }
                }
            };
        }

        private List<Question> SeedQuestionsLichSuAndDifficultLevel5()
        {
            var type = (int)QuestionType.SingleChoice;
            var difficultLevel = 5;

            return new List<Question>
            {
                new Question
                {
                    Content = "Hội nghị Ianta đã có những quyết định nào đối với Trung Hoa Dân quốc?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Quy định Trung Quốc cần phải trở thành một quốc gia thống nhất và dân chủ.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Cải tổ chính phủ với sự tham gia của Đảng cộng sản và các đảng phái dân chủ.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Trả lại cho Trung Quốc vùng Mãn Châu, đảo Đài Loan và quần đảo Bành Hồ.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tất cả ý trên.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Vấn đề nước Đức được hội nghị Postđam được quy định như thế nào?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Phân chia khu vực chiếm đóng và kiểm soát nước Đức sau chiến tranh giữa các nước lớn.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Thống nhất mục tiêu tiêu diệt tận gốc chủ nghĩa phát xít Đức.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Khẳng định nước Đức trở thành một quốc gia hòa bình và thống nhất.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tất cả ý trên.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nước Cộng hòa Liên bang Đức được thành lập vào thời gian nào?",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tháng 9/1949.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Tháng 9/1948.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tháng 8/1948.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tháng 10/1949.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Sau khi chiến tranh thế giới thứ hai kết thúc, một trật tự thế giới mới đã được hình thành với đặc trưng lớn là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Thế giới chia làm 2 phe XHCN và TBCN do Liên Xô và Mĩ đứng đầu mỗi phe.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Mĩ và Liên Xô ra sức chạy đua vũ trang.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Thế giới chìm trong <b>Chiến tranh lạnh</b> do Mĩ phát động.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Loài người đứng trước thảm hoạ <b>đung đưa trên miệng hố chiến tranh</b>.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Vai trò quan trọng nhất của tổ chức Liên hợp quốc là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Duy trì hoà bình và an ninh quốc tế.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Thúc đẩy quan hệ hữu nghị hợp tác giữa tất cả các nước.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Giải quyết các vụ tranh chấp và xung đột khu vực.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Giúp đỡ các dân tộc về kinh tế, văn hoá, giáo dục, y tế, nhân đạo..",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Ngay sau khi Chiến tranh thế giới thứ hai kết thúc, vấn đề trung tâm trong nhiều cuộc gặp gỡ giữa nguyên thủ 3 cường quốc Liên Xô, Mĩ, Anh với những bất đồng sâu sắc, đó là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Vấn đề tương lai nước Nhật.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Vấn đề tương lai của Triều Tiên.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Vấn đề tương lai nước Đức.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Vấn đề tương lai của châu Âu.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Mọi quyết định của Hội đồng Bảo an phải được sự nhất trí của 5 nước uỷ viên thường trực là:",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Mĩ, Anh, Pháp, Đức, Nhật Bản.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Liên Xô (Liên bang Nga), Trung Quốc, Mĩ, Anh, Nhật.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Liên Xô (Liên bang Nga), Đức, Mĩ, Anh, Trung Quốc.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Liên Xô (Liên bang Nga), Trung Quốc, Mĩ, Anh, Pháp.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nguyên thủ 3 quốc gia Liên Xô, Mĩ, Anh đến Hội nghị Ianta với công việc trọng tâm là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Nhanh chóng đánh bại hoàn toàn các nước phát xít.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Thành lập tổ chức Liên hợp quốc để giữ gìn hoà bình và an ninh thế giới.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Phân chia thành quả chiến thắng giữa các nước thắng trận.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Bàn biện pháp kết thúc sớm Chiến tranh thế giới thứ hai.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trong các quyết định của Hội nghị Ianta, quyết định đưa đến sự phân chia hai cực trong quan hệ quốc tế là",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tiêu diệt tận gốc chủ nghĩa phát xít Đức và chủ nghĩa quân phiệt Nhật.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Liên Xô tham gia chống Nhật ở Châu Á.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Thành lập Liên hợp quốc để giữ gìn hoà bình và an ninh thế giới.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Thoả thuận việc đóng quân, phân chia phạm vi ảnh hưởng ở châu Âu và châu Á.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Theo thỏa thuận của cường quốc tại Hội nghị Ianta, Đông Nam Á thuộc phạm vi ảnh hưởng của",
                    Type = type,
                    DifficultLevel = difficultLevel,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Các nước Đông Âu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Các nước Tây Âu.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Mĩ, Anh và Liên Xô.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Đức, Pháp và Nhật Bản.",
                            DSS = false
                        }
                    }
                }
            };
        }

        #endregion
    }
}
