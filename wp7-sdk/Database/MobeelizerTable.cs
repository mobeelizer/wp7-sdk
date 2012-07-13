using System;
using System.Collections.Generic;
using System.Data.Linq;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Linq;
using System.Linq.Expressions;

namespace Com.Mobeelizer.Mobile.Wp7.Database 
{
    public class MobeelizerTable<T>: ITable<T> where T : MobeelizerWp7Model
    {
        private ITable<T> table;

        internal MobeelizerTable(ITable<T> table)
        {
            this.table = table;
        }
   
        public void Attach(T entity)
        {
            this.table.Attach(entity);
        }

        public void DeleteOnSubmit(T entity)
        {
            this.table.DeleteOnSubmit(entity);
        }

        public void InsertOnSubmit(T entity)
        {
            this.table.InsertOnSubmit(entity);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MobeelizerTableEnumerator<T>(table.GetEnumerator());
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
                System.Linq.Expressions.Expression expression = this.table.Expression;
                object value = (expression as ConstantExpression).Value;
                return expression;  
            }
        }

        public IQueryProvider Provider
        {
            get { return new MobeelizerQueryProvider(this.table.Provider); }
        }
    }


    public class MobeelizerQueryProvider : IQueryProvider
    {
        private IQueryProvider provider;

        internal MobeelizerQueryProvider(IQueryProvider provider)
        {
            this.provider = provider;
        }

        public IQueryable<TElement> CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
        {
            return new MobeelizerQuerable<TElement>(this.provider.CreateQuery<TElement>(expression));
        }

        public IQueryable CreateQuery(System.Linq.Expressions.Expression expression)
        {
            return this.provider.CreateQuery(expression);
        }

        public TResult Execute<TResult>(System.Linq.Expressions.Expression expression)
        {
            return this.provider.Execute<TResult>(expression);
        }

        public object Execute(System.Linq.Expressions.Expression expression)
        {
            return this.provider.Execute(expression);
        }
    }

    public class MobeelizerQuerable<T> : IQueryable<T>
    {
        private IQueryable<T> querable;

        internal MobeelizerQuerable(IQueryable<T> querable)
        {
            this.querable = querable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return querable.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return querable.GetEnumerator();
        }

        public Type ElementType
        {
            get { return querable.ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return this.querable.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return this.querable.Provider; }
        }
    }
}
