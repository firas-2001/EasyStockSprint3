namespace EasyStock.Models
{
    public static class MouvementType
    {
        public const string Entree = "Entree";
        public const string Sortie = "Sortie";
        public const string Retour = "Retour";

        public static IReadOnlyList<string> All => new[] { Entree, Sortie, Retour };

        public static string ToDisplay(string typeMouvement)
        {
            return typeMouvement switch
            {
                Entree => "Entrée",
                Sortie => "Sortie",
                Retour => "Retour",
                _ => typeMouvement
            };
        }
    }
}
