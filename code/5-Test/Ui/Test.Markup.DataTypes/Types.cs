namespace My {
    using System.Windows.Media;

    public class Detail {
        public string City { get; set; }
        public static string Mountains = null;
        public int Provinces { get; set; }
        public int MetropolitanCities = 0;
        public override string ToString() {
            (string name, string value) city = (nameof(City), City);
            (string name, string value) mountains = (nameof(Mountains), Mountains);
            (string name, string value) provinces = (nameof(Provinces), Provinces.ToString());
            (string name, string value) metropolitanCities = (nameof(MetropolitanCities), MetropolitanCities.ToString());
            return DefinitionSet.Dump(GetType().Name, city, mountains, provinces, metropolitanCities);
        } //Detail
    } //class Detail

    public class Fun {
        public string Animal { get; set; }
        public string Dish { get; set; }
        public string Festival = null;
        public string RacingColorName { get; set; }
        public Color RacingColor { get; set; }
        public string Tragedy { get; set; }
        public string Comedy { get; set; }
        public override string ToString() {
            (string name, string value) animal = (nameof(Animal), Animal);
            (string name, string value) dish = (nameof(Dish), Dish);
            (string name, string value) festival = (nameof(Festival), Festival);
            (string name, string value) racingColor = (nameof(RacingColor), RacingColorName + RacingColor.ToString());
            (string name, string value) tragedy = (nameof(Tragedy), Tragedy);
            (string name, string value) comedy = (nameof(Comedy), Comedy);
            return DefinitionSet.Dump(GetType().Name, animal, dish, festival, racingColor, tragedy, comedy);
        }
    } // class Fun

}
