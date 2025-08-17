using Prescripto.DTOs;
using Prescripto.DTOs.TTACDtos;
using Prescripto.Models;
using System.Data;
using System.Data.SqlClient;

namespace Prescripto.Repositories
{
    public class PharmcyRepository : IPharmcyRepository
    {        
        public CoNameDto GetPharmcyName(string connection, string username)
        {
            CoNameDto coName = new CoNameDto();
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = $@"Declare @CoName varchar(200),@id int, @Lname varchar(200),@Hname Varchar(200),@ValMojodi int
                                        SELECT @CoName = CoName FROM Pharmcy

                                        select @id = id, @Lname = Lname from Users
                                           Where username collate Arabic_CI_AS in (
                                           Select case when default_database_name = 'master' then 'dbo' else name end from sys.sql_logins
                                           where name = '{username}')

                                        select @Hname = HOST_NAME();
                                                            if exists(Select val From Setting  where[key] = 'manfi' and[sys] = @Hname)
                                                            Select @ValMojodi = val From Setting  where[key] = 'manfi' and[sys] = @Hname
                                        else
                                                                select @ValMojodi = 0

                                        Select @CoName as CName,@id as id, @Lname as Lname,@Hname as Hname,@ValMojodi as ValMojodi";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            coName.CoName = reader.GetString(0);
                            coName.id = Convert.ToInt32(reader.GetValue(1));
                            coName.Lname = reader.GetString(2);
                            coName.Hname = reader.GetString(3);
                            coName.ValMojodi = Convert.ToInt32(reader.GetValue(4)); ;
                        }
                    }
                    con.Close();
                }
            }
            return coName;
        }

        public ExistsNoskhe GetExistsNoskhe(string connection, string WebTrackingCode, string ShDaftarcheh, string DateFac)
        {
            ExistsNoskhe noskhe=new ExistsNoskhe();
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = $"Select top 1 Sh_Noskhe,jamkolpardakht from FacHeder Where WebTrackingCode = '{WebTrackingCode}' and ShDaftarcheh = '{ShDaftarcheh}' and DateFac ={DateFac} order by CodeFacheder Desc"; ;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            noskhe.Sh_noskhe = Convert.ToInt32(reader.GetValue(0));
                            noskhe.jamkolpardakht = Convert.ToInt64(reader.GetValue(1));
                        }
                    }
                    con.Close();
                }

            }
            return noskhe;
        }

        public int GetSettingMojodi(string connection)
        {
            int val = 0;
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"Declare @Hname Varchar(200)
                                                select @Hname = HOST_NAME();
                                                                        if exists(Select val From Setting  where[key] = 'manfi' and[sys] = @Hname)
                                                                        Select val From Setting  where[key] = 'manfi' and[sys] = @Hname
                                                else
                                                                            select 0";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            val = Convert.ToInt32(reader.GetString(0));
                        }
                    }
                    con.Close();
                }
            }
            return val;
        }

        public NameDr GetNameDr(string connection, int DocId)
        {
            NameDr Dr = new NameDr();
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"Select D.NameDr,D.CodeTakh,T.Takhasos,T.CodeTakh2 from Dr D
                                                inner join Takhasos T on D.CodeTakh = T.CodeTakh where D.Mama = 0 and D.CodeDr =" + Convert.ToInt32(DocId) + " ";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Dr.NameDoctor = string.IsNullOrEmpty(reader.GetString(0)) ? "-" : reader.GetString(0);
                            Dr.CodeTakh = Convert.ToInt32(reader.GetValue(1));
                            Dr.Takhasos = string.IsNullOrEmpty(reader.GetString(2)) ? "-" : reader.GetString(2);                            
                            Dr.Codetakh2 = Convert.ToInt32(reader.GetValue(3));
                        }
                        else
                        {
                            Dr.NameDoctor = "-" ;
                            Dr.CodeTakh = 0 ;
                            Dr.Takhasos = "-" ;
                            Dr.Codetakh2 = 0 ;
                        }
                    }
                    con.Close();
                }
            }
            return Dr;
        }

        public List<RadifKala> Getkalanoskhe(string connection, ListKalaNoskhe listKalaNoskhe)
        {
            List<RadifKala> taminRadifKala = new List<RadifKala>();
            try
            {
                using (var con = new SqlConnection(connection))
                {
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        try
                        {
                            foreach (var item in listKalaNoskhe.Radifs)
                            {
                                RadifKala taminRadif = new RadifKala();
                                taminRadif.gencode = item.gencode;
                                taminRadif.drugName = item.drugName;
                                taminRadif.tedUsage = item.tedUsage;
                                taminRadif.orgShare = item.orgShare;
                                taminRadif.per = item.per;
                                taminRadif.totalAmount = item.totalAmount;
                                taminRadif.paymentByOrg = item.paymentByOrg;
                                taminRadif.byYaraneh = item.byYaraneh;
                                taminRadif.subsidy = item.subsidy;
                                taminRadif.paymentByPatient = item.paymentByPatient;
                                taminRadif.byOrg = item.byOrg;
                                int codearia = 0;
                                string faname, gencode, price, mojodi;
                                con.Open();
                                cmd.CommandText = @"select code from tamin_kala where tamin_code='" + item.gencode + "'";
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    try
                                    {
                                        if (reader.Read())
                                        {
                                            codearia = Convert.ToInt32(reader["code"].ToString());
                                        }
                                        if (codearia == 0 || string.IsNullOrEmpty(codearia.ToString()))
                                        {
                                            cmd.CommandText = @"select Top 1 Code from v_kala_tamin where Gencode='" + item.gencode + "'";
                                            using (SqlDataReader readerView = cmd.ExecuteReader())
                                            {
                                                try
                                                {
                                                    if (readerView.Read())
                                                    {
                                                        codearia = Convert.ToInt32(readerView["code"].ToString());
                                                    }
                                                }
                                                catch
                                                {
                                                    codearia = 0;
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        reader.Close();
                                        cmd.CommandText = @"select Top 1 Code from v_kala_tamin where Gencode='" + item.gencode + "'";
                                        using (SqlDataReader readerView2 = cmd.ExecuteReader())
                                        {
                                            try
                                            {
                                                if (readerView2.Read())
                                                {
                                                    codearia = Convert.ToInt32(readerView2["code"].ToString());
                                                }
                                            }
                                            catch
                                            {
                                                codearia = 2;
                                            }
                                        }
                                    }

                                }
                                if (codearia > 0)
                                {
                                    cmd.CommandText = @"select k.code,k.Faname,k.gencode,k.Price,isnull(M.mojodi,0) as mojodi from Kala K
                                                            Left join Mojodi M on M.code=K.code and M.anbcode=1 where K.code=" + codearia + "";
                                    using (SqlDataReader readerkala = cmd.ExecuteReader())
                                    {
                                        try
                                        {
                                            if (readerkala.Read())
                                            {
                                                taminRadif.CodeKalaAria = Convert.ToInt32(readerkala["code"].ToString());
                                                taminRadif.NameAria = (readerkala["Faname"].ToString());
                                                taminRadif.GencodeAria = (readerkala["gencode"].ToString());
                                                taminRadif.PriceAria = (readerkala["Price"].ToString());
                                                taminRadif.MojodiAria = (readerkala["mojodi"].ToString());
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                else
                                {
                                    taminRadif.CodeKalaAria = 0;
                                    taminRadif.NameAria = "-";
                                    taminRadif.GencodeAria = "";
                                    taminRadif.PriceAria = "";
                                    taminRadif.MojodiAria = "0";
                                }
                                con.Close();
                                taminRadifKala.Add(taminRadif);
                            }
                        }
                        catch (Exception ex)
                        {
                            return null ;
                        }
                    }
                }
                return taminRadifKala;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Takhasos> Gettakhasos(string connection)
        {
            List<Takhasos> takhasoses = new List<Takhasos>();
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {                       
                        con.Open();
                        cmd.CommandText = @"Select CodeTakh,Takhasos from Takhasos ";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                            Takhasos t = new Takhasos();
                                t.CodeTakh = Convert.ToInt32(reader["CodeTakh"].ToString());
                                t.TakhasosName = reader["Takhasos"].ToString();
                                takhasoses.Add(t);
                            }
                        }
                        con.Close();                        
                }
            }
            return takhasoses;           
        }

        public int InsertDr(string connection, string Codedr, string NameDr, int codetakh)
        {
            int codetakh2 = 0;
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                        con.Open();
                        cmd.CommandText = @"insert into Dr(codedr,Namedr , codetakh,mama)values(" + Codedr + ",cast('" + NameDr + "' as varchar(35))," + codetakh + ",0)";
                        cmd.ExecuteNonQuery();
                        con.Close();
                }
                using (var cmd = con.CreateCommand())
                {
                        con.Open();
                        cmd.CommandText = @" select CodeTakh2 from Takhasos where CodeTakh= " + codetakh + " ";

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                codetakh2 = reader.GetInt32(0);
                            }
                        }
                        con.Close();
                }
            }
            return codetakh2;
        }

        public List<KalaSmal> Getkalasmal(string conn, string CodeSazeman, int page, int size, string search)
        {
            List<KalaSmal> products = new List<KalaSmal>();

            using (var connection = new SqlConnection(conn))
            using (var command = new SqlCommand(@"SP_KalaSmal_WebExtension", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@anb", "1");
                command.Parameters.AddWithValue("@tamincode", CodeSazeman);
                command.Parameters.AddWithValue("@PageNumber", page);
                command.Parameters.AddWithValue("@PageSize", size);
                command.Parameters.AddWithValue("@search", search);
                command.CommandTimeout = 0;
                connection.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter dr = new SqlDataAdapter(command);
                dr.Fill(dt);
                foreach (DataRow item in dt.Rows)
                {
                    products.Add(new KalaSmal
                    {
                        Code = Convert.ToInt32(item["code"].ToString()),
                        Faname = item["Faname"].ToString(),
                        Enname = item["Enname"].ToString(),
                        Price = string.IsNullOrEmpty(item["Price"].ToString()) ? null : Convert.ToDouble(item["Price"]),
                        Genname = item["Genname"].ToString(),
                        Gencode = item["Gencode"].ToString(),
                        N1 = item["N1"].ToString(),
                        N2 = item["N2"].ToString(),
                        N3 = item["N3"].ToString(),
                        N4 = item["N4"].ToString(),
                        N5 = item["N5"].ToString(),
                        Mojodi = string.IsNullOrEmpty(item["Mojodi"].ToString()) ? null : Convert.ToDouble(item["Mojodi"]),
                        MojodiAnb = string.IsNullOrEmpty(item["MojodiAnb"].ToString()) ? null : Convert.ToDouble(item["MojodiAnb"]),
                    });
                }
                connection.Close();
            }
            return products;
        }

        public List<Fard> getFard(string connection)
        {
            List<Fard> fards = new List<Fard>();
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"Select codem,replace(name,NCHAR(1610),NCHAR(1740))  as name from Fard ";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Fard t = new Fard();
                            t.id = Convert.ToInt32(reader["codem"].ToString());
                            t.name = reader["name"].ToString();
                            fards.Add(t);
                        }
                    }
                    con.Close();
                }
            }
            return fards;
        }

        public int GetSettingKala(string connection, string Hname)
        {
            int val = 0;
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"select val from setting where Tag='Form' and [key]='Modcodeex' and [sys]='"+ Hname +"'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            val = Convert.ToInt32(reader.GetString(0));
                        }
                    }
                    con.Close();
                }
            }
            return val;
        }

        public int GetCheckCodeKala(string connection, string Code)
        {
            int val = 0;
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"select Code from Kala where code=" + Code + "";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            val = Convert.ToInt32(reader.GetValue(0));
                        }
                    }
                    con.Close();
                }
            }
            return val;
        }

        public List<Fard> getdastedaroo(string connection)
        {
            List<Fard> fards = new List<Fard>();
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"Select idDastdaroie,replace(Dastdaroie,NCHAR(1610),NCHAR(1740))  as name from Dastedaroie ";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Fard t = new Fard();
                            t.id = Convert.ToInt32(reader["idDastdaroie"].ToString());
                            t.name = reader["name"].ToString();
                            fards.Add(t);
                        }
                    }
                    con.Close();
                }
            }
            return fards;
        }

        public List<Fard> getdruggroup(string connection)
        {
            List<Fard> fards = new List<Fard>();
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"Select Codegroup,replace(Druggroup,NCHAR(1610),NCHAR(1740))  as name from Druggroup ";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Fard t = new Fard();
                            t.id = Convert.ToInt32(reader["Codegroup"].ToString());
                            t.name = reader["name"].ToString();
                            fards.Add(t);
                        }
                    }
                    con.Close();
                }
            }
            return fards;
        }

        public List<Fard> getnoekala(string connection)
        {
            List<Fard> fards = new List<Fard>();
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"Select idnoeKala,replace(NoeKala,NCHAR(1610),NCHAR(1740))  as name from NoeKala ";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Fard t = new Fard();
                            t.id = Convert.ToInt32(reader["idnoeKala"].ToString());
                            t.name = reader["name"].ToString();
                            fards.Add(t);
                        }
                    }
                    con.Close();
                }
            }
            return fards;
        }
        public int getLastCodeKala(string connection)
        {
            int code = 0;
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @" select max(isnull(Code,0))+1 as code from Kala";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            code = Convert.ToInt32(reader["code"].ToString());
                        }
                    }
                    con.Close();
                }
            }
            return code;
        }

        public int getcodekalawithCodegroup(string connection, int Codegroup)
        {
            int code = 0;
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @" Declare @codegroup int , @Lcodegroup int
                                        Declare @maxCode bigint
                                        set @codegroup = "+ Codegroup + @"
                                        select @Lcodegroup=LEN(@codegroup)
                                        select @maxCode=max(Code)+1 from Kala where  left(code,@Lcodegroup) = @codegroup  and len(code)=@Lcodegroup+4 
                                        if(@maxCode >0) 
                                        begin select @maxCode as code end 
                                        else 
                                        begin 
                                        select (@codegroup*10000)+1  as code
                                        end";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            code = Convert.ToInt32(reader["code"].ToString());
                        }
                    }
                    con.Close();
                }
            }
            return code;
        }

        public bool CreateKala(string connection, kala kala)
        {
            try
            {
                using (var con = new SqlConnection(connection))
                {
                    using (var cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = $@"insert into kala(code,faname,enname,gencode,price,pricebimeh,frasli,buy,ghavaldore,irc,codegroup,dastedaroo,[type],fani,otc,moafazmaliat,needtaeed,useintarkib)
                                            values({kala.code},cast('{kala.faname.Replace("'"," ")}' as varchar(200)),cast('{kala.enname.Replace("'", " ")}' as varchar(200)),{kala.gencode},{kala.price},{kala.price},{kala.price},{kala.buy},{kala.buy}
                                                   ,'{kala.irc}',{kala.codegroup},{kala.dastedaroo},{kala.type},{kala.fani},{kala.otc},1,0,0)

                                            Declare @barcode bigint
                                            Select @barcode=gtn from Products where irc='{kala.irc}'
                                            if(@barcode)>0
                                            begin
                                            if not exists(select * from barcode where Barcode=@barcode)
                                            begin
                                                    insert into barcode(code,barcode)values({kala.code},@barcode)
                                            end  
                                            end
                                            
                                            update Products set Code = {kala.code},Connection='IRC' where irc='{kala.irc}'
                                                ";
                        cmd.ExecuteNonQuery();
                        
                        con.Close();
                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
