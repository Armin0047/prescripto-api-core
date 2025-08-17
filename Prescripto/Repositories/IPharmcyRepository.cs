using Prescripto.DTOs;
using Prescripto.DTOs.TTACDtos;
using Prescripto.Models;

namespace Prescripto.Repositories
{
    public interface IPharmcyRepository
    {
        CoNameDto GetPharmcyName(string connection,string username);
        ExistsNoskhe GetExistsNoskhe(string connection, string WebTrackingCode, string ShDaftarcheh, string DateFac);
        int GetSettingMojodi(string connection);
        NameDr GetNameDr(string connection, int DocId);
        List<RadifKala> Getkalanoskhe(string connection, ListKalaNoskhe listKalaNoskhe);
        List<Takhasos> Gettakhasos(string connection);
        int InsertDr(string connection, string Codedr, string NameDr, int codetakh);
        List<KalaSmal> Getkalasmal(string connection, string CodeSazeman, int page, int size, string search);
        List<Fard> getFard(string connection);
        int GetSettingKala(string connection, string Hname);
        int GetCheckCodeKala(string connection, string Code);
        List<Fard> getdastedaroo(string connection);
        List<Fard> getdruggroup(string connection);
        List<Fard> getnoekala(string connection);
        int getLastCodeKala(string connection);
        int getcodekalawithCodegroup(string connection, int Codegroup);

        bool CreateKala(string connection, kala kala);
    }
}
