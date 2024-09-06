using AutoMapper;
using HieuTM.API.B1.Entities;
using HieuTM.API.B1.ViewModels.SinhViens;

namespace HieuTM.API.B1.MapperProfiles
{
    public class SinhVienProfile : Profile
    {
        public SinhVienProfile()
        {
            CreateMap<SinhVien, SinhVienVM>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => (string.IsNullOrWhiteSpace(src.FullName)) ?
                                                "N/A" :
                                                src.FullName))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => CalculateAge(src.DOB)));
        }

        private int CalculateAge(DateTime dob)
        {
            int ageValid = 18;
            int actualAge = DateTime.Now.Year - dob.Year;
            if (actualAge > ageValid) return -1;
            else return actualAge;
        }
    }
}