namespace Prescripto.DTOs
{
    public class ListKalaNoskhe
    {
        public string ReqNum { get; set; }
        public string RegDate { get; set; }
        public string ReqDate { get; set; }
        public string TotalAmount { get; set; }
        public string PaymentByOrg { get; set; }
        public string PaymentByPatient { get; set; }
        public string Subsidy { get; set; }
        public string NationalCode { get; set; }
        public string CustType { get; set; }
        public string DocID { get; set; }
        public int? codetakh { get; set; }
        public int? codetakh2 { get; set; }
        public string? mobile { get; set; }
        public string? bimarname { get; set; }
        public int? Codesazeman { get; set; }
        public List<RadifKala> Radifs { get; set; }
    }
    public class RadifKala
    {
        public string? sabt { get; set; }
        public string? gencode { get; set; }
        public string? drugName { get; set; }
        public string? tedUsage { get; set; }
        public string? orgShare { get; set; }
        public string? per { get; set; }
        public string? totalAmount { get; set; }
        public string? paymentByOrg { get; set; }
        public string? byYaraneh { get; set; }
        public string? subsidy { get; set; }
        public string? paymentByPatient { get; set; }
        public int? CodeKalaAria { get; set; }
        public string? GencodeAria { get; set; }
        public string? NameAria { get; set; }
        public string? PriceAria { get; set; }
        public string? MojodiAria { get; set; }
        public string? byOrg { get; set; }

    }
}
