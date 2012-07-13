using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    public class MobeelizerErrorsHolder
    {

        internal bool IsValid
        {
            get
            {
                //TODO
                return true;
            }
        }

        internal void AddFieldCanNotBeEmpty(string field)
        {
            throw new NotImplementedException();
        }

        internal void AddFieldMissingReferenceError(string field, string guid)
        {
            throw new NotImplementedException();
        }

        internal void AddIncorectValue(string p)
        {
            throw new NotImplementedException();
        }

        internal void AddFieldMustBeLessThanOrEqualTo(string p, double maxValue)
        {
            throw new NotImplementedException();
        }

        internal void AddFieldMustBeLessThan(string p, double maxValue)
        {
            throw new NotImplementedException();
        }

        internal void AddFieldMustBeGreaterThanOrEqual(string p, double minValue)
        {
            throw new NotImplementedException();
        }

        internal void AddFieldMustBeGreaterThan(string p, double minValue)
        {
            throw new NotImplementedException();
        }

        internal void AddFieldIsTooLong(string p, int maxLength)
        {
            throw new NotImplementedException();
        }
    }
}
