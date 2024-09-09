using HieuTM.ConsoleUI.Models;

namespace HieuTM.ConsoleUI.UI
{
    internal static class SinhVienUI
    {
        public static void Show(this SinhVienVM vm)
        {
            Console.WriteLine($"{vm.FullName} - {vm.DOB.ToString("dd-MM-yyyy")}");
        }
    }
}
