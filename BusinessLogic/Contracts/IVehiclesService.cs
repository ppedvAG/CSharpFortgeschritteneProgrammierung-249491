﻿using BusinessLogic.Data;

namespace BusinessLogic.Contracts
{
    public interface IVehiclesService
    {
        string Create(Car car);
        bool Delete(Guid id);
        IEnumerable<Car> GetAll();
        Car? GetById(Guid id);
        bool Update(Guid id, Car car);
    }
}