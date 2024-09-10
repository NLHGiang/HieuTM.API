using HieuTM.ConsoleUI.UI;

SinhVienUIService sinhVienUIService = new();
while (true)
{
    string choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            await sinhVienUIService.Get();
            break;
        case "2":
            await sinhVienUIService.Post();
            break;
        case "3":
            break;
        case "4":
            break;
        default:
            break;
    }
}