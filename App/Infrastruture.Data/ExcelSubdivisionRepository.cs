using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core;
using Interfaces;
using System.Linq;


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
            IEnumerable<Subdivision> Subdivisions = GetSubdivisions();
            Subdivision output = new Subdivision(0,0);
            if (id < Subdivisions.Count() && id>=0)
            {
                output = Subdivisions.First((obj) => obj.id == id);
            }
            return output;
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

            //координаты городов
            Dictionary<string, Coordinates> coordinates_of_city = ctx.Coordinates_of_city;

            //считанные данные по подразделению
            Dictionary<string, string> read_subdiv = new Dictionary<string, string>();

            //возвращаемый список подразделений
            List<Subdivision> Subdivisions = new List<Subdivision>();
            Random r = new Random();
            int counter_of_id = 1;
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
                                   
                                    if (read_subdiv.ContainsKey("Армия:"))
                                    {
                                        new_subdiv = new Army(x, y);
                                        new_subdiv.Name = read_subdiv["Армия:"];
                                    }
                                    if (read_subdiv.ContainsKey("Состав армии:"))
                                    {
                                        new_subdiv.Composition = read_subdiv["Состав армии:"];
                                    }
                                    if (read_subdiv.ContainsKey("Корпус:"))
                                    {
                                        new_subdiv = new Corps(x, y);
                                        new_subdiv.Name = read_subdiv["Корпус:"];
                                    }
                                    if (read_subdiv.ContainsKey("Состав корпуса:"))
                                    {
                                        new_subdiv.Composition = read_subdiv["Состав корпуса:"];
                                    }
                                    location = read_subdiv["Расположение штаба:"];
                                    new_subdiv.Location = location;
                                }
                                break;
                            case "Расположение дивизии:":
                                if (read_subdiv.ContainsKey("Расположение дивизии:"))
                                {
                                    new_subdiv = new Division(x, y);
                                    new_subdiv.Name = read_subdiv["Дивизия:"];
                                    location = read_subdiv["Расположение дивизии:"];
                                    new_subdiv.Location = location;
                                    new_subdiv.Composition = read_subdiv["Состав дивизии:"];
                                }
                                break;
                            case "Расположение бригады:":
                                if (read_subdiv.ContainsKey("Расположение бригады:"))
                                {
                                    new_subdiv = new Brigade(x, y);
                                    new_subdiv.Name = read_subdiv["Бригада:"];
                                    location = read_subdiv["Расположение бригады:"];
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
                            int count_of_existing_subdivisions = (from s in Subdivisions where s.Location == location select s).Count();
                            if (count_of_existing_subdivisions>=1)
                            {
                                new_subdiv.coord.X = coordinates_of_city[location].X + r.Next(-1,1)*r.NextDouble()+r.Next(-1,1);
                                new_subdiv.coord.Y = coordinates_of_city[location].Y + r.Next(-1,1)*r.NextDouble() + r.Next(-1, 1);
                            }
                            else
                            {
                                new_subdiv.coord.X = coordinates_of_city[location].X;
                                new_subdiv.coord.Y = coordinates_of_city[location].Y;
                            }
                        }
                        new_subdiv.id = counter_of_id;
                        counter_of_id++;
                        new_subdiv.type = new_subdiv.GetType().Name;

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
