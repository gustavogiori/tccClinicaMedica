using Giori_Consul.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Giori_Consul.Services
{
    public static class QueryService<T>
    {

        public static List<T> GetListFromFilter(string campo, string valor, IQueryable<T> itens)
        {
            // recupera o objeto IQueryable da Context do EF
            IQueryable<T> query = itens;
            // cria alias do objecto Lambda
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            // obtem tipo da propriedade
            Type type;
            var _propertyNames = campo.Split('.');
            if (_propertyNames.Length > 1)
                type = typeof(T).GetProperty(_propertyNames[0]).PropertyType.GetProperty(_propertyNames[1]).PropertyType;
            else
                type = typeof(T).GetProperty(campo).PropertyType;
            // cria Expression para o campo
            MemberExpression propertyExpression = Expression.PropertyOrField(param, campo);
            // cria Expression para o valor
            ConstantExpression valueExpression = Expression.Constant(Convert.ChangeType(valor, type), type);
            MethodInfo methodInfo;
            if (type == typeof(string))
                methodInfo = type.GetMethod("Contains", new[] { type });
            else
                methodInfo = type.GetMethod("Equals", new[] { type });

            var predicate = Expression.Lambda<Func<T, bool>>(
                // aplica tipo de Filtro, no caso do exemplo Contains (LIKE campo '%valor%')
                Expression.Call(propertyExpression, methodInfo, valueExpression)
            , param);

            // adiciona predicate ao where da query
            query = query.Where(predicate);
            // executa a consulta no banco de dados
            var result = query.ToList();

            return result;

        }

        public static IList<SelectListItem> GetItensFilter(Type type)
        {

            IList<SelectListItem> time = new List<SelectListItem>();
            foreach (var item in type.GetProperties())
            {
                var customProperty = item.CustomAttributes.ToList();
                string text = string.Empty;
                if (customProperty.Count > 0)
                {
                    var customName = customProperty.FirstOrDefault(x => x.AttributeType.Name == nameof(DisplayAttribute));
                    var ignore = customProperty.FirstOrDefault(x => x.AttributeType.Name == nameof(IgnoreListFilterAttribute));
                    if (customName != null)
                    {
                        if (customName.NamedArguments.Count > 0)
                        {
                            text = customName.NamedArguments[0].TypedValue.Value.ToString();
                        }
                    }
                    if (ignore != null)
                    {
                        continue;
                    }
                }
                var display = text == string.Empty ? item.Name : text;
                var select = new SelectListItem { Text = display, Value = item.Name };
                time.Add(select);
            }
            return time;
        }
    }
}