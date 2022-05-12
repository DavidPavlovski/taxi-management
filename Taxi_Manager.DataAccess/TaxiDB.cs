using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Taxi_Manager.DataAccess.Interfaces;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Helpers;

namespace Taxi_Manager.DataAccess
{
    public class TaxiDB<T> : ITaxiDB<T> where T : BaseEntity
    {
        private readonly string _dataDirectory;
        private readonly string _dataFile;
        private int IdCounter { get; set; }
        public TaxiDB()
        {

            _dataDirectory = @"Data";

            _dataFile = _dataDirectory + @$"\{typeof(T).Name}Data.json";
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
            if (!File.Exists(_dataFile))
            {
                File.Create(_dataFile).Close();
            }

            List<T> data = ReadFromFile();
            if (data == null)
            {
                IdCounter = 0;
                WriteToFile(new List<T>());
            }
            else if (data.Count > 0)
            {
                IdCounter = data.Max(x => x.Id);
            }
        }

        private List<T> ReadFromFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(_dataFile))
                {
                    string jsonData = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<T>>(jsonData);
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                Console.ReadLine();
                return null;
            }
        }

        private bool WriteToFile(List<T> entities)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_dataFile))
                {
                    string data = JsonConvert.SerializeObject(entities, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    });
                    sw.WriteLine(data);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                Console.ReadLine();
                return false;
            }
        }
        public int Add(T entity)
        {
            List<T> data = ReadFromFile();
            entity.Id = ++IdCounter;
            data.Add(entity);
            WriteToFile(data);
            return entity.Id;
        }

        public List<T> GetAll()
        {
            return ReadFromFile();
        }
        public T GetById(int id)
        {
            return ReadFromFile().Single(e => e.Id == id);
        }
        public bool RemoveById(int id)
        {
            try
            {
                List<T> data = ReadFromFile();
                T entity = data.Single(e => e.Id == id);
                data.Remove(entity);
                WriteToFile(data);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Update(T entity)
        {
            try
            {
                List<T> data = ReadFromFile();
                T dbEntity = data.Single(e => e.Id == entity.Id);
                data.Remove(dbEntity);
                data.Add(entity);
                WriteToFile(data.OrderBy(x => x.Id).ToList());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
