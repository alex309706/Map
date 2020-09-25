using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core;
namespace Interfaces
{
    public interface ISubdivisionRepository:IDisposable
    {
        IEnumerable<Subdivision> GetSubdivisions();
        Subdivision GetSubdivision(int id);
        void Create(Subdivision item);

        void Update(Subdivision item);
        void Delete(int id);
        void Save();

    }
}
