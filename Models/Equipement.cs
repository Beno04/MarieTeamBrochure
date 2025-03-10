namespace MarieTeamBrochure.Models
{
    public class Equipement
    {
        public string Id { get; set; }
        public string Desc { get; set; }

        public Equipement(string id_equip, string desc_equip)
        {
            Id = id_equip;
            Desc = desc_equip;
        }
    }
}
