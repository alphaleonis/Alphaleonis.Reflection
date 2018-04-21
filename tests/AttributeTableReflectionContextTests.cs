using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Alphaleonis.Reflection;
using System.Diagnostics;

namespace Tests.Alphaleonis.Reflection
{
   [TestClass]
   public class AttributeTableReflectionContextTests
   {
      #region Utility Methods

      private IEnumerable<TestAttribute> CreateTestAttributes<T>()
      {
         return CreateTestAttributes(typeof(T).Name);
      }

      private IEnumerable<TestAttribute> CreateTestAttributes(Type type)
      {
         return CreateTestAttributes(type.Name);
      }

      private IEnumerable<TestAttribute> CreateTestAttributes(string name)
      {
         return new TestAttribute[] {
               new InheritedMultiAttribute(name),
               new InheritedSingleAttribute(name),
               new NonInheritedMultiAttribute(name),
               new NonInheritedSingleAttribute(name)
         };
      }

      private ICustomAttributeTable CreateTable()
      {
         // TODO PP: Tidy up this method.
         CustomAttributeTableBuilder builder = new CustomAttributeTableBuilder();
         builder
            .AddTypeAttributes<UndecoratedTypes.IBase1>(CreateTestAttributes<UndecoratedTypes.IBase1>())
               .AddPropertyAttributes<UndecoratedTypes.IBase1>(nameof(UndecoratedTypes.IBase1.ImplementedProperty), CreateTestAttributes<UndecoratedTypes.IBase1>())
               .AddPropertyAttributes<UndecoratedTypes.IBase1>(nameof(UndecoratedTypes.IBase1.HiddenProperty), CreateTestAttributes<UndecoratedTypes.IBase1>())
               .AddMemberAttributes<UndecoratedTypes.IBase1>(c => c.ImplementedMethod1(0, 0), CreateTestAttributes<UndecoratedTypes.IBase1>())
               .AddMemberAttributes<UndecoratedTypes.IBase1>(c => c.HiddenMethod1(0), CreateTestAttributes<UndecoratedTypes.IBase1>())
               .AddMemberAttributes<UndecoratedTypes.IBase1>(c => c.OverloadedMethod(default(int)), CreateTestAttributes(nameof(UndecoratedTypes.IBase1) + "int"))
               .AddMemberAttributes<UndecoratedTypes.IBase1>(c => c.OverloadedMethod(default(long)), CreateTestAttributes(nameof(UndecoratedTypes.IBase1) + "long"))

            .AddTypeAttributes<UndecoratedTypes.IBase2>(CreateTestAttributes<UndecoratedTypes.IBase2>())
               .AddMemberAttributes<UndecoratedTypes.IBase2>(c => c.ImplementedMethod1(0, 0), CreateTestAttributes<UndecoratedTypes.IBase2>())

            .AddTypeAttributes<UndecoratedTypes.IComposite>(CreateTestAttributes<UndecoratedTypes.IComposite>())
               .AddPropertyAttributes<UndecoratedTypes.IComposite>(nameof(UndecoratedTypes.IComposite.HiddenProperty), CreateTestAttributes<UndecoratedTypes.IComposite>())
               .AddMemberAttributes<UndecoratedTypes.IComposite>(c => c.HiddenMethod1(0), CreateTestAttributes<UndecoratedTypes.IComposite>())

            .AddTypeAttributes<UndecoratedTypes.Base>(CreateTestAttributes<UndecoratedTypes.Base>())
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.HiddenProperty, CreateTestAttributes<UndecoratedTypes.Base>())
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.ImplementedProperty, CreateTestAttributes<UndecoratedTypes.Base>())
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.OverriddenProperty, CreateTestAttributes<UndecoratedTypes.Base>())
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.OverriddenProperty2, CreateTestAttributes<UndecoratedTypes.Base>())
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.HiddenMethod1(0), CreateTestAttributes<UndecoratedTypes.Base>())
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.ImplementedMethod1(0, 0), CreateTestAttributes<UndecoratedTypes.Base>())
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.OverriddenMethod(0, 0), CreateTestAttributes<UndecoratedTypes.Base>())
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.OverloadedMethod(default(int)), CreateTestAttributes(nameof(UndecoratedTypes.Base) + "int"))
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.OverloadedMethod(default(long)), CreateTestAttributes(nameof(UndecoratedTypes.Base) + "long"))
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.m_baseField, CreateTestAttributes(nameof(UndecoratedTypes.Base)))
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.m_hiddenBaseField, CreateTestAttributes(nameof(UndecoratedTypes.Base)))
               .AddMemberAttributes<UndecoratedTypes.Base>(c => UndecoratedTypes.Base.s_baseField, CreateTestAttributes(nameof(UndecoratedTypes.Base)))
               .AddMemberAttributes<UndecoratedTypes.Base>(c => UndecoratedTypes.Base.s_hiddenBaseField, CreateTestAttributes(nameof(UndecoratedTypes.Base)))
               .AddEventAttributes<UndecoratedTypes.Base>(nameof(UndecoratedTypes.Base.StaticBaseEvent), CreateTestAttributes(nameof(UndecoratedTypes.Base) + "_S"))
               .AddEventAttributes<UndecoratedTypes.Base>(nameof(UndecoratedTypes.Base.NonVirtualBaseEvent), CreateTestAttributes(nameof(UndecoratedTypes.Base) + "_V"))
               .AddEventAttributes<UndecoratedTypes.Base>(nameof(UndecoratedTypes.Base.HiddenBaseEvent), CreateTestAttributes(nameof(UndecoratedTypes.Base) + "_H"))
               .AddEventAttributes<UndecoratedTypes.Base>(nameof(UndecoratedTypes.Base.OverriddenEvent), CreateTestAttributes(nameof(UndecoratedTypes.Base) + "_O"))
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.GenericMethod(default(long), default(int)), CreateTestAttributes(nameof(UndecoratedTypes.Base) + "void"))
                  .AddParameterAttributes<UndecoratedTypes.Base>(c => c.GenericMethod(Decorate.Parameter<long>(CreateTestAttributes(nameof(UndecoratedTypes.Base) + "long")),
                                                                                       Decorate.Parameter<int>(CreateTestAttributes(nameof(UndecoratedTypes.Base) + "int"))))
               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.GenericMethod<string>(default(int), default(string)), CreateTestAttributes(nameof(UndecoratedTypes.Base) + "T"))
                  .AddParameterAttributes<UndecoratedTypes.Base>(c => c.GenericMethod<string>(Decorate.Parameter<long>(CreateTestAttributes(nameof(UndecoratedTypes.Base) + "long")),
                                                                                             (Decorate.Parameter<string>(CreateTestAttributes(nameof(UndecoratedTypes.Base) + "T")))))
                  .AddReturnParameterAttributes<UndecoratedTypes.Base>(c => c.GenericMethod<string>(default(long), default(string)), CreateTestAttributes(nameof(UndecoratedTypes.Base) + "T").ToArray())

               .AddMemberAttributes<UndecoratedTypes.Base>(c => c.GenericMethod<string, string>(default(string), default(string)), CreateTestAttributes(nameof(UndecoratedTypes.Base) + "TU"))
                  .AddParameterAttributes<UndecoratedTypes.Base>(c => c.GenericMethod<string, string>(Decorate.Parameter<string>(CreateTestAttributes(nameof(UndecoratedTypes.Base) + "U")),
                                                                                                      Decorate.Parameter<string>(CreateTestAttributes(nameof(UndecoratedTypes.Base) + "T"))))
                  .AddReturnParameterAttributes<UndecoratedTypes.Base>(c => c.GenericMethod<string, string>(default(string), default(string)), CreateTestAttributes(nameof(UndecoratedTypes.Base) + "TU").ToArray())

            .AddTypeAttributes<UndecoratedTypes.Derived>(CreateTestAttributes<UndecoratedTypes.Derived>())
               .AddMemberAttributes<UndecoratedTypes.Derived>(der => der.HiddenProperty, CreateTestAttributes<UndecoratedTypes.Derived>())
               .AddMemberAttributes<UndecoratedTypes.Derived>(der => der.OverriddenProperty2, CreateTestAttributes<UndecoratedTypes.Derived>())
               .AddMemberAttributes<UndecoratedTypes.Derived>(c => c.m_derivedField, CreateTestAttributes(nameof(UndecoratedTypes.Derived)))
               .AddMemberAttributes<UndecoratedTypes.Derived>(c => c.m_hiddenBaseField, CreateTestAttributes(nameof(UndecoratedTypes.Derived)))
               .AddMemberAttributes<UndecoratedTypes.Derived>(c => UndecoratedTypes.Derived.s_hiddenBaseField, CreateTestAttributes(nameof(UndecoratedTypes.Derived)))
               .AddEventAttributes<UndecoratedTypes.Derived>(nameof(UndecoratedTypes.Derived.HiddenBaseEvent), CreateTestAttributes(nameof(UndecoratedTypes.Derived)))
            ;

         builder.AddTypeAttributes<UndecoratedTypes.SubDerived>(CreateTestAttributes<UndecoratedTypes.SubDerived>())
            .AddMemberAttributes<UndecoratedTypes.SubDerived>(der => der.OverriddenProperty, CreateTestAttributes<UndecoratedTypes.SubDerived>())
            .AddMemberAttributes<UndecoratedTypes.SubDerived>(der => der.OverriddenProperty2, CreateTestAttributes<UndecoratedTypes.SubDerived>())
            .AddMemberAttributes<UndecoratedTypes.SubDerived>(c => c.OverloadedMethod(default(int)), CreateTestAttributes(nameof(UndecoratedTypes.SubDerived) + "int"))
            .AddMemberAttributes<UndecoratedTypes.SubDerived>(c => c.OverloadedMethod(default(long)), CreateTestAttributes(nameof(UndecoratedTypes.SubDerived) + "long"))
            .AddMemberAttributes<UndecoratedTypes.SubDerived>(c => c.OverriddenMethod(0, 0), CreateTestAttributes(nameof(UndecoratedTypes.SubDerived)))
            .AddEventAttributes<UndecoratedTypes.SubDerived>(nameof(UndecoratedTypes.SubDerived.OverriddenEvent), CreateTestAttributes(nameof(UndecoratedTypes.SubDerived)))
            .AddReturnParameterAttributes<UndecoratedTypes.SubDerived>(c => c.GenericMethod<string>(default(long), default(string)), CreateTestAttributes(nameof(UndecoratedTypes.SubDerived) + "T").ToArray())
            ;

         builder.AddParameterAttributes<UndecoratedTypes.SubDerived>(c => c.GenericMethod(Decorate.Parameter<long>(CreateTestAttributes(nameof(UndecoratedTypes.SubDerived) + "long")), Decorate.Parameter<int>(CreateTestAttributes(nameof(UndecoratedTypes.SubDerived) + "int"))));
         builder.AddParameterAttributes<UndecoratedTypes.SubDerived>(c => c.GenericMethod<object>(default(long), Decorate.Parameter<object>(CreateTestAttributes(nameof(UndecoratedTypes.SubDerived) + "T"))));
         builder.AddParameterAttributes<UndecoratedTypes.SubDerived>(c => c.GenericMethod<string, string>(Decorate.Parameter<string>(CreateTestAttributes(nameof(UndecoratedTypes.SubDerived) + "U")),
                                                                                                         Decorate.Parameter<string>(CreateTestAttributes(nameof(UndecoratedTypes.SubDerived) + "T"))))
         ;

         builder.AddTypeAttributes<UndecoratedTypes.GenericDerived>(CreateTestAttributes<UndecoratedTypes.GenericDerived>())
         .AddTypeAttributes(typeof(UndecoratedTypes.GenericDerived<>), CreateTestAttributes("GenericDerived<TType>"))
         ;
         builder.AddMemberAttributes<UndecoratedTypes.GenericDerived<object>>(cls => cls.GenericMethod(null, null), CreateTestAttributes("GenericDerived<TType>"))
      ;
         return builder.CreateTable();
      }


      private void TestAll(Action<TestInfo> action)
      {
         AttributeTableReflectionContext context = new AttributeTableReflectionContext(CreateTable(), AttributeTableReflectionContextOptions.Default);
         TestAll(action, context);

         context = new AttributeTableReflectionContext(CreateTable(), AttributeTableReflectionContextOptions.HonorPropertyAttributeInheritance | AttributeTableReflectionContextOptions.HonorEventAttributeInheritance);
         TestAll(action, context);
      }

      private void TestAll(Action<TestInfo> action, AttributeTableReflectionContext context)
      {
         action(TestInfo.Create<DecoratedTypes.IBase1, UndecoratedTypes.IBase1>(context));
         action(TestInfo.Create<DecoratedTypes.IBase2, UndecoratedTypes.IBase2>(context));
         action(TestInfo.Create<DecoratedTypes.IComposite, UndecoratedTypes.IComposite>(context));
         action(TestInfo.Create<DecoratedTypes.Base, UndecoratedTypes.Base>(context));
         action(TestInfo.Create<DecoratedTypes.Derived, UndecoratedTypes.Derived>(context));
         action(TestInfo.Create<DecoratedTypes.SubDerived, UndecoratedTypes.SubDerived>(context));
         action(TestInfo.Create(typeof(DecoratedTypes.GenericDerived<>), typeof(UndecoratedTypes.GenericDerived<>), context));
         action(TestInfo.Create(typeof(DecoratedTypes.GenericDerived), typeof(UndecoratedTypes.GenericDerived), context));
      }

      private IEnumerable<T> FilterAttributes<T>(IEnumerable<T> list)
      {
         return list.Where(attr => !(attr is AttributeTableReflectionContextIdentifierAttribute));
      }

      #endregion

      #region Nested Types

      private struct TestInfo
      {
         public TestInfo(Type sourceType, Type targetType, AttributeTableReflectionContext reflectionContext)
         {
            SourceType = sourceType;
            TargetType = targetType;
            ReflectionContext = reflectionContext;
         }

         public static TestInfo Create<SourceType, TargetType>(AttributeTableReflectionContext reflectionContext)
         {
            return new TestInfo(typeof(SourceType), reflectionContext.MapType(typeof(TargetType)), reflectionContext);
         }

         public static TestInfo Create(Type sourceType, Type targetType, AttributeTableReflectionContext reflectionContext)
         {
            return new TestInfo(sourceType, reflectionContext.MapType(targetType), reflectionContext);
         }

         public readonly Type SourceType;
         public readonly Type TargetType;
         public readonly AttributeTableReflectionContext ReflectionContext;
      }

      #endregion

      #region GetCustomAttributes Tests

      [TestMethod]
      public void GetCustomAttributes_Type_InheritIsTrue_ReturnsCorrectAttributes()
      {
         TestAll(info =>
         {
            SequenceAssert.AreEquivalent(info.SourceType.GetCustomAttributes(true), FilterAttributes(info.TargetType.GetCustomAttributes(true)), $"Attribute mismatch on type {info.SourceType.Name} (inherit=true)");
         });
      }

      [TestMethod]
      public void GetCustomAttributes_Type_InheritIsFalse_ReturnsCorrectAttributes()
      {
         TestAll(info =>
         {
            SequenceAssert.AreEquivalent(info.SourceType.GetCustomAttributes(false), FilterAttributes(info.TargetType.GetCustomAttributes(false)), $"Attribute mismatch on type {info.SourceType.Name} (inherit=true)");
         });
      }

      [TestMethod]
      public void GetCustomAttributes_PropertyInfo_ReturnsCorrectAttributes()
      {
         TestAll(info =>
         {
            PropertyInfo[] sourceProperties = info.SourceType.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var sourceProperty in sourceProperties)
            {
               var targetProperty = info.TargetType.GetProperty(sourceProperty.Name, sourceProperty.PropertyType, sourceProperty.GetIndexParameters().Select(p => p.ParameterType).ToArray());

               Assert.IsNotNull(targetProperty, $"Property {sourceProperty.Name} was not found in class {info.TargetType.Name}");

               SequenceAssert.AreEquivalent(sourceProperty.GetCustomAttributes(false), FilterAttributes(targetProperty.GetCustomAttributes(false)), $"Attribute mismatch och property {sourceProperty.DeclaringType.Name}.{sourceProperty.Name} (inherit=false)");

               bool honorInheritance = info.ReflectionContext.Options.HasFlag(AttributeTableReflectionContextOptions.HonorPropertyAttributeInheritance);

               // The default PropertyInfo.GetCustomAttributes ignores the inherit flag, so we need to use the Attribute.GetCustomAttributes method
               // to get inheritance behavior. However, this method can only be used with RuntimeTypes, so we cannot use it for the target type. Instead
               // this is controlled via the reflection context options.
               object[] expectedInherited = honorInheritance ? Attribute.GetCustomAttributes(sourceProperty, true) : sourceProperty.GetCustomAttributes(true);
               SequenceAssert.AreEquivalent(expectedInherited, FilterAttributes(targetProperty.GetCustomAttributes(true)), $"Attribute mismatch och property {sourceProperty.DeclaringType.Name}.{sourceProperty.Name} (inherit=true)");
            }
         });
      }

      [TestMethod]
      public void GetCustomAttributes_FieldInfo_ReturnsCorrectAttributes()
      {
         TestAll(info =>
         {
            FieldInfo[] sourceFields = info.SourceType.GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var sourceField in sourceFields)
            {
               var targetField = info.TargetType.GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                  .SingleOrDefault(field => field.Name == sourceField.Name && field.DeclaringType.Name == sourceField.DeclaringType.Name);

               Assert.IsNotNull(targetField, $"Field {sourceField.Name} was not found in class {info.TargetType.Name}");

               SequenceAssert.AreEquivalent(sourceField.GetCustomAttributes(false), FilterAttributes(targetField.GetCustomAttributes(false)), $"Attribute mismatch och field {sourceField.DeclaringType.Name}.{sourceField.Name} (inherit=false)");

               object[] expectedInherited = sourceField.GetCustomAttributes(true);
               SequenceAssert.AreEquivalent(expectedInherited, FilterAttributes(targetField.GetCustomAttributes(true)), $"Attribute mismatch och field {sourceField.DeclaringType.Name}.{sourceField.Name} (inherit=true)");
            }
         });
      }

      [TestMethod]
      public void GetCustomAttributes_EventInfo_ReturnsCorrectAttributes()
      {
         TestAll(info =>
         {
            EventInfo[] sourceEvents = info.SourceType.GetEvents(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var sourceEvent in sourceEvents)
            {
               var targetProperty = info.TargetType.GetEvents(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                  .SingleOrDefault(ev => ev.Name == sourceEvent.Name && ev.DeclaringType.Name == sourceEvent.DeclaringType.Name);

               Assert.IsNotNull(targetProperty, $"Event {sourceEvent.Name} was not found in class {info.TargetType.Name}");

               SequenceAssert.AreEquivalent(sourceEvent.GetCustomAttributes(false), FilterAttributes(targetProperty.GetCustomAttributes(false)), $"Attribute mismatch on event {sourceEvent.DeclaringType.Name}.{sourceEvent.Name} (inherit=false)");

               bool honorInheritance = info.ReflectionContext.Options.HasFlag(AttributeTableReflectionContextOptions.HonorPropertyAttributeInheritance);

               // The default EventInfo.GetCustomAttributes ignores the inherit flag, so we need to use the Attribute.GetCustomAttributes method
               // to get inheritance behavior. However, this method can only be used with RuntimeTypes, so we cannot use it for the target type. Instead
               // this is controlled via the reflection context options.
               object[] expectedInherited = honorInheritance ? Attribute.GetCustomAttributes(sourceEvent, true) : sourceEvent.GetCustomAttributes(true);
               SequenceAssert.AreEquivalent(expectedInherited, FilterAttributes(targetProperty.GetCustomAttributes(true)), $"Attribute mismatch on event {sourceEvent.DeclaringType.Name}.{sourceEvent.Name} (inherit=true)");
            }
         });
      }

      [TestMethod]
      public void GetCustomAttributes_MethodInfo_ReturnsCorrectAttributes()
      {
         TestAll(info =>
         {
            var sourceMethods = info.SourceType.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                          .Where(m => m.IsSpecialName == false && !m.DeclaringType.Equals(typeof(object)));

            foreach (var sourceMethod in sourceMethods)
            {
               var targetMethod = info.TargetType.GetMethods().SingleOrDefault(method =>
                  method.Name == sourceMethod.Name &&
                  method.GetParameters().SequenceEqual(sourceMethod.GetParameters(), ParameterInfoComparer.Default) &&
                  method.GetGenericArguments().Length == sourceMethod.GetGenericArguments().Length
               );

               //(sourceMethod.Name, sourceMethod.GetParameters().Select(p => p.ParameterType).ToArray());
               Assert.IsNotNull(targetMethod, $"Method {sourceMethod.Name} was not found in class {info.TargetType.Name}");
               bool stop2 = sourceMethod.IsGenericMethod;
               SequenceAssert.AreEquivalent(sourceMethod.GetCustomAttributes(false), FilterAttributes(targetMethod.GetCustomAttributes(false)), $"Attribute mismatch och method {sourceMethod.DeclaringType.Name}.{sourceMethod.Name}({String.Join(",", sourceMethod.GetParameters().Select(p => p.ParameterType.Name))}) (inherit=false)");
               SequenceAssert.AreEquivalent(sourceMethod.GetCustomAttributes(true), FilterAttributes(targetMethod.GetCustomAttributes(true)), $"Attribute mismatch och method {sourceMethod.DeclaringType.Name}.{sourceMethod.Name}({String.Join(",", sourceMethod.GetParameters().Select(p => p.ParameterType.Name))}) (inherit=true)");
            }
         });
      }

      [TestMethod]
      public void GetCustomAttributes_ParameterInfo_ReturnsCorrectAttributes()
      {
         TestAll(info =>
         {
            var sourceMethods = info.SourceType.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                          .Where(m => m.IsSpecialName == false && !m.DeclaringType.Equals(typeof(object)));

            foreach (var sourceMethod in sourceMethods)
            {
               var targetMethod = info.TargetType.GetMethods().SingleOrDefault(method =>
                  method.Name == sourceMethod.Name &&
                  method.GetParameters().SequenceEqual(sourceMethod.GetParameters(), ParameterInfoComparer.Default) &&
                  method.GetGenericArguments().Length == sourceMethod.GetGenericArguments().Length
               );

               Assert.IsNotNull(targetMethod, $"Method {sourceMethod.Name} was not found in class {info.TargetType.Name}");

               foreach (var sourceParameter in sourceMethod.GetParameters())
               {
                  var targetParameter = targetMethod.GetParameters()[sourceParameter.Position];

                  SequenceAssert.AreEquivalent(sourceParameter.GetCustomAttributes(false), FilterAttributes(targetParameter.GetCustomAttributes(false)), $"Attribute mismatch on parameter {sourceParameter.Name} of method {sourceMethod.DeclaringType.Name}.{sourceMethod.Name}({String.Join(",", sourceMethod.GetParameters().Select(p => p.ParameterType.Name))}) (inherit=false)");
                  SequenceAssert.AreEquivalent(sourceParameter.GetCustomAttributes(true), FilterAttributes(targetParameter.GetCustomAttributes(true)), $"Attribute mismatch on parameter {sourceParameter.Name} of method {sourceMethod.DeclaringType.Name}.{sourceMethod.Name}({String.Join(",", sourceMethod.GetParameters().Select(p => p.ParameterType.Name))}) (inherit=true)");
               }
            }
         });
      }

      [TestMethod]
      public void GetCustomAttributes_ReturnParameterInfo_ReturnsCorrectAttributes()
      {
         TestAll(info =>
         {
            var sourceMethods = info.SourceType.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                          .Where(m => m.IsSpecialName == false && !m.DeclaringType.Equals(typeof(object)));

            foreach (var sourceMethod in sourceMethods)
            {
               var targetMethod = info.TargetType.GetMethods().SingleOrDefault(method =>
                  method.Name == sourceMethod.Name &&
                  method.GetParameters().SequenceEqual(sourceMethod.GetParameters(), ParameterInfoComparer.Default) &&
                  method.GetGenericArguments().Length == sourceMethod.GetGenericArguments().Length
               );

               Assert.IsNotNull(targetMethod, $"Method {sourceMethod.Name} was not found in class {info.TargetType.Name}");

               
               var sourceParameter = sourceMethod.ReturnParameter;
               var targetParameter = targetMethod.ReturnParameter;

               if (!sourceParameter.GetCustomAttributes(false).SequenceEqual(FilterAttributes(targetParameter.GetCustomAttributes(false))))
                  Debugger.Break();
               SequenceAssert.AreEquivalent(sourceParameter.GetCustomAttributes(false), FilterAttributes(targetParameter.GetCustomAttributes(false)), $"Attribute mismatch on return parameter of method {sourceMethod.DeclaringType.Name}.{sourceMethod.Name}({String.Join(",", sourceMethod.GetParameters().Select(p => p.ParameterType.Name))}) (inherit=false)");
               SequenceAssert.AreEquivalent(sourceParameter.GetCustomAttributes(true), FilterAttributes(targetParameter.GetCustomAttributes(true)), $"Attribute mismatch on return parameter of method {sourceMethod.DeclaringType.Name}.{sourceMethod.Name}({String.Join(",", sourceMethod.GetParameters().Select(p => p.ParameterType.Name))}) (inherit=true)");
            }
         });
      }

      private class ParameterInfoComparer : IEqualityComparer<ParameterInfo>
      {
         public static readonly ParameterInfoComparer Default = new ParameterInfoComparer();

         public bool Equals(ParameterInfo x, ParameterInfo y)
         {
            if (x == null)
               return y == null;

            if (y == null)
               return false;

            if (x.Position != y.Position)
               return false;

            Type xType = x.ParameterType;
            Type yType = y.ParameterType;

            if (xType.IsGenericParameter)
            {
               if (!yType.IsGenericParameter)
                  return false;

               return xType.GenericParameterPosition == yType.GenericParameterPosition;
            }
            else
            {
               return xType.Equals(yType) && x.Position == y.Position;
            }
         }

         public int GetHashCode(ParameterInfo obj)
         {
            throw new NotImplementedException();
         }
      }
      #endregion
   }
}
