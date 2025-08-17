using Prescripto.DTOs.TTACDtos;
using Prescripto.Models;

namespace Prescripto.Services
{
    public interface ITTACAriaService
    {
        List<Fard> facgroups(DbConfig connection);

        List<Fard> getAnbar(DbConfig connection);
        List<TTAcKalaAria> tTAcKalas(DbConfig connection,List<ListTTAcKala> listTTAcKalas);

        bool updateKala(DbConfig connection, int codeKala, string irc);

        int insertfactors(DbConfig connection, FachederFactor fachederFactor);
    }
}
