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
            String model = entity.GetType().Name;
            String guid = (entity as MobeelizerWp7Model).guid;
            var query = from meta in db.ModelMetadata where meta.Model == model && meta.Guid == guid select meta;
            MobeelizerModelMetadata metadata = query.Single();
            metadata.Modyfied = 1;
            metadata.Deleted = 1;
            //this.table.DeleteOnSubmit(entity);
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
            get { return this.table.ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get 
            {
                var query = from record in this.table join m in db.ModelMetadata on record.guid equals m.Guid where m.Deleted == 0 select record;
                return query.Expression;
            }
        }

        public IQueryProvider Provider
        {
            get { return this.table.Provider; }
        }
    }


}
