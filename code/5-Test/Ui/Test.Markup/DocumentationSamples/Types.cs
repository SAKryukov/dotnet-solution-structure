namespace My {
    using System.Windows.Media;

    class DuckTyped {
        internal string Country { get; set; }
        internal string Language { get; set; }
        internal string Capital { get; set; }
        internal double Area { get; set; }
        internal double PopulationDensity { get; set; }
        internal string AreaUnits { get; set; }
        internal string PopulationDensityUnits { get; set; }
        public override string ToString() {
            (string name, string value) country = (nameof(Country), Country);
            (string name, string value) language = (nameof(Language), Language);
            (string name, string value) capital = (nameof(Capital), Capital);
            (string name, string value) area = (nameof(area), Area.ToString() + AreaUnits);
            (string name, string value) populationDensity = (nameof(PopulationDensity), PopulationDensity.ToString() + PopulationDensityUnits);
            return DefinitionSet.Dump(GetType().Name, country, language, capital, area, populationDensity);
        }
    } //class DuckTyped

    class Detail {
        internal string City { get; set; }
        public static string Mountains = null;
        internal int Provinces { get; set; }
        public int MetropolitanCities = 0;
        internal Color[] FlagColors { get; set; }
        public override string ToString() {
            (string name, string value) city = (nameof(City), City);
            (string name, string value) mountains = (nameof(Mountains), Mountains);
            (string name, string value) provinces = (nameof(Provinces), Provinces.ToString());
            (string name, string value) metropolitanCities = (nameof(MetropolitanCities), MetropolitanCities.ToString());
            return DefinitionSet.Dump(GetType().Name, city, mountains, provinces, metropolitanCities);
        } //Detail
    } //class Detail

    class Fun {
        internal string Animal { get; set; }
        internal string Dish { get; set; }
        public string Festival = null;
        internal string Tragedy { get; set; }
        internal string Comedy { get; set; }
        public override string ToString() {
            (string name, string value) animal = (nameof(Animal), Animal);
            (string name, string value) dish = (nameof(Dish), Dish);
            (string name, string value) festival = (nameof(Festival), Festival);
            (string name, string value) tragedy = (nameof(Tragedy), Tragedy);
            (string name, string value) comedy = (nameof(Comedy), Comedy);
            return DefinitionSet.Dump(GetType().Name, animal, dish, festival, tragedy, comedy);
        }
    } // class Fun

    /*
    Main, duck:
    Country: Italy !Italiano Italia
    Language: Italian !Italiano Italiano
    Capital: Rome !Italiano Roma
    Area: 301,230 square kilometres !Italiano 

    Detail:
    City: Milan !Italiano Milano
    Mountains Alps !Italiano Alpi
    Provinces: 107
    MetropolitanCities: 14
    Flag Colors: Green White Red

    Fun:
    Animal: |Italiano Mediterranean buffalo Italiano Bufalo mediterraneo italiano
    Dish: Lasagna !Italiano Lasagna Lasagne al forno
    Festival: Venice Film Festival !Italiano Mostra internazionale d'arte cinematografica
    Tragedy: Romeo and Juliet !Italiano Romeo e Giulietta
    Comedy: The Servant of Two Masters !Italiano Il servitore di due padroni
    */

}
