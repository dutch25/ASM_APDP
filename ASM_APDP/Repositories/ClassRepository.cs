using ASM_APDP.Data;
using ASM_APDP.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_APDP.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly DatabaseContext _context;

        public ClassRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Class> GetAllClasses()
        {
            return _context.Classes;
        }

        public Class GetClassById(int id)
        {
            return _context.Classes.Find(id);
        }

        public Class GetClassByName(string className)
        {
            return _context.Classes.FirstOrDefault(c => c.ClassName == className);
        }

        public bool CreateClass(Class classEntity)
        {
            _context.Classes.Add(classEntity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateClassAsync(Class classEntity)
        {
            _context.Classes.Update(classEntity);
            return await _context.SaveChangesAsync() > 0;
        }

        public bool DeleteClass(int id)
        {
            var classEntity = _context.Classes.Find(id);
            if (classEntity == null) return false;

            _context.Classes.Remove(classEntity);
            return _context.SaveChanges() > 0;
        }
    }
}

