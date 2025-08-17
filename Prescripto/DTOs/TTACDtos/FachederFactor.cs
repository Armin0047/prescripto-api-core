namespace Prescripto.DTOs.TTACDtos
{
    public class FachederFactor
    {
        public int codem { get; set; }
        public int anbcode { get; set; }
        public int datefac { get; set; }
        public int? facgroup { get; set; }
        public string? Shfacmoshtari { get; set; }
        public string? sharh { get; set; }
        public string? datetasvieh { get; set; }
        public decimal jamkol { get; set; }
        public decimal Hbarbary { get; set; }
        public decimal Hother { get; set; }
        public decimal takhfif { get; set; }
        public decimal Hmaliat { get; set; }
        public decimal jamkhales { get; set; }

        public List<FacRadif> Radifs { get; set; }
    }

    public class FacRadif
    {
        public bool sabt { get; set; }
        public bool update { get; set; }
        public int code { get; set; }
        public int tedbasteh { get; set; }
        public int tedDarBasteh { get; set; }
        public int ted { get; set; }
        public decimal ghjoje { get; set; }
        public decimal ghmasraf { get; set; }
        public string serial { get; set; }
        public int ex { get; set; }
        public decimal tkhfif { get; set; }
        public decimal maliat { get; set; }


    }
}
