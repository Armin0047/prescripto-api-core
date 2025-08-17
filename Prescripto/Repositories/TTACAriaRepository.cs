using Prescripto.DTOs.TTACDtos;
using System.Data.SqlClient;

namespace Prescripto.Repositories
{
    public class TTACAriaRepository : ITTACAriaRepository
    {
        public List<Fard> getfacFroup(string connection)
        {
            List<Fard> fards = new List<Fard>();
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"Select id,replace(name,NCHAR(1610),NCHAR(1740))  as name from fac_group ";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Fard t = new Fard();
                            t.id = Convert.ToInt32(reader["id"].ToString());
                            t.name = reader["name"].ToString();
                            fards.Add(t);
                        }
                    }
                    con.Close();
                }
            }
            return fards;
        }

        public List<Fard> getAnbarlist(string connection)
        {
            List<Fard> fards = new List<Fard>();
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"Select anbcode,anbname from Anbar ";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Fard t = new Fard();
                            t.id = Convert.ToInt32(reader["anbcode"].ToString());
                            t.name = reader["anbname"].ToString();
                            fards.Add(t);
                        }
                    }
                    con.Close();
                }
            }
            return fards;
        }

        public List<TTAcKalaAria> selectKala(string connection, List<ListTTAcKala> listTTAcKalas)
        {
            List<TTAcKalaAria> tTAcKalas = new List<TTAcKalaAria>();
            using (var con = new SqlConnection(connection))
            {
                using(var cmd = con.CreateCommand())
                {
                    foreach (var item in listTTAcKalas)
                    {
                        TTAcKalaAria tTAc = new TTAcKalaAria();
                        tTAc.ircTTAC = item.ircTTAC;
                        tTAc.rdf=item.rdf;
                        tTAc.tedbastehTTAC = item.tedbastehTTAC;
                        tTAc.fanameTTAC=item.fanameTTAC;
                        tTAc.ennameTTAc = item.ennameTTAc;
                        tTAc.bachTTAC = item.bachTTAC;
                        tTAc.gencodeTTAC= item.gencodeTTAC;
                        tTAc.expTTAC = item.expTTAC.Length > 10 ? item.expTTAC.Substring(0,10) : item.expTTAC ;

                        con.Open();
                        cmd.CommandText = @"Select K.Code,K.FaName,K.GenCode,Round(K.Price,0) as PriceAria,P.TedDarBasteh,
                                            case when cast(P.buy as bigint) =0 then cast(K.buy as bigint) else cast(P.buy as bigint) end as buy,
                                            case when cast(P.Price as bigint)=0 then cast(K.Price as bigint) else cast(P.Price as bigint) end as Price,
                                            case when isnull(MoafAzMaliat,1)=1 then 0 else cast (isnull((select MaliatKH from Pharmcy),0) as int) end as Maliat
                                            From Products P
                                            Left join Kala K on K.Code=P.Code
                                            Where P.Irc='" + item.ircTTAC+"'";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tTAc.CodeKalaAria = reader.IsDBNull(0)  ? 0 : Convert.ToInt32(reader.GetValue(0));
                                tTAc.fanameAria = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                tTAc.gencodeAria = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                tTAc.priceAria = reader.IsDBNull(3) ? 0 : Convert.ToInt64(reader.GetValue(3));
                                tTAc.TedDarBasteh = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                                tTAc.buy = reader.IsDBNull(5) ? 0 : Convert.ToInt64(reader.GetValue(5));
                                tTAc.price = reader.IsDBNull(6) ? 0 : Convert.ToInt64(reader.GetValue(6));
                                tTAc.Maliat = reader.GetInt32(7);
                            }
                        }
                        con.Close();
                        tTAcKalas.Add(tTAc);
                    }
                }
            }
            return tTAcKalas;
        }

        public bool updateKala(string connection, int codeKala, string irc)
        {
            try
            {
                using (var con = new SqlConnection(connection))
                {
                    using (var cmd = con.CreateCommand())
                    {

                        con.Open();
                        
                            cmd.CommandText = "update Products set code="+codeKala+" where irc='"+irc+"'";
                            cmd.ExecuteNonQuery();                        
                        con.Close();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int insertfactor(string connection, FachederFactor fachederFactor)
        {
            int code = 0;
            int countsabt = 0;
            int Noe = 0;
            foreach (var item in fachederFactor.Radifs)
            {
                if (item.sabt)
                {
                    countsabt++;
                    break;
                }
            }
            if (countsabt > 0)
            {
                using (var con = new SqlConnection(connection))
                {
                    using (var cmd = con.CreateCommand())
                    {

                        con.Open();
                        cmd.CommandText = $"select Noe from Anbar where AnbCode={fachederFactor.anbcode}";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Noe = reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader.GetValue(0));                                
                            }
                        }
                        con.Close();
                    }
                }
                int Shfacmoshtari =string.IsNullOrEmpty(fachederFactor.Shfacmoshtari) ? 0 : Convert.ToInt32(fachederFactor.Shfacmoshtari) ;
                string? idgroup =  fachederFactor.facgroup > 0 ?  fachederFactor.facgroup.ToString() : "null" ;
                if (Noe > 0 && Noe == 1)
                {
                    int codefacheder = 0;
                    using (var con = new SqlConnection(connection))
                    {
                        using (var cmd = con.CreateCommand())
                        {

                            con.Open();
                            cmd.CommandText = $@" insert into FacHeder (sh_noskhe,state,anb_code,CodeUser,
                                                    JamKolNoskhe,JamKolPardakht,DateFac,CodeSazeMan,takhfif,TakhfifRadif,
		                                                    BimarName,MoafAzMaliat,RadifLoked,HBarbary,HOther,ArzeshAfzodeh,Date_Sabt,ShFacMoshatri,tabadol,idGroup )
                                                Values( isnull( (select max(Sh_noskhe) from facheder Where state=7 and anb_code=1),0)+1,7,{fachederFactor.anbcode},dbo.Fu_getactiveuser(),
                                                        {fachederFactor.jamkol},{fachederFactor.jamkhales},dbo.Milady2Shams(GETDATE()),{fachederFactor.codem},{fachederFactor.takhfif},{fachederFactor.takhfif},
                                                        '{fachederFactor.sharh.Replace("'"," ")}',1,1,{fachederFactor.Hbarbary},{fachederFactor.Hother},{fachederFactor.Hmaliat},{fachederFactor.datefac},
                                                        case when {Shfacmoshtari} > 2147483647  then right({Shfacmoshtari},8) else {Shfacmoshtari} end,0,{idgroup})

                                                     select SCOPE_IDENTITY()  as CodeFacheder
                                                ";
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    codefacheder = Convert.ToInt32(reader["CodeFacheder"].ToString());
                                }
                            }
                            con.Close();

                            if(codefacheder > 0)
                            {
                                int rdf = 1;
                                foreach (var item in fachederFactor.Radifs)
                                {
                                    if(item.sabt)
                                    {
                                        decimal? takhfif = item.tkhfif > 100 ? item.tkhfif : 0;
                                        decimal? Darsadtakhfif = takhfif == 0 ? item.tkhfif : (item.tkhfif / (item.ted * item.ghjoje)) * 100;
                                        decimal? maliatvalue = item.maliat > 100 ? item.maliat : 0;
                                        decimal? maliat = maliatvalue == 0 ? item.maliat : 0 ;//(item.maliat / (item.ted * item.ghjoje)) * 100;
                                        cmd.CommandText = $@"insert into Facradif(CodeFacHeder,code,codem,ted,gh,state,
                                                                                    buy,JamRadif,Sharh,rdf,AnbCode,tedBaste,tedDarKol,
                                                                                    EnghezaShamsi,EnghezaMiladi,codeuser,maliat,takhfif,
                                                                                    maliatvalue,DarsadTakhfif)
                                                                Values({codefacheder},{item.code},0,{item.ted},{item.ghjoje},7,
                                                                       {item.ghmasraf},{item.ted*item.ghjoje},{item.serial},{rdf},{fachederFactor.anbcode},{item.tedbasteh},{item.tedDarBasteh},
                                                                       case when {item.ex} > 19900101 then 
                                                                        dbo.Milady2Shams(cast({item.ex} as varchar(10)) + ' 00:00:00.000' ) else {item.ex} end,
                                                                        case when {item.ex} > 19900101 then {item.ex} end,dbo.Fu_getactiveuser(),{maliat},{takhfif},
                                                                                    {maliatvalue},{Darsadtakhfif})";
                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                        rdf++;
                                        cmd.CommandText = $"update kala set buy={item.ghjoje} where code={item.code}";
                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    if (item.update)
                                    {
                                        cmd.CommandText = $"update kala set price={item.ghmasraf} where code={item.code}";
                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                                
                            }
                            cmd.CommandText = $"select sh_noskhe from facheder where codefacheder={codefacheder}";
                            con.Open();
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    code = Convert.ToInt32(reader["sh_noskhe"].ToString());
                                }
                            }
                            con.Close();
                            return code;
                        }
                    }
                }

                else if(Noe > 0 && Noe == 2)
                {
                    int codefacheder = 0;
                    using (var con = new SqlConnection(connection))
                    {
                        using (var cmd = con.CreateCommand())
                        {

                            con.Open();
                            cmd.CommandText = $@" Declare @code int
                                                    Select @code = isnull(max(code),0)+1 from Fac_Heder where state=1
                                                    insert into Fac_Heder(code,state,Date_Fac,code_m,Takhfif,Sharh,
                                                                         Anb_Code,Iduser,JamFac,JamKhales,
                                                                         ArzeshAfzodeh,ShFacMoshatri,date_sabt,codeuser,MoafAzMaliat,
                                                                         TakhfifRadif,RadifLoked,HBarbary,HMaliat,idGroup )
                                                    values(@code,1,dbo.Milady2Shams(GETDATE()),{fachederFactor.codem},{fachederFactor.takhfif},'{fachederFactor.sharh.Replace("'"," ")}',
                                                            {fachederFactor.anbcode},dbo.Fu_getactiveuser(),{fachederFactor.jamkol},{fachederFactor.jamkhales},
                                                            {fachederFactor.Hmaliat},case when {Shfacmoshtari} > 2147483647  then right({Shfacmoshtari},8) else {Shfacmoshtari} end,{fachederFactor.datefac},dbo.Fu_getactiveuser(),1,
                                                            {fachederFactor.takhfif},1,{fachederFactor.Hbarbary},{fachederFactor.Hother},{idgroup})
                                                    select @code as code    ";
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    codefacheder = Convert.ToInt32(reader["code"].ToString());
                                }
                            }
                            con.Close();

                            if (codefacheder > 0)
                            {
                                int rdf = 1;
                                foreach (var item in fachederFactor.Radifs)
                                {
                                    if (item.sabt)
                                    {
                                        decimal? takhfif = item.tkhfif > 100 ? item.tkhfif : 0;
                                        decimal? Darsadtakhfif = takhfif == 0 ? item.tkhfif : (item.tkhfif / (item.ted * item.ghjoje)) * 100;
                                        decimal? maliatvalue = item.maliat > 100 ? item.maliat : 0;
                                        decimal? maliat = maliatvalue == 0 ? item.maliat : (item.maliat / (item.ted * item.ghjoje)) * 100;
                                        cmd.CommandText = $@"insert into Fac_radif(Filter,KalaCode,State,Radif,ted,GhJoje,JamRadif,
                                                                                    AnbCode,serial,maliat,avarez,ghMasraf,tedDarKol,
                                                                                    tedBaste,EnghezaShamsi,EnghezaMiladi,codeuser,fiTakhfif,MaliatValue,tkhfif)
                                                                Values({codefacheder},{item.code},1,{rdf},{item.ted},{item.ghjoje},{item.ted * item.ghjoje},
                                                                       {fachederFactor.anbcode},{item.serial},{maliat},0,{item.ghmasraf},{item.tedDarBasteh},
                                                                        {item.tedbasteh},
                                                                       case when {item.ex} > 19900101 then 
                                                                        dbo.Milady2Shams(cast({item.ex} as varchar(10)) + ' 00:00:00.000' ) else {item.ex} end,
                                                                        case when {item.ex} > 19900101 then {item.ex} end,dbo.Fu_getactiveuser(),{takhfif},
                                                                                    {maliatvalue},{Darsadtakhfif})";
                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                        rdf++;
                                        cmd.CommandText = $"update kala set buy={item.ghjoje} where code={item.code}";
                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    if (item.update)
                                    {
                                        cmd.CommandText = $"update kala set price={item.ghmasraf} where code={item.code}";
                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }

                            }
                            code = codefacheder;
                            return code;
                        }
                    }
                }
            }














            return 0;
        }        
    }
}
