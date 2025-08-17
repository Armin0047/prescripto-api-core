using Prescripto.DTOs.TTACDtos;
using Prescripto.Models;
using Prescripto.Repositories;

namespace Prescripto.Services
{
    public class TTACAriaService : ITTACAriaService
    {
        private readonly ITTACAriaRepository _repository;
        private readonly DbConnectionInfo _connectionService;
        public TTACAriaService(ITTACAriaRepository repository, DbConnectionInfo connectionService)
        {
            _repository = repository;
            _connectionService = connectionService;
        }
        public List<Fard> facgroups(DbConfig connection)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                List<Fard> f = new List<Fard>();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                f = _repository.getfacFroup(con);
                return f;
            }
            return null;
        }
        public List<Fard> getAnbar(DbConfig connection)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                List<Fard> f = new List<Fard>();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                f = _repository.getAnbarlist(con);
                return f;
            }
            return null;
        }

        public List<TTAcKalaAria> tTAcKalas(DbConfig connection, List<ListTTAcKala> listTTAcKalas)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                List<TTAcKalaAria> listTTAcKala = new List<TTAcKalaAria>();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                listTTAcKala = _repository.selectKala(con, listTTAcKalas);
                return listTTAcKala;
            }
            return null;
        }

        public bool updateKala(DbConfig connection, int codeKala, string irc)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                var f = _repository.updateKala(con, codeKala, irc);
                return f;
            }
            return false;
        }

        public int insertfactors(DbConfig connection, FachederFactor fachederFactor)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                var f = _repository.insertfactor(con, fachederFactor);
                return f;
            }
            return 0;
        }
    }


}
