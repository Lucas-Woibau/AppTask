using AppTask.Database;
using AppTask.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTask.Repositories
{
    public class TaskModelRepository : ITaskModelRepository
    {
        private AppTaskContext _db;
        public TaskModelRepository()
        {
            _db = new AppTaskContext();
        }
        public IList<TaskModel> GetAll()
        {
            return _db.Tasks.OrderByDescending(o => o.PrevisionDate).ToList();
        }

        public TaskModel GetById(int id)
        {
            return _db.Tasks.Include(a => a.SubTasks).FirstOrDefault(x => x.Id == id)!;
        }

        public void Add(TaskModel task)
        {
            _db.Tasks.Add(task);
            _db.SaveChanges();
        }
        public void Update(TaskModel task)
        {
            _db.Tasks.Update(task);
            _db.SaveChanges();
        }

        public void Delete(TaskModel task)
        {
            GetById(task.Id);

            foreach (var subtask in task.SubTasks)
            {
                _db.Remove(subtask);
            }
            _db.Tasks.Remove(task);
            _db.SaveChanges();
        }
    }
}
