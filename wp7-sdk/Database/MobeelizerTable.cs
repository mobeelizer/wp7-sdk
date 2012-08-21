using System;
using System.Collections.Generic;
using System.Data.Linq;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Linq;
using System.Linq.Expressions;

namespace Com.Mobeelizer.Mobile.Wp7.Database 
{
    internal class MobeelizerTable<T>: ITable<T> where T : MobeelizerWp7Model
    {
        private ITable<T> table;

        private MobeelizerDatabaseContext db;

        internal MobeelizerTable(ITable<T> table, MobeelizerDatabaseContext db)
        {
            this.table = table;
            this.db = db;
        }
   
        public void Attach(T entity)
        {
            this.table.Attach(entity);
        }

        public void DeleteOnSubmit(T entity)
        {
            String guid = entity.Guid;
            var query = from MobeelizerWp7Model record in this.table where record.Guid == guid select record;
            var result = query.Single();
            result.Deleted = true;
        }

        public void InsertOnSubmit(T entity)
        {
            this.table.InsertOnSubmit(entity);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return table.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Type ElementType
        {
            get 
            { 
                return this.table.ElementType; 
            }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get 
            {   
                var query = (from record in this.table where record.Deleted == false select record);
                return query.Expression;
            }
        }


        public IQueryProvider Provider
        {
            get 
            { 
                return this.table.Provider; 
            }
        }
    }

}
