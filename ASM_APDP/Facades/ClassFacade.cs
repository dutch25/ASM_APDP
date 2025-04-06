using ASM_APDP.Data;
using ASM_APDP.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_APDP.Facades
{
    public class ClassFacade : IClassFacade
    {
        private readonly DatabaseContext _context;

        public ClassFacade(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Class> GetAllClasses()
        {
            return _context.Classes
                .Include(c => c.User)
                .Include(c => c.Course)
                .ToList();
        }

        public Class GetClassById(int id)
        {
            return _context.Classes
                .Include(c => c.User)
                .Include(c => c.Course)
                .FirstOrDefault(c => c.ClassID == id);
        }

        public Class GetClassByName(string className)
        {
            return _context.Classes
                .Include(c => c.User)
                .Include(c => c.Course)
                .FirstOrDefault(c => c.ClassName == className);
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