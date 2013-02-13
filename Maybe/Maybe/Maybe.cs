using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartClasses
{
    public static class Maybe
    {
        /// <summary>
        /// Возвращает объект, если он не null, в противном случае не возвращает ничего.
        /// </summary>
        /// <example>
        /// string postCode = this.With(x => person)
        ///     .With(x => x.Address)
        ///     .With(x => x.PostCode);
        /// </example>
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o);
        }

        /// <summary>
        /// Возвращает failureValue, если объект is null
        /// </summary>
        /// <example>
        /// bool is_null = this.With(x => person).With(x => x.Address)
        ///     .IsNull();
        /// </example>
        public static bool Return<TInput>(this TInput o) 
            where TInput : class
        {
            return o == null;
        }

        /// <summary>
        /// Возвращает true, если объект is null
        /// </summary>
        /// <example>
        /// string postCode = this.With(x => person).With(x => x.Address)
        ///     .Return(x => x.PostCode, string.Empty);
        /// </example>
        public static TResult IsNull<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failureValue)
            where TInput : class
        {
            if (o == null) return failureValue;
            return evaluator(o);
        }

        /// <summary>
        /// Возвращает объект, если выполняется условие
        /// </summary>
        /// <example>
        /// string postCode = this.With(x => person)
        ///     .With(x => x.Address)
        ///     .With(x => x.PostCode)
        ///     .If(x => x.Length > 10);
        /// </example>
        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? o : null;
        }
        /// <summary>
        /// Возвращает объект, если не выполняется условие
        /// </summary>
        /// <example>
        /// string postCode = this.With(x => person)
        ///     .With(x => x.Address)
        ///     .With(x => x.PostCode)
        ///     .Unless(x => x.Length > 10);
        /// </example>
        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
          where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? null : o;
        }

        /// <summary>
        /// Возвращает объект, если выполнилось деййствие action
        /// </summary>
        /// <example>
        /// string postCode = this.With(x => person)
        ///     .If(x => HasMedicalRecord(x))]
        ///     .With(x => x.Address)
        ///     .Do(x => CheckAddress(x));
        /// </example>
        public static TInput Do<TInput>(this TInput o, Action<TInput> action, out Exception exception)
            where TInput : class
        {
            TInput l_obj;
            Exception l_e;
            if (o == null)
            {
                l_obj = null;
                l_e = null;
            }
            else
            {
                try
                {
                    action(o);
                    l_e = null;
                    l_obj = o;
                }
                catch (Exception e)
                {
                    l_e = e;
                    l_obj = null;
                };
            }
            exception = l_e;
            return l_obj;
        }


    }
}
