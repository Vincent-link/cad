using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RegulatoryModel.Command
{
   public class ReflectionClass
    {
      
            /// <summary>
            /// 创建对象实例
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="fullName">命名空间.类型名</param>
            /// <param name="assemblyName">程序集</param>
            /// <returns></returns>
            public static T CreateInstance<T>(string fullName, string assemblyName)
            {
                string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
                Type o = Type.GetType(path);//加载类型
                object obj = Activator.CreateInstance(o, true);//根据类型创建实例
                return (T)obj;//类型转换并返回
            }


            /// <summary>
            /// 创建对象字符串
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="fullName">命名空间.类型名</param>
            /// <param name="assemblyName">程序集</param>
            /// <returns></returns>
            public static string GetAllPropertyInfo<T>(T t, string type)
            {
                string result = "#" + type + "#";
                PropertyInfo[] pps = GetPropertyInfos(t.GetType());
                foreach (var pp in pps)
                {
                    try
                    {
                        object value = pp.GetValue(t, null);
                        if (!value.GetType().IsValueType && value is Autodesk.AutoCAD.Geometry.Entity2d)
                        {
                            result += "\"" + pp.Name + "\":\"#" + GetAllPropertyInfo(t, type) + "#\","; ;
                        }
                        else
                        {
                            result += "\"" + pp.Name + "\":\"" + value.ToString() + "\",";
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                return result.TrimEnd(',');
            }
            /// <summary>
            /// 创建对象字符串
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="fullName">命名空间.类型名</param>
            /// <param name="assemblyName">程序集</param>
            /// <returns></returns>
            public static string GetAllPropertyInfoEx<T>(T t, string type)
            {
                string result = "";
                //string result = "#" + type + "#";
                PropertyInfo[] pps = GetPropertyInfos(t.GetType());
                foreach (var pp in pps)
                {
                    try
                    {
                        object value = pp.GetValue(t, null);
                        if (!value.GetType().IsValueType)//&& value is Autodesk.AutoCAD.Geometry.Entity2d)
                        {
                            result += "\"" + pp.Name + "\":\"#" + GetAllPropertyInfo(t, type) + "#\","; ;
                        }
                        else
                        {
                            result += "\"" + pp.Name + "\":\"" + value.ToString() + "\",";
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                return result.TrimEnd(',');
            }
            // <summary>
            /// 创建对象实例
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="fullName">命名空间.类型名</param>
            /// <param name="assemblyName">程序集</param>
            /// <returns></returns>
            public static T CreateInstance<T>(Type tp)
            {
                object obj = Activator.CreateInstance(tp, true);//根据类型创建实例
                return (T)obj;//类型转换并返回
            }

            /// <summary>
            /// 创建对象实例
            /// </summary>
            /// <typeparam name="T">要创建对象的类型</typeparam>
            /// <param name="assemblyName">类型所在程序集名称</param>
            /// <param name="nameSpace">类型所在命名空间</param>
            /// <param name="className">类型名</param>
            /// <returns></returns>
            public static T CreateInstance<T>(string assemblyName, string nameSpace, string className)
            {
                try
                {
                    string fullName = nameSpace + "." + className;//命名空间.类型名
                                                                  //此为第一种写法
                    object ect = Assembly.Load(assemblyName).CreateInstance(fullName);//加载程序集，创建程序集里面的 命名空间.类型名 实例
                    return (T)ect;//类型转换并返回
                                  //下面是第二种写法
                                  //string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
                                  //Type o = Type.GetType(path);//加载类型
                                  //object obj = Activator.CreateInstance(o, true);//根据类型创建实例
                                  //return (T)obj;//类型转换并返回
                }
                catch
                {
                    //发生异常，返回类型的默认值
                    return default(T);
                }

            }


            public static PropertyInfo[] GetPropertyInfos(Type type)
            {
                return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }

            /// <summary>
            /// 实体属性反射
            /// </summary>
            /// <typeparam name="S">赋值对象</typeparam>
            /// <typeparam name="T">被赋值对象</typeparam>
            /// <param name="s"></param>
            /// <param name="t"></param>
            public static void AutoMapping<S, T>(S s, T t)
            {
                PropertyInfo[] pps = GetPropertyInfos(s.GetType());
                Type target = t.GetType();

                foreach (var pp in pps)
                {
                    PropertyInfo targetPP = target.GetProperty(pp.Name);

                    object value = pp.GetValue(s, null);

                    if (targetPP != null && value != null)
                    {
                        targetPP.SetValue(t, value, null);
                    }
                }
            }

       

            public static string CallingMethod(string assName, string className, string funName)
            {
                try
                {
                    // 1.Load(命名空间名称)，GetType(命名空间.类名)

                    Type type = string.IsNullOrEmpty(assName) ? Assembly.GetExecutingAssembly().GetType(className) : Assembly.Load(className.Split('.')[0]).GetType(className);
                    //2.GetMethod(需要调用的方法名称)
                    MethodInfo method = type.GetMethod(funName);
                    // 3.调用的实例化方法（非静态方法）需要创建类型的一个实例
                    object obj = Activator.CreateInstance(type);
                    //4.方法需要传入的参数
                    // object[] parameters = new object[] { "" };
                    // 5.调用方法，如果调用的是一个静态方法，就不需要第3步（创建类型的实例）
                    // 相应地调用静态方法时，Invoke的第一个参数为null
                    string result = (string)method.Invoke(obj, null);
                    // string strs = GetInvokeInfo("SayHello(string name)", result);
                    return result;
                }
                catch
                {
                    return "";
                }

            }
            public static string CallingMethod(string assName, string className, string funName, string para)
            {
                try
                {
                    // 1.Load(命名空间名称)，GetType(命名空间.类名)

                    Type type = string.IsNullOrEmpty(assName) ? Assembly.GetExecutingAssembly().GetType(className) : Assembly.Load(className.Split('.')[0]).GetType(className);
                    //2.GetMethod(需要调用的方法名称)
                    MethodInfo method = type.GetMethod(funName, new Type[] { typeof(string) });
                    // 3.调用的实例化方法（非静态方法）需要创建类型的一个实例
                    object obj = Activator.CreateInstance(type);
                    //4.方法需要传入的参数
                    object[] parameters = new object[] { para };
                    // 5.调用方法，如果调用的是一个静态方法，就不需要第3步（创建类型的实例）
                    // 相应地调用静态方法时，Invoke的第一个参数为null
                    string result = (string)method.Invoke(obj, parameters);
                    // string strs = GetInvokeInfo("SayHello(string name)", result);
                    return result;
                }
                catch
                {
                    return "";
                }

            }
            /// <summary>
            /// 调用方法输出信息
            /// </summary>
            /// <param name="method">调用方法名</param>
            /// <param name="str">返回信息</param>
            /// <returns></returns>
            public static string GetInvokeInfo(string method, string str)
            {
                string result = string.Format("调用方法：{0}，输出：{1}", method, str);
                return result;
            }
            private static void AddList<T>(T t, object item)
            {
                //这段可以忽略 就是要获得list这个成员信息 FieldInfo

                //FieldInfo fInfo = t.GetType().GetField(ToLowerFirst(className) + "List");

                ////列表类型

                //Type classListType = fInfo.FieldType;

                ////根据成员的类型创建列表 List<T> list = new List<T>();
                //object entityList = Activator.CreateInstance(classListType);

                //BindingFlags flag = BindingFlags.Instance | BindingFlags.Public;
                //MethodInfo methodInfo = classListType.GetMethod("Add", flag);
                //methodInfo.Invoke(entityList, new object[] {item });//相当于List<T>调用Add方法
            }
            public static Type FindType(string assName, string className)
            {
                Type[] types = Assembly.Load(assName).GetTypes();

                ////    遍历类
                for (int i = 0; i < types.Length; i++)
                {
                    if (types[i].Name.Equals(className))
                    {
                        return types[i];
                    }

                }
                return null;
            }


            public static string AnalysisLayer()
            {
                string str = "";
                return str;
            }
            public static string AnalysisBlock()
            {
                string str = "";
                return str;
            }

        }
    
}
