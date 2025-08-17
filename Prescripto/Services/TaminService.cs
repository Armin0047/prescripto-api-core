using Prescripto.DTOs;
using Prescripto.Models;
using Prescripto.Repositories;

namespace Prescripto.Services
{
    public class TaminService :ITaminService
    {
        private readonly ITaminRepository _repository;
        private readonly DbConnectionInfo _connectionService;
        public TaminService(ITaminRepository repository, DbConnectionInfo connectionService)
        {
            _repository = repository;
            _connectionService = connectionService;
        }
        public List<Sazeman> Sazeman(DbConfig connection)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                List<Sazeman> sazeman = new List<Sazeman>();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                sazeman = _repository.GetSazemanList(con);
                return sazeman;
            }
            return null;
        }

        public bool updateKala(DbConfig connection, int codeKala, string CodeKalaNew, string Gencode)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                var df = _repository.SetUpdateKala(con, codeKala, CodeKalaNew, Gencode);
                return df;
            }
            return false;
        }

        public ExistsNoskhe SabtNoskheTamin(DbConfig connection, ListKalaNoskhe taminHeder)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                ExistsNoskhe noskhe = new ExistsNoskhe();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                noskhe = _repository.InsertFactor(con, taminHeder);
                return noskhe;
            }
            return null;
        }
    }
}
