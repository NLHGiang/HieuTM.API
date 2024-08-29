using HieuTM.API.B1.ViewModels.SinhViens;

namespace HieuTM.API.B1.Services
{
    public interface ISinhVienService
    {
        List<SinhVienVM> GetList();
        SinhVienVM GetById(int id);
        bool Create(CreateSinhVienVM request);
        bool Update(UpdateSinhVienVM request);
        bool Delete(int id);
    }
}
