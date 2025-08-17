using Prescripto.DTOs;
using Prescripto.DTOs.TTACDtos;
using Prescripto.Models;

namespace Prescripto.Services
{
    public interface IPharmcyService
    {
        CoNameDto GetCoName(DbConfig connection, string username);
        ExistsNoskhe ExistsNoskhe(DbConfig connection, string WebTrackingCode, string ShDaftarcheh, string DateFac);
        int GetSettingMojodi(DbConfig connection);
        NameDr GetNameDr(DbConfig connection, int DocId);
        List<RadifKala> GetListKalaNoskhes(DbConfig connection, ListKalaNoskhe listKalaNoskhe);
        List<Takhasos> GetListTakhasos(DbConfig connection);
        int InsertDoctor(DbConfig connection, string Codedr, string NameDr, int codetakh);
        List<KalaSmal> kalaSmal(DbConfig connection, string CodeSazeman, int page , int size, string search );

        List<Fard> fards(DbConfig connection);

        int GetSettingKala(DbConfig connection,string Hname);

        int GetCheckCodeKala(DbConfig connection, string Hname);

        List<Fard> dastedaroo(DbConfig connection);

        List<Fard> druggroup(DbConfig connection);

        List<Fard> noekala(DbConfig connection);

        int LastCodeKala(DbConfig connection);
        int codekalawithCodegroup(DbConfig connection,int Codegroup);

        bool CreateKala(DbConfig connection, kala kala);

    }
}
