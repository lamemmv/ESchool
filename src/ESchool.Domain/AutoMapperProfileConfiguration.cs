using AutoMapper;

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
            
        }

        private void ModelsToDTOs()
		{
			
		}

        private string TrimNull(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? value : value.Trim();
        }
    }
}
