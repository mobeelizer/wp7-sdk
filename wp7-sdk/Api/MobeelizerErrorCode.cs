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
using System.Collections.Generic;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    public enum MobeelizerErrorCode
    {
        OK,
        EMPTY,
        TOO_LONG,
        GREATER_THAN,
        GREATER_THAN_OR_EQUAL_TO,
        LESS_THAN,
        LESS_THAN_OR_EQUAL_TO,
        NOT_FOUND
    }

    public static class MobeelizerErrorCodeExtension
    {
        private static Dictionary<MobeelizerErrorCode, String> errorMessages = new Dictionary<MobeelizerErrorCode, string>()
        {
            {MobeelizerErrorCode.EMPTY, "Value can't be empty."},
            {MobeelizerErrorCode.TOO_LONG,"Value is too long (maximum is {0} characters)."},
            {MobeelizerErrorCode.GREATER_THAN,"Value must be greater than {0}."},
            {MobeelizerErrorCode.GREATER_THAN_OR_EQUAL_TO,"Value must be greater than or equal to {0}."},
            {MobeelizerErrorCode.LESS_THAN,"Value must be less than {0}."},
            {MobeelizerErrorCode.LESS_THAN_OR_EQUAL_TO,"Value must be less than or equal to {0}."},
            {MobeelizerErrorCode.NOT_FOUND,"Relation '{0}' must exist."}
        };

        public static String GetMessage(this MobeelizerErrorCode errorCode)
        {
            return errorMessages[errorCode];
        }
    }
}
