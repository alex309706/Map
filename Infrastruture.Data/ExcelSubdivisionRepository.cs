using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core;
using Interfaces;



namespace Infrastructure.Data
{
    public class ExcelSubdivisionRepository : ISubdivisionRepository
    {

        readonly Context ctx;

        public ExcelSubdivisionRepository(string path)
        {
            ctx = new Context(path);
        }
        public void Create(Subdivision item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Subdivision GetSubdivision(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Subdivision item)
        {
            throw new NotImplementedException();
        }
        //обработка строк
        public IEnumerable<Subdivision> GetSubdivisions()
        {
            //Считанные строки из Excel
            List<Dictionary<string, string>> list_of_strings = ctx.List_of_strings;

            //координаты со второго листа
            Dictionary<string, Coordinates> coordinates_of_city = ctx.Coordinates_of_city;

            //считанные данные по подразделению
            Dictionary<string, string> read_subdiv = new Dictionary<string, string>();

            //возвращаемый список подразделений
            List<Subdivision> Subdivisions = new List<Subdivision>();

            foreach (Dictionary<string, string> Dictionary in list_of_strings)
            {
                foreach (KeyValuePair<string, string> kp in Dictionary)
                {
                    read_subdiv.Add(kp.Key, kp.Value);
                    string key = kp.Key;
                    int index = key.IndexOf(" ") == -1 ? 0 : key.IndexOf(" ");
                    if (key.Substring(0, index) == "Расположение")
                    {
                        //Первоначальные координаты
                        double x = 0;
                        double y = 0;

                        string location=string.Empty;
                        //добавляемое подразделение в список подразделений
                        Subdivision new_subdiv = new Subdivision(x, y);
                        switch (key)
                        {
                            case "Расположение штаба:":
                                if (read_subdiv.ContainsKey("Расположение штаба:"))
                                {
                                    new_subdiv = new Army(x, y);
                                    location= read_subdiv["Расположение штаба:"];
                                    new_subdiv.Location = location;
                                    if (read_subdiv.ContainsKey("Армия:"))
                                    {
                                        new_subdiv.Name = read_subdiv["Армия:"];
                                    }
                                    if (read_subdiv.ContainsKey("Состав армии:"))
                                    {
                                        new_subdiv.Name = read_subdiv["Состав армии:"];
                                    }
                                }
                                break;
                            case "Расположение дивизии:":
                                if (read_subdiv.ContainsKey("Расположение дивизии:"))
                                {
                                    new_subdiv = new Division(x, y);
                                    location = read_subdiv["Расположение дивизии:"];
                                    new_subdiv.Location = location;
                                    new_subdiv.Composition = read_subdiv["Состав дивизии:"];
                                }
                                break;
                            case "Расположение бригады:":
                                if (read_subdiv.ContainsKey("Расположение бригады:"))
                                {
                                    new_subdiv = new Brigade(x, y);
                                    location= read_subdiv["Расположение бригады:"];
                                    new_subdiv.Location = location;
                                    new_subdiv.Composition = read_subdiv["Состав бригады:"];
                                }
                                break;
                            case "Расположение полка:":
                                if (read_subdiv.ContainsKey("Расположение полка:"))
                                {
                                    new_subdiv = new Regiment(x, y);
                                    location= read_subdiv["Расположение полка:"];
                                    new_subdiv.Location = location;
                                    new_subdiv.Composition = read_subdiv["Состав полка:"];
                                }
                                break;
                            default:
                                break;
                        }
                        if (read_subdiv.ContainsKey("Численность:"))
                        {
                            new_subdiv.Strength = read_subdiv["Численность:"];
                        }

                        if (read_subdiv.ContainsKey("Документ удостоверяющий:"))
                        {
                            new_subdiv.Document = read_subdiv["Документ удостоверяющий:"];
                        }
                        if (read_subdiv.ContainsKey("Командир:"))
                        {
                            new_subdiv.Commander = read_subdiv["Командир:"];
                        }
                        if (read_subdiv.ContainsKey("Командующий:"))
                        {
                            new_subdiv.Commander = read_subdiv["Командующий:"];
                        }
                        //если координаты указаны в таблице, они будут заменены
                        if (coordinates_of_city.ContainsKey(location))
                        {
                            new_subdiv.coord.X = coordinates_of_city[location].X;
                            new_subdiv.coord.Y = coordinates_of_city[location].Y;
                        }
                        //добавление в список подразделений
                        Subdivisions.Add(new_subdiv);
                        //
                        read_subdiv.Clear();
                    }
                }
            }
            return Subdivisions;
        }
      
    }
}
