using Prescripto.DTOs;

namespace Prescripto.Repositories
{
    public interface ITaminRepository
    {
        List<Sazeman> GetSazemanList(string Connection);
        bool SetUpdateKala(string connection, int codeKala, string CodeKalaNew, string Gencode);

        ExistsNoskhe InsertFactor(string connection, ListKalaNoskhe taminHeder);
    }
}
