
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OfficeOpenXml.Table;

namespace CashmereUtil.Reporting.MSExcel
{
    public static class EPPLUSExtentions
    {
        public static ExcelRangeBase LoadFromCollectionFiltered<T>(
          this ExcelRangeBase @this,
          IEnumerable<T> collection)
          where T : class
        {
            MemberInfo[] array = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => !Attribute.IsDefined(p, typeof(EpplusIgnore))).ToArray();
            return @this.LoadFromCollectionFiltered<T>(collection, false, TableStyles.None, BindingFlags.Instance | BindingFlags.Public, array);
        }

        public static ExcelRangeBase LoadFromCollectionFiltered<T>(
          this ExcelRangeBase @this,
          IEnumerable<T> collection,
          bool PrintHeaders,
          TableStyles TableStyle)
          where T : class
        {
            MemberInfo[] array = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => !Attribute.IsDefined(p, typeof(EpplusIgnore))).ToArray();
            return @this.LoadFromCollectionFiltered<T>(collection, PrintHeaders, TableStyle, BindingFlags.Instance | BindingFlags.Public, array);
        }

        public static ExcelRangeBase LoadFromCollectionFiltered<T>(
          this ExcelRangeBase @this,
          IEnumerable<T> Collection,
          bool PrintHeaders,
          TableStyles TableStyle,
          BindingFlags memberFlags,
          MemberInfo[] Members)
          where T : class
        {
            typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => !Attribute.IsDefined(p, typeof(EpplusIgnore))).ToArray();
            return @this.LoadFromCollection(Collection, PrintHeaders, TableStyle, memberFlags, Members);
        }
    }
}
