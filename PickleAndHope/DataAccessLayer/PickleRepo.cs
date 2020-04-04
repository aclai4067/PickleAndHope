using PickleAndHope.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickleAndHope.DataAccessLayer
{
    public class PickleRepo
    {
        static List<Pickle> _pickles = new List<Pickle>() 
        { 
            new Pickle() 
            { 
                Id = 1, 
                Type = "Dill", 
                NumberInStock = 9, 
                Price = 1.25m, 
                Size = "large" 
            } 
        };

        public void Add(Pickle pickle)
        {
            pickle.Id = _pickles.Max(p => p.Id) + 1;
            _pickles.Add(pickle);
        }

        public void Remove(string type)
        {
            throw new NotImplementedException();
        }

        public Pickle Update(Pickle pickle)
        {
            var pickleToUpdate = GetByType(pickle.Type);
            pickleToUpdate.NumberInStock += pickle.NumberInStock;
            return pickleToUpdate;
        }

        public Pickle GetByType(string type)
        {
            return _pickles.FirstOrDefault(p => p.Type == type);
        }

        public List<Pickle> GetAll()
        {
            return _pickles;
        }

        public Pickle GetById(int id)
        {
            return _pickles.FirstOrDefault(p => p.Id == id);
        }
    }
}
