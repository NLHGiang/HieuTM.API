using AutoMapper;
using HieuTM.API.B1.Entities;
using HieuTM.API.B1.ViewModels.SinhViens;

namespace HieuTM.API.B1.Services
{
    public class SinhVienService : ISinhVienService
    {
        static List<SinhVien> _listSinhVien = new()
        {
            new SinhVien()
            {
                Id = 1,
                FullName = "NLHGiang",
                DOB = new DateTime(2002,6,9)
            },
            new SinhVien()
            {
                Id = 2,
                FullName = "GiangNLH",
                DOB = new DateTime(2003,6,9)
            },
            new SinhVien()
            {
                Id = 3,
                FullName = "        ",
                DOB = new DateTime(2020,6,9)
            }
        };

        private readonly ILogger<SinhVienService> _logger;
        private readonly IMapper _mapper;

        public SinhVienService(
            ILogger<SinhVienService> logger,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public List<SinhVienVM> GetList()
        {
            // Map List<SinhVien> -> List<SinhVienVM>
            List<SinhVienVM> vms = _mapper.Map<List<SinhVienVM>>(_listSinhVien);

            return vms;
        }

        public SinhVienVM GetById(int id)
        {
            var entity = _listSinhVien.FirstOrDefault(e => e.Id == id);

            // Map SinhVien -> SinhVienVM
            SinhVienVM vm = _mapper.Map<SinhVienVM>(entity);

            return vm;
        }

        public bool Create(CreateSinhVienVM request)
        {
            try
            {
                // Map from CreateSinhVienVM -> SinhVien
                SinhVien entity = new();

                _listSinhVien.Add(entity);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ex: {ex.Message}");
                return false;
            }
        }

        public bool Update(UpdateSinhVienVM request)
        {
            try
            {
                var entity = _listSinhVien.FirstOrDefault(e => e.Id == request.Id);
                _listSinhVien.Remove(entity);

                // Map from UpdateSinhVienVM -> SinhVien

                _listSinhVien.Add(entity);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ex: {ex.Message}");
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var entity = _listSinhVien.FirstOrDefault(e => e.Id == id);
                _listSinhVien.Remove(entity);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ex: {ex.Message}");
                return false;
            }
        }
    }
}
