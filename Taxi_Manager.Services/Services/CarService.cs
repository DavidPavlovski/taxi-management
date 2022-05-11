using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Domain.Enums;
using Taxi_Manager.Helpers;
using Taxi_Manager.Services.Interfaces;

namespace Taxi_Manager.Services.Services
{
    public class CarService : BaseService<Car>, ICarService
    {
        public List<Car> GetAvailableCarsForShift(Shift shift)
        {
            return Db.GetAll().Where(x => x.HasValidLicence() && x.IsAvailableForShif(shift)).ToList();
        }
        public void CheckLicenceStatus()
        {
            Db.GetAll().ForEach(x => x.CheckLicencePlateExpiration());
        }

        public Car SelectCarToAssign(Shift selectedShift, IUIService uiService)
        {
            List<Car> filteredCars = GetAvailableCarsForShift(selectedShift);
            if(filteredCars.Count == 0)
            {
                throw new Exception($"No cars available for {selectedShift} shift.");
            }
            while (true)
            {
                try
                {
                    Console.Clear();
                    uiService.PrintEntites(filteredCars);
                    int index = ConsoleHelper.GetNumberInput(ConsoleHelper.GetInput("Enter car index : "), filteredCars.Count);
                    return filteredCars[index - 1];
                }
                catch (Exception ex)
                {
                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                }
            }
        }
    }
}
