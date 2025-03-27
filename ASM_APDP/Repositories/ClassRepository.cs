using ASM_APDP.Data;
using ASM_APDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASM_APDP.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly DatabaseContext _context;

        public ClassRepository(DatabaseContext context)
        {
            _context = context;
        }

        public int Create(Class classEntity)
        {
            try
            {
                _context.Classes.Add(classEntity);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var classEntity = _context.Classes.Find(id);
                if (classEntity == null)
                {
                    return false;
                }
                _context.Classes.Remove(classEntity);
                return _context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Class> GetAll()
        {
            try
            {
                return _context.Classes.ToList();
            }
            catch (Exception)
            {
                return Enumerable.Empty<Class>();
            }
        }

        public Class GetClassByID(int id)
        {
            try
            {
                return _context.Classes.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Class GetClassByName(string className)
        {
            try
            {
                return _context.Classes.FirstOrDefault(c => c.ClassName == className);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Update(Class classEntity)
        {
            try
            {
                _context.Classes.Update(classEntity);
                return _context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}