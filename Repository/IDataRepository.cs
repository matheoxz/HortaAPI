using System;
using HortaIoT.Models;
using System.Collections.Generic;

namespace HortaIoT.Repository{

    public interface IDataRepository{
        
        List<DataModel> GetAll();

        List<DataModel> GetRange(DateTimeOffset start, DateTimeOffset end);

        List<DataModel> GetByCultivation(string cultivationName);

        void Add(DataModel data);
    }
}