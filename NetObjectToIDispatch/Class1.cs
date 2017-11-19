using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using NeuralNet_Backpropagation;

namespace NetObjectToIDispatch
{
    public class EnumVariantImpl : IEnumVARIANT
    {
        private const int S_OK = 0;
        private const int S_FALSE = 1;

        IEnumerator enumerator = null;

        public EnumVariantImpl(IEnumerator enumerator)
        {
            this.enumerator = enumerator;
        }

        public IEnumVARIANT Clone()
        {
            throw new NotImplementedException();
        }


        public int Reset()
        {
            enumerator.Reset();
            return S_OK;
        }

        public int Skip(int celt)
        {
            for (; celt > 0; celt--)
                if (!enumerator.MoveNext())
                    return S_FALSE;
            return S_OK;
        }

        public int Next(int celt, object[] rgVar, IntPtr pceltFetched)
        {
            if (celt == 1 && enumerator.MoveNext())
            {


                rgVar[0] = AutoWrap.ОбернутьОбъект(enumerator.Current);
                //   pceltFetched = new IntPtr(1);
                if (pceltFetched != IntPtr.Zero)
                    Marshal.WriteInt32(pceltFetched, 1);


                return S_OK;
            }
            else
            {
                return S_FALSE;
            }
        }
    }

    [ComVisible(true)]
    [ProgId("NetObjectToIDispatch")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [Guid("1BD846DC-63F2-4070-AF23-8AECD6C158CC")]
    public class NetObjectToIDispatch
    {
        public object CreateObject(string type)
        { return new AutoWrap(System.Activator.CreateInstance(Type.GetType(type))); }

        public object CreateObjectWhithParam(string type, Object[] args)
        { return new AutoWrap(System.Activator.CreateInstance(Type.GetType(type), args)); }

        public object CreateArray(string type, int length)
        {
            return new AutoWrap(Array.CreateInstance(Type.GetType(type), length));
        }

        public object Activator
        {
            get { return new AutoWrap(typeof(System.Activator)); }
        }
        public object ПолучитьТип(string type, string путь)
        {
            Type result = Type.GetType(type,
                (aName) => путь.Trim().Length != 0 ?
                    Assembly.LoadFrom(путь) :
                    Assembly.Load(aName),
                (assem, name, ignore) => assem == null ?
                    Type.GetType(name, false, ignore) :
                    assem.GetType(name, false, ignore), true
            );

            return new AutoWrap(result);
        }
        public object ПолучитьИнтерфейс(object obj, string InterfaseName)
        {

            if (obj is AutoWrap)
                obj = ((AutoWrap)obj).O;

            Type type = obj.GetType().GetInterface(InterfaseName, true);
            return new AutoWrap(obj, type);
        }

        public object ПолучитьМассивИзЛистаСтрок(object Лист)
        {
            return new AutoWrap(((List<String>)(((AutoWrap)Лист).O)).ToArray());
        }

        public object ТипКакОбъект(object Тип)
        {
            Type T = ((Type)Тип);

            return new AutoWrap(T, T.GetType());
        }

        public object ПолучитьМетоды(object Тип)
        {
            //   AutoWrap T = ((AutoWrap)Тип);
            //   return T.T.GetMethods();

            return new AutoWrap(((Type)Тип).GetMethods());
        }

        public object GetIntegration1C()
        {
            return new AutoWrap(new Integration1C());
        }

        public object ВыполнитьМетод(object obj, string ИмяМетода)
        {
            if (obj is AutoWrap)
                obj = ((AutoWrap)obj).O;
            Type T = obj.GetType();



            return new AutoWrap(T.InvokeMember(ИмяМетода, BindingFlags.DeclaredOnly |
                                                          BindingFlags.Public | BindingFlags.NonPublic |
                                                          BindingFlags.Instance | BindingFlags.InvokeMethod, null, obj, null));
        }
    }
    [ProgId("AutoWrapNetObjectToIDispatch")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Guid("72EAFB10-099F-4e96-A17E-B67E34DACA53")]
    public class AutoWrap : IReflect
    {
        protected internal object O = null;
        protected internal Type T = null;

        FieldInfo[] Поля = null;
        MemberInfo[] Мемберы = null;
        MethodInfo[] Методы = null;
        PropertyInfo[] Свойства = null;

        BindingFlags staticBinding = BindingFlags.Public | BindingFlags.Static;
        bool ЭтоТип;

        public static object ОбернутьОбъект(object obj)
        {
            if (obj != null)
            {
                if (!(obj.GetType().IsPrimitive
                      || obj.GetType() == typeof(System.Decimal)
                      || obj.GetType() == typeof(System.DateTime)
                      || obj.GetType() == typeof(System.String)))
                    obj = new AutoWrap(obj);
            }
            return obj;
        }
        public AutoWrap() { }
        public AutoWrap(object obj)
        {
            O = obj;
            if (O is Type)
            {
                T = O as Type;
                ЭтоТип = true;
            }
            else
            {
                T = O.GetType();
                ЭтоТип = false;
            }

        }
        public AutoWrap(object obj, Type type)
        {
            O = obj;
            T = type;
            ЭтоТип = false;


        }

        #region IReflect Members
        public System.Reflection.FieldInfo GetField(string name, System.Reflection.BindingFlags bindingAttr)
        {
            if (ЭтоТип)
                return T.GetField(name, staticBinding);
            else
                return T.GetField(name, bindingAttr);
        }    /* SNIP other IReflect methods */

        public object InvokeMember(string name, System.Reflection.BindingFlags invokeAttr, System.Reflection.Binder binder, object target, object[] args, System.Reflection.ParameterModifier[] modifiers, System.Globalization.CultureInfo culture, string[] namedParameters)
        {        // Unwrap any AutoWrap'd objects (they need to be raw if a paramater)

            if (name == "[DISPID=-4]")
            {
                return new EnumVariantImpl(((IEnumerable)O).GetEnumerator());
            }

            if (args != null && args.Length > 0)
            {
                for (int x = 0; x < args.Length; x++)
                {
                    if (args[x] is AutoWrap)
                    { args[x] = ((AutoWrap)args[x]).O; }
                }
            }
            // Invoke whatever needs be invoked!
            object obj;
            if (ЭтоТип)
                obj = T.InvokeMember(name, invokeAttr, binder, null, args, modifiers, culture, namedParameters);
            else
                obj = T.InvokeMember(name, invokeAttr, binder, O, args, modifiers, culture, namedParameters);
            // Wrap any return objects (that are not primative types) 

            //} 

            return ОбернутьОбъект(obj);
        }
        //public object UnderlyingObject
        //{ get { return O; } }
        public Type UnderlyingSystemType
        {
            get { return T.UnderlyingSystemType; }
        }
        #endregion


        public FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            if (Поля != null) return Поля;

            if (ЭтоТип)
                Поля = T.GetFields(staticBinding);
            else
                Поля = T.GetFields(bindingAttr);

            return Поля;
        }

        public MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
        {

            if (ЭтоТип)
                return T.GetMember(name, staticBinding);
            else
                return T.GetMember(name, bindingAttr);


        }

        public MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            if (Мемберы != null) return Мемберы;

            if (ЭтоТип)
                Мемберы = T.GetMembers(staticBinding);
            else
                Мемберы = T.GetMembers(bindingAttr);

            return Мемберы;
        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
        {


            if (ЭтоТип)
                return T.GetMethod(name, staticBinding);
            else
                return T.GetMethod(name, bindingAttr);


        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
        {
            //  throw new NotImplementedException();



            if (ЭтоТип)
                return T.GetMethod(name, staticBinding, binder, types, modifiers);
            else
                return T.GetMethod(name, bindingAttr, binder, types, modifiers);
        }

        public MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            if (Методы != null) return Методы;
            if (ЭтоТип)
                Методы = T.GetMethods(staticBinding);
            else
                Методы = T.GetMethods(bindingAttr);

            return Методы;
        }

        public PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            if (Свойства != null) return Свойства;

            if (ЭтоТип)
                Свойства = T.GetProperties(staticBinding);
            else
                Свойства = T.GetProperties(bindingAttr);

            return Свойства;
        }

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            if (ЭтоТип)
                return T.GetProperty(name, staticBinding, binder, returnType, types, modifiers);
            else
                return T.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
        }

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
        {
            if (ЭтоТип)
                return T.GetProperty(name, staticBinding);
            else
                return T.GetProperty(name, bindingAttr);
        }

    }
}