using Prescripto.Models;
using Prescripto.DTOs;

namespace Prescripto.Services
{
    public interface ITaminService
    {
        List<Sazeman> Sazeman(DbConfig connection);
        bool updateKala(DbConfig connection, int codeKala, string CodeKalaNew, string Gencode);

        ExistsNoskhe SabtNoskheTamin(DbConfig connection, ListKalaNoskhe taminHeder);
    }

}
