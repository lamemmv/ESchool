﻿using System.Collections.Generic;
using ESchool.Data.Mapping.Examinations;
using ESchool.Data.Mapping.Files;
using ESchool.Data.Mapping.Logs;
using ESchool.Data.Mapping.Messages;
using ESchool.Data.Mapping.Settings;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping
{
    public sealed class ObjectEntityMapper : IEntityMapper
    {
        public IEnumerable<IEntityMap> Mappings
        {
            get
            {
                return new List<IEntityMap>()
                {
                    new LogMap(),
                    new SettingMap(),
                    new BlobMap(),
                    new EmailAccountMap(),
                    new QueuedEmailMap(),

                    new GroupMap(),
                    new QTagMap(),
                    new QuestionMap(),
                    new AnswerMap(),
                    new ExamPaperMap(),
                    new QuestionExamPaperMap(),
                    new StudentMap(),
                    new ExamMap(),
                    new StudentExamMap(),
                    new StudentExamPaperResultMap()
                };
            }
        }

        public void MapEntities(ModelBuilder modelBuilder)
        {
            foreach (IEntityMap map in Mappings)
            {
                map.Map(modelBuilder);
            }
        }
    }
}
