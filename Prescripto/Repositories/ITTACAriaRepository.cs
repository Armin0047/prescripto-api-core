using Prescripto.DTOs.TTACDtos;

namespace Prescripto.Repositories
{
    public interface ITTACAriaRepository
    {
        List<Fard> getfacFroup(string connection);

        List<Fard> getAnbarlist(string connection);

        List<TTAcKalaAria> selectKala(string connection, List<ListTTAcKala> listTTAcKalas);

        bool updateKala(string connection, int codeKala, string irc);
        int insertfactor(string connection, FachederFactor fachederFactor);
    }
}
