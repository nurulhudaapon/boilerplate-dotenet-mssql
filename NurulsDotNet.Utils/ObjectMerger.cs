using System;
using System.Linq;

namespace NurulsDotNet.Utils
{
  public static class ObjectMerger
  {
    /// <summary>
    /// Merge one object with another
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectA">Base data object (existing data)</param>
    /// <param name="objectB">new data object</param>
    /// <returns></returns>
    public static T MergeObjects<T>(T objectA, T objectB)
    {
      var objResult = Activator.CreateInstance(typeof(T));

      var allProperties = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite);
      foreach (var property in allProperties)
      {
        var isPropertyValueType = property.PropertyType.IsValueType;
        object defaultPropertyValue;

        if (isPropertyValueType) defaultPropertyValue = Activator.CreateInstance(property.PropertyType);
        else defaultPropertyValue = null;

        var objectAPropertyValue = property.GetValue(objectA, null);
        var objectBPropertyValue = property.GetValue(objectB, null);


        var isObjectAPropertyNullOrDefault = objectAPropertyValue == null || objectAPropertyValue == defaultPropertyValue;
        var isObjectBPropertyNullOrDefault = objectBPropertyValue == null || objectBPropertyValue == defaultPropertyValue;

        //set objectB property value to null if type is Guid and value is default
        if (property.PropertyType == typeof(Guid) &&  Guid.Empty.Equals(objectBPropertyValue)) isObjectBPropertyNullOrDefault = true;

        if (!isObjectBPropertyNullOrDefault) property.SetValue(objResult, objectBPropertyValue, null);
        else property.SetValue(objResult, objectAPropertyValue, null);

        if (isObjectAPropertyNullOrDefault) property.SetValue(objResult, objectBPropertyValue, null);

      }
      return (T)objResult;
    }
  }
}
