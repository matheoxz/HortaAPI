using System;
using HortaIoT.Models;
using System.Collections.Generic;

namespace HortaIoT.Repository{

    public interface ICultivationRepository{
        
        List<CultivationModel> GetAll();

        CultivationModel GetByName(string name);

        void Add(CultivationModel cultivation);

        void Update(string name, CultivationModel cultivationModel);

        void Delete(string name);
    }
}