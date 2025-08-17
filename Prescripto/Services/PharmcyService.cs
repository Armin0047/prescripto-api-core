using Prescripto.DTOs;
using Prescripto.DTOs.TTACDtos;
using Prescripto.Models;
using Prescripto.Repositories;

namespace Prescripto.Services
{
    public class PharmcyService : IPharmcyService       
    {
        private readonly IPharmcyRepository _repository;
        private readonly DbConnectionInfo _connectionService;
        public PharmcyService(IPharmcyRepository repository, DbConnectionInfo connectionService)
        {
            _repository = repository;
            _connectionService = connectionService;
        }

        public CoNameDto GetCoName(DbConfig connection,string username)
        {            

            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                var name = _repository.GetPharmcyName(con, username);
                return new CoNameDto { CoName = name.CoName,id=name.id,Lname=name.Lname,Hname=name.Hname,ValMojodi=name.ValMojodi };
            }
            return null;

        }

        public ExistsNoskhe ExistsNoskhe(DbConfig connection, string WebTrackingCode, string ShDaftarcheh, string DateFac)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                ExistsNoskhe noskhe=new ExistsNoskhe();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                noskhe = _repository.GetExistsNoskhe(con, WebTrackingCode, ShDaftarcheh, DateFac);
                return  noskhe ;
            }
            return null;
        }

        public int GetSettingMojodi(DbConfig connection)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                ExistsNoskhe noskhe = new ExistsNoskhe();
                var con = _connectionService.Build(connection.Server);
                var val = _repository.GetSettingMojodi(con);
                return val;
            }
            return 0;
        }

        public NameDr GetNameDr(DbConfig connection, int DocId)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                NameDr Dr = new NameDr();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                Dr = _repository.GetNameDr(con, DocId);
                return Dr;
            }
            return null;
        }

        public List<RadifKala> GetListKalaNoskhes(DbConfig connection, ListKalaNoskhe listKalaNoskhe)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                List<RadifKala> kalanoskhe = new List<RadifKala>();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                kalanoskhe = _repository.Getkalanoskhe(con, listKalaNoskhe);
                return kalanoskhe;
            }
            return null;
        }

        public List<Takhasos> GetListTakhasos(DbConfig connection)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                List<Takhasos> takh = new List<Takhasos>();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                takh = _repository.Gettakhasos(con);
                return takh;
            }
            return null;
        }

        public int InsertDoctor(DbConfig connection, string Codedr, string NameDr, int codetakh)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                var codetakh2 = _repository.InsertDr(con, Codedr,NameDr,codetakh);
                return codetakh2;
            }
            return 0;
        }

        public List<KalaSmal> kalaSmal(DbConfig connection, string CodeSazeman, int page, int size, string search)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                List<KalaSmal> kala = new List<KalaSmal>();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                kala = _repository.Getkalasmal(con, CodeSazeman, page, size, search);
                return kala;
            }
            return null;
        }

        public List<Fard> fards(DbConfig connection)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                List<Fard> f = new List<Fard>();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                f = _repository.getFard(con);
                return f;
            }
            return null;
        }

        public int GetSettingKala(DbConfig connection,string Hname)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                var con = _connectionService.Build(connection.Server);
                var val = _repository.GetSettingKala(con, Hname);
                return val;
            }
            return 0;
        }
        public int GetCheckCodeKala(DbConfig connection, string Code)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                var con = _connectionService.Build(connection.Server);
                var val = _repository.GetCheckCodeKala(con, Code);
                return val;
            }
            return 0;
        }

        public List<Fard> dastedaroo(DbConfig connection)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                List<Fard> f = new List<Fard>();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                f = _repository.getdastedaroo(con);
                return f;
            }
            return null;
        }

        public List<Fard> druggroup(DbConfig connection)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                List<Fard> f = new List<Fard>();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                f = _repository.getdruggroup(con);
                return f;
            }
            return null;
        }
        public List<Fard> noekala(DbConfig connection)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                List<Fard> f = new List<Fard>();
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                f = _repository.getnoekala(con);
                return f;
            }
            return null;
        }

        public int LastCodeKala(DbConfig connection)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                var f = _repository.getLastCodeKala(con);
                return f;
            }
            return 0;
        }

        public int codekalawithCodegroup(DbConfig connection, int Codegroup)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                var f = _repository.getcodekalawithCodegroup(con,Codegroup);
                return f;
            }
            return 0;
        }

        public bool CreateKala(DbConfig connection, kala kala)
        {
            if (_connectionService.DynamicConnection(connection.Server, connection.Database, connection.Username, connection.Password))
            {
                var con = _connectionService.Build(connection.Server, connection.Database, connection.Username, connection.Password);
                var f = _repository.CreateKala(con, kala);
                return f;
            }
            return false;
        }
    }
}
