﻿namespace My {
    using System.Windows.Media;

    public class DuckTyped {
        public string Country { get; set; }
        public string Language { get; set; }
        public string Capital { get; set; }
        public double Area { get; set; }
        public double PopulationDensity { get; set; }
        public string AreaUnits { get; set; }
        public string PopulationDensityUnits { get; set; }
        public override string ToString() {
            (string name, string value) country = (nameof(Country), Country);
            (string name, string value) language = (nameof(Language), Language);
            (string name, string value) capital = (nameof(Capital), Capital);
            (string name, string value) area = (nameof(area), Area.ToString() + AreaUnits);
            (string name, string value) populationDensity = (nameof(PopulationDensity), PopulationDensity.ToString() + PopulationDensityUnits);
            return DefinitionSet.Dump(GetType().Name, country, language, capital, area, populationDensity);
        }
    } //class DuckTyped

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

    /*
    Main, duck:
    Country: Italy !Italiano Italia
    Language: Italian !Italiano Italiano
    Capital: Rome !Italiano Roma
    Area: 301,230 square kilometres !Italiano 
    PopulationDensity
    AreaUnits
    PopulationDensityUnits

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
    RacingColor: Red !Italiano Rosso corsa
    Tragedy: Romeo and Juliet !Italiano Romeo e Giulietta
    Comedy: The Servant of Two Masters !Italiano Il servitore di due padroni
    */

}