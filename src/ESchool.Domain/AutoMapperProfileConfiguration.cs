using AutoMapper;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.ViewModels.Examinations;

namespace ESchool.Domain
{
    public sealed class AutoMapperProfileConfiguration : Profile
	{
		public AutoMapperProfileConfiguration()
		{
			ViewModelsToModels();
			ModelsToDTOs();
		}

		private void ViewModelsToModels()
		{
            CreateMap<QTagCreateViewModel, QTag>()
                .ForMember(vm => vm.Name, map => map.MapFrom(s => s.Name.Trim()))
                .ForMember(vm => vm.Description, map => map.MapFrom(s => TrimNull(s.Description)));
            CreateMap<QuestionCreateViewModel, Question>()
                .ForMember(vm => vm.Content, map => map.MapFrom(s => s.Content.Trim()))
                .ForMember(vm => vm.Description, map => map.MapFrom(s => TrimNull(s.Description)));
            //CreateMap<CreateMenuViewModel, Menu>()
            //	.ForMember(vm => vm.Name, map => map.MapFrom(s => s.Name.Trim()))
            //	.ForMember(vm => vm.Description, map => map.MapFrom(s => s.Description.TrimNull()))
            //	.ForMember(vm => vm.IconName, map => map.MapFrom(s => s.IconName.TrimNull()));

            //CreateMap<Schedule, ScheduleViewModel>()
            //   .ForMember(vm => vm.Creator,
            //        map => map.MapFrom(s => s.Creator.Name))
            //   .ForMember(vm => vm.Attendees, map =>
            //        map.MapFrom(s => s.Attendees.Select(a => a.UserId)));

            //CreateMap<Schedule, ScheduleDetailsViewModel>()
            //   .ForMember(vm => vm.Creator,
            //        map => map.MapFrom(s => s.Creator.Name))
            //   .ForMember(vm => vm.Attendees, map =>
            //        map.UseValue(new List<UserViewModel>()))
            //    .ForMember(vm => vm.Status, map =>
            //        map.MapFrom(s => ((ScheduleStatus)s.Status).ToString()))
            //    .ForMember(vm => vm.Type, map =>
            //       map.MapFrom(s => ((ScheduleType)s.Type).ToString()))
            //   .ForMember(vm => vm.Statuses, map =>
            //        map.UseValue(Enum.GetNames(typeof(ScheduleStatus)).ToArray()))
            //   .ForMember(vm => vm.Types, map =>
            //        map.UseValue(Enum.GetNames(typeof(ScheduleType)).ToArray()));

            //CreateMap<User, UserViewModel>()
            //    .ForMember(vm => vm.SchedulesCreated,
            //        map => map.MapFrom(u => u.SchedulesCreated.Count()));
        }

        private void ModelsToDTOs()
		{
			//CreateMap<Restaurant, AdminRestaurantDto>();
            //CreateMap<Restaurant, RestaurantDto>();
            //CreateMap<Address, AdminAddressDto>();
			//CreateMap<Menu, AdminHierarchyMenuDto>();

			//CreateMap<Schedule, ScheduleViewModel>()
			//   .ForMember(vm => vm.Creator,
			//        map => map.MapFrom(s => s.Creator.Name))
			//   .ForMember(vm => vm.Attendees, map =>
			//        map.MapFrom(s => s.Attendees.Select(a => a.UserId)));

			//CreateMap<Schedule, ScheduleDetailsViewModel>()
			//   .ForMember(vm => vm.Creator,
			//        map => map.MapFrom(s => s.Creator.Name))
			//   .ForMember(vm => vm.Attendees, map =>
			//        map.UseValue(new List<UserViewModel>()))
			//    .ForMember(vm => vm.Status, map =>
			//        map.MapFrom(s => ((ScheduleStatus)s.Status).ToString()))
			//    .ForMember(vm => vm.Type, map =>
			//       map.MapFrom(s => ((ScheduleType)s.Type).ToString()))
			//   .ForMember(vm => vm.Statuses, map =>
			//        map.UseValue(Enum.GetNames(typeof(ScheduleStatus)).ToArray()))
			//   .ForMember(vm => vm.Types, map =>
			//        map.UseValue(Enum.GetNames(typeof(ScheduleType)).ToArray()));

			//CreateMap<User, UserViewModel>()
			//    .ForMember(vm => vm.SchedulesCreated,
			//        map => map.MapFrom(u => u.SchedulesCreated.Count()));
		}

        private string TrimNull(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? value : value.Trim();
        }
    }
}
