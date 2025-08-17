using Prescripto.DTOs;
using System.Data.SqlClient;

namespace Prescripto.Repositories
{
    public class TaminRepository : ITaminRepository
    {
        public List<Sazeman> GetSazemanList(string connection)
        {
            List<Sazeman> sazeman = new List<Sazeman>();
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"exec sp_tamin_bimeh";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Sazeman s = new Sazeman();
                            s.codeSazeman = Convert.ToInt32(reader.GetValue(0));
                            s.nameSazeman = reader.GetValue(1).ToString();
                            sazeman.Add(s);
                        }
                    }
                    con.Close();
                }
            }
            return sazeman;
        }

        public bool SetUpdateKala(string connection, int codeKala, string CodeKalaNew, string Gencode)
        {
            try
            {
                using (var con = new SqlConnection(connection))
                {
                    using (var cmd = con.CreateCommand())
                    {

                        con.Open();
                        if (codeKala == 0)
                        {
                            cmd.CommandText = "insert into tamin_kala(code,tamin_code)values(" + CodeKalaNew + ",'" + Gencode + "')";
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.CommandText = " if exists ( select * from  tamin_kala where tamin_code='" + Gencode + "') " +
                                              " Update tamin_kala set code=" + CodeKalaNew + " where tamin_code='" + Gencode + "' " +
                                              " else " +
                                              " insert into tamin_kala(code,tamin_code)values(" + CodeKalaNew + ",'" + Gencode + "')";
                            cmd.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                } return true;
            }
            catch 
            {
                return false;
            }
            
        }

        public ExistsNoskhe InsertFactor(string connection, ListKalaNoskhe taminHeder)
        {
            ExistsNoskhe noskhe=new ExistsNoskhe();
            int CodeFacheder = 0;
            int SH_Noskhe = 0;
            Int64 jamkolpardakht = 0;
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                        decimal sumsahmbimar = Convert.ToDecimal(taminHeder.PaymentByPatient.Replace(",", "").Replace("ريال", "")) + Convert.ToDecimal(taminHeder.Subsidy.Replace(",", "").Replace("ريال", ""));
                        con.Open();
                        cmd.CommandText = $@"
                                                    Declare @takh int,@takh2 int
                                                    Select @takh=isnull(t.CodeTakh,1),@takh2=isnull(t.CodeTakh2,20) from dr d
                                                    left join Takhasos T on d.CodeTakh=t.CodeTakh 
                                                    where d.codedr={taminHeder.DocID} and d.mama=0

                                                    insert into FacHeder (sh_noskhe,state,anb_code,sabtshod,sabtshod2,CodeUser,
                                                    CodeTakh,CodeTakh2,SahmBimar,JamKolNoskhe,JamKolPardakht,EzafeDaryafty,ShSafhe,codedr,DateFac,
                                                        SahmSazeman,CodeSazeMan,Taeed,takhfif,mama,
		                                                    ShDaftarcheh,BimarName,DateNoskhe,Nezam,ShomareMeli,mobile,MoafAzMaliat,RadifLoked,SazemanIsAzad,
                                                        WebTrackingCode,trackingCode)
                                                    Values( isnull( (select max(Sh_noskhe) from facheder Where state=0 and anb_code=1),0)+1 ,0,1,1,1,dbo.Fu_getactiveuser(),
                                                    @takh,@takh2,{sumsahmbimar},{taminHeder.TotalAmount.Replace(",", "").Replace("ريال", "")},0,0,1,{taminHeder.DocID},{taminHeder.RegDate.Replace("/", "")},
                                                    {taminHeder.PaymentByOrg.Replace(",", "").Replace("ريال", "")},{taminHeder.Codesazeman},1,{taminHeder.Subsidy.Replace(",", "").Replace("ريال", "")},0,
                                                    '{taminHeder.NationalCode}','{taminHeder.bimarname.Replace("'", "")}',{taminHeder.ReqDate.Replace("/", "")},{taminHeder.DocID},'{taminHeder.NationalCode}','{taminHeder.mobile.Replace("'", "")}',1,0,0,
                                                    '{taminHeder.ReqNum}','{taminHeder.ReqNum}') "
                                           + " select SCOPE_IDENTITY()  as CodeFacheder ";

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CodeFacheder = Convert.ToInt32(reader["CodeFacheder"].ToString());
                            }
                        }

                        if (CodeFacheder > 0)
                        {
                            int rdf = 1;
                            foreach (var item in taminHeder.Radifs)
                            {
                                decimal ezafe = 0;
                                string Flag = "0";
                                decimal sahmbimar = 0;

                                ezafe = Convert.ToInt32(item.tedUsage.Replace(",", "")) * Convert.ToInt32(item.PriceAria.Replace(",", "")) > Convert.ToInt32(item.totalAmount.Replace(",", "")) ? (Convert.ToInt32(item.tedUsage.Replace(",", "")) * Convert.ToInt32(item.PriceAria.Replace(",", ""))) - Convert.ToInt32(item.totalAmount.Replace(",", "")) : 0;
                                Flag = Convert.ToInt32(item.paymentByOrg.Replace(",", "")) > 0 ? "0" : "16777058";
                                sahmbimar = Convert.ToInt32(item.paymentByOrg.Replace(",", "")) > 0 ? Convert.ToInt32(item.paymentByPatient.Replace(",", "")) + Convert.ToInt32(item.subsidy.Replace(",", "")) : 0;
                                ezafe = ezafe + (Convert.ToInt32(item.paymentByOrg.Replace(",", "")) > 0 ? 0 : Convert.ToInt32(item.paymentByPatient.Replace(",", "")) + Convert.ToInt32(item.subsidy == null || item.subsidy == "" ? 0 : item.subsidy.Replace(",", "")));


                                int dastordaroo = 0;
                                int gh = Convert.ToInt32(item.tedUsage.Replace(",", "")) * Convert.ToInt32(item.PriceAria.Replace(",", "")) < Convert.ToInt32(item.totalAmount.Replace(",", "")) ? Convert.ToInt32(item.totalAmount.Replace(",", "")) / Convert.ToInt32(item.tedUsage.Replace(",", "")) : Convert.ToInt32(item.PriceAria.Replace(",", ""));

                                cmd.CommandText = $@"if exists( Select * from DastorDaroi where DastorDaroi='{item.orgShare}' and Des='Tamin')
                                                                begin
                                                                Select top 1 code from DastorDaroi where DastorDaroi='{item.orgShare}' and Des='Tamin' order by code
                                                                end
                                                                else 
                                                                begin
                                                                insert into DastorDaroi(DastorDaroi,Des)
                                                                values('{item.orgShare}','Tamin')
                                                                select SCOPE_IDENTITY()
                                                                end
                                                                ";
                                using (var reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        dastordaroo = Convert.ToInt32(reader.GetValue(0));
                                    }
                                }

                                cmd.CommandText = $@"insert into Facradif(CodeFacHeder,code,codem,per,ted,gh,state,
                                                               JamRadif,SahmBimar,
                                                               EzafeDariafty,SahmSazeman,flag,
                                                               rdf,AnbCode,CodeDastorDarie,codeuser,acceptpeygham)
                                                        values({CodeFacheder},{item.CodeKalaAria},0,{100 - Convert.ToDouble(item.per.Replace("%", ""))},{item.tedUsage.Replace(",", "")},{gh},0,
                                                               {item.totalAmount.Replace(",", "")},{sahmbimar},
                                                               {ezafe},{item.paymentByOrg.Replace(",", "")},{Flag},{rdf},1,{dastordaroo},dbo.Fu_getactiveuser(),1)";
                                cmd.ExecuteNonQuery();
                                rdf++;
                            }

                            cmd.CommandText = $"Select Sh_noskhe,jamkolpardakht From facHeder where CodefacHeder={CodeFacheder}";
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    SH_Noskhe = Convert.ToInt32(reader["Sh_noskhe"].ToString());
                                    jamkolpardakht = Convert.ToInt64(reader["jamkolpardakht"]);
                                }
                            }
                            if (Convert.ToDouble(taminHeder.Subsidy.Replace(",", "").Replace("ريال", "")) > 0)
                            {
                                cmd.CommandText = $@"Declare @Kheyriecode int
                                            Select @Kheyriecode = min(Kheyriecode) from Kheyrie where Kheyrie = 'سهم صعب العلاج تامين اجتماعي'
                                            insert into Sandogh(Sh_Noskhe, Mablagh, UserId, KheyrieCode, State)values({SH_Noskhe}, {Convert.ToDouble(taminHeder.Subsidy.Replace(",", "").Replace("ريال", ""))}, dbo.Fu_GetActiveUser(), @Kheyriecode, 50)";
                                cmd.ExecuteNonQuery();
                            }


                            cmd.CommandText = $"exec sp_updatejamfacheader {CodeFacheder},0";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = $"Select Sh_noskhe,jamkolpardakht From facHeder where CodefacHeder={CodeFacheder}";
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    noskhe.Sh_noskhe= Convert.ToInt32(reader["Sh_noskhe"].ToString());
                                    noskhe.jamkolpardakht = Convert.ToInt64(reader["jamkolpardakht"]);
                                }
                            }
                        }
                        con.Close();                                            
                }
            }
            return noskhe;
        }
    }
}
